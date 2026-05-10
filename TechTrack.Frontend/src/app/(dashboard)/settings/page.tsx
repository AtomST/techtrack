'use client';

import { useState } from 'react';
import { useAuth } from '@/hooks/useAuth';
import { Button } from '@/components/ui/Button';
import { Settings, LogOut, AlertTriangle } from 'lucide-react';

export default function SettingsPage() {
  const { user, logout } = useAuth();
  const [isLoggingOut, setIsLoggingOut] = useState(false);

  const handleLogout = async () => {
    setIsLoggingOut(true);
    await logout();
    setIsLoggingOut(false);
  };

  return (
    <div className="p-8">
      <div className="max-w-2xl mx-auto">
        <div className="flex items-center space-x-3 mb-6">
          <Settings className="w-8 h-8 text-gray-700" />
          <h1 className="text-2xl font-bold text-gray-900">Настройки</h1>
        </div>

        <div className="bg-white rounded-lg shadow-sm divide-y divide-gray-200">
          <div className="p-6">
            <h2 className="text-lg font-semibold text-gray-900 mb-4">Профиль</h2>
            <div className="space-y-3">
              <div>
                <label className="text-sm text-gray-500">Имя</label>
                <p className="text-gray-900">{user?.fullName || 'Не указано'}</p>
              </div>
              <div>
                <label className="text-sm text-gray-500">Email</label>
                <p className="text-gray-900">{user?.email}</p>
              </div>
              <div>
                <label className="text-sm text-gray-500">ID пользователя</label>
                <p className="text-gray-900 text-sm">{user?.id || 'Не указан'}</p>
              </div>
            </div>
          </div>

          <div className="p-6">
            <h2 className="text-lg font-semibold text-gray-900 mb-4">Безопасность</h2>
            <div className="space-y-4">
              <Button variant="outline" onClick={handleLogout} loading={isLoggingOut}>
                <LogOut className="w-4 h-4 mr-2" />
                Выйти из аккаунта
              </Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
