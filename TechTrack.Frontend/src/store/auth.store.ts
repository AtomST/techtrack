import { create } from 'zustand';
import { User } from '@/types';
import { authService } from '@/services/auth.service';

interface AuthState {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  setUser: (user: User | null) => void;
  logout: () => Promise<void>;
  initialize: () => Promise<void>;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  isAuthenticated: false,
  isLoading: true,

  setUser: (user) => set({ user, isAuthenticated: !!user }),

  logout: async () => {
    await authService.logout();
    set({ user: null, isAuthenticated: false });
  },

  initialize: async () => {
    console.log('[Auth Store] Initializing...');
    
    try {
      // Проверяем наличие accessToken в sessionStorage
      const user = authService.getCurrentUser();
      const isAuthenticated = authService.isAuthenticated();

      if (isAuthenticated && user) {
        // AccessToken еще валиден
        console.log('[Auth Store] Valid access token found');
        set({ user, isAuthenticated: true, isLoading: false });
        return;
      }

      console.log('[Auth Store] No valid access token, attempting refresh...');
      
      // Пытаемся обновить через refresh token
      if (typeof window !== 'undefined') {
        try {
          const newAccessToken = await authService.refresh();
          
          // Декодируем новый токен для получения пользователя
          const refreshedUser = authService.getCurrentUser();
          
          if (refreshedUser) {
            console.log('[Auth Store] Session restored successfully via refresh');
            set({ user: refreshedUser, isAuthenticated: true, isLoading: false });
            return;
          }
        } catch (refreshError: any) {
          // Refresh token тоже истек или невалиден
          console.log('[Auth Store] Refresh failed:', refreshError.message);
          
          // Clear any remaining session data
          if (typeof window !== 'undefined') {
            sessionStorage.removeItem('accessToken');
            sessionStorage.removeItem('user');
            sessionStorage.removeItem('userEmail');
            sessionStorage.removeItem('userFullName');
          }
        }
      }

      // Нет валидных токенов
      console.log('[Auth Store] No valid tokens, user not authenticated');
      set({ user: null, isAuthenticated: false, isLoading: false });
    } catch (error) {
      console.error('[Auth Store] Initialization error:', error);
      
      // Clear session on any error
      if (typeof window !== 'undefined') {
        sessionStorage.removeItem('accessToken');
        sessionStorage.removeItem('user');
        sessionStorage.removeItem('userEmail');
        sessionStorage.removeItem('userFullName');
      }
      
      set({ user: null, isAuthenticated: false, isLoading: false });
    }
  },
}));
