import axios, { AxiosInstance } from 'axios';
import { useAuthStore } from '@/store/authStore';

class ApiClient {
  private client: AxiosInstance;
  private isRefreshing = false;
  private failedQueue: Array<{
    resolve: (value?: unknown) => void;
    reject: (reason?: unknown) => void;
  }> = [];

  constructor() {
    this.client = axios.create({
      baseURL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:8080/api',
      timeout: 30000,
      headers: {
        'Content-Type': 'application/json',
      },
      withCredentials: true, // Важно для отправки refreshToken cookie
    });

    // Интерсептор запроса - добавляем токен
    this.client.interceptors.request.use(
      (config) => {
        const { accessToken } = useAuthStore.getState();
        console.log('🔵 Request interceptor - token exists:', !!accessToken);
        if (accessToken) {
          config.headers.Authorization = `Bearer ${accessToken}`;
        }
        console.log('🚀 REQUEST:', config.method?.toUpperCase(), config.url);
        if (config.data) {
          console.log('📦 DATA:', config.data);
        }
        return config;
      },
      (error) => {
        console.error('❌ REQUEST ERROR:', error);
        return Promise.reject(error);
      }
    );

    // Интерсептор ответа - обрабатываем 401 и обновляем токен
    this.client.interceptors.response.use(
      (response) => {
        if (response.data && response.data.statusCode) {
          if (response.data.statusCode >= 200 && response.data.statusCode < 300) {
            return response.data.data !== undefined ? response.data.data : response.data;
          }
          return Promise.reject(response.data);
        }
        return response.data;
      },
      async (error) => {
        const originalRequest = error.config;
        
        console.log('❌ RESPONSE ERROR:', error.response?.status, error.response?.config?.url);
        
        // Если 401 и не повторный запрос
        if (error.response?.status === 401 && !originalRequest._retry) {
          if (this.isRefreshing) {
            // Если уже обновляем токен, добавляем в очередь
            return new Promise((resolve, reject) => {
              this.failedQueue.push({ resolve, reject });
            }).then(() => {
              return this.client(originalRequest);
            }).catch((err) => {
              return Promise.reject(err);
            });
          }

          originalRequest._retry = true;
          this.isRefreshing = true;

          try {
            // Запрашиваем новый accessToken через refresh endpoint
            // refreshToken отправится автоматически в cookie
            console.log('🔄 Refreshing token...');
            const response = await axios.post(
              `${process.env.NEXT_PUBLIC_API_URL}/auth/refresh`,
              {},
              { withCredentials: true }
            );
            
            console.log('🔄 Refresh response:', response.data);
            
            if (response.data && response.data.data && response.data.data.accessToken) {
              const newToken = response.data.data.accessToken;
              
              // Обновляем токен в store
              const { accessToken, refreshToken, user } = useAuthStore.getState();
              useAuthStore.getState().setAuth(user, newToken, refreshToken || '');
              
              // Обрабатываем очередь запросов
              this.failedQueue.forEach((promise) => {
                promise.resolve();
              });
              this.failedQueue = [];
              
              // Повторяем оригинальный запрос с новым токеном
              originalRequest.headers.Authorization = `Bearer ${newToken}`;
              return this.client(originalRequest);
            } else {
              throw new Error('No accessToken in refresh response');
            }
          } catch (refreshError) {
            console.error('🔄 Refresh failed:', refreshError);
            this.failedQueue.forEach((promise) => {
              promise.reject(refreshError);
            });
            this.failedQueue = [];
            
            // Очищаем store и редиректим на логин
            useAuthStore.getState().clearAuth();
            if (typeof window !== 'undefined') {
              window.location.href = '/login';
            }
            return Promise.reject(refreshError);
          } finally {
            this.isRefreshing = false;
          }
        }
        
        // Форматируем ошибку для единообразного ответа
        const errorData = error.response?.data;
        if (errorData && (errorData.message || errorData.Message)) {
          return Promise.reject({
            statusCode: errorData.statusCode || error.response?.status,
            message: errorData.message || errorData.Message,
            errors: errorData.errors,
          });
        }
        
        return Promise.reject({
          statusCode: error.response?.status || 500,
          message: error.message || 'Произошла ошибка',
        });
      }
    );
  }

  async get<T>(url: string): Promise<T> {
    return this.client.get(url) as Promise<T>;
  }

  async post<T>(url: string, data?: any): Promise<T> {
    console.log('📤 POST called with URL:', url);
    return this.client.post(url, data) as Promise<T>;
  }

  async put<T>(url: string, data?: any): Promise<T> {
    return this.client.put(url, data) as Promise<T>;
  }

  async delete<T>(url: string): Promise<T> {
    return this.client.delete(url) as Promise<T>;
  }
}

export const apiClient = new ApiClient();
