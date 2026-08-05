export const MAINTENANCE_STATUSES = {
  1: { label: 'Завершено', color: 'green' },
  2: { label: 'Отменено', color: 'gray' },
  3: { label: 'Запланировано', color: 'blue' },
  4: { label: 'Просрочено', color: 'red' },
} as const;

export const MAINTENANCE_TYPES = {
  1: { label: 'Профилактика', color: 'blue' },
  2: { label: 'Ремонт', color: 'orange' },
  3: { label: 'Модернизация', color: 'purple' },
} as const;

export const RECURRENCE_TYPES = {
  1: { label: 'Ежедневно', value: 'day' },
  2: { label: 'Еженедельно', value: 'week' },
  3: { label: 'Ежемесячно', value: 'month' },
} as const;

export const NOTIFICATION_TYPES = {
  1: { label: 'ТО завершено', color: 'green' },
  2: { label: 'ТО просрочено', color: 'red' },
  3: { label: 'Напоминание о ТО', color: 'blue' },
  4: { label: 'Критическая неисправность', color: 'orange' },
} as const;
