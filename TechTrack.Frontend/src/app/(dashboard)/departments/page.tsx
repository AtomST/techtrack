'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useDepartmentStore } from '@/store/departmentStore';
import { Button } from '@/components/ui/Button';
import { Modal } from '@/components/ui/Modal';
import { Input } from '@/components/ui/Input';
import { Building2, Plus, Users, Settings } from 'lucide-react';
import { departmentService } from '@/services/api/departments';
import toast from 'react-hot-toast';

export default function DepartmentsPage() {
  const router = useRouter();
  const { departments, fetchDepartments, loading } = useDepartmentStore();
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [newDepartmentName, setNewDepartmentName] = useState('');
  const [creating, setCreating] = useState(false);

  useEffect(() => {
    fetchDepartments();
  }, []);

  const handleCreateDepartment = async () => {
    if (!newDepartmentName.trim()) {
      toast.error('Введите название отдела');
      return;
    }

    setCreating(true);
    try {
      await departmentService.createDepartment({ name: newDepartmentName });
      toast.success('Отдел создан');
      await fetchDepartments();
      setShowCreateModal(false);
      setNewDepartmentName('');
    } catch (error: any) {
      toast.error(error.message || 'Ошибка создания отдела');
    } finally {
      setCreating(false);
    }
  };

  if (loading && departments.length === 0) {
    return (
      <div className="flex items-center justify-center h-full">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-900">Отделы</h1>
        <Button onClick={() => setShowCreateModal(true)}>
          <Plus className="w-4 h-4 mr-2" />
          Создать отдел
        </Button>
      </div>

      {departments.length === 0 ? (
        <div className="bg-white rounded-lg shadow-sm p-12 text-center">
          <Building2 className="w-16 h-16 text-gray-300 mx-auto mb-4" />
          <h3 className="text-lg font-medium text-gray-900">Нет отделов</h3>
          <p className="text-gray-500 mt-1">Создайте свой первый отдел</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {departments.map((dept) => (
            <div
              key={dept.id}
              className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 hover:shadow-md transition-shadow cursor-pointer"
              onClick={() => router.push(`/departments/${dept.id}`)}
            >
              <Building2 className="w-10 h-10 text-blue-600 mb-3" />
              <h3 className="text-lg font-semibold text-gray-900">{dept.name}</h3>
              <div className="flex items-center justify-between mt-4 pt-4 border-t border-gray-100">
                <div className="flex items-center text-sm text-gray-500">
                  <Users className="w-4 h-4 mr-1" />
                  {dept.equipments?.length || 0} оборудования
                </div>
                <Settings className="w-4 h-4 text-gray-400" />
              </div>
            </div>
          ))}
        </div>
      )}

      <Modal isOpen={showCreateModal} onClose={() => setShowCreateModal(false)} title="Создание отдела">
        <div className="space-y-4">
          <Input
            label="Название отдела"
            placeholder="IT отдел"
            value={newDepartmentName}
            onChange={(e) => setNewDepartmentName(e.target.value)}
          />
          <div className="flex justify-end space-x-3 pt-4">
            <Button variant="outline" onClick={() => setShowCreateModal(false)}>
              Отмена
            </Button>
            <Button onClick={handleCreateDepartment} loading={creating}>
              Создать
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  );
}
