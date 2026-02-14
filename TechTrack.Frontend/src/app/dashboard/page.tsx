'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { useQuery } from '@tanstack/react-query';
import { useAuthStore } from '@/store/auth.store';
import { departmentService } from '@/services/department.service';
import { equipmentService } from '@/services/equipment.service';
import { issueService } from '@/services/issue-maintenance.service';
import { Sidebar } from '@/components/sidebar';
import { EquipmentDetail } from '@/components/equipment-detail';
import { Equipment, Department } from '@/types';
import { Button } from '@/components/ui/button';
import { LogOut, Settings, Building2 } from 'lucide-react';

export default function DashboardPage() {
  const router = useRouter();
  const { user, isAuthenticated, isLoading: authLoading, logout } = useAuthStore();
  const [selectedEquipment, setSelectedEquipment] = useState<Equipment | null>(null);

  useEffect(() => {
    if (!authLoading && !isAuthenticated) {
      router.push('/login');
    }
  }, [isAuthenticated, authLoading, router]);

  // Fetch departments
  const { data: departmentsResponse, isLoading: departmentsLoading } = useQuery({
    queryKey: ['departments', user?.companyId],
    queryFn: () => departmentService.getDepartments(user?.companyId),
    enabled: !!user?.companyId,
  });

  const departments = departmentsResponse || [];

  // Fetch equipment for all departments
  const { data: equipmentByDepartment = {}, isLoading: equipmentLoading } = useQuery({
    queryKey: ['equipment', user?.companyId, departments],
    queryFn: async () => {
      if (!user?.companyId || !departments.length) return {};
      
      const result: Record<string, Equipment[]> = {};
      
      for (const dept of departments) {
        // Equipment уже есть в Department.equipments
        result[dept.id] = dept.equipments || [];
      }
      
      return result;
    },
    enabled: !!user?.companyId && departments.length > 0,
  });

  // Fetch issues for selected equipment
  const { data: issuesResponse } = useQuery({
    queryKey: ['issues', selectedEquipment?.id],
    queryFn: () => issueService.getIssues(selectedEquipment!.id),
    enabled: !!selectedEquipment,
  });

  const issues = issuesResponse || [];

  const handleLogout = async () => {
    await logout();
    router.push('/login');
  };

  if (authLoading || !isAuthenticated) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary mx-auto"></div>
          <p className="mt-4 text-muted-foreground">Загрузка...</p>
        </div>
      </div>
    );
  }

  if (!user?.companyId) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center max-w-md">
          <Building2 className="h-16 w-16 mx-auto text-muted-foreground mb-4" />
          <h2 className="text-2xl font-bold mb-2">Компания не найдена</h2>
          <p className="text-muted-foreground mb-6">
            Вы не привязаны ни к одной компании. Обратитесь к администратору.
          </p>
          <Button onClick={handleLogout}>
            Выйти
          </Button>
        </div>
      </div>
    );
  }

  return (
    <div className="h-screen flex flex-col">
      {/* Header */}
      <header className="border-b bg-card">
        <div className="flex items-center justify-between px-6 py-4">
          <div>
            <h1 className="text-2xl font-bold">TechTrack</h1>
            <p className="text-sm text-muted-foreground">
              {user.fullName || user.email}
            </p>
          </div>
          <div className="flex items-center gap-2">
            <Button variant="ghost" size="icon">
              <Settings className="h-5 w-5" />
            </Button>
            <Button variant="ghost" size="icon" onClick={handleLogout}>
              <LogOut className="h-5 w-5" />
            </Button>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <div className="flex-1 flex overflow-hidden">
        <Sidebar
          departments={departments}
          equipmentByDepartment={equipmentByDepartment}
          selectedEquipment={selectedEquipment}
          onEquipmentSelect={setSelectedEquipment}
        />

        {selectedEquipment ? (
          <EquipmentDetail
            equipment={selectedEquipment}
            issues={issues}
            onViewAllIssues={() => router.push(`/equipment/${selectedEquipment.id}/issues`)}
            onViewMaintenance={() => router.push(`/equipment/${selectedEquipment.id}/maintenance`)}
            onAddIssue={() => router.push(`/equipment/${selectedEquipment.id}/issues/new`)}
          />
        ) : (
          <div className="flex-1 flex items-center justify-center text-muted-foreground">
            <div className="text-center">
              <Building2 className="h-16 w-16 mx-auto mb-4 opacity-50" />
              <p>Выберите технику для просмотра деталей</p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
