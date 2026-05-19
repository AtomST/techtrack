'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { Menu, X, LayoutDashboard, Briefcase, Users, Bell, Settings, ChevronLeft, ChevronRight } from 'lucide-react';
import { cn } from '@/utils/cn';

interface SidebarProps {
  collapsed?: boolean;
  onToggle?: () => void;
}

export function Sidebar({ collapsed = false, onToggle }: SidebarProps) {
  const pathname = usePathname();
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  const links = [
    { href: '/dashboard', label: 'Дашборд', icon: LayoutDashboard },
    { href: '/departments', label: 'Отделы', icon: Briefcase },
    { href: '/employees', label: 'Сотрудники', icon: Users },
    { href: '/notifications', label: 'Уведомления', icon: Bell },
    { href: '/settings', label: 'Настройки', icon: Settings },
  ];

  if (!mounted) {
    return (
      <aside className={`bg-white border-r border-gray-200 transition-all duration-300 ${collapsed ? 'w-16' : 'w-64'}`}>
        <div className="p-4" />
      </aside>
    );
  }

  return (
    <aside className={`bg-white border-r border-gray-200 transition-all duration-300 ${collapsed ? 'w-16' : 'w-64'}`}>
      <div className="flex justify-end p-2">
        <button
          onClick={onToggle}
          className="p-1.5 rounded-md hover:bg-gray-100 transition-colors"
          title={collapsed ? 'Развернуть' : 'Свернуть'}
        >
          {collapsed ? <ChevronRight className="w-4 h-4 text-gray-500" /> : <ChevronLeft className="w-4 h-4 text-gray-500" />}
        </button>
      </div>

      <nav className="px-2 space-y-1">
        {links.map((link) => {
          const isActive = pathname === link.href || pathname?.startsWith(link.href + '/');
          const Icon = link.icon;
          
          return (
            <Link
              key={link.href}
              href={link.href}
              className={cn(
                'flex items-center px-3 py-2 rounded-md transition-colors',
                collapsed ? 'justify-center' : 'space-x-3',
                isActive
                  ? 'bg-blue-50 text-blue-700'
                  : 'text-gray-700 hover:bg-gray-100'
              )}
              title={collapsed ? link.label : undefined}
            >
              <Icon className={cn('w-5 h-5 flex-shrink-0', isActive ? 'text-blue-700' : 'text-gray-500')} />
              {!collapsed && <span className="text-sm">{link.label}</span>}
            </Link>
          );
        })}
      </nav>
    </aside>
  );
}
