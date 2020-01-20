# LdapHelper.Core
How to Use Active Directory (via LDAP) in .NetStandard 2.x+ ASP.Net Core 3.x+ (using the C# Novell LDAP library)

> I Read following project and modified, fix and add some features to that and also upgrade that for work in .net standard 2.x+ and aspnetcore 3.x+
> https://github.com/brechtb86/dotnet/tree/master/brechtbaekelandt.ldap
> thanks to [brechtb86](https://github.com/brechtb86)

## How to Use
> you can fix data in **appsettings.ldap.json** and add them to your application config instead of hard coding ldap settings

### Ldap Auth

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

---

### Ldap Auth + *AspNetIdentity*

1. Install the nuget package
```
Install-Package MicroLib.LdapHelper.Core.Identity
```
2. Config DI Container
```
    services.AddLdapIdentityHelperServices(new LdapSettings
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
    
    services.AddIdentity<LdapIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<LdapIdentityDbContext>()
                .AddUserManager<LdapUserManager>()
                .AddSignInManager<LdapSignInManager>()
                .AddDefaultTokenProviders();
```


