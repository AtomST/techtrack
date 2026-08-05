import { apiClient } from './client';
import { Equipment, Issue, Maintenance } from '@/types';

export const equipmentService = {
  createEquipment: async (departmentId: string, data: { 
    name: string; 
    serialNumber?: string; 
    description?: string; 
    responsibleUserId?: string 
  }): Promise<Equipment> => {
    return apiClient.post(`/departments/${departmentId}/equipments`, data);
  },

  getEquipments: async (departmentId: string): Promise<Equipment[]> => {
    return apiClient.get(`/departments/${departmentId}/equipments`);
  },

  createIssue: async (equipmentId: string, data: { 
    name: string; 
    description?: string; 
    statusId: number 
  }): Promise<{ issueId: string }> => {
    return apiClient.post(`/equipments/${equipmentId}/issues`, data);
  },

  getIssues: async (equipmentId: string): Promise<Issue[]> => {
    return apiClient.get(`/equipments/${equipmentId}/issues`);
  },

  createMaintenance: async (equipmentId: string, data: { 
    name: string; 
    description?: string; 
    solvedIssuesId?: string[] 
  }): Promise<void> => {
    return apiClient.post(`/equipments/${equipmentId}/maintenance`, data);
  },

  getMaintenances: async (equipmentId: string): Promise<Maintenance[]> => {
    return apiClient.get(`/equipments/${equipmentId}/maintenance`);
  },

  completeMaintenance: async (maintenanceId: string, data: { 
    description?: string; 
    maintenanceTypeId?: number 
  }): Promise<void> => {
    return apiClient.post(`/maintenances/${maintenanceId}/complete`, data);
  },

  scheduleMaintenance: async (equipmentId: string, data: any): Promise<{ scheduleRecordId: string; maintenanceRecordId: string }> => {
    return apiClient.post(`/equipments/${equipmentId}/schedule`, data);
  },
};
