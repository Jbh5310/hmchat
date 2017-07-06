using Google.Apis.Calendar.v3.Data;
using ICalTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google;

namespace ICalTest
{
    public class ICalEvent
    {
        public static string C_TIMEZONE_SEOUL { get; } = "Asia/Seoul";
        public static string C_CALENDAR_ID { get; } = "primary";

        private CalendarService ICalService { get; set; }

        public ICalEvent(CalendarService _Service)
        {
            this.ICalService = _Service;
        }

        /// <summary>
        /// ICal에 이벤트를 추가
        /// </summary>
        /// <param name="_Event"></param>
        public async Task InsertEvent(HMEvent _Event)
        {
            Event iCalEvent  = _Event.CraeteICalEvent();
            try
            {
                EventsResource.InsertRequest requestEvent = ICalService.Events.Insert(iCalEvent, C_CALENDAR_ID);
                Event createdEvent = requestEvent.Execute();
                _Event.Id = createdEvent.Id;
            }
            catch (GoogleApiException ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        

        public async Task<List<TimePeriod>> GetUserEvents(HMFreeBusy _HMFreeBusy)
        {
            List<FreeBusyRequestItem> items = _HMFreeBusy.PersonList(_HMFreeBusy.Person.ToArray());

            FreeBusyRequest fbr = new FreeBusyRequest();
            fbr.TimeMin = DateTime.Parse(_HMFreeBusy.StartDt);
            fbr.TimeMax = DateTime.Parse(_HMFreeBusy.EndDt);
            fbr.TimeZone = _HMFreeBusy.TimeZone;
            fbr.Items = items;

            FreeBusyResponse fbrService = ICalService.Freebusy.Query(fbr).Execute();

            List<HMEvent> lstEvent = new List<HMEvent>();
            IDictionary<string, FreeBusyCalendar> busyCalendar = fbrService.Calendars;

            List<FreeBusyCalendar> freeBusyCalendars = new List<FreeBusyCalendar>();

            List<TimePeriod> lstTimePeriod = new List<TimePeriod>();
            foreach (string mail in _HMFreeBusy.Person.ToArray())
            {
                foreach (TimePeriod bb in busyCalendar[mail].Busy)
                {
                    lstTimePeriod.Add(bb);
                }
            }

            return lstTimePeriod;
        }
    }
}
