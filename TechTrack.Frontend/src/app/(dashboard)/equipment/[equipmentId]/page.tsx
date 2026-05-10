'use client';

import { useState, useEffect } from 'react';
import { useParams } from 'next/navigation';
import { IssueList } from '@/components/equipment/IssueList';
import { MaintenanceLog } from '@/components/equipment/MaintenanceLog';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/Tabs';
import { useEquipmentStore } from '@/store/equipmentStore';

export default function EquipmentDetailPage() {
  const params = useParams();
  const equipmentId = params.equipmentId as string;
  const { selectedEquipment, setSelectedEquipment, fetchIssues, fetchMaintenances } = useEquipmentStore();
  const [activeTab, setActiveTab] = useState('issues');

  useEffect(() => {
    if (equipmentId) {
      fetchIssues(equipmentId);
      fetchMaintenances(equipmentId);
    }
  }, [equipmentId]);

  if (!selectedEquipment) {
    return <div className="p-8">Загрузка...</div>;
  }

  return (
    <div className="p-8">
      <div className="bg-white rounded-lg shadow-sm mb-6 p-6">
        <h1 className="text-2xl font-bold text-gray-900">{selectedEquipment.name}</h1>
        <p className="text-gray-600 mt-1">{selectedEquipment.description}</p>
      </div>

      <Tabs value={activeTab} onValueChange={setActiveTab}>
        <TabsList>
          <TabsTrigger value="issues">Текущие неисправности</TabsTrigger>
          <TabsTrigger value="maintenance">Журнал ТО</TabsTrigger>
        </TabsList>

        <TabsContent value="issues">
          <IssueList equipmentId={equipmentId} />
        </TabsContent>

        <TabsContent value="maintenance">
          <MaintenanceLog equipmentId={equipmentId} />
        </TabsContent>
      </Tabs>
    </div>
  );
}
