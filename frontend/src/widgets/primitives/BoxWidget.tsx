import {
  Align,
  BorderRadius,
  BorderStyle,
  getAlign,
  getBorderRadius,
  getBorderStyle,
  getBorderThickness,
  getHeight,
  getMargin,
  getPadding,
  getWidth,
} from '@/lib/styles';
import { cn } from '@/lib/utils';
import React from 'react';

interface BoxWidgetProps {
  id: string;
  children?: React.ReactNode;
  color?: string | undefined;
  borderRadius: BorderRadius;
  borderThickness: string;
  borderStyle: BorderStyle;
  padding?: string;
  margin?: string;
  width?: string;
  height?: string;
  contentAlign: Align;
}

export const BoxWidget: React.FC<BoxWidgetProps> = ({
  children,
  width,
  height,
  borderStyle,
  borderRadius,
  borderThickness,
  color,
  padding,
  margin,
  contentAlign,
}) => {
  const styles = {
    ...getBorderStyle(borderStyle),
    ...getBorderThickness(borderThickness),
    ...getBorderRadius(borderRadius),
    ...getPadding(padding),
    ...getMargin(margin),
    ...getAlign('Vertical', contentAlign),
    ...getWidth(width),
    ...getHeight(height),
    ...(color
      ? {
          backgroundColor: `var(--${color}-light)`,
          borderColor: `var(--${color}-dark)`,
          color: `var(--${color}-foreground)`,
        }
      : {}),
  };

  return (
    <>
      <div style={styles} className={cn('font-mono', 'font-bold')}>
        {children}
      </div>
    </>
  );
};
