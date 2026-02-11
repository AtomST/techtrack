// src/components/sidebar/Sidebar.tsx
"use client";

import { getCompanyStructure } from "@/services/mock.api";
import { useEffect, useState } from "react";
import { DepartmentItem } from "./DepartmentItem";
import { Department } from "@/types/department";

export function Sidebar({
  onSelectEquipment,
}: {
  onSelectEquipment: (id: string) => void;
}) {
  const [departments, setDepartments] = useState<Department[]>([]);

  useEffect(() => {
    getCompanyStructure().then(setDepartments);
  }, []);

  return (
    <div className="w-72 border-r p-4">
      <h2 className="font-bold mb-4">Отделы</h2>
      {departments.map((d) => (
        <DepartmentItem
          key={d.id}
          department={d}
          onSelectEquipment={onSelectEquipment}
        />
      ))}
    </div>
  );
}
