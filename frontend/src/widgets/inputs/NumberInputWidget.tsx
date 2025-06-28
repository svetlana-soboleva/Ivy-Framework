import { memo, useCallback, useMemo } from 'react';
import { useEventHandler, EventHandler } from "@/components/EventHandlerContext";
import NumberInput from "@/components/NumberInput";
import { Slider } from '@/components/ui/slider';
import { cn } from '@/lib/utils';
import { inputStyles } from '@/lib/styles';
import { InvalidIcon } from '@/components/InvalidIcon';

const formatStyleMap = {
  Decimal: 'decimal',
  Currency: 'currency',
  Percent: 'percent'
} as const;

type FormatStyle = keyof typeof formatStyleMap;

interface NumberInputBaseProps {
  id: string;
  placeholder?: string;
  value: number;
  formatStyle?: FormatStyle;
  min?: number;
  max?: number;
  step?: number;
  precision?: number;
  disabled?: boolean;
  invalid?: string;
  onValueChange: (value: number) => void;
  currency?: string | undefined;
}

interface NumberInputWidgetProps extends Omit<NumberInputBaseProps, 'onValueChange'> {
  variant?: "Default" | "Slider";
}

const SliderVariant = memo(({
  value,
  min = 0,
  max = 100,
  step = 1,
  disabled = false,
  invalid,
  onValueChange,
}: NumberInputBaseProps) => {
  const handleSliderChange = useCallback((values: number[]) => {
    const newValue = values[0];
    if (typeof newValue === 'number') {
      onValueChange(newValue);
    }
  }, [onValueChange]);

  return (
    <div className="relative w-full mt-8">
      <Slider
        min={min}
        max={max}
        step={step}
        defaultValue={[value]}
        disabled={disabled}
        onValueChange={handleSliderChange}
        className={cn(invalid && inputStyles.invalid)}
      />
      <span
          className="mt-4 flex w-full items-center justify-between gap-1 text-xs font-sm text-muted-foreground"
          aria-hidden="true"
        >
          <span>{min}</span>
          <span>{max}</span>
        </span>
      {invalid && (
        <div className="absolute right-2.5 top-2.5">
          <InvalidIcon message={invalid} />
        </div>
      )}
    </div>
  );
});

SliderVariant.displayName = 'SliderVariant';

const NumberVariant = memo(({
  placeholder = "",
  value,
  min = 0,
  max = 100,
  step = 1,
  formatStyle = 'Decimal',
  precision = 2,
  disabled = false,
  invalid,
  onValueChange,
  currency
}: NumberInputBaseProps) => {
  const formatConfig = useMemo(() => ({
    style: formatStyleMap[formatStyle],
    minimumFractionDigits: 0,
    maximumFractionDigits: precision,
    useGrouping: true,
    notation: 'standard' as const,
    currency: currency || undefined
  }), [formatStyle, precision]);

  const handleNumberChange = useCallback((newValue: number | null) => {
    if (newValue !== null) {
      onValueChange(newValue);
    }
  }, [onValueChange]);

  return (
    <div className="relative">
      <NumberInput 
        min={min}
        max={max}
        step={step}
        format={formatConfig}
        placeholder={placeholder}
        value={value}
        disabled={disabled}
        onChange={handleNumberChange}
        className={cn(
          invalid && inputStyles.invalid,
          invalid && "pr-8"
        )}
      />
      {invalid && (
        <div className="absolute right-8 top-2">
          <InvalidIcon message={invalid} />
        </div>
      )}
    </div>
  );
});

NumberVariant.displayName = 'NumberVariant';

export const NumberInputWidget = memo(({ 
  id, 
  variant = "Default",
  ...props
}: NumberInputWidgetProps) => {
  const eventHandler = useEventHandler() as EventHandler;
   
  const handleChange = useCallback((newValue: number) => {
    const boundedValue = Math.min(Math.max(newValue, props.min ?? 0), props.max ?? 100);
    eventHandler("OnChange", id, [boundedValue]);
  }, [eventHandler, id, props.min, props.max]);

  return variant === "Slider" ? (
    <SliderVariant id={id} {...props} onValueChange={handleChange} />
  ) : (
    <NumberVariant id={id} {...props} onValueChange={handleChange} />
  );
});

NumberInputWidget.displayName = 'NumberInputWidget';