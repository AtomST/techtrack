import { apiClient } from '@/lib/api-client';
import { Department, CreateDepartmentRequest, ApiResponse } from '@/types';

export const departmentService = {
  async getDepartments(companyId?: string): Promise<Department[]> {
    const url = companyId 
      ? `/api/companies/${companyId}/departments`
      : '/api/companies/departments';
    const response = await apiClient.get<ApiResponse<Department[]>>(url);
    
    return response.data.data || [];
  },

  async getDepartment(companyId: string, departmentId: string): Promise<Department> {
    const response = await apiClient.get<ApiResponse<Department>>(
      `/api/companies/${companyId}/departments/${departmentId}`
    );
    
    return response.data.data;
  },

  async createDepartment(data: CreateDepartmentRequest): Promise<Department> {
    const response = await apiClient.post<ApiResponse<Department>>(
      '/api/companies/departments', 
      data
    );
    
    return response.data.data;
  },

  async updateDepartment(
    companyId: string,
    departmentId: string,
    data: Partial<CreateDepartmentRequest>
  ): Promise<Department> {
    const response = await apiClient.put<ApiResponse<Department>>(
      `/api/companies/${companyId}/departments/${departmentId}`,
      data
    );
    
    return response.data.data;
  },

  async deleteDepartment(companyId: string, departmentId: string): Promise<void> {
    await apiClient.delete(`/api/companies/${companyId}/departments/${departmentId}`);
  },
};
