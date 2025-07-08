import * as React from 'react';
import { useState, useCallback, useMemo } from 'react';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import { Input } from '@/components/ui/input';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { format } from 'date-fns';
import { Calendar as CalendarIcon, Clock } from 'lucide-react';
import { cn } from '@/lib/utils';
import { useEventHandler } from '@/components/EventHandlerContext';
import { inputStyles } from '@/lib/styles';

type VariantType = 'Date' | 'DateTime' | 'Time';

interface DateTimeInputWidgetProps {
  id: string;
  value?: string;
  placeholder?: string;
  disabled: boolean;
  variant?: VariantType;
  nullable?: boolean;
  invalid?: string;
  format?: string;
  'data-testid'?: string;
}

interface BaseVariantProps {
  id: string;
  value?: string;
  placeholder?: string;
  disabled: boolean;
  nullable?: boolean;
  invalid?: string;
  format?: string;
  'data-testid'?: string;
}

interface DateVariantProps extends BaseVariantProps {
  onDateChange: (date: Date | undefined) => void;
}

interface DateTimeVariantProps extends BaseVariantProps {
  onDateChange: (date: Date | undefined) => void;
  onTimeChange: (time: string) => void;
}

interface TimeVariantProps extends BaseVariantProps {
  onTimeChange: (time: string) => void;
}

const DateVariant: React.FC<DateVariantProps> = ({
  value,
  placeholder,
  disabled,
  invalid,
  onDateChange,
  format: formatProp,
  'data-testid': dataTestId,
}) => {
  const [open, setOpen] = useState(false);
  const date = value ? new Date(value) : undefined;

  const handleSelect = useCallback(
    (selectedDate: Date | undefined) => {
      onDateChange(selectedDate);
      setOpen(false);
    },
    [onDateChange]
  );

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          disabled={disabled}
          variant="outline"
          className={cn(
            'w-full justify-start text-left font-normal',
            !date && 'text-muted-foreground',
            invalid && inputStyles.invalid
          )}
          data-testid={dataTestId}
        >
          <CalendarIcon className="mr-2 h-4 w-4" />
          {date ? (
            format(date, formatProp || 'yyyy-MM-dd')
          ) : (
            <span>{placeholder || 'Pick a date'}</span>
          )}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-auto p-0" align="start">
        <Calendar
          mode="single"
          selected={date}
          onSelect={handleSelect}
          initialFocus
        />
      </PopoverContent>
    </Popover>
  );
};

const DateTimeVariant: React.FC<DateTimeVariantProps> = ({
  value,
  placeholder,
  disabled,
  invalid,
  onDateChange,
  onTimeChange,
  format: formatProp,
  'data-testid': dataTestId,
}) => {
  const [open, setOpen] = useState(false);
  const date = value ? new Date(value) : undefined;

  // Extract time from date or use current time
  const timeValue = useMemo(() => {
    if (date) {
      return format(date, formatProp || 'HH:mm:ss');
    }
    return '00:00:00';
  }, [date, formatProp]);

  const handleDateSelect = useCallback(
    (selectedDate: Date | undefined) => {
      if (selectedDate) {
        // Preserve the time when selecting a new date
        const currentTime = date ? date : new Date();
        const newDateTime = new Date(selectedDate);
        newDateTime.setHours(
          currentTime.getHours(),
          currentTime.getMinutes(),
          currentTime.getSeconds()
        );
        onDateChange(newDateTime);
      } else {
        onDateChange(undefined);
      }
      // Do not close the popover here, allow user to pick time
    },
    [date, onDateChange]
  );

  const handleTimeChange = useCallback(
    (time: string) => {
      if (date && time) {
        const [hours, minutes, seconds] = time.split(':').map(Number);
        const newDateTime = new Date(date);
        newDateTime.setHours(hours, minutes, seconds);
        onDateChange(newDateTime);
      } else if (time) {
        // If no date is selected, create a new date with the selected time
        const [hours, minutes, seconds] = time.split(':').map(Number);
        const newDateTime = new Date();
        newDateTime.setHours(hours, minutes, seconds);
        onDateChange(newDateTime);
      }
      onTimeChange(time);
    },
    [date, onDateChange, onTimeChange]
  );

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          disabled={disabled}
          variant="outline"
          className={cn(
            'w-full justify-start text-left font-normal',
            !date && 'text-muted-foreground',
            invalid && inputStyles.invalid
          )}
          data-testid={dataTestId}
        >
          <CalendarIcon className="mr-2 h-4 w-4" />
          {date ? (
            format(date, formatProp || 'yyyy-MM-dd')
          ) : (
            <span>{placeholder || 'Pick a date & time'}</span>
          )}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-auto p-0" align="start">
        <div className="flex flex-col gap-2 p-2">
          <Calendar
            mode="single"
            selected={date}
            onSelect={handleDateSelect}
            initialFocus
          />
          <div className="flex items-center gap-2">
            <Clock className="h-4 w-4 text-muted-foreground" />
            <Input
              type="time"
              step="1"
              value={timeValue}
              onChange={e => handleTimeChange(e.target.value)}
              disabled={disabled}
              className={cn(
                'bg-background appearance-none [&::-webkit-calendar-picker-indicator]:hidden',
                invalid && inputStyles.invalid
              )}
              data-testid={dataTestId ? `${dataTestId}-time` : undefined}
            />
          </div>
        </div>
      </PopoverContent>
    </Popover>
  );
};

