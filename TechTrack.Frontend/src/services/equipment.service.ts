import { apiClient } from '@/lib/api-client';
import { Equipment, CreateEquipmentRequest, ApiResponse } from '@/types';

export const equipmentService = {
  async getEquipment(companyId: string, departmentId: string): Promise<Equipment[]> {
    const response = await apiClient.get<ApiResponse<Equipment[]>>(
      `/api/companies/${companyId}/departments/${departmentId}/equipments`
    );
    
    return response.data.data || [];
  },

  async getEquipmentById(
    companyId: string,
    departmentId: string,
    equipmentId: string
  ): Promise<Equipment> {
    const response = await apiClient.get<ApiResponse<Equipment>>(
      `/api/companies/${companyId}/departments/${departmentId}/equipments/${equipmentId}`
    );
    
    return response.data.data;
  },

  async createEquipment(
    companyId: string,
    departmentId: string,
    data: CreateEquipmentRequest
  ): Promise<Equipment> {
    const response = await apiClient.post<ApiResponse<Equipment>>(
      `/api/companies/${companyId}/departments/${departmentId}/equipments`,
      data
    );
    
    return response.data.data;
  },

  async updateEquipment(
    companyId: string,
    departmentId: string,
    equipmentId: string,
    data: Partial<CreateEquipmentRequest>
  ): Promise<Equipment> {
    const response = await apiClient.put<ApiResponse<Equipment>>(
      `/api/companies/${companyId}/departments/${departmentId}/equipments/${equipmentId}`,
      data
    );
    
    return response.data.data;
  },

  async deleteEquipment(
    companyId: string,
    departmentId: string,
    equipmentId: string
  ): Promise<void> {
    await apiClient.delete(
      `/api/companies/${companyId}/departments/${departmentId}/equipments/${equipmentId}`
    );
  },
};
