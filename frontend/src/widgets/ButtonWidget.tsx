import React, { useCallback } from 'react';
import { Button } from '@/components/ui/button';
import Icon from '@/components/Icon';
import { cn, getIvyHost, camelCase } from '@/lib/utils';
import { useEventHandler } from '@/components/event-handler';
import withTooltip from '@/hoc/withTooltip';
import { Loader2 } from 'lucide-react';
import {
  BorderRadius,
  getBorderRadius,
  getColor,
  getWidth,
} from '@/lib/styles';

interface ButtonWidgetProps {
  id: string;
  title: string;
  icon?: string;
  iconPosition?: 'Left' | 'Right';
  size?: 'Default' | 'Small' | 'Large';
  variant?:
    | 'Primary'
    | 'Inline'
    | 'Destructive'
    | 'Outline'
    | 'Secondary'
    | 'Ghost'
    | 'Link'
    | 'Inline';
  disabled: boolean;
  tooltip?: string;
  foreground?: string;
  loading?: boolean;
  url?: string;
  width?: string;
  children?: React.ReactNode;
  borderRadius?: BorderRadius;
  'data-testid'?: string;
}

const getUrl = (url: string) => {
  if (url.startsWith('http://') || url.startsWith('https://')) {
    return url;
  }
  return `${getIvyHost()}${url.startsWith('/') ? '' : '/'}${url}`;
};

export const ButtonWidget: React.FC<ButtonWidgetProps> = ({
  id,
  title,
  icon,
  iconPosition,
  variant,
  disabled,
  tooltip,
  foreground,
  url,
  loading,
  width,
  children,
  borderRadius,
  size,
  'data-testid': dataTestId,
}) => {
  const eventHandler = useEventHandler();

  const styles: React.CSSProperties = {
    ...getWidth(width),
    ...getColor(foreground),
    ...getBorderRadius(borderRadius),
  };

  let buttonSize: 'icon' | 'default' | 'sm' | 'lg' | null | undefined =
    'default';
  let iconSize: number = 4;

  if (icon && icon != 'None' && !title) {
    buttonSize = 'icon';
  }

  if (size == 'Small') {
    buttonSize = 'sm';
    iconSize = 3;
  }

  if (size == 'Large') {
    buttonSize = 'lg';
    iconSize = 5;
  }

  const iconStyles = {
    width: `${iconSize * 0.25}rem`,
    height: `${iconSize * 0.25}rem`,
  };

  const ButtonWithTooltip = withTooltip(Button);

  const handleClick = useCallback(() => {
    if (disabled) return;
    if (url) {
      window.open(getUrl(url), '_blank');
      return;
    }
    eventHandler('OnClick', id, []);
  }, [id, disabled, url, eventHandler]);

  const hasChildren = !!children;

  return (
    <ButtonWithTooltip
      style={styles}
      size={buttonSize}
      onClick={handleClick}
      variant={
        (variant === 'Primary' ? 'default' : camelCase(variant)) as
          | 'default'
          | 'destructive'
          | 'outline'
          | 'secondary'
          | 'ghost'
          | 'link'
          | 'inline'
      }
      disabled={disabled}
      className={cn(
        buttonSize !== 'icon' && 'w-min',
        hasChildren &&
          'p-2 h-auto items-start justify-start text-left inline-block',
        (variant === 'Link' || variant === 'Inline') &&
          'min-w-0 max-w-full overflow-hidden'
      )}
      tooltipText={
        tooltip ||
        ((variant === 'Link' || variant === 'Inline') && title
          ? title
          : undefined)
      }
      data-testid={dataTestId}
    >
      {!hasChildren && (
        <>
          {iconPosition == 'Left' && (
            <>
              {loading && (
                <Loader2 className="animate-spin" style={iconStyles} />
              )}
              {!loading && icon && icon != 'None' && (
                <Icon style={iconStyles} name={icon} />
              )}
            </>
          )}
          {variant === 'Link' || variant === 'Inline' ? (
            <span className="truncate">{title}</span>
          ) : (
            title
          )}
          {iconPosition == 'Right' && (
            <>
              {loading && (
                <Loader2 className="animate-spin" style={iconStyles} />
              )}
              {!loading && icon && icon != 'None' && (
                <Icon style={iconStyles} name={icon} />
              )}
            </>
          )}
        </>
      )}
      {children}
    </ButtonWithTooltip>
  );
};
