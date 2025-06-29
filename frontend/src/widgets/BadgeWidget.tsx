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
  size?: string;
  disabled: boolean;
}

export const BadgeWidget: React.FC<BadgeWidgetProps> = ({ 
  id, 
  title, 
  icon = undefined,
  variant = "default",
  size = "default",
  disabled = false
}) => {
    const eventHandler = useEventHandler();

    const sizeClasses = {
      small: "text-xs px-2 py-0.5",
      default: "text-sm px-2.5 py-0.5", 
      large: "text-base px-3 py-1"
    };

    const iconSizeClasses = {
      small: "h-2.5 w-2.5",
      default: "h-3 w-3",
      large: "h-4 w-4"
    };

    return (
      <Badge
        onClick={() => !disabled && eventHandler("OnClick", id, [])}
        variant={camelCase(variant) as "default" | "destructive" | "outline" | "secondary"}
        className={cn(
          "w-min whitespace-nowrap",
          sizeClasses[size as keyof typeof sizeClasses] || sizeClasses.default
        )}
      >
        {icon && icon!="None" && (
          <Icon 
            className={cn("mr-1", iconSizeClasses[size as keyof typeof iconSizeClasses] || iconSizeClasses.default)} 
            name={icon} 
          />
        )}
        {title}
      </Badge>
    );
  };