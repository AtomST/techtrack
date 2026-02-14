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
    try {
      // Проверяем наличие accessToken в sessionStorage
      const user = authService.getCurrentUser();
      const isAuthenticated = authService.isAuthenticated();

      if (isAuthenticated && user) {
        // AccessToken еще валиден
        set({ user, isAuthenticated: true, isLoading: false });
        return;
      }

      // AccessToken истек или отсутствует, пробуем обновить через refresh token
      if (typeof window !== 'undefined') {
        try {
          const newAccessToken = await authService.refresh();
          
          // Декодируем новый токен для получения пользователя
          const refreshedUser = authService.getCurrentUser();
          
          if (refreshedUser) {
            set({ user: refreshedUser, isAuthenticated: true, isLoading: false });
            return;
          }
        } catch (refreshError) {
          // Refresh token тоже истек или невалиден
          console.log('Refresh token expired or invalid');
        }
      }

      // Нет валидных токенов
      set({ user: null, isAuthenticated: false, isLoading: false });
    } catch (error) {
      console.error('Auth initialization error:', error);
      set({ user: null, isAuthenticated: false, isLoading: false });
    }
  },
}));
