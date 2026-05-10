'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { useAuthStore } from '@/store/authStore';

export function Header() {
  const { user, clearAuth } = useAuthStore();
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  const handleLogout = () => {
    clearAuth();
    window.location.href = '/login';
  };

  if (!mounted) {
    return (
      <header className="bg-white border-b border-gray-200 px-6 py-3">
        <div className="text-xl font-bold text-blue-600">TechTrack</div>
      </header>
    );
  }

  return (
    <header className="bg-white border-b border-gray-200 px-6 py-3">
      <div className="flex justify-between items-center">
        <Link href="/dashboard" className="text-xl font-bold text-blue-600">
          TechTrack
        </Link>
        <div className="flex items-center space-x-4">
          <span>{user?.fullName || user?.email}</span>
          <button onClick={handleLogout} className="text-red-600 hover:text-red-700">
            Выйти
          </button>
        </div>
      </div>
    </header>
  );
}
