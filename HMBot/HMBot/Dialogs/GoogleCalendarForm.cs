using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;
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




        // 4) 종료일시 
        [Describe("종료일시")]
        [Prompt("종료일시를 입력해주세요. \n\n(예시:2017년7월6일 오후6시)")    ]

        public string DateTo { get; set; }





        public static IForm<GoogleCalendarForm> BuildForm()
        {
            OnCompletionAsyncDelegate<GoogleCalendarForm> processFlightScheduleSearch = async (context, state) =>
            {
                //await context.PostAsync($"[출발일 : {state.DateFrom}, 출발지 : {state.Departure}, 도착지 : {state.Arrival}] 스케줄 정보를 조회 중입니다. 잠시만 기다려 주세요.");
            };

            return new FormBuilder<GoogleCalendarForm>()
                .Field(nameof(DateFrom))
                .Field(nameof(DateTo),
                    validate: async (state, response) =>
                    {
                        //모든 날짜 포멧을 yyyy-MM-dd 형식으로 변환하여 전송
                        var result = new ValidateResult { IsValid = true, Value = response };

                        var departureDate = (response as string).Trim();
                        if (Regex.IsMatch(departureDate, @"^(20)\d{2}-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[0-1])$"))
                        {
                            result.Value = departureDate;
                        }
                        else if (Regex.IsMatch(departureDate, @"^(20)\d{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[0-1])$"))
                        {
                            result.Value = departureDate.Substring(0, 4) + "-" + departureDate.Substring(4, 2) + "-" + departureDate.Substring(6, 2);
                        }
                        else if (Regex.IsMatch(departureDate, @"^(0[1-9]|1[012])/(0[1-9]|[12][0-9]|3[0-1])$"))
                        {
                            result.Value = DateTime.Now.Year + "-" + departureDate.Substring(0, 2) + "-" + departureDate.Substring(3, 2);
                        }
                        else if (Regex.IsMatch(departureDate, @"^(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[0-1])$"))
                        {
                            result.Value = DateTime.Now.Year + "-" + departureDate.Substring(0, 2) + "-" + departureDate.Substring(2, 2);
                        }
                        else
                        {
                            DateTime tempDateTime;
                            if (DateTime.TryParse(departureDate, out tempDateTime))
                            {
                                result.Value = tempDateTime.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                result.Feedback = "입력하신 날짜 형식을 이해 할 수 없습니다.";
                                result.IsValid = false;
                            }
                        }
                        return result;
                    })
                .AddRemainingFields()
                .OnCompletion(processFlightScheduleSearch)
                .Build();
        }
    }
}