import React from 'react';

export function Kbd({ children }: { children: React.ReactNode }) {
  return (
    <kbd className="rounded w-min bg-muted px-2 py-0.5 text-foreground border border-border text-nowrap">
      {children}
    </kbd>
  );
}
