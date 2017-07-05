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
#pragma warning disable 649
#pragma warning disable CS1998

namespace HMBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string CESOption = "CES회의실/차량예약";
        private const string ScheduleOption = "일정등록";
        //private const string MyReservationOption = "예약조회";
        //private const string FAQOption = "서비스문의";
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

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            //// return our reply to the user
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            //switch (activity.Type)
            //{
                //case ActivityTypes.ConversationUpdate:
                //case ActivityTypes.ContactRelationUpdate:
                    //if (!userWelcomed)
                    //{
                    //    this.userWelcomed = true;
                    //    await context.PostAsync(Responses.WelcomeMessage);
                    //}
                    //else
                    //{
                    //    await context.PostAsync(Responses.WelcomeBackMessage);
                    //}
                    this.ShowOptions(context);
                //    break;
                //default:
                //    context.Wait(MessageReceivedAsync);
                //    break;
          //  }

            //this.ShowOptions(context);

            //context.Wait(MessageReceivedAsync);
        }

      private void ShowOptions(IDialogContext context)
        {
        PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { CESOption, ScheduleOption}, "안녕하세요HM봇 입니다^^, \r\n서비스를 선택하여주십시오.");

        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
             
                //language = "ko";
                string optionSelected = await result;
                LuisService luisService;


                switch (optionSelected)
                {
                    case CESOption:
                        await context.PostAsync("CES회의실/차량예약을 선택하셨습니다.");
                        //context.Call(new FlightScheduleDialog(luisService), this.ResumeAfterOptionDialog);
                        break;
                    case ScheduleOption:
                        await context.PostAsync("일정등록을 선택하셨습니다.");
                       // context.Call(new FlightStatusLuisDialog(luisService), this.ResumeAfterOptionDialog);
                        break;
                        ////case MyReservationOption:
                        ////    await context.PostAsync("예약 조회를 선택하셨습니다.\n\n입력예시:");
                        ////   // context.Call(new FlightStatusLuisDialog(luisService), this.ResumeAfterOptionDialog);
                        ////    break;
                        ////case FAQOption:
                        ////    await context.PostAsync("서비스 문의를 선택하셨습니다.\n\n입력예시:");
                        ////  //  context.Call(new FAQSearchDialog(), this.ResumeAfterOptionDialog);
                        ////    break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

    }
}

