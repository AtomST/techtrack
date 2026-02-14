'use client';

import { useEffect, ComponentType } from 'react';
import { useRouter } from 'next/navigation';
import { useAuthStore } from '@/store/auth.store';

interface WithAuthOptions {
  redirectTo?: string;
  requireCompany?: boolean;
}

export function withAuth<P extends object>(
  Component: ComponentType<P>,
  options: WithAuthOptions = {}
) {
  const { redirectTo = '/login', requireCompany = false } = options;

  return function ProtectedRoute(props: P) {
    const router = useRouter();
    const { user, isAuthenticated, isLoading } = useAuthStore();

    useEffect(() => {
      if (!isLoading) {
        if (!isAuthenticated) {
          // Not authenticated, redirect to login
          router.push(redirectTo);
        } else if (requireCompany && !user?.companyId) {
          // Authenticated but no company
          router.push('/no-company');
        }
      }
    }, [isAuthenticated, isLoading, user, router]);

    // Show loading while checking auth
    if (isLoading) {
      return (
        <div className="min-h-screen flex items-center justify-center">
          <div className="text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary mx-auto"></div>
            <p className="mt-4 text-muted-foreground">Загрузка...</p>
          </div>
        </div>
      );
    }

    // Don't render if not authenticated or missing company
    if (!isAuthenticated || (requireCompany && !user?.companyId)) {
      return null;
    }

    // Render the protected component
    return <Component {...props} />;
  };
}
