using System.Text;
using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon: Icons.Upload, path: ["Widgets", "Inputs"])]
public class FileInputApp : SampleBase
{
    protected override object? BuildSample()
    {
        var singleFile = UseState<FileInput?>(() => null);
        var multipleFiles = UseState<IEnumerable<FileInput>?>(() => null);
        
        var onChangedState = UseState<FileInput?>(() => null);
        var onChangeLabel = UseState("");
        var onBlurState = UseState<FileInput?>(() => null);
        var onBlurLabel = UseState("");

        var dataBinding = Layout.Grid().Columns(3)
                          | Text.InlineCode("FileInput?")
                          | singleFile.ToFileInput()
                          | singleFile

                          | Text.InlineCode("IEnumerable<FileInput>?")
                          | multipleFiles.ToFileInput()
                          | multipleFiles
            ;

        return Layout.Vertical()
               | Text.H1("File Inputs")
               | Text.H2("Variants")
               | (Layout.Grid().Columns(5)
                  | null!
                  | Text.InlineCode("Empty")
                  | Text.InlineCode("With Value")
                  | Text.InlineCode("Disabled")
                  | Text.InlineCode("Invalid")

                  | Text.InlineCode("Single File")
                  | singleFile.ToFileInput().Placeholder("Drop a file here")
                  | singleFile.ToFileInput()
                  | singleFile.ToFileInput().Disabled()
                  | singleFile.ToFileInput().Invalid("File is too large")

                  | Text.InlineCode("Multiple Files")
                  | multipleFiles.ToFileInput().Placeholder("Drop files here")
                  | multipleFiles.ToFileInput()
                  | multipleFiles.ToFileInput().Disabled()
                  | multipleFiles.ToFileInput().Invalid("Too many files selected")
               )

               | Text.H2("File Type Restrictions")
               | (Layout.Grid().Columns(3)
                  | Text.InlineCode("Accept")
                  | Text.InlineCode("File Input")
                  | Text.InlineCode("Description")

                  | Text.Block("Images")
                  | singleFile.ToFileInput().Accept(".jpg,.jpeg,.png,.gif")
                  | Text.Block("Common image formats")

                  | Text.Block("Documents")
                  | singleFile.ToFileInput().Accept(".pdf,.doc,.docx,.txt")
                  | Text.Block("PDF and Word documents")

                  | Text.Block("All Files")
                  | singleFile.ToFileInput().Accept("*")
                  | Text.Block("Any file type")
               )

               | Text.H2("File Size Limits")
               | (Layout.Grid().Columns(4)
                  | Text.InlineCode("Min Size")
                  | Text.InlineCode("Max Size")
                  | Text.InlineCode("File Input")
                  | Text.InlineCode("Description")

                  | Text.Block("1KB")
                  | Text.Block("10MB")
                  | singleFile.ToFileInput().MinSize(1024).MaxSize(10 * 1024 * 1024)
                  | Text.Block("Small files only")

                  | Text.Block("None")
                  | Text.Block("5MB")
                  | singleFile.ToFileInput().MaxSize(5 * 1024 * 1024)
                  | Text.Block("Max 5MB files")

                  | Text.Block("100KB")
                  | Text.Block("None")
                  | singleFile.ToFileInput().MinSize(100 * 1024)
                  | Text.Block("Min 100KB files")
               )

               | Text.H2("File Count Limits")
               | (Layout.Grid().Columns(4)
                  | Text.InlineCode("Min Files")
                  | Text.InlineCode("Max Files")
                  | Text.InlineCode("File Input")
                  | Text.InlineCode("Description")

                  | Text.Block("1")
                  | Text.Block("5")
                  | multipleFiles.ToFileInput().MinFiles(1).MaxFiles(5)
                  | Text.Block("1-5 files required")

                  | Text.Block("None")
                  | Text.Block("3")
                  | multipleFiles.ToFileInput().MaxFiles(3)
                  | Text.Block("Max 3 files")

                  | Text.Block("2")
                  | Text.Block("None")
                  | multipleFiles.ToFileInput().MinFiles(2)
                  | Text.Block("Min 2 files")
               )

               | Text.H2("Data Binding")
               | dataBinding

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

               | Text.H2("File Content")
               | (singleFile.Value?.ToPlainText() is string content ? Text.Block(content) : Text.Block("No file selected"))
            ;
    }
}