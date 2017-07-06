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
            var conversationid = context.MakeMessage().Conversation.Id;

            var message = context.MakeMessage();
            message.Type = "message";
            message.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = $"{System.Configuration.ConfigurationManager.AppSettings["AppWebSite"]}?id={HttpUtility.UrlEncode(conversationid)}",
                Type = "구글 로그인",
                Title = "일정을 만들려면 로그인이 필요합니다."
            };

            cardButtons.Add(plButton);

            SigninCard plCard = new SigninCard("구글 아이디로 로그인해주세요.", new List<CardAction>() { plButton });
            Attachment plAttachment = plCard.ToAttachment();
            message.Attachments.Add(plAttachment);

            await context.PostAsync(message);

            context.Done("로그인성공");
        }
    }
}