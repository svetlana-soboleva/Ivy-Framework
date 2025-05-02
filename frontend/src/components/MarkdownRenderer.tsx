import React, { lazy, Suspense, memo } from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import remarkGemoji from 'remark-gemoji';
import remarkMath from 'remark-math';
import rehypeKatex from 'rehype-katex';
import rehypeRaw from 'rehype-raw';
import 'katex/dist/katex.min.css';
import { cn } from '@/lib/utils';
import CopyToClipboardButton from './CopyToClipboardButton';
import ivyPrismTheme from '@/lib/ivy-prism-theme';
import { ChevronRight } from 'lucide-react';

// Lazy load only the syntax highlighter component
const SyntaxHighlighter = lazy(() => import('react-syntax-highlighter').then(mod => ({ default: mod.Prism })));

interface MarkdownRendererProps {
  content: string;
  onLinkClick?: (url: string) => void;
}

const MemoizedImage = memo(
  ({ src, alt, ...props }: React.ImgHTMLAttributes<HTMLImageElement>) => (
    <img
      src={src}
      alt={alt}
      className="max-w-full h-auto"
      loading="lazy"
      {...props}
    />
  ),
  (prevProps, nextProps) => prevProps.src === nextProps.src && prevProps.alt === nextProps.alt
);

const MarkdownRenderer: React.FC<MarkdownRendererProps> = ({ content, onLinkClick }) => {

  // Check if the content contains math blocks
  const hasMath = content.includes('$') || content.includes('\\begin{');
  // Check if the content contains code blocks
  const hasCodeBlocks = content.includes('```');

  // Dynamically select plugins based on content
  const remarkPlugins: any[] = [remarkGfm, remarkGemoji];
  if (hasMath) remarkPlugins.push(remarkMath);
  
  // Always include rehypeRaw to properly handle HTML elements
  const rehypePlugins: any[] = [rehypeRaw];
  if (hasMath) rehypePlugins.push(rehypeKatex);

  const components: any = {
    // Headers
    h1: ({ children }: { children: React.ReactNode }) => (
      <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl">
        {children}
      </h1>
    ),
    h2: ({ children }: { children: React.ReactNode }) => (
      <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight first:mt-0">
        {children}
      </h2>
    ),
    h3: ({ children }: { children: React.ReactNode }) => (
      <h3 className="scroll-m-20 text-2xl font-semibold tracking-tight">
        {children}
      </h3>
    ),
    h4: ({ children }: { children: React.ReactNode }) => (
      <h4 className="scroll-m-20 text-xl font-semibold tracking-tight">
        {children}
      </h4>
    ),

    // Paragraphs and text
    p: ({ children }: { children: React.ReactNode }) => (
      <p className="scroll-m-20 text-md leading-8">{children}</p>
    ),
    strong: ({ children }: { children: React.ReactNode }) => (
      <strong className="font-semibold">{children}</strong>
    ),
    em: ({ children }: { children: React.ReactNode }) => (
      <em className="italic">{children}</em>
    ),
    
    // Lists
    ul: ({ children }: { children: React.ReactNode }) => (
      <ul className="ml-6 list-disc [&>li:first-child]:mt-0">{children}</ul>
    ),
    ol: ({ children }: { children: React.ReactNode }) => (
      <ol className="ml-6 list-decimal [&>li:first-child]:mt-0">{children}</ol>
    ),
    li: ({ children }: { children: React.ReactNode }) => (
      <li className="mt-3">{children}</li>
    ),

    // Code blocks - only render syntax highlighter if needed
    code: ({
      className,
      children,
      ...props
    }: {
      className?: string;
      inline?: boolean;
      children: React.ReactNode;
    }) => {
      const match = /language-(\w+)/.exec(className || '');
      const content = String(children).replace(/\n$/, '');
      
      if (match && hasCodeBlocks) {
        return (
          <Suspense fallback={<pre>{content}</pre>}>
            <div className="relative">
              <div className="absolute top-2 right-2 z-10">
                <CopyToClipboardButton textToCopy={content} />
              </div>
              <SyntaxHighlighter 
                language={match[1]}
                style={ivyPrismTheme}>
                customStyle={{margin:0}}
                {content}
              </SyntaxHighlighter>
            </div>
          </Suspense>
        );
      }
      
      return (
        <code className={cn("relative rounded bg-muted px-[0.3rem] py-[0.3rem] font-mono text-sm", className)} {...props}>
          {children}
        </code>
      );
    },

    // Pre tag (for code blocks)
    pre: ({ children }: { children: React.ReactNode }) => (
      <>{children}</>
    ),

    // Links
    a: ({
      children,
      href,
      ...props
    }: React.AnchorHTMLAttributes<HTMLAnchorElement>) => {
      const isExternalLink = href?.match(/^(https?:\/\/|mailto:|tel:)/i);
      
      return (
        <a
          {...props}
          href={href}
          className="text-primary underline brightness-90 hover:brightness-100"
          target={isExternalLink ? "_blank" : undefined}
          rel={isExternalLink ? "noopener noreferrer" : undefined}
          onClick={(e) => {
            if (!isExternalLink && onLinkClick && href) {
              e.preventDefault();
              onLinkClick(href);
            }
          }}
        >
          {children}
        </a>
      );
    },

    // Blockquotes
    blockquote: ({
      children,
    }: React.BlockquoteHTMLAttributes<HTMLQuoteElement>) => (
      <blockquote className="border-l-2 pl-6 italic">{children}</blockquote>
    ),

    // Tables
    table: ({ children }: { children: React.ReactNode }) => (
      <table className="w-full border-collapse border border-border">
        {children}
      </table>
    ),
    thead: ({ children }: { children: React.ReactNode }) => (
      <thead className="bg-muted">{children}</thead>
    ),
    tr: ({ children }: { children: React.ReactNode }) => (
      <tr className="border border-border">{children}</tr>
    ),
    th: ({ children }: { children: React.ReactNode }) => (
      <th className="border border-border px-4 py-2 text-left font-bold">
        {children}
      </th>
    ),
    td: ({ children }: { children: React.ReactNode }) => (
      <td className="border border-border px-4 py-2">{children}</td>
    ),

    // Details and Summary
    details: ({ children, ...props }: React.HTMLAttributes<HTMLElement>) => (
      <details
        {...props}
        className="group border rounded-lg border-border p-4"
        >
      {children}
    </details>
    ),
    summary: ({ children, ...props }: React.HTMLAttributes<HTMLElement>) => (
      <summary 
        className="flex items-center gap-2 cursor-pointer font-medium text-lg hover:text-primary [&::-webkit-details-marker]:hidden" 
        {...props}
        >
        <ChevronRight className="h-4 w-4 transition-transform duration-200 group-open:rotate-90" />
        {children}
      </summary>
    ),

    // Images
    img: (props: React.ImgHTMLAttributes<HTMLImageElement>) => (
      <MemoizedImage {...props} />
    ),
  };

  return (
    <div className="flex flex-col gap-8">
      <ReactMarkdown
        key={content}
        components={components}
        remarkPlugins={remarkPlugins}
        rehypePlugins={rehypePlugins}
      >
        {content}
      </ReactMarkdown>
    </div>
  );
};

export default MarkdownRenderer;