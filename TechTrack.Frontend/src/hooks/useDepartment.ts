'use client';

import { useEffect } from 'react';
import { useDepartmentStore } from '@/store/departmentStore';

export const useDepartment = () => {
  const {
    departments,
    selectedDepartment,
    fetchDepartments,
    setSelectedDepartment,
    loading,
  } = useDepartmentStore();

  useEffect(() => {
    if (departments.length === 0 && !loading) {
      fetchDepartments();
    }
  }, []);

  return {
    departments,
    selectedDepartment,
    fetchDepartments,
    setSelectedDepartment,
    loading,
  };
};
