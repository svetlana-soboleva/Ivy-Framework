import { HtmlRenderer } from '@/components/HtmlRenderer';
import React from 'react';

interface HtmlWidgetProps {
  id: string;
  content: string;
}

export const HtmlWidget: React.FC<HtmlWidgetProps> = ({ id, content }) => (
  <HtmlRenderer content={content} key={id} />
);
