// Authentication types
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  FullName: string;
  phoneNumber: string;
  Password: string;
}

export interface ApiResponse<T> {
  statusCode: number;
  data: T;
}

export interface AuthData {
  accessToken: string;
}

export interface AuthResponse {
  statusCode: number;
  data: AuthData;
}

export interface DecodedToken {
  nameid: string;
  role: string;
  company_id: string;
  nbf: number;
  exp: number;
  iat: number;
}

export interface User {
  id: string;
  email: string;
  fullName: string;
  companyId?: string;
  role: UserRole;
}

export enum UserRole {
  ADMIN = 'Admin',
  MANAGER = 'Manager',
  TECHNICIAN = 'Technician',
  VIEWER = 'Viewer',
  UNDEFINED = 'Undefined',
}

// Company types
export interface Company {
  id: string;
  name: string;
  description?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateCompanyRequest {
  name: string;
  description?: string;
}

// Department types
export interface Department {
  id: string;
  name: string;
  companyId: string;
  company?: Company;
  responsibleUserId?: string;
  equipments: Equipment[];
}

export interface CreateDepartmentRequest {
  name: string;
  companyId: string;
  responsibleUserId?: string;
}

// Equipment types
export interface Equipment {
  id: string;
  name: string;
  serialNumber: string;
  description?: string;
  currentStatusId: number;
  equipmentStatus?: EquipmentStatus;
  departmentId: string;
}

export interface EquipmentStatus {
  id: number;
  name: string;
}

export interface CreateEquipmentRequest {
  name: string;
  serialNumber: string;
  description?: string;
  currentStatusId: number;
  departmentId: string;
}

// Issue types
export interface Issue {
  id: string;
  equipmentId: string;
  title: string;
  description: string;
  severity: IssueSeverity;
  status: IssueStatus;
  reportedBy: string;
  assignedTo?: string;
  createdAt: string;
  updatedAt: string;
  resolvedAt?: string;
}

export enum IssueSeverity {
  LOW = 'LOW',
  MEDIUM = 'MEDIUM',
  HIGH = 'HIGH',
  CRITICAL = 'CRITICAL',
}

export enum IssueStatus {
  OPEN = 'OPEN',
  IN_PROGRESS = 'IN_PROGRESS',
  RESOLVED = 'RESOLVED',
  CLOSED = 'CLOSED',
}

export interface CreateIssueRequest {
  equipmentId: string;
  title: string;
  description: string;
  severity: IssueSeverity;
}

// Maintenance types
export interface Maintenance {
  id: string;
  equipmentId: string;
  type: MaintenanceType;
  scheduledDate: string;
  completedDate?: string;
  description: string;
  performedBy?: string;
  notes?: string;
  status: MaintenanceStatus;
  createdAt: string;
  updatedAt: string;
}

export enum MaintenanceType {
  PREVENTIVE = 'PREVENTIVE',
  CORRECTIVE = 'CORRECTIVE',
  INSPECTION = 'INSPECTION',
}

export enum MaintenanceStatus {
  SCHEDULED = 'SCHEDULED',
  IN_PROGRESS = 'IN_PROGRESS',
  COMPLETED = 'COMPLETED',
  CANCELLED = 'CANCELLED',
}

export interface CreateMaintenanceRequest {
  equipmentId: string;
  type: MaintenanceType;
  scheduledDate: string;
  description: string;
}
