'use client';

import { useEffect, ReactNode } from 'react';
import { useAuthStore } from '@/store/authStore';

export function AuthProvider({ children }: { children: ReactNode }) {
  const { setAuth, user, accessToken } = useAuthStore();

  useEffect(() => {
    // Восстанавливаем сессию из localStorage при загрузке клиента
    const token = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');
    const savedUser = localStorage.getItem('auth-storage');
    
    if (token && refreshToken && savedUser && !user) {
      try {
        const parsed = JSON.parse(savedUser);
        if (parsed.state?.user) {
          setAuth(parsed.state.user, token, refreshToken);
        }
      } catch (error) {
        console.error('Failed to restore auth:', error);
      }
    }
  }, []);

  return <>{children}</>;
}
