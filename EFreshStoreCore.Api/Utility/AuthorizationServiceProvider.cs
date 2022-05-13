using System.Security.Claims;
using System.Threading.Tasks;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;
using Microsoft.Owin.Security.OAuth;

namespace EFreshStoreCore.Api.Utility
{
    public class AuthorizationServiceProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (LoginRepository loginRepo = new LoginRepository())
            {
                var user = loginRepo.ValidateUser(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    return;
                }
                //UserType aType = _usertypeManager.GetFirstOrDefault(u=> u.Id.Equals(user.UserTypeId));
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, user.UserType.Name));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                //identity.AddClaim(new Claim("Email", user.Username));
                context.Validated(identity);
            }
        }
    }
}
