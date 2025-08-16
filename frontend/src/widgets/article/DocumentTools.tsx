import { Button } from '@/components/ui/button';
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { useToast } from '@/hooks/use-toast';
import { Copy, Download } from 'lucide-react';
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
  const extractCleanText = (): string => {
    if (!articleRef.current) return '';

    const articleElement = articleRef.current;
    const clone = articleElement.cloneNode(true) as HTMLElement;

    // Remove navigation elements, buttons, and other UI components
    const elementsToRemove = [
      'nav',
      'button',
      '.toc',
      '.table-of-contents',
      '.navigation',
      '.breadcrumb',
      '.pagination',
    ];

    elementsToRemove.forEach(selector => {
      const elements = clone.querySelectorAll(selector);
      elements.forEach(el => el.remove());
    });

    // Get clean text content
    let textContent = clone.textContent || '';

    // Clean up extra whitespace and normalize line breaks
    textContent = textContent
      .replace(/\n\s*\n\s*\n/g, '\n\n')
      .replace(/^\s+|\s+$/g, '')
      .replace(/[ \t]+/g, ' ');

    return textContent;
  };

  const extractMarkdownContent = (): string => {
    if (!articleRef.current) return '';

    const articleElement = articleRef.current;
    let markdownContent = '';

    // Extract headings and content in markdown format
    const elements = articleElement.querySelectorAll(
      'h1, h2, h3, h4, h5, h6, p, ul, ol, blockquote, pre, code'
    );

    elements.forEach(element => {
      const tagName = element.tagName.toLowerCase();
      const text = element.textContent || '';

      switch (tagName) {
        case 'h1':
          markdownContent += `# ${text}\n\n`;
          break;
        case 'h2':
          markdownContent += `## ${text}\n\n`;
          break;
        case 'h3':
          markdownContent += `### ${text}\n\n`;
          break;
        case 'h4':
          markdownContent += `#### ${text}\n\n`;
          break;
        case 'h5':
          markdownContent += `##### ${text}\n\n`;
          break;
        case 'h6':
          markdownContent += `###### ${text}\n\n`;
          break;
        case 'p':
          if (text.trim()) {
            markdownContent += `${text}\n\n`;
          }
          break;
        case 'ul': {
          const liItems = element.querySelectorAll('li');
          liItems.forEach(li => {
            markdownContent += `- ${li.textContent || ''}\n`;
          });
          markdownContent += '\n';
          break;
        }
        case 'ol': {
          const olItems = element.querySelectorAll('li');
          olItems.forEach((li, index) => {
            markdownContent += `${index + 1}. ${li.textContent || ''}\n`;
          });
          markdownContent += '\n';
          break;
        }
        case 'blockquote':
          markdownContent += `> ${text}\n\n`;
          break;
        case 'pre':
          markdownContent += `\`\`\`\n${text}\n\`\`\`\n\n`;
          break;
        case 'code':
          if (!element.closest('pre')) {
            markdownContent = markdownContent.replace(/(.*)$/, `$1\`${text}\``);
          }
          break;
      }
    });

    // Add source attribution if available
    if (documentSource) {
      markdownContent += `\n---\n*Source: [${documentSource}](${documentSource})*\n`;
    }

    return markdownContent.trim();
  };

  const copyTextContent = async () => {
    try {
      const textContent = extractCleanText();

      if (!textContent.trim()) {
        toast({
          title: 'Copy Failed',
          description: 'No content found to copy',
          variant: 'destructive',
        });
        return;
      }

      const attribution = documentSource ? `\n\nSource: ${documentSource}` : '';
      const finalContent = textContent + attribution;

      await navigator.clipboard.writeText(finalContent);
      toast({
        title: 'Copied!',
        description: 'Document text copied to clipboard',
      });
    } catch {
      try {
        // Fallback for older browsers
        const textArea = document.createElement('textarea');
        textArea.value = extractCleanText();
        document.body.appendChild(textArea);
        textArea.select();
        document.execCommand('copy');
        document.body.removeChild(textArea);
        toast({
          title: 'Copied!',
          description: 'Document text copied to clipboard',
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

  const saveAsMarkdown = () => {
    try {
      const markdownContent = extractMarkdownContent();

      if (!markdownContent.trim()) {
        toast({
          title: 'Export Failed',
          description: 'No content found to export',
          variant: 'destructive',
        });
        return;
      }

      const fileName = generateFileName();
      const blob = new Blob([markdownContent], { type: 'text/markdown' });
      const url = URL.createObjectURL(blob);

      const link = document.createElement('a');
      link.href = url;
      link.download = `${fileName}.md`;
      document.body.appendChild(link);
      link.click();

      document.body.removeChild(link);
      URL.revokeObjectURL(url);
    } catch {
      toast({
        title: 'Download Failed',
        description: 'Failed to save document',
        variant: 'destructive',
      });
    }
  };

  return (
    <TooltipProvider>
      <div className="flex gap-2">
        <Tooltip>
          <TooltipTrigger asChild>
            <Button
              variant="ghost"
              size="sm"
              onClick={copyTextContent}
              className="h-8 px-2"
            >
              <Copy className="w-4 h-4" />
            </Button>
          </TooltipTrigger>
          <TooltipContent>
            <p>Copy document text</p>
          </TooltipContent>
        </Tooltip>

        <Tooltip>
          <TooltipTrigger asChild>
            <Button
              variant="ghost"
              size="sm"
              onClick={saveAsMarkdown}
              className="h-8 px-2"
            >
              <Download className="w-4 h-4" />
            </Button>
          </TooltipTrigger>
          <TooltipContent>
            <p>Download as Markdown</p>
          </TooltipContent>
        </Tooltip>
      </div>
    </TooltipProvider>
  );
};
