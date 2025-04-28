using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ivy.Auth;

public class AuthController() : Controller
{
    [Route("auth/set-jwt")]
    [HttpPatch]
    public IActionResult SetJwt([FromBody] string? jwt)
    {
        if(string.IsNullOrEmpty(jwt))
        {
            HttpContext.Response.Cookies.Delete("jwt");
        }
        else
        {
            HttpContext.Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                //Secure = true, //todo: enable this in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
        }
        return Ok();
    }
}
