using MicroLib.LdapHelper.Core.Identity.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MicroLib.LdapHelper.Core.Identity.Identity
{
    public class LdapFirstSigninManager : SignInManager<LdapIdentityUser>
    {
        public LdapFirstSigninManager(
            LdapFirstUserManager userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<LdapIdentityUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<LdapFirstSigninManager> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<LdapIdentityUser> userConfirmation
            ) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, userConfirmation)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">userName souldn't has @domain.com</param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <param name="lockOutOnFailure"></param>
        /// <returns></returns>
        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockOutOnFailure)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            return await PasswordSignInAsync(user, password, rememberMe, lockOutOnFailure);
        }
    }
}
