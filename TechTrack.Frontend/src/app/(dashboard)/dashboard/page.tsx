'use client';

import { useState, useEffect } from 'react';
import { DepartmentNav } from '@/components/layout/DepartmentNav';
import { IssueList } from '@/components/equipment/IssueList';
import { MaintenanceLog } from '@/components/equipment/MaintenanceLog';
import { useCompanyStore } from '@/store/companyStore';
import { useEquipmentStore } from '@/store/equipmentStore';
import { Equipment } from '@/types';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/Tabs';

export default function DashboardPage() {
  const { departments, fetchDepartments, selectedDepartment, setSelectedDepartment } = useCompanyStore();
  const { selectedEquipment, setSelectedEquipment, fetchIssues, fetchMaintenances } = useEquipmentStore();
  const [activeTab, setActiveTab] = useState('issues');

  useEffect(() => {
    fetchDepartments();
  }, []);

  useEffect(() => {
    if (selectedEquipment) {
      fetchIssues(selectedEquipment.id);
      fetchMaintenances(selectedEquipment.id);
    }
  }, [selectedEquipment]);

  const handleEquipmentSelect = (equipment: Equipment) => {
    setSelectedEquipment(equipment);
  };

  return (
    <div className="flex h-[calc(100vh-64px)]">
      <DepartmentNav
        departments={departments}
        selectedDepartment={selectedDepartment}
        onSelectDepartment={setSelectedDepartment}
        onSelectEquipment={handleEquipmentSelect}
      />

      <div className="flex-1 overflow-auto">
        {selectedEquipment ? (
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
                <IssueList equipmentId={selectedEquipment.id} />
              </TabsContent>

              <TabsContent value="maintenance">
                <MaintenanceLog equipmentId={selectedEquipment.id} />
              </TabsContent>
            </Tabs>
          </div>
        ) : (
          <div className="flex items-center justify-center h-full">
            <div className="text-center">
              <svg className="mx-auto h-16 w-16 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
              <h3 className="mt-4 text-lg font-medium text-gray-900">Выберите технику</h3>
              <p className="mt-1 text-gray-500">Выберите отдел и технику из списка слева</p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
