import { useEventHandler } from '@/components/EventHandlerContext';
import MarkdownRenderer from '@/components/MarkdownRenderer';
import React, { useCallback } from 'react';

interface MarkdownWidgetProps {
  id: string;
  content: string;
}

const MarkdownWidget: React.FC<MarkdownWidgetProps> = ({ id, content }) => {
  const eventHandler = useEventHandler();

  const handleLinkClick = useCallback(
    (href: string) => eventHandler('OnLinkClick', id, [href]),
    [eventHandler, id]
  );

  return <MarkdownRenderer content={content} onLinkClick={handleLinkClick} />;
};

export default React.memo(MarkdownWidget);
