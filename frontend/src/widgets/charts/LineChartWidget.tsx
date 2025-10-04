import React from 'react';
import ReactECharts from 'echarts-for-react';
import { ColorScheme, ExtendedTooltipProps } from './shared';
import { getHeight, getWidth } from '@/lib/styles';
import {
  generateDataProps,
  generateEChartGrid,
  generateEChartLegend,
  getColors,
} from './sharedUtils';
import {
  CartesianGridProps,
  ChartType,
  LegendProps,
  LinesProps,
  MarkArea,
  MarkLine,
  XAxisProps,
  YAxisProps,
} from './chartTypes';

interface LineChartData {
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

  const series = valueKeys.map(key => ({
    name: key,
    type: ChartType.Line,
    data: data.map(d => Number(d[key])),
    step: lines?.find(l => l.curveType === 'Step') ? 'middle' : false,
    smooth: lines?.find(l => l.curveType === 'Natural') ? true : false,
  }));

  const option = {
    grid: generateEChartGrid(cartesianGrid),
    xAxis: {
      position:
        xAxis?.[0]?.orientation?.toLowerCase() === 'top' ? 'top' : 'bottom',
      type: 'category',
      data: categories,
      axisLabel: {
        formatter: (value: string) =>
          value.length > 10 ? value.match(/.{1,10}/g)?.join('\n') : value,
        interval: 'auto',
      },
    },
    yAxis: {
      type: 'value',
      axisLabel: {
        formatter: (value: number) => {
          if (Math.abs(value) >= 1e9) return (value / 1e9).toFixed(1) + 'B';
          if (Math.abs(value) >= 1e6) return (value / 1e6).toFixed(1) + 'M';
          if (Math.abs(value) >= 1e3) return (value / 1e3).toFixed(1) + 'K';
          return value;
        },
      },
      position:
        yAxis?.[0]?.orientation?.toLowerCase() === 'right' ? 'right' : 'left',
    },
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
    legend: generateEChartLegend(legend),
    color: colors,
    series: series,
  };
  return (
    <div>
      <ReactECharts option={option} style={{ ...styles, minHeight: 300 }} />
    </div>
  );
};

export default LineChartWidget;
