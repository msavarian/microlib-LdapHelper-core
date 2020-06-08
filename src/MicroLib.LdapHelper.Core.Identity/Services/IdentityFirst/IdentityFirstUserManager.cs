using MicroLib.LdapHelper.Core.Identity.Identity.Models;
using MicroLib.LdapHelper.Core.Identity.Services.LdapFirst;
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

namespace MicroLib.LdapHelper.Core.Identity.Services.IdentityFirst
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
        /// <param name = "user"></ param >
        /// < param name="password"></param>
        /// <returns></returns>
        public override async Task<bool> CheckPasswordAsync(LdapIdentityUser user, string password)
        {
            return _ldapService.Authenticate(user.SamAccountName, password) == LdapBindStatusEnum.Suceesful_Bind ? true : false;
        }

        /// <summary>
        /// if user find in both IdentityDb and Ldap, then returns LdapIdentityUser
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override Task<LdapIdentityUser> FindByNameAsync(string userName)
        {
            var ldapuser = _ldapService.GetUserByUserName(userName);
            var localuser = base.FindByNameAsync(userName).Result;

            // i check if the user are in both Identitydb and ldap, and also password just check with ldap
            if (ldapuser != null && localuser != null)
            {
                // we should set ldapUserId with IdentityDbId till userManager Can fetch userclaims
                ldapuser.Id = localuser.Id;
                return Task.FromResult(ldapuser);
            }
            return Task.FromResult<LdapIdentityUser>(null);
        }

        public override IQueryable<LdapIdentityUser> Users
        {
            get
            {
                return (from u in base.Users.ToList()
                        join l in _ldapService.GetAllUsers().ToList()
                        on u.UserName equals l.SamAccountName
                        select new LdapIdentityUser
                        {
                            Id = u.Id,
                            UserName = u.UserName,

                            DistinguishedName = string.IsNullOrEmpty(l.DistinguishedName) ? "" : l.DistinguishedName,
                            DisplayName = string.IsNullOrEmpty(l.DisplayName) ? "" : l.DisplayName,
                            Name = string.IsNullOrEmpty(l.Name) ? "" : l.Name,
                            FirstName = string.IsNullOrEmpty(l.FirstName) ? "" : l.FirstName,
                            LastName = string.IsNullOrEmpty(l.LastName) ? "" : l.LastName,

                            UserPrincipalName = string.IsNullOrEmpty(l.UserPrincipalName) ? "" : l.UserPrincipalName,
                            SamAccountName = string.IsNullOrEmpty(l.SamAccountName) ? "" : l.SamAccountName,
                            Email = string.IsNullOrEmpty(l.Email) ? "" : l.Email,

                        }).AsQueryable();
            }
        }
    }
}