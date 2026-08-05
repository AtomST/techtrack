import { apiClient } from './client';
import { Notification } from '@/types';

export const notificationService = {
  getMyNotifications: async (): Promise<Notification[]> => {
    return apiClient.get('/notifications/my');
  },
};
