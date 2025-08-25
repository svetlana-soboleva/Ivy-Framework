import { Button } from '@/components/ui/button';
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from '@/components/ui/collapsible';
import { ChevronDown, ChevronRight } from 'lucide-react';
import React from 'react';

interface ExpandableWidgetProps {
  id: string;
  disabled?: boolean;
  slots?: {
    Header: React.ReactNode;
    Content: React.ReactNode;
  };
}

export const ExpandableWidget: React.FC<ExpandableWidgetProps> = ({
  id,
  disabled = false,
  slots,
}) => {
  const [isOpen, setIsOpen] = React.useState(false);

  // Use useEffect to handle disabled state changes
  React.useEffect(() => {
    if (disabled && isOpen) {
      setIsOpen(false);
    }
  }, [disabled, isOpen]);

  return (
    <Collapsible
      key={id}
      open={isOpen}
      onOpenChange={disabled ? undefined : setIsOpen}
      className="w-full rounded-md border border-border p-2 shadow-sm data-[disabled=true]:cursor-not-allowed data-[disabled=true]:opacity-50"
      data-disabled={disabled}
    >
      <div className="flex justify-between space-x-4">
        <CollapsibleTrigger asChild disabled={disabled}>
          <Button
            variant="ghost"
            disabled={disabled}
            className="w-full p-0 disabled:cursor-not-allowed disabled:opacity-50"
          >
            <div className="ml-2">{slots?.Header}</div>
            {!isOpen && <ChevronRight className="h-4 w-4 ml-auto mr-2" />}
            {isOpen && <ChevronDown className="h-4 w-4 ml-auto mr-2" />}
          </Button>
        </CollapsibleTrigger>
      </div>
      <CollapsibleContent className="space-y-4 p-2">
        {slots?.Content}
      </CollapsibleContent>
    </Collapsible>
  );
};
