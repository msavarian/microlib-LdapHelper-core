using MicroLib.LdapHelper.Core.Identity.Identity.Models;
using MicroLib.LdapHelper.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MicroLib.LdapHelper.Core.Identity.Services.IdentityBase
{
    public class IdentityBaseSigninManager : SignInManager<IdentityUser>
    {
        private readonly ILdapBaseService<LdapIdentityUser> _ldapService;

        public IdentityBaseSigninManager(
            IdentityBaseUserManager userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<IdentityBaseSigninManager> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<IdentityUser> userConfirmation,
            ILdapBaseService<LdapIdentityUser> ldapService
            ) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, userConfirmation)
        {
            _ldapService = ldapService;
        }

        /// <summary>f
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