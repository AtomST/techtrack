import { apiClient } from './client';
import { Company } from '@/types';

export const companyService = {
  createCompany: async (data: { name: string }): Promise<Company> => {
    return apiClient.post('/companies', data);
  },

  getCompanies: async (): Promise<Company[]> => {
    return apiClient.get('/companies');
  },

  getCompany: async (id: string): Promise<Company> => {
    return apiClient.get(`/companies/${id}`);
  },

  addEmployee: async (companyId: string, email: string): Promise<void> => {
    return apiClient.post(`/companies/${companyId}/employees`, { email });
  },

  getEmployees: async (companyId: string): Promise<any[]> => {
    return apiClient.get(`/companies/${companyId}/employees`);
  },
};
