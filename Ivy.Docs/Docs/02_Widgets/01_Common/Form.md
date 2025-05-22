# Form

Forms can be scaffolded from a state object using `ToForm()` and further customized via the fluent `FormBuilder` API.

## Basic Usage

```csharp
auto var model = UseState(() => new UserModel());
return model.ToForm();
```

Forms can also be presented in a sheet or dialog:

```csharp
auto form = model.ToForm();
form.ToDialog(isOpen, "User Information");
```

<WidgetDocs Type="Ivy.Form" ExtensionTypes="Ivy.Forms.FormExtensions;Ivy.Forms.UseFormExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Forms/Form.cs"/>
