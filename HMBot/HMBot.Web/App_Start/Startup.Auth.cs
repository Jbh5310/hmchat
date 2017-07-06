using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace HMBot.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit
        // http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables logging in with the Google login provider.
            var google = new GoogleOAuth2AuthenticationOptions()
            {
                AccessType = "offline",     // Request a refresh token.
                ClientId = GoogleClientSecrets.ClientId,
                ClientSecret = GoogleClientSecrets.ClientSecret,
                Provider = new GoogleOAuth2AuthenticationProvider() { }
            };

            foreach (var scope in GoogleRequestedScopes.Scopes)
            {
                google.Scope.Add(scope);
            }

            app.UseGoogleAuthentication(google);
        }
    }
}