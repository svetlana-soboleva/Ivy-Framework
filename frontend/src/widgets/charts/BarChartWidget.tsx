import React from 'react';
import {
  ColorScheme,
  ExtendedBarProps,
  ExtendedTooltipProps,
  // generateBarProps,
  // generateLabelListProps,
} from './shared';
import {
  applyHorizontalStackRounding,
  generateDataProps,
  generateEChartGrid,
  generateEChartLegend,
  getColorGeneratorEChart,
} from './sharedUtils';
import {
  ExtendedXAxisProps,
  ExtendedYAxisProps,
  // generateXAxisProps,
  // generateYAxisProps,
  // generateLegendProps,
} from './shared';
import { getHeight, getWidth } from '@/lib/styles';
import { StackOffsetType } from 'recharts/types/util/types';
//import { camelCase } from 'lodash';
import ReactECharts from 'echarts-for-react';
import { CartesianGridProps, LegendProps } from './chartTypes';

interface BarChartWidgetProps {
  id: string;
  data: Record<string, unknown>[];
  width?: string;
  height?: string;
  bars?: ExtendedBarProps[];
  cartesianGrid?: CartesianGridProps;
  xAxis?: ExtendedXAxisProps[];
  yAxis?: ExtendedYAxisProps[];
  tooltip?: ExtendedTooltipProps;
  legend?: LegendProps;
  referenceLines?: unknown;
  referenceAreas?: unknown;
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
  //xAxis,
  //yAxis,
  tooltip,
  legend,
  // referenceLines,
  // referenceAreas,
  // referenceDots,
  colorScheme,
  //stackOffset,
  // barGap,
  // barCategoryGap,
  // maxBarSize,
  // reverseStackOrder,
  layout,
}) => {
  const styles: React.CSSProperties = {
    ...getWidth(width),
    ...getHeight(height),
    minHeight: 300,
  };
  const { categories, valueKeys } = generateDataProps(data);
  const allStackOne = bars?.every(b => b.stackId === 1);
  const hasDifferentStacks = bars && !allStackOne;
  const colors = getColorGeneratorEChart(
    colorScheme ?? 'Default',
    valueKeys.length
  );
  const series = valueKeys.map((key, i) => ({
    name: key,
    type: 'bar',
    data: data.map(d => d[key]),
    stack: hasDifferentStacks ? String(bars?.[i]?.stackId) : undefined,
  }));
  const isVertical = layout?.toLowerCase() === 'vertical';
  if (!isVertical) {
    applyHorizontalStackRounding(series);
  }
  const option = {
    grid: generateEChartGrid(cartesianGrid),
    color: colors,
    xAxis: isVertical
      ? { type: 'value' }
      : { type: 'category', data: categories },
    yAxis: isVertical
      ? { type: 'category', data: categories }
      : { type: 'value' },
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
