'use client';

import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Modal } from '@/components/ui/Modal';
import { Input } from '@/components/ui/Input';
import { Button } from '@/components/ui/Button';
import { departmentService } from '@/services/api/departments';
import { usersService } from '@/services/api/users';
import { useAuthStore } from '@/store/authStore';
import { useCompanyStore } from '@/store/companyStore';
import toast from 'react-hot-toast';

const schema = z.object({
  name: z.string().min(2, 'Название должно содержать минимум 2 символа'),
  responsibleUserId: z.string().optional(),
});

type FormData = z.infer<typeof schema>;

interface CreateDepartmentModalProps {
  isOpen: boolean;
  onClose: () => void;
}

export function CreateDepartmentModal({ isOpen, onClose }: CreateDepartmentModalProps) {
  const [loading, setLoading] = useState(false);
  const [users, setUsers] = useState<any[]>([]);
  const [loadingUsers, setLoadingUsers] = useState(false);
  const { user } = useAuthStore();
  const { fetchDepartments } = useCompanyStore();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  useEffect(() => {
    if (isOpen && user?.companyId) {
      loadUsers();
    }
  }, [isOpen, user?.companyId]);

  const loadUsers = async () => {
    setLoadingUsers(true);
    try {
      const usersList = await usersService.getCompanyUsers(user?.companyId || '');
      setUsers(usersList);
    } catch (error) {
      console.error('Failed to load users:', error);
    } finally {
      setLoadingUsers(false);
    }
  };

  const onSubmit = async (data: FormData) => {
    setLoading(true);
    try {
      await departmentService.createDepartment(data);
      toast.success('Отдел создан успешно');
      await fetchDepartments();
      reset();
      onClose();
    } catch (error: any) {
      toast.error(error.message || 'Ошибка создания отдела');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Создание отдела" size="md">
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Название отдела
          </label>
          <input
            type="text"
            placeholder="IT отдел"
            {...register('name')}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          {errors.name && (
            <p className="mt-1 text-sm text-red-600">{errors.name.message}</p>
          )}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Руководитель отдела
          </label>
          <select
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
            {...register('responsibleUserId')}
          >
            <option value="">Не назначен</option>
            {users.map((u) => (
              <option key={u.id} value={u.id}>
                {u.fullName} ({u.email})
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
            Создать
          </Button>
        </div>
      </form>
    </Modal>
  );
}
