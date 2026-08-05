'use client';

import { useEffect } from 'react';
import { useNotifications } from '@/hooks/useNotifications';
import { Spinner } from '@/components/ui/Spinner';
import { Bell, CheckCircle, AlertTriangle, Info, Calendar, AlertCircle } from 'lucide-react';
import { formatDateTime } from '@/utils/dateUtils';
import { NOTIFICATION_TYPES } from '@/utils/constants';
import { Button } from '@/components/ui/Button';

export default function NotificationsPage() {
  const { notifications, loading, refreshNotifications } = useNotifications();

  useEffect(() => {
    refreshNotifications();
  }, []);

  const getNotificationIcon = (typeId: number) => {
    switch (typeId) {
      case 1:
        return <CheckCircle className="w-5 h-5 text-green-500" />;
      case 2:
        return <AlertTriangle className="w-5 h-5 text-red-500" />;
      case 3:
        return <Calendar className="w-5 h-5 text-blue-500" />;
      case 4:
        return <AlertCircle className="w-5 h-5 text-orange-500" />;
      default:
        return <Info className="w-5 h-5 text-gray-500" />;
    }
  };

  const getNotificationColor = (typeId: number) => {
    switch (typeId) {
      case 1:
        return 'bg-green-50 border-green-200';
      case 2:
        return 'bg-red-50 border-red-200';
      case 3:
        return 'bg-blue-50 border-blue-200';
      case 4:
        return 'bg-orange-50 border-orange-200';
      default:
        return 'bg-gray-50 border-gray-200';
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-full">
        <Spinner size="lg" />
      </div>
    );
  }

  return (
    <div className="p-8">
      <div className="max-w-4xl mx-auto">
        <div className="flex justify-between items-center mb-6">
          <div className="flex items-center space-x-3">
            <Bell className="w-8 h-8 text-gray-700" />
            <h1 className="text-2xl font-bold text-gray-900">Уведомления</h1>
          </div>
          <Button variant="outline" onClick={refreshNotifications}>
            Обновить
          </Button>
        </div>

        {notifications.length === 0 ? (
          <div className="bg-white rounded-lg shadow-sm p-12 text-center">
            <Bell className="w-16 h-16 text-gray-300 mx-auto mb-4" />
            <h3 className="text-lg font-medium text-gray-900">Нет уведомлений</h3>
            <p className="text-gray-500 mt-1">У вас пока нет уведомлений</p>
          </div>
        ) : (
          <div className="space-y-3">
            {notifications.map((notification) => (
              <div
                key={notification.id}
                className={`rounded-lg border p-4 ${getNotificationColor(notification.notificationTypeId)} hover:shadow-md transition-shadow`}
              >
                <div className="flex items-start space-x-3">
                  {getNotificationIcon(notification.notificationTypeId)}
                  <div className="flex-1">
                    <div className="flex items-start justify-between">
                      <h3 className="font-semibold text-gray-900">
                        {notification.title}
                      </h3>
                      <span className="text-xs text-gray-500">
                        {formatDateTime(notification.createdAt)}
                      </span>
                    </div>
                    <p className="text-sm text-gray-700 mt-1 whitespace-pre-line">
                      {notification.message}
                    </p>
                    <div className="mt-2">
                      <span className="inline-flex items-center px-2 py-1 rounded-full text-xs font-medium bg-white bg-opacity-50 text-gray-600">
                        {NOTIFICATION_TYPES[notification.notificationTypeId as keyof typeof NOTIFICATION_TYPES]?.label || 'Уведомление'}
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
