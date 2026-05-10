'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';

export function Sidebar() {
  const pathname = usePathname();
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  const links = [
    { href: '/dashboard', label: 'Дашборд' },
    { href: '/departments', label: 'Отделы' },
    { href: '/employees', label: 'Сотрудники' },
    { href: '/notifications', label: 'Уведомления' },
    { href: '/settings', label: 'Настройки' },
  ];

  if (!mounted) {
    return (
      <aside className="w-64 bg-white border-r border-gray-200">
        <nav className="p-4 space-y-1">{/* Placeholder */}</nav>
      </aside>
    );
  }

  return (
    <aside className="w-64 bg-white border-r border-gray-200">
      <nav className="p-4 space-y-1">
        {links.map((link) => (
          <Link
            key={link.href}
            href={link.href}
            className={`block px-3 py-2 rounded-md ${
              pathname === link.href
                ? 'bg-blue-50 text-blue-700'
                : 'text-gray-700 hover:bg-gray-100'
            }`}
          >
            {link.label}
          </Link>
        ))}
      </nav>
    </aside>
  );
}
