import { Separator } from '@/components/ui/separator';
import { getHeight, getWidth } from '@/lib/styles';
import { cn } from '@/lib/utils';
import React from 'react';

interface SeparatorWidgetProps {
  id: string;
  orientation: 'Vertical' | 'Horizontal';
  text?: string;
  height?: string;
  width?: string;
}

export const SeparatorWidget: React.FC<SeparatorWidgetProps> = ({ orientation, text, width, height }) => {
  
  var styles = {}
  if(orientation === 'Vertical') {
    styles = {...styles, ...getWidth(width)}
  } else {
    styles = {...styles, ...getHeight(height)}
  }

  var separator = <Separator
    orientation={orientation === 'Vertical' ? 'vertical' : 'horizontal'}
    className={cn(
      orientation === 'Vertical' && 'h-full'
    )} />


  if (text) {
    return (
      <div className="relative flex items-center justify-center">
        {separator}
        <span className="absolute px-2 text-xs text-muted-foreground bg-background">
          {text}
        </span>
      </div>
    );
  }
  
  return <div style={styles} className="flex items-center justify-center">{separator}</div>
};