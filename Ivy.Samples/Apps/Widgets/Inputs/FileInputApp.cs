using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon: Icons.Upload, path: ["Widgets", "Inputs"])]
public class FileInputApp : SampleBase
{
    protected override object? BuildSample()
    {
        // Mock file for 'With Value' example
        var mockFile = new FileInput {
            Name = "example.txt",
            Type = "text/plain",
            Size = 1234,
            LastModified = DateTime.Now,
            Content = null
        };

        var singleFile = UseState<FileInput?>(() => null);
        var singleFileWithValue = UseState<FileInput?>(() => mockFile);
        var multipleFiles = UseState<IEnumerable<FileInput>?>(() => null);
        var multipleFilesWithValue = UseState<IEnumerable<FileInput>?>(() => new[] { mockFile });
        var disabledFile = UseState<FileInput?>(() => null);
        var invalidFile = UseState<FileInput?>(() => null);
        var placeholderFile = UseState<FileInput?>(() => null);
        var limitedFiles = UseState<IEnumerable<FileInput>?>(() => null);
        var textFiles = UseState<IEnumerable<FileInput>?>(() => null);
        var pdfFiles = UseState<IEnumerable<FileInput>?>(() => null);
        var imageFiles = UseState<IEnumerable<FileInput>?>(() => null);

        var onChangedState = UseState<FileInput?>(() => null);
        var onChangeLabel = UseState("");
        var onBlurState = UseState<FileInput?>(() => null);
        var onBlurLabel = UseState("");

        var dataBinding = Layout.Grid().Columns(3)
                          | Text.InlineCode("FileInput")
                          | (Layout.Vertical()
                             | singleFile.ToFileInput()
                             | singleFile.ToFileInput().Multiple()
                          )
                          | singleFile

                          | Text.InlineCode("FileInput?")
                          | (Layout.Vertical()
                             | singleFile.ToFileInput()
                             | singleFile.ToFileInput().Multiple()
                          )
                          | singleFile

                          | Text.InlineCode("IEnumerable<FileInput>")
                          | (Layout.Vertical()
                             | multipleFiles.ToFileInput()
                          )
                          | multipleFiles
            ;

        return Layout.Vertical()
               | Text.H1("File Inputs")
               | Text.H2("Variants")
               | (Layout.Grid().Columns(6)
                  | null!
                  | Text.InlineCode("Empty")
                  | Text.InlineCode("With Value")
                  | Text.InlineCode("Disabled")
                  | Text.InlineCode("Invalid")
                  | Text.InlineCode("With Placeholder")

                  | Text.InlineCode("Single File")
                  | singleFile.ToFileInput()
                  | singleFileWithValue.ToFileInput()
                  | singleFile.ToFileInput().Disabled()
                  | singleFile.ToFileInput().Invalid("Please select a valid file")
                  | placeholderFile.ToFileInput().Placeholder("Click to select a file")

                  | Text.InlineCode("Multiple Files")
                  | multipleFiles.ToFileInput().Multiple()
                  | multipleFilesWithValue.ToFileInput().Multiple()
                  | multipleFiles.ToFileInput().Multiple().Disabled()
                  | multipleFiles.ToFileInput().Multiple().Invalid("Please select valid files")
                  | multipleFiles.ToFileInput().Multiple().Placeholder("Click to select files")
               )

               // Data Binding:
               | Text.H2("Data Binding")
               | dataBinding

               // File Type Restrictions:
               | Text.H2("File Type Restrictions")
               | (Layout.Grid().Columns(3)
                  | Text.InlineCode("File Type")
                  | Text.InlineCode("Accept Property")
                  | Text.InlineCode("File Input")

                  | Text.Block("Text Files")
                  | Text.InlineCode(".txt,.md,.csv")
                  | textFiles.ToFileInput().Multiple().Accept(".txt,.md,.csv").Placeholder("Select text files")

                  | Text.Block("PDF Files")
                  | Text.InlineCode(".pdf")
                  | pdfFiles.ToFileInput().Multiple().Accept(".pdf").Placeholder("Select PDF files")

                  | Text.Block("Images")
                  | Text.InlineCode(".jpg,.jpeg,.png,.gif,.webp")
                  | imageFiles.ToFileInput().Multiple().Accept(".jpg,.jpeg,.png,.gif,.webp").Placeholder("Select image files")

                  | Text.Block("All Files")
                  | Text.InlineCode("(default)")
                  | singleFile.ToFileInput().Placeholder("Select any file")
               )

               // File Count Limits:
               | Text.H2("File Count Limits")
               | (Layout.Grid().Columns(3)
                  | Text.InlineCode("Max Files")
                  | Text.InlineCode("Description")
                  | Text.InlineCode("File Input")

                  | Text.Block("No Limit")
                  | Text.Block("Default behavior - no restriction on number of files")
                  | multipleFiles.ToFileInput().Multiple().Placeholder("Select unlimited files")

                  | Text.Block("1 File")
                  | Text.Block("Single file selection only")
                  | singleFile.ToFileInput().MaxFiles(1).Placeholder("Select one file")

                  | Text.Block("3 Files")
                  | Text.Block("Maximum of 3 files allowed")
                  | limitedFiles.ToFileInput().Multiple().MaxFiles(3).Placeholder("Select up to 3 files")

                  | Text.Block("5 Files")
                  | Text.Block("Maximum of 5 files allowed")
                  | limitedFiles.ToFileInput().Multiple().MaxFiles(5).Placeholder("Select up to 5 files")
               )

               // Events: 
               | Text.H2("Events")
               | Text.H3("OnChange")
               | Layout.Horizontal(
                   new FileInput<FileInput?>(onChangedState.Value, e =>
                   {
                       onChangedState.Set(e.Value);
                       onChangeLabel.Set("Changed");
                   }),
                   onChangeLabel
               )
               | Text.H3("OnBlur")
               | Layout.Horizontal(
                   onBlurState.ToFileInput().HandleBlur(e => onBlurLabel.Set("Blur")),
                   onBlurLabel
               )

               // File Content Display:
               | Text.H2("File Content Display")
               | (Layout.Grid().Columns(2)
                  | Text.InlineCode("File Input")
                  | Text.InlineCode("File Details")

                  | singleFile.ToFileInput().Placeholder("Select a text file to view content")
                  | singleFile.ToDetails().Remove(e => e.Content)

                  | singleFile.ToFileInput().Placeholder("Select a file to view as plain text")
                  | singleFile.Value?.ToPlainText()
               )
            ;
    }
}