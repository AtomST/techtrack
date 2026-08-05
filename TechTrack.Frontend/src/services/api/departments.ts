import { apiClient } from './client';
import { Department, Equipment } from '@/types';

export const departmentService = {
  createDepartment: async (data: { name: string; responsibleUserId?: string }): Promise<{ departmentId: string }> => {
    return apiClient.post('/departments', data);
  },

  getAllDepartments: async (): Promise<Department[]> => {
    return apiClient.get('/departments');
  },

  getMyDepartments: async (): Promise<Department[]> => {
    return apiClient.get('/departments/my');
  },

  getDepartment: async (id: string): Promise<Department & { equipments: Equipment[] }> => {
    return apiClient.get(`/departments/${id}`);
  },

  addEmployee: async (departmentId: string, userId: string): Promise<void> => {
    return apiClient.post(`/departments/${departmentId}/employees`, { id: userId });
  },

  getEmployees: async (departmentId: string): Promise<any[]> => {
    return apiClient.get(`/departments/${departmentId}/employees`);
  },
};
