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
import { logger } from '../../lib/logger';

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

  const extractMarkdownContent = async (): Promise<string> => {
    logger.info('🔍 extractMarkdownContent started');

    if (!articleRef.current) {
      logger.info('❌ No articleRef.current');
      return '';
    }

    const articleElement = articleRef.current;
    let markdownContent = '';

    // Search for tabs with proper ARIA roles OR Radix-generated attributes
    logger.info('🔍 Searching for tabs...');

    let tabGroups = articleElement.querySelectorAll('[role="tablist"]');
    logger.info(`📋 Article [role="tablist"]: ${tabGroups.length}`);

    // Also look for Radix tabs (they might use different attributes)
    const radixTabLists = articleElement.querySelectorAll('[data-orientation]');
    logger.info(`🎛️ Article [data-orientation]: ${radixTabLists.length}`);

    // Look for any element that contains tab-like buttons
    const tabContainers = articleElement.querySelectorAll('div');
    let possibleTabContainers = 0;
    Array.from(tabContainers).forEach(container => {
      const buttons = container.querySelectorAll('button, div[role="tab"]');
      if (buttons.length >= 2) {
        const hasTabLikeText = Array.from(buttons).some(btn => {
          const text = btn.textContent?.trim().toLowerCase() || '';
          return (
            text.includes('demo') ||
            text.includes('code') ||
            text.includes('example')
          );
        });
        if (hasTabLikeText) {
          possibleTabContainers++;
          if (possibleTabContainers <= 3) {
            logger.info(
              `🎯 Possible tab container:`,
              container.className,
              Array.from(buttons).map(b => b.textContent?.trim())
            );
          }
        }
      }
    });
    logger.info(`📦 Possible tab containers: ${possibleTabContainers}`);

    // If no standard tabs found, search entire document
    if (tabGroups.length === 0) {
      logger.info('🌐 Searching entire document...');
      tabGroups = document.querySelectorAll('[role="tablist"]');
      logger.info(`📋 Document [role="tablist"]: ${tabGroups.length}`);

      const docRadixTabs = document.querySelectorAll('[data-orientation]');
      logger.info(`🎛️ Document [data-orientation]: ${docRadixTabs.length}`);

      // If we found Radix tabs but no standard ones, use them
      if (tabGroups.length === 0 && docRadixTabs.length > 0) {
        logger.info('✅ Using Radix tabs from document');
        tabGroups = docRadixTabs;
      }
    }

    logger.info(`🎯 Final tab groups: ${tabGroups.length}`);

    // Find the currently active tab
    let originalActiveTab = articleElement.querySelector(
      '[role="tab"][aria-selected="true"], [role="tab"][data-state="active"]'
    );

    // If not found in article, search document
    if (!originalActiveTab) {
      originalActiveTab = document.querySelector(
        '[role="tab"][aria-selected="true"], [role="tab"][data-state="active"]'
      );
    }

    logger.info(
      `📌 Original active tab:`,
      originalActiveTab?.textContent?.trim()
    );

    if (tabGroups.length > 0) {
      markdownContent += '\n## All Tabs Content\n\n';

      // For each tab group, trigger backend events to load all tabs
      for (const tabList of Array.from(tabGroups)) {
        const tabs = Array.from(tabList.querySelectorAll('[role="tab"]'));

        logger.info(`🔄 Processing tab group with ${tabs.length} tabs`);

        // Force backend loading by actually clicking each tab (simulating real user behavior)
        for (let tabIndex = 0; tabIndex < tabs.length; tabIndex++) {
          const tabElement = tabs[tabIndex] as HTMLElement;
          const tabLabel = tabElement.textContent?.trim() || `Tab ${tabIndex}`;

          logger.info(
            `Opening tab ${tabIndex} (${tabLabel}) to force backend load...`
          );

          // Actually click the tab - this triggers the real backend loading
          tabElement.click();

          // Wait for backend to respond and cache the content (longer wait)
          await new Promise(resolve => setTimeout(resolve, 1000));

          // Check if content was loaded
          const activePanel = articleElement.querySelector(
            '[role="tabpanel"]:not([hidden])'
          );
          const hasContent =
            activePanel && (activePanel.textContent?.trim().length || 0) > 20;
          const contentLength = activePanel?.textContent?.trim().length || 0;

          logger.info(
            `Tab ${tabIndex} (${tabLabel}): Content loaded = ${hasContent ? 'Yes' : 'No'} (${contentLength} chars)`
          );
        }
      }

      // Now extract content from ALL loaded tabs sequentially
      for (const tabList of Array.from(tabGroups)) {
        const tabs = Array.from(tabList.querySelectorAll('[role="tab"]'));

        for (let i = 0; i < tabs.length; i++) {
          const tab = tabs[i] as HTMLElement;
          const tabLabel = tab.textContent?.trim() || `Tab ${i + 1}`;

          // Click this tab to make its panel visible for extraction
          tab.click();

          // Wait for panel to become active
          await new Promise(resolve => setTimeout(resolve, 100));

          const activePanel = articleElement.querySelector(
            '[role="tabpanel"]:not([hidden]), [role="tabpanel"][data-state="active"]'
          );

          if (activePanel) {
            markdownContent += `### ${tabLabel}\n\n`;

            const codeBlocks = activePanel.querySelectorAll('pre, code');

            if (codeBlocks.length > 0) {
              codeBlocks.forEach(codeBlock => {
                const code = codeBlock.textContent?.trim();
                if (code && code.length > 10) {
                  markdownContent += `\`\`\`\n${code}\n\`\`\`\n\n`;
                }
              });
            } else {
              const panelText = activePanel.textContent?.trim();
              if (panelText && panelText.length > 10) {
                markdownContent += `${panelText}\n\n`;
              }
            }
          }
        }
      }

      // Restore original active tab by clicking it
      if (originalActiveTab && originalActiveTab instanceof HTMLElement) {
        logger.info('🔄 Restoring original active tab...');
        originalActiveTab.click();
        await new Promise(resolve => setTimeout(resolve, 300));
      }
    }

    // Now work with a clone for the rest of the content extraction
    const clone = articleElement.cloneNode(true) as HTMLElement;

    // Remove UI elements but keep all content - IMPORTANT: Don't remove tab content
    const elementsToRemove = [
      'nav',
      'button:not([data-copy-button])',
      '.toc',
      '.table-of-contents',
      '.navigation',
      '.breadcrumb',
      '.pagination',
      '.sidebar',
      '[data-testid="copy-button"]',
    ];

    elementsToRemove.forEach(selector => {
      const elements = clone.querySelectorAll(selector);
      elements.forEach(el => el.remove());
    });

    // Make all tab content visible for extraction by temporarily overriding display styles
    const hiddenElements = clone.querySelectorAll(
      '[style*="display: none"], [style*="display:none"], .hidden, [hidden]'
    );
    const originalStyles = new Map<Element, string>();

    hiddenElements.forEach(element => {
      // Store original style for restoration (though we won't restore since this is a clone)
      originalStyles.set(element, element.getAttribute('style') || '');

      // Temporarily make visible for extraction
      if (element instanceof HTMLElement) {
        element.style.display = 'block';
        element.style.visibility = 'visible';
        element.removeAttribute('hidden');
        element.classList.remove('hidden');
      }
    });

    // Get ALL content elements in document order - much more comprehensive
    const walker = document.createTreeWalker(clone, NodeFilter.SHOW_ELEMENT, {
      acceptNode: node => {
        const element = node as Element;
        const tagName = element.tagName.toLowerCase();

        // Accept all content elements
        if (
          [
            'h1',
            'h2',
            'h3',
            'h4',
            'h5',
            'h6',
            'p',
            'div',
            'section',
            'article',
            'ul',
            'ol',
            'li',
            'blockquote',
            'pre',
            'code',
            'table',
            'thead',
            'tbody',
            'tr',
            'th',
            'td',
            'dl',
            'dt',
            'dd',
          ].includes(tagName)
        ) {
          return NodeFilter.FILTER_ACCEPT;
        }
        return NodeFilter.FILTER_SKIP;
      },
    });

    const processedElements = new Set<Element>();
    let currentElement: Element | null;

    while ((currentElement = walker.nextNode() as Element)) {
      // Skip if we've already processed this element or its parent
      if (processedElements.has(currentElement)) continue;

      const tagName = currentElement.tagName.toLowerCase();
      const text = currentElement.textContent?.trim() || '';

      // Skip empty elements
      if (!text && !['table', 'ul', 'ol'].includes(tagName)) continue;

      switch (tagName) {
        case 'h1':
        case 'h2':
        case 'h3':
        case 'h4':
        case 'h5':
        case 'h6': {
          const level = parseInt(tagName[1]);
          markdownContent += `${'#'.repeat(level)} ${text}\n\n`;
          processedElements.add(currentElement);
          break;
        }

        case 'p':
          if (text && !currentElement.closest('li, td, th')) {
            markdownContent += `${text}\n\n`;
            processedElements.add(currentElement);
          }
          break;

        case 'div':
        case 'section':
        case 'article': {
          // Only process if it's a direct content container, not nested
          if (!currentElement.closest('p, li, td, th') && text) {
            // Check if it contains mostly just text (not other block elements)
            const childBlocks = currentElement.querySelectorAll(
              'h1, h2, h3, h4, h5, h6, p, div, section, ul, ol, table'
            );
            if (childBlocks.length === 0) {
              markdownContent += `${text}\n\n`;
              processedElements.add(currentElement);
            }
          }
          break;
        }

        case 'ul':
        case 'ol':
          if (!processedElements.has(currentElement)) {
            markdownContent += extractListToMarkdown(currentElement);
            processedElements.add(currentElement);
          }
          break;

        case 'table':
          if (!processedElements.has(currentElement)) {
            markdownContent += extractTableToMarkdown(
              currentElement as HTMLTableElement
            );
            processedElements.add(currentElement);
          }
          break;

        case 'pre':
          if (!processedElements.has(currentElement)) {
            const codeElement = currentElement.querySelector('code');
            const codeText = codeElement
              ? codeElement.textContent
              : currentElement.textContent;
            markdownContent += `\`\`\`\n${codeText || ''}\n\`\`\`\n\n`;
            processedElements.add(currentElement);
          }
          break;

        case 'code':
          // Only process inline code (not inside pre)
          if (
            !currentElement.closest('pre') &&
            !processedElements.has(currentElement)
          ) {
            // This is tricky - we'll skip standalone code elements to avoid duplication
            // They're usually handled by their parent elements
          }
          break;

        case 'blockquote':
          if (!processedElements.has(currentElement)) {
            markdownContent += `> ${text}\n\n`;
            processedElements.add(currentElement);
          }
          break;

        case 'dl':
          if (!processedElements.has(currentElement)) {
            markdownContent += extractDefinitionListToMarkdown(currentElement);
            processedElements.add(currentElement);
          }
          break;
      }
    }

    // Add source attribution if available
    if (documentSource) {
      markdownContent += `\n---\n*Source: [${documentSource}](${documentSource})*\n`;
    }

    return markdownContent.trim();
  };

  // Helper function to extract table as markdown
  const extractTableToMarkdown = (table: HTMLTableElement): string => {
    let tableMarkdown = '\n';
    const rows = Array.from(table.querySelectorAll('tr'));

    if (rows.length === 0) return '';

    rows.forEach((row, rowIndex) => {
      const cells = Array.from(row.querySelectorAll('td, th'));
      const cellTexts = cells.map(cell =>
        (cell.textContent || '').trim().replace(/\|/g, '\\|')
      );

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

  // Helper function to extract definition lists as markdown
  const extractDefinitionListToMarkdown = (dl: Element): string => {
    let dlMarkdown = '\n';
    const children = Array.from(dl.children);

    for (let i = 0; i < children.length; i++) {
      const element = children[i];
      const tagName = element.tagName.toLowerCase();
      const text = element.textContent?.trim() || '';

      if (tagName === 'dt' && text) {
        dlMarkdown += `**${text}**\n`;
      } else if (tagName === 'dd' && text) {
        dlMarkdown += `: ${text}\n\n`;
      }
    }

    return dlMarkdown;
  };

  const copyTextContent = async () => {
    try {
      // Show loading state
      toast({
        title: 'Loading Content...',
        description: 'Fetching all tab content from backend...',
      });

      const markdownContent = await extractMarkdownContent();
      const textContent = extractCleanText();

      const attribution = documentSource ? `\n\nSource: ${documentSource}` : '';
      const fullContent = markdownContent + '\n\n' + textContent + attribution;

      if (!fullContent.trim()) {
        toast({
          title: 'Copy Failed',
          description: 'No content found to copy',
          variant: 'destructive',
        });
        return;
      }

      await navigator.clipboard.writeText(fullContent);
      toast({
        title: 'Copied!',
        description: 'Complete document including all tabs copied to clipboard',
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

  const saveAsMarkdown = async () => {
    try {
      // Show loading state
      toast({
        title: 'Extracting Content...',
        description: 'Fetching all tab content from backend...',
      });

      const markdownContent = await extractMarkdownContent();

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

      toast({
        title: 'Export Complete!',
        description: `Downloaded ${fileName}.md with complete content including all tabs`,
      });
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
