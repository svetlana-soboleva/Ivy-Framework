import React, { useCallback } from 'react';
import Icon from '@/components/Icon';
import { camelCase } from '@/lib/utils';
import { useEventHandler } from '@/components/EventHandlerContext';
import { Badge } from '@/components/ui/badge';
import { cn } from '@/lib/utils';

interface BadgeWidgetProps {
  id: string;
  title: string;
  icon?: string;
  iconPosition?: "Left" | "Right";
  variant?: string;
  size?: "Default" | "Small" | "Large";
  disabled: boolean;
}

export const BadgeWidget: React.FC<BadgeWidgetProps> = ({ 
  id, 
  title, 
  icon = undefined,
  iconPosition = "Left",
  variant = "default",
  size = "Default",
  disabled = false
}) => {
    const eventHandler = useEventHandler();

    let badgeClasses = "text-sm px-2.5 py-0.5";
    let iconClasses = "h-3 w-3";

    switch (size) {
      case "Small":
        badgeClasses = "text-xs px-2 py-0.5";
        iconClasses = "h-2.5 w-2.5";
        break;
      case "Large":
        badgeClasses = "text-base px-3 py-1";
        iconClasses = "h-4 w-4";
        break;
      default:
        break;
    }

    const handleClick = useCallback(() => {
      if (!disabled) {
        eventHandler("OnClick", id, []);
      }
    }, [disabled, eventHandler, id]);

    return (
      <Badge
        onClick={handleClick}
        variant={camelCase(variant) as "default" | "destructive" | "outline" | "secondary"}
        className={cn(
          "w-min whitespace-nowrap",
          badgeClasses,
          disabled && "opacity-50 cursor-not-allowed"
        )}
      >
        {iconPosition === "Left" && icon && icon !== "None" && (
          <Icon 
            className={cn("mr-1", iconClasses)} 
            name={icon} 
          />
        )}
        {title}
        {iconPosition === "Right" && icon && icon !== "None" && (
          <Icon 
            className={cn("ml-1", iconClasses)} 
            name={icon} 
          />
        )}
      </Badge>
    );
  };