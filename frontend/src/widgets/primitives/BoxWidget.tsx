import {
  Align,
  BorderRadius,
  BorderStyle,
  getAlign,
  getBorderRadius,
  getBorderStyle,
  getBorderThickness,
  getColor,
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
  className?: string;
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
  className,
}) => {
  const isIvyDemoSurface = className?.includes('ivy-demo-surface');

  const styles: React.CSSProperties = {
    // Layout and spacing should always apply
    ...getPadding(padding),
    ...getMargin(margin),
    ...getAlign('Vertical', contentAlign),
    ...getWidth(width),
    ...getHeight(height),
    // Visual styling comes from props unless ivy-demo-surface is used
    ...(!isIvyDemoSurface && getBorderStyle(borderStyle)),
    ...(!isIvyDemoSurface && getBorderThickness(borderThickness)),
    ...(!isIvyDemoSurface && getBorderRadius(borderRadius)),
    ...(!isIvyDemoSurface && getColor(color, 'backgroundColor', 'background')),
    ...(!isIvyDemoSurface && getColor(color, 'borderColor', 'foreground')),
    ...(!isIvyDemoSurface && getColor(color, 'color', 'foreground')),
  };

  return (
    <>
      <div style={styles} className={cn(className)}>
        {children}
      </div>
    </>
  );
};
