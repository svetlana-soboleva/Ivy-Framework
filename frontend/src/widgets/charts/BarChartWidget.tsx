import React from 'react';
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
  bars,
  // cartesianGrid,
  //xAxis,
  //yAxis,
  tooltip,
  //legend,
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

  const categoryKey = Object.keys(data[0]).find(
    k => typeof data[0][k] === 'string'
  )!;

  //  categories for x-axis (only for Category type)
  const categories = data.map(d => d[categoryKey.toLowerCase()]);

  const valueKeys = Object.keys(data[0]).filter(
    k => typeof data[0][k] === 'number'
  );
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
  const option = {
    color: colors,
    xAxis: isVertical
      ? { type: 'value' }
      : { type: 'category', data: categories },
    yAxis: isVertical
      ? { type: 'category', data: categories }
      : { type: 'value' },
    series,
    tooltip: tooltip
      ? { show: true, trigger: 'axis', animation: tooltip.animated ?? true }
      : undefined,
    legend: { show: true },
  };

  //log all the props to debug
  //console.log('data:', JSON.stringify(data));
  //console.log('width:', String(width));
  //console.log('height:', String(height));
  //console.log('layout:', String(layout));
  //console.log('bars:', JSON.stringify(bars));
  //console.log('cartesianGrid:', JSON.stringify(cartesianGrid));
  //console.log('xAxis:', JSON.stringify(xAxis));
  //console.log('yAxis:', JSON.stringify(yAxis));
  // console.log('tooltip:', JSON.stringify(tooltip));
  //console.log('legend:', JSON.stringify(legend));
  // console.log('referenceLines:', JSON.stringify(referenceLines));
  // console.log('referenceAreas:', JSON.stringify(referenceAreas));
  // console.log('referenceDots:', JSON.stringify(referenceDots));
  // console.log('colorScheme:', String(colorScheme));
  //console.log('stackOffset:', String(stackOffset));
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
