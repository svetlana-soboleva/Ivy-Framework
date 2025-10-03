import { CartesianGridProps, LegendProps } from './chartTypes';
import { ColorScheme } from './shared';

const defaultColors = ['chart-1', 'chart-2', 'chart-3', 'chart-4', 'chart-5'];
const rainbowColors = [
  'blue',
  'cyan',
  'yellow',
  'red',
  'orange',
  'purple',
  'lime',
  'indigo',
  'rose',
  'green',
  'pink',
  'teal',
  'amber',
  'violet',
  'emerald',
  'fuchsia',
  'sky',
];

export const getColors = (scheme: ColorScheme): string[] => {
  switch (scheme) {
    case 'Default':
      return defaultColors.map(name =>
        getComputedStyle(document.documentElement)
          .getPropertyValue(`--${name}`)
          .trim()
      );
    case 'Rainbow':
      return rainbowColors;
    default:
      return [];
  }
};

export const generateDataProps = (data: Record<string, unknown>[]) => {
  const categoryKey = Object.keys(data[0]).find(
    k => typeof data[0][k] === 'string'
  )!;
  const categories = data.map(d => d[categoryKey.toLowerCase()]);
  const valueKeys = Object.keys(data[0]).filter(
    k => typeof data[0][k] === 'number'
  );
  return {
    categoryKey,
    categories,
    valueKeys,
  };
};

export function generateEChartGrid(cartesianGrid?: CartesianGridProps) {
  if (!cartesianGrid) return {};

  return {
    show: cartesianGrid.vertical || cartesianGrid.horizontal,
    ...(cartesianGrid.width != null && { width: cartesianGrid.width }),
    ...(cartesianGrid.height != null && { height: cartesianGrid.height }),
    ...(cartesianGrid.x != null && { x: cartesianGrid.x }),
    ...(cartesianGrid.y != null && { y: cartesianGrid.y }),
    left: 5,
    right: 5,
    top: 35,
    bottom: 30,
    containLabel: true,
  };
}

export function generateEChartLegend(legend?: LegendProps) {
  if (!legend) return { show: true };

  return {
    show: true,
    icon: legend.iconType ? legend.iconType : 'rect',
    itemWidth: legend.iconSize ?? 14,
    itemHeight: legend.iconSize ?? 14,
    itemGap: 16,
    orient: legend.layout?.toLowerCase(),
    bottom: legend.verticalAlign?.toLowerCase() === 'bottom' ? 0 : undefined,
    top: legend.verticalAlign?.toLowerCase() === 'top' ? 0 : undefined,
  };
}
