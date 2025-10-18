import React, { useEffect, useState } from 'react';
import { sanitizeUrl, sanitizeId, loadScript } from './shared';
import EmbedErrorFallback from './EmbedErrorFallback';

interface RedditEmbedProps {
  url: string;
}

const RedditEmbed: React.FC<RedditEmbedProps> = ({ url }) => {
  const [postId, setPostId] = useState<string | null>(null);
  const [scriptLoaded, setScriptLoaded] = useState(false);
  const [scriptError, setScriptError] = useState(false);

  useEffect(() => {
    const extractPostId = (redditUrl: string): string | null => {
      const match = redditUrl.match(/reddit\.com\/r\/[^/]+\/comments\/([^/]+)/);
      return match ? sanitizeId(match[1]) : null;
    };

    setPostId(extractPostId(url));
  }, [url]);

  useEffect(() => {
    if (postId) {
      loadScript('https://embed.redditmedia.com/widgets/platform.js')
        .then(() => {
          setScriptLoaded(true);
        })
        .catch(() => {
          setScriptError(true);
        });
    }
  }, [postId]);

  const sanitizedUrl = sanitizeUrl(url);
  if (!postId || scriptError || !sanitizedUrl) {
    return <EmbedErrorFallback url={url} platform="Reddit" />;
  }

  return (
    <div className="redditwrapper">
      <blockquote className="reddit-card">
        <a href={sanitizedUrl}>
          <p>Posted by u/reddit</p>
          <p>{scriptLoaded ? 'Loading Reddit post...' : 'Loading script...'}</p>
        </a>
      </blockquote>
    </div>
  );
};

export default RedditEmbed;
