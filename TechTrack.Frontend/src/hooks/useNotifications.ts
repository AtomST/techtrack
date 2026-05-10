'use client';

import { useEffect, useState } from 'react';
import { useNotificationStore } from '@/store/notificationStore';
import { notificationService } from '@/services/api/notifications';
import { Notification } from '@/types';

export const useNotifications = () => {
  const { notifications, fetchNotifications, loading } = useNotificationStore();
  const [pollingInterval, setPollingInterval] = useState<NodeJS.Timeout | null>(null);

  // Функция для принудительного обновления уведомлений
  const refreshNotifications = async () => {
    await fetchNotifications();
  };

  // Запускаем периодический опрос уведомлений (каждые 30 секунд)
  useEffect(() => {
    refreshNotifications();
    
    const interval = setInterval(refreshNotifications, 30000);
    setPollingInterval(interval);

    return () => {
      if (pollingInterval) {
        clearInterval(pollingInterval);
      }
    };
  }, []);

  // Функция для ручного обновления после действий (создание ТО, завершение и т.д.)
  const updateAfterAction = async () => {
    await refreshNotifications();
  };

  return {
    notifications,
    loading,
    refreshNotifications: refreshNotifications,
    updateAfterAction,
  };
};
