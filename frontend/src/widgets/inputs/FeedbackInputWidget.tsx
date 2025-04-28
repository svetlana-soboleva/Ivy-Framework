import { EmojiRating } from '@/components/EmojiRating';
import { useEventHandler } from '@/components/EventHandlerContext';
import { StarRating } from '@/components/StarRating';
import ThumbsRating from '@/components/ThumbsRating';
import React, { useCallback } from 'react';

interface FeedbackInputWidgetProps {
  id: string;
  value: number;
  variant: 'Thumbs' | 'Emojis' | 'Stars';
  disabled: boolean;
  events: string[];
}

export const FeedbackInputWidget: React.FC<FeedbackInputWidgetProps> = ({
  id,
  value,
  variant,
  disabled,
  events
}) => {
  const eventHandler = useEventHandler();

  const handleChange = useCallback((e:number) => {
    if(!events.includes("OnChange")) return;
    if(disabled) return;
    eventHandler("OnChange", id, [e]);
  }, [id, disabled]);

  if(variant === 'Thumbs') {
    return (
      <ThumbsRating disabled={disabled} value={value} onRate={handleChange}/>
    );
  }

  if(variant === 'Emojis') {
    return (
      <EmojiRating disabled={disabled} value={value} onRate={handleChange}/>
    );
  }

  if(variant === 'Stars') {
    return (
      <StarRating disabled={disabled} value={value} onRate={handleChange}/>
    );
  }

};