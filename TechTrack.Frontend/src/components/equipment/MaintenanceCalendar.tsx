'use client';

import { useState, useEffect } from 'react';
import { Maintenance } from '@/types';
import { format, startOfMonth, endOfMonth, eachDayOfInterval, isSameMonth, isSameDay, startOfWeek, endOfWeek } from 'date-fns';
import { ru } from 'date-fns/locale';
import { Calendar, Clock, Wrench } from 'lucide-react';

interface MaintenanceCalendarProps {
  maintenances: Maintenance[];
  onDateClick?: (date: Date) => void;
}

export function MaintenanceCalendar({ maintenances, onDateClick }: MaintenanceCalendarProps) {
  const [currentMonth, setCurrentMonth] = useState(new Date());
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);

  const monthStart = startOfMonth(currentMonth);
  const monthEnd = endOfMonth(currentMonth);
  const startDate = startOfWeek(monthStart, { weekStartsOn: 1 });
  const endDate = endOfWeek(monthEnd, { weekStartsOn: 1 });

  const days = eachDayOfInterval({ start: startDate, end: endDate });

  const getMaintenancesForDate = (date: Date) => {
    return maintenances.filter(m => {
      if (!m.scheduledDate) return false;
      const scheduledDate = new Date(m.scheduledDate);
      return isSameDay(scheduledDate, date);
    });
  };

  const getStatusColor = (statusId: number) => {
    switch (statusId) {
      case 1: return 'bg-green-100 text-green-800 border-green-200';
      case 2: return 'bg-gray-100 text-gray-800 border-gray-200';
      case 3: return 'bg-blue-100 text-blue-800 border-blue-200';
      case 4: return 'bg-red-100 text-red-800 border-red-200';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (statusId: number) => {
    switch (statusId) {
      case 1: return 'Завершено';
      case 2: return 'Отменено';
      case 3: return 'Запланировано';
      case 4: return 'Просрочено';
      default: return 'Неизвестно';
    }
  };

  const prevMonth = () => {
    setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1));
  };

  const nextMonth = () => {
    setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1));
  };

  const goToToday = () => {
    setCurrentMonth(new Date());
    setSelectedDate(new Date());
  };

  return (
    <div className="bg-white rounded-lg shadow-sm p-6">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-xl font-semibold text-gray-900">Календарь ТО</h2>
        <div className="flex space-x-2">
          <button
            onClick={goToToday}
            className="px-3 py-1 text-sm bg-gray-100 hover:bg-gray-200 rounded-md transition"
          >
            Сегодня
          </button>
          <button
            onClick={prevMonth}
            className="px-3 py-1 text-sm bg-gray-100 hover:bg-gray-200 rounded-md transition"
          >
            ←
          </button>
          <span className="px-3 py-1 text-sm font-medium">
            {format(currentMonth, 'LLLL yyyy', { locale: ru })}
          </span>
          <button
            onClick={nextMonth}
            className="px-3 py-1 text-sm bg-gray-100 hover:bg-gray-200 rounded-md transition"
          >
            →
          </button>
        </div>
      </div>

      <div className="grid grid-cols-7 gap-1 mb-2">
        {['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'].map(day => (
          <div key={day} className="text-center text-sm font-medium text-gray-500 py-2">
            {day}
          </div>
        ))}
      </div>

      <div className="grid grid-cols-7 gap-1">
        {days.map((day, idx) => {
          const isCurrentMonth = isSameMonth(day, currentMonth);
          const dayMaintenances = getMaintenancesForDate(day);
          const isToday = isSameDay(day, new Date());
          const isSelected = selectedDate && isSameDay(day, selectedDate);

          return (
            <div
              key={idx}
              onClick={() => {
                setSelectedDate(day);
                onDateClick?.(day);
              }}
              className={`
                min-h-[100px] p-2 border rounded-lg cursor-pointer transition
                ${isCurrentMonth ? 'bg-white' : 'bg-gray-50'}
                ${isToday ? 'border-blue-500 border-2' : 'border-gray-200'}
                ${isSelected ? 'ring-2 ring-blue-300' : ''}
                hover:shadow-md
              `}
            >
              <div className={`text-sm font-medium mb-1 ${!isCurrentMonth ? 'text-gray-400' : 'text-gray-700'}`}>
                {format(day, 'd')}
              </div>
              <div className="space-y-1">
                {dayMaintenances.slice(0, 3).map(m => (
                  <div
                    key={m.id}
                    className={`text-xs p-1 rounded truncate ${getStatusColor(m.maintenanceStatusId)}`}
                    title={m.name}
                  >
                    {m.name}
                  </div>
                ))}
                {dayMaintenances.length > 3 && (
                  <div className="text-xs text-gray-500 text-center">
                    +{dayMaintenances.length - 3}
                  </div>
                )}
              </div>
            </div>
          );
        })}
      </div>

      {selectedDate && (
        <div className="mt-6 pt-4 border-t border-gray-200">
          <h3 className="font-semibold text-gray-900 mb-3">
            ТО на {format(selectedDate, 'dd MMMM yyyy', { locale: ru })}
          </h3>
          <div className="space-y-2">
            {getMaintenancesForDate(selectedDate).map(m => (
              <div key={m.id} className={`p-3 rounded-lg border ${getStatusColor(m.maintenanceStatusId)}`}>
                <div className="flex items-start justify-between">
                  <div>
                    <div className="flex items-center space-x-2">
                      <Wrench className="w-4 h-4" />
                      <span className="font-medium">{m.name}</span>
                    </div>
                    {m.description && (
                      <p className="text-sm mt-1">{m.description}</p>
                    )}
                    <div className="flex items-center space-x-4 mt-2 text-xs">
                      <span className="flex items-center">
                        <Clock className="w-3 h-3 mr-1" />
                        {m.scheduledDate && format(new Date(m.scheduledDate), 'HH:mm')}
                      </span>
                      <span>Статус: {getStatusText(m.maintenanceStatusId)}</span>
                    </div>
                  </div>
                </div>
              </div>
            ))}
            {getMaintenancesForDate(selectedDate).length === 0 && (
              <p className="text-gray-500 text-sm">Нет запланированных ТО на эту дату</p>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
