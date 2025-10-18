import React, {
  useState,
  useEffect,
  useCallback,
  useMemo,
  useRef,
} from 'react';

import Icon from '@/components/Icon';
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from '@/components/ui/collapsible';
import { ScrollArea } from '@/components/ui/scroll-area';
import { ChevronRight, PanelLeftClose, PanelLeftOpen } from 'lucide-react';
import { MenuItem, WidgetEventHandlerType } from '@/types/widgets';
import { useFocusable } from '@/hooks/use-focus-management';
import { sidebarMenuRef } from './sidebar-refs';
import { useEventHandler } from '@/components/event-handler';
import { cn } from '@/lib/utils';

interface SidebarLayoutWidgetProps {
  slots?: {
    SidebarHeader?: React.ReactNode[];
    SidebarContent?: React.ReactNode[];
    SidebarFooter?: React.ReactNode[];
    MainContent: React.ReactNode[];
  };
  showToggleButton?: boolean;
  autoCollapseThreshold?: number; // Width threshold for auto-collapse (default: 768px)
  mainAppSidebar?: boolean;
  mainContentPadding?: number; // Padding for main content area (default: 2)
}

// Helper function to check if a slot has meaningful content by checking props.children
const hasContent = (slot?: React.ReactNode[]): boolean => {
  if (!slot || slot.length === 0) return false;

  return slot.some(node => {
    if (node === null || node === undefined) return false;
    if (typeof node === 'string') return node.trim().length > 0;
    if (typeof node === 'number') return true;
    if (React.isValidElement(node)) {
      const props = node.props as { children?: React.ReactNode };
      if (props.children === null || props.children === undefined) return false;
      if (typeof props.children === 'string')
        return props.children.trim().length > 0;
      if (Array.isArray(props.children)) return props.children.length > 0;
      return true;
    }
    return false;
  });
};

