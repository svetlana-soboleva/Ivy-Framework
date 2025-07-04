import React from 'react';

interface DialogBodyWidgetProps {
  id: string;
  children?: React.ReactNode;
}

export const DialogBodyWidget: React.FC<DialogBodyWidgetProps> = ({
  children
}) => (
  <div className="flex-1 min-h-0 flex flex-col overflow-hidden">
    <div className="flex-1 min-h-0 overflow-auto">
      {children}
    </div>
  </div>
);