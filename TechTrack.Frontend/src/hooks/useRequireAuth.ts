'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from './useAuth';

interface UseRequireAuthOptions {
  redirectTo?: string;
  requireCompany?: boolean;
}

/**
 * Hook to protect pages - redirects if not authenticated
 * Returns true if ready to render, false if still loading/redirecting
 */
export function useRequireAuth(options: UseRequireAuthOptions = {}) {
  const { redirectTo = '/login', requireCompany = false } = options;
  const router = useRouter();
  const { isAuthenticated, isLoading, hasCompany } = useAuth();

  useEffect(() => {
    if (!isLoading) {
      if (!isAuthenticated) {
        router.push(redirectTo);
      } else if (requireCompany && !hasCompany) {
        router.push('/no-company');
      }
    }
  }, [isAuthenticated, isLoading, hasCompany, router, redirectTo, requireCompany]);

  // Return true if ready to render content
  return {
    isReady: !isLoading && isAuthenticated && (!requireCompany || hasCompany),
    isLoading,
  };
}
