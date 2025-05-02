import CopyToClipboardButton from '@/components/CopyToClipboardButton';
import { getHeight, getWidth } from '@/lib/styles';
import React, { CSSProperties, useState } from 'react';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import ivyPrismTheme from '@/lib/ivy-prism-theme';

interface CodeWidgetProps {
  id: string;
  content: string;
  language: string;
  showCopyButton?: boolean;
  showLineNumbers?: boolean;
  showBorder?: boolean;
  width?: string;
  height?: string;
}

const CodeWidget: React.FC<CodeWidgetProps> = ({ id, content, language, showCopyButton, showLineNumbers, showBorder, width, height }) => {
  const [isHovered, setIsHovered] = useState(false);

  var styles: CSSProperties = { 
    ...getWidth(width),
    ...getHeight(height),
    margin: 0,
    overflow: isHovered ? 'auto' : 'hidden',
  };

  if(!showBorder) {
    styles.border = 'none';
    styles.padding = '0';
  }

  return (<div 
    className="relative" 
    onMouseEnter={() => setIsHovered(true)}
    onMouseLeave={() => setIsHovered(false)}
  >
    {showCopyButton && <div className="absolute top-2 right-2 z-10">
      <CopyToClipboardButton textToCopy={content} />
    </div>}
    <SyntaxHighlighter 
      language={language} 
      customStyle={styles}
      style={ivyPrismTheme} 
      showLineNumbers={showLineNumbers}
      key={id} >
      {content}
    </SyntaxHighlighter>
  </div>);
};

export default CodeWidget;