using Google.Apis.Calendar.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMBot.Web
{
    /// <summary>
    /// Holds the Google API client secrets. Replace the values below with credentials from your developer console
    /// (https://console.developers.google.com).
    /// </summary>
    internal static class GoogleClientSecrets
    {
        public const string ClientId = "412813997078-l75hk4m0knpha1ku8ib57b36er72ehar.apps.googleusercontent.com";
        public const string ClientSecret = "iqiTWmbc5uWvd73AyMUlKIUf";
    }

    internal static class GoogleClaimTypes
    {
        public const string GoogleUserId = "GoogleUserId";
        public const string GoogleAccessToken = "GoogleAccessToken";
        public const string GoogleRefreshToken = "GoogleRefreshToken";
        public const string GoogleTokenIssuedAt = "GoogleTokenIssuedAt";
        public const string GoogleTokenExpiresIn = "GoogleTokenExpiresIn";
    }

    internal static class GoogleRequestedScopes
    {
        public static string[] Scopes
        {
            get
            {
                return new[] {
                    "openid",
                    "email",
                    CalendarService.Scope.Calendar,
                };
            }
        }
    }
}