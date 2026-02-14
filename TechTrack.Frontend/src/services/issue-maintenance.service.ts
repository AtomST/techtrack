import { apiClient } from '@/lib/api-client';
import { Issue, CreateIssueRequest, Maintenance, CreateMaintenanceRequest, ApiResponse } from '@/types';

export const issueService = {
  async getIssues(equipmentId: string): Promise<Issue[]> {
    const response = await apiClient.get<ApiResponse<Issue[]>>(
      `/api/equipments/${equipmentId}/issues`
    );
    
    return response.data.data || [];
  },

  async getIssue(equipmentId: string, issueId: string): Promise<Issue> {
    const response = await apiClient.get<ApiResponse<Issue>>(
      `/api/equipments/${equipmentId}/issues/${issueId}`
    );
    
    return response.data.data;
  },

  async createIssue(data: CreateIssueRequest): Promise<Issue> {
    const response = await apiClient.post<ApiResponse<Issue>>('/api/issues', data);
    
    return response.data.data;
  },

  async updateIssue(issueId: string, data: Partial<CreateIssueRequest>): Promise<Issue> {
    const response = await apiClient.put<ApiResponse<Issue>>(
      `/api/issues/${issueId}`, 
      data
    );
    
    return response.data.data;
  },

  async deleteIssue(issueId: string): Promise<void> {
    await apiClient.delete(`/api/issues/${issueId}`);
  },
};

export const maintenanceService = {
  async getMaintenances(equipmentId: string): Promise<Maintenance[]> {
    const response = await apiClient.get<ApiResponse<Maintenance[]>>(
      `/api/equipments/${equipmentId}/maintenances`
    );
    
    return response.data.data || [];
  },

  async getMaintenance(equipmentId: string, maintenanceId: string): Promise<Maintenance> {
    const response = await apiClient.get<ApiResponse<Maintenance>>(
      `/api/equipments/${equipmentId}/maintenances/${maintenanceId}`
    );
    
    return response.data.data;
  },

  async createMaintenance(data: CreateMaintenanceRequest): Promise<Maintenance> {
    const response = await apiClient.post<ApiResponse<Maintenance>>(
      '/api/maintenances', 
      data
    );
    
    return response.data.data;
  },

  async updateMaintenance(
    maintenanceId: string,
    data: Partial<CreateMaintenanceRequest>
  ): Promise<Maintenance> {
    const response = await apiClient.put<ApiResponse<Maintenance>>(
      `/api/maintenances/${maintenanceId}`,
      data
    );
    
    return response.data.data;
  },

  async deleteMaintenance(maintenanceId: string): Promise<void> {
    await apiClient.delete(`/api/maintenances/${maintenanceId}`);
  },
};
