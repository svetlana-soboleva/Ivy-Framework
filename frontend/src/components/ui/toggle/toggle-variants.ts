import { cva } from 'class-variance-authority';

export const toggleVariants = cva(
  'inline-flex items-center justify-center gap-2 rounded-md text-sm font-medium transition-colors hover:bg-muted hover:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring cursor-pointer disabled:cursor-not-allowed disabled:opacity-50 data-[state=on]:bg-primary data-[state=on]:text-primary-foreground [&_svg]:pointer-events-none [&_svg]:shrink-0',
  {
    variants: {
      variant: {
        default: 'bg-transparent',
        outline:
          'border border-input bg-transparent shadow-sm hover:bg-accent hover:text-accent-foreground',
      },
      size: {
        Small: 'h-8 px-1.5 min-w-8 [&_svg]:size-3',
        Medium: 'h-9 px-2 min-w-9 [&_svg]:size-4',
        Large: 'h-10 px-2.5 min-w-10 [&_svg]:size-6',
      },
    },
    defaultVariants: {
      variant: 'default',
      size: 'Medium',
    },
  }
);
