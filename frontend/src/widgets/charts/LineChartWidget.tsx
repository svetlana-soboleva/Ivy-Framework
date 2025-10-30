import React, { useEffect, useState } from 'react';
import ReactECharts from 'echarts-for-react';
import { getHeight, getWidth } from '@/lib/styles';
import { useTheme } from '@/components/theme-provider';
import {
  generateDataProps,
  generateEChartGrid,
  generateEChartLegend,
  generateSeries,
  generateTooltip,
  generateTextStyle,
  generateXAxis,
  generateYAxis,
  getColors,
  getTransformValueFn,
  generateEChartToolbox,
} from './sharedUtils';
import { LineChartWidgetProps, ChartType } from './chartTypes';

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
  toolbox,
  referenceLines,
  referenceAreas,
  referenceDots,
  colorScheme,
}) => {
  const { theme } = useTheme();
  const [themeColors, setThemeColors] = useState({
    foreground: '#000000',
    mutedForeground: '#666666',
    fontSans: 'Geist, sans-serif',
    background: '#ffffff',
  });

  useEffect(() => {
    const getThemeColors = () => {
      const root = document.documentElement;
      const computedStyle = getComputedStyle(root);

      // Use the theme value directly instead of checking DOM classes
      const isDarkMode =
        theme === 'dark' ||
        (theme === 'system' &&
          window.matchMedia('(prefers-color-scheme: dark)').matches);

      return {
        foreground:
          computedStyle.getPropertyValue('--foreground').trim() ||
          (isDarkMode ? '#f8f8f8' : '#000000'),
        mutedForeground:
          computedStyle.getPropertyValue('--muted-foreground').trim() ||
          (isDarkMode ? '#a1a1aa' : '#666666'),
        fontSans:
          computedStyle.getPropertyValue('--font-sans').trim() ||
          'Geist, sans-serif',
        background:
          computedStyle.getPropertyValue('--background').trim() ||
          (isDarkMode ? '#000000' : '#ffffff'),
      };
    };

    // Update colors on next frame to avoid synchronous setState in effect
    const frame = requestAnimationFrame(() => {
      setThemeColors(getThemeColors());
    });

    return () => {
      cancelAnimationFrame(frame);
    };
  }, [theme]);

  // When height is Full (100%), use flex to expand. Otherwise use explicit height.
  const heightStyle = height ? getHeight(height) : {};
  const isFull = height?.toLowerCase().startsWith('full');

  const styles: React.CSSProperties = {
    ...getWidth(width),
    ...(isFull
      ? { display: 'flex', flexDirection: 'column', height: '100%' }
      : {}),
  };

  const chartStyles: React.CSSProperties = {
    ...(isFull
      ? { flex: 1, minHeight: '200px' }
      : { ...heightStyle, minHeight: '200px' }),
    width: '100%',
  };

  const colors = getColors(colorScheme);
  const { categories, valueKeys } = generateDataProps(data);
  const { transform, largeSpread, minValue, maxValue } =
    getTransformValueFn(data);

  const option = {
    grid: generateEChartGrid(cartesianGrid),
    xAxis: generateXAxis(ChartType.Line, categories as string[], xAxis, false, {
      mutedForeground: themeColors.mutedForeground,
      fontSans: themeColors.fontSans,
    }),
    yAxis: generateYAxis(
      largeSpread,
      transform,
      minValue,
      maxValue,
      yAxis,
      false,
      undefined,
      {
        mutedForeground: themeColors.mutedForeground,
        fontSans: themeColors.fontSans,
      }
    ),
    tooltip: generateTooltip(tooltip, 'shadow', {
      foreground: themeColors.foreground,
      fontSans: themeColors.fontSans,
      background: themeColors.background,
    }),
    toolbox: generateEChartToolbox(toolbox),
    legend: generateEChartLegend(legend, {
      foreground: themeColors.foreground,
      fontSans: themeColors.fontSans,
    }),
    textStyle: generateTextStyle(themeColors.foreground, themeColors.fontSans),
    color: colors,
    series: generateSeries(
      data,
      valueKeys,
      lines,
      transform,
      referenceDots,
      referenceLines,
      referenceAreas
    ),
  };

  return (
    <div style={styles}>
      <ReactECharts key={theme} option={option} style={chartStyles} />
    </div>
  );
};

export default LineChartWidget;
