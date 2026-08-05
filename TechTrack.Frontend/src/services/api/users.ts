import { apiClient } from './client';
import { User } from '@/types';

export const usersService = {
  assignRole: async (userId: string, role: string): Promise<void> => {
    return apiClient.post(`/users/${userId}/role`, { role });
  },

  getCurrentUser: async (): Promise<User> => {
    return apiClient.get('/users/me');
  },

  getCompanyUsers: async (companyId: string): Promise<any[]> => {
    return apiClient.get(`/companies/${companyId}/employees`);
  },

  getDepartmentUsers: async (departmentId: string): Promise<any[]> => {
    return apiClient.get(`/departments/${departmentId}/employees`);
  },
};
