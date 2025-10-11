import {
  CartesianGridProps,
  ChartType,
  LegendProps,
  LinesProps,
  XAxisProps,
  YAxisProps,
} from './chartTypes';
import { LineChartData } from './LineChartWidget';
import { ColorScheme, ExtendedTooltipProps } from './shared';

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
  if (data.length === 0) {
    return { categoryKey: '', categories: [], valueKeys: [] };
  }
  const categoryKey = Object.keys(data[0]).find(
    k => typeof data[0][k] === 'string'
  );
  if (!categoryKey) {
    return { categoryKey: '', categories: [], valueKeys: [] };
  }
  const categories = data.map(d => d[categoryKey]);
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
  const defaultGrid = {
    show: true,
    left: 30,
    right: 10,
    top: 30,
    bottom: 30,
    containLabel: true,
  };

  if (!cartesianGrid) return defaultGrid;

  return {
    show: cartesianGrid.vertical || cartesianGrid.horizontal,
    ...(cartesianGrid.width != null && { width: cartesianGrid.width }),
    ...(cartesianGrid.height != null && { height: cartesianGrid.height }),
    ...(cartesianGrid.x != null && { x: cartesianGrid.x }),
    ...(cartesianGrid.y != null && { y: cartesianGrid.y }),
    defaultGrid,
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
    type: 'scroll',
  };
}

export const getTransformValueFn = (data: LineChartData[]) => {
  const allValues = data.flatMap(d =>
    Object.values(d).filter(v => typeof v === 'number')
  ) as number[];
  const minValue = Math.min(...allValues);
  const maxValue = Math.max(...allValues);
  const largeSpread = Math.abs(maxValue / (minValue || 1)) > 1e5;

  const transform = (v: number) => {
    if (!largeSpread) return v;
    const sign = Math.sign(v);
    return sign * Math.log10(Math.abs(v) + 1);
  };

  return { transform, largeSpread, minValue, maxValue };
};

export const generateSeries = (
  data: LineChartData[],
  valueKeys: string[],
  lines?: LinesProps[],
  transform?: (v: number) => number
) => {
  return valueKeys.map(key => ({
    name: key,
    type: ChartType.Line,
    data: data.map(d =>
      transform ? transform(Number(d[key] ?? 0)) : Number(d[key] ?? 0)
    ),
    step: lines?.find(l => l.curveType === 'Step') ? 'middle' : false,
    smooth: lines?.find(l => l.curveType === 'Natural') ? true : false,
    showSymbol: true,
    symbolSize: 6,
    lineStyle: { width: 2, opacity: 0.9 },
    emphasis: {
      focus: 'self',
      lineStyle: { width: 3, opacity: 1 },
      itemStyle: { borderWidth: 2, borderColor: '#fff' },
    },
    blur: { lineStyle: { opacity: 0.6 } },
    animation: true,
    animationDuration: 800,
  }));
};

export const generateXAxis = (categories: string[], xAxis?: XAxisProps[]) => ({
  position: xAxis?.[0]?.orientation?.toLowerCase() === 'top' ? 'top' : 'bottom',
  type: 'category',
  data: categories,
  axisLabel: {
    formatter: (value: string) =>
      value.length > 10 ? value.match(/.{1,10}/g)?.join('\n') : value,
    interval: 'auto',
  },
});

export const generateYAxis = (
  largeSpread: boolean,
  transformValue: (v: number) => number,
  minValue: number,
  maxValue: number,
  yAxis?: YAxisProps[]
) => ({
  axisLabel: {
    formatter: (value: number) => {
      if (largeSpread) {
        const unscaled = Math.sign(value) * (10 ** Math.abs(value) - 1);
        if (Math.abs(unscaled) >= 1e9) return (unscaled / 1e9).toFixed(0) + 'B';
        if (Math.abs(unscaled) >= 1e6) return (unscaled / 1e6).toFixed(0) + 'M';
        if (Math.abs(unscaled) >= 1e3) return (unscaled / 1e3).toFixed(0) + 'K';
        return unscaled.toFixed(0);
      }
      if (Math.abs(value) >= 1e9) return (value / 1e9).toFixed(0) + 'B';
      if (Math.abs(value) >= 1e6) return (value / 1e6).toFixed(0) + 'M';
      if (Math.abs(value) >= 1e3) return (value / 1e3).toFixed(0) + 'K';
      return value;
    },
  },
  min: largeSpread ? transformValue(minValue) : 'dataMin',
  max: largeSpread ? transformValue(maxValue) : 'dataMax',
  position:
    yAxis?.[0]?.orientation?.toLowerCase() === 'right' ? 'right' : 'left',
});

export const generateTooltip = (tooltip?: ExtendedTooltipProps) => ({
  trigger: 'axis',
  axisPointer: {
    type: 'shadow',
    animated: tooltip?.animated ?? true,
    shadowStyle: { opacity: 0.5 },
  },
});
