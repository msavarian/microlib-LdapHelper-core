using MicroLib.LdapHelper.Core.Identity.Identity.Models;
using MicroLib.LdapHelper.Core.Models;
using MicroLib.LdapHelper.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MicroLib.LdapHelper.Core.Identity.Services.IdentityBase
{
    public class IdentityBaseUserManager : UserManager<IdentityUser>
    {
        private readonly ILdapBaseService<LdapIdentityUser> _ldapService;

        public IdentityBaseUserManager(
            ILdapBaseService<LdapIdentityUser> ldapService,
            IUserStore<IdentityUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<IdentityBaseUserManager> logger) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _ldapService = ldapService;
        }



        /// <summary>
        /// Checks the given password agains the configured LDAP server.
        /// </summary>
        /// <param name = "user"></ param >
        /// < param name="password"></param>
        /// <returns></returns>
        public override async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return _ldapService.Authenticate(user.UserName, password) == LdapBindStatusEnum.Suceesful_Bind ? true : false;
        }
    }
}