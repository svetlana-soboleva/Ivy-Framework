import * as React from 'react';
import { ScrollArea, ScrollBar } from "@/components/ui/scroll-area";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import Icon from '@/components/Icon';
import { RotateCw, X, ChevronDown } from 'lucide-react';
import { useEventHandler } from '@/components/EventHandlerContext';
import { Badge } from '@/components/ui/badge';
import { cn } from '@/lib/utils';
import { getHeight, getPadding, getWidth } from '@/lib/styles';
import {
  DropdownMenu,
  DropdownMenuTrigger,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator
} from '@/components/ui/dropdown-menu';
import { DndContext, closestCenter, DragOverlay, PointerSensor, useSensor, useSensors } from '@dnd-kit/core';
import { SortableContext, useSortable, arrayMove, verticalListSortingStrategy } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { Button } from '@/components/ui/button';

interface TabWidgetProps {
  children: React.ReactNode[];
  title: string;
  id: string;
  badge?:string;
  icon?:string;
}

export const TabWidget: React.FC<TabWidgetProps> = ({
  children
}) => {
  return (
    <div className='h-full'> 
      {children} 
    </div>
  );
};

TabWidget.displayName = 'TabWidget';

interface TabsLayoutWidgetProps {
  id: string;
  variant?: "Tabs" | "Content";
  removeParentPadding?: boolean;
  selectedIndex?: number;
  children: React.ReactElement<TabWidgetProps>[];
  events: string[];
  width?: string;
  height?: string;
  padding?: string;
}

// Helper component for sortable tab triggers
interface SortableTabTriggerProps {
  id: string;
  value: string;
  onClick: () => void;
  onMouseDown: (e: React.MouseEvent) => void;
  className?: string;
  onMouseEnter?: () => void; // add this
  onMouseLeave?: () => void; // add this
  children: React.ReactNode;
}

function SortableTabTrigger({ id, value, onClick, onMouseDown, className, children, ...props }: SortableTabTriggerProps) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({ id });
  const style = {
    transform: transform ? `translate3d(${transform.x}px, 0, 0)` : undefined,
    transition,
    opacity: isDragging ? 0.5 : 1,
    zIndex: isDragging ? 100 : undefined,
  };
  return (
    <TabsTrigger
      ref={setNodeRef}
      style={style}
      value={value}
      onClick={onClick}
      onMouseDown={onMouseDown}
      className={className}
      {...attributes}
      {...listeners}
      {...props}
    >
      {children}
    </TabsTrigger>
  );
}

// Helper component for sortable dropdown menu items
interface SortableDropdownMenuItemProps {
  id: string;
  children: React.ReactNode;
  onClick: () => void;
  isActive?: boolean;
}
function SortableDropdownMenuItem({ id, children, onClick, isActive }: SortableDropdownMenuItemProps) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({ id });
  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
    zIndex: isDragging ? 100 : undefined,
    cursor: 'grab',
    width: '100%',
    display: 'flex',
    alignItems: 'center',
  };
  return (
    <div
      ref={setNodeRef}
      style={style}
      {...attributes}
      {...listeners}
      onClick={onClick}
      className={cn(
        "group w-full flex items-center px-2 py-1.5 text-sm font-normal text-accent-foreground cursor-pointer select-none rounded-sm transition-colors hover:bg-accent hover:text-accent-foreground",
        isActive && "bg-accent text-accent-foreground"
      )}
      tabIndex={0}
    >
      <span className="truncate text-left">{children}</span>
      <div className="ml-auto flex items-center gap-1 invisible group-hover:visible group-hover:relative">
        {/* Close button */}
        <button
          type="button"
          tabIndex={-1}
          className="opacity-60 p-1 hover:opacity-100 transition-colors"
          onClick={e => {
            e.stopPropagation();
            if (typeof window !== 'undefined') {
              const customEvent = new CustomEvent('tab-close', { detail: { id } });
              window.dispatchEvent(customEvent);
            }
          }}
        >
          <X className="w-3 h-3" />
        </button>
      </div>
    </div>
  );
}

