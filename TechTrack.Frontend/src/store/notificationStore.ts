import { create } from 'zustand';
import { Notification } from '@/types';
import { notificationService } from '@/services/api/notifications';

interface NotificationState {
  notifications: Notification[];
  loading: boolean;
  fetchNotifications: () => Promise<void>;
}

export const useNotificationStore = create<NotificationState>((set) => ({
  notifications: [],
  loading: false,

  fetchNotifications: async () => {
    set({ loading: true });
    try {
      const notifications = await notificationService.getMyNotifications();
      set({ notifications });
    } catch (error) {
      console.error('Failed to fetch notifications:', error);
      set({ notifications: [] });
    } finally {
      set({ loading: false });
    }
  },
}));
