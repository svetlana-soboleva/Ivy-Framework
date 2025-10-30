import { getColor, getOverflow, getWidth, Overflow } from '@/lib/styles';
import { cn } from '@/lib/utils';
import React from 'react';
import { textBlockClassMap } from '../../lib/textBlockClassMap';
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from '@/components/ui/tooltip';

type TextBlockVariant =
  | 'Literal'
  | 'H1'
  | 'H2'
  | 'H3'
  | 'H4'
  | 'P'
  | 'Inline'
  | 'Block'
  | 'Blockquote'
  | 'InlineCode'
  | 'Lead'
  | 'Large'
  | 'Small'
  | 'Muted'
  | 'Danger'
  | 'Warning'
  | 'Success'
  | 'Label'
  | 'Strong';

interface TextBlockWidgetProps {
  content: string;
  variant: TextBlockVariant;
  width?: string;
  strikeThrough?: boolean;
  color: string;
  noWrap?: boolean;
  overflow?: Overflow;
  bold?: boolean;
  italic?: boolean;
  muted?: boolean;
}

interface VariantMap {
  [key: string]: React.FC<{
    children: string;
    className?: string;
    style?: React.CSSProperties;
  }>;
}
const variantMap: VariantMap = {
  Literal: ({ children, className, style }) => (
    <span className={className} style={style}>
      {children}
    </span>
  ),
  H1: ({ children, className, style }) => (
    <h1 className={cn(textBlockClassMap.H1, className)} style={style}>
      {children}
    </h1>
  ),
  H2: ({ children, className, style }) => (
    <h2 className={cn(textBlockClassMap.H2, className)} style={style}>
      {children}
    </h2>
  ),
  H3: ({ children, className, style }) => (
    <h3 className={cn(textBlockClassMap.H3, className)} style={style}>
      {children}
    </h3>
  ),
  H4: ({ children, className, style }) => (
    <h4 className={cn(textBlockClassMap.H4, className)} style={style}>
      {children}
    </h4>
  ),
  Block: ({ children, className, style }) => {
    const spanRef = React.useRef<HTMLSpanElement>(null);
    const [isTruncated, setIsTruncated] = React.useState(false);
    const [showTooltip, setShowTooltip] = React.useState(false);
    React.useEffect(() => {
      const checkTruncation = () => {
        const el = spanRef.current;
        if (el) {
          setIsTruncated(el.scrollWidth > el.clientWidth);
        }
      };
      checkTruncation();
      // Optionally, listen for window resize to re-check truncation
      window.addEventListener('resize', checkTruncation);
      return () => {
        window.removeEventListener('resize', checkTruncation);
      };
    }, [children, style]);
    return (
      <div className={cn('flex items-center min-w-0', className)} style={style}>
        <TooltipProvider>
          <Tooltip>
            <TooltipTrigger asChild>
              <span
                ref={spanRef}
                className="overflow-hidden text-ellipsis"
                onMouseEnter={() => setShowTooltip(true)}
                onMouseLeave={() => setShowTooltip(false)}
              >
                {children}
              </span>
            </TooltipTrigger>
            {showTooltip && isTruncated && typeof children === 'string' && (
              <TooltipContent className="bg-popover text-popover-foreground shadow-md">
                {children}
              </TooltipContent>
            )}
          </Tooltip>
        </TooltipProvider>
      </div>
    );
  },
  P: ({ children, className, style }) => (
    <p className={cn(textBlockClassMap.P, className)} style={style}>
      {children}
    </p>
  ),
  Inline: ({ children, className, style }) => (
    <span className={cn(className)} style={style}>
      {children}
    </span>
  ),
  Blockquote: ({ children, className, style }) => (
    <blockquote
      className={cn(textBlockClassMap.Blockquote, className)}
      style={style}
    >
      {children}
    </blockquote>
  ),
  InlineCode: ({ children, className, style }) => (
    <code className={cn(textBlockClassMap.InlineCode, className)} style={style}>
      {children}
    </code>
  ),
  Lead: ({ children, className, style }) => (
    <p className={cn(textBlockClassMap.Lead, className)} style={style}>
      {children}
    </p>
  ),
  Large: ({ children, className, style }) => (
    <div
      className={cn('text-semi-lead font-semibold', className)}
      style={style}
    >
      {children}
    </div>
  ),
  Small: ({ children, className, style }) => (
    <div
      className={cn('text-large-body font-medium leading-none', className)}
      style={style}
    >
      {children}
    </div>
  ),
  Muted: ({ children, className, style }) => (
    <div
      className={cn('text-sm text-muted-foreground', className)}
      style={style}
    >
      {children}
    </div>
  ),
  Danger: ({ children, className, style }) => (
    <div
      className={cn(
        'text-large-body text-destructive font-semibold',
        className
      )}
      style={style}
    >
      {children}
    </div>
  ),
  Warning: ({ children, className, style }) => (
    <div
      className={cn('text-large-body text-amber font-semibold', className)}
      style={style}
    >
      {children}
    </div>
  ),
  Success: ({ children, className, style }) => (
    <div
      className={cn('text-large-body text-green font-semibold', className)}
      style={style}
    >
      {children}
    </div>
  ),
  Label: ({ children, className, style }) => (
    <div
      className={cn(
        'text-large-label font-medium leading-none flex items-center',
        className
      )}
      style={style}
    >
      {children}
    </div>
  ),
  Strong: ({ children, className, style }) => (
    <strong className={cn('font-semibold', className)} style={style}>
      {children}
    </strong>
  ),
};

export const TextBlockWidget: React.FC<TextBlockWidgetProps> = ({
  content,
  variant,
  width,
  color,
  strikeThrough,
  noWrap,
  overflow,
  bold,
  italic,
  muted,
}) => {
  const styles: React.CSSProperties = {
    ...getWidth(width),
    ...getColor(color, 'color', 'background'),
    ...getOverflow(overflow),
  };

  const Component = variantMap[variant];
  return (
    <Component
      style={styles}
      className={cn(
        strikeThrough && 'line-through',
        noWrap && 'whitespace-nowrap',
        bold && 'font-semibold',
        italic && 'italic',
        muted && 'text-muted-foreground'
      )}
    >
      {content}
    </Component>
  );
};
