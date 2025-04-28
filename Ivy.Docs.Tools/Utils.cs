using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ivy.Docs.Tools;

public static class Utils
{
    public static string GetRelativeFolderWithoutOrder(string inputFolder, string inputFile)
    {
        var relativePath = Path.GetRelativePath(inputFolder, Path.GetDirectoryName(inputFile)!);
        var parts = relativePath
            .Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => Regex.Replace(p, @"^\d+_", ""))
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToArray();

        var result = Path.Combine(parts);
        return result == "." ? "" : result;
    }

    public static (int? order, string name) GetOrderFromFileName(string filename)
    {
        // Get name without extension first
        string nameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
    
        var parts = nameWithoutExtension.Split('_');
        if (parts.Length > 1 && int.TryParse(parts[0], out int order))
        {
            return (order, string.Join("_", parts.Skip(1)));
        }
        return (null, nameWithoutExtension);
    }
    
    // public static string? ExtractClassName(string code)
    // {
    //     // Pattern to match "public class [ClassName] :"
    //     string pattern = @"public\s+class\s+(\w+)\s*:";
    //     
    //     // Create regex and find match
    //     Regex regex = new Regex(pattern);
    //     Match match = regex.Match(code);
    //     
    //     if (match is { Success: true, Groups.Count: > 1 })
    //     {
    //         // Return the captured class name
    //         return match.Groups[1].Value;
    //     }
    //
    //     return null;
    // }
    
    public static bool IsView(string code, out string? className)
    {
        className = null;

        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetCompilationUnitRoot();

        var classes = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>();

        foreach (var cls in classes)
        {
            if (cls.BaseList == null) continue;

            var inheritsViewBase = cls.BaseList.Types
                .Any(t => t.Type.ToString() == "ViewBase");

            if (!inheritsViewBase) continue;

            var hasBuildOverride = cls.Members
                .OfType<MethodDeclarationSyntax>()
                .Any(m =>
                    m.Identifier.Text == "Build" &&
                    m.Modifiers.Any(SyntaxKind.OverrideKeyword));

            if (!hasBuildOverride) continue;

            className = cls.Identifier.Text;
            return true;
        }

        return false;
    }
    
    public static string RenameClass(string code, string className)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetRoot();

        var classNode = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .FirstOrDefault();

        if (classNode == null)
            return code;

        var renamedClass = classNode.WithIdentifier(SyntaxFactory.Identifier(className));
        var newRoot = root.ReplaceNode(classNode, renamedClass);

        return newRoot.NormalizeWhitespace().ToFullString();
    }
    
    public static string? GetGitFileUrl(string localFilePath)
    {
        try
        {
            // Ensure the file exists
            if (!File.Exists(localFilePath))
                return null;

            // Get the directory containing the file
            string directory = Path.GetDirectoryName(localFilePath)!;
            
            // Change to the directory
            string currentDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(directory);
            
            // Get the repository remote URL
            string remoteUrl = RunGitCommand("config --get remote.origin.url");
            if (string.IsNullOrEmpty(remoteUrl))
                throw new Exception("No remote origin found for this Git repository.");
            
            // Clean up the remote URL (convert SSH to HTTPS if needed)
            remoteUrl = ConvertToHttpsUrl(remoteUrl.Trim());
            
            // Get the repository root directory
            string repoRoot = RunGitCommand("rev-parse --show-toplevel").Trim();
            
            // Get the current branch name
            string branch = RunGitCommand("rev-parse --abbrev-ref HEAD").Trim();
            
            // Get the relative path of the file within the repo
            string relativePath = localFilePath;
            if (!string.IsNullOrEmpty(repoRoot))
            {
                relativePath = Path.GetRelativePath(repoRoot, localFilePath);
            }
            
            // Replace backslashes with forward slashes for URL
            relativePath = relativePath.Replace('\\', '/');
            
            // Construct the URL
            string fileUrl = $"{remoteUrl}/blob/{branch}/{relativePath}";
            
            // Restore the original directory
            Directory.SetCurrentDirectory(currentDirectory);
            
            return fileUrl;
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    private static string RunGitCommand(string arguments)
    {
        using Process process = new Process();
        process.StartInfo.FileName = "git";
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;
            
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
            
        if (process.ExitCode != 0)
        {
            string error = process.StandardError.ReadToEnd();
            throw new Exception($"Git command failed: {error}");
        }
            
        return output;
    }
    
    private static string ConvertToHttpsUrl(string gitUrl)
    {
        // Convert SSH URL to HTTPS URL if needed
        // Example: git@github.com:username/repo.git -> https://github.com/username/repo
        if (gitUrl.StartsWith("git@"))
        {
            // SSH format: git@github.com:username/repo.git
            Regex sshRegex = new Regex(@"git@([^:]+):([^\.]+)\.git");
            Match match = sshRegex.Match(gitUrl);
            
            if (match.Success)
            {
                string host = match.Groups[1].Value;
                string path = match.Groups[2].Value;
                return $"https://{host}/{path}";
            }
        }
        
        // Already HTTPS format or other format
        // Remove .git suffix if present
        if (gitUrl.EndsWith(".git"))
        {
            gitUrl = gitUrl.Substring(0, gitUrl.Length - 4);
        }
        
        return gitUrl;
    }
}