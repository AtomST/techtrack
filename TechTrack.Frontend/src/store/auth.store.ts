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

      console.log('[Auth Store] No valid access token, checking for refresh token...');
      
      // НЕ вызываем refresh автоматически!
      // Пусть первый 401 запрос вызовет refresh через interceptor
      // Это избежит лишних запросов при отсутствии refresh cookie
      
      set({ user: null, isAuthenticated: false, isLoading: false });
    } catch (error) {
      console.error('[Auth Store] Initialization error:', error);
      set({ user: null, isAuthenticated: false, isLoading: false });
    }
  },
}));
