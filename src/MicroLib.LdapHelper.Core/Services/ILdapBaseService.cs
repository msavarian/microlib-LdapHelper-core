using MicroLib.LdapHelper.Core.Models;
using System.Collections.Generic;

namespace MicroLib.LdapHelper.Core.Services
{
    public interface ILdapBaseService<TLdapUser>
    {
        ICollection<LdapEntry> GetGroups(string groupName, bool getChildGroups = false);

        ICollection<TLdapUser> GetUsersInGroup(string groupName);

        ICollection<TLdapUser> GetUsersInGroups(ICollection<LdapEntry> groups = null);

        ICollection<TLdapUser> GetUsersByEmailAddress(string emailAddress);

        ICollection<TLdapUser> GetAllUsers();

        TLdapUser GetAdministrator();

        TLdapUser GetUserByUserName(string userName);

        void AddUser(TLdapUser user, string password);

        void DeleteUser(string distinguishedName);

        /// <summary>
        /// </summary>
        /// <param name="samAccountName"> I Change distinguishedName to samAccountName</param>
        /// <param name="password"></param>
        /// <returns></returns>
        LdapBindStatusEnum Authenticate(string samAccountName, string password);
    }
}
