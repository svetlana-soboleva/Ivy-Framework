import * as React from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import { DayPicker } from 'react-day-picker';
import './calendar.css';

export type CalendarProps = React.ComponentProps<typeof DayPicker>;

function Calendar({ showOutsideDays = true, ...props }: CalendarProps) {
  return (
    <DayPicker
      showOutsideDays={showOutsideDays}
      className="calendar-container"
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
      {...props}
    />
  );
}
Calendar.displayName = 'Calendar';

export { Calendar };
