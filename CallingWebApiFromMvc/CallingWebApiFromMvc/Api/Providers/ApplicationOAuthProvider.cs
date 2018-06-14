namespace Levelnis.Learning.CallingWebApiFromMvc.Api.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OAuth;
    using System.Linq;
    using System.Data.Entity;

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            #region Login: Code will be change.

            //var userNameEncpt = "/g+LmvXNQ35Ct8yFyeUYrRz6mF1MsORkfWN5bTVz1UA=";
            //var userList = await userManager.Users.Where(x => x.Email == userNameEncpt).ToListAsync();
            //if (userList.Any() == false)
            //{
            //    context.SetError("invalid_grant", "Sorry, OnTrack Health doesn't recognize that email.");
            //    return;
            //}
            //var isActiveUserList = userList.Any(x => x.IsDeleted == false);
            //if (isActiveUserList == false)
            //{
            //    context.SetError("invalid_grant", "Your account is not active anymore.");
            //    return;
            //}

            //var userName = userList.FirstOrDefault().Id;

            #endregion

            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
            var cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);
            var properties = CreateProperties(user.UserName);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        private static AuthenticationProperties CreateProperties(string userName)
        {
            var data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}