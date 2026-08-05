'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Modal } from '@/components/ui/Modal';
import { Input } from '@/components/ui/Input';
import { Button } from '@/components/ui/Button';
import { equipmentService } from '@/services/api/equipment';
import { useEquipmentStore } from '@/store/equipmentStore';
import toast from 'react-hot-toast';

const schema = z.object({
  name: z.string().min(2, 'Название должно содержать минимум 2 символа'),
  description: z.string().optional(),
  statusId: z.number().min(1, 'Выберите критичность'),
});

type FormData = z.infer<typeof schema>;

interface CreateIssueModalProps {
  isOpen: boolean;
  onClose: () => void;
  equipmentId: string;
}

const statusOptions = [
  { id: 1, name: 'Низкая', color: 'bg-green-100 text-green-800 border-green-200', description: 'Может быть исправлена позже'  },
  { id: 2, name: 'Средняя', color: 'bg-yellow-100 text-yellow-800 border-yellow-200', description: 'Требует внимания в ближайшее время' },
  { id: 3, name: 'Критическая', color: 'bg-red-100 text-red-800 border-red-200', description: 'Требует немедленного вмешательства' },
];

export function CreateIssueModal({ isOpen, onClose, equipmentId }: CreateIssueModalProps) {
  const [loading, setLoading] = useState(false);
  const { fetchIssues } = useEquipmentStore();

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
      statusId: 1,
    },
  });

  const selectedStatusId = watch('statusId');

  const onSubmit = async (data: FormData) => {
    setLoading(true);
    try {
      await equipmentService.createIssue(equipmentId, {
        name: data.name,
        description: data.description,
        statusId: data.statusId,
      });
      toast.success('Неисправность добавлена');
      await fetchIssues(equipmentId);
      reset();
      onClose();
    } catch (error: any) {
      toast.error(error.message || 'Ошибка добавления неисправности');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Добавление неисправности" size="md">
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
        {/* Название */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Название неисправности
          </label>
          <input
            type="text"
            placeholder="Кончилась"
            {...register('name')}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          {errors.name && (
            <p className="mt-1 text-sm text-red-600">{errors.name.message}</p>
          )}
        </div>

        {/* Описание */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Описание
          </label>
          <textarea
            placeholder="Подробное описание проблемы..."
            {...register('description')}
            rows={4}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none"
          />
          {errors.description && (
            <p className="mt-1 text-sm text-red-600">{errors.description.message}</p>
          )}
        </div>

        {/* Критичность */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Критичность
          </label>
          <div className="space-y-2">
            {statusOptions.map((option) => (
              <label
                key={option.id}
                className={`
                  flex items-start p-3 rounded-lg border-2 cursor-pointer transition-all
                  ${selectedStatusId === option.id 
                    ? `${option.color} border-current` 
                    : 'border-gray-200 hover:border-gray-300'
                  }
                `}
              >
                <input
                  type="radio"
                  value={option.id}
                  checked={selectedStatusId === option.id}
                  onChange={() => setValue('statusId', option.id)}
                  className="mt-0.5 mr-3"
                />
                <div className="flex-1">
                  <div className="font-medium">{option.name}</div>
                  <div className="text-sm text-gray-500">{option.description}</div>
                </div>
              </label>
            ))}
          </div>
          {errors.statusId && (
            <p className="mt-1 text-sm text-red-600">{errors.statusId.message}</p>
          )}
        </div>

        {/* Кнопки */}
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
