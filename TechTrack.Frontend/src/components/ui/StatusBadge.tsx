'use client';

interface StatusBadgeProps {
  statusId?: number;
  size?: 'sm' | 'md' | 'lg';
}

const statusColors: Record<number, { bg: string; ring: string }> = {
  1: { bg: 'bg-green-500', ring: 'ring-green-300' },      // Работает
  2: { bg: 'bg-yellow-500', ring: 'ring-yellow-300' },    // Предупреждение
  3: { bg: 'bg-red-500', ring: 'ring-red-300' },          // Критический
  4: { bg: 'bg-gray-400', ring: 'ring-gray-300' },        // В обслуживании
};

const statusLabels: Record<number, string> = {
  1: 'Работает',
  2: 'Предупреждение',
  3: 'Критический',
  4: 'В обслуживании',
};

export function StatusBadge({ statusId, size = 'md' }: StatusBadgeProps) {
  const colors = statusColors[statusId || 4] || statusColors[4];
  const label = statusLabels[statusId || 4] || 'Неизвестно';
  
  const sizeClasses = {
    sm: 'w-2 h-2',
    md: 'w-3 h-3',
    lg: 'w-4 h-4',
  };

  return (
    <div className="flex items-center gap-2" title={label}>
      <div className={`${sizeClasses[size]} ${colors.bg} rounded-full ring-2 ${colors.ring} ring-offset-1`} />
    </div>
  );
}
