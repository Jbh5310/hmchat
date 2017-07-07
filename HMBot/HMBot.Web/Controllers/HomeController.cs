using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using HMBot.Web.Models;
using System.Configuration;
using Microsoft.Bot.Connector;
using HMBot.Models;

namespace HMBot.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string convid, string channelid)
        {
            var convsationid = new ConversatoinIdentityViewModel
            {
                ChannelId = channelid,
                CoversatoinId = convid

            };
            return View(convsationid);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string convid, string channelid)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Home", new { convid = convid, channelid = channelid }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string convid, string channelid)
        {
            var loginInfo = await HttpContext.GetOwinContext().Authentication.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            var identity = await HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

            var userId = identity.FindFirstValue(GoogleClaimTypes.GoogleUserId);

            var token = new GoogleToken()
            {
                AccessToken = identity.FindFirstValue(GoogleClaimTypes.GoogleAccessToken),
                RefreshToken = identity.FindFirstValue(GoogleClaimTypes.GoogleRefreshToken),
                Issued = DateTime.FromBinary(long.Parse(identity.FindFirstValue(GoogleClaimTypes.GoogleTokenIssuedAt))),
                ExpiresInSeconds = long.Parse(identity.FindFirstValue(GoogleClaimTypes.GoogleTokenExpiresIn)),
                ClientID = GoogleClientSecrets.ClientId,
                ClientSecret = GoogleClientSecrets.ClientSecret,
                UserID = userId
            };

            // 여기서 Microsoft Bot Framework의 State에 토큰 정보를 저장. 
            var botCred = new MicrosoftAppCredentials(ConfigurationManager.AppSettings["MicrosoftAppId"], ConfigurationManager.AppSettings["MicrosoftAppPassword"]);
            var stateClient = new StateClient(botCred);
            BotState botState = new BotState(stateClient);
            BotData botData = new BotData(eTag: "*");
            botData.SetProperty<GoogleToken>("GoogleTokenInfo", token);

            await stateClient.BotState.SetConversationDataAsync(channelid, convid, botData);


            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await HttpContext.GetOwinContext().Authentication.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }


                // 여기서 ... 

            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        private const string XsrfKey = "XsrfId";

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                //// this line fixed the problem with returing null
                //context.RequestContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;

                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}