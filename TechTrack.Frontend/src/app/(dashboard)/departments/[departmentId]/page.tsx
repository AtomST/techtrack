'use client';

import { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { departmentService } from '@/services/api/departments';
import { equipmentService } from '@/services/api/equipment';
import { Button } from '@/components/ui/Button';
import { Modal } from '@/components/ui/Modal';
import { Input } from '@/components/ui/Input';
import { Building2, Plus, ArrowLeft, UserPlus } from 'lucide-react';
import { AddEmployeeModal } from '@/components/modals/AddEmployeeModal';
import toast from 'react-hot-toast';

interface Equipment {
  id: string;
  name: string;
  description?: string;
  createdAt: string;
}

interface DepartmentDetail {
  id: string;
  name: string;
  companyId: string;
  responsibleUserId?: string;
  equipments: Equipment[];
}

export default function DepartmentDetailPage() {
  const params = useParams();
  const router = useRouter();
  const departmentId = params.departmentId as string;
  const [department, setDepartment] = useState<DepartmentDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [showEquipmentModal, setShowEquipmentModal] = useState(false);
  const [showEmployeeModal, setShowEmployeeModal] = useState(false);
  const [newEquipmentName, setNewEquipmentName] = useState('');
  const [newEquipmentDesc, setNewEquipmentDesc] = useState('');
  const [creating, setCreating] = useState(false);

  useEffect(() => {
    fetchDepartment();
  }, [departmentId]);

  const fetchDepartment = async () => {
    try {
      const data = await departmentService.getDepartment(departmentId);
      // Приводим данные к нужному типу
      const formattedData: DepartmentDetail = {
        id: data.id,
        name: data.name,
        companyId: data.companyId,
        responsibleUserId: data.responsibleUserId,
        equipments: data.equipments || [],
      };
      setDepartment(formattedData);
    } catch (error) {
      console.error('Failed to fetch department:', error);
      toast.error('Ошибка загрузки отдела');
    } finally {
      setLoading(false);
    }
  };

  const handleAddEquipment = async () => {
    if (!newEquipmentName.trim()) {
      toast.error('Введите название оборудования');
      return;
    }

    setCreating(true);
    try {
      await equipmentService.createEquipment(departmentId, {
        name: newEquipmentName,
        description: newEquipmentDesc,
      });
      toast.success('Оборудование добавлено');
      await fetchDepartment();
      setShowEquipmentModal(false);
      setNewEquipmentName('');
      setNewEquipmentDesc('');
    } catch (error: any) {
      toast.error(error.message || 'Ошибка добавления оборудования');
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

  if (!department) {
    return <div className="p-8">Отдел не найден</div>;
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
          <h1 className="text-2xl font-bold text-gray-900">{department.name}</h1>
          <p className="text-gray-500 mt-1">ID: {department.id}</p>
        </div>
        <div className="flex space-x-3">
          <Button variant="outline" onClick={() => setShowEmployeeModal(true)}>
            <UserPlus className="w-4 h-4 mr-2" />
            Добавить сотрудника
          </Button>
          <Button onClick={() => setShowEquipmentModal(true)}>
            <Plus className="w-4 h-4 mr-2" />
            Добавить оборудование
          </Button>
        </div>
      </div>

      {department.equipments.length === 0 ? (
        <div className="bg-white rounded-lg shadow-sm p-12 text-center">
          <Building2 className="w-16 h-16 text-gray-300 mx-auto mb-4" />
          <h3 className="text-lg font-medium text-gray-900">Нет оборудования</h3>
          <p className="text-gray-500 mt-1">Добавьте оборудование в отдел</p>
        </div>
      ) : (
        <div>
          <h2 className="text-xl font-semibold text-gray-900 mb-4">Оборудование</h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {department.equipments.map((equipment) => (
              <div
                key={equipment.id}
                className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 hover:shadow-md transition-shadow cursor-pointer"
                onClick={() => router.push(`/equipment/${equipment.id}`)}
              >
                <h3 className="text-lg font-semibold text-gray-900">{equipment.name}</h3>
                {equipment.description && (
                  <p className="text-sm text-gray-500 mt-1">{equipment.description}</p>
                )}
                <p className="text-xs text-gray-400 mt-4">
                  Добавлено: {new Date(equipment.createdAt).toLocaleDateString()}
                </p>
              </div>
            ))}
          </div>
        </div>
      )}

      <Modal isOpen={showEquipmentModal} onClose={() => setShowEquipmentModal(false)} title="Добавление оборудования">
        <div className="space-y-4">
          <Input
            label="Название"
            placeholder="Ноутбук Dell XPS"
            value={newEquipmentName}
            onChange={(e) => setNewEquipmentName(e.target.value)}
          />
          <Input
            label="Описание"
            placeholder="Описание оборудования..."
            value={newEquipmentDesc}
            onChange={(e) => setNewEquipmentDesc(e.target.value)}
          />
          <div className="flex justify-end space-x-3 pt-4">
            <Button variant="outline" onClick={() => setShowEquipmentModal(false)}>
              Отмена
            </Button>
            <Button onClick={handleAddEquipment} loading={creating}>
              Добавить
            </Button>
          </div>
        </div>
      </Modal>

      <AddEmployeeModal
        isOpen={showEmployeeModal}
        onClose={() => setShowEmployeeModal(false)}
        companyId={department.companyId}
      />
    </div>
  );
}