export const SidebarLayoutWidget: React.FC<SidebarLayoutWidgetProps> = ({
  slots,
  showToggleButton = true,
  autoCollapseThreshold = 768,
  mainAppSidebar = false,
  mainContentPadding,
}) => {
  // Initialize sidebar state based on current window width (only for main app sidebar)
  const getInitialSidebarState = () => {
    if (!mainAppSidebar) return true;

    // Check if we're in a browser environment
    if (typeof window !== 'undefined') {
      return window.innerWidth >= autoCollapseThreshold;
    }

    return true; // Default to open if we can't determine width
  };

  const [isSidebarOpen, setIsSidebarOpen] = useState(getInitialSidebarState);
  const [isManuallyToggled, setIsManuallyToggled] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);
  const resizeObserverRef = useRef<ResizeObserver | null>(null);

  // Handle manual toggle
  const handleManualToggle = useCallback(() => {
    setIsSidebarOpen(prev => !prev);
    setIsManuallyToggled(true);
  }, []);

  // Auto-collapse/expand based on width (only for main app sidebar)
  useEffect(() => {
    if (!containerRef.current || !mainAppSidebar) return;

    const handleResize = (entries: ResizeObserverEntry[]) => {
      const entry = entries[0];
      if (!entry) return;

      const containerWidth = entry.contentRect.width;

      // Only auto-collapse/expand if user hasn't manually toggled
      if (!isManuallyToggled) {
        if (containerWidth < autoCollapseThreshold) {
          setIsSidebarOpen(false);
        } else {
          setIsSidebarOpen(true);
        }
      }
    };

    resizeObserverRef.current = new ResizeObserver(handleResize);
    resizeObserverRef.current.observe(containerRef.current);

    return () => {
      if (resizeObserverRef.current) {
        resizeObserverRef.current.disconnect();
      }
    };
  }, [autoCollapseThreshold, isManuallyToggled, mainAppSidebar]);

  // Reset manual toggle flag when width changes significantly (only for main app sidebar)
  useEffect(() => {
    if (!containerRef.current || !mainAppSidebar) return;

    const handleResize = (entries: ResizeObserverEntry[]) => {
      const entry = entries[0];
      if (!entry) return;

      const containerWidth = entry.contentRect.width;

      // Reset manual toggle flag when width changes significantly
      // This allows auto-behavior to resume after significant size changes
      if (
        containerWidth < autoCollapseThreshold * 0.8 ||
        containerWidth > autoCollapseThreshold * 1.2
      ) {
        setIsManuallyToggled(false);
      }
    };

    const observer = new ResizeObserver(handleResize);
    observer.observe(containerRef.current);

    return () => {
      observer.disconnect();
    };
  }, [autoCollapseThreshold, mainAppSidebar]);

  return (
    <div
      ref={containerRef}
      className="grid h-full w-full remove-parent-padding"
      style={{
        gridTemplateColumns: isSidebarOpen ? '16rem 1fr' : '0 1fr',
        transition: 'grid-template-columns 300ms ease-in-out',
      }}
    >
      {/* Custom Sidebar with Slide Animation */}
      <div
        className={`flex h-full w-[256px] flex-col bg-sidebar text-sidebar-foreground border-r border-border transition-transform duration-300 ease-in-out relative overflow-hidden ${
          isSidebarOpen ? 'translate-x-0' : '-translate-x-full'
        }`}
      >
        {hasContent(slots?.SidebarHeader) && (
          <div className="flex flex-col shrink-0 p-2 space-y-4">
            {slots?.SidebarHeader}
          </div>
        )}
        {slots?.SidebarContent && (
          <div className="flex-1 overflow-hidden">
            <ScrollArea className="h-full w-full">
              <div className="p-2 space-y-2">{slots.SidebarContent}</div>
            </ScrollArea>
          </div>
        )}
        {hasContent(slots?.SidebarFooter) && (
          <div className="flex flex-col shrink-0">
            <div className="flex flex-col px-4 py-3 gap-4 min-h-0">
              {slots?.SidebarFooter}
            </div>
          </div>
        )}
      </div>

      {/* Toggle Button - Only show for main app sidebar */}
      {showToggleButton && mainAppSidebar && (
        <button
          onClick={handleManualToggle}
          className="absolute top-2 z-50 p-2 rounded-md bg-background hover:bg-sidebar-accent hover:text-accent-foreground cursor-pointer transition-all duration-200"
          style={{
            left: isSidebarOpen ? 'calc(16rem + 8px)' : '8px',
            transition: 'left 300ms ease-in-out',
            transform: 'translateX(0)', // Ensure button moves with its parent sidebar
          }}
          aria-label={isSidebarOpen ? 'Close sidebar' : 'Open sidebar'}
        >
          <div className="transition-transform duration-300 ease-in-out">
            {isSidebarOpen ? (
              <PanelLeftClose className="h-4 w-4" />
            ) : (
              <PanelLeftOpen className="h-4 w-4" />
            )}
          </div>
        </button>
      )}

      {/* Main Content - Always takes full remaining width */}
      <div
        className={cn(
          `relative h-full overflow-auto`,
          !mainAppSidebar ? `p-${mainContentPadding}` : ''
        )}
      >
        {slots?.MainContent}
      </div>
    </div>
  );
};

interface SidebarMenuWidgetProps {
  id: string;
  items: MenuItem[];
  searchActive?: boolean;
}

type FlatMenuItem = MenuItem & { isGroup?: boolean };
const flattenMenuItems = (
  items: MenuItem[],
  parentExpanded = true
): FlatMenuItem[] => {
  let flat: FlatMenuItem[] = [];
  for (const item of items) {
    if (
      item.children &&
      item.children.length > 0 &&
      (parentExpanded || item.expanded)
    ) {
      flat.push({ ...(item as MenuItem), isGroup: true });
      flat = flat.concat(flattenMenuItems(item.children, true));
    } else {
      flat.push(item);
    }
  }
  return flat;
};

