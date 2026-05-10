'use client';

import { useState, useEffect } from 'react';
import { Maintenance } from '@/types';
import { useEquipmentStore } from '@/store/equipmentStore';
import { Button } from '@/components/ui/Button';
import { Modal } from '@/components/ui/Modal';
import { Input } from '@/components/ui/Input';
import { ScheduleMaintenanceModal } from '@/components/modals/ScheduleMaintenanceModal';
import { CreateMaintenanceModal } from '@/components/modals/CreateMaintenanceModal';
import { MaintenanceCalendar } from './MaintenanceCalendar';
import { Calendar, CheckCircle, Clock, Wrench, PlusCircle } from 'lucide-react';
import { formatDateTime, isOverdue } from '@/utils/dateUtils';
import toast from 'react-hot-toast';

const MAINTENANCE_STATUSES: Record<number, { label: string; color: string }> = {
  1: { label: 'Завершено', color: 'green' },
  2: { label: 'Отменено', color: 'gray' },
  3: { label: 'Запланировано', color: 'blue' },
  4: { label: 'Просрочено', color: 'red' },
};

const MAINTENANCE_TYPES: Record<number, { label: string; color: string }> = {
  1: { label: 'Профилактика', color: 'blue' },
  2: { label: 'Ремонт', color: 'orange' },
  3: { label: 'Модернизация', color: 'purple' },
};

interface MaintenanceLogProps {
  equipmentId: string;
}

