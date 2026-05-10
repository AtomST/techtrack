import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { User } from '@/types';

interface AuthState {
  user: User | null;
  accessToken: string | null;
  refreshToken: string | null;
  setAuth: (user: User | null, accessToken: string, refreshToken: string) => void;
  clearAuth: () => void;
  updateUser: (user: User) => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      user: null,
      accessToken: null,
      refreshToken: null,
      setAuth: (user, accessToken, refreshToken) => {
        // Сохраняем только в Zustand store, не дублируем в localStorage
        set({ user, accessToken, refreshToken });
      },
      clearAuth: () => {
        set({ user: null, accessToken: null, refreshToken: null });
      },
      updateUser: (user) => set({ user }),
    }),
    {
      name: 'auth-storage', // Это единственное место хранения
    }
  )
);
