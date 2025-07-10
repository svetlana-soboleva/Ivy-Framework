import { useEventHandler } from '@/components/EventHandlerContext';
import { InvalidIcon } from '@/components/InvalidIcon';
import { inputStyles } from '@/lib/styles';
import { Input } from '@/components/ui/input';
import React, { useState, useEffect } from 'react';

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
  const [displayValue, setDisplayValue] = useState(value);
  const [inputValue, setInputValue] = useState(value);

  useEffect(() => {
    setDisplayValue(value);
    setInputValue(value);
  }, [value]);

  const handleColorChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    setDisplayValue(newValue);
    setInputValue(newValue);
    eventHandler('OnChange', id, [newValue]);
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    setInputValue(newValue);
  };

  const handleInputBlur = () => {
    const convertedValue = convertToHex(inputValue);
    setDisplayValue(convertedValue);
    setInputValue(convertedValue);
    eventHandler('OnChange', id, [convertedValue]);
    eventHandler('OnBlur', id, [convertedValue]);
  };

  const handleInputKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      handleInputBlur();
    }
  };

  const convertToHex = (colorValue: string): string => {
    if (!colorValue) return '';

    // If it's already a hex color, return as is
    if (colorValue.startsWith('#')) {
      return colorValue;
    }

    // Try to parse RGB format
    const rgbMatch = colorValue.match(/rgb\((\d+),\s*(\d+),\s*(\d+)\)/);
    if (rgbMatch) {
      const r = parseInt(rgbMatch[1]);
      const g = parseInt(rgbMatch[2]);
      const b = parseInt(rgbMatch[3]);
      return `#${r.toString(16).padStart(2, '0')}${g.toString(16).padStart(2, '0')}${b.toString(16).padStart(2, '0')}`;
    }

    // Try to parse OKLCH format (simplified conversion)
    const oklchMatch = colorValue.match(
      /oklch\(([^,]+),\s*([^,]+),\s*([^)]+)\)/
    );
    if (oklchMatch) {
      // For demo purposes, convert to a simple hex
      // In a real implementation, you'd want proper OKLCH to RGB conversion
      const hash = Math.abs(
        colorValue.split('').reduce((a, b) => a + b.charCodeAt(0), 0)
      );
      return `#${(hash % 0xffffff).toString(16).padStart(6, '0')}`;
    }

    // If it's a named color, try to convert
    const namedColors: Record<string, string> = {
      red: '#ff0000',
      green: '#00ff00',
      blue: '#0000ff',
      black: '#000000',
      white: '#ffffff',
      gray: '#808080',
      grey: '#808080',
      yellow: '#ffff00',
      cyan: '#00ffff',
      magenta: '#ff00ff',
      orange: '#ffa500',
      purple: '#800080',
      pink: '#ffc0cb',
      brown: '#a52a2a',
      lime: '#00ff00',
      navy: '#000080',
      teal: '#008080',
      silver: '#c0c0c0',
      gold: '#ffd700',
      maroon: '#800000',
      olive: '#808000',
    };

    const lowerValue = colorValue.toLowerCase();
    if (namedColors[lowerValue]) {
      return namedColors[lowerValue];
    }

    // If we can't parse it, return the original value
    return colorValue;
  };

  const getDisplayColor = (): string => {
    if (!displayValue) return '#000000';

    // Try to convert to hex for display
    const hexValue = convertToHex(displayValue);
    return hexValue.startsWith('#') ? hexValue : '#000000';
  };

  return (
    <div className="flex items-center space-x-2">
      <div className="relative">
        <input
          type="color"
          value={getDisplayColor()}
          onChange={handleColorChange}
          disabled={disabled}
          className={`w-10 h-10 p-1 rounded-lg border cursor-pointer ${
            disabled ? 'opacity-50 cursor-not-allowed' : ''
          } ${invalid ? inputStyles.invalidInput : 'border-gray-300'}`}
        />
      </div>
      <div className="relative">
        <Input
          type="text"
          value={inputValue}
          onChange={handleInputChange}
          onBlur={handleInputBlur}
          onKeyDown={handleInputKeyDown}
          placeholder={placeholder || 'Enter color'}
          disabled={disabled}
          className={`${invalid ? inputStyles.invalidInput + ' pr-8' : ''}`}
        />
        {invalid && (
          <div className="absolute top-1/2 -translate-y-1/2 flex items-center right-2">
            <span className="flex items-center">
              <InvalidIcon message={invalid} />
            </span>
          </div>
        )}
      </div>
      {displayValue && (
        <div className="text-xs text-gray-500 min-w-0">{displayValue}</div>
      )}
    </div>
  );
};
