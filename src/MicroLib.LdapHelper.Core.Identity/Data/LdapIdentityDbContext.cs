using MicroLib.LdapHelper.Core.Identity.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace MicroLib.LdapHelper.Core.Identity.Data
{
    public class LdapIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public LdapIdentityDbContext(DbContextOptions<LdapIdentityDbContext> options)
            : base(options)
        {
        }
    }
}