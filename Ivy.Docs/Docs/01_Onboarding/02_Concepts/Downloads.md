# Downloads

Downloads in Ivy provide a seamless way to handle file downloads in your applications. Whether you're downloading generated reports, user-uploaded files, or any other type of content, Ivy's download functionality makes it easy to manage file transfers between the server and client.

## Overview

The download system in Ivy supports:
- Direct file downloads
- Generated content downloads
- Large file transfers
- Progress tracking
- Error handling

## Basic Usage

Here's a simple example of downloading a file:

```csharp
var client = this.UseService<IClientProvider>();
client.DownloadFile("example.csv", csvContent);
```

## Downloading Generated Content

You can download dynamically generated content:

```csharp
public class ReportView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        
        return new Button(
            "Download Report",
            onClick: async _ => {
                var report = await GenerateReport();
                client.DownloadFile(
                    "report.pdf",
                    report,
                    contentType: "application/pdf"
                );
            }
        );
    }
}
```

## Progress Tracking

For large files, you can track download progress:

```csharp
var client = this.UseService<IClientProvider>();
var progress = UseState(0.0);

client.DownloadFile(
    "large-file.zip",
    fileContent,
    onProgress: p => progress.Set(p)
);
```

## Error Handling

Proper error handling is essential for downloads:

```csharp
public class DownloadView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var error = UseState<string?>(null);

        return Layout.Vertical(
            error.Value != null
                ? new Alert(error.Value, variant: AlertVariant.Error)
                : null,
            new Button(
                "Download",
                onClick: async _ => {
                    try {
                        await client.DownloadFileAsync(
                            "file.txt",
                            content
                        );
                    } catch (Exception ex) {
                        error.Set($"Download failed: {ex.Message}");
                    }
                }
            )
        );
    }
}
```

## Best Practices

1. **File Naming**: Use clear, descriptive filenames
2. **Content Types**: Always specify the correct content type
3. **Error Handling**: Implement proper error handling for failed downloads
4. **Progress Feedback**: Show progress for large files
5. **Security**: Validate file types and sizes before download
6. **Caching**: Consider implementing caching for frequently downloaded files

## Examples

### CSV Export

```csharp
public class DataExportView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var isExporting = UseState(false);

        return new Button(
            "Export to CSV",
            disabled: isExporting.Value,
            onClick: async _ => {
                isExporting.Set(true);
                try {
                    var data = await api.GetExportData();
                    var csv = ConvertToCsv(data);
                    await client.DownloadFileAsync(
                        $"export-{DateTime.Now:yyyy-MM-dd}.csv",
                        csv,
                        contentType: "text/csv"
                    );
                } finally {
                    isExporting.Set(false);
                }
            }
        );
    }
}
```

### Large File Download with Progress

```csharp
public class LargeFileView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var progress = UseState(0.0);
        var isDownloading = UseState(false);

        return Layout.Vertical(
            isDownloading.Value
                ? new ProgressBar(progress.Value)
                : null,
            new Button(
                "Download Large File",
                disabled: isDownloading.Value,
                onClick: async _ => {
                    isDownloading.Set(true);
                    try {
                        await client.DownloadFileAsync(
                            "large-file.zip",
                            fileContent,
                            onProgress: p => progress.Set(p)
                        );
                    } finally {
                        isDownloading.Set(false);
                        progress.Set(0.0);
                    }
                }
            )
        );
    }
}
```