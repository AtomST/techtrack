import { format, formatDistance, isAfter, isBefore, addDays } from 'date-fns';
import { ru } from 'date-fns/locale';

export const formatDate = (date: string | Date, pattern = 'dd.MM.yyyy'): string => {
  return format(new Date(date), pattern, { locale: ru });
};

export const formatDateTime = (date: string | Date): string => {
  return format(new Date(date), 'dd.MM.yyyy HH:mm', { locale: ru });
};

export const getRelativeTime = (date: string | Date): string => {
  return formatDistance(new Date(date), new Date(), { addSuffix: true, locale: ru });
};

export const isOverdue = (date: string | Date): boolean => {
  return isBefore(new Date(date), new Date());
};

export const isUpcoming = (date: string | Date, days = 7): boolean => {
  const futureDate = addDays(new Date(), days);
  return !isAfter(new Date(date), futureDate);
};
