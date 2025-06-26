using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon: Icons.Check, path: ["Widgets", "Inputs"])]
public class BoolInputApp : SampleBase
{
   protected override object? BuildSample()
   {
      var falseState = UseState(false);
      var trueState = UseState(true);
      var nullState = UseState((bool?)null);

      var variants = Layout.Grid().Columns(6)
             | null!
             | Text.Block("True")
             | Text.Block("False")
             | Text.Block("Disabled")
             | Text.Block("Invalid")
             | Text.Block("Nullable")

             | Text.InlineCode("BoolInputs.Checkbox")
             | trueState.ToBoolInput().Label("Label").Description("Description")
             | falseState.ToBoolInput().Label("Label").Description("Description")
             | trueState.ToBoolInput().Label("Label").Description("Description").Disabled()
             | trueState.ToBoolInput().Label("Label").Description("Description").Invalid("Invalid")
             | nullState.ToBoolInput().Label("Label").Description("Description")

             | null!
             | trueState.ToBoolInput().Label("Label")
             | falseState.ToBoolInput().Label("Label")
             | trueState.ToBoolInput().Label("Label").Disabled()
             | trueState.ToBoolInput().Label("Label").Invalid("Invalid")
             | nullState.ToBoolInput().Label("Label")

             | Text.InlineCode("BoolInputs.Switch")
             | trueState.ToSwitchInput().Label("Label").Description("Description")
             | falseState.ToSwitchInput().Label("Label").Description("Description")
             | trueState.ToSwitchInput().Label("Label").Description("Description").Disabled()
             | trueState.ToSwitchInput().Label("Label").Description("Description").Invalid("Invalid")
             | new Box("Not Implemented")

             | null!
             | trueState.ToSwitchInput().Label("Label")
             | falseState.ToSwitchInput().Label("Label")
             | trueState.ToSwitchInput().Label("Label").Disabled()
             | trueState.ToSwitchInput().Label("Label").Invalid("Invalid")
             | new Box("Not Implemented")

             | Text.InlineCode("BoolInputs.Toggle")
             | trueState.ToToggleInput(Icons.Magnet).Label("Label").Description("Description")
             | falseState.ToToggleInput(Icons.Magnet).Label("Label").Description("Description")
             | trueState.ToToggleInput(Icons.Magnet).Label("Label").Description("Description").Disabled()
             | trueState.ToToggleInput(Icons.Magnet).Label("Label").Description("Description").Invalid("Invalid")
             | new Box("Not Implemented")

             | null!
             | trueState.ToToggleInput(Icons.Baby).Label("Label")
             | falseState.ToToggleInput(Icons.Baby).Label("Label")
             | trueState.ToToggleInput(Icons.Baby).Label("Label").Disabled()
             | trueState.ToToggleInput(Icons.Baby).Label("Label").Invalid("Invalid")
             | new Box("Not Implemented")
          ;

      var dataBinding = CreateNumericTypeTests();

      return Layout.Vertical()
             | Text.H1("BoolInput")
             | Text.H2("Variants")
             | variants
             | Text.H2("Data Binding")
             | dataBinding
             ;
   }

   private object CreateNumericTypeTests()
   {
      var numericTypes = new (string TypeName, object NonNullableState, object NullableState)[]
      {
         // Signed integer types
         ("sbyte", UseState((sbyte)0), UseState((sbyte?)null)),
         ("short", UseState((short)0), UseState((short?)null)),
         ("int", UseState(0), UseState((int?)null)),
         ("long", UseState((long)0), UseState((long?)null)),
         ("Int128", UseState((Int128)0), UseState((Int128?)null)),
         ("IntPtr", UseState((IntPtr)0), UseState((IntPtr?)null)),
         
         // Unsigned integer types
         ("byte", UseState((byte)0), UseState((byte?)null)),
         ("ushort", UseState((ushort)0), UseState((ushort?)null)),
         ("uint", UseState((uint)0), UseState((uint?)null)),
         ("ulong", UseState((ulong)0), UseState((ulong?)null)),
         ("UInt128", UseState((UInt128)0), UseState((UInt128?)null)),
         ("UIntPtr", UseState((UIntPtr)0), UseState((UIntPtr?)null)),
         
         // Floating-point types
         ("Half", UseState((Half)0), UseState((Half?)null)),
         ("float", UseState(0.0f), UseState((float?)null)),
         ("double", UseState(0.0), UseState((double?)null)),
         ("decimal", UseState((decimal)0), UseState((decimal?)null)),
         
         // Boolean types
         ("bool", UseState(false), UseState((bool?)false))
      };

      var gridItems = new List<object>
      {
         Text.InlineCode("Type"),
         Text.InlineCode("Non-Nullable"),
         Text.InlineCode("State"),
         Text.InlineCode("Type"),
         Text.InlineCode("Nullable"),
         Text.InlineCode("State")
      };

      foreach (var (typeName, nonNullableState, nullableState) in numericTypes)
      {
         // Non-nullable columns (first 3)
         gridItems.Add(Text.InlineCode(typeName));
         gridItems.Add(CreateBoolInputVariants(nonNullableState));
         gridItems.Add(nonNullableState);

         // Nullable columns (next 3)
         gridItems.Add(Text.InlineCode($"{typeName}?"));
         gridItems.Add(CreateBoolInputVariants(nullableState));
         gridItems.Add(nullableState);
      }

      return Layout.Grid().Columns(6) | gridItems.ToArray();
   }

   private static object CreateBoolInputVariants(object state)
   {
      var anyState = state as IAnyState;
      if (anyState == null)
         return Text.Block("Not an IAnyState");

      return Layout.Vertical()
             | anyState.ToBoolInput()
             | anyState.ToBoolInput().Variant(BoolInputs.Switch)
             | anyState.ToBoolInput().Variant(BoolInputs.Toggle).Icon(Icons.Star);
   }
}