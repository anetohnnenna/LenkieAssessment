using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServerAuthenticationService
{
    public class IdentityConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
      new IdentityResource[]
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
      };
        public static IEnumerable<ApiScope> ApiScopes => new[]
       {
             new ApiScope("api.read"),
             new ApiScope("api.write"),
        };

        public static List<TestUser> GetTestUsers(string dd, string fff)
        {
            return new List<TestUser>()
    {
        new TestUser
        {
            SubjectId = "1",
            Username = "demo",
            Password = "demo".Sha256()
        }
    };
        }



        public static IEnumerable<Client> Clients()
       {
            return new List<Client> {

            new Client
            {
                ClientId = "client",
                ClientName = "Client Credentials Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256())},
                AllowedScopes = { "api.read" }
            }
            };
        }

  

        public static IEnumerable<ApiResource> ApiResources => new[]
        {
            new ApiResource("myApi")
            {
                Scopes = new List<string> { "api.read", "api.write"},
                ApiSecrets = new List<Secret> { new Secret("superSecret".Sha256())},
            
            }
        };
            }
}
