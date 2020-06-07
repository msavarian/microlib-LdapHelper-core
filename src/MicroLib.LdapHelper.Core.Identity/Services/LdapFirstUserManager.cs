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
    public class LdapFirstUserManager : UserManager<LdapIdentityUser>
    {
        private readonly ILdapBaseService<LdapIdentityUser> _ldapService;

        public LdapFirstUserManager(
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


        public LdapIdentityUser GetAdministrator()
        {
            return _ldapService.GetAdministrator();
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

        public override Task<bool> HasPasswordAsync(LdapIdentityUser user)
        {
            return Task.FromResult(true);
        }

        public override Task<LdapIdentityUser> FindByIdAsync(string userId)
        {
            return FindByNameAsync(userId);
        }

        public override Task<LdapIdentityUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(_ldapService.GetUserByUserName(userName));
        }

        public override async Task<IdentityResult> CreateAsync(LdapIdentityUser user, string password)
        {
            try
            {
                _ldapService.AddUser(user, password);
            }
            catch (Exception e)
            {
                return await Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "LdapUserCreateFailed", Description = e.Message ?? "The user could not be created." }));
            }

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteUserAsync(string distinguishedName)
        {
            try
            {
                _ldapService.DeleteUser(distinguishedName);
            }
            catch (Exception e)
            {
                return await Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "LdapUserDeleteFailed", Description = e.Message ?? "The user could not be deleted." }));
            }

            return await Task.FromResult(IdentityResult.Success);
        }

        public override Task<string> GetEmailAsync(LdapIdentityUser user)
        {
            return base.GetEmailAsync(user);
        }

        public override Task<string> GetUserIdAsync(LdapIdentityUser user)
        {
            return base.GetUserIdAsync(user);
        }

        public override Task<string> GetUserNameAsync(LdapIdentityUser user)
        {
            return base.GetUserNameAsync(user);
        }

        public override Task<string> GetPhoneNumberAsync(LdapIdentityUser user)
        {
            return base.GetPhoneNumberAsync(user);
        }

        public override IQueryable<LdapIdentityUser> Users => _ldapService.GetAllUsers().AsQueryable();
    }
}
