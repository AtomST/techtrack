import { type ClassValue, clsx } from 'clsx';
import { twMerge } from 'tailwind-merge';
import { IssueSeverity, IssueStatus, MaintenanceStatus } from '@/types';

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function getEquipmentStatusColor(statusId: number): string {
  const statusColors: Record<number, string> = {
    0: 'bg-gray-100 text-gray-800',      // Unknown/Undefined
    1: 'bg-green-100 text-green-800',    // Operational
    2: 'bg-yellow-100 text-yellow-800',  // Maintenance
    3: 'bg-orange-100 text-orange-800',  // Repair
    4: 'bg-red-100 text-red-800',        // Offline
  };

  return statusColors[statusId] || 'bg-gray-100 text-gray-800';
}

export function getEquipmentStatusName(statusId: number): string {
  const statusNames: Record<number, string> = {
    0: 'Неизвестно',
    1: 'Работает',
    2: 'На обслуживании',
    3: 'В ремонте',
    4: 'Не работает',
  };

  return statusNames[statusId] || 'Неизвестно';
}

export function getStatusColor(status: IssueStatus | MaintenanceStatus): string {
  const statusColors: Record<string, string> = {
    // Issue
    OPEN: 'bg-red-100 text-red-800',
    IN_PROGRESS: 'bg-blue-100 text-blue-800',
    RESOLVED: 'bg-green-100 text-green-800',
    CLOSED: 'bg-gray-100 text-gray-800',
    
    // Maintenance
    SCHEDULED: 'bg-blue-100 text-blue-800',
    COMPLETED: 'bg-green-100 text-green-800',
    CANCELLED: 'bg-gray-100 text-gray-800',
  };

  return statusColors[status] || 'bg-gray-100 text-gray-800';
}

export function getSeverityColor(severity: IssueSeverity): string {
  const severityColors: Record<IssueSeverity, string> = {
    LOW: 'bg-blue-100 text-blue-800',
    MEDIUM: 'bg-yellow-100 text-yellow-800',
    HIGH: 'bg-orange-100 text-orange-800',
    CRITICAL: 'bg-red-100 text-red-800',
  };

  return severityColors[severity];
}

export function formatDate(date: string): string {
  return new Date(date).toLocaleDateString('ru-RU', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  });
}

export function formatDateTime(date: string): string {
  return new Date(date).toLocaleString('ru-RU', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
}
