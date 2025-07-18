/**
 * Logger utility for frontend debugging
 * Provides formatted timestamp logs with different log levels
 */

class Logger {
  /**
   * Format current time as HH:mm:ss:milliseconds
   */
  private formatTime(): string {
    const now = new Date();
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const seconds = String(now.getSeconds()).padStart(2, '0');
    const milliseconds = String(now.getMilliseconds()).padStart(3, '0');

    return `${hours}:${minutes}:${seconds}:${milliseconds}`;
  }

  /**
   * Log message with timestamp at debug level
   */
  debug(...args: unknown[]): void {
    console.debug(`[${this.formatTime()}]`, ...args);
  }

  /**
   * Log message with timestamp at info level
   */
  info(...args: unknown[]): void {
    console.info(`[${this.formatTime()}]`, ...args);
  }

  /**
   * Log message with timestamp at warn level
   */
  warn(...args: unknown[]): void {
    console.warn(`[${this.formatTime()}]`, ...args);
  }

  /**
   * Log message with timestamp at error level
   */
  error(...args: unknown[]): void {
    console.error(`[${this.formatTime()}]`, ...args);
  }
}

// Export singleton instance
export const logger = new Logger();
