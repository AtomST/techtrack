'use client';

import { useRouter, useParams } from 'next/navigation';
import { useQuery } from '@tanstack/react-query';
import { issueService } from '@/services/issue-maintenance.service';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { ArrowLeft, Plus } from 'lucide-react';
import { getStatusColor, getSeverityColor, formatDateTime } from '@/lib/utils';

export default function IssuesPage() {
  const router = useRouter();
  const params = useParams();
  const equipmentId = params.equipmentId as string;

  const { data: issues = [], isLoading } = useQuery({
    queryKey: ['issues', equipmentId],
    queryFn: () => issueService.getIssues(equipmentId),
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
                <h1 className="text-2xl font-bold">Все неисправности</h1>
                <p className="text-sm text-muted-foreground">
                  Всего: {issues.length}
                </p>
              </div>
            </div>
            <Button onClick={() => router.push(`/equipment/${equipmentId}/issues/new`)}>
              <Plus className="h-4 w-4 mr-2" />
              Добавить
            </Button>
          </div>
        </div>
      </div>

      <div className="container mx-auto px-4 py-6">
        <div className="space-y-4">
          {issues.length === 0 ? (
            <Card>
              <CardContent className="py-12 text-center text-muted-foreground">
                Нет неисправностей
              </CardContent>
            </Card>
          ) : (
            issues.map((issue) => (
              <Card key={issue.id} className="hover:shadow-md transition-shadow">
                <CardHeader>
                  <div className="flex items-start justify-between">
                    <CardTitle className="text-lg">{issue.title}</CardTitle>
                    <div className="flex gap-2">
                      <Badge className={getSeverityColor(issue.severity)}>
                        {issue.severity}
                      </Badge>
                      <Badge className={getStatusColor(issue.status)}>
                        {issue.status}
                      </Badge>
                    </div>
                  </div>
                </CardHeader>
                <CardContent>
                  <p className="text-muted-foreground mb-4">{issue.description}</p>
                  <div className="grid grid-cols-2 gap-4 text-sm">
                    <div>
                      <span className="text-muted-foreground">Создано:</span>
                      <p className="font-medium">{formatDateTime(issue.createdAt)}</p>
                    </div>
                    <div>
                      <span className="text-muted-foreground">Обновлено:</span>
                      <p className="font-medium">{formatDateTime(issue.updatedAt)}</p>
                    </div>
                    {issue.assignedTo && (
                      <div>
                        <span className="text-muted-foreground">Назначено:</span>
                        <p className="font-medium">{issue.assignedTo}</p>
                      </div>
                    )}
                    {issue.resolvedAt && (
                      <div>
                        <span className="text-muted-foreground">Решено:</span>
                        <p className="font-medium">{formatDateTime(issue.resolvedAt)}</p>
                      </div>
                    )}
                  </div>
                </CardContent>
              </Card>
            ))
          )}
        </div>
      </div>
    </div>
  );
}
