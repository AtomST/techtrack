'use client';

import { useAuthStore } from '@/store/auth.store';

export function useAuth() {
  const { user, isAuthenticated, isLoading, logout } = useAuthStore();

  return {
    user,
    isAuthenticated,
    isLoading,
    logout,
    // Helper flags
    isReady: !isLoading,
    hasCompany: !!user?.companyId,
  };
}
