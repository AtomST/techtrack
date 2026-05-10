'use client';

import { Equipment } from '@/types';
import { useRouter } from 'next/navigation';
import { Wrench, AlertTriangle } from 'lucide-react';

interface EquipmentCardProps {
  equipment: Equipment;
  issuesCount?: number;
}

export function EquipmentCard({ equipment, issuesCount = 0 }: EquipmentCardProps) {
  const router = useRouter();

  return (
    <div
      className="bg-white rounded-lg shadow-sm border border-gray-200 p-4 hover:shadow-md transition-shadow cursor-pointer"
      onClick={() => router.push(`/equipment/${equipment.id}`)}
    >
      <div className="flex items-start justify-between">
        <div className="flex-1">
          <h3 className="font-semibold text-gray-900">{equipment.name}</h3>
          {equipment.description && (
            <p className="text-sm text-gray-500 mt-1">{equipment.description}</p>
          )}
        </div>
        {issuesCount > 0 && (
          <div className="flex items-center text-red-500 text-sm">
            <AlertTriangle className="w-4 h-4 mr-1" />
            {issuesCount}
          </div>
        )}
      </div>
      <div className="flex items-center justify-between mt-3 pt-3 border-t border-gray-100">
        <div className="flex items-center text-xs text-gray-400">
          <Wrench className="w-3 h-3 mr-1" />
          ID: {equipment.id.slice(0, 8)}...
        </div>
      </div>
    </div>
  );
}
