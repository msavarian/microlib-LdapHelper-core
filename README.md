# MicroLib.LdapHelper.Core
How to Use Active Directory (via LDAP) in .NetStandard 2.x ASP.Net Core 3.x (using the C# Novell LDAP library)

> I tried to rewrite (re-implementing) [this source code](https://github.com/brechtb86/dotnet/tree/master/brechtbaekelandt.ldap), thanks to [brechtb86](https://github.com/brechtb86).
> I add new features, upgrade that to .net standard 2.x and aspnetcore 3.x and also working on integration with identity-core

## Contents
- [How to Use](#How-to-Use)
  - [Configuration](#configuration)
  - [Use **Ldap** to authenticate the user](#use-ldap-to-authenticate-the-user)
  - [Use **Ldap and Identity** to hybrid authenticate the user](#Use-**Ldap-and-Identity**-to-hybrid-authenticate-the-user)

## How to Use

### Configuration
> first things first, you can correct and fill data in **appsettings.ldap.json** and add them to your **appsettings.json** instead of hard coding ldap settings
```
{
  "LdapSettings": {
    "ServerName": "servername.domain.com",
    "ServerPort": 389,
    "UseSSL": false,
    "Credentials": {
      "DomainUserName": "domain.com\\username",
      "Password": "xxxxxx"
    },
    "UpnSuffixes": "domain1.com,domain2.com",
    "SearchBase": "DC=domain,DC=com",
    "ContainerName": "DC=domain,DC=com",
    "DomainName": "domain.com",
    "DomainDistinguishedName": "DC=domain,DC=com"
  }
}
```
> you can bind above settings to the **MicroLib.LdapHelper.Core.Settings.LdapSettings.cs** class at startup.cs


### Use **Ldap** to authenticate the user

1. Install the nuget package
```
Install-Package MicroLib.LdapHelper.Core
```
2. Config DI Container
```
    services.AddLdapHelperServices(new LdapSettings
    {
        ServerName = "server.domain.com",
        ServerPort = 389,
        UseSSL = false,
        UpnSuffixes = new string[] { "domain1.com", "domain2.com" },
        
        Credentials = new LdapCredentials
        {
            DomainUserName = "user@domain.com",
            Password = "password"
        },
        
        SearchBase = "DC=domain,DC=com",
        ContainerName = "DC=domain,DC=com",
        DomainName = "domain.com",
        DomainDistinguishedName = "DC=domain,DC=com"
    });
    
    // in console apps
    services.AddOptions();
```
> Inject **ILdapBaseService<LdapUser>** in the class(login controller) and work with ldap directly

---

### Use **Ldap and Identity** to hybrid authenticate the user

 - Install the nuget package
```
Install-Package MicroLib.LdapHelper.Core.Identity

// also you need to install following nuget packages for use Identity
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore Version="3.1.0"
Install-Package Microsoft.AspNetCore.Identity.UI Version="3.1.0"
Install-Package Microsoft.EntityFrameworkCore.SqlServer Version="3.1.0"
Install-Package Microsoft.EntityFrameworkCore.Tools Version="3.1.0"
```

 - Config DI Container
```
    services.**AddLdapIdentityHelperServices(new LdapSettings
    {
        ServerName = "server.domain.com",
        ServerPort = 389,
        UseSSL = false,
        UpnSuffixes =new string[] { "domain1.com","domain2.com"},

        Credentials = new LdapCredentials
        {
            DomainUserName = "user@domain.com",
            Password = "password"
        },

        SearchBase = "DC=domain,DC=com",
        ContainerName = "DC=domain,DC=com",
        DomainName = "domain.com",
        DomainDistinguishedName = "DC=domain,DC=com"
    });
```

> Inject **ILdapBaseService<LdapIdentityUser>** in the class(login controller) and work with ldap directly instead of UserManager and Identity

---

### In this library there are two different ways for Hybrid-Authentication (Ldap + Identity Core)

1. the first way that I named it "LdapBase", and it means all users managment functionality will be by ActiveDirectory. in this way we use IdentityCore just for Signing-in (security) and working with UserManager (but with the active directory repository)
``` 
    services.AddIdentity<LdapIdentityUser, IdentityRole>(options =>
                {
                ///
                })
                .AddEntityFrameworkStores<LdapIdentityDbContext>()
                .AddUserManager<LdapBaseUserManager>() 
                .AddSignInManager<LdapBaseSignInManager>() 
                .AddDefaultTokenProviders();
```

2. the second way that I named it "IdentityBase", and you will able to work with IdentityCore as before, but the usernames in AspNetUsers table should be same with Username in ActiveDirectory and also password in IdentityDb will not matter yet (the users passwords checks from ldap)
``` 
    services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                ///
                })
                .AddEntityFrameworkStores<LdapIdentityDbContext>()
                .AddUserManager<IdentityBaseUserManager>() 
                .AddSignInManager<IdentityBaseSignInManager>() 
                .AddDefaultTokenProviders();
```



