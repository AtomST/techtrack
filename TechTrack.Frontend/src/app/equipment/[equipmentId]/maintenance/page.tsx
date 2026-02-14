'use client';

import { useRouter, useParams } from 'next/navigation';
import { useQuery } from '@tanstack/react-query';
import { maintenanceService } from '@/services/issue-maintenance.service';
import { withAuth } from '@/components/withAuth';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { ArrowLeft, Plus, Calendar } from 'lucide-react';
import { getStatusColor, formatDate, formatDateTime } from '@/lib/utils';

function MaintenancePage() {
  const router = useRouter();
  const params = useParams();
  const equipmentId = params.equipmentId as string;

  const { data: maintenances = [], isLoading } = useQuery({
    queryKey: ['maintenances', equipmentId],
    queryFn: () => maintenanceService.getMaintenances(equipmentId),
    enabled: !!equipmentId,
  });

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary mx-auto"></div>
          <p className="mt-4 text-muted-foreground">Загрузка...</p>
        </div>
      </div>
    );
  }

  const upcomingMaintenance = maintenances.filter(
    (m) => m.status === 'SCHEDULED' || m.status === 'IN_PROGRESS'
  );
  const completedMaintenance = maintenances.filter(
    (m) => m.status === 'COMPLETED'
  );

  return (
    <div className="min-h-screen bg-background">
      <div className="border-b bg-card">
        <div className="container mx-auto px-4 py-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-4">
              <Button
                variant="ghost"
                size="icon"
                onClick={() => router.back()}
              >
                <ArrowLeft className="h-5 w-5" />
              </Button>
              <div>
                <h1 className="text-2xl font-bold">Журнал ТО</h1>
                <p className="text-sm text-muted-foreground">
                  Всего записей: {maintenances.length}
                </p>
              </div>
            </div>
            <Button onClick={() => router.push(`/equipment/${equipmentId}/maintenance/new`)}>
              <Plus className="h-4 w-4 mr-2" />
              Запланировать
            </Button>
          </div>
        </div>
      </div>

      <div className="container mx-auto px-4 py-6 space-y-8">
        {/* Upcoming Maintenance */}
        <div>
          <h2 className="text-xl font-semibold mb-4 flex items-center gap-2">
            <Calendar className="h-5 w-5" />
            Предстоящие ({upcomingMaintenance.length})
          </h2>
          <div className="space-y-4">
            {upcomingMaintenance.length === 0 ? (
              <Card>
                <CardContent className="py-8 text-center text-muted-foreground">
                  Нет запланированных работ
                </CardContent>
              </Card>
            ) : (
              upcomingMaintenance.map((maintenance) => (
                <Card key={maintenance.id} className="hover:shadow-md transition-shadow">
                  <CardHeader>
                    <div className="flex items-start justify-between">
                      <div>
                        <CardTitle className="text-lg">{maintenance.description}</CardTitle>
                        <p className="text-sm text-muted-foreground mt-1">
                          {maintenance.type}
                        </p>
                      </div>
                      <Badge className={getStatusColor(maintenance.status)}>
                        {maintenance.status}
                      </Badge>
                    </div>
                  </CardHeader>
                  <CardContent>
                    <div className="grid grid-cols-2 gap-4 text-sm">
                      <div>
                        <span className="text-muted-foreground">Запланировано:</span>
                        <p className="font-medium">{formatDate(maintenance.scheduledDate)}</p>
                      </div>
                      {maintenance.performedBy && (
                        <div>
                          <span className="text-muted-foreground">Исполнитель:</span>
                          <p className="font-medium">{maintenance.performedBy}</p>
                        </div>
                      )}
                    </div>
                    {maintenance.notes && (
                      <div className="mt-4">
                        <span className="text-muted-foreground text-sm">Примечания:</span>
                        <p className="text-sm mt-1">{maintenance.notes}</p>
                      </div>
                    )}
                  </CardContent>
                </Card>
              ))
            )}
          </div>
        </div>

        {/* Completed Maintenance */}
        <div>
          <h2 className="text-xl font-semibold mb-4">
            История ({completedMaintenance.length})
          </h2>
          <div className="space-y-4">
            {completedMaintenance.length === 0 ? (
              <Card>
                <CardContent className="py-8 text-center text-muted-foreground">
                  Нет выполненных работ
                </CardContent>
              </Card>
            ) : (
              completedMaintenance.map((maintenance) => (
                <Card key={maintenance.id}>
                  <CardHeader>
                    <div className="flex items-start justify-between">
                      <div>
                        <CardTitle className="text-lg">{maintenance.description}</CardTitle>
                        <p className="text-sm text-muted-foreground mt-1">
                          {maintenance.type}
                        </p>
                      </div>
                      <Badge className={getStatusColor(maintenance.status)}>
                        {maintenance.status}
                      </Badge>
                    </div>
                  </CardHeader>
                  <CardContent>
                    <div className="grid grid-cols-3 gap-4 text-sm">
                      <div>
                        <span className="text-muted-foreground">Запланировано:</span>
                        <p className="font-medium">{formatDate(maintenance.scheduledDate)}</p>
                      </div>
                      {maintenance.completedDate && (
                        <div>
                          <span className="text-muted-foreground">Выполнено:</span>
                          <p className="font-medium">{formatDate(maintenance.completedDate)}</p>
                        </div>
                      )}
                      {maintenance.performedBy && (
                        <div>
                          <span className="text-muted-foreground">Исполнитель:</span>
                          <p className="font-medium">{maintenance.performedBy}</p>
                        </div>
                      )}
                    </div>
                    {maintenance.notes && (
                      <div className="mt-4">
                        <span className="text-muted-foreground text-sm">Примечания:</span>
                        <p className="text-sm mt-1">{maintenance.notes}</p>
                      </div>
                    )}
                  </CardContent>
                </Card>
              ))
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

export default withAuth(MaintenancePage);
