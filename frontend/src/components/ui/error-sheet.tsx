'use client';

import { useErrorSheet } from '@/hooks/use-error-sheet';
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
} from '@/components/ui/sheet';
import { Exception } from '@/components/Exception';

export function ErrorSheet() {
  const { errors, hideError, clearError } = useErrorSheet();

  return (
    <>
      {errors.map(({ id, title, message, stackTrace, open }) => (
        <Sheet
          key={id}
          open={open}
          onOpenChange={isOpen => {
            if (!isOpen) {
              hideError(id);
              setTimeout(() => clearError(id), 300);
            }
          }}
        >
          <SheetContent
            side="right"
            className="w-full sm:max-w-lg overflow-y-auto"
          >
            <SheetHeader>
              <SheetTitle>Error Details</SheetTitle>
              <SheetDescription>
                An error occurred in the application
              </SheetDescription>
            </SheetHeader>
            <div className="mt-6">
              <Exception
                title={title}
                message={message}
                stackTrace={stackTrace}
              />
            </div>
          </SheetContent>
        </Sheet>
      ))}
    </>
  );
}
