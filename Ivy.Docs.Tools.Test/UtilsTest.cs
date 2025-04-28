using Ivy.Docs.Tools;

namespace Ivy.MdToIvy.Test;

public class UtilsTest 
{
    [Theory]
    [InlineData(@"C:\Foo\Bar\Input\", @"C:\Foo\Bar\Input\01_Baz\03_Fizz\Goo\Qux.md", @"Baz\Fizz\Goo")]
    [InlineData(@"C:\Data\", @"C:\Data\Baz\Fizz\Qux.md", @"Baz\Fizz")]
    [InlineData(@"C:\Root\", @"C:\Root\001_Alpha\Beta\Qux.md", @"Alpha\Beta")]
    [InlineData(@"C:\Test\", @"C:\Test\Qux.md", @"")]
    [InlineData(@"C:\Test\", @"C:\Test\01_Baz\Qux.md", @"Baz")]
    [InlineData(@"C:\Test\", @"C:\Test\Baz\Qux.md", @"Baz")]
    [InlineData(@"/mnt/data/input/", @"/mnt/data/input/01_Baz/03_Fizz/Goo/Qux.md", @"Baz\Fizz\Goo")]
    public void GetRelativeFolderWithoutOrder_ReturnsExpected(string inputFolder, string inputFile, string expected)
    {
        var result = Utils.GetRelativeFolderWithoutOrder(inputFolder, inputFile);
        Assert.Equal(expected, result);
    }
}