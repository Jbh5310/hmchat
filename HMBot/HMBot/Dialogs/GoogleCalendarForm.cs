using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMBot.Dialogs
{
    [Serializable]
    public class GoogleCalendarForm
    {
        // 4개의 Properties 
        // 1) 제목 
        // 2) 시작일시 
        // 3) 종료일시 
        // 4) 장소

        public string Title { get; set; }

        public static IForm<GoogleCalendarForm> BuildForm()
        {
            return new FormBuilder<GoogleCalendarForm>()
                .Build();
        }
    }
}