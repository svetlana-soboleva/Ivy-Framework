import React, { useCallback, useMemo } from 'react';
import { Checkbox } from "@/components/ui/checkbox";
import { Switch } from "@/components/ui/switch";
import { Label } from "@/components/ui/label";
import { Toggle } from "@/components/ui/toggle";
import Icon from '@/components/Icon';
import { useEventHandler } from '@/components/EventHandlerContext';
import NullableCheckbox from '@/components/NullableCheckbox';
import { InvalidIcon } from '@/components/InvalidIcon';
import { inputStyles } from '@/lib/styles';
import { cn } from '@/lib/utils';

type VariantType = 'Checkbox' | 'Switch' | 'Toggle';

interface BoolInputWidgetProps {
  id: string;
  label?: string;
  description?: string;
  value: boolean | null;
  disabled?: boolean;
  nullable?: boolean;
  invalid?: string;
  variant: VariantType;
  icon?: string;
}

interface BaseVariantProps {
  id: string;
  label?: string;
  description?: string;
  invalid?: string;
  value: boolean | null;
  disabled: boolean;
}

interface CheckboxVariantProps extends BaseVariantProps {
  nullable: boolean;
  onCheckedChange: (checked: boolean | null) => void;
}

interface SwitchVariantProps extends BaseVariantProps {
  onCheckedChange: (checked: boolean) => void;
}

interface ToggleVariantProps extends BaseVariantProps {
  icon?: string;
  onPressedChange: (pressed: boolean) => void;
}

const InputLabel: React.FC<{ id: string; label?: string; description?: string; invalid?: string }> = React.memo(
  ({ id, label, description, invalid }) => {
    if (!label && !description && !invalid) return null;

    return (
      <div className="grid gap-1.5 leading-none">
        <div className="flex items-center space-x-2">
          {label && (
            <Label
              htmlFor={id}
              className={cn(invalid && inputStyles.invalid)}
            >
              {label}
            </Label>
          )}
          {invalid && <InvalidIcon message={invalid} />}
        </div>
        {description && (
          <p className="text-sm text-muted-foreground">{description}</p>
        )}
      </div>
    );
  }
);

const VariantComponents = {
  Checkbox: React.memo(({
    id,
    label,
    description,
    value,
    disabled,
    nullable,
    invalid,
    onCheckedChange
  }: CheckboxVariantProps) => (
    <div className="flex items-start space-x-2" onClick={(e) => e.stopPropagation()}>
      {nullable ? (
        <NullableCheckbox
          id={id}
          checked={value}
          onCheckedChange={onCheckedChange}
          disabled={disabled}
        />
      ) : (
        <Checkbox
          id={id}
          checked={!!value}
          onCheckedChange={checked => onCheckedChange(checked ? true : false)}
          disabled={disabled}
        />
      )}
      <InputLabel id={id} label={label} description={description} invalid={invalid} />
    </div>
  )),

  Switch: React.memo(({
    id,
    label,
    description,
    value,
    disabled,
    invalid,
    onCheckedChange
  }: SwitchVariantProps) => (
    <div className="flex items-start space-x-2" onClick={(e) => e.stopPropagation()}>
      <Switch
        id={id}
        checked={!!value}
        onCheckedChange={onCheckedChange}
        disabled={disabled}
      />
      <InputLabel id={id} label={label} description={description} invalid={invalid} />
    </div>
  )),

  Toggle: React.memo(({
    id,
    label,
    description,
    value,
    disabled,
    icon,
    invalid,
    onPressedChange
  }: ToggleVariantProps) => (
    <div className="flex items-start space-x-2" onClick={(e) => e.stopPropagation()}>
      <Toggle
        id={id}
        pressed={!!value}
        onPressedChange={onPressedChange}
        disabled={disabled}
        aria-label={label}
      >
        {icon && <Icon className="h-4 w-4" name={icon} />}
      </Toggle>
      <InputLabel id={id} label={label} description={description} invalid={invalid} />
    </div>
  ))
};

export const BoolInputWidget: React.FC<BoolInputWidgetProps> = ({
  id,
  label,
  description,
  value,
  disabled = false,
  invalid,
  nullable = false,
  variant,
  icon
}) => {
  const eventHandler = useEventHandler();

  const handleChange = useCallback((newValue: boolean | null) => {
    if (disabled) return;
    eventHandler("OnChange", id, [newValue]);
  }, [disabled, eventHandler, id]);

  const VariantComponent = useMemo(() => VariantComponents[variant], [variant]);

  return (
    <VariantComponent
      id={id}
      label={label}
      description={description}
      value={value}
      disabled={disabled}
      nullable={nullable}
      icon={icon}
      invalid={invalid}
      onCheckedChange={handleChange}
      onPressedChange={handleChange}
    />
  );
};

export default React.memo(BoolInputWidget);