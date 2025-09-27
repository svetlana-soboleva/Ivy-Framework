import React from 'react';
// import {
//   ChartConfig,
//   ChartContainer,
//   ChartTooltip,
//   ChartTooltipContent,
// } from '@/components/ui/chart';
import {
  ColorScheme,
  ExtendedBarProps,
  ExtendedTooltipProps,
  // generateBarProps,
  // generateLabelListProps,
  getColorGeneratorEChart,
} from './shared';
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

interface BarChartWidgetProps {
  id: string;
  data: Record<string, unknown>[];
  width?: string;
  height?: string;
  bars?: ExtendedBarProps[];
  cartesianGrid?: unknown;
  xAxis?: ExtendedXAxisProps[];
  yAxis?: ExtendedYAxisProps[];
  tooltip?: ExtendedTooltipProps;
  legend?: unknown;
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
  // bars,
  // cartesianGrid,
  xAxis,
  yAxis,
  tooltip,
  // legend,
  // referenceLines,
  // referenceAreas,
  // referenceDots,
  colorScheme,
  // stackOffset,
  // barGap,
  // barCategoryGap,
  // maxBarSize,
  // reverseStackOrder,
  // layout,
}) => {
  const styles: React.CSSProperties = {
    ...getWidth(width),
    ...getHeight(height),
    minHeight: 300,
  };

  //const chartConfig = {} satisfies ChartConfig;
  const firstData = data[0] ?? {};
  const keys = Object.keys(data[0]);
  // Determine x-axis key
  const xDataKey = xAxis?.[0]?.dataKey ?? keys[0];

  // Determine y-axis key if provided (optional)
  const yDataKey = yAxis?.length
    ? yAxis.map(y => y.dataKey).filter(Boolean)
    : keys.slice(1);

  const categoryKey = keys[0]; // e.g., "month"

  // Build categories for x-axis (only for Category type)
  const categories = data.map(d => d[categoryKey.toLowerCase()]);

  // Series: only numeric keys, exclude xDataKey and yDataKey
  const valueKeys = Object.keys(firstData).filter(
    key =>
      key !== xDataKey &&
      !(Array.isArray(yDataKey) && yDataKey.includes(key)) &&
      typeof firstData[key] === 'number'
  );

  const series = valueKeys.map(key => ({
    name: key,
    type: 'bar',
    data: data.map(d => d[key]),
  }));

  // Colors
  const colors = getColorGeneratorEChart(
    colorScheme ?? 'Default',
    valueKeys.length
  );

  // ECharts option
  const option = {
    color: colors,
    xAxis: [
      {
        type: 'category',
        data: categories,
      },
    ],
    yAxis: [{ type: 'value' }],
    series,
    tooltip: tooltip
      ? { show: true, trigger: 'axis', animation: tooltip.animated ?? true }
      : undefined,
    legend: { show: true },
  };
  //log all the props

  // console.log('data:', JSON.stringify(data));
  // console.log('width:', String(width));
  // console.log('height:', String(height));
  //console.log('bars:', JSON.stringify(bars));
  //console.log('cartesianGrid:', JSON.stringify(cartesianGrid));
  // console.log('xAxis:', JSON.stringify(xAxis));
  // console.log('yAxis:', JSON.stringify(yAxis));
  // console.log('tooltip:', JSON.stringify(tooltip));
  // console.log('legend:', JSON.stringify(legend));
  // console.log('referenceLines:', JSON.stringify(referenceLines));
  // console.log('referenceAreas:', JSON.stringify(referenceAreas));
  // console.log('referenceDots:', JSON.stringify(referenceDots));
  // console.log('colorScheme:', String(colorScheme));
  // console.log('stackOffset:', String(stackOffset));
  // console.log('barGap:', String(barGap));
  // console.log('barCategoryGap:', String(barCategoryGap));
  // console.log('maxBarSize:', String(maxBarSize));
  // console.log('reverseStackOrder:', String(reverseStackOrder));
  // console.log('layout:', String(layout));

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