const CollapsibleMenuItem: React.FC<{
  item: MenuItem;
  eventHandler: WidgetEventHandlerType;
  widgetId: string;
  level: number;
}> = ({ item, eventHandler, widgetId, level }) => {
  const [isOpen, setIsOpen] = useState(item.expanded);

  const onItemClick = (item: MenuItem) => {
    if (!item.tag) return;
    eventHandler('OnSelect', widgetId, [item.tag]);
  };

  const onCtrlRightMouseClick = (e: React.MouseEvent, item: MenuItem) => {
    if (e.ctrlKey && e.button === 2 && !!item.tag) {
      e.preventDefault();
      eventHandler('OnCtrlRightClickSelect', widgetId, [item.tag]);
    }
  };

  if (!!item.children && item.children!.length > 0) {
    return (
      <Collapsible
        className="group/collapsible"
        key={item.label}
        open={isOpen}
        onOpenChange={setIsOpen}
      >
        <li className="relative">
          <CollapsibleTrigger asChild>
            <button
              className="flex w-full items-center gap-2 rounded-lg p-2 text-large-label hover:bg-sidebar-accent hover:text-accent-foreground cursor-pointer h-8"
              onClick={() => {
                // For items with children, toggle the collapsible state
                // Only try to navigate if the item has a tag
                if (item.tag) {
                  onItemClick(item);
                }
              }}
              onMouseDown={e => onCtrlRightMouseClick(e, item)}
            >
              <Icon name={item.icon} size={16} />
              <span className="text-sm">{item.label}</span>
              <ChevronRight className="ml-auto h-4 w-4 transition-transform group-data-[state=open]/collapsible:rotate-90" />
            </button>
          </CollapsibleTrigger>
          <CollapsibleContent>
            <ul className="mt-1 space-y-1 px-3">
              {item.children &&
                renderMenuItems(
                  item.children!,
                  eventHandler,
                  widgetId,
                  level + 1
                )}
            </ul>
          </CollapsibleContent>
        </li>
      </Collapsible>
    );
  } else {
    return (
      <li key={item.label}>
        <button
          className="flex w-full items-center gap-2 rounded-lg p-2 text-large-label hover:bg-sidebar-accent hover:text-accent-foreground cursor-pointer h-8"
          onClick={() => onItemClick(item)}
          onMouseDown={e => onCtrlRightMouseClick(e, item)}
        >
          <Icon name={item.icon} size={16} />
          <span className="text-sm">{item.label}</span>
        </button>
      </li>
    );
  }
};

const renderMenuItems = (
  items: MenuItem[],
  eventHandler: WidgetEventHandlerType,
  widgetId: string,
  level: number
) => {
  const onItemClick = (item: MenuItem) => {
    if (!item.tag) return;
    eventHandler('OnSelect', widgetId, [item.tag]);
  };

  const onCtrlRightMouseClick = (e: React.MouseEvent, item: MenuItem) => {
    if (e.ctrlKey && e.button === 2 && !!item.tag) {
      e.preventDefault();
      eventHandler('OnCtrlRightClickSelect', widgetId, [item.tag]);
    }
  };

  return items.map(item => {
    if ('children' in item) {
      if (level === 0) {
        return (
          <div key={item.label} className="space-y-1 mt-6 first:mt-0">
            <h4 className="px-3 py-2 text-small-label text-muted-foreground mb-0">
              {item.label}
            </h4>
            <ul className="space-y-1">
              {item.children &&
                renderMenuItems(item.children!, eventHandler, widgetId, 1)}
            </ul>
          </div>
        );
      } else {
        return (
          <CollapsibleMenuItem
            key={item.label}
            item={item}
            eventHandler={eventHandler}
            widgetId={widgetId}
            level={level}
          />
        );
      }
    } else {
      if (level === 0) {
        return <></>;
      }
      if (level === 1) {
        return (
          <li key={item.tag}>
            <button
              className="flex w-full items-center gap-2 rounded-lg p-2 text-body hover:bg-sidebar-accent hover:text-accent-foreground cursor-pointer h-8"
              onClick={() => onItemClick(item)}
              onMouseDown={e => onCtrlRightMouseClick(e, item)}
            >
              <Icon name={item.icon} size={16} />
              <span className="text-sm">{item.label}</span>
            </button>
          </li>
        );
      } else {
        return (
          <li key={item.tag}>
            <button
              className="flex w-full items-center gap-2 rounded-lg p-2 text-body hover:bg-sidebar-accent hover:text-accent-foreground cursor-pointer h-8"
              onClick={() => onItemClick(item)}
              onMouseDown={e => onCtrlRightMouseClick(e, item)}
            >
              <Icon name={item.icon} size={16} />
              <span className="text-sm">{item.label}</span>
            </button>
          </li>
        );
      }
    }
  });
};

