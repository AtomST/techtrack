'use client';

import { useState } from 'react';
import { Modal } from '@/components/ui/Modal';
import { Input } from '@/components/ui/Input';
import { Button } from '@/components/ui/Button';
import { companyService } from '@/services/api/companies';
import toast from 'react-hot-toast';

interface AddEmployeeModalProps {
  isOpen: boolean;
  onClose: () => void;
  companyId: string;
}

export function AddEmployeeModal({ isOpen, onClose, companyId }: AddEmployeeModalProps) {
  const [email, setEmail] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async () => {
    if (!email.trim()) {
      toast.error('Введите email сотрудника');
      return;
    }

    setLoading(true);
    try {
      await companyService.addEmployee(companyId, email);
      toast.success('Сотрудник добавлен в компанию');
      setEmail('');
      onClose();
    } catch (error: any) {
      toast.error(error.message || 'Ошибка добавления сотрудника');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Добавление сотрудника" size="md">
      <div className="space-y-6">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Email сотрудника
          </label>
          <input
            type="email"
            placeholder="employee@company.com"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <p className="text-xs text-gray-500 mt-1">
            Пользователь с таким email должен быть зарегистрирован в системе
          </p>
        </div>

        <div className="flex justify-end space-x-3 pt-4 border-t border-gray-100">
          <Button variant="outline" onClick={onClose}>
            Отмена
          </Button>
          <Button onClick={handleSubmit} loading={loading}>
            Добавить
          </Button>
        </div>
      </div>
    </Modal>
  );
}
