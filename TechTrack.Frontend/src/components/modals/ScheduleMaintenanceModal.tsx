'use client';

import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Modal } from '@/components/ui/Modal';
import { Input } from '@/components/ui/Input';
import { Button } from '@/components/ui/Button';
import { useEquipmentStore } from '@/store/equipmentStore';
import { departmentService } from '@/services/api/departments';
import { useAuthStore } from '@/store/authStore';
import toast from 'react-hot-toast';

const schema = z.object({
  maintenanceName: z.string().min(2, 'Название должно содержать минимум 2 символа'),
  recurrenceTypeId: z.number(),
  intervalValue: z.number().min(1, 'Интервал должен быть больше 0'),
  nextMaintenanceDate: z.string().min(1, 'Выберите дату'),
  responsibleUserId: z.string().optional(),
  notificationAdvanceDays: z.number().min(0).max(30),
});

type FormData = z.infer<typeof schema>;

interface ScheduleMaintenanceModalProps {
  isOpen: boolean;
  onClose: () => void;
  equipmentId: string;
}

export function ScheduleMaintenanceModal({ isOpen, onClose, equipmentId }: ScheduleMaintenanceModalProps) {
  const [loading, setLoading] = useState(false);
  const [users, setUsers] = useState<any[]>([]);
  const { scheduleMaintenance } = useEquipmentStore();
  const { user } = useAuthStore();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      recurrenceTypeId: 1,
      intervalValue: 1,
      notificationAdvanceDays: 3,
    },
  });

  useEffect(() => {
    if (isOpen) {
      fetchUsers();
    }
  }, [isOpen]);

  const fetchUsers = async () => {
    try {
      let departments;
      if (user?.role === 'Admin' || user?.role === 'CompanyHead') {
        departments = await departmentService.getAllDepartments();
      } else {
        departments = await departmentService.getMyDepartments();
      }
      
      const allUsers: any[] = [];
      for (const dept of departments) {
        try {
          const employees = await departmentService.getEmployees(dept.id);
          allUsers.push(...employees);
        } catch (err) {
          console.error(`Failed to fetch employees for department ${dept.id}:`, err);
        }
      }
      setUsers(allUsers);
    } catch (error) {
      console.error('Failed to fetch users:', error);
    }
  };

  const onSubmit = async (data: FormData) => {
    setLoading(true);
    try {
      // Преобразуем локальную дату в UTC
      const localDate = new Date(data.nextMaintenanceDate);
      const utcDate = new Date(Date.UTC(
        localDate.getFullYear(),
        localDate.getMonth(),
        localDate.getDate(),
        localDate.getHours(),
        localDate.getMinutes(),
        localDate.getSeconds()
      ));
      
      await scheduleMaintenance(equipmentId, {
        ...data,
        nextMaintenanceDate: utcDate.toISOString(),
      });
      toast.success('ТО запланировано');
      reset();
      onClose();
    } catch (error: any) {
      toast.error(error.message || 'Ошибка планирования ТО');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Планирование ТО" size="lg">
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Название ТО
          </label>
          <input
            type="text"
            placeholder="Ежемесячная проверка"
            {...register('maintenanceName')}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          {errors.maintenanceName && (
            <p className="mt-1 text-sm text-red-600">{errors.maintenanceName.message}</p>
          )}
        </div>
        
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Периодичность
            </label>
            <select
              className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
              {...register('recurrenceTypeId', { valueAsNumber: true })}
            >
              <option value={1}>Ежедневно</option>
              <option value={2}>Еженедельно</option>
              <option value={3}>Ежемесячно</option>
            </select>
          </div>
          
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Интервал
            </label>
            <input
              type="number"
              {...register('intervalValue', { valueAsNumber: true })}
              className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
            {errors.intervalValue && (
              <p className="mt-1 text-sm text-red-600">{errors.intervalValue.message}</p>
            )}
          </div>
        </div>
        
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Дата следующего ТО (локальное время)
          </label>
          <input
            type="datetime-local"
            {...register('nextMaintenanceDate')}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <p className="text-xs text-gray-500 mt-1">
            Дата будет автоматически преобразована в UTC при отправке на сервер
          </p>
          {errors.nextMaintenanceDate && (
            <p className="mt-1 text-sm text-red-600">{errors.nextMaintenanceDate.message}</p>
          )}
        </div>
        
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Ответственный
          </label>
          <select
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
            {...register('responsibleUserId')}
          >
            <option value="">Не назначен</option>
            {users.map((usr) => (
              <option key={usr.userId} value={usr.userId}>
                {usr.userName || usr.userFullname || usr.fullName}
              </option>
            ))}
          </select>
        </div>
        
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Дней до напоминания
          </label>
          <input
            type="number"
            {...register('notificationAdvanceDays', { valueAsNumber: true })}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          {errors.notificationAdvanceDays && (
            <p className="mt-1 text-sm text-red-600">{errors.notificationAdvanceDays.message}</p>
          )}
        </div>
        
        <div className="flex justify-end space-x-3 pt-4 border-t border-gray-100">
          <Button type="button" variant="outline" onClick={onClose}>
            Отмена
          </Button>
          <Button type="submit" loading={loading}>
            Запланировать
          </Button>
        </div>
      </form>
    </Modal>
  );
}
