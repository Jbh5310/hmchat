using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis;

namespace HMBot.Dialogs
{
    [Serializable]
    public class GoogleCalendarForm
    {

        // 4개의 Properties 
        // 1) 제목 
        [Describe("제목")]
        [Prompt("제목을 입력하여주십시오 \n\n(예시:대원제약 본사 출장)")]
   //     [Template(TemplateUsage.NotUnderstood, "입력하신 출발지를 조회할 수 없습니다.\n\n인천의 경우, '인천' 또는 'ICN' 형식으로 입력해주세요.")]
    //    [Template(TemplateUsage.Help, new string[] { "출발지는 공항코드, 한국어, 영어, 일본어, 중국어(간체,번체)를 지원합니다. \n\n인천의 경우 'ICN', '인천', 'Incheon', '仁川' 형식으로 입력해주세요." })]
        public string Title { get; set; }


        // 2) 장소
        [Describe("장소")]
        [Prompt("장소를 입력해 주세요. \n\n(예시:장한평 , 부산)")]
    
        public string  Place { get; set; }




        // 3) 시작일시 
        [Describe("시작일시")]
        [Prompt("시작일시를 입력해주세요. \n\n(예시:2017년7월6일 오전9시)")]

        public string DateFrom { get; set; }




        // 3) 시작일시 
        [Describe("종료일시")]
        [Prompt("종료일시를 입력해주세요. \n\n(예시:2017년7월6일 오후10시)")]

        public string DateTo { get; set; }




        // 3) 종료일시 


        public static IForm<GoogleCalendarForm> BuildForm()
        {
            return new FormBuilder<GoogleCalendarForm>()
                .Build();
        }
    }
}