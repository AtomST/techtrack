// src/components/equipment/EquipmentJournal.tsx
import { getJournal } from "@/services/mock.api";

export async function EquipmentJournal({
  equipmentId,
}: {
  equipmentId: string;
}) {
  const entries = await getJournal(equipmentId);

  return (
    <div>
      <h2 className="text-lg font-bold mb-4">Журнал ТО</h2>
      <ul>
        {entries.map((e) => (
          <li key={e.date} className="mb-2 border-b pb-2">
            <div>{e.date}</div>
            <div className="text-sm text-gray-600">
              {e.description}
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}
