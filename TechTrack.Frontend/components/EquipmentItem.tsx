// src/components/sidebar/EquipmentItem.tsx
import { Equipment } from "@/types/equipment";
import { StatusBadge } from "./common/StatusBadge";

export function EquipmentItem({
  equipment,
  onClick,
}: {
  equipment: Equipment;
  onClick: () => void;
}) {
  return (
    <div
      className="ml-4 flex justify-between cursor-pointer hover:bg-gray-100 p-1 rounded"
      onClick={onClick}
    >
      <span>{equipment.name}</span>
      <StatusBadge status={equipment.status} />
    </div>
  );
}
