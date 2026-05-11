import { create } from 'zustand';
import { Department } from '@/types';
import { departmentService } from '@/services/api/departments';
import { useAuthStore } from './authStore';

interface DepartmentState {
  departments: Department[];
  selectedDepartment: Department | null;
  loading: boolean;
  fetchDepartments: () => Promise<void>;
  setSelectedDepartment: (department: Department | null) => void;
}

export const useDepartmentStore = create<DepartmentState>((set, get) => ({
  departments: [],
  selectedDepartment: null,
  loading: false,

  fetchDepartments: async () => {
    set({ loading: true });
    try {
      // Получаем пользователя из store (уже с ролью)
      const { user } = useAuthStore.getState();
      const role = user?.role;
      
      console.log('fetchDepartments - user role:', role);
      
      let departments: Department[] = [];
      
      // Admin или CompanyHead получают все отделы
      if (role === 'Admin' || role === 'CompanyHead' || role === 'PlatformAdmin' || role === 'Dev') {
        console.log('Using getAllDepartments');
        departments = await departmentService.getAllDepartments();
      } else {
        console.log('Using getMyDepartments');
        departments = await departmentService.getMyDepartments();
      }
      
      set({ departments });
    } catch (error) {
      console.error('Failed to fetch departments:', error);
      set({ departments: [] });
    } finally {
      set({ loading: false });
    }
  },

  setSelectedDepartment: (department) => set({ selectedDepartment: department }),
}));
