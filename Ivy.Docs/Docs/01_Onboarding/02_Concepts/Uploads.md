# Uploads

Uploads in Ivy provide a robust system for handling file uploads in your applications. Whether you're allowing users to upload documents, images, or any other type of file, Ivy's upload functionality makes it easy to manage file transfers from the client to the server.

## Overview

The upload system in Ivy supports:
- Single and multiple file uploads
- Drag and drop interfaces
- Progress tracking
- File validation
- Error handling
- Preview capabilities

## Basic Usage

Here's a simple example of handling file uploads:

```csharp
var client = this.UseService<IClientProvider>();
client.UploadFiles(async files => {
    foreach (var file in files)
    {
        await ProcessFile(file);
    }
});
```

## File Input Component

The most common way to handle uploads is using the FileInput component:

```csharp
public class UploadView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var isUploading = UseState(false);

        return new FileInput(
            "Choose Files",
            multiple: true,
            accept: ".pdf,.doc,.docx",
            onChange: async files => {
                isUploading.Set(true);
                try {
                    await client.UploadFilesAsync(files);
                } finally {
                    isUploading.Set(false);
                }
            }
        );
    }
}
```

## Progress Tracking

Track upload progress for better user feedback:

```csharp
public class UploadWithProgressView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var progress = UseState(0.0);
        var isUploading = UseState(false);

        return Layout.Vertical(
            isUploading.Value
                ? new ProgressBar(progress.Value)
                : null,
            new FileInput(
                "Upload Files",
                onChange: async files => {
                    isUploading.Set(true);
                    try {
                        await client.UploadFilesAsync(
                            files,
                            onProgress: p => progress.Set(p)
                        );
                    } finally {
                        isUploading.Set(false);
                        progress.Set(0.0);
                    }
                }
            )
        );
    }
}
```

## Drag and Drop

Ivy provides built-in support for drag and drop file uploads:

```csharp
public class DragDropView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var isDragging = UseState(false);

        return new DragDrop(
            onDragEnter: _ => isDragging.Set(true),
            onDragLeave: _ => isDragging.Set(false),
            onDrop: async files => {
                isDragging.Set(false);
                await client.UploadFilesAsync(files);
            }
        )
        | new TextBlock(
            isDragging.Value
                ? "Drop files here"
                : "Drag files here or click to upload"
          );
    }
}
```

## File Validation

Validate files before upload:

```csharp
public class ValidatedUploadView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var error = UseState<string?>(null);

        return Layout.Vertical(
            error.Value != null
                ? new Alert(error.Value, variant: AlertVariant.Error)
                : null,
            new FileInput(
                "Upload Image",
                accept: ".jpg,.jpeg,.png",
                onChange: async files => {
                    foreach (var file in files)
                    {
                        if (file.Size > 5 * 1024 * 1024) // 5MB limit
                        {
                            error.Set("File size must be less than 5MB");
                            return;
                        }
                    }
                    await client.UploadFilesAsync(files);
                }
            )
        );
    }
}
```

## Best Practices

1. **File Validation**: Validate file types and sizes before upload
2. **Progress Feedback**: Show upload progress for better UX
3. **Error Handling**: Implement proper error handling
4. **Security**: Validate files on the server side
5. **User Feedback**: Provide clear feedback about upload status
6. **Accessibility**: Ensure upload interfaces are accessible

## Examples

### Image Upload with Preview

```csharp
public class ImageUploadView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var preview = UseState<string?>(null);
        var isUploading = UseState(false);

        return Layout.Vertical(
            preview.Value != null
                ? new Image(preview.Value)
                : null,
            new FileInput(
                "Upload Image",
                accept: "image/*",
                onChange: async files => {
                    if (files.Length == 0) return;
                    
                    isUploading.Set(true);
                    try {
                        var file = files[0];
                        preview.Set(URL.CreateObjectURL(file));
                        await client.UploadFilesAsync(files);
                    } finally {
                        isUploading.Set(false);
                    }
                }
            )
        );
    }
}
```

### Multiple File Upload with List

```csharp
public class MultiFileUploadView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var files = UseState(new List<FileInfo>());
        var isUploading = UseState(false);

        return Layout.Vertical(
            new FileInput(
                "Upload Files",
                multiple: true,
                onChange: async newFiles => {
                    isUploading.Set(true);
                    try {
                        files.Set(files.Value.Concat(newFiles).ToList());
                        await client.UploadFilesAsync(newFiles);
                    } finally {
                        isUploading.Set(false);
                    }
                }
            ),
            new ListView(
                files.Value.Select(f => new TextBlock(f.Name))
            )
        );
    }
}
```

