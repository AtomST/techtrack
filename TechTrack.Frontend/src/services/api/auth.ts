import { apiClient } from './client';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  phoneNumber?: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  user: {
    id: string;
    email: string;
    fullName: string;
  };
}

export interface RegisterResponse {
  accessToken: string;
}

export const authService = {
  login: async (data: LoginRequest): Promise<LoginResponse> => {
    const response = await apiClient.post<LoginResponse>('/auth/login', data);
    return response;
  },

  register: async (data: RegisterRequest): Promise<RegisterResponse> => {
    const response = await apiClient.post<RegisterResponse>('/auth/register', data);
    return response;
  },

  refresh: async (refreshToken: string): Promise<{ accessToken: string }> => {
    return apiClient.post('/auth/refresh', { refreshToken });
  },

  logout: async (): Promise<void> => {
    return apiClient.post('/auth/logout');
  },

  logoutAll: async (): Promise<void> => {
    return apiClient.post('/auth/logout-all');
  },
};
