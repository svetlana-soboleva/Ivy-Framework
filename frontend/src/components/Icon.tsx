import { cn } from '@/lib/utils';
import { Folder, icons } from 'lucide-react';

interface IconProps {
    name?: string;
    color?: string;
    size?: string | number;
    className?: string;
    style?: React.CSSProperties;
}

const Icon: React.FC<IconProps> = ({ name, color, size, className, style }) => {

   if(name === 'None') {
      return <Folder className="invisible" size={size}/>; //todo: hack to hide the icon  while taking the same space
   }

    if (!name || !(name in icons)) {
        return null;
    }

    const LucideIcon = icons[name as keyof typeof icons];

    return <LucideIcon style={style} color={color} size={size} className={cn(className)}/>;
};

export default Icon;