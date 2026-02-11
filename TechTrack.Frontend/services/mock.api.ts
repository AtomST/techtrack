// src/services/mock.api.ts
import { Department } from "@/types/department";
import { JournalEntry } from "@/types/journal";

export async function getCompanyStructure(): Promise<Department[]> {
  return [
    {
      id: "dep-1",
      name: "IT",
      equipments: [
        { id: "eq-1", name: "Server", status: "Green" },
        { id: "eq-2", name: "Router", status: "Yellow" },
      ],
    },
    {
      id: "dep-2",
      name: "Production",
      equipments: [
        { id: "eq-3", name: "CNC", status: "Red" },
      ],
    },
  ];
}

export async function getJournal(
  equipmentId: string
): Promise<JournalEntry[]> {
  return [
    {
      date: "2024-10-01",
      description: "Плановое обслуживание",
    },
    {
      date: "2024-10-15",
      description: "Замена комплектующих",
    },
  ];
}
