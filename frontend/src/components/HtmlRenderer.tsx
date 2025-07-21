import React from 'react';
import { ivyTagClassMap } from '@/lib/utils';

interface HtmlRendererProps {
  content: string;
  className?: string;
  allowedTags?: string[];
  onLinkClick?: (
    event: React.MouseEvent<HTMLAnchorElement>,
    href: string
  ) => void;
}

const sanitizeHtml = (
  html: string,
  allowedTags: string[] = [
    'p',
    'div',
    'span',
    'h1',
    'h2',
    'h3',
    'h4',
    'h5',
    'h6',
    'ul',
    'ol',
    'li',
    'a',
    'strong',
    'em',
    'b',
    'i',
    'br',
  ]
): string => {
  const temp = document.createElement('div');
  temp.innerHTML = html;

  // Remove any script tags and inline event handlers
  const scripts = temp.getElementsByTagName('script');
  const elements = temp.getElementsByTagName('*');

  // Remove script tags
  while (scripts[0]) {
    scripts[0].parentNode?.removeChild(scripts[0]);
  }

  // Remove inline event handlers and potentially harmful attributes
  for (let i = 0; i < elements.length; i++) {
    const element = elements[i];
    const attrs = element.attributes;

    for (let j = attrs.length - 1; j >= 0; j--) {
      const attr = attrs[j];
      if (
        attr.name.startsWith('on') ||
        (attr.name === 'href' && attr.value.startsWith('javascript:'))
      ) {
        element.removeAttribute(attr.name);
      }
    }

    // Remove elements with tags not in allowedTags
    if (!allowedTags.includes(element.tagName.toLowerCase())) {
      element.parentNode?.removeChild(element);
    }
  }

  return temp.innerHTML;
};

function domToReact(node: ChildNode): React.ReactNode {
  if (node.nodeType === Node.TEXT_NODE) {
    return node.textContent;
  }
  if (node.nodeType !== Node.ELEMENT_NODE) {
    return null;
  }
  const el = node as HTMLElement;
  const tag = el.tagName.toLowerCase();
  const className = ivyTagClassMap[tag] || undefined;
  const children = Array.from(el.childNodes).map(domToReact);
  const props: Record<string, unknown> = { className };

  // Special handling for <a>, <img>, <code>, etc.
  if (tag === 'a') {
    props.href = el.getAttribute('href');
    props.target = el.getAttribute('target') || undefined;
    props.rel = el.getAttribute('rel') || undefined;
    props.onClick = (e: React.MouseEvent<HTMLAnchorElement>) => {
      if (
        props.href &&
        typeof props.href === 'string' &&
        props.href.startsWith('javascript:')
      ) {
        e.preventDefault();
        return;
      }
    };
  } else if (tag === 'img') {
    props.src = el.getAttribute('src');
    props.alt = el.getAttribute('alt') || '';
    props.loading = 'lazy';
  } else if (tag === 'code') {
    props.children = el.textContent;
    return React.createElement('code', props);
  }

  return React.createElement(tag, props, ...children);
}

export const HtmlRenderer: React.FC<Omit<HtmlRendererProps, 'onLinkClick'>> = ({
  content,
  className = '',
  allowedTags,
}) => {
  // Sanitize the HTML content
  const sanitizedContent = sanitizeHtml(content, allowedTags);

  // Parse the sanitized HTML into a DOM tree
  const temp = document.createElement('div');
  temp.innerHTML = sanitizedContent;
  const nodes = Array.from(temp.childNodes);

  // Render as React elements with Ivy classes
  return (
    <div
      className={
        className ? `${className} flex flex-col gap-2` : 'flex flex-col gap-2'
      }
    >
      {nodes.map((node, i) => (
        <React.Fragment key={i}>{domToReact(node)}</React.Fragment>
      ))}
    </div>
  );
};
