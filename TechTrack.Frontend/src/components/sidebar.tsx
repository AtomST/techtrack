'use client';

import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { ChevronDown, ChevronRight, Plus, Monitor, Loader2 } from 'lucide-react';
import { Department, Equipment } from '@/types';
import { equipmentService } from '@/services/equipment.service';
import { useAuth } from '@/hooks/useAuth';
import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { getEquipmentStatusColor, getEquipmentStatusName } from '@/lib/utils';

interface SidebarProps {
  departments: Department[];
  selectedEquipment: Equipment | null;
  onEquipmentSelect: (equipment: Equipment) => void;
  onAddDepartment?: () => void;
  onAddEquipment?: (departmentId: string) => void;
}

export function Sidebar({
  departments,
  selectedEquipment,
  onEquipmentSelect,
  onAddDepartment,
  onAddEquipment,
}: SidebarProps) {
  const [expandedDepartments, setExpandedDepartments] = useState<Set<string>>(new Set());

  const toggleDepartment = (departmentId: string) => {
    setExpandedDepartments((prev) => {
      const next = new Set(prev);
      if (next.has(departmentId)) {
        next.delete(departmentId);
      } else {
        next.add(departmentId);
      }
      return next;
    });
  };

  return (
    <div className="w-80 border-r bg-card h-full flex flex-col">
      <div className="p-4 border-b">
        <div className="flex items-center justify-between">
          <h2 className="text-lg font-semibold">Отделы</h2>
          {onAddDepartment && (
            <Button size="sm" variant="ghost" onClick={onAddDepartment}>
              <Plus className="h-4 w-4" />
            </Button>
          )}
        </div>
      </div>

      <div className="flex-1 overflow-y-auto">
        {departments.length === 0 ? (
          <div className="p-4 text-center text-muted-foreground">
            Нет отделов
          </div>
        ) : (
          departments.map((department) => (
            <DepartmentItem
              key={department.id}
              department={department}
              isExpanded={expandedDepartments.has(department.id)}
              onToggle={() => toggleDepartment(department.id)}
              selectedEquipment={selectedEquipment}
              onEquipmentSelect={onEquipmentSelect}
              onAddEquipment={onAddEquipment}
            />
          ))
        )}
      </div>
    </div>
  );
}

interface DepartmentItemProps {
  department: Department;
  isExpanded: boolean;
  onToggle: () => void;
  selectedEquipment: Equipment | null;
  onEquipmentSelect: (equipment: Equipment) => void;
  onAddEquipment?: (departmentId: string) => void;
}

function DepartmentItem({
  department,
  isExpanded,
  onToggle,
  selectedEquipment,
  onEquipmentSelect,
  onAddEquipment,
}: DepartmentItemProps) {
  const { user } = useAuth();

  // Lazy load equipment when department is expanded
  const { data: equipment = [], isLoading } = useQuery({
    queryKey: ['equipment', user?.companyId, department.id],
    queryFn: () => equipmentService.getEquipment(user!.companyId!, department.id),
    enabled: isExpanded && !!user?.companyId,
  });

  return (
    <div className="border-b">
      <button
        onClick={onToggle}
        className="w-full px-4 py-3 flex items-center justify-between hover:bg-accent transition-colors"
      >
        <div className="flex items-center gap-2">
          {isExpanded ? (
            <ChevronDown className="h-4 w-4" />
          ) : (
            <ChevronRight className="h-4 w-4" />
          )}
          <span className="font-medium">{department.name}</span>
          {isLoading && isExpanded && (
            <Loader2 className="h-3 w-3 animate-spin text-muted-foreground" />
          )}
        </div>
        <div className="flex items-center gap-2">
          {!isLoading && isExpanded && (
            <Badge variant="secondary" className="text-xs">
              {equipment.length}
            </Badge>
          )}
          {onAddEquipment && (
            <Button
              size="sm"
              variant="ghost"
              onClick={(e) => {
                e.stopPropagation();
                onAddEquipment(department.id);
              }}
            >
              <Plus className="h-3 w-3" />
            </Button>
          )}
        </div>
      </button>

      {isExpanded && (
        <div className="bg-muted/50">
          {isLoading ? (
            <div className="px-4 py-2 text-sm text-muted-foreground flex items-center gap-2">
              <Loader2 className="h-4 w-4 animate-spin" />
              Загрузка техники...
            </div>
          ) : equipment.length === 0 ? (
            <div className="px-4 py-2 text-sm text-muted-foreground">
              Нет техники
            </div>
          ) : (
            equipment.map((item) => (
              <button
                key={item.id}
                onClick={() => onEquipmentSelect(item)}
                className={cn(
                  'w-full px-8 py-2 text-left hover:bg-accent transition-colors flex items-center justify-between',
                  selectedEquipment?.id === item.id && 'bg-accent'
                )}
              >
                <div className="flex items-center gap-2 flex-1 min-w-0">
                  <Monitor className="h-4 w-4 text-muted-foreground flex-shrink-0" />
                  <span className="text-sm truncate">{item.name}</span>
                </div>
                <Badge
                  className={cn(
                    'text-xs flex-shrink-0 ml-2',
                    getEquipmentStatusColor(item.currentStatusId)
                  )}
                >
                  {item.equipmentStatus?.name || getEquipmentStatusName(item.currentStatusId)}
                </Badge>
              </button>
            ))
          )}
        </div>
      )}
    </div>
  );
}
