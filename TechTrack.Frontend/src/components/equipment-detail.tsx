'use client';

import { useState } from 'react';
import { Equipment, Issue } from '@/types';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { getEquipmentStatusColor, getEquipmentStatusName, getSeverityColor, formatDate } from '@/lib/utils';
import { AlertCircle, Wrench, Plus, ArrowRight } from 'lucide-react';

interface EquipmentDetailProps {
  equipment: Equipment;
  issues: Issue[];
  onViewAllIssues: () => void;
  onViewMaintenance: () => void;
  onAddIssue: () => void;
}

export function EquipmentDetail({
  equipment,
  issues,
  onViewAllIssues,
  onViewMaintenance,
  onAddIssue,
}: EquipmentDetailProps) {
  const openIssues = issues.filter((issue) => 
    issue.status === 'OPEN' || issue.status === 'IN_PROGRESS'
  );

  return (
    <div className="flex-1 p-6 overflow-y-auto">
      <div className="max-w-4xl mx-auto space-y-6">
        {/* Equipment Info */}
        <Card>
          <CardHeader>
            <div className="flex items-start justify-between">
              <div>
                <CardTitle>{equipment.name}</CardTitle>
                <CardDescription>
                  Серийный номер: {equipment.serialNumber}
                </CardDescription>
              </div>
              <Badge className={getEquipmentStatusColor(equipment.currentStatusId)}>
                {equipment.equipmentStatus?.name || getEquipmentStatusName(equipment.currentStatusId)}
              </Badge>
            </div>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 gap-4 text-sm">
              <div>
                <span className="text-muted-foreground">Серийный номер:</span>
                <p className="font-medium">{equipment.serialNumber}</p>
              </div>
              {equipment.description && (
                <div className="col-span-2">
                  <span className="text-muted-foreground">Описание:</span>
                  <p className="font-medium">{equipment.description}</p>
                </div>
              )}
            </div>
          </CardContent>
        </Card>

        {/* Current Issues */}
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div className="flex items-center gap-2">
                <AlertCircle className="h-5 w-5 text-destructive" />
                <CardTitle>Текущие неисправности</CardTitle>
                {openIssues.length > 0 && (
                  <Badge variant="destructive">{openIssues.length}</Badge>
                )}
              </div>
              <div className="flex gap-2">
                <Button size="sm" variant="outline" onClick={onAddIssue}>
                  <Plus className="h-4 w-4 mr-1" />
                  Добавить
                </Button>
                <Button size="sm" variant="outline" onClick={onViewAllIssues}>
                  Все неисправности
                  <ArrowRight className="h-4 w-4 ml-1" />
                </Button>
              </div>
            </div>
          </CardHeader>
          <CardContent>
            {openIssues.length === 0 ? (
              <p className="text-muted-foreground text-center py-8">
                Нет активных неисправностей
              </p>
            ) : (
              <div className="space-y-3">
                {openIssues.map((issue) => (
                  <div
                    key={issue.id}
                    className="border rounded-lg p-4 hover:bg-accent/50 transition-colors"
                  >
                    <div className="flex items-start justify-between mb-2">
                      <h4 className="font-medium">{issue.title}</h4>
                      <div className="flex gap-2">
                        <Badge className={getSeverityColor(issue.severity)}>
                          {issue.severity}
                        </Badge>
                      </div>
                    </div>
                    <p className="text-sm text-muted-foreground">{issue.description}</p>
                    <div className="mt-2 text-xs text-muted-foreground">
                      Создано: {formatDate(issue.createdAt)}
                    </div>
                  </div>
                ))}
              </div>
            )}
          </CardContent>
        </Card>

        {/* Maintenance Section */}
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div className="flex items-center gap-2">
                <Wrench className="h-5 w-5 text-primary" />
                <CardTitle>Техническое обслуживание</CardTitle>
              </div>
              <Button size="sm" variant="outline" onClick={onViewMaintenance}>
                Журнал ТО
                <ArrowRight className="h-4 w-4 ml-1" />
              </Button>
            </div>
          </CardHeader>
          <CardContent>
            <p className="text-muted-foreground text-center py-4">
              Перейдите в журнал ТО для просмотра и планирования обслуживания
            </p>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
