import React from 'react';
import { ColorScheme } from './sharedUtils';
import {
  generateDataProps,
  generateEChartGrid,
  generateEChartLegend,
  getColors,
} from './sharedUtils';
import { getHeight, getWidth } from '@/lib/styles';
import { StackOffsetType } from 'recharts/types/util/types';
import ReactECharts from 'echarts-for-react';
import {
  BarProps,
  CartesianGridProps,
  ChartType,
  LegendProps,
  MarkArea,
  MarkLine,
  ToolTipProps,
  XAxisProps,
  YAxisProps,
} from './chartTypes';

interface BarChartWidgetProps {
  id: string;
  data: Record<string, unknown>[];
  width?: string;
  height?: string;
  bars?: BarProps[];
  cartesianGrid?: CartesianGridProps;
  xAxis?: XAxisProps[];
  yAxis?: YAxisProps[];
  tooltip?: ToolTipProps;
  legend?: LegendProps;
  referenceLines?: MarkLine[];
  referenceAreas?: MarkArea[];
  referenceDots?: unknown;
  colorScheme: ColorScheme;
  stackOffset: StackOffsetType;
  barGap?: number;
  barCategoryGap?: number | string;
  maxBarSize?: number;
  reverseStackOrder?: boolean;
  layout?: 'horizontal' | 'vertical';
}

const BarChartWidget: React.FC<BarChartWidgetProps> = ({
  data,
  width,
  height,
  bars,
  cartesianGrid,
  xAxis,
  yAxis,
  tooltip,
  legend,
  //referenceLines,
  //referenceAreas,
  //referenceDots,
  colorScheme,
  //stackOffset,
  barGap,
  barCategoryGap,
  maxBarSize,
  reverseStackOrder,
  layout,
}) => {
  const styles: React.CSSProperties = {
    ...getWidth(width),
    ...getHeight(height),
    minHeight: 300,
  };

  const { categories, valueKeys } = generateDataProps(data);
  const colors = getColors(colorScheme);
  const series = valueKeys.map((key, i) => ({
    name: key,
    type: ChartType.Bar,
    data: data.map(d => d[key]),
    stack:
      bars && bars[i]?.stackId !== undefined
        ? String(bars[i].stackId)
        : undefined,
    barGap: barGap ? `${barGap}%` : '0%',
    barCategoryGap: barCategoryGap ? `${barCategoryGap}%` : '0%',
    maxBarSize: maxBarSize || undefined,
    stackOrder: reverseStackOrder ? 'seriesDesc' : 'seriesAsc',
  }));
  const isVertical = layout?.toLowerCase() === 'vertical';

  const xAxisOption = {
    type: isVertical ? 'value' : 'category',
    data: isVertical ? undefined : categories,
    position:
      xAxis?.[0]?.orientation?.toLowerCase() === 'top' ? 'top' : 'bottom',
  };

  const yAxisOption = {
    type: isVertical ? 'category' : 'value',
    data: isVertical ? categories : undefined,
    position:
      yAxis?.[0]?.orientation?.toLowerCase() === 'right' ? 'right' : 'left',
  };

  const option = {
    grid: generateEChartGrid(cartesianGrid),
    color: colors,
    xAxis: xAxisOption,
    yAxis: yAxisOption,
    toolbox: {
      feature: {
        saveAsImage: {},
      },
    },
    series,
    legend: generateEChartLegend(legend),
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        shadowStyle: {
          opacity: 0.5,
        },
        type: 'shadow',
        animated: tooltip?.animated ?? true,
      },
    },
  };

  return (
    <div style={styles}>
      <ReactECharts
        option={option}
        style={{ width: '100%', height: '100%', minHeight: 300 }}
      />
    </div>
  );
};

export default BarChartWidget;
