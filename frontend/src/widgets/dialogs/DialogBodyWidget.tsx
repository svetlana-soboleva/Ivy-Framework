import React from 'react';

interface DialogBodyWidgetProps {
  id: string;
  children?: React.ReactNode;
}

export const DialogBodyWidget: React.FC<DialogBodyWidgetProps> = ({
  children
}) => (
  <section className="flex-1 min-h-0 flex flex-col overflow-hidden" role="dialog" aria-describedby="dialog-description">
    <div className="flex-1 min-h-0 overflow-auto" id="dialog-description">
      {children}
    </div>
  </section>
);