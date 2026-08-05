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

// Функция для извлечения данных из токена
const parseJwt = (token: string): any => {
  try {
    const parts = token.split('.');
    if (parts.length === 3) {
      return JSON.parse(atob(parts[1]));
    }
  } catch (e) {
    console.error('Failed to parse JWT:', e);
  }
  return null;
};

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      user: null,
      accessToken: null,
      refreshToken: null,
      setAuth: (user, accessToken, refreshToken) => {
        // Извлекаем роль и companyId из токена
        let userWithRole = user;
        if (accessToken && !userWithRole?.role) {
          const payload = parseJwt(accessToken);
          if (payload) {
            userWithRole = {
              id: payload.nameid || user?.id || '',
              email: user?.email || '',
              fullName: user?.fullName || '',
              role: payload.role,
              companyId: payload.company_id,
            };
            console.log('Extracted from token - role:', payload.role, 'companyId:', payload.company_id);
          }
        }
        set({ user: userWithRole, accessToken, refreshToken });
      },
      clearAuth: () => {
        set({ user: null, accessToken: null, refreshToken: null });
      },
      updateUser: (user) => set({ user }),
    }),
    {
      name: 'auth-storage',
    }
  )
);
