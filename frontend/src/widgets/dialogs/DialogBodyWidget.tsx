import { DialogDescription } from '@/components/ui/dialog';
import React from 'react';

interface DialogBodyWidgetProps {
  id: string;
  children?: React.ReactNode;
}

export const DialogBodyWidget: React.FC<DialogBodyWidgetProps> = ({
  children
}) => (
  <DialogDescription>
      {children}
  </DialogDescription>
);