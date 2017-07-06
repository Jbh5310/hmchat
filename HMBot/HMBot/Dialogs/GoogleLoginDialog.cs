using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace HMBot.Dialogs
{
    [Serializable]
    public class GoogleLoginDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Type = "message";
            message.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = $"{System.Configuration.ConfigurationManager.AppSettings["AppWebSite"]}/Home/Login?userid={HttpUtility.UrlEncode(activity.From.Id)}",
                Type = "signin",
                Title = "Authentication Required"
            };

            cardButtons.Add(plButton);

            SigninCard plCard = new SigninCard("Please login to Office 365", new List<CardAction>() { plButton });
            Attachment plAttachment = plCard.ToAttachment();
            message.Attachments.Add(plAttachment);

            await context.PostAsync(message);

            context.Done("로그인성공");
        }
    }
}