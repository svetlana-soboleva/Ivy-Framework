import { GridCell, GridCellKind, Item } from '@glideapps/glide-data-grid';
import { Align, DataColumn, DataRow } from '../types/types';

/**
 * Converts Align enum to contentAlign value for GridCell
 */
export function getContentAlign(align?: Align): 'left' | 'center' | 'right' {
  if (!align) return 'left';

  switch (align) {
    case Align.Left:
      return 'left';
    case Align.Center:
      return 'center';
    case Align.Right:
      return 'right';
    default:
      return 'left';
  }
}

/**
 * Creates an empty/fallback cell for out-of-bounds requests
 */
export function createEmptyCell(): GridCell {
  return {
    kind: GridCellKind.Text,
    data: '',
    displayData: '',
    allowOverlay: false,
    readonly: true,
  };
}

/**
 * Creates a cell for null or undefined values
 */
export function createNullCell(editable: boolean): GridCell {
  return {
    kind: GridCellKind.Text,
    data: '',
    displayData: '', // Show empty instead of "null" text
    allowOverlay: editable,
    readonly: !editable,
    style: 'faded',
  };
}

/**
 * Checks if a string value looks like a Lucide icon name (PascalCase)
 * @deprecated Use column type ColType.Icon instead of heuristics
 */
export function isProbablyIconValue(value: unknown): boolean {
  return (
    typeof value === 'string' &&
    /^[A-Z][a-zA-Z0-9]*$/.test(value) &&
    value.length > 2 &&
    !value.includes(' ')
  );
}

/**
 * Creates an icon cell
 */
export function createIconCell(iconName: string, align?: Align): GridCell {
  return {
    kind: GridCellKind.Custom,
    allowOverlay: false,
    readonly: true,
    copyData: iconName,
    data: {
      kind: 'icon-cell',
      iconName,
      align: align ? getContentAlign(align) : undefined,
    },
  };
}

/**
 * Checks if a column type represents a date/timestamp
 */
export function isDateColumnType(columnType: string): boolean {
  return columnType.includes('date') || columnType.includes('timestamp');
}

/**
 * Checks if a column type represents a numeric type
 */
export function isNumericColumnType(columnType: string): boolean {
  return (
    columnType.includes('int') ||
    columnType.includes('float') ||
    columnType.includes('double') ||
    columnType.includes('decimal') ||
    columnType.includes('number')
  );
}

/**
 * Formats a date value for display
 */
export function formatDateValue(dateValue: Date, columnType: string): string {
  const hasTime =
    columnType.includes('datetime') ||
    columnType.includes('timestamp') ||
    dateValue.getHours() !== 0 ||
    dateValue.getMinutes() !== 0 ||
    dateValue.getSeconds() !== 0;

  return hasTime ? dateValue.toLocaleString() : dateValue.toLocaleDateString();
}

/**
 * Parses a date from various input formats
 */
export function parseDateValue(cellValue: unknown): Date | null {
  if (typeof cellValue === 'number') {
    const date = new Date(cellValue);
    return !isNaN(date.getTime()) ? date : null;
  }

  if (typeof cellValue === 'string') {
    const date = new Date(cellValue);
    return !isNaN(date.getTime()) ? date : null;
  }

  return null;
}

/**
 * Creates a date/timestamp cell
 */
export function createDateCell(
  cellValue: unknown,
  columnType: string,
  editable: boolean,
  align?: Align
): GridCell | null {
  const dateValue = parseDateValue(cellValue);

  if (!dateValue) {
    return null;
  }

  const displayData = formatDateValue(dateValue, columnType);

  return {
    kind: GridCellKind.Text,
    data: displayData,
    displayData,
    allowOverlay: editable,
    readonly: !editable,
    contentAlign: align ? getContentAlign(align) : undefined,
  };
}

/**
 * Formats a number for display
 */
export function formatNumberValue(value: number): string {
  return Number.isInteger(value) ? value.toString() : value.toFixed(2);
}

/**
 * Creates a numeric cell
 */
export function createNumberCell(
  cellValue: number,
  editable: boolean,
  align?: Align
): GridCell {
  const displayData = formatNumberValue(cellValue);

  return {
    kind: GridCellKind.Number,
    data: cellValue,
    displayData,
    allowOverlay: editable,
    readonly: !editable,
    contentAlign: align ? getContentAlign(align) : undefined,
  };
}

/**
 * Creates a boolean cell
 */
export function createBooleanCell(
  cellValue: boolean,
  editable: boolean,
  align?: Align
): GridCell {
  return {
    kind: GridCellKind.Boolean,
    data: cellValue,
    allowOverlay: false,
    readonly: !editable,
    contentAlign: align ? getContentAlign(align) : undefined,
  };
}

/**
 * Creates a text cell
 */
export function createTextCell(
  cellValue: unknown,
  editable: boolean,
  align?: Align
): GridCell {
  const stringValue = String(cellValue);

  return {
    kind: GridCellKind.Text,
    data: stringValue,
    displayData: stringValue,
    allowOverlay: editable,
    readonly: !editable,
    contentAlign: align ? getContentAlign(align) : undefined,
  };
}

/**
 * Gets the ordered columns based on columnOrder array
 */
export function getOrderedColumns(
  columns: DataColumn[],
  columnOrder: number[]
): DataColumn[] {
  return columnOrder.length === columns.length
    ? columnOrder.map(idx => columns[idx])
    : columns;
}

/**
 * Main function to get cell content for a grid cell
 * Filters out hidden columns and applies column ordering
 */
export function getCellContent(
  cell: Item,
  data: DataRow[],
  columns: DataColumn[],
  columnOrder: number[],
  editable: boolean
): GridCell {
  const [col, row] = cell;

  // Apply column order first, then filter out hidden columns
  let orderedCols: DataColumn[];
  if (columnOrder.length === columns.length) {
    // Map using columnOrder indices, then filter hidden
    orderedCols = columnOrder
      .map(idx => columns[idx])
      .filter(col => !col.hidden);
  } else {
    // No reordering, just filter hidden columns
    orderedCols = columns.filter(col => !col.hidden);
  }

  // Safety check
  if (row >= data.length || col >= orderedCols.length) {
    return createEmptyCell();
  }

  const rowData = data[row];
  const column = orderedCols[col];
  const originalColumnIndex = columns.indexOf(column);
  const cellValue = rowData.values[originalColumnIndex];
  const columnType = column.type?.toLowerCase() || 'text';
  const align = column.align;

  // Handle null/undefined values
  if (cellValue === null || cellValue === undefined) {
    return createNullCell(editable);
  }

  // Handle explicit icon type from backend metadata
  if (column.type === 'Icon' && typeof cellValue === 'string') {
    return createIconCell(cellValue, align);
  }

  // Handle Date and DateTime types
  if (isDateColumnType(columnType)) {
    const dateCell = createDateCell(cellValue, columnType, editable, align);
    if (dateCell) {
      return dateCell;
    }
  }

  // Handle numeric types
  if (typeof cellValue === 'number' && isNumericColumnType(columnType)) {
    return createNumberCell(cellValue, editable, align);
  }

  // Handle boolean types
  if (typeof cellValue === 'boolean') {
    return createBooleanCell(cellValue, editable, align);
  }

  // REMOVED: Heuristic icon detection (now that we have proper type metadata from backend)
  // The heuristic was causing false positives for PascalCase text like "Active", "EMP0001", etc.
  // Now that column.type is properly preserved from backend (#1273), we don't need this fallback

  // Default to text
  return createTextCell(cellValue, editable, align);
}
