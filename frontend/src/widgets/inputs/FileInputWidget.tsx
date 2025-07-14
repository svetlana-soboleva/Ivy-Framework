import React, { useCallback, useState, useRef } from 'react';
import { Input } from '@/components/ui/input';
import { useEventHandler } from '@/components/EventHandlerContext';
import { Upload, X } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { cn } from '@/lib/utils';
import { getWidth } from '@/lib/styles';
import { InvalidIcon } from '@/components/InvalidIcon';

/**
 * Represents a file uploaded through the FileInput widget.
 * Contains metadata about the file and optionally its binary content.
 */
interface FileInput {
  /** The original filename of the uploaded file (e.g., "document.pdf") */
  name: string;
  /** The size of the file in bytes */
  size: number;
  /** The MIME type of the file (e.g., "application/pdf", "image/jpeg") */
  type: string;
  /** The last modified timestamp of the file from the user's system */
  lastModified: Date;
  /** The binary content of the file, encoded as base64 string */
  content?: string;
}

/**
 * Props for the FileInputWidget component.
 * Defines all configuration options and validation rules for file uploads.
 */
interface FileInputWidgetProps {
  /** Unique identifier for this widget instance */
  id: string;
  /** Current file value(s) - can be single file, array of files, or null */
  value?: FileInput | FileInput[] | null;
  /** When true, the file input is disabled and cannot accept files */
  disabled: boolean;
  /** Error message to display when validation fails (e.g., "File too large") */
  invalid?: string;

  /** CSS width value for the widget container */
  width?: string;
  /** Comma-separated list of accepted file extensions or MIME types (e.g., ".pdf,.doc" or "image/*") */
  accept?: string;
  /** When true, allows selecting multiple files. When false, only single file selection is allowed */
  multiple?: boolean;
  /** Minimum file size in bytes. Files smaller than this will be rejected */
  minSize?: number;
  /** Maximum file size in bytes. Files larger than this will be rejected */
  maxSize?: number;
  /** Minimum number of files required when multiple is true. Validation fails if fewer files are selected */
  minFiles?: number;
  /** Maximum number of files allowed when multiple is true. Validation fails if more files are selected */
  maxFiles?: number;
}

/**
 * FileInputWidget component that provides drag-and-drop file upload functionality.
 * Supports single and multiple file selection with comprehensive validation options.
 *
 * Features:
 * - Drag and drop interface with visual feedback
 * - File type restrictions via accept prop
 * - File size limits (min/max)
 * - File count limits (min/max)
 * - Real-time validation with user-friendly error messages
 * - Base64 encoding of file content for backend processing
 */
