using MicroLib.LdapHelper.Core.Identity.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MicroLib.LdapHelper.Core.Identity.Identity
{
    public class LdapSignInManager : SignInManager<LdapIdentityUser>
    {
        public LdapSignInManager(
            LdapUserManager userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<LdapIdentityUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<LdapSignInManager> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<LdapIdentityUser> userConfirmation
            ) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, userConfirmation)
        {
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockOutOnFailure)
        {
            // userName souldn't has @domain.com
            var user = await UserManager.FindByNameAsync(userName);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await PasswordSignInAsync(user, password, rememberMe, lockOutOnFailure);
        }
    }
}
