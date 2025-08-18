import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { TooltipProvider } from '@/components/ui/tooltip';
import { useToast } from '@/hooks/use-toast';
import { Copy, Download, ChevronDown, ExternalLink } from 'lucide-react';
import React from 'react';

interface DocumentToolsProps {
  articleRef: React.RefObject<HTMLElement | null>;
  documentSource?: string;
  title?: string;
}

export const DocumentTools: React.FC<DocumentToolsProps> = ({
  articleRef,
  documentSource,
  title = 'document',
}) => {
  const { toast } = useToast();

  const copyTextContent = async () => {
    try {
      // Show loading state
      toast({
        title: 'Loading Content...',
        description: 'Extracting API sections from page...',
      });

      // Extract API sections from the rendered page
      const apiContent = extractApiSections();

      if (!apiContent.trim()) {
        toast({
          title: 'Copy Failed',
          description: 'No API content found to copy',
          variant: 'destructive',
        });
        return;
      }

      await navigator.clipboard.writeText(apiContent);
      toast({
        title: 'Copied!',
        description: 'API sections copied to clipboard',
      });
    } catch (error) {
      try {
        // Fallback: try to copy text content from the page
        if (articleRef.current) {
          const textContent = articleRef.current.textContent || '';
          if (textContent.trim()) {
            await navigator.clipboard.writeText(textContent);
            toast({
              title: 'Copied!',
              description: 'Document text copied to clipboard (fallback)',
            });
            return;
          }
        }

        toast({
          title: 'Copy Failed',
          description:
            error instanceof Error ? error.message : 'Failed to copy content',
          variant: 'destructive',
        });
      } catch {
        toast({
          title: 'Copy Failed',
          description: 'Failed to copy text to clipboard',
          variant: 'destructive',
        });
      }
    }
  };

  // Extract API sections from the rendered page
  const extractApiSections = (): string => {
    if (!articleRef.current) return '';

    const articleElement = articleRef.current;
    let apiContent = '';

    // Look for API-related sections
    const apiSelectors = [
      'h1, h2, h3, h4, h5, h6', // Headings
      'p', // Paragraphs
      'ul, ol', // Lists
      'table', // Tables
      'pre, code', // Code blocks
      'div[class*="api"]', // Divs with "api" in class
      'section[class*="api"]', // Sections with "api" in class
      '[data-testid*="api"]', // Elements with "api" in test ID
    ];

    // Find all elements that might contain API content
    const apiElements = articleElement.querySelectorAll(
      apiSelectors.join(', ')
    );

    // Process elements in document order
    const processedElements = new Set<Element>();

    apiElements.forEach(element => {
      if (processedElements.has(element)) return;

      const tagName = element.tagName.toLowerCase();
      const text = element.textContent?.trim() || '';

      // Skip empty elements
      if (!text && !['table', 'ul', 'ol'].includes(tagName)) return;

      // Check if this element is part of an API section
      if (isApiSection(element)) {
        switch (tagName) {
          case 'h1':
          case 'h2':
          case 'h3':
          case 'h4':
          case 'h5':
          case 'h6': {
            const level = parseInt(tagName[1]);
            apiContent += `${'#'.repeat(level)} ${text}\n\n`;
            processedElements.add(element);
            break;
          }

          case 'p':
            if (text && !element.closest('li, td, th')) {
              apiContent += `${text}\n\n`;
              processedElements.add(element);
            }
            break;

          case 'ul':
          case 'ol':
            if (!processedElements.has(element)) {
              apiContent += extractListToMarkdown(element);
              processedElements.add(element);
            }
            break;

          case 'table':
            if (!processedElements.has(element)) {
              apiContent += extractTableToMarkdown(element as HTMLTableElement);
              processedElements.add(element);
            }
            break;

          case 'pre':
            if (!processedElements.has(element)) {
              const codeElement = element.querySelector('code');
              const codeText = codeElement
                ? codeElement.textContent
                : element.textContent;
              apiContent += `\`\`\`\n${codeText || ''}\n\`\`\`\n\n`;
              processedElements.add(element);
            }
            break;

          case 'code':
            // Only process inline code (not inside pre)
            if (!element.closest('pre') && !processedElements.has(element)) {
              // Skip standalone code elements to avoid duplication
            }
            break;
        }
      }
    });

    return apiContent.trim();
  };

  // Check if an element is part of an API section
  const isApiSection = (element: Element): boolean => {
    // Check if the element or its ancestors contain API-related content
    const apiKeywords = [
      'constructor',
      'supported types',
      'properties',
      'events',
      'api',
      'interface',
      'class',
      'method',
      'parameter',
      'return',
      'example',
      'usage',
    ];

    // Check the element itself
    const elementText = element.textContent?.toLowerCase() || '';
    if (apiKeywords.some(keyword => elementText.includes(keyword))) {
      return true;
    }

    // Check parent elements
    let parent = element.parentElement;
    while (parent) {
      const parentText = parent.textContent?.toLowerCase() || '';
      if (apiKeywords.some(keyword => parentText.includes(keyword))) {
        return true;
      }
      parent = parent.parentElement;
    }

    // Check for specific API section indicators
    const apiIndicators = [
      '[class*="api"]',
      '[class*="constructor"]',
      '[class*="properties"]',
      '[class*="events"]',
      '[data-testid*="api"]',
      'h1, h2, h3, h4, h5, h6', // Headings often indicate section boundaries
    ];

    return apiIndicators.some(selector => {
      try {
        return element.matches(selector) || element.closest(selector);
      } catch {
        return false;
      }
    });
  };

  // Helper function to extract table as markdown with better cleaning
  const extractTableToMarkdown = (table: HTMLTableElement): string => {
    let tableMarkdown = '\n';
    const rows = Array.from(table.querySelectorAll('tr'));

    if (rows.length === 0) return '';

    rows.forEach((row, rowIndex) => {
      const cells = Array.from(row.querySelectorAll('td, th'));
      const cellTexts = cells.map(cell => {
        // Clean up the cell content by removing CSS styles and data attributes
        let cellText = cell.textContent || '';

        // Remove CSS style blocks that might be embedded
        cellText = cellText.replace(/\{[^}]*\}/g, '');

        // Remove data attributes that might be in the text
        cellText = cellText.replace(/\[data-[^\]]*\]/g, '');

        // Remove any remaining CSS selectors
        cellText = cellText.replace(/[a-zA-Z-]+\s*\{[^}]*\}/g, '');

        // Remove webkit scrollbar and other CSS pseudo-elements
        cellText = cellText.replace(/::-webkit-scrollbar[^;]*;?/g, '');
        cellText = cellText.replace(/::-moz-scrollbar[^;]*;?/g, '');
        cellText = cellText.replace(/::-ms-scrollbar[^;]*;?/g, '');
        cellText = cellText.replace(/::scrollbar[^;]*;?/g, '');

        // Remove any remaining CSS rules
        cellText = cellText.replace(/[a-zA-Z-]+\s*:\s*[^;]*;?/g, '');

        // Clean up extra whitespace and normalize
        cellText = cellText.trim().replace(/\s+/g, ' ');

        // Escape pipe characters for markdown tables
        return cellText.replace(/\|/g, '\\|');
      });

      if (cellTexts.some(text => text.length > 0)) {
        tableMarkdown += `| ${cellTexts.join(' | ')} |\n`;

        // Add separator row after header
        if (rowIndex === 0 && row.querySelector('th')) {
          tableMarkdown += `| ${cellTexts.map(() => '---').join(' | ')} |\n`;
        }
      }
    });

    return tableMarkdown + '\n';
  };

  // Helper function to extract lists as markdown
  const extractListToMarkdown = (list: Element): string => {
    const items = Array.from(list.querySelectorAll(':scope > li')); // Direct children only
    const isOrdered = list.tagName.toLowerCase() === 'ol';
    let listMarkdown = '\n';

    items.forEach((item, index) => {
      const text = item.textContent?.trim() || '';
      if (text) {
        const prefix = isOrdered ? `${index + 1}. ` : '- ';
        listMarkdown += `${prefix}${text}\n`;
      }
    });

    return listMarkdown + '\n';
  };

  const generateFileName = (): string => {
    if (title && title !== 'document') {
      return title
        .toLowerCase()
        .replace(/\s+/g, '-')
        .replace(/[^a-z0-9-]/g, '');
    }

    if (articleRef.current) {
      const firstHeading = articleRef.current.querySelector('h1');
      if (firstHeading?.textContent) {
        return firstHeading.textContent
          .toLowerCase()
          .replace(/\s+/g, '-')
          .replace(/[^a-z0-9-]/g, '')
          .substring(0, 50);
      }
    }

    if (documentSource) {
      try {
        const url = new URL(documentSource);
        const pathParts = url.pathname.split('/');
        const fileName = pathParts[pathParts.length - 1];

        if (fileName && fileName.includes('.')) {
          return fileName
            .split('.')[0]
            .toLowerCase()
            .replace(/[^a-z0-9-]/g, '-');
        }
      } catch {
        // Ignore URL parsing errors
      }
    }

    const timestamp = new Date().toISOString().split('T')[0];
    return `ivy-document-${timestamp}`;
  };

  const saveAsMarkdown = async () => {
    try {
      // Show loading state
      toast({
        title: 'Preparing Download...',
        description: 'Extracting API sections...',
      });

      // Extract the same content that gets copied
      const apiContent = extractApiSections();

      if (!apiContent.trim()) {
        toast({
          title: 'Download Failed',
          description: 'No API content found to download',
          variant: 'destructive',
        });
        return;
      }

      // Create and download the file
      const blob = new Blob([apiContent], { type: 'text/markdown' });
      const url = URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = generateFileName();
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      URL.revokeObjectURL(url);

      toast({
        title: 'Downloaded!',
        description: 'API sections saved as markdown file',
      });
    } catch (error) {
      console.error('Error downloading markdown:', error);
      toast({
        title: 'Download Failed',
        description:
          error instanceof Error
            ? error.message
            : 'Failed to download markdown',
        variant: 'destructive',
      });
    }
  };

  return (
    <TooltipProvider>
      <div className="flex">
        <Button
          variant="ghost"
          size="sm"
          onClick={copyTextContent}
          className="h-8 px-2 flex items-center gap-1 rounded-r-none border-r border-border/50"
        >
          <Copy className="w-4 h-4" />
          <span className="text-xs">Copy Page</span>
        </Button>

        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button
              variant="ghost"
              size="sm"
              className="h-8 px-1 flex items-center gap-1 rounded-l-none"
            >
              <ChevronDown className="w-4 h-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-48">
            <DropdownMenuItem onClick={saveAsMarkdown}>
              <Download className="w-4 h-4 mr-2" />
              Download as Markdown
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem disabled>
              <ExternalLink className="w-4 h-4 mr-2" />
              Open in ChatGPT
            </DropdownMenuItem>
            <DropdownMenuItem disabled>
              <ExternalLink className="w-4 h-4 mr-2" />
              Open in Claude
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </TooltipProvider>
  );
};
