import { cva } from 'class-variance-authority';

// Size variants for BoolInput components
export const boolInputSizeVariants = {
  Small: 'text-xs',
  Medium: 'text-sm',
  Large: 'text-base',
};

// Label size variants
export const labelSizeVariants = cva(
  'text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70',
  {
    variants: {
      size: {
        Small: 'text-xs',
        Medium: 'text-sm',
        Large: 'text-base',
      },
    },
    defaultVariants: {
      size: 'Medium',
    },
  }
);

// Description size variants
export const descriptionSizeVariants = cva('text-muted-foreground', {
  variants: {
    size: {
      Small: 'text-xs',
      Medium: 'text-sm',
      Large: 'text-base',
    },
  },
  defaultVariants: {
    size: 'Medium',
  },
});
