'use client';

import { useState, useEffect } from 'react';
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
  maintenanceTypeId: z.number().min(1, 'Выберите тип ТО'),
  solvedIssuesId: z.array(z.string()).optional(),
});

type FormData = z.infer<typeof schema>;

interface CreateMaintenanceModalProps {
  isOpen: boolean;
  onClose: () => void;
  equipmentId: string;
}

const maintenanceTypes = [
  { id: 1, name: 'Профилактика', color: 'bg-blue-100 text-blue-800', description: 'Плановое обслуживание' },
  { id: 2, name: 'Ремонт', color: 'bg-orange-100 text-orange-800', description: 'Восстановление работоспособности' },
  { id: 3, name: 'Модернизация', color: 'bg-purple-100 text-purple-800', description: 'Улучшение характеристик' },
];

export function CreateMaintenanceModal({ isOpen, onClose, equipmentId }: CreateMaintenanceModalProps) {
  const [loading, setLoading] = useState(false);
  const { issues, fetchIssues, fetchMaintenances } = useEquipmentStore();
  const [selectedIssues, setSelectedIssues] = useState<string[]>([]);
  const [isLoadingIssues, setIsLoadingIssues] = useState(false);

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
      maintenanceTypeId: 1,
    },
  });

  useEffect(() => {
    if (isOpen && equipmentId) {
      const loadIssues = async () => {
        setIsLoadingIssues(true);
        try {
          await fetchIssues(equipmentId);
        } catch (error) {
          console.error('Failed to load issues:', error);
        } finally {
          setIsLoadingIssues(false);
        }
      };
      loadIssues();
      reset();
      setSelectedIssues([]);
    }
  }, [isOpen, equipmentId]);

  const selectedTypeId = watch('maintenanceTypeId');

  const toggleIssue = (issueId: string) => {
    setSelectedIssues(prev =>
      prev.includes(issueId)
        ? prev.filter(id => id !== issueId)
        : [...prev, issueId]
    );
    setValue('solvedIssuesId', selectedIssues);
  };

  const onSubmit = async (data: FormData) => {
    setLoading(true);
    try {
      await equipmentService.createMaintenance(equipmentId, {
        name: data.name,
        description: data.description,
        solvedIssuesId: selectedIssues,
      });
      toast.success('Внеплановое ТО добавлено');
      await fetchMaintenances(equipmentId);
      reset();
      setSelectedIssues([]);
      onClose();
    } catch (error: any) {
      toast.error(error.message || 'Ошибка добавления ТО');
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    reset();
    setSelectedIssues([]);
    onClose();
  };

  // Фильтруем только НЕ решенные неисправности (statusId !== 4 и нет resolvedByMaintenanceId)
  const unresolvedIssues = issues.filter(issue => 
    issue.statusId !== 4 && !issue.resolvedByMaintenanceId
  );

  return (
    <Modal isOpen={isOpen} onClose={handleClose} title="Внеплановое ТО" size="md">
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Название ТО <span className="text-red-500">*</span>
          </label>
          <input
            type="text"
            placeholder="Замена картриджа"
            {...register('name')}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          {errors.name && (
            <p className="mt-1 text-sm text-red-600">{errors.name.message}</p>
          )}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Тип ТО <span className="text-red-500">*</span>
          </label>
          <div className="space-y-2">
            {maintenanceTypes.map((type) => (
              <label
                key={type.id}
                className={`
                  flex items-start p-3 rounded-lg border-2 cursor-pointer transition-all
                  ${selectedTypeId === type.id 
                    ? `${type.color} border-current` 
                    : 'border-gray-200 hover:border-gray-300'
                  }
                `}
              >
                <input
                  type="radio"
                  value={type.id}
                  checked={selectedTypeId === type.id}
                  onChange={() => setValue('maintenanceTypeId', type.id)}
                  className="mt-0.5 mr-3"
                />
                <div className="flex-1">
                  <div className="font-medium">{type.name}</div>
                  <div className="text-sm text-gray-500">{type.description}</div>
                </div>
              </label>
            ))}
          </div>
          {errors.maintenanceTypeId && (
            <p className="mt-1 text-sm text-red-600">{errors.maintenanceTypeId.message}</p>
          )}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Описание
          </label>
          <textarea
            placeholder="Подробное описание работ..."
            {...register('description')}
            rows={3}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none"
          />
        </div>

        {isLoadingIssues && (
          <div className="text-center py-4 text-gray-500">Загрузка неисправностей...</div>
        )}

        {!isLoadingIssues && unresolvedIssues.length > 0 && (
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Решаемые неисправности
            </label>
            <div className="space-y-2 max-h-48 overflow-y-auto border rounded-lg p-3">
              {unresolvedIssues.map((issue) => (
                <label
                  key={issue.id}
                  className="flex items-center p-2 rounded cursor-pointer hover:bg-gray-50"
                >
                  <input
                    type="checkbox"
                    checked={selectedIssues.includes(issue.id)}
                    onChange={() => toggleIssue(issue.id)}
                    className="mr-3"
                  />
                  <div className="flex-1">
                    <div className="text-sm font-medium">{issue.name}</div>
                    {issue.description && (
                      <div className="text-xs text-gray-500">{issue.description}</div>
                    )}
                    <div className="text-xs text-gray-400 mt-0.5">
                      Статус: {issue.statusId === 1 ? 'Критическая' : issue.statusId === 2 ? 'Средняя' : 'Низкая'}
                    </div>
                  </div>
                </label>
              ))}
            </div>
          </div>
        )}

        {!isLoadingIssues && unresolvedIssues.length === 0 && (
          <div className="text-sm text-gray-500 bg-gray-50 p-3 rounded-lg">
            Нет активных неисправностей для решения
          </div>
        )}

        <div className="flex justify-end space-x-3 pt-4 border-t border-gray-100">
          <Button type="button" variant="outline" onClick={handleClose}>
            Отмена
          </Button>
          <Button type="submit" loading={loading}>
            Добавить ТО
          </Button>
        </div>
      </form>
    </Modal>
  );
}
