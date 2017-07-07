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
using Microsoft.Bot.Builder.Luis;
using System.Configuration;
using Microsoft.Bot.Builder.Luis.Models;
using ICalTest.Model;
using ICalTest;

namespace HMBot.Dialogs
{

    [Serializable]
    public class GoogleCalendarForm
    {

        string asdf;
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
        [Prompt("시작일시를 입력해주세요. \n\n(예시:2017년7월6일 09시)")]

        public string DateFrom { get; set; }




        // 4) 종료일시 
        [Describe("종료일시")]
        [Prompt("종료일시를 입력해주세요. \n\n(예시:2017년7월6일 18시)")]

        public string DateTo { get; set; }


        public static IForm<GoogleCalendarForm> BuildForm()
        {
            OnCompletionAsyncDelegate<GoogleCalendarForm> processScheduleSearch = async (context, state) =>
            {




                LuisResult LuisResult = await  ParseUserInput(state.DateFrom);

             //   state.DateFrom = DateTime.Parse(LuisResult.Entities[0].Entity + LuisResult.Entities[1].Entity + LuisResult.Entities[2].Entity + LuisResult.Entities[3].Entity);
                state.DateFrom =  LuisResult.Entities[0].Entity.Replace("년", "-") + "0" + LuisResult.Entities[1].Entity.Replace("월", "-") + LuisResult.Entities[2].Entity.Replace("일", "") + LuisResult.Entities[3].Entity.Replace("시", "") + ":00";

                state.DateFrom = DateTime.Parse(state.DateFrom).ToString();

              //  DateTime.ParseExact()
               // DateTime.ParseExact

                LuisResult LuisResult2 = await ParseUserInput(state.DateTo);

                state.DateTo = LuisResult2.Entities[0].Entity.Replace("년", "-") + "0" + LuisResult2.Entities[1].Entity.Replace("월", "-") + LuisResult2.Entities[2].Entity.Replace("일", "") + LuisResult2.Entities[3].Entity.Replace("시", "") + ":00";

                //HMEvent newEvent = new HMEvent();
                //newEvent.Subject = state.Title;
                //newEvent.StartDt = state.DateFrom;
                //newEvent.EndDt = state.DateTo;
                //newEvent.Location = state.Place;
                //newEvent.AddAttendd("jeipil@gmail.com");
                //newEvent.AddAttendd("hanmiitrnd@gmail.com");

                //ICalEvent iCal = new ICalEvent(service);
                //iCal.InsertEvent(newEvent);



            };

            return new FormBuilder<GoogleCalendarForm>()
                .Field(nameof(Title))
                .Field(nameof(Place))
                .Field(nameof(DateFrom))
                .Field(nameof(DateTo),
                    validate: async (state, response) =>
                    {
                        //모든 날짜 포멧을 yyyy-MM-dd 형식으로 변환하여 전송
                        var result = new ValidateResult { IsValid = true, Value = response };
                        var DateFrom = (response as string).Trim();
      

  
                        return result;
                    })
                .AddRemainingFields()
                .OnCompletion(processScheduleSearch)
                .Build();
        }


            public static async Task<LuisResult> ParseUserInput(string strInput)
            {
                string strRet = string.Empty;
                string strEscaped = Uri.EscapeDataString(strInput);


                using (var client = new HttpClient())
                {
                    string uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/c9d5cb47-ee4e-4e2f-b7a6-a9c0c1ee8054?subscription-key=2117fd4436f644ca94c87bdabd8ce2f3&timezoneOffset=0&verbose=true&q=" + strEscaped;
                    HttpResponseMessage msg = await client.GetAsync(uri);

                if (msg.IsSuccessStatusCode)
                {
                    var jsonResponse = await msg.Content.ReadAsStringAsync();
                    var _Data = JsonConvert.DeserializeObject<LuisResult>(jsonResponse);
                    string stringData = "";

                   // stringData =  _Data.Entities[0].Entity.Replace("년", "-") + "0" + _Data.Entities[1].Entity.Replace("월", "-") + _Data.Entities[2].Entity.Replace("일", "") + _Data.Entities[3].Entity.Replace("시", "")+ ":00";

                    return _Data;



    
                }

            }
                return null;
            }

        //public static async Task<LuisResult> ParseUserInput2(string strInput)
        //{
        //    string strRet = string.Empty;
        //    string strEscaped = Uri.EscapeDataString(strInput);


        //    using (var client = new HttpClient())
        //    {
        //        string uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/c9d5cb47-ee4e-4e2f-b7a6-a9c0c1ee8054?subscription-key=2117fd4436f644ca94c87bdabd8ce2f3&timezoneOffset=0&verbose=true&q=" + strEscaped;
        //        HttpResponseMessage msg = await client.GetAsync(uri);


        //        if (msg.IsSuccessStatusCode)
        //        {
        //            var jsonResponse = await msg.Content.ReadAsStringAsync();
        //            var _Data = JsonConvert.DeserializeObject<LuisResult>(jsonResponse);

        //            string stringData = "";

        //            stringData = _Data.Entities[0].Entity.Replace("년", "-") + "0" + _Data.Entities[1].Entity.Replace("월", "-") + _Data.Entities[2].Entity.Replace("일", "") + _Data.Entities[3].Entity.Replace("시", "") + ":00";

        //            return _Data;
        //        }
        //    }
        //    return null;
        //}

    }
}