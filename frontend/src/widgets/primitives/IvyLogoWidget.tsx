import IvyLogo from '@/components/IvyLogo';
import { getHeight, getWidth } from '@/lib/styles';

interface IvyLogoWidgetProps {
  width?: string;
  height?: string;
  color?: string;
}

export const IvyLogoWidget: React.FC<IvyLogoWidgetProps> = ({
  width,
  height,
  color,
}) => {
  const styles = {
    ...getWidth(width),
    ...getHeight(height),
    ...(color ? { color: `var(--${color})` } : { color: 'var(--primary)' }),
  };

  return <IvyLogo style={styles} className="text-primary" />;
};
