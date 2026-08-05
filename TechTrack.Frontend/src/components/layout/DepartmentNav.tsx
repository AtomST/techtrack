'use client';

import { useState, useEffect } from 'react';
import { ChevronDown, ChevronRight, Plus } from 'lucide-react';
import { Department, Equipment } from '@/types';
import { Button } from '@/components/ui/Button';
import { StatusBadge } from '@/components/ui/StatusBadge';
import { CreateDepartmentModal } from '@/components/modals/CreateDepartmentModal';
import { CreateEquipmentModal } from '@/components/modals/CreateEquipmentModal';
import { useDepartmentStore } from '@/store/departmentStore';
import { departmentService } from '@/services/api/departments';

interface DepartmentNavProps {
  departments: Department[];
  selectedDepartment: Department | null;
  onSelectDepartment: (department: Department) => void;
  onSelectEquipment: (equipment: Equipment) => void;
}

const getRoleFromToken = (): string | null => {
  if (typeof window === 'undefined') return null;
  const token = localStorage.getItem('accessToken');
  if (!token) return null;
  
  try {
    const parts = token.split('.');
    if (parts.length === 3) {
      const payload = JSON.parse(atob(parts[1]));
      return payload.role;
    }
  } catch (e) {
    console.error('Failed to parse token:', e);
  }
  return null;
};

export function DepartmentNav({
  departments,
  selectedDepartment,
  onSelectDepartment,
  onSelectEquipment,
}: DepartmentNavProps) {
  const [expandedDepartments, setExpandedDepartments] = useState<Set<string>>(new Set());
  const [equipmentsMap, setEquipmentsMap] = useState<Record<string, Equipment[]>>({});
  const [loadingEquipments, setLoadingEquipments] = useState<Record<string, boolean>>({});
  const [showCreateDepartment, setShowCreateDepartment] = useState(false);
  const [showCreateEquipment, setShowCreateEquipment] = useState(false);
  const [selectedDeptForEquipment, setSelectedDeptForEquipment] = useState<Department | null>(null);

  const role = getRoleFromToken();
  const canCreateDepartment = role === 'Admin' || role === 'CompanyHead' || role === 'PlatformAdmin';
  const canCreateEquipment = role === 'Admin' || role === 'CompanyHead' || role === 'PlatformAdmin' ||
                            role === 'DepartmentHead' || role === 'Manager';

  const loadEquipments = async (departmentId: string) => {
    if (equipmentsMap[departmentId]) return;
    
    setLoadingEquipments(prev => ({ ...prev, [departmentId]: true }));
    try {
      const department = await departmentService.getDepartment(departmentId);
      const equipments = department.equipments || [];
      setEquipmentsMap(prev => ({ ...prev, [departmentId]: equipments }));
    } catch (error) {
      console.error('Failed to load equipments:', error);
    } finally {
      setLoadingEquipments(prev => ({ ...prev, [departmentId]: false }));
    }
  };

  const toggleDepartment = async (deptId: string) => {
    const newExpanded = new Set(expandedDepartments);
    if (newExpanded.has(deptId)) {
      newExpanded.delete(deptId);
    } else {
      newExpanded.add(deptId);
      await loadEquipments(deptId);
    }
    setExpandedDepartments(newExpanded);
  };

  useEffect(() => {
    if (selectedDepartment && !expandedDepartments.has(selectedDepartment.id)) {
      setExpandedDepartments(prev => new Set([...prev, selectedDepartment.id]));
      loadEquipments(selectedDepartment.id);
    }
  }, [selectedDepartment]);

  return (
    <>
      <div className="w-80 bg-white border-r border-gray-200 flex flex-col">
        <div className="p-4 border-b border-gray-200">
          <h2 className="text-lg font-semibold text-gray-900">Отделы</h2>
          {canCreateDepartment && (
            <Button
              size="sm"
              variant="outline"
              className="mt-2 w-full"
              onClick={() => setShowCreateDepartment(true)}
            >
              <Plus className="w-4 h-4 mr-2" />
              Создать отдел
            </Button>
          )}
        </div>

        <div className="flex-1 overflow-y-auto">
          {departments.length === 0 ? (
            <div className="p-4 text-center text-gray-500 text-sm">
              Нет доступных отделов
            </div>
          ) : (
            departments.map((dept) => (
              <div key={dept.id} className="border-b border-gray-100">
                <div
                  className={`flex items-center justify-between p-3 cursor-pointer hover:bg-gray-50 ${
                    selectedDepartment?.id === dept.id ? 'bg-blue-50' : ''
                  }`}
                  onClick={() => {
                    onSelectDepartment(dept);
                    toggleDepartment(dept.id);
                  }}
                >
                  <div className="flex items-center flex-1">
                    {expandedDepartments.has(dept.id) ? (
                      <ChevronDown className="w-4 h-4 mr-2 text-gray-400" />
                    ) : (
                      <ChevronRight className="w-4 h-4 mr-2 text-gray-400" />
                    )}
                    <span className="text-sm font-medium">{dept.name}</span>
                  </div>
                  {canCreateEquipment && (
                    <Button
                      size="sm"
                      variant="ghost"
                      onClick={(e) => {
                        e.stopPropagation();
                        setSelectedDeptForEquipment(dept);
                        setShowCreateEquipment(true);
                      }}
                    >
                      <Plus className="w-3 h-3" />
                    </Button>
                  )}
                </div>

                {expandedDepartments.has(dept.id) && (
                  <div className="ml-6 pb-2">
                    {loadingEquipments[dept.id] ? (
                      <div className="p-2 text-sm text-gray-400">Загрузка...</div>
                    ) : equipmentsMap[dept.id]?.length === 0 ? (
                      <div className="p-2 text-sm text-gray-400">Нет оборудования</div>
                    ) : (
                      equipmentsMap[dept.id]?.map((equipment) => (
                        <div
                          key={equipment.id}
                          className="p-2 text-sm text-gray-600 hover:bg-gray-50 cursor-pointer rounded flex items-center justify-between"
                          onClick={() => onSelectEquipment(equipment)}
                        >
                          <div className="flex items-center gap-2 flex-1 min-w-0">
                            <StatusBadge statusId={equipment.currentStatusId} size="sm" />
                            <span className="truncate">{equipment.name}</span>
                            {equipment.serialNumber && (
                              <span className="text-xs text-gray-400 flex-shrink-0">
                                #{equipment.serialNumber}
                              </span>
                            )}
                          </div>
                        </div>
                      ))
                    )}
                  </div>
                )}
              </div>
            ))
          )}
        </div>
      </div>

      {canCreateDepartment && (
        <CreateDepartmentModal
          isOpen={showCreateDepartment}
          onClose={() => setShowCreateDepartment(false)}
        />
      )}

      {canCreateEquipment && selectedDeptForEquipment && (
        <CreateEquipmentModal
          isOpen={showCreateEquipment}
          onClose={() => {
            setShowCreateEquipment(false);
            setSelectedDeptForEquipment(null);
          }}
          departmentId={selectedDeptForEquipment.id}
        />
      )}
    </>
  );
}
