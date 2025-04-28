import { Align, BorderRadius, BorderStyle, getAlign, getBackground, getBorderColor, getBorderRadius, getBorderStyle, getBorderThickness, getColor, getHeight, getPadding, getWidth } from '@/lib/styles';
import { cn } from '@/lib/utils';
import React from 'react';

interface BoxWidgetProps {
  id: string;
  children?: React.ReactNode;
  color?: string | undefined;
  borderRadius: BorderRadius;
  borderThickness: number;
  borderStyle: BorderStyle;
  width?: string;
  height?: string;
  padding: string;
  align: Align;
}

export const BoxWidget: React.FC<BoxWidgetProps> = ({ 
  children, width, height, borderStyle, borderRadius, borderThickness, color, padding, align }) => {
    const styles = { 
      ...getBorderStyle(borderStyle), 
      ...getBorderThickness(borderThickness), 
      ...getBorderRadius(borderRadius),
      ...getBorderColor(color),
      ...getPadding(padding),
      ...getAlign('Vertical', align),
      ...getWidth(width),
      ...getHeight(height),
      ...getBackground(color),
      ...getColor(color)
  };

    return (
      <>
        <div 
          style={styles}
          className={cn(
          "font-mono",
          "font-bold",
        )}>
          {children}
        </div>
      </>
    );
  };