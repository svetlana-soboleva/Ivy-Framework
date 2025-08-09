import React, {
  lazy,
  Suspense,
  memo,
  useMemo,
  useCallback,
  useState,
  useEffect,
  useRef,
} from 'react';
import ReactMarkdown, { defaultUrlTransform } from 'react-markdown';
import remarkGfm from 'remark-gfm';
import remarkGemoji from 'remark-gemoji';
import remarkMath from 'remark-math';
import rehypeKatex from 'rehype-katex';
import rehypeRaw from 'rehype-raw';
import 'katex/dist/katex.min.css';
import { cn, getIvyHost } from '@/lib/utils';
import CopyToClipboardButton from './CopyToClipboardButton';
import { createPrismTheme } from '@/lib/ivy-prism-theme';
import { textBlockClassMap, textContainerClass } from '@/lib/textBlockClassMap';
import { ScrollArea, ScrollBar } from '@/components/ui/scroll-area';

const SyntaxHighlighter = lazy(() =>
  import('react-syntax-highlighter').then(mod => ({ default: mod.Prism }))
);

// Mermaid component for rendering diagrams
const MermaidDiagram = memo(({ content }: { content: string }) => {
  const elementRef = useRef<HTMLDivElement>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let mounted = true;

    const renderDiagram = async () => {
      if (!elementRef.current) return;

      try {
        setIsLoading(true);
        setError(null);

        // Dynamically import mermaid
        const mermaid = (await import('mermaid')).default;

        // Initialize mermaid with theme-aware configuration
        mermaid.initialize({
          startOnLoad: false,
          theme: 'base',
          themeVariables: {
            // Use CSS custom properties for theme integration
            primaryColor: 'hsl(var(--primary))',
            primaryTextColor: 'hsl(var(--primary-foreground))',
            primaryBorderColor: 'hsl(var(--border))',
            lineColor: 'hsl(var(--border))',
            sectionBkgColor: 'hsl(var(--muted))',
            altSectionBkgColor: 'hsl(var(--background))',
            gridColor: 'hsl(var(--border))',
            secondaryColor: 'hsl(var(--secondary))',
            tertiaryColor: 'hsl(var(--muted))',
            background: 'hsl(var(--background))',
            mainBkg: 'hsl(var(--background))',
            secondBkg: 'hsl(var(--muted))',
            tertiaryBkg: 'hsl(var(--accent))',
          },
          fontFamily: 'inherit',
          securityLevel: 'strict', // Prevent script injection
        });

        // Generate unique ID for this diagram
        const id = `mermaid-${Math.random().toString(36).substr(2, 9)}`;

        // Clear the element first
        elementRef.current.innerHTML = '';

        // Render the diagram
        const { svg } = await mermaid.render(id, content.trim());

        if (mounted && elementRef.current) {
          elementRef.current.innerHTML = svg;
          setIsLoading(false);
        }
      } catch (err) {
        console.error('Mermaid rendering error:', err);
        if (mounted) {
          setError(
            err instanceof Error ? err.message : 'Failed to render diagram'
          );
          setIsLoading(false);
        }
      }
    };

    renderDiagram();

    return () => {
      mounted = false;
    };
  }, [content]);

  if (error) {
    return (
      <div className="rounded-md border border-destructive bg-destructive/10 p-4">
        <div className="flex items-center gap-2 text-destructive text-sm font-medium mb-2">
          <svg
            className="h-4 w-4"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth={1.5}
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126ZM12 15.75h.007v.008H12v-.008Z"
            />
          </svg>
          Mermaid Error
        </div>
        <div className="text-sm text-muted-foreground">{error}</div>
        <details className="mt-2">
          <summary className="cursor-pointer text-xs text-muted-foreground hover:text-foreground">
            Show diagram source
          </summary>
          <pre className="mt-2 p-2 bg-muted rounded text-xs overflow-x-auto">
            {content}
          </pre>
        </details>
      </div>
    );
  }

  return (
    <div className="relative">
      <div className="absolute top-2 right-2 z-10">
        <CopyToClipboardButton textToCopy={content} />
      </div>
      <div className="mermaid-container rounded-md border bg-background p-4 overflow-x-auto">
        {isLoading && (
          <div className="flex items-center justify-center p-8 text-muted-foreground">
            <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-primary"></div>
            <span className="ml-2 text-sm">Loading diagram...</span>
          </div>
        )}
        <div
          ref={elementRef}
          className="mermaid-diagram"
          style={{ minHeight: isLoading ? '100px' : 'auto' }}
        />
      </div>
    </div>
  );
});

interface MarkdownRendererProps {
  content: string;
  onLinkClick?: (url: string) => void;
}