export const SidebarMenuWidget: React.FC<SidebarMenuWidgetProps> = ({
  id,
  items,
  searchActive = false,
}) => {
  const eventHandler = useEventHandler();
  const [selectedIndex, setSelectedIndex] = useState(0);
  // Register only the sidebar menu container with useFocusable
  const { ref: focusRef } = useFocusable('sidebar-navigation', 1);

  const flatItems: FlatMenuItem[] = useMemo(() => {
    return searchActive ? flattenMenuItems(items).filter(i => !i.isGroup) : [];
  }, [searchActive, items]);

  useEffect(() => {
    setSelectedIndex(0);
  }, [searchActive]);

  const handleMenuKeyDown = useCallback(
    (e: React.KeyboardEvent) => {
      if (!searchActive || flatItems.length === 0) return;
      if (e.key === 'ArrowDown') {
        setSelectedIndex(idx => Math.min(idx + 1, flatItems.length - 1));
        e.preventDefault();
      } else if (e.key === 'ArrowUp') {
        setSelectedIndex(idx => Math.max(idx - 1, 0));
        e.preventDefault();
      } else if (e.key === 'Enter') {
        const item = flatItems[selectedIndex];
        if (item && item.tag) {
          eventHandler('OnSelect', id, [item.tag]);
        }
        e.preventDefault();
      }
    },
    [searchActive, flatItems, selectedIndex, eventHandler, id]
  );

  const renderMenuItemsWithHighlight = (
    items: MenuItem[],
    level: number,
    flatIdxRef: { current: number }
  ) => {
    const onCtrlRightMouseClick = (e: React.MouseEvent, item: MenuItem) => {
      if (e.ctrlKey && e.button === 2 && !!item.tag) {
        e.preventDefault();
        eventHandler('OnCtrlRightClickSelect', id, [item.tag]);
      }
    };

    return items.map(item => {
      if (item.children && item.children.length > 0) {
        return (
          <div key={item.label} className="space-y-1 mt-6 first:mt-0">
            <h4 className="px-3 py-2 text-small-label text-muted-foreground mb-0">
              {item.label}
            </h4>
            <ul className="space-y-1">
              {renderMenuItemsWithHighlight(
                item.children,
                level + 1,
                flatIdxRef
              )}
            </ul>
          </div>
        );
      } else {
        const flatIdx = flatIdxRef.current;
        flatIdxRef.current++;
        const isActive = searchActive && flatIdx === selectedIndex;
        return (
          <li key={item.tag}>
            <button
              className={`flex w-full items-center gap-2 rounded-lg p-2 text-sm hover:bg-sidebar-accent hover:text-accent-foreground cursor-pointer h-8 ${
                isActive ? 'bg-accent text-accent-foreground' : ''
              }`}
              tabIndex={-1} // Not focusable
              onClick={() =>
                item.tag && eventHandler('OnSelect', id, [item.tag])
              }
              onMouseDown={e => onCtrlRightMouseClick(e, item)}
              onMouseEnter={() => {
                if (searchActive) {
                  setSelectedIndex(flatIdx);
                }
              }}
            >
              <Icon name={item.icon} size={16} />
              <span className="text-sm">{item.label}</span>
            </button>
          </li>
        );
      }
    });
  };

  const flatIdxRef = { current: 0 };
  return (
    <div
      ref={el => {
        focusRef(el);
        (
          sidebarMenuRef as React.MutableRefObject<HTMLDivElement | null>
        ).current = el;
      }}
      tabIndex={0}
      onFocus={() => {
        if (searchActive && flatItems.length > 0) setSelectedIndex(0);
      }}
      onKeyDown={handleMenuKeyDown}
      style={{ outline: 'none' }}
      data-sidebar-menu-widget
    >
      {searchActive ? (
        flatItems.length > 0 ? (
          renderMenuItemsWithHighlight(items, 0, flatIdxRef)
        ) : (
          <div className="flex items-center justify-center p-4 text-descriptive text-muted-foreground">
            No results found
          </div>
        )
      ) : (
        renderMenuItems(items, eventHandler, id, 0)
      )}
    </div>
  );
};
