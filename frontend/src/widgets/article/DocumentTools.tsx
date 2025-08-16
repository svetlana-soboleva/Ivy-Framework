import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
  DropdownMenuSeparator,
} from '@/components/ui/dropdown-menu';
import { logger } from '@/lib/logger';
import { Copy, Download, MoreVertical } from 'lucide-react';
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
      .replace(/\n\s*\n\s*\n/g, '\n\n') // Replace multiple newlines with double newlines
      .replace(/^\s+|\s+$/g, '') // Trim start and end
      .replace(/[ \t]+/g, ' '); // Normalize spaces

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
          // Only add inline code if not already inside a pre block
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

      // Add source attribution
      const attribution = documentSource ? `\n\nSource: ${documentSource}` : '';
      const finalContent = textContent + attribution;

      await navigator.clipboard.writeText(finalContent);

      // TODO: Add toast notification
      logger.info('Text content copied to clipboard');
    } catch (error) {
      console.error('Failed to copy text:', error);
      // Fallback for older browsers
      const textArea = document.createElement('textarea');
      textArea.value = extractCleanText();
      document.body.appendChild(textArea);
      textArea.select();
      document.execCommand('copy');
      document.body.removeChild(textArea);
    }
  };

  const generateFileName = (): string => {
    // If we have a meaningful title, use it
    if (title && title !== 'document') {
      return title
        .toLowerCase()
        .replace(/\s+/g, '-')
        .replace(/[^a-z0-9-]/g, '');
    }

    // Try to extract title from the first H1 in the document
    if (articleRef.current) {
      const firstHeading = articleRef.current.querySelector('h1');
      if (firstHeading?.textContent) {
        return firstHeading.textContent
          .toLowerCase()
          .replace(/\s+/g, '-')
          .replace(/[^a-z0-9-]/g, '')
          .substring(0, 50); // Limit length
      }
    }

    // Try to extract filename from documentSource URL
    if (documentSource) {
      try {
        const url = new URL(documentSource);
        const pathParts = url.pathname.split('/');
        const fileName = pathParts[pathParts.length - 1];

        if (fileName && fileName.includes('.')) {
          // Remove extension and use the base name
          return fileName
            .split('.')[0]
            .toLowerCase()
            .replace(/[^a-z0-9-]/g, '-');
        }
      } catch {
        logger.error(
          'Failed to generate file name from document source',
          documentSource
        );
      }
    }

    // Final fallback with timestamp
    const timestamp = new Date().toISOString().split('T')[0]; // YYYY-MM-DD format
    return `ivy-document-${timestamp}`;
  };

  const saveAsMarkdown = () => {
    try {
      const markdownContent = extractMarkdownContent();
      const fileName = generateFileName();

      // Create blob and download
      const blob = new Blob([markdownContent], { type: 'text/markdown' });
      const url = URL.createObjectURL(blob);

      const link = document.createElement('a');
      link.href = url;
      link.download = `${fileName}.md`;
      document.body.appendChild(link);
      link.click();

      // Cleanup
      document.body.removeChild(link);
      URL.revokeObjectURL(url);

      logger.info(`Markdown file downloaded: ${fileName}.md`);
    } catch (error) {
      console.error('Failed to save markdown file:', error);
    }
  };

  return (
    <div className="mt-8">
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button
            variant="ghost"
            size="sm"
            className="justify-start w-full h-auto p-0 font-normal hover:bg-transparent"
          >
            <div className="text-body flex items-center gap-2">
              <MoreVertical className="w-4 h-4" />
              Document Tools
            </div>
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="start" className="w-48">
          <DropdownMenuItem onClick={copyTextContent}>
            <Copy className="w-4 h-4 mr-2" />
            Copy text content
          </DropdownMenuItem>
          <DropdownMenuSeparator />
          <DropdownMenuItem onClick={saveAsMarkdown}>
            <Download className="w-4 h-4 mr-2" />
            Save as Markdown
          </DropdownMenuItem>
        </DropdownMenuContent>
      </DropdownMenu>
    </div>
  );
};
