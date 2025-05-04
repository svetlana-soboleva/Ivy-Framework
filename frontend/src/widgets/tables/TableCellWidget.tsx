import React from 'react';
import { TableCell } from '@/components/ui/table';
import { cn } from '@/lib/utils';
import { Align, getAlign } from '@/lib/styles';

interface TableCellWidgetProps {
  id: string;
  isHeader?: boolean;
  isFooter?: boolean;
  align: Align;
  children?: React.ReactNode;
}

export const TableCellWidget: React.FC<TableCellWidgetProps> = ({ 
  children,
  isHeader,
  isFooter,
  align,
}) => {

    var alignStyles = {
      ...getAlign('Horizontal', align),
      ...getAlign('Vertical', align)
    }

    return (
      <TableCell 
        className={cn(
        isHeader && 'header-cell bg-gray-100 font-semibold',
        isFooter && 'footer-cell bg-gray-100 font-semibold',
        "text-nowrap",  
        "border",
        "max-w-[400px] overflow-ellipsis overflow-clip"
        )}>
          <div style={alignStyles}>
            {children}
          </div>
      </TableCell>
    );
  };