'use client';

import { useState } from 'react';
import { ChevronDown, ChevronRight, Plus, Monitor } from 'lucide-react';
import { Department, Equipment } from '@/types';
import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';

interface SidebarProps {
  departments: Department[];
  equipmentByDepartment: Record<string, Equipment[]>;
  selectedEquipment: Equipment | null;
  onEquipmentSelect: (equipment: Equipment) => void;
  onAddDepartment?: () => void;
  onAddEquipment?: (departmentId: string) => void;
}

export function Sidebar({
  departments,
  equipmentByDepartment,
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
          departments.map((department) => {
            const isExpanded = expandedDepartments.has(department.id);
            const equipment = equipmentByDepartment[department.id] || [];

            return (
              <div key={department.id} className="border-b">
                <button
                  onClick={() => toggleDepartment(department.id)}
                  className="w-full px-4 py-3 flex items-center justify-between hover:bg-accent transition-colors"
                >
                  <div className="flex items-center gap-2">
                    {isExpanded ? (
                      <ChevronDown className="h-4 w-4" />
                    ) : (
                      <ChevronRight className="h-4 w-4" />
                    )}
                    <span className="font-medium">{department.name}</span>
                  </div>
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
                </button>

                {isExpanded && (
                  <div className="bg-muted/50">
                    {equipment.length === 0 ? (
                      <div className="px-4 py-2 text-sm text-muted-foreground">
                        Нет техники
                      </div>
                    ) : (
                      equipment.map((item) => (
                        <button
                          key={item.id}
                          onClick={() => onEquipmentSelect(item)}
                          className={cn(
                            'w-full px-8 py-2 text-left hover:bg-accent transition-colors flex items-center gap-2',
                            selectedEquipment?.id === item.id && 'bg-accent'
                          )}
                        >
                          <Monitor className="h-4 w-4 text-muted-foreground" />
                          <span className="text-sm">{item.name}</span>
                        </button>
                      ))
                    )}
                  </div>
                )}
              </div>
            );
          })
        )}
      </div>
    </div>
  );
}
