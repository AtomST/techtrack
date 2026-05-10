'use client';

import { useState, useEffect } from 'react';
import { companyService } from '@/services/api/companies';
import { departmentService } from '@/services/api/departments';
import { usersService } from '@/services/api/users';
import { Button } from '@/components/ui/Button';
import { Modal } from '@/components/ui/Modal';
import { Input } from '@/components/ui/Input';
import { Users, UserPlus, Building2, Crown } from 'lucide-react';
import { AddEmployeeModal } from '@/components/modals/AddEmployeeModal';
import { useAuthStore } from '@/store/authStore';
import toast from 'react-hot-toast';

interface Employee {
  userId: string;
  userFullname: string;
  departmentId: string;
  departmentName: string;
  email?: string;
}

export default function EmployeesPage() {
  const { user } = useAuthStore();
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [departments, setDepartments] = useState<any[]>([]);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showRoleModal, setShowRoleModal] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState<Employee | null>(null);
  const [selectedRole, setSelectedRole] = useState('');
  const [loading, setLoading] = useState(false);
  const [isClient, setIsClient] = useState(false);

  useEffect(() => {
    setIsClient(true);
  }, []);

  useEffect(() => {
    if (isClient && user?.companyId) {
      fetchEmployees();
      fetchDepartments();
    }
  }, [isClient, user?.companyId]);

  const fetchEmployees = async () => {
    if (!user?.companyId) return;
    try {
      const data = await companyService.getEmployees(user.companyId);
      setEmployees(data);
    } catch (error) {
      console.error('Failed to fetch employees:', error);
    }
  };

  const fetchDepartments = async () => {
    try {
      let data;
      if (user?.role === 'Admin' || user?.role === 'CompanyHead') {
        data = await departmentService.getAllDepartments();
      } else {
        data = await departmentService.getMyDepartments();
      }
      setDepartments(data);
    } catch (error) {
      console.error('Failed to fetch departments:', error);
    }
  };

  const handleAssignRole = async () => {
    if (!selectedEmployee || !selectedRole) return;

    setLoading(true);
    try {
      await usersService.assignRole(selectedEmployee.userId, selectedRole);
      toast.success('Роль назначена');
      setShowRoleModal(false);
      setSelectedEmployee(null);
      setSelectedRole('');
      await fetchEmployees();
    } catch (error: any) {
      toast.error(error.message || 'Ошибка назначения роли');
    } finally {
      setLoading(false);
    }
  };

  if (!isClient) {
    return (
      <div className="flex items-center justify-center h-full">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <div className="flex items-center space-x-3">
          <Users className="w-8 h-8 text-gray-700" />
          <h1 className="text-2xl font-bold text-gray-900">Сотрудники</h1>
        </div>
        <Button onClick={() => setShowAddModal(true)}>
          <UserPlus className="w-4 h-4 mr-2" />
          Добавить сотрудника
        </Button>
      </div>

      {employees.length === 0 ? (
        <div className="bg-white rounded-lg shadow-sm p-12 text-center">
          <Users className="w-16 h-16 text-gray-300 mx-auto mb-4" />
          <h3 className="text-lg font-medium text-gray-900">Нет сотрудников</h3>
          <p className="text-gray-500 mt-1">Добавьте сотрудников в компанию</p>
          <Button className="mt-4" onClick={() => setShowAddModal(true)}>
            <UserPlus className="w-4 h-4 mr-2" />
            Добавить сотрудника
          </Button>
        </div>
      ) : (
        <div className="bg-white rounded-lg shadow-sm overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Сотрудник
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Отдел
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Действия
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {employees.map((employee) => (
                <tr key={employee.userId}>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex items-center">
                      <div className="flex-shrink-0 h-10 w-10 bg-blue-100 rounded-full flex items-center justify-center">
                        <span className="text-blue-600 font-medium">
                          {employee.userFullname?.charAt(0) || '?'}
                        </span>
                      </div>
                      <div className="ml-4">
                        <div className="text-sm font-medium text-gray-900">
                          {employee.userFullname}
                        </div>
                        <div className="text-sm text-gray-500">
                          {employee.email || `ID: ${employee.userId.slice(0, 8)}...`}
                        </div>
                      </div>
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex items-center text-sm text-gray-900">
                      <Building2 className="w-4 h-4 mr-2 text-gray-400" />
                      {employee.departmentName || 'Не назначен'}
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm">
                    <Button
                      size="sm"
                      variant="outline"
                      onClick={() => {
                        setSelectedEmployee(employee);
                        setShowRoleModal(true);
                      }}
                    >
                      <Crown className="w-4 h-4 mr-2" />
                      Назначить роль
                    </Button>
                    </td>
                  </tr>
                ))}
            </tbody>
          </table>
        </div>
      )}

      <AddEmployeeModal
        isOpen={showAddModal}
        onClose={() => {
          setShowAddModal(false);
          fetchEmployees();
        }}
        companyId={user?.companyId || ''}
      />

      <Modal isOpen={showRoleModal} onClose={() => setShowRoleModal(false)} title="Назначение роли">
        <div className="space-y-4">
          <p className="text-sm text-gray-600">
            Сотрудник: <span className="font-medium">{selectedEmployee?.userFullname}</span>
          </p>
          
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Роль
            </label>
            <select
              className="w-full rounded-lg border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={selectedRole}
              onChange={(e) => setSelectedRole(e.target.value)}
            >
              <option value="">Выберите роль</option>
              <option value="Admin">Admin</option>
              <option value="PlatformAdmin">PlatformAdmin</option>
              <option value="Manager">Manager</option>
              <option value="DepartmentHead">DepartmentHead</option>
              <option value="CompanyHead">CompanyHead</option>
              <option value="Employee">Employee</option>
            </select>
          </div>

          <div className="flex justify-end space-x-3 pt-4">
            <Button variant="outline" onClick={() => setShowRoleModal(false)}>
              Отмена
            </Button>
            <Button onClick={handleAssignRole} loading={loading}>
              Назначить
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  );
}
