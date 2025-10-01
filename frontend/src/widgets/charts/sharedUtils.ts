import { CartesianGridProps, LegendProps } from './chartTypes';
import { ColorScheme } from './shared';

//colors for echart
const defaultColorsEChart = [
  'oklch(0.75 0.16 164.07)',
  'oklch(0.69 0.2 23.69)',
  'oklch(0.45 0.31 264.05)',
  'oklch(0.89 0.19 106.02)',
  'oklch(0.49 0.19 309.75)',
];

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

export const getColorGeneratorEChart = (
  scheme: ColorScheme,
  length: number
): string[] => {
  switch (scheme) {
    case 'Rainbow':
      return Array.from(
        { length },
        (_, i) => rainbowColors[i % rainbowColors.length]
      );
    default:
      return Array.from(
        { length },
        (_, i) => defaultColorsEChart[i % defaultColorsEChart.length]
      );
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

/**
 * Adds rounded border radius to the end of stacks for horizontal bar charts.
 * Modifies the series array in place.
 */
// eslint-disable-next-line @typescript-eslint/no-explicit-any
export function applyHorizontalStackRounding(series: any[]) {
  const stackInfo: Record<
    string,
    { stackStart: number[]; stackEnd: number[] }
  > = {};

  for (let i = 0; i < series[0].data.length; ++i) {
    for (let j = 0; j < series.length; ++j) {
      const stackName = series[j].stack;
      if (!stackName) continue;
      if (!stackInfo[stackName]) {
        stackInfo[stackName] = { stackStart: [], stackEnd: [] };
      }
      const info = stackInfo[stackName];
      const datum = series[j].data[i];
      if (datum && datum !== '-') {
        if (info.stackStart[i] == null) info.stackStart[i] = j;
        info.stackEnd[i] = j;
      }
    }
  }
  for (let i = 0; i < series.length; ++i) {
    const dataArr = series[i].data;
    const info = stackInfo[series[i].stack];
    if (!info) continue;
    for (let j = 0; j < dataArr.length; ++j) {
      const isEnd = info.stackEnd[j] === i;
      const topBorder = isEnd ? 20 : 0;
      const bottomBorder = 0;
      dataArr[j] = {
        value: typeof dataArr[j] === 'object' ? dataArr[j].value : dataArr[j],
        itemStyle: {
          borderRadius: [topBorder, topBorder, bottomBorder, bottomBorder],
        },
      };
    }
  }
  return series;
}

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
