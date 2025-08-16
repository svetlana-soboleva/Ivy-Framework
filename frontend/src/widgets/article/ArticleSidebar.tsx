import { TableOfContents } from '@/widgets/article/TableOfContents';
import { GitHubContributors } from '@/widgets/article/GitHubContributors';
import React from 'react';

interface ArticleSidebarProps {
  articleRef: React.RefObject<HTMLElement | null>;
  showToc?: boolean;
  documentSource?: string;
}

export const ArticleSidebar: React.FC<ArticleSidebarProps> = ({
  articleRef,
  showToc,
  documentSource,
}) => {
  // Only show sidebar if TOC or contributors should be displayed
  if (!showToc && !documentSource) return null;

  return (
    <div className="hidden lg:block w-64">
      <div className="sticky top-8 w-64">
        <TableOfContents articleRef={articleRef} show={showToc} />
        <GitHubContributors documentSource={documentSource} />
      </div>
    </div>
  );
};
