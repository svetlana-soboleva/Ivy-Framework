using System.Reflection;
using Ivy.Core;
using Ivy.Core.Docs;
using Ivy.Docs.Helpers;

namespace Ivy.Docs.Test;

public class TypeUtilsTests
{
    [Theory]
    [InlineData("Ivy.ColorInput", typeof(Ivy.ColorInput<>))]
    [InlineData("Ivy.Button", typeof(Ivy.Button))]
    public void GetTypeFromName_ReturnsExpectedType(string typeName, Type expectedType)
    {
        Type? result = TypeUtils.GetTypeFromName(typeName);
        Assert.Equal(expectedType, result);
    }
    
    public class Foo
    {
        public string Name { get; set; }
        public string? Name2 { get; set; }
    }
    
    [Fact]
    public void IsNullableReference_FooName_ReturnsFalse()
    {
        // Arrange
        var property = typeof(Foo).GetProperty(nameof(Foo.Name))!;
        
        // Act
        bool result = TypeUtils.IsNullableReference(property);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsNullableReference_FooName2_ReturnsTrue()
    {
        // Arrange
        var property = typeof(Foo).GetProperty(nameof(Foo.Name2))!;
        
        // Act
        bool result = TypeUtils.IsNullableReference(property);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void GetExtensionMethods_ReturnsExpected0() => 
        GetExtensionMethods_ReturnsExpected(
            typeof(Bar), typeof(BarExtensions), typeof(Bar).GetProperty(nameof(Bar.Name))!, 
            """
            Name(string name)
            Uppercase(string name = "foo")
            """ 
            );
    
    private void GetExtensionMethods_ReturnsExpected(Type baseType, Type extensionsType, PropertyInfo propertyInfo, string expectedResult)
    {
        string result = TypeUtils.GetExtensionMethods(propertyInfo, baseType, extensionsType);
        Assert.Equal(expectedResult, result);
    }
}

public record Bar
{
    [Prop] public string Name { get; set; }
}

public static class BarExtensions
{
    public static Bar Name(this Bar bar, string name)
    {
        return bar with { Name = name };
    }
    
    [RelatedTo(nameof(Bar.Name))]
    public static Bar Uppercase(this Bar bar, string name = "foo")
    {
        return bar with { Name = name.ToUpper() };
    }
}