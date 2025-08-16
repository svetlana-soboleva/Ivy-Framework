import { TableOfContents } from '@/widgets/article/TableOfContents';
import { GitHubContributors } from '@/widgets/article/GitHubContributors';
import { DocumentTools } from '@/widgets/article/DocumentTools';
import React from 'react';

interface ArticleSidebarProps {
  articleRef: React.RefObject<HTMLElement | null>;
  showToc?: boolean;
  documentSource?: string;
  title?: string;
}

export const ArticleSidebar: React.FC<ArticleSidebarProps> = ({
  articleRef,
  showToc,
  documentSource,
  title,
}) => {
  // Only show sidebar if TOC, contributors, or tools should be displayed
  if (!showToc && !documentSource) return null;

  return (
    <div className="hidden lg:block w-64">
      <div className="sticky top-8 w-64 flex flex-col gap-4">
        <DocumentTools
          articleRef={articleRef}
          documentSource={documentSource}
          title={title}
        />
        <TableOfContents articleRef={articleRef} show={showToc} />
        <GitHubContributors documentSource={documentSource} />
      </div>
    </div>
  );
};
