import { useState, ComponentProps } from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import { DayPicker } from 'react-day-picker';
import './calendar.css';

export type CalendarProps = ComponentProps<typeof DayPicker>;

function Calendar({ showOutsideDays = true, ...props }: CalendarProps) {
  const [internalSelected, setInternalSelected] = useState<Date>();
  return (
    <DayPicker
      showOutsideDays={showOutsideDays}
      className="calendar-container"
      mode="single"
      selected={internalSelected}
      onSelect={setInternalSelected}
      components={{
        PreviousMonthButton: props => (
          <button {...props}>
            <ChevronLeft className="h-4 w-4" />
          </button>
        ),
        NextMonthButton: props => (
          <button {...props}>
            <ChevronRight className="h-4 w-4" />
          </button>
        ),
      }}
      {...(props as Omit<CalendarProps, 'selected' | 'onSelect' | 'mode'>)}
    />
  );
}
Calendar.displayName = 'Calendar';

export { Calendar };
