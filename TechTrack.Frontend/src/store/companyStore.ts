import { create } from 'zustand';
import { Department, Equipment, Company } from '@/types';
import { departmentService } from '@/services/api/departments';
import { companyService } from '@/services/api/companies';

interface CompanyState {
  companies: Company[];
  loadingCompanies: boolean;
  fetchCompanies: () => Promise<void>;
  createCompany: (name: string) => Promise<void>;
  
  departments: Department[];
  selectedDepartment: Department | null;
  departmentsDetailsMap: Record<string, Department & { equipments: Equipment[] }>;
  loadingDepartments: boolean;
  loadingDepartmentDetails: Record<string, boolean>;
  fetchDepartments: () => Promise<void>;
  setSelectedDepartment: (department: Department | null) => void;
  fetchDepartmentDetails: (departmentId: string) => Promise<Department & { equipments: Equipment[] }>;
  clearDepartmentDetails: (departmentId: string) => void;
}

// Функция для получения роли из токена
const getRoleFromToken = (): string | null => {
  if (typeof window === 'undefined') return null;
  const token = localStorage.getItem('accessToken');
  if (!token) return null;
  
  try {
    const parts = token.split('.');
    if (parts.length === 3) {
      const payload = JSON.parse(atob(parts[1]));
      console.log('Token role:', payload.role);
      return payload.role;
    }
  } catch (e) {
    console.error('Failed to parse token:', e);
  }
  return null;
};

export const useCompanyStore = create<CompanyState>((set, get) => ({
  companies: [],
  loadingCompanies: false,
  
  fetchCompanies: async () => {
    set({ loadingCompanies: true });
    try {
      const companies = await companyService.getCompanies();
      set({ companies });
    } catch (error) {
      console.error('Failed to fetch companies:', error);
      set({ companies: [] });
    } finally {
      set({ loadingCompanies: false });
    }
  },
  
  createCompany: async (name: string) => {
    try {
      await companyService.createCompany({ name });
      await get().fetchCompanies();
    } catch (error) {
      console.error('Failed to create company:', error);
      throw error;
    }
  },
  
  departments: [],
  selectedDepartment: null,
  departmentsDetailsMap: {},
  loadingDepartments: false,
  loadingDepartmentDetails: {},

  fetchDepartments: async () => {
    set({ loadingDepartments: true });
    try {
      const role = getRoleFromToken();
      console.log('FetchDepartments - role:', role);
      
      let departments: Department[] = [];
      
      const rolesWithFullAccess = ['Admin', 'CompanyHead', 'PlatformAdmin', 'Dev'];
      
      if (role && rolesWithFullAccess.includes(role)) {
        console.log('Calling getAllDepartments');
        departments = await departmentService.getAllDepartments();
      } else {
        console.log('Calling getMyDepartments');
        departments = await departmentService.getMyDepartments();
      }
      
      console.log('Departments received:', departments.length);
      set({ departments });
    } catch (error) {
      console.error('Failed to fetch departments:', error);
      set({ departments: [] });
    } finally {
      set({ loadingDepartments: false });
    }
  },

  setSelectedDepartment: (department) => set({ selectedDepartment: department }),

  fetchDepartmentDetails: async (departmentId: string) => {
    // Если уже загружены, возвращаем из кэша
    if (get().departmentsDetailsMap[departmentId]) {
      return get().departmentsDetailsMap[departmentId];
    }
    
    set({ loadingDepartmentDetails: { ...get().loadingDepartmentDetails, [departmentId]: true } });
    
    try {
      console.log('Fetching department details for:', departmentId);
      const departmentDetails = await departmentService.getDepartment(departmentId);
      console.log('Department details received:', departmentDetails);
      
      set({ 
        departmentsDetailsMap: { ...get().departmentsDetailsMap, [departmentId]: departmentDetails } 
      });
      return departmentDetails;
    } catch (error) {
      console.error('Failed to fetch department details:', error);
      throw error;
    } finally {
      set({ loadingDepartmentDetails: { ...get().loadingDepartmentDetails, [departmentId]: false } });
    }
  },

  clearDepartmentDetails: (departmentId: string) => {
    const newMap = { ...get().departmentsDetailsMap };
    delete newMap[departmentId];
    set({ departmentsDetailsMap: newMap });
  },
}));
