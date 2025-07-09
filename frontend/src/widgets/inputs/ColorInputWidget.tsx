import { useEventHandler } from '@/components/EventHandlerContext';
import { inputStyles } from '@/lib/styles';
import { cn } from '@/lib/utils';
import { InvalidIcon } from '@/components/InvalidIcon';
import React, { useState, useMemo } from 'react';

interface ColorInputWidgetProps {
  id: string;
  value: string;
  disabled?: boolean;
  invalid?: string;
  placeholder?: string;
}

export const ColorInputWidget: React.FC<ColorInputWidgetProps> = ({
  id,
  value,
  disabled = false,
  invalid,
  placeholder,
}) => {
  const eventHandler = useEventHandler();
  const [inputValue, setInputValue] = useState(value);
  const [isValid, setIsValid] = useState(true);

  // Convert value to hex for the color picker
  const hexValue = useMemo(() => {
    if (!value) return '#000000';

    // If it's already a hex color, use it
    if (/^#[0-9A-Fa-f]{6}$/.test(value)) {
      return value;
    }

    // If it's a Colors enum, convert to hex
    if (/^[A-Za-z]+$/.test(value)) {
      const colorMap: Record<string, string> = {
        Black: '#000000',
        White: '#ffffff',
        Slate: '#6a7489',
        Gray: '#6e727f',
        Zinc: '#717179',
        Neutral: '#737373',
        Stone: '#76716d',
        Red: '#dd5860',
        Orange: '#dc824d',
        Amber: '#deb145',
        Yellow: '#e5e04c',
        Lime: '#afd953',
        Green: '#86d26f',
        Emerald: '#76cd94',
        Teal: '#5b9ba8',
        Cyan: '#4469c0',
        Sky: '#373bda',
        Blue: '#381ff4',
        Indigo: '#4b28e2',
        Violet: '#6637d1',
        Purple: '#844cc0',
        Fuchsia: '#a361af',
        Pink: '#c377a0',
        Rose: '#e48e91',
        Primary: '#381ff4',
        Secondary: '#6e727f',
        Destructive: '#dd5860',
      };
      return colorMap[value] || '#000000';
    }

    // For rgb and oklch, return a default hex
    return '#000000';
  }, [value]);

  const validateColor = (colorStr: string): boolean => {
    if (!colorStr) return true;

    // Hex color validation
    if (/^#[0-9A-Fa-f]{6}$/.test(colorStr)) return true;

    // RGB color validation
    if (/^rgb\(\s*\d+\s*,\s*\d+\s*,\s*\d+\s*\)$/.test(colorStr)) return true;

    // OKLCH color validation
    if (/^oklch\(\s*[\d.]+\s+[\d.]+\s+[\d.]+\s*\)$/.test(colorStr)) return true;

    // Colors enum validation
    const colorEnums = [
      'Black',
      'White',
      'Slate',
      'Gray',
      'Zinc',
      'Neutral',
      'Stone',
      'Red',
      'Orange',
      'Amber',
      'Yellow',
      'Lime',
      'Green',
      'Emerald',
      'Teal',
      'Cyan',
      'Sky',
      'Blue',
      'Indigo',
      'Violet',
      'Purple',
      'Fuchsia',
      'Pink',
      'Rose',
      'Primary',
      'Secondary',
      'Destructive',
    ];
    if (colorEnums.includes(colorStr)) return true;

    return false;
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    setInputValue(newValue);

    const valid = validateColor(newValue);
    setIsValid(valid);

    if (valid) {
      eventHandler('OnChange', id, [newValue]);
    }
  };

  const handleColorPickerChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const hexColor = e.target.value;
    setInputValue(hexColor);
    setIsValid(true);
    eventHandler('OnChange', id, [hexColor]);
  };

  const handleBlur = () => {
    eventHandler('OnBlur', id, [inputValue]);
  };

  return (
    <div className="flex items-center space-x-2">
      <div className="relative">
        <input
          type="color"
          value={hexValue}
          onChange={handleColorPickerChange}
          disabled={disabled}
          className={cn(
            'w-10 h-10 p-1 border rounded-lg cursor-pointer',
            disabled && 'opacity-50 cursor-not-allowed',
            invalid && inputStyles.invalid
          )}
        />
      </div>
      <div className="relative flex-1">
        <input
          type="text"
          value={inputValue}
          onChange={handleInputChange}
          onBlur={handleBlur}
          disabled={disabled}
          placeholder={placeholder}
          className={cn(
            'w-full px-3 py-2 border rounded-md text-sm',
            'focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent',
            disabled && 'opacity-50 cursor-not-allowed bg-gray-100',
            invalid && inputStyles.invalid,
            !isValid && !invalid && 'border-red-300 focus:ring-red-500'
          )}
        />
        {invalid && (
          <div className="absolute right-3 top-1/2 transform -translate-y-1/2">
            <InvalidIcon message={invalid} />
          </div>
        )}
      </div>
    </div>
  );
};
