'use client';

import { useState, useEffect } from 'react';
import { Issue } from '@/types';
import { useEquipmentStore } from '@/store/equipmentStore';
import { Button } from '@/components/ui/Button';
import { CreateIssueModal } from '@/components/modals/CreateIssueModal';
import { AlertTriangle, CheckCircle, Info, Wrench } from 'lucide-react';

interface IssueListProps {
  equipmentId: string;
}

const getIssueStatus = (issue: Issue) => {
  if (issue.resolvedByMaintenanceId) {
    return { 
      label: 'Решена (ТО)', 
      color: 'bg-purple-100 text-purple-800 border-purple-200', 
      icon: <Wrench className="w-5 h-5 text-purple-500" />,
      order: 4
    };
  }
  if (issue.isResolved) {
    return { 
      label: 'Решена', 
      color: 'bg-green-100 text-green-800 border-green-200', 
      icon: <CheckCircle className="w-5 h-5 text-green-500" />,
      order: 3
    };
  }
  switch (issue.statusId) {
    case 1:
      return { 
        label: 'Критическая', 
        color: 'bg-red-100 text-red-800 border-red-200', 
        icon: <AlertTriangle className="w-5 h-5 text-red-500" />,
        order: 0
      };
    case 2:
      return { 
        label: 'Средняя', 
        color: 'bg-yellow-100 text-yellow-800 border-yellow-200', 
        icon: <AlertTriangle className="w-5 h-5 text-yellow-500" />,
        order: 1
      };
    case 3:
      return { 
        label: 'Низкая', 
        color: 'bg-blue-100 text-blue-800 border-blue-200', 
        icon: <Info className="w-5 h-5 text-blue-500" />,
        order: 2
      };
    default:
      return { 
        label: 'Неизвестно', 
        color: 'bg-gray-100 text-gray-800 border-gray-200', 
        icon: <Info className="w-5 h-5 text-gray-500" />,
        order: 5
      };
  }
};

export function IssueList({ equipmentId }: IssueListProps) {
  const { issues, fetchIssues, loadingIssues } = useEquipmentStore();
  const [showCreateIssue, setShowCreateIssue] = useState(false);
  const [hasLoaded, setHasLoaded] = useState(false);

  useEffect(() => {
    if (!hasLoaded && equipmentId) {
      fetchIssues(equipmentId);
      setHasLoaded(true);
    }
  }, [equipmentId, hasLoaded, fetchIssues]);

  // Сортируем неисправности: сначала активные, потом решенные
  const sortedIssues = [...issues].sort((a, b) => {
    const statusA = getIssueStatus(a);
    const statusB = getIssueStatus(b);
    return statusA.order - statusB.order;
  });

  if (loadingIssues) {
    return <div className="text-center py-8">Загрузка...</div>;
  }

  return (
    <div className="bg-white rounded-lg shadow-sm">
      <div className="border-b border-gray-200 px-6 py-4 flex justify-between items-center">
        <h2 className="text-xl font-semibold text-gray-900">Неисправности</h2>
        <Button onClick={() => setShowCreateIssue(true)}>Добавить неисправность</Button>
      </div>

      <div className="p-6">
        {sortedIssues.length === 0 ? (
          <div className="text-center py-8 text-gray-500">
            Нет зарегистрированных неисправностей
          </div>
        ) : (
          <div className="space-y-3">
            {sortedIssues.map((issue) => {
              const status = getIssueStatus(issue);
              return (
                <div key={issue.id} className={`border rounded-lg p-4 ${status.color} border-opacity-50`}>
                  <div className="flex items-start justify-between">
                    <div className="flex items-start space-x-3 flex-1">
                      {status.icon}
                      <div className="flex-1">
                        <div className="flex items-center gap-2 flex-wrap">
                          <h3 className="font-medium text-gray-900">{issue.name}</h3>
                          <span className={`text-xs px-2 py-0.5 rounded-full ${status.color} border`}>
                            {status.label}
                          </span>
                        </div>
                        {issue.description && (
                          <p className="text-sm text-gray-600 mt-1">{issue.description}</p>
                        )}
                        <p className="text-xs text-gray-400 mt-2">
                          Создано: {new Date(issue.createdAt).toLocaleString()}
                        </p>
                        {issue.resolvedByMaintenanceId && (
                          <p className="text-xs text-purple-600 mt-1">
                            Решена через техническое обслуживание
                          </p>
                        )}
                      </div>
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>

      <CreateIssueModal
        isOpen={showCreateIssue}
        onClose={() => setShowCreateIssue(false)}
        equipmentId={equipmentId}
      />
    </div>
  );
}