const ImageOverlay = ({
  src,
  alt,
  onClose,
}: {
  src: string | undefined;
  alt: string | undefined;
  onClose: () => void;
}) => {
  // Handle click on the overlay background to close it
  const handleBackdropClick = (e: React.MouseEvent) => {
    if (e.target === e.currentTarget) {
      onClose();
    }
  };

  return (
    <div
      className="fixed inset-0 bg-black/70 flex items-center justify-center z-50 cursor-zoom-out"
      onClick={handleBackdropClick}
    >
      <div className="relative max-w-[90vw] max-h-[90vh]">
        <img
          src={src}
          alt={alt}
          className="max-w-full max-h-[90vh] object-contain"
        />
        <button
          className="absolute top-4 right-4 bg-black/50 text-white rounded-full w-8 h-8 flex items-center justify-center"
          onClick={onClose}
        >
          ✕
        </button>
      </div>
    </div>
  );
};

const hasContentFeature = (content: string, feature: RegExp): boolean => {
  return feature.test(content);
};

const CodeBlock = memo(
  ({
    className,
    children,
    hasCodeBlocks,
    hasMermaid,
  }: {
    className?: string;
    children: React.ReactNode;
    inline?: boolean;
    hasCodeBlocks: boolean;
    hasMermaid: boolean;
  }) => {
    const match = /language-(\w+)/.exec(className || '');
    const content = String(children).replace(/\n$/, '');
    const isTerminal = match && match[1] === 'terminal';
    const isMermaid = match && match[1] === 'mermaid';

    // Create dynamic theme that adapts to current CSS variables
    const dynamicTheme = useMemo(() => createPrismTheme(), []);

    if (match && hasCodeBlocks) {
      // Handle Mermaid diagrams
      if (isMermaid && hasMermaid) {
        return (
          <Suspense
            fallback={
              <div className="rounded-md border bg-background p-4">
                <div className="flex items-center justify-center p-8 text-muted-foreground">
                  <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-primary"></div>
                  <span className="ml-2 text-sm">Loading Mermaid...</span>
                </div>
              </div>
            }
          >
            <MermaidDiagram content={content} />
          </Suspense>
        );
      }

      if (isTerminal) {
        // Handle terminal blocks with prompt styling
        const lines = content.split('\n').filter(line => line.trim());
        const cleanContent = lines.join('\n'); // Remove any empty lines

        return (
          <div className="relative">
            <div className="absolute top-2 right-2 z-10">
              <CopyToClipboardButton textToCopy={cleanContent} />
            </div>
            <ScrollArea className="w-full ivy-demo-surface">
              <pre className="p-4 bg-muted rounded-md font-mono text-body">
                {lines.map((line, index) => (
                  <div key={index} className="flex">
                    <span className="text-muted-foreground select-none pointer-events-none mr-2">
                      {'> '}
                    </span>
                    <span className="flex-1">{line}</span>
                  </div>
                ))}
              </pre>
              <ScrollBar orientation="horizontal" />
            </ScrollArea>
          </div>
        );
      }

      return (
        <Suspense
          fallback={
            <ScrollArea className="w-full border border-border rounded-md">
              <pre className="p-4 bg-muted rounded-md">{content}</pre>
              <ScrollBar orientation="horizontal" />
            </ScrollArea>
          }
        >
          <div className="relative">
            <div className="absolute top-2 right-2 z-10">
              <CopyToClipboardButton textToCopy={content} />
            </div>
            <ScrollArea className="w-full ivy-demo-surface">
              <SyntaxHighlighter
                language={match[1]}
                style={dynamicTheme}
                customStyle={{ margin: 0 }}
              >
                {content}
              </SyntaxHighlighter>
              <ScrollBar orientation="horizontal" />
            </ScrollArea>
          </div>
        </Suspense>
      );
    }

    return (
      <code className={cn(textBlockClassMap.code, className)}>{children}</code>
    );
  }
);

