import { useState, useEffect, useCallback, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import { WidgetEventHandlerType, WidgetNode } from '@/types/widgets';
import { useToast } from '@/hooks/use-toast';
import { showError } from '@/hooks/use-error-sheet';
import { getIvyHost, getMachineId } from '@/lib/utils';
import { logger } from '@/lib/logger';
import { applyPatch, Operation } from 'fast-json-patch';
import { cloneDeep } from 'lodash';
import { ToastAction } from '@/components/ui/toast';
import { setThemeGlobal } from '@/components/theme-provider';

type UpdateMessage = Array<{
  viewId: string;
  indices: number[];
  patch: Operation[];
}>;

type RefreshMessage = {
  widgets: WidgetNode;
};

type ErrorMessage = {
  title: string;
  description: string;
  stackTrace?: string;
};

type AuthToken = {
  jwt: string;
  refreshToken?: string;
  expiresAt?: string;
  tag?: unknown;
};

const widgetTreeToXml = (node: WidgetNode) => {
  const tagName = node.type.replace('Ivy.', '');
  const attributes: string[] = [`Id="${escapeXml(node.id)}"`];
  if (node.props) {
    for (const [key, value] of Object.entries(node.props)) {
      const pascalCaseKey = key.charAt(0).toUpperCase() + key.slice(1);
      attributes.push(`${pascalCaseKey}="${escapeXml(String(value))}"`);
    }
  }
  let childrenXml = '';
  if (node.children && node.children.length > 0) {
    childrenXml = node.children.map(child => widgetTreeToXml(child)).join('');
    return `<${tagName} ${attributes.join(' ')}>${childrenXml}</${tagName}>`;
  } else {
    return `<${tagName} ${attributes.join(' ')} />`;
  }
};

const escapeXml = (str: string) => {
  return str
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&apos;');
};

function applyUpdateMessage(
  tree: WidgetNode,
  message: UpdateMessage
): WidgetNode {
  const newTree = cloneDeep(tree);

  message.forEach(update => {
    let parent = newTree;
    if (update.indices.length === 0) {
      applyPatch(parent, update.patch);
    } else {
      update.indices.forEach((index, i) => {
        if (i === update.indices.length - 1) {
          applyPatch(parent.children![index], update.patch);
        } else {
          parent = parent.children![index];
        }
      });
    }
  });

  return newTree;
}

export const useBackend = (
  appId: string | null,
  appArgs: string | null,
  parentId: string | null
) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const [widgetTree, setWidgetTree] = useState<WidgetNode | null>(null);
  const [disconnected, setDisconnected] = useState(false);
  const { toast } = useToast();
  const machineId = getMachineId();
  const connectionId = connection?.connectionId;
  const cleanupInProgressRef = useRef(false);
  const connectionRef = useRef<signalR.HubConnection | null>(null);

  useEffect(() => {
    if (import.meta.env.DEV && widgetTree) {
      const parser = new DOMParser();
      let xml;
      try {
        const xmlString = widgetTreeToXml(widgetTree);
        if (!xmlString) {
          logger.warn('Empty XML string generated from widget tree');
          return;
        }
        xml = parser.parseFromString(xmlString, 'application/xml');
        const parserError = xml.querySelector('parsererror');
        if (parserError) {
          logger.error('XML parsing error', { error: parserError.textContent });
          return;
        }
      } catch (error) {
        logger.error('Error converting widget tree to XML', { error });
        return;
      }
      logger.debug(`[${connectionId}]`, xml);
    }
  }, [widgetTree, connectionId]);

  const handleRefreshMessage = useCallback((message: RefreshMessage) => {
    setWidgetTree(message.widgets);
  }, []);

  const handleUpdateMessage = useCallback((message: UpdateMessage) => {
    setWidgetTree(currentTree => {
      if (!currentTree) {
        logger.warn('No current widget tree available for update');
        return null;
      }
      return applyUpdateMessage(currentTree, message);
    });
  }, []);

  const handleHotReloadMessage = useCallback(() => {
    logger.debug('Sending HotReload message');
    connection?.invoke('HotReload').catch(err => {
      logger.error('SignalR Error when sending HotReload:', err);
    });
  }, [connection]);

  const handleSetJwt = useCallback(
    (token: AuthToken) => {
      const storageKey = `ivy-token-${appId}`;
      if (token.jwt) {
        localStorage.setItem(storageKey, JSON.stringify(token));
        logger.debug('JWT token stored');
      } else {
        localStorage.removeItem(storageKey);
        logger.debug('JWT token removed');
      }
    },
    [appId]
  );

  const handleSetTheme = useCallback((theme: string) => {
    const normalizedTheme = theme?.toLowerCase();
    if (['dark', 'light', 'system'].includes(normalizedTheme)) {
      logger.info('Setting theme globally', { theme: normalizedTheme });
      setThemeGlobal(normalizedTheme as 'dark' | 'light' | 'system');
    } else {
      logger.error('Invalid theme value received', { theme });
    }
  }, []);

  const handleError = useCallback(
    (error: ErrorMessage) => {
      toast({
        title: error.title,
        description: error.description,
        variant: 'destructive',
        action: (
          <ToastAction
            altText="View error details"
            onClick={() => {
              showError({
                title: error.title,
                message: error.description,
                stackTrace: error.stackTrace,
              });
            }}
          >
            Details
          </ToastAction>
        ),
      });
    },
    [toast]
  );

  useEffect(() => {
    let cancelled = false;

    const setupConnection = async () => {
      // Don't create connection if appId is null or empty
      if (!appId) {
        logger.debug('Skipping connection setup: appId is null or empty');
        setConnection(null);
        setDisconnected(false);
        return;
      }

      // Prevent multiple cleanup operations
      if (cleanupInProgressRef.current) {
        return;
      }

      // Clean up the previous connection and wait for it to fully stop
      if (connectionRef.current) {
        cleanupInProgressRef.current = true;
        try {
          // Remove all event listeners first
          connectionRef.current.off('Refresh');
          connectionRef.current.off('Update');
          connectionRef.current.off('Toast');
          connectionRef.current.off('Error');
          connectionRef.current.off('CopyToClipboard');
          connectionRef.current.off('HotReload');
          connectionRef.current.off('SetJwt');
          connectionRef.current.off('SetTheme');
          connectionRef.current.off('OpenUrl');
          connectionRef.current.off('reconnecting');
          connectionRef.current.off('reconnected');
          connectionRef.current.off('close');

          // Wait for connection to stop if it's not already disconnected
          if (
            connectionRef.current.state !==
            signalR.HubConnectionState.Disconnected
          ) {
            await connectionRef.current.stop();
            logger.debug('Previous connection stopped successfully');
          }
        } catch (err) {
          logger.warn('Error stopping previous SignalR connection:', err);
        } finally {
          cleanupInProgressRef.current = false;
        }
      }

      // Don't proceed if effect was cancelled during cleanup
      if (cancelled) return;

      const newConnection = new signalR.HubConnectionBuilder()
        .withUrl(
          `${getIvyHost()}/messages?appId=${appId ?? ''}&appArgs=${appArgs ?? ''}&machineId=${machineId}&parentId=${parentId ?? ''}`
        )
        .withAutomaticReconnect()
        .build();

      // Don't proceed if effect was cancelled during connection creation
      if (cancelled) {
        newConnection.stop().catch(() => {});
        return;
      }

      // Set up event handlers before starting connection
      newConnection.on('Refresh', message => {
        logger.debug(`[${newConnection.connectionId}] Refresh`, message);
        handleRefreshMessage(message);
      });

      newConnection.on('Update', message => {
        logger.debug(`[${newConnection.connectionId}] Update`, message);
        handleUpdateMessage(message);
      });

      newConnection.on('Toast', message => {
        logger.debug(`[${newConnection.connectionId}] Toast`, message);
        toast(message);
      });

      newConnection.on('Error', message => {
        logger.debug(`[${newConnection.connectionId}] Error`, message);
        handleError(message);
      });

      newConnection.on('SetJwt', jwt => {
        logger.debug(`[${newConnection.connectionId}] SetJwt`);
        handleSetJwt(jwt);
      });

      newConnection.on('SetTheme', theme => {
        logger.debug(`[${newConnection.connectionId}] SetTheme`, { theme });
        handleSetTheme(theme);
      });

      newConnection.on('CopyToClipboard', (text: string) => {
        logger.debug(`[${newConnection.connectionId}] CopyToClipboard`);
        navigator.clipboard.writeText(text);
      });

      newConnection.on('OpenUrl', (url: string) => {
        logger.debug(`[${newConnection.connectionId}] OpenUrl`, { url });
        window.open(url, '_blank');
      });

      newConnection.on('HotReload', () => {
        logger.debug(`[${newConnection.connectionId}] HotReload`);
        handleHotReloadMessage();
      });

      newConnection.onreconnecting(() => {
        logger.warn(`[${newConnection.connectionId}] Reconnecting`);
        setDisconnected(true);
      });

      newConnection.onreconnected(() => {
        logger.info(`[${newConnection.connectionId}] Reconnected`);
        setDisconnected(false);
      });

      newConnection.onclose(() => {
        logger.warn(`[${newConnection.connectionId}] Closed`);
        setDisconnected(true);
      });

      try {
        // Start the connection
        await newConnection.start();

        // Don't proceed if effect was cancelled during connection start
        if (cancelled) {
          newConnection.stop().catch(() => {});
          return;
        }

        logger.info('✅ WebSocket connection established for:', {
          appId,
          parentId,
          connectionId: newConnection.connectionId,
        });

        // Only update state if effect wasn't cancelled
        connectionRef.current = newConnection;
        setConnection(newConnection);
        setDisconnected(false);
      } catch (error) {
        logger.error('SignalR connection failed:', error);
        if (!cancelled) {
          setDisconnected(true);
        }
      }
    };

    setupConnection();

    return () => {
      cancelled = true;
      if (connectionRef.current && !cleanupInProgressRef.current) {
        cleanupInProgressRef.current = true;
        connectionRef.current
          .stop()
          .catch(err => {
            logger.warn(
              'Error stopping SignalR connection during cleanup:',
              err
            );
          })
          .finally(() => {
            cleanupInProgressRef.current = false;
          });
      }
    };
  }, [
    appArgs,
    appId,
    machineId,
    parentId,
    handleRefreshMessage,
    handleUpdateMessage,
    handleHotReloadMessage,
    toast,
    handleSetJwt,
    handleSetTheme,
    handleError,
  ]);

  const eventHandler: WidgetEventHandlerType = useCallback(
    (eventName, widgetId, args) => {
      logger.debug(`[${connectionId}] Event: ${eventName}`, { widgetId, args });
      if (!connection) {
        logger.warn('No SignalR connection available for event', {
          eventName,
          widgetId,
        });
        return;
      }
      connection.invoke('Event', eventName, widgetId, args).catch(err => {
        logger.error('SignalR Error when sending event:', err);
      });
    },
    [connection, connectionId]
  );

  return {
    connection,
    widgetTree,
    eventHandler,
    disconnected,
  };
};
