import React from 'react';
import ReactECharts from 'echarts-for-react';
import { ColorScheme, ExtendedTooltipProps } from './shared';
import { getHeight, getWidth } from '@/lib/styles';
import {
  generateDataProps,
  generateEChartGrid,
  generateEChartLegend,
  generateSeries,
  generateTooltip,
  generateXAxis,
  generateYAxis,
  getColors,
  getTransformValueFn,
} from './sharedUtils';
import {
  CartesianGridProps,
  LegendProps,
  LinesProps,
  MarkArea,
  MarkLine,
  XAxisProps,
  YAxisProps,
} from './chartTypes';

export interface LineChartData {
  [key: string]: string | number;
}

interface LineChartWidgetProps {
  id: string;
  data: LineChartData[];
  width?: string;
  height?: string;
  lines?: LinesProps[];
  cartesianGrid?: CartesianGridProps;
  xAxis?: XAxisProps[];
  yAxis?: YAxisProps[];
  tooltip?: ExtendedTooltipProps;
  legend?: LegendProps;
  referenceLines?: MarkLine[];
  referenceAreas?: MarkArea[];
  referenceDots?: unknown;
  colorScheme: ColorScheme;
}

const LineChartWidget: React.FC<LineChartWidgetProps> = ({
  data,
  width,
  height,
  lines,
  cartesianGrid,
  xAxis,
  yAxis,
  tooltip,
  legend,
  //referenceLines,
  //referenceAreas,
  //referenceDots,
  colorScheme,
}) => {
  const styles: React.CSSProperties = {
    ...getWidth(width),
    ...getHeight(height),
    minHeight: 300,
  };

  const colors = getColors(colorScheme);
  const { categories, valueKeys } = generateDataProps(data);
  const { transform, largeSpread, minValue, maxValue } =
    getTransformValueFn(data);

  const option = {
    grid: generateEChartGrid(cartesianGrid),
    xAxis: generateXAxis(categories as string[], xAxis),
    yAxis: generateYAxis(largeSpread, transform, minValue, maxValue, yAxis),
    tooltip: generateTooltip(tooltip),
    legend: generateEChartLegend(legend),
    color: colors,
    series: generateSeries(data, valueKeys, lines, transform),
  };

  return <ReactECharts option={option} style={styles} />;
};

export default LineChartWidget;
