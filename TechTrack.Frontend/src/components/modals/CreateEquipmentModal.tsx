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
import { useCompanyStore } from '@/store/companyStore';
import toast from 'react-hot-toast';

const schema = z.object({
  name: z.string().min(2, 'Название должно содержать минимум 2 символа'),
  description: z.string().optional(),
});

type FormData = z.infer<typeof schema>;

interface CreateEquipmentModalProps {
  isOpen: boolean;
  onClose: () => void;
  departmentId: string;
}

export function CreateEquipmentModal({ isOpen, onClose, departmentId }: CreateEquipmentModalProps) {
  const [loading, setLoading] = useState(false);
  const { fetchDepartments } = useCompanyStore();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  const onSubmit = async (data: FormData) => {
    setLoading(true);
    try {
      await equipmentService.createEquipment(departmentId, data);
      toast.success('Оборудование добавлено успешно');
      await fetchDepartments();
      reset();
      onClose();
    } catch (error: any) {
      toast.error(error.message || 'Ошибка добавления оборудования');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Добавление оборудования">
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <Input
          label="Название оборудования"
          placeholder="Ноутбук Dell XPS"
          {...register('name')}
          error={errors.name?.message}
        />
        
        <Input
          label="Описание"
          placeholder="Описание оборудования..."
          {...register('description')}
          error={errors.description?.message}
        />
        
        <div className="flex justify-end space-x-3 pt-4">
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
