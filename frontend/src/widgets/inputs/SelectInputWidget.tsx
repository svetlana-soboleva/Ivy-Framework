import React from 'react';
import { useEventHandler, EventHandler } from '@/components/EventHandlerContext';
import { 
  Select, 
  SelectContent, 
  SelectGroup, 
  SelectItem, 
  SelectLabel, 
  SelectTrigger, 
  SelectValue 
} from '@/components/ui/select';
import {
  RadioGroup,
  RadioGroupItem
} from "@/components/ui/radio-group";
import {
  ToggleGroup,
  ToggleGroupItem
} from "@/components/ui/toggle-group";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { InvalidIcon } from '@/components/InvalidIcon';
import { cn } from '@/lib/utils';
import { inputStyles } from '@/lib/styles';

export type NullableSelectValue = string | number | string[] | number[] | null | undefined;

interface Option {
  value: string | number;
  label: string;
  group?: string;
}

interface SelectInputWidgetProps {
  id: string;
  placeholder?: string;
  value?: NullableSelectValue;
  variant?: "Select" | "List" | "Toggle";
  nullable?: boolean;
  disabled?: boolean;
  invalid?: string;
  options: Option[];
  eventHandler: EventHandler;
  selectMany: boolean;
  separator: string;
}

const ToggleVariant: React.FC<SelectInputWidgetProps> = ({
  id,
  value,
  disabled = false,
  invalid,
  options = [],
  eventHandler
}) => {
  // Filter out any options with null/undefined/empty values
  const validOptions = options.filter(option => 
    option.value != null && option.value.toString().trim() !== ''
  );

  // Only convert value to string if it exists and isn't empty
  const stringValue = value != null && value.toString().trim() !== '' 
    ? value.toString() 
    : undefined;

  return (
    <div className="flex items-center gap-2">
      <div className="flex-1">
        <ToggleGroup
          type="single"
          value={stringValue}
          onValueChange={(newValue: string) => eventHandler("OnChange", id, [newValue])}
          disabled={disabled}
          className="flex flex-wrap gap-2"
        >
          {validOptions.map((option) => (
            <ToggleGroupItem
              key={option.value}
              value={option.value.toString()}
              aria-label={option.label}
              className={cn(
                "px-3 py-2",
                invalid && stringValue === option.value.toString() && inputStyles.invalid
              )}
            >
              {option.label}
            </ToggleGroupItem>
          ))}
        </ToggleGroup>
      </div>
      {invalid && (
        <InvalidIcon message={invalid} className="flex-shrink-0" />
      )}
    </div>
  );
};

const RadioVariant: React.FC<SelectInputWidgetProps> = ({
  id,
  value,
  disabled = false,
  invalid,
  options = [],
  eventHandler
}) => {
  const validOptions = options.filter(option => 
    option.value != null && option.value.toString().trim() !== ''
  );

  const stringValue = value != null && value.toString().trim() !== '' 
    ? value.toString() 
    : undefined;

  return (
    <div className="flex items-center gap-2">
      <div className="flex-1">
        <RadioGroup
          value={stringValue}
          onValueChange={(newValue) => eventHandler("OnChange", id, [newValue])}
          disabled={disabled}
          className="flex flex-col space-y-2"
        >
          {validOptions.map((option) => (
            <div key={option.value} className="flex items-center space-x-2">
              <RadioGroupItem 
                value={option.value.toString()} 
                id={`${id}-${option.value}`}
                className={cn(
                  invalid && stringValue === option.value.toString() && inputStyles.invalid
                )}
              />
              <Label 
                htmlFor={`${id}-${option.value}`}
                className={cn(
                  invalid && stringValue === option.value.toString() && inputStyles.invalid
                )}
              >
                {option.label}
              </Label>
            </div>
          ))}
        </RadioGroup>
      </div>
      {invalid && (
        <InvalidIcon message={invalid} className="flex-shrink-0" />
      )}
    </div>
  );
};

