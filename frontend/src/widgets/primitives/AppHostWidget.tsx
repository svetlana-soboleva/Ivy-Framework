import ErrorBoundary from '@/components/ErrorBoundary';
import { EventHandlerProvider } from '@/components/EventHandlerContext';
import React, { useLayoutEffect, useRef } from 'react';
import { loadingState, renderWidgetTree } from '../WidgetRenderer';
import { useBackend } from '@/hooks/use-backend';

interface AppHostWidgetProps {
  id: string;
  appId: string;
  appArgs: string | null;
  parentId: string | null;
}

export const AppHostWidget: React.FC<AppHostWidgetProps> = ({
  appId,
  appArgs,
  parentId,
}) => {
  const { widgetTree, eventHandler } = useBackend(appId, appArgs, parentId);
  const containerRef = useRef<HTMLDivElement>(null);

  // Always scroll to top when widgetTree changes (on navigation)
  useLayoutEffect(() => {
    if (containerRef.current) {
      containerRef.current.scrollTop = 0;
    }
  }, [widgetTree]);

  return (
    <div ref={containerRef} className="w-full h-full p-4 overflow-y-auto">
      <ErrorBoundary>
        <EventHandlerProvider eventHandler={eventHandler}>
          <>{renderWidgetTree(widgetTree || loadingState())}</>
        </EventHandlerProvider>
      </ErrorBoundary>
    </div>
  );
};
