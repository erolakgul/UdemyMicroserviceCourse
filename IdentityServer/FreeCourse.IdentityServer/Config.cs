﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
                   new ApiResource[]
                   {
                     //new IdentityResources.OpenId(),
                     //new IdentityResources.Profile(),
                     new ApiResource("credential_catalog")  // bu aud için bir veya daha fazla ApiScopes tanımlanır
                     {
                        Scopes = { "catalog_fullpermission" }
                     },
                     new ApiResource("credential_photo_stock")
                     {
                        Scopes = { "photo_stock_fullpermission" }
                     },
                      new ApiResource("resource_basket")
                     {
                        Scopes = { "basket_fullpermission" }
                     },
                      new ApiResource("resource_discount")
                     {
                        Scopes = { "discount_fullpermission" , "discount_read" , "discount_write" }   // read ve write ekstra tanımlandı
                     },
                      new ApiResource("resource_order")
                     {
                        Scopes = { "order_fullpermission" }
                     },
                      new ApiResource("resource_payment")
                     {
                        Scopes = { "payment_fullpermission" }
                     },
                      new ApiResource("resource_gateway")
                     {
                        Scopes = { "gateway_fullpermission" }
                     },
                      new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
                   };

        // üyelik ile ilgili işlemler
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                      //new IdentityResources.OpenId(),
                      //new IdentityResources.Profile(),
                      new IdentityResources.Email(),  // kullanıcının email ine erişebilsin
                      new IdentityResources.OpenId(), // // bu parametreyi mutlaka alması lazım, openid protokolünün zorunlu kıldığı alan
                      new IdentityResources.Profile(), // kullanıcının profil bilgilerine erişebilsin
                      new IdentityResource(){
                                             Name = "roles",DisplayName ="Roles",
                                             Description = "User Roles", UserClaims = new [] {"role"} // role bilgisi de bu role isim claim e maplensin
                                            }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                //new ApiScope("scope1"),
                //new ApiScope("scope2"),
                new ApiScope("catalog_fullpermission","Full Access for Catalog Api"),

                new ApiScope("photo_stock_fullpermission","Full Access for Photo Api"),

                new ApiScope("basket_fullpermission","Full Access for Basket Api"),

                new ApiScope("discount_fullpermission","Full Access for Discount Api"),

                new ApiScope("order_fullpermission","Full Access for Order Api"),

                new ApiScope("payment_fullpermission","Full Access for Payment Api"),

                new ApiScope("gateway_fullpermission","Full Access for Gateway Api"),

            #region örnek scope izin tanımlamaları
		        new ApiScope("discount_read","read Access for Discount Api"),

                new ApiScope("discount_write","write Access for Discount Api"), 
         	#endregion

                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //ClientCredentials
                new Client()
                {
                    ClientName = "Asp.NetCore MVC", // merkezi bir üyelik sistemi kullansaydık, yani identityserver ın arayüzünü kullanarak token alınsaydı, her bir client için bir name tanımlyacaktık, fakat biz sadece endpointlerini kullanacağız
                    ClientId = "WebMvcClient",
                    ClientSecrets = {new Secret("secretKey".Sha256())}, //secretKey bilgisini veritabanında da tutabilirdik
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // token üretmede hangi yöntemi kullanacaksak onu belirtiyoruz
                    AllowedScopes = { "catalog_fullpermission", "photo_stock_fullpermission" 
                                    ,"gateway_fullpermission"
                                    , IdentityServerConstants.LocalApi.ScopeName }  // hangi scope lara izin verilecekse onlar tanımlanır

                },
        
              //ResourceOwnerPassword
               new Client()
                {
                    ClientName = "Asp.NetCore MVC", // merkezi bir üyelik sistemi kullansaydık, yani identityserver ın arayüzünü kullanarak token alınsaydı, her bir client için bir name tanımlyacaktık, fakat biz sadece endpointlerini kullanacağız
                    ClientId = "WebMvcClientForUser",
                    ClientSecrets = {new Secret("secretKey".Sha256())}, //secretKey bilgisini veritabanında da tutabilirdik
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,// refresh token a izin verir // token üretmede hangi yöntemi kullanacaksak onu belirtiyoruz
                    AllowOfflineAccess = true,
                    AllowedScopes = { "gateway_fullpermission" ,"basket_fullpermission","discount_fullpermission","order_fullpermission",
                                       "payment_fullpermission",
                                       IdentityServerConstants.StandardScopes.Email, 
                                       IdentityServerConstants.StandardScopes.OpenId, 
                                       IdentityServerConstants.StandardScopes.Profile,
                                       IdentityServerConstants.LocalApi.ScopeName,
                                       IdentityServerConstants.StandardScopes.OfflineAccess , // refresh token
                                     // kullanıcı ofline olsa bile elindeki refresh token ile tekrar bir istekte bulunup token alır
                                     // eğer refresh token yoksa, kullanıcıdan sürekli kullanıcı adı ve şifresiyle login olmasını bekleriz
                                     // access token ömrünü 1 saat,refresh token(cookie de tutulan) ömrünü ise ör 60 gün yapabiliriz
                                     "roles" // identityresources ta tanımlanan key
                                    }  // identityresource daki claim lerden hangilerine izin verilecekse onlar tanımlanır
                                      ,
                    AccessTokenLifetime = 1*60*60 // access token süresini 1 saat atadık
                   ,RefreshTokenExpiration = TokenExpiration.Absolute // 61.ci gün ömrü dolsun diye
                   ,AbsoluteRefreshTokenLifetime = (int) (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds // 60 gün
                   ,RefreshTokenUsage = TokenUsage.ReUse
                },

               // discount api için read client ları ÖRNEK
               //new Client()
               // {
               //     ClientName = "Asp.NetCore MVC", 
               //     ClientId = "WebMvcClientForUser",
               //     ClientSecrets = {new Secret("secretKey".Sha256())},
               //     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
               //     AllowOfflineAccess = true,
               //     AllowedScopes = {  "basket_fullpermission","discount_read",
               //                        IdentityServerConstants.StandardScopes.Email,
               //                        IdentityServerConstants.StandardScopes.OpenId,
               //                        IdentityServerConstants.StandardScopes.Profile,
               //                        IdentityServerConstants.LocalApi.ScopeName,
               //                        IdentityServerConstants.StandardScopes.OfflineAccess , 
               //                      "roles"
               //                     } 
               //                       ,
               //     AccessTokenLifetime = 1*60*60 
               //    ,RefreshTokenExpiration = TokenExpiration.Absolute
               //    ,AbsoluteRefreshTokenLifetime = (int) (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds 
               //    ,RefreshTokenUsage = TokenUsage.ReUse
               // },

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