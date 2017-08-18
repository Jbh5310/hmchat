using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using System.Linq;
using Microsoft.Bot.Builder.Luis;
using System.Configuration;
using ICalTest;
using HMBot.Services;
using HMBot.Models;
using HMBot.Services.Model;
#pragma warning disable 649
#pragma warning disable CS1998

namespace HMBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string CESOption = "CES회의실/차량예약";
        private const string ScheduleOption = "일정등록";
        private bool userWelcomed = false;
        private string language;

        //public Task StartAsync(IDialogContext context)
        //{
        //    context.Wait(MessageReceivedAsync);

        //    return Task.CompletedTask;
        //}

        public async Task StartAsync(IDialogContext context)
        {

            context.Wait(this.MessageReceivedAsync);

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //  this.ShowOptions(context);
            var activity = await result;
            switch (activity.Type)
            {
                case ActivityTypes.ConversationUpdate:
                case ActivityTypes.ContactRelationUpdate:
                    if (!userWelcomed)
                    {
                        this.userWelcomed = true;
                        await context.PostAsync(Internal.Responses.WelcomeMessage);
                    }
                    else
                    {
                        await context.PostAsync(Internal.Responses.WelcomeBackMessage);
                    }
                    this.ShowOptions(context);
                    break;
                default:
                    context.Wait(MessageReceivedAsync);
                    break;
            }
        }

        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { CESOption, ScheduleOption }, "안녕하세요HM봇 입니다^^, \r\n서비스를 선택하여주십시오.");
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selectedOption = await result;
                
                switch (selectedOption)
                {
                    case CESOption:
                        await context.PostAsync("준비중인 서비스입니다...");
                        PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { CESOption, ScheduleOption }, "안녕하세요HM봇 입니다^^, \r\n서비스를 선택하여주십시오.");
                        // TODO: state 보고 로그인 되었는지 확인 

                        // 로그인 해야 하면 
                        break;

                    case ScheduleOption:
                        await context.PostAsync("일정등록을 선택하셨습니다.");
                        // TODO: state 보고 로그인 되었는지 확인
                        // 지금은 확인하지 않고 로그인 메시지 띄움 
                        context.Call(new GoogleLoginDialog(), AfterLoginAsync);

                        
                        // TODO: 로그인이 되어 있으면 Google Calendar Dialog 바로 시작 
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task AfterLoginAsync(IDialogContext context, IAwaitable<string> result)
        {
            // 로그인하셨나요? 


            // 로그인 성공 후에는 
            // Google Calendar Dialog 시작 
            var googleCalendarForm = new FormDialog<GoogleCalendarForm>(new GoogleCalendarForm(), GoogleCalendarForm.BuildForm, FormOptions.PromptInStart);
            context.Call(googleCalendarForm, googleCaleadarComplete);
        }

        private async Task googleCaleadarComplete(IDialogContext context, IAwaitable<GoogleCalendarForm> result)
        {


            var re = await result;
         

            // 구글 캘린더 등록

            // state에서 Google 토큰정보 꺼내옴 
            // get state 
            //HMBot.Models.GoogleToken tokenInfo = null;
            //context.ConversationData.TryGetValue("GoogleTokenInfo", out tokenInfo);

            //// test
            var msg = context.MakeMessage();
            var botCred = new MicrosoftAppCredentials(ConfigurationManager.AppSettings["MicrosoftAppId"], ConfigurationManager.AppSettings["MicrosoftAppPassword"]);
            var stateClient = new StateClient(botCred);
            BotState botState = new BotState(stateClient);

            BotData userData = await stateClient.BotState.GetConversationDataAsync((msg.ChannelId == "emulator" ? "skype" : msg.ChannelId), msg.Conversation.Id);
            var token = userData.GetProperty<GoogleToken>("GoogleTokenInfo");

            if (token == null)
            {
                // 다시 로그인 

                context.Call(new GoogleLoginDialog(), AfterLoginAsync);
            }

            // 서비스 호출
            var service = new ICalEvent(token);

            HMEvent newEvent = new HMEvent();
            newEvent.Subject = re.Title;
            newEvent.StartDt = re.DateFrom;
            newEvent.EndDt = re.DateTo;
            newEvent.Location = re.Place;
            newEvent.AddAttendd(re.Attendd);
            newEvent.AddAttendd("jeipilh@gmail.com");
            newEvent.AddAttendd("hanmiitrnd@gmail.com");


           await service.InsertEvent(newEvent);


            //service.InsertEvent();

            await context.PostAsync("일정등록을 완료하였습니다, 일정을 확인해주세요.");

            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { CESOption, ScheduleOption }, "안녕하세요HM봇 입니다^^, \r\n서비스를 선택하여주십시오.");

            // 추가 참석자가 있는지 물어보고 
            // 있다면 GoogleAttendeeDialog 로 이동 

            // 없다면 끝. 다시 처음으로 돌아가서 메뉴 
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"오류가 발생했습니다. 빠른 시일안에 개선하겠습니다.: {ex.Message}");
            }
            finally
            {
                this.ShowOptions(context);
            }
        }

    }
}