export function MaintenanceLog({ equipmentId }: MaintenanceLogProps) {
  const { maintenances, fetchMaintenances, completeMaintenance, loading } = useEquipmentStore();
  const [showScheduleModal, setShowScheduleModal] = useState(false);
  const [showUnscheduledModal, setShowUnscheduledModal] = useState(false);
  const [showCompleteModal, setShowCompleteModal] = useState(false);
  const [selectedMaintenance, setSelectedMaintenance] = useState<Maintenance | null>(null);
  const [completionDescription, setCompletionDescription] = useState('');
  const [completionTypeId, setCompletionTypeId] = useState<number>(1);
  const [viewMode, setViewMode] = useState<'list' | 'calendar'>('list');

  useEffect(() => {
    fetchMaintenances(equipmentId);
  }, [equipmentId]);

  const handleComplete = async () => {
    if (!selectedMaintenance) return;
    
    try {
      await completeMaintenance(selectedMaintenance.id, {
        description: completionDescription,
        maintenanceTypeId: completionTypeId,
      });
      toast.success('ТО завершено');
      setShowCompleteModal(false);
      setSelectedMaintenance(null);
      setCompletionDescription('');
    } catch (error: any) {
      toast.error(error.message || 'Ошибка завершения ТО');
    }
  };

  const getStatusBadge = (statusId: number) => {
    const status = MAINTENANCE_STATUSES[statusId];
    if (!status) {
      return <span className="px-2 py-1 text-xs rounded-full bg-gray-100 text-gray-800">Неизвестно</span>;
    }
    const colors: Record<string, string> = {
      green: 'bg-green-100 text-green-800',
      gray: 'bg-gray-100 text-gray-800',
      blue: 'bg-blue-100 text-blue-800',
      red: 'bg-red-100 text-red-800',
    };
    return (
      <span className={`px-2 py-1 text-xs rounded-full ${colors[status.color]}`}>
        {status.label}
      </span>
    );
  };

  const getTypeBadge = (typeId?: number) => {
    if (!typeId) return null;
    const type = MAINTENANCE_TYPES[typeId];
    if (!type) return null;
    const colors: Record<string, string> = {
      blue: 'bg-blue-100 text-blue-800',
      orange: 'bg-orange-100 text-orange-800',
      purple: 'bg-purple-100 text-purple-800',
    };
    return (
      <span className={`px-2 py-1 text-xs rounded-full ${colors[type.color]}`}>
        {type.label}
      </span>
    );
  };

  if (loading && maintenances.length === 0) {
    return <div className="text-center py-8">Загрузка...</div>;
  }

  return (
    <div className="space-y-6">
      <div className="bg-white rounded-lg shadow-sm">
        <div className="border-b border-gray-200 px-6 py-4 flex justify-between items-center flex-wrap gap-2">
          <h2 className="text-xl font-semibold text-gray-900">Журнал ТО</h2>
          <div className="flex space-x-2">
            <Button
              variant={viewMode === 'list' ? 'primary' : 'outline'}
              size="sm"
              onClick={() => setViewMode('list')}
            >
              Список
            </Button>
            <Button
              variant={viewMode === 'calendar' ? 'primary' : 'outline'}
              size="sm"
              onClick={() => setViewMode('calendar')}
            >
              <Calendar className="w-4 h-4 mr-2" />
              Календарь
            </Button>
            <Button size="sm" variant="outline" onClick={() => setShowUnscheduledModal(true)}>
              <PlusCircle className="w-4 h-4 mr-2" />
              Внеплановое ТО
            </Button>
            <Button size="sm" onClick={() => setShowScheduleModal(true)}>
              <Calendar className="w-4 h-4 mr-2" />
              Запланировать ТО
            </Button>
          </div>
        </div>

        <div className="p-6">
          {viewMode === 'calendar' ? (
            <MaintenanceCalendar maintenances={maintenances} />
          ) : (
            <div className="space-y-3">
              {maintenances.length === 0 ? (
                <div className="text-center py-8 text-gray-500">
                  Нет записей о техническом обслуживании
                </div>
              ) : (
                maintenances.map((maintenance) => (
                  <div key={maintenance.id} className="border rounded-lg p-4 hover:shadow-md transition-shadow">
                    <div className="flex items-start justify-between flex-wrap gap-2">
                      <div className="flex-1">
                        <div className="flex items-center space-x-2 mb-2 flex-wrap gap-2">
                          <Wrench className="w-5 h-5 text-gray-500" />
                          <h3 className="font-medium text-gray-900">{maintenance.name}</h3>
                          {getStatusBadge(maintenance.maintenanceStatusId)}
                          {getTypeBadge(maintenance.maintenanceTypeId)}
                          {!maintenance.maintenanceScheduleRecordId && (
                            <span className="text-xs bg-purple-100 text-purple-800 px-2 py-0.5 rounded">
                              Внеплановое
                            </span>
                          )}
                        </div>
                        
                        {maintenance.description && (
                          <p className="text-sm text-gray-600 mt-1">{maintenance.description}</p>
                        )}
                        
                        <div className="flex items-center space-x-4 mt-2 text-xs text-gray-500 flex-wrap gap-2">
                          {maintenance.scheduledDate && (
                            <div className="flex items-center">
                              <Calendar className="w-3 h-3 mr-1" />
                              План: {formatDateTime(maintenance.scheduledDate)}
                            </div>
                          )}
                          {maintenance.completedAt && (
                            <div className="flex items-center">
                              <CheckCircle className="w-3 h-3 mr-1 text-green-500" />
                              Выполнено: {formatDateTime(maintenance.completedAt)}
                            </div>
                          )}
                          {maintenance.maintenanceStatusId === 3 && maintenance.scheduledDate && isOverdue(maintenance.scheduledDate) && (
                            <div className="flex items-center text-red-600">
                              <Clock className="w-3 h-3 mr-1" />
                              Просрочено
                            </div>
                          )}
                        </div>

                        {maintenance.solvedIssues && maintenance.solvedIssues.length > 0 && (
                          <div className="mt-3 pt-2 border-t border-gray-100">
                            <p className="text-xs text-gray-500">Решенные неисправности:</p>
                            <div className="flex flex-wrap gap-1 mt-1">
                              {maintenance.solvedIssues.map(issue => (
                                <span key={issue.id} className="text-xs bg-green-50 text-green-700 px-2 py-0.5 rounded">
                                  {issue.name}
                                </span>
                              ))}
                            </div>
                          </div>
                        )}
                      </div>
                      
                      {maintenance.maintenanceStatusId === 3 && (
                        <Button
                          size="sm"
                          variant="primary"
                          onClick={() => {
                            setSelectedMaintenance(maintenance);
                            setShowCompleteModal(true);
                          }}
                        >
                          Завершить
                        </Button>
                      )}
                    </div>
                  </div>
                ))
              )}
            </div>
          )}
        </div>
      </div>

      <ScheduleMaintenanceModal
        isOpen={showScheduleModal}
        onClose={() => setShowScheduleModal(false)}
        equipmentId={equipmentId}
      />

      <CreateMaintenanceModal
        isOpen={showUnscheduledModal}
        onClose={() => setShowUnscheduledModal(false)}
        equipmentId={equipmentId}
      />

      <Modal isOpen={showCompleteModal} onClose={() => setShowCompleteModal(false)} title="Завершение ТО">
        <div className="space-y-4">
          <p className="text-sm text-gray-600">
            ТО: <span className="font-medium">{selectedMaintenance?.name}</span>
          </p>
          
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Тип ТО
            </label>
            <select
              className="w-full rounded-lg border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={completionTypeId}
              onChange={(e) => setCompletionTypeId(Number(e.target.value))}
            >
              <option value={1}>Профилактика</option>
              <option value={2}>Ремонт</option>
              <option value={3}>Модернизация</option>
            </select>
          </div>
          
          <Input
            label="Описание выполнения"
            placeholder="Что было сделано..."
            value={completionDescription}
            onChange={(e) => setCompletionDescription(e.target.value)}
          />
          
          <div className="flex justify-end space-x-3 pt-4">
            <Button variant="outline" onClick={() => setShowCompleteModal(false)}>
              Отмена
            </Button>
            <Button onClick={handleComplete}>
              Завершить
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  );
}
