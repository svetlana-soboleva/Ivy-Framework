import React, { useState, useEffect, useCallback, useMemo } from 'react';

import Icon from '@/components/Icon';
import { useEventHandler } from '@/components/EventHandlerContext';
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from '@/components/ui/collapsible';
import { ChevronRight, PanelLeftClose, PanelLeftOpen } from 'lucide-react';
import { MenuItem, WidgetEventHandlerType } from '@/types/widgets';
import { useFocusable } from '@/hooks/use-focus-management';

interface SidebarLayoutWidgetProps {
  slots?: {
    SidebarHeader?: React.ReactNode[];
    SidebarContent?: React.ReactNode[];
    SidebarFooter?: React.ReactNode[];
    MainContent: React.ReactNode[];
  };
}

export const SidebarLayoutWidget: React.FC<SidebarLayoutWidgetProps> = ({
  slots,
}) => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);

  return (
    <div className="flex flex-row h-full w-full remove-parent-padding">
      <div className="flex h-full w-full">
        {/* Custom Sidebar with Slide Animation */}
        <div
          className={`flex h-full w-64 flex-col bg-sidebar text-sidebar-foreground border-r border-border transition-transform duration-300 ease-in-out relative ${
            isSidebarOpen ? 'translate-x-0' : '-translate-x-full'
          }`}
        >
          {slots?.SidebarHeader && (
            <div className="flex flex-col shrink-0 border-b p-2 space-y-4">
              {slots.SidebarHeader}
            </div>
          )}
          {slots?.SidebarContent && (
            <div className="flex-1 overflow-hidden">
              <div className="h-full overflow-y-auto p-2">
                {slots.SidebarContent}
              </div>
            </div>
          )}
          {slots?.SidebarFooter && (
            <div className="flex h-16 shrink-0 items-center border-t px-4">
              {slots.SidebarFooter}
            </div>
          )}
        </div>

        {/* Toggle Button - Outside Sidebar but moves with it */}
        <button
          onClick={() => setIsSidebarOpen(!isSidebarOpen)}
          className="absolute top-4 z-50 p-2 rounded-md bg-background border border-border hover:bg-accent hover:text-accent-foreground cursor-pointer transition-all duration-200"
          style={{
            left: isSidebarOpen ? 'calc(16rem + 32px)' : '1rem',
            transition: 'left 300ms ease-in-out',
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

        {/* Main Content */}
        <div className="flex-1 overflow-hidden relative">
          {slots?.MainContent}
        </div>
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
              className="flex w-full items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium hover:bg-accent hover:text-accent-foreground cursor-pointer"
              onClick={() => {
                // For items with children, toggle the collapsible state
                // Only try to navigate if the item has a tag
                if (item.tag) {
                  onItemClick(item);
                }
              }}
              onMouseDown={e => onCtrlRightMouseClick(e, item)}
            >
              <Icon name={item.icon} size={20} />
              <span>{item.label}</span>
              <ChevronRight className="ml-auto transition-transform group-data-[state=open]/collapsible:rotate-90" />
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
          className="flex w-full items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium hover:bg-accent hover:text-accent-foreground cursor-pointer"
          onClick={() => onItemClick(item)}
          onMouseDown={e => onCtrlRightMouseClick(e, item)}
        >
          <Icon name={item.icon} size={20} />
          <span>{item.label}</span>
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
          <div key={item.label} className="space-y-1">
            <h4 className="px-3 py-2 text-xs font-semibold text-muted-foreground">
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
              className="flex w-full items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium hover:bg-accent hover:text-accent-foreground cursor-pointer"
              onClick={() => onItemClick(item)}
              onMouseDown={e => onCtrlRightMouseClick(e, item)}
            >
              <Icon name={item.icon} size={20} />
              <span>{item.label}</span>
            </button>
          </li>
        );
      } else {
        return (
          <li key={item.tag}>
            <button
              className="flex w-full items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium hover:bg-accent hover:text-accent-foreground cursor-pointer"
              onClick={() => onItemClick(item)}
              onMouseDown={e => onCtrlRightMouseClick(e, item)}
            >
              <Icon name={item.icon} size={20} />
              <span>{item.label}</span>
            </button>
          </li>
        );
      }
    }
  });
};

export const sidebarMenuRef = React.createRef<HTMLDivElement>();

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
    return items.map(item => {
      if (item.children && item.children.length > 0) {
        return (
          <div key={item.label} className="space-y-1">
            <h4 className="px-3 py-2 text-xs font-semibold text-muted-foreground">
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
              className={`flex w-full items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium hover:bg-accent hover:text-accent-foreground cursor-pointer text-sm ${
                isActive ? 'bg-accent text-accent-foreground' : ''
              }`}
              tabIndex={-1} // Not focusable
              onClick={() =>
                item.tag && eventHandler('OnSelect', id, [item.tag])
              }
              onMouseEnter={() => {
                if (searchActive) {
                  setSelectedIndex(flatIdx);
                }
              }}
            >
              <Icon name={item.icon} size={20} />
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
          <div className="flex items-center justify-center p-4 text-sm text-muted-foreground">
            No results found
          </div>
        )
      ) : (
        renderMenuItems(items, eventHandler, id, 0)
      )}
    </div>
  );
};
