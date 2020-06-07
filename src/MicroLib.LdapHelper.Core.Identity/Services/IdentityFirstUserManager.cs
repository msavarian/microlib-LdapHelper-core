using MicroLib.LdapHelper.Core.Identity;
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

namespace MicroLib.LdapHelper.Core.Identity.Identity
{
    public class IdentityFirstUserManager : UserManager<LdapIdentityUser>
    {
        private readonly ILdapBaseService<LdapIdentityUser> _ldapService;

        public IdentityFirstUserManager(
            ILdapBaseService<LdapIdentityUser> ldapService,
            IUserStore<LdapIdentityUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<LdapIdentityUser> passwordHasher,
            IEnumerable<IUserValidator<LdapIdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<LdapIdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<LdapFirstUserManager> logger) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _ldapService = ldapService;
        }



        /// <summary>
        /// Checks the given password agains the configured LDAP server.
        /// </summary>
        /// <param name = "user" ></ param >
        /// < param name="password"></param>
        /// <returns></returns>
        public override async Task<bool> CheckPasswordAsync(LdapIdentityUser user, string password)
        {
            return _ldapService.Authenticate(user.SamAccountName, password) == LdapBindStatusEnum.Suceesful_Bind ? true : false;
        }
        public override Task<LdapIdentityUser> FindByNameAsync(string userName)
        {
            var ldapuser = (this._ldapService.GetUserByUserName(userName));
            var localuser = (base.FindByNameAsync(userName)).Result;
            if (ldapuser != null && localuser != null)
                return Task.FromResult(ldapuser);
            return null;
        }
    }
}