// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
                   new ApiResource[]
                   {
                     //new IdentityResources.OpenId(),
                     //new IdentityResources.Profile(),
                     new ApiResource("resource_catalog")  // bu aud için bir veya daha fazla ApiScopes tanımlanır
                     {
                        Scopes = { "catalog_fullpermission" }
                     },
                     new ApiResource("photostock_catalog")
                     {
                        Scopes = { "photo_stock_fullpermission" }
                     },
                      new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
                   };

        // üyelik ile ilgili işlemler
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                //new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                //new ApiScope("scope1"),
                //new ApiScope("scope2"),
                new ApiScope("catalog_fullpermission","Full Access for Catalog Api"),

                new ApiScope("photo_stock_fullpermission","Full Access for Photo Api"),

                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client()
                {
                    ClientName = "Asp.NetCore MVC", // merkezi bir üyelik sistemi kullansaydık, yani identityserver ın arayüzünü kullanarak token alınsaydı, her bir client için bir name tanımlyacaktık, fakat biz sadece endpointlerini kullanacağız
                    ClientId = "WebMvcClient",
                    ClientSecrets = {new Secret("secretKey".Sha256())}, //secretKey bilgisini veritabanında da tutabilirdik
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // token üretmede hangi yöntemi kullanacaksak onu belirtiyoruz
                    AllowedScopes = { "catalog_fullpermission", "photo_stock_fullpermission" , IdentityServerConstants.LocalApi.ScopeName }  // hangi scope lara izin verilecekse onlar tanımlanır

                },
                //// m2m client credentials flow client
                //new Client
                //{
                //    ClientId = "m2m.client",
                //    ClientName = "Client Credentials Client",

                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                //    AllowedScopes = { "scope1" }
                //},

                //// interactive client using code flow + pkce
                //new Client
                //{
                //    ClientId = "interactive",
                //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RedirectUris = { "https://localhost:44300/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //    AllowOfflineAccess = true,
                //    AllowedScopes = { "openid", "profile", "scope2" }
                //},


            };
    }
}