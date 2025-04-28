using Ivy.Apps;
using Ivy.Client;
using Ivy.Core;
using Ivy.Helpers;
using Ivy.Shared;

namespace Ivy.Auth;

[App()]
public class DefaultAuthApp() : ViewBase
{
    public override object? Build()
    {
        var user = this.UseState<string>("");
        var password = this.UseState<string>("");
        var auth = this.UseService<IAuthService>();
        var client = this.UseService<IClientProvider>();
        var loading = this.UseState<bool>(false);
        var result = this.UseState<string?>((string?)null!);

        var click = new Action(async void () =>
        {
            try
            {
                loading.Set(true);
                var token = await auth.LoginAsync(user.Value, password.Value);
                if (token != null)
                {
                    client.SetJwt(token);
                }
            }
            catch (Exception e)
            {
                result.Set(e.Message);
            }
            finally
            {
                loading.Set(false);
            }
        });
        
        return 
            Layout.Horizontal().Align(Align.Center).Height(Size.Screen()) 
            | (new Card().Width(100).Title("Login")
               | (Layout.Vertical()
                  | Text.Label("User:")
                  | user.ToTextInput().Disabled(loading.Value)
                  | Text.Label("Password:")
                  | password.ToPasswordInput().Disabled(loading.Value)
                  | new Button("Login").Width(Size.Full()).HandleClick(click).Loading(loading.Value).Disabled(loading.Value)
                  | result
                   // | new Separator("OR")
                   // | new Button("Login with Google").Width(Size.Full()).Variant(ButtonVariant.Secondary)
               ));
    }
}