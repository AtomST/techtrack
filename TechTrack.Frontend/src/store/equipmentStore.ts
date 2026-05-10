import { create } from 'zustand';
import { Equipment, Issue, Maintenance } from '@/types';
import { equipmentService } from '@/services/api/equipment';

interface EquipmentState {
  equipments: Equipment[];
  selectedEquipment: Equipment | null;
  issues: Issue[];
  maintenances: Maintenance[];
  loading: boolean;
  loadingIssues: boolean;
  loadingMaintenances: boolean;
  fetchEquipments: (departmentId: string) => Promise<void>;
  setSelectedEquipment: (equipment: Equipment | null) => void;
  fetchIssues: (equipmentId: string) => Promise<void>;
  fetchMaintenances: (equipmentId: string) => Promise<void>;
  createMaintenance: (equipmentId: string, data: any) => Promise<void>;
  completeMaintenance: (maintenanceId: string, data: any) => Promise<void>;
  scheduleMaintenance: (equipmentId: string, data: any) => Promise<void>;
}

export const useEquipmentStore = create<EquipmentState>((set, get) => ({
  equipments: [],
  selectedEquipment: null,
  issues: [],
  maintenances: [],
  loading: false,
  loadingIssues: false,
  loadingMaintenances: false,

  fetchEquipments: async (departmentId: string) => {
    set({ loading: true });
    try {
      const equipments = await equipmentService.getEquipments(departmentId);
      set({ equipments });
    } finally {
      set({ loading: false });
    }
  },

  setSelectedEquipment: (equipment) => set({ selectedEquipment: equipment }),

  fetchIssues: async (equipmentId: string) => {
    // Проверяем, не идет ли уже загрузка
    const state = get();
    if (state.loadingIssues) return;
    
    set({ loadingIssues: true });
    try {
      const issues = await equipmentService.getIssues(equipmentId);
      set({ issues });
    } finally {
      set({ loadingIssues: false });
    }
  },

  fetchMaintenances: async (equipmentId: string) => {
    const state = get();
    if (state.loadingMaintenances) return;
    
    set({ loadingMaintenances: true });
    try {
      const maintenances = await equipmentService.getMaintenances(equipmentId);
      set({ maintenances });
    } finally {
      set({ loadingMaintenances: false });
    }
  },

  createMaintenance: async (equipmentId: string, data: any) => {
    set({ loading: true });
    try {
      await equipmentService.createMaintenance(equipmentId, data);
      await get().fetchMaintenances(equipmentId);
    } finally {
      set({ loading: false });
    }
  },

  completeMaintenance: async (maintenanceId: string, data: any) => {
    set({ loading: true });
    try {
      await equipmentService.completeMaintenance(maintenanceId, data);
      const { selectedEquipment } = get();
      if (selectedEquipment) {
        await get().fetchMaintenances(selectedEquipment.id);
      }
    } finally {
      set({ loading: false });
    }
  },

  scheduleMaintenance: async (equipmentId: string, data: any) => {
    set({ loading: true });
    try {
      await equipmentService.scheduleMaintenance(equipmentId, data);
      await get().fetchMaintenances(equipmentId);
    } finally {
      set({ loading: false });
    }
  },
}));
