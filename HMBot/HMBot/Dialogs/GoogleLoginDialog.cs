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
            var conversationid = message.Conversation.Id;
            var channelid = message.ChannelId;

            if (channelid == "emulator") channelid = "skype";

            message.Type = "message";
            message.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = $"{System.Configuration.ConfigurationManager.AppSettings["LoginURL"]}?convid={HttpUtility.UrlEncode(conversationid)}&channelid={HttpUtility.UrlEncode(channelid)}",
                Type = "signin",
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