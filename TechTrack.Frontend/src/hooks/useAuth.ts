'use client';

import { useRouter } from 'next/navigation';
import { useAuthStore } from '@/store/authStore';
import { authService } from '@/services/api/auth';
import toast from 'react-hot-toast';

export const useAuth = () => {
  const router = useRouter();
  const { user, accessToken, setAuth, clearAuth } = useAuthStore();

  const login = async (email: string, password: string) => {
    try {
      const response = await authService.login({ email, password });
      console.log('Login response:', response);
      
      if (response && response.accessToken) {
        // Передаем токен, user может быть null - роль извлечется из токена
        setAuth(response.user || null, response.accessToken, response.refreshToken || '');
        toast.success('Вход выполнен успешно');
        
        // Небольшая задержка перед редиректом
        setTimeout(() => {
          router.push('/dashboard');
        }, 100);
        return true;
      }
      return false;
    } catch (error: any) {
      console.error('Login error:', error);
      toast.error(error?.message || 'Ошибка входа');
      return false;
    }
  };

  const logout = async () => {
    try {
      await authService.logout();
    } catch (error) {
      console.error('Logout error:', error);
    } finally {
      clearAuth();
      router.push('/login');
    }
  };

  return {
    user,
    isAuthenticated: !!accessToken,
    login,
    logout,
  };
};
