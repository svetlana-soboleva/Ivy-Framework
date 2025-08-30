import React from 'react';
import { ScrollArea } from '@/components/ui/scroll-area';

interface FooterLayoutWidgetProps {
  slots?: {
    Footer?: React.ReactNode[];
    Content?: React.ReactNode[];
  };
}

export const FooterLayoutWidget: React.FC<FooterLayoutWidgetProps> = ({
  slots,
}) => {
  if (!slots?.Footer || !slots?.Content) {
    return (
      <div className="text-red-500">
        Error: FooterLayout requires both Footer and Content slots.
      </div>
    );
  }

  return (
    <div className="h-full flex flex-col">
      <div className="flex-1 min-h-0 overflow-hidden">
        <ScrollArea className="h-full">
          <div>{slots.Content}</div>
        </ScrollArea>
      </div>
      <div className="flex-none p-4 border-t w-full bg-background">
        {slots.Footer}
      </div>
    </div>
  );
};
