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
            GetAttendees(_Event, newEvent);
            return newEvent;
        }

        private void GetAttendees(HMEvent _Event, Event _NewEvent)
        {
            List<EventAttendee> lstAttendee = new List<EventAttendee>();
            foreach (string attendee in _Event.AttendeeList)
            {
                lstAttendee.Add(new EventAttendee()
                {
                    Email = attendee
                });
            }
            _NewEvent.Attendees = lstAttendee.ToArray<EventAttendee>();
        }

        public async Task GetUserEvents(params string[] _Users)
        {
            throw new NotImplementedException();
            //FreeBusyRequest fbr = new FreeBusyRequest();
            //fbr.TimeMin = DateTime.Parse("2017-07-06 00:00:01");
            //fbr.TimeMax = DateTime.Parse("2017-07-06 23:59:59");
            //fbr.TimeZone = "Asia/Seoul";
            //FreeBusyRequestItem c = new FreeBusyRequestItem();
            //c.Id = "jbh5310@gmail.com";
            //fbr.Items = new List<FreeBusyRequestItem>();
            //fbr.Items.Add(c);
            //FreeBusyRequestItem c = new FreeBusyRequestItem();
            //c.Id = "jbh5310@gmail.com";
            //fbr.Items = new List<FreeBusyRequestItem>();
            //fbr.Items.Add(c);
        }
    }
}
