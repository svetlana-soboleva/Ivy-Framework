import { Checkbox } from "@/components/ui/checkbox";

type NullableCheckboxProps = {
  id: string;
  disabled: boolean;
  checked: boolean | null
  onCheckedChange: (checked: boolean | null) => void;
  className?: string;
};

const NullableCheckbox = ({
  id,
  disabled,
  checked,
  onCheckedChange,
  className = ""
}: NullableCheckboxProps) => {
  const handleClick = () => {
    if (checked === null) {
      onCheckedChange(true);
    } else if (checked === true) {
      onCheckedChange(false);
    } else {
      onCheckedChange(null);
    }
  };

  return (
    <Checkbox 
      id={id}
      checked={checked === null ? "indeterminate" : checked}
      onClick={handleClick}
      disabled={disabled}
      className={`${checked === null ? "data-[state=checked]:bg-gray-500" : ""} ${className}`}
    />
  );
};

export default NullableCheckbox;