export const FileInputWidget: React.FC<FileInputWidgetProps> = ({
  id,
  value,
  disabled,
  invalid,
  width,
  accept,
  multiple = false,
  minSize,
  maxSize,
  minFiles,
  maxFiles,
}) => {
  /** Event handler for communicating with the backend */
  const handleEvent = useEventHandler();
  /** Tracks whether files are being dragged over the drop zone */
  const [isDragging, setIsDragging] = useState(false);
  /** Reference to the hidden file input element for programmatic access */
  const inputRef = useRef<HTMLInputElement>(null);

  /**
   * Converts a browser File object to our FileInput interface format.
   * Reads the file content and encodes it as base64 for backend processing.
   *
   * @param file - The browser File object to convert
   * @returns Promise that resolves to our FileInput format
   */
  const convertFileToUploadFile = useCallback(
    async (file: File): Promise<FileInput> => {
      const arrayBuffer = await file.arrayBuffer();
      const base64 = btoa(String.fromCharCode(...new Uint8Array(arrayBuffer)));

      return {
        name: file.name,
        size: file.size,
        type: file.type,
        lastModified: new Date(file.lastModified),
        content: base64,
      };
    },
    []
  );

  /**
   * Validates files against size and count restrictions.
   * Returns an error message if validation fails, null if validation passes.
   *
   * @param files - Array of files to validate
   * @returns Error message string or null if validation passes
   */
  const validateFiles = useCallback(
    (files: File[]): string | null => {
      // Check file count limits
      if (minFiles !== undefined && files.length < minFiles) {
        return `At least ${minFiles} file${minFiles > 1 ? 's' : ''} required`;
      }
      if (maxFiles !== undefined && files.length > maxFiles) {
        return `Maximum ${maxFiles} file${maxFiles > 1 ? 's' : ''} allowed`;
      }

      // Check file size limits
      for (const file of files) {
        if (minSize !== undefined && file.size < minSize) {
          return `File "${file.name}" is too small. Minimum size: ${formatFileSize(minSize)}`;
        }
        if (maxSize !== undefined && file.size > maxSize) {
          return `File "${file.name}" is too large. Maximum size: ${formatFileSize(maxSize)}`;
        }
      }

      return null;
    },
    [minFiles, maxFiles, minSize, maxSize]
  );

  /**
   * Converts bytes to a human-readable file size string.
   * Automatically selects appropriate unit (Bytes, KB, MB, GB).
   *
   * @param bytes - Number of bytes to format
   * @returns Human-readable file size string (e.g., "1.5 MB")
   */
  const formatFileSize = (bytes: number): string => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  };

  const handleChange = useCallback(
    async (e: React.ChangeEvent<HTMLInputElement>) => {
      const files = e.target.files;
      if (!files) return;

      const fileArray = Array.from(files);
      const validationError = validateFiles(fileArray);

      if (validationError) {
        handleEvent('OnChange', id, [null]);
        return;
      }

      const selectedFiles = multiple
        ? await Promise.all(fileArray.map(convertFileToUploadFile))
        : await convertFileToUploadFile(fileArray[0]);

      handleEvent('OnChange', id, [selectedFiles]);
    },
    [id, multiple, handleEvent, convertFileToUploadFile, validateFiles]
  );

  const handleClear = useCallback(() => {
    handleEvent('OnChange', id, [null]);
  }, [id, handleEvent]);

  const handleDragEnter = useCallback(
    (e: React.DragEvent) => {
      e.preventDefault();
      e.stopPropagation();
      if (!disabled) {
        setIsDragging(true);
      }
    },
    [disabled]
  );

  const handleDragLeave = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);
  }, []);

  const handleDragOver = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
  }, []);

  const handleDrop = useCallback(
    async (e: React.DragEvent) => {
      e.preventDefault();
      e.stopPropagation();
      setIsDragging(false);

      if (disabled) return;

      const files = Array.from(e.dataTransfer.files);
      if (files.length === 0) return;

      const validationError = validateFiles(files);

      if (validationError) {
        handleEvent('OnChange', id, [null]);
        // You could emit an error event here if needed
        return;
      }

      const selectedFiles = multiple
        ? await Promise.all(files.map(convertFileToUploadFile))
        : await convertFileToUploadFile(files[0]);

      handleEvent('OnChange', id, [selectedFiles]);
    },
    [
      id,
      multiple,
      handleEvent,
      disabled,
      convertFileToUploadFile,
      validateFiles,
    ]
  );

  const handleClick = useCallback(() => {
    if (!disabled && inputRef.current) {
      inputRef.current.click();
    }
  }, [disabled]);

  const displayValue = value
    ? Array.isArray(value)
      ? value.map(f => f.name).join(', ')
      : value.name
    : '';

  return (
    <div
      className="relative"
      style={{ ...getWidth(width) }}
      onDragEnter={handleDragEnter}
      onDragLeave={handleDragLeave}
      onDragOver={handleDragOver}
      onDrop={handleDrop}
    >
      <div
        className={cn(
          'relative rounded-md border-2 border-dashed transition-colors min-h-[100px]',
          isDragging && !disabled
            ? 'border-primary bg-primary/5'
            : 'border-muted-foreground/25',
          disabled ? 'opacity-50 cursor-not-allowed' : 'cursor-pointer'
        )}
        onClick={handleClick}
      >
        <Input
          ref={inputRef}
          type="file"
          id={id}
          accept={accept}
          multiple={multiple}
          onChange={handleChange}
          disabled={disabled}
          className="hidden"
        />
        <div className="absolute inset-0 flex flex-col items-center justify-center p-4 text-center">
          <Upload className="h-6 w-6 mb-2 text-muted-foreground" />
          <p className="text-sm text-muted-foreground">
            {displayValue || (
              <>
                Drag and drop your {multiple ? 'files' : 'file'} here or click
                to select
                {/* {accept && (
                  <span className="block mt-1 text-xs">
                    Accepted file types: {accept}
                  </span>
                )} */}
              </>
            )}
          </p>
        </div>
        {value && !disabled && (
          <Button
            type="button"
            variant="ghost"
            size="icon"
            className="absolute right-2 top-2 h-6 w-6 z-10"
            onClick={e => {
              e.stopPropagation();
              handleClear();
            }}
          >
            <X className="h-4 w-4" />
          </Button>
        )}
      </div>
      {invalid && <InvalidIcon message={invalid} />}
    </div>
  );
};
