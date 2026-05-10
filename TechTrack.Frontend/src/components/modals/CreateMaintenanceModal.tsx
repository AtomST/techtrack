'use client';

import { useState, useEffect, useCallback } from 'react';
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
  solvedIssuesId: z.array(z.string()).optional(),
});

type FormData = z.infer<typeof schema>;

interface CreateMaintenanceModalProps {
  isOpen: boolean;
  onClose: () => void;
  equipmentId: string;
}

export function CreateMaintenanceModal({ isOpen, onClose, equipmentId }: CreateMaintenanceModalProps) {
  const [loading, setLoading] = useState(false);
  const { issues, fetchIssues, fetchMaintenances } = useEquipmentStore();
  const [selectedIssues, setSelectedIssues] = useState<string[]>([]);
  const [isLoadingIssues, setIsLoadingIssues] = useState(false);

  // Загружаем неисправности только при открытии модального окна
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
    }
  }, [isOpen, equipmentId]); // Убираем fetchIssues из зависимостей

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  const toggleIssue = (issueId: string) => {
    setSelectedIssues(prev =>
      prev.includes(issueId)
        ? prev.filter(id => id !== issueId)
        : [...prev, issueId]
    );
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

  const unresolvedIssues = issues.filter(i => i.statusId !== 4);

  return (
    <Modal isOpen={isOpen} onClose={handleClose} title="Внеплановое ТО" size="md">
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Название ТО
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
