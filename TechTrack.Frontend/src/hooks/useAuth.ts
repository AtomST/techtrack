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
      
      if (response && response.accessToken) {
        setAuth(response.user || null, response.accessToken, response.refreshToken || '');
        toast.success('Вход выполнен успешно');
        router.push('/dashboard');
        return true;
      }
      return false;
    } catch (error: any) {
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
