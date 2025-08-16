import { Users } from 'lucide-react';
import React, { useEffect, useState } from 'react';

interface Contributor {
  login: string;
  avatar_url: string;
  html_url: string;
  contributions: number;
}

interface GitHubCommit {
  author: {
    login: string;
    avatar_url: string;
    html_url: string;
  } | null;
}

interface GitHubContributorsProps {
  documentSource?: string;
}

export const GitHubContributors: React.FC<GitHubContributorsProps> = ({
  documentSource,
}) => {
  const [contributors, setContributors] = useState<Contributor[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!documentSource) return;

    // Extract repo and file path from GitHub URL
    const getApiUrl = (githubUrl: string): string | null => {
      try {
        const url = new URL(githubUrl);
        const pathParts = url.pathname.split('/');

        // Expected format: /owner/repo/blob/branch/path/to/file
        if (pathParts.length < 6 || pathParts[3] !== 'blob') {
          return null;
        }

        const owner = pathParts[1];
        const repo = pathParts[2];
        const filePath = pathParts.slice(5).join('/');

        return `https://api.github.com/repos/${owner}/${repo}/commits?path=${encodeURIComponent(filePath)}&per_page=5`;
      } catch {
        return null;
      }
    };

    const apiUrl = getApiUrl(documentSource);
    if (!apiUrl) return;

    setLoading(true);
    setError(null);

    fetch(apiUrl)
      .then(response => {
        if (!response.ok) {
          throw new Error(`GitHub API error: ${response.status}`);
        }
        return response.json();
      })
      .then((commits: GitHubCommit[]) => {
        // Extract unique contributors from commits
        const contributorMap = new Map<string, Contributor>();

        commits.forEach(commit => {
          if (commit.author) {
            const login = commit.author.login;
            if (contributorMap.has(login)) {
              contributorMap.get(login)!.contributions += 1;
            } else {
              contributorMap.set(login, {
                login: commit.author.login,
                avatar_url: commit.author.avatar_url,
                html_url: commit.author.html_url,
                contributions: 1,
              });
            }
          }
        });

        const sortedContributors = Array.from(contributorMap.values())
          .sort((a, b) => b.contributions - a.contributions)
          .slice(0, 5);

        setContributors(sortedContributors);
      })
      .catch(err => {
        console.error('Failed to fetch contributors:', err);
        setError('Failed to load contributors');
      })
      .finally(() => {
        setLoading(false);
      });
  }, [documentSource]);

  if (!documentSource) return null;

  return (
    <div className="mt-8">
      <div className="text-body mb-4 flex items-center gap-2">
        <Users className="w-4 h-4" />
        Contributors
      </div>

      {loading && (
        <div className="flex flex-col gap-3">
          {[...Array(3)].map((_, i) => (
            <div key={i} className="flex items-center gap-3">
              <div className="w-8 h-8 bg-muted rounded-full animate-pulse"></div>
              <div className="flex-1">
                <div className="h-3 bg-muted rounded animate-pulse w-3/4"></div>
              </div>
            </div>
          ))}
        </div>
      )}

      {error && <div className="text-sm text-muted-foreground">{error}</div>}

      {!loading && !error && contributors.length === 0 && (
        <div className="text-sm text-muted-foreground">
          No contributors found
        </div>
      )}

      {!loading && !error && contributors.length > 0 && (
        <div className="flex flex-col gap-3">
          {contributors.map(contributor => (
            <a
              key={contributor.login}
              href={contributor.html_url}
              target="_blank"
              rel="noopener noreferrer"
              className="flex items-center gap-3 hover:bg-muted rounded-md p-2 transition-colors"
            >
              <img
                src={contributor.avatar_url}
                alt={contributor.login}
                className="w-8 h-8 rounded-full"
              />
              <div className="flex-1 min-w-0">
                <div className="text-sm font-medium truncate">
                  {contributor.login}
                </div>
                <div className="text-xs text-muted-foreground">
                  {contributor.contributions} commit
                  {contributor.contributions !== 1 ? 's' : ''}
                </div>
              </div>
            </a>
          ))}
        </div>
      )}
    </div>
  );
};
