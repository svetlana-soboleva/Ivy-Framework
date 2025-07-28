import React from 'react';

export const HtmlRenderer: React.FC<{ content: string }> = ({ content }) => {
  // Simple HTML rendering with typography system
  return (
    <div className="text-body" dangerouslySetInnerHTML={{ __html: content }} />
  );
};
