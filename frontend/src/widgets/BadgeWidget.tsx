import React from 'react';
import Icon from '@/components/Icon';
import { camelCase } from '@/lib/utils';
import { useEventHandler } from '@/components/EventHandlerContext';
import { Badge } from '@/components/ui/badge';
import { cn } from '@/lib/utils';

interface BadgeWidgetProps {
  id: string;
  title: string;
  icon?: string;
  variant?: string;
  size?: "Default" | "Small" | "Large";
  disabled: boolean;
}

export const BadgeWidget: React.FC<BadgeWidgetProps> = ({ 
  id, 
  title, 
  icon = undefined,
  variant = "default",
  size = "Default",
  disabled = false
}) => {
    const eventHandler = useEventHandler();

    const sizeClasses = {
      Small: "text-xs px-2 py-0.5",
      Default: "text-sm px-2.5 py-0.5", 
      Large: "text-base px-3 py-1"
    };

    const iconSizeClasses = {
      Small: "h-2.5 w-2.5",
      Default: "h-3 w-3",
      Large: "h-4 w-4"
    };

    return (
      <Badge
        onClick={() => !disabled && eventHandler("OnClick", id, [])}
        variant={camelCase(variant) as "default" | "destructive" | "outline" | "secondary"}
        className={cn(
          "w-min whitespace-nowrap",
          sizeClasses[size] || sizeClasses.Default
        )}
      >
        {icon && icon!="None" && (
          <Icon 
            className={cn("mr-1", iconSizeClasses[size] || iconSizeClasses.Default)} 
            name={icon} 
          />
        )}
        {title}
      </Badge>
    );
  };