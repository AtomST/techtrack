'use client';

import { Equipment } from '@/types';
import { EquipmentCard } from './EquipmentCard';
import { useEquipmentStore } from '@/store/equipmentStore';

interface EquipmentListProps {
  equipments: Equipment[];
  issuesMap?: Record<string, number>;
}

export function EquipmentList({ equipments, issuesMap = {} }: EquipmentListProps) {
  if (equipments.length === 0) {
    return (
      <div className="text-center py-8 text-gray-500">
        Нет оборудования в этом отделе
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      {equipments.map((equipment) => (
        <EquipmentCard
          key={equipment.id}
          equipment={equipment}
          issuesCount={issuesMap[equipment.id] || 0}
        />
      ))}
    </div>
  );
}
