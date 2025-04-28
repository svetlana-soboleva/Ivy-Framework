import { ThumbsUp, ThumbsDown } from 'lucide-react';
import { cn } from "@/lib/utils";

interface ThumbsRatingProps {
  value?: number;
  onRate?: (rating: number) => void;
  size?: 'sm' | 'md' | 'lg';
  className?: string;
  disabled?: boolean;
}

const ThumbsRating = ({
  value = 0,
  onRate,
  size = 'sm',
  className,
  disabled = false,
}: ThumbsRatingProps) => {
  const iconSizes = {
    sm: 16,
    md: 24,
    lg: 32
  };

  const handleClick = (rating: number) => {
    if (disabled) return;
    onRate?.(value === rating ? 0 : rating);
  };

  return (
    <div className={cn("flex gap-1 items-center", disabled && "opacity-50", className)}>
      <button
        onClick={() => handleClick(1)}
        disabled={disabled}
        className={cn(
          "p-2 rounded-full transition-all",
          "hover:bg-red-100 dark:hover:bg-red-900/30",
          "focus-visible:outline-none focus-visible:ring-2",
          "focus-visible:ring-ring focus-visible:ring-offset-2",
          value === 1 && "bg-red-100 dark:bg-red-900/30",
          disabled && "cursor-not-allowed hover:bg-transparent dark:hover:bg-transparent"
        )}
      >
        <ThumbsDown
          size={iconSizes[size]}
          className={cn(
            "transition-colors",
            value === 1 ? "text-red-600 dark:text-red-400" : "text-muted-foreground"
          )}
        />
      </button>

      <button
        onClick={() => handleClick(2)}
        disabled={disabled}
        className={cn(
          "p-2 rounded-full transition-all",
          "hover:bg-green-100 dark:hover:bg-green-900/30",
          "focus-visible:outline-none focus-visible:ring-2",
          "focus-visible:ring-ring focus-visible:ring-offset-2",
          value === 2 && "bg-green-100 dark:bg-green-900/30",
          disabled && "cursor-not-allowed hover:bg-transparent dark:hover:bg-transparent"
        )}
      >
        <ThumbsUp
          size={iconSizes[size]}
          className={cn(
            "transition-colors",
            value === 2 ? "text-green-600 dark:text-green-400" : "text-muted-foreground"
          )}
        />
      </button>
    </div>
  );
};

export default ThumbsRating;