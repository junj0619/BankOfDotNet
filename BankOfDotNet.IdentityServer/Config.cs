using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankOfDotNet.IdentityServer
{
    public class Config
    {
        //Resource owner password flow users
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser {
                    SubjectId = "1",
                    Username = "John",
                    Password = "password"
                },
                new TestUser {
                    SubjectId = "2",
                    Username = "Bob",
                    Password = "password"
                }
            };
        }

        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource> {
                new ApiResource("bankOfDotNetApi", "Customer API for BankOfDotNet")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            
            return new List<Client>
            {
                //Client-Credential based grant type (machine to manchie, trusted 1st party sources, server2server)
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = { GrantType.ClientCredentials },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "bankOfDotNetApi" }
                },

                //Resource Owner Password (user involved, trusted 1st party apps(spa, js, native 1st party))
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"bankOfDotNetApi"}
                }
            };

            
            //Authorization Code (google, facebook, etc user involved, web app(server app))
            //Implicit (web applications, user, server side web apps) 
            //Hybrid = Implicit + Authorization Code get access token and Auth Code together good for native apps, server side web apps, mobile apps

            //OATH 2.0 and OIDC (open id connect) protocols
        }
    }
}
