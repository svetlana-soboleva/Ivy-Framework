import React from 'react';
import Icon from '@/components/Icon';
import { camelCase } from '@/lib/utils';
import { useEventHandler } from '@/components/EventHandlerContext';
import { Badge } from '@/components/ui/badge';

interface BadgeWidgetProps {
  id: string;
  title: string;
  icon?: string;
  variant?: string;
  disabled: boolean;
}

export const BadgeWidget: React.FC<BadgeWidgetProps> = ({ 
  id, 
  title, 
  icon = undefined,
  variant = "default",
  disabled = false
}) => {
    const eventHandler = useEventHandler();

    return (
      <Badge
        onClick={() => !disabled && eventHandler("OnClick", id, [])}
        variant={camelCase(variant) as "default" | "destructive" | "outline" | "secondary"}
        className="w-min whitespace-nowrap"
      >
        {icon && icon!="None" && <Icon className="h-3 w-3 mr-1" name={icon} />}
        {title}
      </Badge>
    );
  };