export const TabsLayoutWidget = ({
  id,
  children,
  events,
  selectedIndex,
  removeParentPadding,
  width,
  height,
  padding,
  variant
}: TabsLayoutWidgetProps) => {
  // Extract tab widgets first
  const tabWidgets = React.Children.toArray(children).filter((child) =>
    React.isValidElement(child) &&
    (child.type as any)?.displayName === 'TabWidget'
  );
  if(tabWidgets.length === 0) return <div className='remove-parent-padding'></div>;

  // All hooks must be called after the early return
  const eventHandler = useEventHandler();
  const tabsLayoutId = id;
  const containerRef = React.useRef<HTMLDivElement>(null);
  const tabsListRef = React.useRef<HTMLDivElement>(null);
  const [tabsOverflowing, setTabsOverflowing] = React.useState(false);
  // Tab order state
  const [hoveredTabId, setHoveredTabId] = React.useState<string | null>(null);

  const tabIds = React.useMemo(
    () => tabWidgets.map(tab => (tab as any).props.id),
    [tabWidgets.length]  // only re-runs when a tab is added/removed
  );

  // 2) Store the "prev known" list in a ref
    const prevTabIdsRef = React.useRef<string[]>(tabIds);

  // 3) Initialize state once
  const [tabOrder, setTabOrder] = React.useState<string[]>(() => tabIds);


  // 4) Sync only on real add/remove
  React.useEffect(() => {
    const prev = prevTabIdsRef.current;
    // If counts differ or any ID went missing/appeared, we've got add/remove
    const added   = tabIds.filter(id => !prev.includes(id));
    const removed = prev.filter(id => !tabIds.includes(id));
    if (added.length || removed.length) {
      // Build a new order:
      //  • take your existing order, filter out removals
      //  • append any brand-new IDs at the end
      setTabOrder(current => [
        ...current.filter(id => !removed.includes(id)),
        ...added
      ]);
      prevTabIdsRef.current = tabIds;
    }
  }, [tabIds]);

  React.useEffect(() => {
    const containerWidth = containerRef.current!.getBoundingClientRect().width;
    const tabsListWidth = tabsListRef.current!.getBoundingClientRect().width;

    console.log("containerWidth:", containerWidth);
    console.log("tabsListWidth:", tabsListWidth);
    if (hoveredTabId === null) {
      setTabsOverflowing(tabsListWidth + 48*2 > containerWidth);
    }

  }, [hoveredTabId, tabOrder]);
  
  // DnD sensors with activation constraint
  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8, // require 8px movement before drag starts
      },
    })
  );
  // Track the currently dragged item for DragOverlay
  const [activeDragId, setActiveDragId] = React.useState<string | null>(null);

  // Map tabOrder to tabWidgets
  const orderedTabWidgets = tabOrder.map(id => tabWidgets.find(tab => (tab as any).props.id === id)).filter(Boolean);

  const [activeTabId, setActiveTabId] = React.useState<string | null>(() => {
    if ((selectedIndex ?? -1) >= 0 && tabOrder[selectedIndex]) {
      return tabOrder[selectedIndex];
    }
    // fallback to first tab if you like:
    return tabOrder[0] ?? null;
  });

  const [loadedTabs, setLoadedTabs] = React.useState<Set<string>>(
    () => new Set()
  );  

  React.useEffect(() => {
    if (activeTabId) {
      setLoadedTabs(prev => {
        const next = new Set(prev);
        next.add(activeTabId);
        return next;
      });
    }
  }, [activeTabId]);


  const showClose = events.includes("OnClose");
  const showRefresh = events.includes("OnRefresh");

  const handleMouseDown = (e: React.MouseEvent, index: number) => {
    if (e.button === 1) {
      e.preventDefault(); 
      eventHandler("OnClose", tabsLayoutId, [index])
    }
  };

  const styles:React.CSSProperties = { 
    ...getWidth(width),
    ...getHeight(height),
  };

  // track previous selectedIndex
  const prevSelRef = React.useRef<number | null>(null);

  React.useEffect(() => {
    if (selectedIndex != null && selectedIndex !== prevSelRef.current) {
      prevSelRef.current = selectedIndex;
      if (tabOrder[selectedIndex]) {
        setActiveTabId(tabOrder[selectedIndex]);
      }
    }
  }, [selectedIndex]);  // remove tabOrder from deps

  // DnD handler
  const handleDragEnd = (event: any) => {
    const { active, over } = event;
    if (active && over && active.id !== over.id) {
      setTabOrder((items) => {
        const oldIndex = items.indexOf(active.id);
        const newIndex = items.indexOf(over.id);
        return arrayMove(items, oldIndex, newIndex);
      });
    }
  };

  React.useEffect(() => {
    function handleTabClose(e: any) {
      if (!e.detail?.id) return;
      const idx = tabOrder.indexOf(e.detail.id);
      if (idx !== -1) {
        eventHandler("OnClose", tabsLayoutId, [idx]);
      }
    }
    window.addEventListener('tab-close', handleTabClose);
    return () => window.removeEventListener('tab-close', handleTabClose);
  }, [tabOrder, eventHandler, tabsLayoutId]);

  React.useEffect(() => {
    function handleTabRefresh(e: any) {
      if (!e.detail?.id) return;
      const idx = tabOrder.indexOf(e.detail.id);
      if (idx !== -1) {
        eventHandler("OnRefresh", tabsLayoutId, [idx]);
      }
    }
    window.addEventListener('tab-refresh', handleTabRefresh);
    return () => window.removeEventListener('tab-refresh', handleTabRefresh);
  }, [tabOrder, eventHandler, tabsLayoutId]);

  return (
    <Tabs value={activeTabId} style={styles} className={
      cn(
        removeParentPadding && 'remove-parent-padding',
        'flex flex-col h-full'
      )}>
      <div className="flex-shrink-0">
        <div className="relative pl-12 pr-12 before:absolute before:inset-x-0 before:bottom-0 before:h-px before:bg-border before:z-0" ref={containerRef}>
         <ScrollArea className="w-full" /* bg-green-500 */ /* style={{
  backgroundImage: `repeating-linear-gradient(
    45deg,
    #3b82f6 0px,
    #3b82f6 10px,
    #60a5fa 10px,
    #60a5fa 20px
  )`
}} */ scrollbarPosition="top" horizontalScroll>
          <div /* className="bg-orange-500" */>
            <DndContext collisionDetection={closestCenter} onDragEnd={handleDragEnd} sensors={sensors} /* className="bg-orange-500" */>
              <SortableContext items={tabOrder}>
                <TabsList
                  ref={tabsListRef}
                  className={cn(
                    "relative h-auto w-max min-w-full gap-0.5 mt-3 bg-transparent p-0 flex justify-start",
                  )}>
                  {orderedTabWidgets.map((tabWidget, index) => {
                    if (React.isValidElement(tabWidget)) {
                      const { title, id, icon, badge  } = tabWidget.props as TabWidgetProps;
                      return (
                        <SortableTabTrigger
                          key={id}
                          id={id}
                          value={id}
                          onMouseEnter={() => setHoveredTabId(id)}
                          onMouseLeave={() => setHoveredTabId(prev => prev === id ? null : prev)}
                          onClick={() => {
                            setLoadedTabs(prev => {
                              const next = new Set(prev);
                              next.add(id);
                              return next;
                            });
                            setActiveTabId(id);
                            eventHandler("OnSelect", tabsLayoutId, [tabOrder.indexOf(id)]);
                          }}                          
                          onMouseDown={(e: React.MouseEvent) => handleMouseDown(e, tabOrder.indexOf(id))}
                          className={cn(
                            "group overflow-hidden rounded-b-none py-2 data-[state=active]:z-10 data-[state=active]:shadow-none",
                            variant === "Tabs" && "border-x border-t border-border bg-muted",
                            variant === "Content" && "border-b-2 data-[state=active]:border-b-primary"
                          )}
                        >
                          {icon && <Icon
                            name={icon}
                            className="-ms-0.5 me-1.5 opacity-60"
                            size={16}
                            aria-hidden="true"
                          />}
                          <span>{title}</span>
                          {badge && <Badge
                            variant="default"
                            className={cn(
                              "ml-2",
                              "w-min",
                              "whitespace-nowrap",
                              "visible",
                              (showClose || showRefresh) && "group-hover:hidden"
                            )}
                          >{badge}</Badge>}
                          {!activeDragId && activeTabId === id && <div className="absolute ml-2 items-center flex gap-0 relative">
                            {showRefresh && <a
                              onClick={(e) => {
                                e.stopPropagation();
                                eventHandler("OnRefresh", tabsLayoutId, [tabOrder.indexOf(id)])
                              }}
                              className="opacity-60 p-1 rounded-full hover:bg-gray-200 hover:opacity-100 transition-colors"
                            >
                              <RotateCw className="w-3 h-3" />
                            </a>}
                            {showClose && <a
                              onClick={(e) => {
                                e.stopPropagation();
                                eventHandler("OnClose", tabsLayoutId, [tabOrder.indexOf(id)])
                              }}
                              className="opacity-60 p-1 rounded-full  hover:bg-gray-200 hover:opacity-100 transition-colors"
                            >
                              <X className="w-3 h-3" />
                            </a>}
                          </div>}
                        </SortableTabTrigger>
                      );
                    }
                    return null;
                  })}
                </TabsList>
              </SortableContext>
            </DndContext>
            </div>
            <ScrollBar orientation="horizontal" className="invisible-scrollbar" />
            <ScrollBar orientation="horizontal" scrollbarPosition="top" className="invisible-scrollbar" />
          </ScrollArea>
          {tabsOverflowing && (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button
                  variant="ghost"
                  size="icon"
                  className="absolute right-0 top-1/2 -translate-y-1/2 h-7 w-7 bg-transparent z-10 mr-3"
                  aria-label="Show more tabs"
                  type="button"
                >
                  <ChevronDown className="w-5 h-5" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DndContext collisionDetection={closestCenter} onDragEnd={handleDragEnd} sensors={sensors}>
                  <SortableContext items={tabOrder} strategy={verticalListSortingStrategy}>
                    <div className="flex flex-col w-48">
                      {orderedTabWidgets.map((tabWidget, index) => {
                        if (React.isValidElement(tabWidget)) {
                          const { title, id } = tabWidget.props as TabWidgetProps;
                          return (
                            <SortableDropdownMenuItem
                              key={id}
                              id={id}
                              onClick={() => {
                                // 1 Mark it as loaded 
                                setLoadedTabs(prev => {
                                  const next = new Set(prev);
                                  next.add(id);
                                  return next;
                                });
                                // 2 Set it as active
                                setActiveTabId(id);
                                // 3 Notify your parent that the active tab has changed
                                eventHandler("OnSelect", tabsLayoutId, [tabOrder.indexOf(id)]);
                              }}
                              isActive={activeTabId === id}
                            >
                              {title}
                            </SortableDropdownMenuItem>
                          );
                        }
                        return null;
                      })}
                    </div>
                  </SortableContext>
                  <DragOverlay>
                    {/** Show a visual clone of the dragged item */}
                    {(() => {
                      const active = orderedTabWidgets.find(tab => (tab as any)?.props.id === activeTabId);
                      if (active && React.isValidElement(active)) {
                        const { title } = active.props as TabWidgetProps;
                        return (
                          <div className="px-3 py-2 bg-muted rounded shadow text-left cursor-pointer select-none">
                            {title}
                          </div>
                        );
                      }
                      return null;
                    })()}
                  </DragOverlay>
                </DndContext>
              </DropdownMenuContent>
            </DropdownMenu>
          )}
        </div>
      </div>
      <div className="flex-1 overflow-hidden">
        {orderedTabWidgets.map((tabWidget) => {
          if (!React.isValidElement(tabWidget)) return null;

          const { id } = tabWidget.props as TabWidgetProps;

          // Only render if the tab has been visited
          if (!loadedTabs.has(id)) return null;

          return (
            <div
              key={id}
              className={cn(
                'h-full overflow-auto',
                activeTabId === id ? 'block' : 'hidden'
              )}
              style={{
                ...getPadding(padding)
              }}
            >
              {tabWidget}
            </div>
          );
        })}
      </div>
    </Tabs>
  );
};
