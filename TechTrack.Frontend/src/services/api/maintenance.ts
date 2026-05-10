import { apiClient } from './client';
import { Maintenance } from '@/types';

export const maintenanceService = {
  getMaintenances: async (equipmentId: string): Promise<Maintenance[]> => {
    return apiClient.get(`/equipments/${equipmentId}/maintenance`);
  },

  createMaintenance: async (equipmentId: string, data: {
    name: string;
    description?: string;
    solvedIssuesId?: string[];
  }): Promise<void> => {
    return apiClient.post(`/equipments/${equipmentId}/maintenance`, data);
  },

  completeMaintenance: async (maintenanceId: string, data: {
    description?: string;
    maintenanceTypeId?: number;
  }): Promise<void> => {
    return apiClient.post(`/maintenances/${maintenanceId}/complete`, data);
  },

  scheduleMaintenance: async (equipmentId: string, data: {
    recurrenceTypeId: number;
    maintenanceName: string;
    intervalValue: number;
    nextMaintenanceDate: string;
    responsibleUserId: string;
    notificationAdvanceDays: number;
  }): Promise<{ scheduleRecordId: string; maintenanceRecordId: string }> => {
    return apiClient.post(`/equipments/${equipmentId}/schedule`, data);
  },
};
