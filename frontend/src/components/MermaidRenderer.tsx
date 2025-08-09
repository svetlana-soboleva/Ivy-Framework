import React, { memo, useRef, useState, useEffect } from 'react';
import CopyToClipboardButton from './CopyToClipboardButton';

interface MermaidRendererProps {
  content: string;
}

const MermaidRenderer = memo(({ content }: MermaidRendererProps) => {
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

        // Get computed CSS variables to use as actual color values
        const computedStyle = getComputedStyle(document.documentElement);
        const getCSSVariable = (variable: string) => {
          const value = computedStyle.getPropertyValue(variable).trim();
          // If it's already an HSL value, return it, otherwise wrap in hsl()
          return value.includes('hsl') ? value : `hsl(${value})`;
        };

        // Initialize mermaid with theme-aware configuration
        mermaid.initialize({
          startOnLoad: false,
          theme: 'base',
          themeVariables: {
            // Convert CSS custom properties to actual color values
            primaryColor: getCSSVariable('--primary'),
            primaryTextColor: getCSSVariable('--primary-foreground'),
            primaryBorderColor: getCSSVariable('--border'),
            lineColor: getCSSVariable('--border'),
            sectionBkgColor: getCSSVariable('--muted'),
            altSectionBkgColor: getCSSVariable('--background'),
            gridColor: getCSSVariable('--border'),
            secondaryColor: getCSSVariable('--secondary'),
            tertiaryColor: getCSSVariable('--muted'),
            background: getCSSVariable('--background'),
            mainBkg: getCSSVariable('--background'),
            secondBkg: getCSSVariable('--muted'),
            tertiaryBkg: getCSSVariable('--accent'),
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

MermaidRenderer.displayName = 'MermaidRenderer';

export default MermaidRenderer;
