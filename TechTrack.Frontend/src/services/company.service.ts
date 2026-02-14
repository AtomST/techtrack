import { apiClient } from '@/lib/api-client';
import { Company, CreateCompanyRequest, ApiResponse } from '@/types';

export const companyService = {
  async getCompanies(): Promise<Company[]> {
    const response = await apiClient.get<ApiResponse<Company[]>>('/api/companies');
    
    return response.data.data || [];
  },

  async getCompany(id: string): Promise<Company> {
    const response = await apiClient.get<ApiResponse<Company>>(`/api/companies/${id}`);
    
    return response.data.data;
  },

  async createCompany(data: CreateCompanyRequest): Promise<Company> {
    const response = await apiClient.post<ApiResponse<Company>>('/api/companies', data);
    
    return response.data.data;
  },

  async updateCompany(id: string, data: Partial<CreateCompanyRequest>): Promise<Company> {
    const response = await apiClient.put<ApiResponse<Company>>(
      `/api/companies/${id}`, 
      data
    );
    
    return response.data.data;
  },

  async deleteCompany(id: string): Promise<void> {
    await apiClient.delete(`/api/companies/${id}`);
  },
};
