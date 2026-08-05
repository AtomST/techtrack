'use client';

import { useEquipmentStore } from '@/store/equipmentStore';

export const useEquipment = () => {
  const {
    equipments,
    selectedEquipment,
    issues,
    maintenances,
    loading,
    fetchEquipments,
    setSelectedEquipment,
    fetchIssues,
    fetchMaintenances,
    createMaintenance,
    completeMaintenance,
    scheduleMaintenance,
  } = useEquipmentStore();

  return {
    equipments,
    selectedEquipment,
    issues,
    maintenances,
    loading,
    fetchEquipments,
    setSelectedEquipment,
    fetchIssues,
    fetchMaintenances,
    createMaintenance,
    completeMaintenance,
    scheduleMaintenance,
  };
};
