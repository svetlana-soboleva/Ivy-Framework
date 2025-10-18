import Icon from '@/components/Icon';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import {
  getHeight,
  getWidth,
  getBorderRadius,
  getBorderStyle,
  getBorderThickness,
  getColor,
  BorderRadius,
  BorderStyle,
} from '@/lib/styles';
import { cn } from '@/lib/utils';
import { useEventHandler } from '@/components/event-handler';
import React, { useCallback } from 'react';
import { EmptyWidget } from './primitives/EmptyWidget';

interface CardWidgetProps {
  id: string;
  events: string[];
  title?: string;
  description?: string;
  icon?: string;
  width?: string;
  height?: string;
  borderThickness?: string;
  borderRadius?: BorderRadius;
  borderStyle?: BorderStyle;
  borderColor?: string;
  hoverVariant?: 'None' | 'Pointer' | 'PointerAndTranslate';
  'data-testid'?: string;
  slots?: {
    Content?: React.ReactNode[];
    Footer?: React.ReactNode[];
  };
}

export const CardWidget: React.FC<CardWidgetProps> = ({
  id,
  events,
  title,
  description,
  icon,
  width,
  height,
  borderThickness,
  borderRadius,
  borderStyle,
  borderColor,
  hoverVariant,
  slots,
  'data-testid': testId,
}) => {
  const eventHandler = useEventHandler();

  const styles = {
    ...getWidth(width),
    ...getHeight(height),
    ...(borderStyle && getBorderStyle(borderStyle)),
    ...(borderThickness && getBorderThickness(borderThickness)),
    ...(borderRadius && getBorderRadius(borderRadius)),
    ...(borderColor && getColor(borderColor, 'borderColor', 'background')),
  };

  const footerIsEmpty =
    slots?.Footer?.length === 0 ||
    slots?.Footer?.some(
      node => React.isValidElement(node) && node.type === EmptyWidget
    );

  const headerIsEmpty = !title && !description && !icon;

  const handleClick = useCallback(() => {
    if (events.includes('OnClick')) eventHandler('OnClick', id, []);
  }, [id, eventHandler, events]);

  const hoverClass =
    hoverVariant === 'None'
      ? null
      : hoverVariant === 'Pointer'
        ? 'cursor-pointer'
        : 'cursor-pointer transform hover:-translate-x-[4px] hover:-translate-y-[4px] active:translate-x-[-2px] active:translate-y-[-2px] transition';

  return (
    <Card
      role="region"
      data-testid={testId}
      style={styles}
      className={cn('flex', 'flex-col', 'overflow-hidden', hoverClass)}
      onClick={handleClick}
    >
      {!headerIsEmpty ? (
        <CardHeader className="flex flex-row items-center justify-between gap-4">
          <div className="flex flex-col">
            {title && <CardTitle>{title}</CardTitle>}
            {description && (
              <CardDescription className="mt-2">{description}</CardDescription>
            )}
          </div>
          {icon && (
            <Icon name={icon} className="h-5 w-5 text-muted-foreground" />
          )}
        </CardHeader>
      ) : (
        <></>
      )}
      <CardContent className={cn('flex-1', headerIsEmpty && 'pt-6')}>
        {slots?.Content}
      </CardContent>
      {!footerIsEmpty && (
        <CardFooter className="flex-none">{slots?.Footer}</CardFooter>
      )}
    </Card>
  );
};
