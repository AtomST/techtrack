// src/components/sidebar/DepartmentItem.tsx
"use client";

import { useState } from "react";
import { Department } from "@/types/department";
import { EquipmentItem } from "./EquipmentItem";

export function DepartmentItem({
  department,
  onSelectEquipment,
}: {
  department: Department;
  onSelectEquipment: (id: string) => void;
}) {
  const [open, setOpen] = useState(false);

  return (
    <div className="mb-2">
      <div
        className="cursor-pointer font-medium"
        onClick={() => setOpen(!open)}
      >
        {department.name}
      </div>

      {open &&
        department.equipments.map((e) => (
          <EquipmentItem
            key={e.id}
            equipment={e}
            onClick={() => onSelectEquipment(e.id)}
          />
        ))}
    </div>
  );
}
