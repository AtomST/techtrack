'use client';

import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Modal } from '@/components/ui/Modal';
import { Input } from '@/components/ui/Input';
import { Button } from '@/components/ui/Button';
import { equipmentService } from '@/services/api/equipment';
import { apiClient } from '@/services/api/client';
import { useAuthStore } from '@/store/authStore';
import { useDepartmentStore } from '@/store/departmentStore';
import toast from 'react-hot-toast';

const schema = z.object({
  name: z.string().min(2, 'Название должно содержать минимум 2 символа'),
  serialNumber: z.string().optional(),
  description: z.string().optional(),
  responsibleUserId: z.string().optional(),
});

type FormData = z.infer<typeof schema>;

interface CreateEquipmentModalProps {
  isOpen: boolean;
  onClose: () => void;
  departmentId: string;
}

interface User {
  userId: string;
  fullName: string;
  email?: string;
}

export function CreateEquipmentModal({ isOpen, onClose, departmentId }: CreateEquipmentModalProps) {
  const [loading, setLoading] = useState(false);
  const [users, setUsers] = useState<User[]>([]);
  const [loadingUsers, setLoadingUsers] = useState(false);
  const { user } = useAuthStore();
  const { fetchDepartments } = useDepartmentStore();

  const {
    register,
    handleSubmit,
    reset,
    setValue,
    watch,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      name: '',
      serialNumber: '',
      description: '',
      responsibleUserId: '',
    },
  });

  useEffect(() => {
    if (isOpen) {
      loadUsers();
      reset();
    }
  }, [isOpen]);

  const loadUsers = async () => {
    setLoadingUsers(true);
    try {
      let usersList: User[] = [];
      
      // Сначала пробуем получить сотрудников отдела
      try {
        console.log('Loading department employees for:', departmentId);
        const response = await apiClient.get<any>(`/departments/${departmentId}/employees`);
        const data = Array.isArray(response) ? response : response?.data || [];
        usersList = data.map((item: any) => ({
          userId: item.userId || item.id,
          fullName: item.fullName,
          email: item.email,
        }));
        console.log('Department employees loaded:', usersList.length);
      } catch (err) {
        console.log('Failed to get department employees, trying company employees:', err);
        // Если не получилось, получаем сотрудников компании
        const companyId = user?.companyId;
        if (companyId) {
          const response = await apiClient.get<any>(`/companies/${companyId}/employees`);
          const data = Array.isArray(response) ? response : response?.data || [];
          usersList = data.map((item: any) => ({
            userId: item.userId || item.id,
            fullName: item.fullName,
            email: item.email,
          }));
          console.log('Company employees loaded:', usersList.length);
        }
      }
      
      setUsers(usersList);
    } catch (error) {
      console.error('Failed to load users:', error);
      toast.error('Не удалось загрузить список пользователей');
    } finally {
      setLoadingUsers(false);
    }
  };

  const onSubmit = async (data: FormData) => {
    console.log('Form submitted with data:', data);
    setLoading(true);
    try {
      const payload = {
        name: data.name,
        serialNumber: data.serialNumber || undefined,
        description: data.description || undefined,
        responsibleUserId: data.responsibleUserId || undefined,
      };
      
      console.log('Creating equipment with payload:', payload);
      await equipmentService.createEquipment(departmentId, payload);
      
      toast.success('Оборудование добавлено успешно');
      // Обновляем список отделов, чтобы отобразить новое оборудование
      await fetchDepartments();
      reset();
      onClose();
    } catch (error: any) {
      console.error('Create equipment error:', error);
      toast.error(error.message || 'Ошибка добавления оборудования');
    } finally {
      setLoading(false);
    }
  };

  const selectedUserId = watch('responsibleUserId');

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Добавление оборудования" size="md">
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Название оборудования <span className="text-red-500">*</span>
          </label>
          <input
            type="text"
            placeholder="Ноутбук Dell XPS"
            {...register('name')}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          {errors.name && (
            <p className="mt-1 text-sm text-red-600">{errors.name.message}</p>
          )}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Серийный номер
          </label>
          <input
            type="text"
            placeholder="SN-12345"
            {...register('serialNumber')}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Описание
          </label>
          <textarea
            placeholder="Описание оборудования..."
            {...register('description')}
            rows={3}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Ответственный пользователь
          </label>
          <select
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
            value={selectedUserId || ''}
            onChange={(e) => setValue('responsibleUserId', e.target.value)}
          >
            <option value="">Не назначен</option>
            {users.map((u) => (
              <option key={u.userId} value={u.userId}>
                {u.fullName} {u.email ? `(${u.email})` : ''}
              </option>
            ))}
          </select>
          {loadingUsers && (
            <p className="text-xs text-gray-500 mt-1">Загрузка пользователей...</p>
          )}
        </div>

        <div className="flex justify-end space-x-3 pt-4 border-t border-gray-100">
          <Button type="button" variant="outline" onClick={onClose}>
            Отмена
          </Button>
          <Button type="submit" loading={loading}>
            Добавить
          </Button>
        </div>
      </form>
    </Modal>
  );
}
