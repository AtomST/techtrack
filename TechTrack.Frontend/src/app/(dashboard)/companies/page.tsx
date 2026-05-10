'use client';

import { useState, useEffect } from 'react';
import { useCompanyStore } from '@/store/companyStore';
import { Button } from '@/components/ui/Button';
import { Input } from '@/components/ui/Input';
import { Modal } from '@/components/ui/Modal';
import { Building2, Plus } from 'lucide-react';
import toast from 'react-hot-toast';

export default function CompaniesPage() {
  const { companies, fetchCompanies, createCompany, loadingCompanies } = useCompanyStore();
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [newCompanyName, setNewCompanyName] = useState('');

  useEffect(() => {
    fetchCompanies();
  }, []);

  const handleCreateCompany = async () => {
    if (!newCompanyName.trim()) {
      toast.error('Введите название компании');
      return;
    }
    await createCompany(newCompanyName);
    setShowCreateModal(false);
    setNewCompanyName('');
  };

  if (loadingCompanies && companies.length === 0) {
    return (
      <div className="flex items-center justify-center h-full">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-900">Компании</h1>
        <Button onClick={() => setShowCreateModal(true)}>
          <Plus className="w-4 h-4 mr-2" />
          Создать компанию
        </Button>
      </div>

      {companies.length === 0 ? (
        <div className="bg-white rounded-lg shadow-sm p-12 text-center">
          <Building2 className="w-16 h-16 text-gray-300 mx-auto mb-4" />
          <h3 className="text-lg font-medium text-gray-900">Нет компаний</h3>
          <p className="text-gray-500 mt-1">Создайте свою первую компанию</p>
          <Button className="mt-4" onClick={() => setShowCreateModal(true)}>
            <Plus className="w-4 h-4 mr-2" />
            Создать компанию
          </Button>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {companies.map((company) => (
            <div key={company.id} className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 hover:shadow-md transition-shadow">
              <Building2 className="w-10 h-10 text-blue-600 mb-3" />
              <h3 className="text-lg font-semibold text-gray-900">{company.name}</h3>
              <p className="text-sm text-gray-500 mt-1">
                Создана: {new Date(company.createdAt).toLocaleDateString()}
              </p>
            </div>
          ))}
        </div>
      )}

      <Modal isOpen={showCreateModal} onClose={() => setShowCreateModal(false)} title="Создание компании">
        <div className="space-y-4">
          <Input
            label="Название компании"
            placeholder="ООО ТехТрек"
            value={newCompanyName}
            onChange={(e) => setNewCompanyName(e.target.value)}
          />
          <div className="flex justify-end space-x-3 pt-4">
            <Button variant="outline" onClick={() => setShowCreateModal(false)}>
              Отмена
            </Button>
            <Button onClick={handleCreateCompany}>
              Создать
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  );
}
