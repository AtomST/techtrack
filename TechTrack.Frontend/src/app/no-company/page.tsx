'use client';

import { useRouter } from 'next/navigation';
import { useAuth } from '@/hooks/useAuth';
import { withAuth } from '@/components/withAuth';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Building2, LogOut } from 'lucide-react';

function NoCompanyPage() {
  const router = useRouter();
  const { user, logout } = useAuth();

  const handleLogout = async () => {
    await logout();
    router.push('/login');
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-indigo-100 p-4">
      <Card className="w-full max-w-md">
        <CardHeader className="text-center">
          <div className="flex justify-center mb-4">
            <div className="rounded-full bg-yellow-100 p-4">
              <Building2 className="h-12 w-12 text-yellow-600" />
            </div>
          </div>
          <CardTitle className="text-2xl">Компания не найдена</CardTitle>
          <CardDescription>
            Ваш аккаунт не привязан ни к одной компании
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="bg-muted p-4 rounded-lg">
            <p className="text-sm text-muted-foreground mb-2">
              <strong>Email:</strong> {user?.email}
            </p>
            <p className="text-sm text-muted-foreground">
              <strong>Имя:</strong> {user?.fullName || 'Не указано'}
            </p>
          </div>

          <div className="space-y-2">
            <p className="text-sm">
              Чтобы начать работу с TechTrack, вам нужно:
            </p>
            <ul className="text-sm text-muted-foreground list-disc list-inside space-y-1">
              <li>Обратитесь к администратору компании</li>
              <li>Попросите добавить вас в существующую компанию</li>
              <li>Или создайте новую компанию через API</li>
            </ul>
          </div>

          <div className="pt-4 border-t">
            <Button 
              variant="outline" 
              className="w-full" 
              onClick={handleLogout}
            >
              <LogOut className="h-4 w-4 mr-2" />
              Выйти из аккаунта
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}

// Wrap with auth (but don't require company)
export default withAuth(NoCompanyPage, { requireCompany: false });
