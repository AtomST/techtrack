'use client';

import { useEffect } from 'react';
import { useCompanyStore } from '@/store/companyStore';

export const useCompany = () => {
  const {
    companies,
    fetchCompanies,
    loadingCompanies,
    departments,
    fetchDepartments,
    loadingDepartments,
  } = useCompanyStore();

  useEffect(() => {
    if (companies.length === 0 && !loadingCompanies) {
      fetchCompanies();
    }
  }, []);

  useEffect(() => {
    if (departments.length === 0 && !loadingDepartments) {
      fetchDepartments();
    }
  }, []);

  return {
    companies,
    departments,
    loadingCompanies,
    loadingDepartments,
    fetchCompanies,
    fetchDepartments,
  };
};
