export type CartesianGridProps = {
  fill: string | null;
  fillOpacity: number | null;
  height: number | null;
  horizontal: boolean;
  strokeDashArray: string | null;
  vertical: boolean;
  width: number | null;
  x: number | null;
  y: number | null;
};

export type LegendProps = {
  align?: 'Left' | 'Center' | 'Right';
  iconSize?: number;
  iconType?: string | null;
  layout?: 'Horizontal' | 'Vertical';
  verticalAlign?: 'Top' | 'Middle' | 'Bottom';
};
