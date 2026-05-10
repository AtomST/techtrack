'use client';

import { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { companyService } from '@/services/api/companies';
import { departmentService } from '@/services/api/departments';
import { Button } from '@/components/ui/Button';
import { Modal } from '@/components/ui/Modal';
import { Input } from '@/components/ui/Input';
import { Building2, Plus, ArrowLeft, Users, UserPlus } from 'lucide-react';
import { AddEmployeeModal } from '@/components/modals/AddEmployeeModal';
import toast from 'react-hot-toast';

interface CompanyDetail {
  id: string;
  name: string;
  createdAt: string;
}

export default function CompanyDetailPage() {
  const params = useParams();
  const router = useRouter();
  const companyId = params.companyId as string;
  const [company, setCompany] = useState<CompanyDetail | null>(null);
  const [departments, setDepartments] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [showDepartmentModal, setShowDepartmentModal] = useState(false);
  const [showEmployeeModal, setShowEmployeeModal] = useState(false);
  const [newDepartmentName, setNewDepartmentName] = useState('');
  const [creating, setCreating] = useState(false);

  useEffect(() => {
    fetchData();
  }, [companyId]);

  const fetchData = async () => {
    try {
      const [companyData, departmentsData] = await Promise.all([
        companyService.getCompany(companyId),
        departmentService.getMyDepartments(),
      ]);
      setCompany(companyData);
      setDepartments(departmentsData.filter(d => d.companyId === companyId));
    } catch (error) {
      console.error('Failed to fetch data:', error);
      toast.error('Ошибка загрузки данных');
    } finally {
      setLoading(false);
    }
  };

  const handleCreateDepartment = async () => {
    if (!newDepartmentName.trim()) {
      toast.error('Введите название отдела');
      return;
    }

    setCreating(true);
    try {
      await departmentService.createDepartment({ name: newDepartmentName });
      toast.success('Отдел создан');
      await fetchData();
      setShowDepartmentModal(false);
      setNewDepartmentName('');
    } catch (error: any) {
      toast.error(error.message || 'Ошибка создания отдела');
    } finally {
      setCreating(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-full">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (!company) {
    return <div className="p-8">Компания не найдена</div>;
  }

  return (
    <div className="p-8">
      <button
        onClick={() => router.back()}
        className="flex items-center text-gray-600 hover:text-gray-900 mb-4"
      >
        <ArrowLeft className="w-4 h-4 mr-1" />
        Назад
      </button>

      <div className="flex justify-between items-start mb-6">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">{company.name}</h1>
          <p className="text-gray-500 mt-1">ID: {company.id}</p>
        </div>
        <div className="flex space-x-3">
          <Button variant="outline" onClick={() => setShowEmployeeModal(true)}>
            <UserPlus className="w-4 h-4 mr-2" />
            Добавить сотрудника
          </Button>
          <Button onClick={() => setShowDepartmentModal(true)}>
            <Plus className="w-4 h-4 mr-2" />
            Создать отдел
          </Button>
        </div>
      </div>

      <div className="mt-8">
        <h2 className="text-xl font-semibold text-gray-900 mb-4">Отделы</h2>
        {departments.length === 0 ? (
          <div className="bg-white rounded-lg shadow-sm p-8 text-center">
            <Users className="w-12 h-12 text-gray-300 mx-auto mb-3" />
            <p className="text-gray-500">Нет отделов</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {departments.map((dept) => (
              <div
                key={dept.id}
                className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 hover:shadow-md transition-shadow cursor-pointer"
                onClick={() => router.push(`/departments/${dept.id}`)}
              >
                <Building2 className="w-8 h-8 text-blue-600 mb-3" />
                <h3 className="text-lg font-semibold text-gray-900">{dept.name}</h3>
                <p className="text-sm text-gray-500 mt-2">
                  Оборудования: {dept.equipments?.length || 0}
                </p>
              </div>
            ))}
          </div>
        )}
      </div>

      <Modal isOpen={showDepartmentModal} onClose={() => setShowDepartmentModal(false)} title="Создание отдела">
        <div className="space-y-4">
          <Input
            label="Название отдела"
            placeholder="IT отдел"
            value={newDepartmentName}
            onChange={(e) => setNewDepartmentName(e.target.value)}
          />
          <div className="flex justify-end space-x-3 pt-4">
            <Button variant="outline" onClick={() => setShowDepartmentModal(false)}>
              Отмена
            </Button>
            <Button onClick={handleCreateDepartment} loading={creating}>
              Создать
            </Button>
          </div>
        </div>
      </Modal>

      <AddEmployeeModal
        isOpen={showEmployeeModal}
        onClose={() => setShowEmployeeModal(false)}
        companyId={companyId}
      />
    </div>
  );
}
