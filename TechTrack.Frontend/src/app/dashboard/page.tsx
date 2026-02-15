'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/hooks/useAuth';
import { departmentService } from '@/services/department.service';
import { issueService } from '@/services/issue-maintenance.service';
import { withAuth } from '@/components/withAuth';
import { Sidebar } from '@/components/sidebar';
import { EquipmentDetail } from '@/components/equipment-detail';
import { Equipment } from '@/types';
import { Button } from '@/components/ui/button';
import { LogOut, Settings, Building2 } from 'lucide-react';

function DashboardPage() {
  const router = useRouter();
  const { user, logout } = useAuth();
  const [selectedEquipment, setSelectedEquipment] = useState<Equipment | null>(null);

  // Fetch departments (no need to check isLoading/isAuthenticated - HOC handles it)
  const { data: departments = [] } = useQuery({
    queryKey: ['departments', user?.companyId],
    queryFn: () => departmentService.getDepartments(user?.companyId),
    enabled: !!user?.companyId,
  });

  // Fetch issues for selected equipment
  const { data: issues = [] } = useQuery({
    queryKey: ['issues', selectedEquipment?.id],
    queryFn: () => issueService.getIssues(selectedEquipment!.id),
    enabled: !!selectedEquipment,
  });

  const handleLogout = async () => {
    await logout();
    router.push('/login');
  };

  return (
    <div className="h-screen flex flex-col">
      {/* Header */}
      <header className="border-b bg-card">
        <div className="flex items-center justify-between px-6 py-4">
          <div>
            <h1 className="text-2xl font-bold">TechTrack</h1>
            <p className="text-sm text-muted-foreground">
              {user?.fullName || user?.email}
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

// Wrap with auth protection that requires company
export default withAuth(DashboardPage, { requireCompany: true });
