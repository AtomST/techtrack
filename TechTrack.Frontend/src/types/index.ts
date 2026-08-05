export interface ApiResponse<T = any> {
  statusCode: number;
  data?: T;
  message?: string;
  errors?: Record<string, string>;
}

export interface User {
  id: string;
  email: string;
  fullName: string;
  role?: string;
  companyId?: string;
  departmentId?: string;
}

export interface Department {
  id: string;
  name: string;
  companyId: string;
  responsibleUserId?: string;
  responsibleUser?: User;
  equipments?: Equipment[];
}

export interface Equipment {
  id: string;
  name: string;
  serialNumber?: string;
  description?: string;
  currentStatusId?: number;
  departmentId: string;
  responsibleUserId?: string;
  responsibleUser?: User;
  createdAt: string;
}

export interface Issue {
  id: string;
  name: string;
  description?: string;
  equipmentId: string;
  statusId: number;
  createdAt: string;
  creatorId?: string;
  isResolved?: boolean;
  resolvedByMaintenanceId?: string;
}

export const UserRole = {
  Undefined: 'Undefined',
  Dev: 'Dev',
  Admin: 'Admin',
  PlatformAdmin: 'PlatformAdmin',
  Manager: 'Manager',
  DepartmentHead: 'DepartmentHead',
  CompanyHead: 'CompanyHead',
  Employee: 'Employee',
} as const;

export type UserRoleType = typeof UserRole[keyof typeof UserRole];
export interface Company {
  id: string;
  name: string;
  createdAt: string;
}


export enum EquipmentStatus {
  Operational = 1,  
  Warning = 2,    
  Critical = 3,    
  Maintenance = 4,  
}

export interface Maintenance {
  id: string;
  name: string;
  description?: string;
  equipmentId: string;
  maintenanceScheduleRecordId?: string;
  scheduledDate?: string;
  completedAt?: string;
  completedByUserId?: string;
  responsibleUserId?: string;
  maintenanceTypeId: number;
  maintenanceStatusId: number;
  solvedIssues?: Issue[];
}

export interface ScheduleMaintenanceRequest {
  recurrenceTypeId: number;
  maintenanceName: string;
  maintenanceDescription?: string;
  intervalValue: number;
  nextMaintenanceDate: string;
  responsibleUserId: string;
  notificationAdvanceDays: number;
}

export interface Notification {
  id: string;
  userId: string;
  relatedEntityId: string;
  title: string;
  message: string;
  notificationTypeId: number;
  createdAt: string;
}

export enum MaintenanceStatus {
  Completed = 1,
  Cancelled = 2,
  Scheduled = 3,
  Overdue = 4
}

export enum MaintenanceType {
  Preventive = 1,
  Repair = 2,
  Upgrade = 3
}
export interface Company {
  id: string;
  name: string;
  createdAt: string;
}
export enum RecurrenceType {
  Day = 1,
  Week = 2,
  Month = 3
}

export enum NotificationType {
  MaintenanceCompleted = 1,
  MaintenanceOverdue = 2,
  MaintenanceReminder = 3,
  CriticalIssueDetected = 4
}
export interface Maintenance {
  id: string;
  name: string;
  description?: string;
  equipmentId: string;
  maintenanceScheduleRecordId?: string;
  scheduledDate?: string;
  completedAt?: string;
  completedByUserId?: string;
  responsibleUserId?: string;
  maintenanceTypeId: number;
  maintenanceStatusId: number;
  solvedIssues?: Issue[];
}

export interface MaintenanceSchedule {
  id: string;
  equipmentId: string;
  maintenanceName: string;
  maintenanceDescription?: string;
  recurrenceTypeId: number;
  intervalValue: number;
  nextMaintenanceDate: string;
  responsibleUserId: string;
  notificationAdvanceDays: number;
  isActive: boolean;
}
