import { create } from 'zustand';
import { Company } from '@/types';
import { companyService } from '@/services/api/companies';

interface CompanyState {
  companies: Company[];
  loadingCompanies: boolean;
  fetchCompanies: () => Promise<void>;
  createCompany: (name: string) => Promise<void>;
}

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
}));
