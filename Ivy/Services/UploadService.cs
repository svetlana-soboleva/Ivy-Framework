using System.Collections.Concurrent;
using System.Reactive.Disposables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Ivy.Services;

public class UploadController(AppSessionStore sessionStore) : Controller
{
    [Route("upload/{connectionId}/{uploadId}")]
    public async Task<IActionResult> Upload(string connectionId, string uploadId, IFormFile file)
    {
        if (string.IsNullOrEmpty(connectionId))
        {
            return BadRequest("connectionId is required.");
        }
        if (string.IsNullOrEmpty(uploadId))
        {
            return BadRequest("uploadId is required.");
        }
        if (file == null)
        {
            return BadRequest("file is required.");
        }
        if (sessionStore.Sessions.TryGetValue(connectionId, out var session))
        {
            var uploadService = session.AppServices.GetRequiredService<IUploadService>();
            return await uploadService.Upload(uploadId, file);
        }
        throw new Exception($"Session for connectionId '{connectionId}' not found.");
    }
}

public class UploadService(string connectionId) : IUploadService, IDisposable
{
    private readonly ConcurrentDictionary<Guid, (Func<byte[], Task> handler, string mimeType, string fileName)> _uploads = new();

    public (IDisposable cleanup, string url) AddUpload(Func<byte[], Task> handler, string mimeType, string fileName)
    {
        var uploadId = Guid.NewGuid();
        _uploads[uploadId] = (handler, mimeType, fileName);

        var cleanup = Disposable.Create(() =>
        {
            _uploads.TryRemove(uploadId, out _);
        });

        return (cleanup, $"/upload/{connectionId}/{uploadId}");
    }

    public async Task<IActionResult> Upload(string uploadId, IFormFile file)
    {
        if (!Guid.TryParse(uploadId, out var guid) || !_uploads.TryGetValue(guid, out var upload))
        {
            return new BadRequestObjectResult($"Invalid or unknown uploadId: '{uploadId}'.");
        }

        var (handler, contentType, fileName) = upload;

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        await handler(fileBytes);

        return new OkResult();
    }

    public void Dispose()
    {
    }
}

public interface IUploadService
{
    (IDisposable cleanup, string url) AddUpload(Func<byte[], Task> handler, string mimeType, string fileName);

    Task<IActionResult> Upload(string uploadId, IFormFile file);
}