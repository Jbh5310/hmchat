using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using HMBot.Models;
using Newtonsoft.Json.Linq;
//using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;


namespace HMBot.Dialogs
{
    [Serializable]
    //  `

    [LuisModel("c9d5cb47-ee4e-4e2f-b7a6-a9c0c1ee8054", "d2d16993b9274d8ca4adc618510caf2a")]

    public class DatetimeDialog  : LuisDialog<object>
    {
        private const string PickDepartureDateEntityType = "date";
        private const string PickDepartureEntityType = "itinerary::from";
        private const string PickArrivalEntityType = "itinerary::to";

        //  private IList<string> titleOptions = new List<string> { "“Very stylish, great stay, great staff”", "“good hotel awful meals”", "“Need more attention to little things”", "“Lovely small hotel ideally situated to explore the area.”", "“Positive surprise”", "“Beautiful suite and resort”" };


        //public DatetimeDialog(params ILuisService[] luis)
        //        : base(luis)

        //    {

        //}

        public DatetimeDialog(ILuisService luis) : base(luis)
        {

        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("이런.. 제가 이해할 수 없는 내용이라;;좀 쉽게 질문 해 주실 수 있을까요?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("flight_schedule_search")]
        [LuisIntent("itinerary_search")]
        public async Task QueryLuis(IDialogContext context, LuisResult result)
        {
            var entities = new List<EntityRecommendation>();
            foreach (var entity in result.Entities)
            {
                switch (entity.Type)
                {
                    case PickDepartureDateEntityType:
                        //    entities.Add(new EntityRecommendation(type: nameof(FlightScheduleForm.DepartureDate)) { Entity = entity.Entity.Replace(" ", "") });
                        break;
                    case PickDepartureEntityType:
                        //    entities.Add(new EntityRecommendation(type: nameof(FlightScheduleForm.Departure)) { Entity = entity.Entity.Replace(" ", "") });
                        break;
                    case PickArrivalEntityType:
                        //    entities.Add(new EntityRecommendation(type: nameof(FlightScheduleForm.Arrival)) { Entity = entity.Entity.Replace(" ", "") });
                        break;
                    default:
                        break;
                }
            }

            //   var flightScheduleForm = new FormDialog<FlightScheduleForm>(new FlightScheduleForm(), FlightScheduleForm.BuildForm, FormOptions.PromptInStart, entities);
            //   context.Call(flightScheduleForm, flightScheduleComplete);
        }

        //private async Task flightScheduleComplete(IDialogContext context, IAwaitable<FlightScheduleForm> result)
        //{
        //    try
        //    {
        //        var searchQuery = await result;
        //        var flightScheduleList = await this.GetFlightSchedule(searchQuery);
        //        await context.PostAsync($"일치하는 스케줄 정보가 {flightScheduleList.Count()} 개 있습니다.");

        //        if (flightScheduleList.Count() > 0)
        //        {
        //            var resultMessage = context.MakeMessage();
        //            resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        //            resultMessage.Attachments = new List<Attachment>();

        //            foreach (var flightSchedule in flightScheduleList)
        //            {
        //                string fileNo = string.Empty;
        //                if (flightSchedule.Status.Contains("도착"))
        //                {
        //                    fileNo = "181/181694";
        //                }
        //                else if (flightSchedule.Status.Contains("출발"))
        //                {
        //                    fileNo = "181/181700";
        //                }
        //                else if (flightSchedule.Status.Contains("비행중"))
        //                {
        //                    fileNo = "181/181698";
        //                }
        //                else if (flightSchedule.Status.Contains("회항"))
        //                {
        //                    fileNo = "68/68409";
        //                }
        //                else if (flightSchedule.Status.Contains("결항"))
        //                {
        //                    fileNo = "160/160033";
        //                }
        //                else
        //                {
        //                    fileNo = "181/181686";
        //                }

        //                //결과값 Link 버튼 URL
        //                //List<CardAction> cardButtons = new List<CardAction>();
        //                //CardAction plButton = new CardAction()
        //                //{
        //                //    Value = "https://www.jinair.com",
        //                //    Type = ActionTypes.OpenUrl,
        //                //    Title = "예약하기"
        //                //};
        //                //cardButtons.Add(plButton);

        //                HeroCard heroCard = new HeroCard()
        //                {
        //                    Title = $"{flightSchedule.Flight} : {flightSchedule.Status}",
        //                    Subtitle = $"출발:스케줄({flightSchedule.DepartureDisplayTitle}) : {flightSchedule.DepartureScheduleTime}({flightSchedule.DepartureTime})\r\n도착:스케줄({flightSchedule.ArrivalDisplayTitle}) : {flightSchedule.ArrivalScheduleTime}({flightSchedule.ArrivalTime})",
        //                    Images = new List<CardImage>()
        //                    {
        //                        new CardImage() { Url = $"http://imageog.flaticon.com/icons/png/128/{fileNo}.png?size=200&ext=png&bg=FFFFFFFF" }
        //                    }
        //                    //,Buttons = cardButtons
        //                };

        //                resultMessage.Attachments.Add(heroCard.ToAttachment());
        //            }

        //            await context.PostAsync(resultMessage);
        //        }
        //    }
        //    catch (FormCanceledException ex)
        //    {
        //        string reply;

        //        if (ex.InnerException == null)
        //        {
        //            reply = "스케줄 조회 동작이 취소되었습니다. 처음으로 돌아갑니다.";
        //        }
        //        else
        //        {
        //            reply = $"오류가 발생했습니다. 오류 상세 내역 : {ex.InnerException.Message}";
        //        }

        //        await context.PostAsync(reply);
        //    }
        //    finally
        //    {
        //        context.Done<object>(null);
        //    }
        //}

        //private async Task<IEnumerable<FlightSchedule>> GetFlightSchedule(FlightScheduleForm searchQuery)
        //{
        //    string requestUri = string.Empty;
        //    IRestResponse response;


        //    ProxyHelper proxy = new ProxyHelper();
        //    var list = proxy.Skd(searchQuery.DepartureDate, searchQuery.Departure.ToString(), searchQuery.Arrival.ToString()).Result;

        //    foreach (var item in list)
        //    {
        //        Console.WriteLine(item);

        //        requestUri = item.ToString();
        //        break;


        //    }


        //    var query = $"";
        //    var client = new RestClient(requestUri);
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddHeader("content-type", "application/x-www-form-urlencoded");
        //    request.AddParameter("application/x-www-form-urlencoded", query, ParameterType.RequestBody);
        //    response = client.Execute(request);





        //    var flightScheduleList = new List<FlightSchedule>();

        //    //string requestUri = string.Empty;
        //    //IRestResponse response;

        //    //requestUri = "http://www.jinair.com/RSV/FlightInfo/Common.asmx/GetFlightInfo";
        //    //var query = $"pDepDate={searchQuery.DepartureDate.Replace("-", "")}&pDepCityCode={searchQuery.Departure}%7CKOR&pArrCityCode={searchQuery.Arrival}%7CKOR";
        //    //var client = new RestClient(requestUri);
        //    //var request = new RestRequest(Method.POST);
        //    //request.AddHeader("cache-control", "no-cache");
        //    //request.AddHeader("content-type", "application/x-www-form-urlencoded");
        //    //request.AddParameter("application/x-www-form-urlencoded", query, ParameterType.RequestBody);
        //    //response = client.Execute(request);
        //    if (response.ResponseStatus == ResponseStatus.Completed)
        //    {
        //        if (response.Content != null)
        //        {
        //            XmlDocument xDoc = new XmlDocument();
        //            xDoc.LoadXml(response.Content);
        //            JObject result = JObject.Parse(xDoc.InnerText);

        //            JArray jaFlightSchedule = (JArray)result["ResultData"];
        //            foreach (var item in jaFlightSchedule)
        //            {
        //                FlightSchedule flightSchedule = new FlightSchedule()
        //                {
        //                    Flight = item["FlightNo"].ToString(),
        //                    DepartureTime = item["Departure"]["DisplayTime"].ToString(),
        //                    DepartureScheduleTime = item["Departure"]["ScheduleTime"].ToString(),
        //                    DepartureDisplayTitle = item["Departure"]["DisplayTimeTitle"].ToString().Replace(" ", ""),
        //                    ArrivalTime = item["Arrival"]["DisplayTime"].ToString(),
        //                    ArrivalScheduleTime = item["Arrival"]["ScheduleTime"].ToString(),
        //                    ArrivalDisplayTitle = item["Arrival"]["DisplayTimeTitle"].ToString().Replace(" ", ""),
        //                    Status = item["Status"].ToString()
        //                };
        //                flightScheduleList.Add(flightSchedule);
        //            }
        //        }
        //    }
        //    return flightScheduleList;
        //}
    }
}