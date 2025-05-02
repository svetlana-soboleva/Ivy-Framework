import { ScrollArea } from '@/components/ui/scroll-area';
import { cn } from '@/lib/utils';
import { InternalLink } from '@/types/widgets';
import { Github } from 'lucide-react';
import React, { useEffect, useState } from 'react';

type TocItem = {
  id: string;
  text: string;
  level: number;
};

const TableOfContents = ({ className }: { className?: string }) => {
  const [headings, setHeadings] = useState<TocItem[]>([]);
  const [activeId, setActiveId] = useState<string>("");

  useEffect(() => {
    // Get all headings from the article
    const articleElement = document.querySelector('article');
    const elements = articleElement ? Array.from(articleElement.querySelectorAll('h1, h2, h3, h4, h5, h6')) : [];
    
    console.log(elements);

    // Process headings and add IDs if they don't exist
    const items = elements.map((element) => {
      // Generate ID if doesn't exist
      if (!element.id) {
        element.id = element.textContent?.toLowerCase()
          .replace(/\s+/g, '-')
          .replace(/[^\w-]/g, '') ?? '';
      }
      
      return {
        id: element.id,
        text: element.textContent ?? '',
        level: parseInt(element.tagName[1])
      };
    });

    setHeadings(items);

    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            setActiveId(entry.target.id);
          }
        });
      },
      { rootMargin: '0px 0px -80% 0px' }
    );

    elements.forEach((element) => observer.observe(element));

    return () => observer.disconnect();
  }, []);

  return (
    <div className={cn("w-64 relative", className)}>
      <div className="font-medium mb-4">Table of Contents</div>
      <ScrollArea className="h-[calc(100vh-4rem)]">
        <nav className="relative">
          {headings.map((heading) => (
            <a
              key={heading.id}
              href={`#${heading.id}`}
              className={cn(
                "block text-sm py-1 hover:text-primary transition-colors",
                heading.level === 1 ? "pl-0" : `pl-${(heading.level - 1) * 4}`,
                activeId === heading.id
                  ? "text-primary font-medium"
                  : "text-muted-foreground"
              )}
              onClick={(e) => {
                e.preventDefault();
                document.getElementById(heading.id)?.scrollIntoView({
                  behavior: "smooth"
                });
              }}
            >
              {heading.text}
            </a>
          ))}
        </nav>
      </ScrollArea>
    </div>
  );
};

interface ArticleWidgetProps {
  children: React.ReactNode[];
  showToc?: boolean;
  showFooter?: boolean;
  previous: InternalLink;
  next: InternalLink;
  documentSource?: string;
}

export const ArticleWidget: React.FC<ArticleWidgetProps> = ({ children, previous, next, documentSource, showFooter, showToc }) => {
  return (
    <div className="flex flex-col gap-8 max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative">
      <div className="flex gap-8">
        <article className="w-[48rem] gap-8 flex flex-col">
          {children}
          {showFooter && (
            <footer className="border-t py-8">
              <div className="flex flex-col gap-6">
                <div className="flex justify-between items-center">
                  <div className="flex-1">
                    {previous && (
                      <a
                        href={previous.appId}
                        className="group flex flex-col gap-2 hover:text-primary transition-colors"
                      >
                        <div className="text-sm">← Previous</div>
                        <div className="font-medium text-muted-foreground">{previous.title}</div>
                      </a>
                    )}
                  </div>
                  <div className="flex-1 flex justify-end">
                    {next && (
                      <a
                        href={next.appId}
                        className="group flex flex-col text-right gap-2 hover:text-primary transition-colors"
                      >
                        <div className="text-sm">Next →</div>
                        <div className="font-medium text-muted-foreground">{next.title}</div>
                      </a>
                    )}
                  </div>
                </div>
                {documentSource && (
                  <div className="flex justify-center">
                    <a
                      href={documentSource}
                      target="_blank"
                      rel="noopener noreferrer"
                      className="group flex items-center gap-2 text-sm text-muted-foreground hover:text-primary transition-colors"
                    >
                      <Github className="w-4 h-4" />
                      Edit this document
                    </a>
                  </div>
                )}
              </div>
            </footer>
          )}
        </article>
        {showToc && (
          <div className="hidden lg:block">
            <TableOfContents className="sticky top-8 h-[calc(100vh-2rem)]" />
          </div>
        )}
      </div>
    </div>
  );
};
