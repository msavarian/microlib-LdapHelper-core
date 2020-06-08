using MicroLib.LdapHelper.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroLib.LdapHelper.Core.Identity.Identity.Models
{
    // Add profile data for application users by adding properties to the LdapUser class
    public class LdapIdentityUser : IdentityUser, ILdapEntry
    {
        [NotMapped]
        public string ObjectSid { get; set; }

        [NotMapped]
        public string ObjectGuid { get; set; }

        [NotMapped]
        public string ObjectCategory { get; set; }

        [NotMapped]
        public string ObjectClass { get; set; }

        [NotMapped]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "You must enter your password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        public string Name { get; set; }
        
        [NotMapped]
        public string CommonName { get; set; }

        [NotMapped]
        public string DistinguishedName { get; set; }

        [NotMapped]
        public string SamAccountName { get; set; }

        [NotMapped]
        public int SamAccountType { get; set; }

        [NotMapped]
        public string[] MemberOf { get; set; }
        
        [NotMapped]
        public bool IsDomainAdmin { get; set; }

        [NotMapped]
        public bool MustChangePasswordOnNextLogon { get; set; }

        [NotMapped]
        public string UserPrincipalName { get; set; }

        [NotMapped]
        public string DisplayName { get; set; }
        
        [NotMapped]
        [Required(ErrorMessage = "You must enter your first name!")]
        public string FirstName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "You must enter your last name!")]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{this.FirstName} {this.LastName}";

        [NotMapped]
        [Required(ErrorMessage = "You must enter your email address!")]
        [EmailAddress(ErrorMessage = "You must enter a valid email address.")]
        public string EmailAddress { get; set; }

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public string Phone { get; set; }

        [NotMapped]
        public LdapAddress Address { get; set; }

        public override string SecurityStamp => Guid.NewGuid().ToString("D");

        public override string UserName
        {
            get => this.Name;
            set => this.Name = value;
        }

        public override string NormalizedUserName => this.UserName;

        public override string NormalizedEmail => this.EmailAddress;

        /// <summary>
        /// I removed Guid.NewGuid().ToString("D"); and let the property to set by the real userId in identityDb
        /// beacuse if this id set randomly, we could not get claims from Identitydb by UserId 
        /// </summary>
        public override string Id { get; set; } //=> Guid.NewGuid().ToString("D");

        public override string Email => this.EmailAddress;
    }
    
}
