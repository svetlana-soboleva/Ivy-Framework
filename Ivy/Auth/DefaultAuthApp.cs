using Ivy.Apps;
using Ivy.Client;
using Ivy.Core;
using Ivy.Helpers;
using Ivy.Shared;

namespace Ivy.Auth;

[App()]
public class DefaultAuthApp : ViewBase
{
    public override object Build()
    {
        var auth = this.UseService<IAuthService>();

        var options = auth.GetAuthOptions();
        
        var renderedOptions = new List<object>();
        
        if(options.Any(e => e.Flow == AuthFlow.EmailPassword))
        {
            var emailOption = options.First(e => e.Flow == AuthFlow.EmailPassword);
            renderedOptions.Add(new PasswordEmailFlowView(emailOption));
        }

        if (options.Any(e => e.Flow == AuthFlow.OAuth))
        {
            var oAuthOptions = options.Where(e => e.Flow == AuthFlow.OAuth).ToList();
            renderedOptions.Add(Layout.Vertical() | oAuthOptions.Select(e => new OAuthFlowView(e)));
        }
        
        return
            Layout.Horizontal().Align(Align.Center).Height(Size.Screen())
            | (new Card().Width(100).Title("Login")
               | renderedOptions.SelectMany(x => new[] { x, new Separator("OR") }).Take(renderedOptions.Count * 2 - 1)
                   .ToArray());
    }
}

public class PasswordEmailFlowView(AuthOption option) : ViewBase
{
    public override object Build()
    {
        var user = this.UseState<string>("");
        var password = this.UseState<string>("");
        var result = this.UseState<string?>();
        var loading = this.UseState<bool>();
        var auth = this.UseService<IAuthService>();
        var client = this.UseService<IClientProvider>();
        
        var login = new Action(async void () =>
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

        return Layout.Vertical()
         | Text.Label("User:")
         | user.ToTextInput().Disabled(loading.Value)
         | Text.Label("Password:")
         | password.ToPasswordInput().Disabled(loading.Value)
         | new Button("Login").Width(Size.Full()).HandleClick(login).Loading(loading.Value).Disabled(loading.Value)
         | result
         ;
    }
}


public class OAuthFlowView(AuthOption option) : ViewBase
{
    public override object? Build()
    {
        return new Button(option.Name).Icon(option.Icon);
    }
}


