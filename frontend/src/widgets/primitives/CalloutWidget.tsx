import Icon from "@/components/Icon";
import { getHeight, getWidth } from "@/lib/styles";
import { cn } from '@/lib/utils';
import React from 'react';

interface CalloutWidgetProps {
  title?: string;
  children?: React.ReactNode;
  variant?: 'Info' | 'Success' | 'Warning' | 'Error';
  width?: string;
  height?: string;
  icon?: string
}

const variantClasses = 
  {
    'Info': 'bg-blue-100 text-blue-800 border-blue-800',
    'Success': 'bg-green-100 text-green-800 border-green-800',
    'Warning': 'bg-yellow-100 text-yellow-800 border-yellow-800',
    'Error': 'bg-red-100 text-red-800 border-red-800'
  };

const defaultIcons = {
  'Info': 'Info',
  'Success': 'CircleCheck',
  'Warning': 'CircleAlert',
  'Error': 'CircleAlert'
};

export const CalloutWidget: React.FC<CalloutWidgetProps> = ({
  title,
  children,
  variant,
  icon,
  width,
  height
}) => {

  const styles: React.CSSProperties = {
    ...getWidth(width),
    ...getHeight(height),
  };

  if (!icon) {
    icon = defaultIcons[variant || 'Info'];
  }

  return (
    <div style={styles} className={
      cn(
        "flex items-center p-4 text-sm border rounded-sm",
        variantClasses[variant || 'Info']
      )} role="alert">
      {icon && <Icon size="30" name={icon} className="mr-4"/>}
      <span className="sr-only">{variant}</span>
      <div className="flex flex-col">
        {title && <div className="font-medium">{title}</div>}
        {children && <div>{children}</div>}
      </div>
    </div>
  );
};