import ErrorBoundary from '@/components/ErrorBoundary';
import { EventHandlerProvider } from '@/components/EventHandlerContext';
import React from 'react';
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
  return (
    <div className="w-full h-full p-4">
      <ErrorBoundary>
        <EventHandlerProvider eventHandler={eventHandler}>
          <>{renderWidgetTree(widgetTree || loadingState())}</>
        </EventHandlerProvider>
      </ErrorBoundary>
    </div>
  );
};