const CheckboxVariant: React.FC<SelectInputWidgetProps> = ({
  id,
  value,
  disabled = false,
  invalid,
  options = [],
  eventHandler,
  separator = ","
}) => {
  const validOptions = options.filter(option => 
    option.value != null && option.value.toString().trim() !== ''
  );

  // Handle array or string values
  let selectedValues: (string | number)[] = [];
  if (Array.isArray(value)) {
    selectedValues = value;
  } else if (value != null && value.toString().trim() !== '') {
    selectedValues = value.toString().split(separator).map(v => v.trim());
  }

  const handleCheckboxChange = (optionValue: string | number, checked: boolean) => {
    let newValues: (string | number)[];
    
    if (checked) {
      newValues = [...selectedValues, optionValue];
    } else {
      newValues = selectedValues.filter(v => v !== optionValue);
    }
    
    eventHandler("OnChange", id, [newValues]);
  };

  return (
    <div className="flex items-center gap-2">
      <div className="flex-1">
        <div className="flex flex-col space-y-2 gap-2">
          {validOptions.map((option) => (
            <div key={option.value} className="flex items-center space-x-2">
              <Checkbox 
                id={`${id}-${option.value}`}
                checked={selectedValues.includes(option.value)}
                onCheckedChange={(checked) => handleCheckboxChange(option.value, checked === true)}
                disabled={disabled}
                className={cn(
                  invalid && selectedValues.includes(option.value) && inputStyles.invalid
                )}
              />
              <Label 
                htmlFor={`${id}-${option.value}`}
                className={cn(
                  invalid && selectedValues.includes(option.value) && inputStyles.invalid
                )}
              >
                {option.label}
              </Label>
            </div>
          ))}
        </div>
      </div>
      {invalid && (
        <InvalidIcon message={invalid} className="flex-shrink-0" />
      )}
    </div>
  );
};

const SelectVariant: React.FC<SelectInputWidgetProps> = ({ 
  id,
  placeholder = "",
  value,
  disabled = false,
  invalid,
  options = [],
  eventHandler
}) => {

  const validOptions = options.filter(option => 
    option.value != null && option.value.toString().trim() !== ''
  );

  const groupedOptions = validOptions.reduce<Record<string, Option[]>>((acc, option) => {
    const key = option.group || "default";
    if (!acc[key]) {
      acc[key] = [];
    }
    acc[key].push(option);
    return acc;
  }, {});

  const stringValue = value != null && value.toString().trim() !== '' 
    ? value.toString() 
    : undefined;

  return (
    <div className="flex items-center gap-2">
      <div className="flex-1">
        <Select 
          key={id} 
          disabled={disabled}
          value={stringValue}
          onValueChange={(newValue) => eventHandler("OnChange", id, [newValue])}
        >
          <SelectTrigger className={cn(
            invalid && inputStyles.invalid
          )}>
            <SelectValue placeholder={placeholder} />
          </SelectTrigger>
          <SelectContent>
            {Object.entries(groupedOptions).map(([group, options]) => (
              <SelectGroup key={group}>
                {group !== "default" && <SelectLabel>{group}</SelectLabel>}
                {options.map((option) => (
                  <SelectItem 
                    key={option.value} 
                    value={option.value.toString()}
                  >
                    {option.label}
                  </SelectItem>
                ))}
              </SelectGroup>
            ))}
          </SelectContent>
        </Select>
      </div>
      {invalid && (
        <InvalidIcon message={invalid} className="flex-shrink-0" />
      )}
    </div>
  );
};

export const SelectInputWidget: React.FC<SelectInputWidgetProps> = (props) => {
  const eventHandler = useEventHandler();
  
  // Normalize undefined to null when nullable
  const normalizedProps = {
    ...props,
    value: props.nullable && props.value === undefined ? null : props.value
  };
  
  switch (normalizedProps.variant) {
    case "List":
      return normalizedProps.selectMany ? <CheckboxVariant {...normalizedProps} eventHandler={eventHandler} /> : <RadioVariant {...normalizedProps} eventHandler={eventHandler} />;
    case "Toggle":
      return <ToggleVariant {...normalizedProps} eventHandler={eventHandler} />;
    default:
      return <SelectVariant {...normalizedProps} eventHandler={eventHandler} />;
  }
};

export default SelectInputWidget;