const TimeVariant: React.FC<TimeVariantProps> = ({
  value,
  placeholder,
  disabled,
  invalid,
  onTimeChange,
  'data-testid': dataTestId,
}) => {
  // Always use 24-hour format 'HH:mm:ss'
  const timeValue = useMemo(() => {
    if (value) {
      const date = new Date(
        `1970-01-01T${value.length <= 5 ? value + ':00' : value}`
      ); // Accept 'HH:mm' or 'HH:mm:ss'
      if (!isNaN(date.getTime())) {
        return format(date, 'HH:mm:ss');
      }
    }
    return '00:00:00';
  }, [value]);

  return (
    <div className="flex items-center gap-2">
      <Clock className="h-4 w-4 text-muted-foreground" />
      <Input
        type="time"
        step="1"
        value={timeValue}
        onChange={e => onTimeChange(e.target.value)}
        disabled={disabled}
        placeholder={placeholder || 'Select time'}
        className={cn(
          'bg-background appearance-none [&::-webkit-calendar-picker-indicator]:hidden',
          invalid && inputStyles.invalid
        )}
        data-testid={dataTestId}
      />
    </div>
  );
};

const VariantComponents = {
  Date: DateVariant,
  DateTime: DateTimeVariant,
  Time: TimeVariant,
};

export const DateTimeInputWidget: React.FC<DateTimeInputWidgetProps> = ({
  id,
  value,
  placeholder,
  disabled = false,
  variant = 'Date',
  nullable = false,
  invalid,
  format: formatProp,
  'data-testid': dataTestId,
}) => {
  const eventHandler = useEventHandler();

  // Normalize undefined to null when nullable
  const normalizedValue = nullable && value === undefined ? undefined : value;

  const handleDateChange = useCallback(
    (selectedDate: Date | undefined) => {
      if (disabled) return;
      const isoString = selectedDate?.toISOString();
      eventHandler('OnChange', id, [isoString]);
    },
    [disabled, eventHandler, id]
  );

  const handleTimeChange = useCallback(
    (time: string) => {
      if (disabled) return;

      // Create a date with the selected time
      const [hours, minutes, seconds] = time.split(':').map(Number);
      const newDateTime = new Date();
      newDateTime.setHours(hours, minutes, seconds);

      eventHandler('OnChange', id, [newDateTime.toISOString()]);
    },
    [disabled, eventHandler, id]
  );

  const VariantComponent = useMemo(() => VariantComponents[variant], [variant]);

  return (
    <VariantComponent
      id={id}
      value={normalizedValue}
      placeholder={placeholder}
      disabled={disabled}
      nullable={nullable}
      invalid={invalid}
      format={formatProp}
      onDateChange={handleDateChange}
      onTimeChange={handleTimeChange}
      data-testid={dataTestId}
    />
  );
};