const MarkdownRenderer: React.FC<MarkdownRendererProps> = ({
  content,
  onLinkClick,
}) => {
  const contentFeatures = useMemo(
    () => ({
      hasMath: hasContentFeature(content, /(\$|\\\(|\\\[|\\begin\{)/),
      hasCodeBlocks: hasContentFeature(content, /```/),
      hasMermaid: hasContentFeature(content, /```mermaid/),
    }),
    [content]
  );

  const plugins = useMemo(() => {
    const remarkPlugins = [remarkGfm, remarkGemoji];
    if (contentFeatures.hasMath)
      remarkPlugins.push(remarkMath as typeof remarkGfm);

    const rehypePlugins = [rehypeRaw];
    if (contentFeatures.hasMath)
      rehypePlugins.push(rehypeKatex as unknown as typeof rehypeRaw);

    return { remarkPlugins, rehypePlugins };
  }, [contentFeatures.hasMath]);

  const handleLinkClick = useCallback(
    (href: string, event: React.MouseEvent<HTMLAnchorElement>) => {
      const isExternalLink = href?.match(/^(https?:\/\/|mailto:|tel:)/i);
      if (!isExternalLink && onLinkClick && href) {
        event.preventDefault();
        onLinkClick(href);
      }
    },
    [onLinkClick]
  );

  const components = useMemo(
    () => ({
      h1: memo(({ children }: { children: React.ReactNode }) => (
        <h1 className={textBlockClassMap.h1}>{children}</h1>
      )),
      h2: memo(({ children }: { children: React.ReactNode }) => (
        <h2 className={textBlockClassMap.h2}>{children}</h2>
      )),
      h3: memo(({ children }: { children: React.ReactNode }) => (
        <h3 className={textBlockClassMap.h3}>{children}</h3>
      )),
      h4: memo(({ children }: { children: React.ReactNode }) => (
        <h4 className={textBlockClassMap.h4}>{children}</h4>
      )),
      p: memo(({ children }: { children: React.ReactNode }) => (
        <p className={textBlockClassMap.p}>{children}</p>
      )),
      ul: memo(({ children }: { children: React.ReactNode }) => (
        <ul className={textBlockClassMap.ul}>{children}</ul>
      )),
      ol: memo(({ children }: { children: React.ReactNode }) => (
        <ol className={textBlockClassMap.ol}>{children}</ol>
      )),
      li: memo(({ children }: { children: React.ReactNode }) => (
        <li className={textBlockClassMap.li}>{children}</li>
      )),
      strong: memo(({ children }: { children: React.ReactNode }) => (
        <strong className={textBlockClassMap.strong}>{children}</strong>
      )),
      em: memo(({ children }: { children: React.ReactNode }) => (
        <em className={textBlockClassMap.em}>{children}</em>
      )),
      code: memo((props: React.ComponentProps<'code'>) => (
        <CodeBlock
          className={props.className}
          children={props.children || ''}
          hasCodeBlocks={contentFeatures.hasCodeBlocks}
          hasMermaid={contentFeatures.hasMermaid}
        />
      )),

      // Pre tag (for code blocks)
      pre: memo(({ children }: { children: React.ReactNode }) => (
        <>{children}</>
      )),

      // Links with memoized click handler
      a: memo(
        ({
          children,
          href,
          ...props
        }: React.AnchorHTMLAttributes<HTMLAnchorElement>) => {
          const isExternalLink = href?.match(/^(https?:\/\/|mailto:|tel:)/i);

          return (
            <a
              {...props}
              className="text-primary underline brightness-90 hover:brightness-100"
              href={href || '#'}
              target={isExternalLink ? '_blank' : undefined}
              rel={isExternalLink ? 'noopener noreferrer' : undefined}
              onClick={e => href && handleLinkClick(href, e)}
            >
              {children}
            </a>
          );
        }
      ),

      // Blockquotes
      blockquote: memo(
        ({ children }: React.BlockquoteHTMLAttributes<HTMLQuoteElement>) => (
          <blockquote className={textBlockClassMap.blockquote}>
            {children}
          </blockquote>
        )
      ),

      // Tables
      table: memo(({ children }: { children: React.ReactNode }) => (
        <table className={textBlockClassMap.table}>{children}</table>
      )),
      thead: memo(({ children }: { children: React.ReactNode }) => (
        <thead className="bg-muted">{children}</thead>
      )),
      tr: memo(({ children }: { children: React.ReactNode }) => (
        <tr className="border border-border">{children}</tr>
      )),
      th: memo(({ children }: { children: React.ReactNode }) => (
        <th className={textBlockClassMap.th}>{children}</th>
      )),
      td: memo(({ children }: { children: React.ReactNode }) => (
        <td className={textBlockClassMap.td}>{children}</td>
      )),

      img: memo(
        (props: React.ImgHTMLAttributes<HTMLImageElement>) => {
          const [showOverlay, setShowOverlay] = useState(false);
          const src = props.src;
          const imageSrc =
            src && !src?.match(/^(https?:\/\/|data:|blob:|app:)/i)
              ? `${getIvyHost()}${src?.startsWith('/') ? '' : '/'}${src}`
              : src;

          return (
            <>
              <img
                {...props}
                src={imageSrc}
                className={cn(textBlockClassMap.img, 'cursor-zoom-in')}
                loading="lazy"
                onClick={() => setShowOverlay(true)}
              />
              {showOverlay && (
                <ImageOverlay
                  src={imageSrc}
                  alt={props.alt}
                  onClose={() => setShowOverlay(false)}
                />
              )}
            </>
          );
        },
        (prevProps, nextProps) =>
          prevProps.src === nextProps.src && prevProps.alt === nextProps.alt
      ),
    }),
    [contentFeatures.hasCodeBlocks, contentFeatures.hasMermaid, handleLinkClick]
  );

  const urlTransform = useCallback((url: string) => {
    if (url.startsWith('app://')) {
      return url;
    }
    return defaultUrlTransform(url);
  }, []);

  return (
    <div className={textContainerClass}>
      <ReactMarkdown
        components={
          components as React.ComponentProps<typeof ReactMarkdown>['components']
        }
        remarkPlugins={plugins.remarkPlugins}
        rehypePlugins={plugins.rehypePlugins}
        urlTransform={urlTransform}
      >
        {content}
      </ReactMarkdown>
    </div>
  );
};

export default memo(MarkdownRenderer);
