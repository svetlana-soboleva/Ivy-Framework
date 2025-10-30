import DataEditor, {
  CompactSelection,
  DataEditorRef,
  GridCell,
  GridSelection,
  GridMouseEventArgs,
  Item,
  Theme,
} from '@glideapps/glide-data-grid';
import React, {
  useMemo,
  useCallback,
  useEffect,
  useRef,
  useState,
} from 'react';
import { useTable } from './DataTableContext';
import { tableStyles } from './styles/style';
import { useThemeWithMonitoring } from '@/components/theme-provider';
import { getSelectionProps } from './utils/selectionModes';
import { getCellContent as getCellContentUtil } from './utils/cellContent';
import { convertToGridColumns } from './utils/columnHelpers';
import { iconCellRenderer } from './utils/customRenderers';
import { generateHeaderIcons, addStandardIcons } from './utils/headerIcons';
import { ThemeColors } from '@/lib/color-utils';
import { useEventHandler } from '@/components/event-handler';
import { useColumnGroups } from './hooks/useColumnGroups';

interface TableEditorProps {
  widgetId: string;
  hasOptions?: boolean;
}

export const DataTableEditor: React.FC<TableEditorProps> = ({
  widgetId,
  hasOptions = false,
}) => {
  const {
    data,
    columns,
    columnWidths,
    visibleRows,
    isLoading,
    hasMore,
    editable,
    config,
    columnOrder,
    loadMoreData,
    handleColumnResize,
    handleSort,
    handleColumnReorder,
  } = useTable();

  const {
    allowColumnReordering,
    allowColumnResizing,
    allowCopySelection,
    allowSorting,
    freezeColumns,
    showIndexColumn,
    selectionMode,
    showGroups,
    enableCellClickEvents,
    showSearch: showSearchConfig,
    showColumnTypeIcons,
    showVerticalBorders,
    enableRowHover,
  } = config;

  const selectionProps = getSelectionProps(selectionMode);

  // Use the enhanced theme hook with custom DataGrid theme generator
  const {
    customTheme: tableTheme,
    colors: themeColors,
    isDark,
  } = useThemeWithMonitoring<Partial<Theme>>({
    monitorDOM: true,
    monitorSystem: true,
    customThemeGenerator: (
      colors: ThemeColors,
      isDark: boolean
    ): Partial<Theme> => ({
      bgCell: colors.background || (isDark ? '#000000' : '#ffffff'),
      bgHeader: colors.background || (isDark ? '#1a1a1f' : '#f9fafb'),
      bgHeaderHasFocus: colors.muted || (isDark ? '#26262b' : '#f3f4f6'),
      bgHeaderHovered: colors.accent || (isDark ? '#26262b' : '#e5e7eb'),
      textHeader: colors.foreground || (isDark ? '#f8f8f8' : '#111827'),
      textDark: colors.foreground || (isDark ? '#f8f8f8' : '#111827'),
      textMedium: colors.mutedForeground || (isDark ? '#a1a1aa' : '#6b7280'),
      textLight: colors.mutedForeground || (isDark ? '#71717a' : '#9ca3af'),
      // bgIconHeader is the background color for icon areas, should be subtle
      bgIconHeader: colors.muted || (isDark ? '#26262b' : '#f3f4f6'),
      // accentColor affects icon foreground colors in headers
      accentColor:
        colors.primary || colors.accent || (isDark ? '#60a5fa' : '#3b82f6'),
      horizontalBorderColor: colors.border || (isDark ? '#404045' : '#d1d5db'),
      linkColor:
        colors.primary || colors.accent || (isDark ? '#3b82f6' : '#2563eb'),
      // Control vertical borders by setting borderColor to transparent when disabled
      borderColor: showVerticalBorders
        ? colors.border || (isDark ? '#404045' : '#d1d5db')
        : 'transparent',
      cellHorizontalPadding: 16,
      cellVerticalPadding: 8,
      headerIconSize: 20,
      // Add proper text colors for group headers and icons
      textGroupHeader:
        colors.mutedForeground || (isDark ? '#a1a1aa' : '#6b7280'),
      // Icon foreground color
      fgIconHeader: colors.mutedForeground || (isDark ? '#9ca3af' : '#6b7280'),
    }),
  });

  const gridRef = useRef<DataEditorRef>(null);
  const containerRef = useRef<HTMLDivElement>(null);
  const [containerWidth, setContainerWidth] = useState<number>(0);
  const [gridSelection, setGridSelection] = useState<GridSelection>({
    columns: CompactSelection.empty(),
    rows: CompactSelection.empty(),
  });
  const [showSearch, setShowSearch] = useState(false);
  const [hoverRow, setHoverRow] = useState<number | undefined>(undefined);
  const scrollThreshold = 10;

  // Generate header icons map for all column icons
  const headerIcons = useMemo(() => {
    const baseIcons = generateHeaderIcons(columns);
    return addStandardIcons(baseIcons);
  }, [columns]);

  // Track container width
  useEffect(() => {
    if (!containerRef.current) return;

    const resizeObserver = new ResizeObserver(entries => {
      for (const entry of entries) {
        setContainerWidth(entry.contentRect.width);
      }
    });

    resizeObserver.observe(containerRef.current);

    return () => {
      resizeObserver.disconnect();
    };
  }, []);

  // Handle keyboard shortcut for search (Ctrl/Cmd + F)
  useEffect(() => {
    if (!showSearchConfig) return;

    const handleKeyDown = (event: KeyboardEvent) => {
      if ((event.ctrlKey || event.metaKey) && event.code === 'KeyF') {
        setShowSearch(current => !current);
        event.stopPropagation();
        event.preventDefault();
      }
    };

    window.addEventListener('keydown', handleKeyDown, true);

    return () => {
      window.removeEventListener('keydown', handleKeyDown, true);
    };
  }, [showSearchConfig]);

  // Handle scroll events
  const handleVisibleRegionChanged = useCallback(
    (range: { x: number; y: number; width: number; height: number }) => {
      const bottomRow = range.y + range.height;
      const shouldLoadMore = bottomRow >= visibleRows - scrollThreshold;
      if (!isLoading && shouldLoadMore && hasMore) {
        loadMoreData();
      }
    },
    [visibleRows, hasMore, loadMoreData, isLoading]
  );

  // Get cell content
  const getCellContent = useCallback(
    (cell: Item): GridCell => {
      return getCellContentUtil(cell, data, columns, columnOrder, editable);
    },
    [data, columns, columnOrder, editable]
  );

  // Handle column header click for sorting
  const handleHeaderMenuClick = useCallback(
    (col: number) => {
      // Only handle sorting if it's enabled globally
      if (!allowSorting) return;

      // Get visible columns to map the correct column index
      const visibleColumns = columns.filter(c => !c.hidden);
      const column = visibleColumns[col];

      // Check if this specific column is sortable (defaults to true if not specified)
      if (column && (column.sortable ?? true)) {
        handleSort(column.name);
      }
    },
    [columns, handleSort, allowSorting]
  );

  // Handle selection changes
  const handleGridSelectionChange = useCallback(
    (newSelection: GridSelection) => {
      setGridSelection(newSelection);
    },
    []
  );

  // Get event handler for sending events to backend
  const eventHandler = useEventHandler();

  // Handle cell single-clicks (for backend events only)
  const handleCellClicked = useCallback(
    (cell: Item) => {
      if (enableCellClickEvents) {
        // Get actual cell value
        const cellContent = getCellContent(cell);
        const visibleColumns = columns.filter(c => !c.hidden);
        const column = visibleColumns[cell[0]];

        // Extract the actual value from the cell based on its kind
        let cellValue: unknown = null;
        if (
          cellContent.kind === 'text' ||
          cellContent.kind === 'number' ||
          cellContent.kind === 'boolean'
        ) {
          cellValue = cellContent.data;
        } else if ('data' in cellContent) {
          // Cast to unknown first, then access the data property
          cellValue = (cellContent as unknown as { data: unknown }).data;
        }

        // Send event to backend with row, column, and value
        eventHandler('OnCellClick', widgetId, [
          cell[1], // row index
          cell[0], // column index
          column?.name || '', // column name
          cellValue, // cell value
        ]);
      }
      // Do NOT prevent default - let selection happen normally!
    },
    [enableCellClickEvents, eventHandler, widgetId, columns, getCellContent]
  );

  // Handle cell double-clicks/activation (for editing)
  const handleCellActivated = useCallback(
    (cell: Item) => {
      if (enableCellClickEvents) {
        const cellContent = getCellContent(cell);
        const visibleColumns = columns.filter(c => !c.hidden);
        const column = visibleColumns[cell[0]];

        // Extract the actual value from the cell based on its kind
        let cellValue: unknown = null;
        if (
          cellContent.kind === 'text' ||
          cellContent.kind === 'number' ||
          cellContent.kind === 'boolean'
        ) {
          cellValue = cellContent.data;
        } else if ('data' in cellContent) {
          // Cast to unknown first, then access the data property
          cellValue = (cellContent as unknown as { data: unknown }).data;
        }

        // Send activation event to backend
        eventHandler('OnCellActivated', widgetId, [
          cell[1], // row index
          cell[0], // column index
          column?.name || '', // column name
          cellValue, // cell value
        ]);
      }
    },
    [enableCellClickEvents, eventHandler, widgetId, columns, getCellContent]
  );

  // Handle row hover
  const onItemHovered = useCallback(
    (args: GridMouseEventArgs) => {
      if (!enableRowHover) return;
      const [, row] = args.location;
      setHoverRow(args.kind !== 'cell' ? undefined : row);
    },
    [enableRowHover]
  );

  // Get row theme override for hover effect
  const getRowThemeOverride = useCallback(
    (row: number) => {
      if (!enableRowHover || row !== hoverRow) return undefined;
      // Use theme-aware colors for hover effect
      return {
        bgCell: themeColors.accent || (isDark ? '#26262b' : '#f7f7f7'),
        bgCellMedium: themeColors.muted || (isDark ? '#1f1f23' : '#f0f0f0'),
      };
    },
    [hoverRow, enableRowHover, themeColors, isDark]
  );

  // Convert columns to grid format with proper widths
  const gridColumns = convertToGridColumns(
    columns,
    columnOrder,
    columnWidths,
    containerWidth,
    showGroups ?? false,
    showColumnTypeIcons ?? true
  );

  // Use column groups hook when showGroups is enabled
  const columnGroupsHook = useColumnGroups(gridColumns);
  const shouldUseColumnGroups = showGroups ?? false;

  // Use grouped columns if showGroups is enabled, otherwise use regular columns
  const finalColumns = shouldUseColumnGroups
    ? columnGroupsHook.columns
    : gridColumns;

  if (finalColumns.length === 0) {
    return null;
  }

  const containerStyle = hasOptions
    ? tableStyles.tableEditor.gridContainerWithOptions
    : tableStyles.tableEditor.gridContainer;

  return (
    <div ref={containerRef} style={containerStyle}>
      <DataEditor
        ref={gridRef}
        columns={finalColumns}
        rows={visibleRows}
        getCellContent={getCellContent}
        customRenderers={[iconCellRenderer]}
        headerIcons={headerIcons}
        onColumnResize={allowColumnResizing ? handleColumnResize : undefined}
        onVisibleRegionChanged={handleVisibleRegionChanged}
        onHeaderClicked={allowSorting ? handleHeaderMenuClick : undefined}
        smoothScrollX={true}
        smoothScrollY={true}
        theme={tableTheme}
        rowHeight={38}
        headerHeight={32}
        freezeColumns={freezeColumns ?? 0}
        getCellsForSelection={(allowCopySelection ?? true) ? true : undefined}
        keybindings={{ search: false }}
        rowSelect={selectionProps.rowSelect}
        columnSelect={selectionProps.columnSelect}
        rangeSelect={selectionProps.rangeSelect}
        gridSelection={gridSelection}
        onGridSelectionChange={handleGridSelectionChange}
        width={containerWidth}
        rowMarkers={showIndexColumn ? 'number' : 'none'}
        onColumnMoved={allowColumnReordering ? handleColumnReorder : undefined}
        groupHeaderHeight={showGroups ? 36 : undefined}
        cellActivationBehavior="double-click"
        onCellClicked={handleCellClicked}
        onCellActivated={handleCellActivated}
        onGroupHeaderClicked={
          shouldUseColumnGroups
            ? columnGroupsHook.onGroupHeaderClicked
            : undefined
        }
        showSearch={showSearchConfig ? showSearch : false}
        onSearchClose={() => setShowSearch(false)}
        onItemHovered={enableRowHover ? onItemHovered : undefined}
        getRowThemeOverride={enableRowHover ? getRowThemeOverride : undefined}
      />
    </div>
  );
};
