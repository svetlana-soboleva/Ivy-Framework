import { useState, useEffect, useCallback, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import { WidgetEventHandlerType, WidgetNode } from '@/types/widgets';
import { useToast } from '@/hooks/use-toast';
import { getAppArgs, getAppId, getIvyHost, getMachineId, getParentId } from '@/lib/utils';
import { applyPatch, Operation } from 'fast-json-patch';
import { setThemeGlobal } from '@/components/ThemeProvider';

type UpdateMessage = Array<{ 
  viewId: string, 
  indices: number[]; 
  patch: Operation[] 
}>;

type RefreshMessage = {
  widgets: WidgetNode,
  removeIvyBranding: boolean
}

// Moved outside the hook to prevent recreating on each render
const escapeXml = (str: string) => {
  return str
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;') 
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&apos;');
}

const widgetTreeToXml = (node: WidgetNode) => {
  const tagName = node.type.replace("Ivy.", "");
  const attributes: string[] = [`Id="${escapeXml(node.id)}"`];
  if (node.props) {
      for (const [key, value] of Object.entries(node.props)) {
          // Convert key to PascalCase
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
}

export const useBackend = () => {
  // Use refs for values that don't need to trigger re-renders
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const [widgetTree, setWidgetTree] = useState<WidgetNode | null>(null);
  const [disconnected, setDisconnected] = useState(false);
  const [removeIvyBranding, setRemoveIvyBranding] = useState(false);
  const { toast } = useToast();
  
  // Store these in refs since they're static and don't change
  const appIdRef = useRef(getAppId());
  const appArgsRef = useRef(getAppArgs());
  const parentIdRef = useRef(getParentId());
  const machineIdRef = useRef(getMachineId());
  const ivyHostRef = useRef(getIvyHost());

  // Parse and log XML only when widgetTree changes
  useEffect(() => {
    if (!widgetTree) return;
    
    try {
      const xmlString = widgetTreeToXml(widgetTree);
      if (!xmlString) return;
      
      const parser = new DOMParser();
      const xml = parser.parseFromString(xmlString, 'application/xml');
      const parserError = xml.querySelector('parsererror');
      if (parserError) return;
      
      console.log(xml);
    } catch (error) {
      // Silently ignore errors
    }
  }, [widgetTree]);

  // Memoize handlers to prevent recreation on each render
  const handleRefreshMessage = useCallback((message: RefreshMessage) => {
    console.log("Refresh", message);
    setRemoveIvyBranding(message.removeIvyBranding);
    setWidgetTree(message.widgets);
  }, []);

  const handleUpdateMessage = useCallback((message: UpdateMessage) => {
    console.log("Update", message);
    setWidgetTree(currentTree => {
      if (!currentTree) return null;
      
      // Create a shallow copy to trigger React's state update
      const newWidgetTree = { ...currentTree };
      
      message.forEach((update) => {
        if (update.indices.length === 0) {
          // Apply patch to root node
          applyPatch(newWidgetTree, update.patch);
        } else {
          // Navigate to the target node
          let parent = newWidgetTree;
          const lastIndex = update.indices.length - 1;
          
          for (let i = 0; i < lastIndex; i++) {
            parent = parent.children![update.indices[i]];
          }
          
          // Apply patch to the target node
          applyPatch(parent.children![update.indices[lastIndex]], update.patch);
        }
      });
      
      return newWidgetTree;
    });
  }, []);

  const handleHotReloadMessage = useCallback(() => {
    console.log("HotReload");
    connectionRef.current?.invoke("HotReload")
      .catch((err) => console.error("SignalR Error:", err));
  }, []);

  const handleSetJwt = useCallback(async (jwt: string | null) => {
    try {
      const response = await fetch(`${ivyHostRef.current}/auth/set-jwt`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(jwt),
        credentials: 'include'
      });
      
      if (response.ok) {
        window.location.reload();
      }
    } catch (error) {
      console.error("Error setting JWT:", error);
    }
  }, []);

  const handleSetTheme = useCallback((theme: string) => {
    const normalizedTheme = theme.toLowerCase();
    if (["dark", "light", "system"].includes(normalizedTheme)) {
      setThemeGlobal(normalizedTheme as "dark" | "light" | "system");
    } else {
      console.error(`Invalid theme value received: ${theme}`);
    }
  }, []);

  // Setup connection once
  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${ivyHostRef.current}/messages?appId=${appIdRef.current ?? ""}&appArgs=${appArgsRef.current ?? ""}&machineId=${machineIdRef.current}&parentId=${parentIdRef.current ?? ""}`)
      .withAutomaticReconnect()
      .build();
    
    connectionRef.current = newConnection;
    
    // Connect and setup event handlers
    newConnection.start()
      .then(() => {
        console.log("Connected!");
        
        // Setup event handlers
        newConnection.on("Refresh", handleRefreshMessage);
        newConnection.on("Update", handleUpdateMessage);
        newConnection.on("Toast", (message) => {
          console.log("Toast", message);
          toast(message);
        });
        newConnection.on("$SetChatPanelUrl", (chatPanelUrl: string | null) => {
          window.parent.postMessage({ type: '$SetChatPanelUrl', url: chatPanelUrl }, '*');
        });
        newConnection.on("SetJwt", handleSetJwt);
        newConnection.on("SetTheme", handleSetTheme);
        newConnection.on("CopyToClipboard", (text: string) => {
          navigator.clipboard.writeText(text);
        });
        newConnection.on("OpenUrl", (url: string) => {
          window.open(url, '_blank');
        });
        newConnection.on("HotReload", handleHotReloadMessage);
        
        // Connection status handlers
        newConnection.onreconnecting(() => setDisconnected(true));
        newConnection.onreconnected(() => setDisconnected(false));
        newConnection.onclose(() => setDisconnected(true));
      })
      .catch((err) => console.error("Connection failed: ", err));
    
    // Cleanup function
    return () => {
      const conn = connectionRef.current;
      if (conn) {
        conn.off("Refresh");
        conn.off("Update");
        conn.off("Toast");
        conn.off("CopyToClipboard");
        conn.off("HotReload");
        conn.off("$SetChatPanelUrl");
        conn.off("SetJwt");
        conn.off("SetTheme");
        conn.off("OpenUrl");
        conn.off("reconnecting");
        conn.off("reconnected");
        conn.off("close");
        conn.stop().catch(console.error);
      }
    };
  }, []); // Empty dependency array ensures this runs only once

  // Create a stable event handler reference
  const eventHandler = useCallback<WidgetEventHandlerType>((eventName, widgetId, args) => {
    console.log("Event", eventName, widgetId, args);
    connectionRef.current?.invoke("Event", eventName, widgetId, args)
      .catch((err) => console.error("SignalR Error:", err));
  }, []);

  return { 
    connection: connectionRef.current, 
    widgetTree, 
    eventHandler, 
    disconnected, 
    removeIvyBranding 
  };
};