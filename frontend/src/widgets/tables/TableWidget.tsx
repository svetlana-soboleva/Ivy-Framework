import React from 'react';
import { Table, TableBody } from '@/components/ui/table';

interface TableWidgetProps {
  id: string;
  children?: React.ReactNode;
}

export const TableWidget: React.FC<TableWidgetProps> = ({ 
  children 
}) => (
  <Table className="text-sm w-fit">
    <TableBody>
      {children}
    </TableBody>
  </Table>
);