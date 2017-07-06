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
        private string C_TIMEZONE_SEOUL { get; } = "Asia/Seoul";
        private string C_CALENDAR_ID { get; } = "primary";
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
            Event iCalEvent  = CraeteICalEvent(_Event);
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

        /// <summary>
        /// Craete iCal Event Object
        /// </summary>
        /// <param name="_Event"></param>
        /// <returns></returns>
        private Event CraeteICalEvent(HMEvent _Event)
        {
            //Event 생성
            Event newEvent = new Event()
            {
                Summary = _Event.Subject,
                Location = _Event.Location,
                Start = new EventDateTime()
                {
                    TimeZone = C_TIMEZONE_SEOUL,
                    DateTime = DateTime.Parse(_Event.StartDt)
                },
                End = new EventDateTime()
                {
                    TimeZone = C_TIMEZONE_SEOUL,
                    DateTime = DateTime.Parse(_Event.EndDt)
                }
            };

            //참석자
            newEvent.Attendees = _Event.GetAttendees(_Event);
            return newEvent;
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
