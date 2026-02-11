// src/app/dashboard/page.tsx
"use client";

import { Sidebar } from "@/components/Sidebar";
import { EquipmentJournal } from "@/components/EquipmentJournal";
import { useState } from "react";

export default function DashboardPage() {
  const [equipmentId, setEquipmentId] = useState<string | null>(null);

  return (
    <div className="flex h-screen">
      <Sidebar onSelectEquipment={setEquipmentId} />
      <div className="flex-1 p-6">
        {equipmentId ? (
          <EquipmentJournal equipmentId={equipmentId} />
        ) : (
          <div className="text-gray-500">
            Выберите оборудование
          </div>
        )}
      </div>
    </div>
  );
}
