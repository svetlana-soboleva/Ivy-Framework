import { StrictMode, useEffect, useState } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import { renderWidgetTree, loadingState } from '@/widgets/WidgetRenderer';
import { useBackend } from '@/hooks/use-backend';
import { Toaster } from '@/components/ui/toaster';
import { ErrorSheet } from '@/components/ErrorSheet';
import ErrorBoundary from './components/ErrorBoundary';
import { EventHandlerProvider } from './components/EventHandlerContext';
import { TextShimmer } from './components/TextShimmer';
import MadeWithIvy from './components/MadeWithIvy';
import { ThemeProvider } from './components/ThemeProvider';
import { getCurrentRoute, updateRoute, type RouteInfo } from './lib/routing';
import { hasLicensedFeature } from './lib/license';

function ConnectionModal() {
  return (
    <div className="fixed inset-0 bg-background/80 flex items-center justify-center z-[1000]">
      <div className="px-8 py-4 bg-card border border-border rounded-lg shadow-lg">
        <TextShimmer>Connection lost. Trying to reconnect...</TextShimmer>
      </div>
    </div>
  );
}

function App() {
  const [route, setRoute] = useState<RouteInfo>(() => getCurrentRoute());
  const { widgetTree, eventHandler, disconnected } = useBackend(
    route.appId,
    route.appArgs,
    route.parentId
  );
  const [removeBranding, setRemoveBranding] = useState(true);

  useEffect(() => {
    hasLicensedFeature('RemoveBranding').then(setRemoveBranding);
  }, []);

  // Handle browser navigation (back/forward)
  useEffect(() => {
    const handlePopState = () => {
      setRoute(getCurrentRoute());
    };

    window.addEventListener('popstate', handlePopState);
    return () => window.removeEventListener('popstate', handlePopState);
  }, []);

  // Listen for navigation events from the backend
  useEffect(() => {
    const handleNavigation = (event: CustomEvent) => {
      const { appId, appArgs, parentId } = event.detail;
      const newRoute = {
        appId,
        appArgs,
        parentId,
        isMainApp: true, // Navigation events are always for main app
      };
      setRoute(newRoute);
      updateRoute(appId, appArgs);
    };

    window.addEventListener(
      'ivy-navigation',
      handleNavigation as EventListener
    );
    return () =>
      window.removeEventListener(
        'ivy-navigation',
        handleNavigation as EventListener
      );
  }, []);

  return (
    <ThemeProvider defaultTheme="light" storageKey="ivy-ui-theme">
      <ErrorBoundary>
        <EventHandlerProvider eventHandler={eventHandler}>
          <>
            {!removeBranding && <MadeWithIvy />}
            {renderWidgetTree(widgetTree || loadingState())}
            <ErrorSheet />
            <Toaster />
            {disconnected && <ConnectionModal />}
          </>
        </EventHandlerProvider>
      </ErrorBoundary>
    </ThemeProvider>
  );
}

const container = document.getElementById('root');
if (!container) throw new Error('Failed to find root element');

interface WindowWithReactRoot extends Window {
  __reactRoot?: ReturnType<typeof createRoot>;
}

let root = (window as WindowWithReactRoot).__reactRoot;
if (!root) {
  root = createRoot(container);
  (window as WindowWithReactRoot).__reactRoot = root;
}

root.render(
  <StrictMode>
    <App />
  </StrictMode>
);
