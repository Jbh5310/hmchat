using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBot.Services.Model
{
    public class HMEvent
    {

        /// <summary>
        /// iCal ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 제목
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 시작일시(YYYY-MM-DD 24HH:MI)
        /// </summary>
        private string strStartDt;
        public string StartDt
        {
            get
            {
                return strStartDt;
            }
            set
            {
                //뒤에 초를 붙여준다.
                strStartDt = value + ":00";
            }
        }
        /// <summary>
        /// 종료일시((YYYY-MM-DD 24HH:MI)
        /// </summary>
        private string strEndDt;
        public string EndDt
        {
            get
            {
                return strEndDt;
            }
            set
            {
                //뒤에 초를 붙여준다.
                strEndDt = value + ":00";
            }
        }
        public string Location { get; set; }

        /// <summary>
        /// 참석자 리스트(참석자는 모두 EMail형식이어야 한다.)
        /// </summary>
        public List<string> AttendeeList { get; set; } = new List<string>();

        public HMEvent() { }
        public HMEvent(string _Subject, string _StartDt, string _EndDt, string _Location,  params string[] _Attendees) : base()
        {
            Subject = _Subject;
            StartDt = _StartDt;
            EndDt = _EndDt;
            Location = _Location;

            foreach(string attendee in _Attendees)
            {
                if (this.AttendeeList.Contains(attendee))
                    continue;

                AttendeeList.Add(attendee);
            }
        }

        /// <summary>
        /// 참석자 추가
        /// </summary>
        /// <param name="attendee"></param>
        public void AddAttendd(string attendee)
        {
            if (AttendeeList.Contains(attendee))
                return;

            this.AttendeeList.Add(attendee);
        }

        public List<EventAttendee> GetAttendees()
        {
            List<EventAttendee> lstAttendee = new List<EventAttendee>();
            foreach (string attendee in AttendeeList)
            {
                lstAttendee.Add(new EventAttendee()
                {
                    Email = attendee
                });
            }
            return lstAttendee;
        }

        /// <summary>
        /// Craete iCal Event Object
        /// </summary>
        /// <param name="_Event"></param>
        /// <returns></returns>
        public Event CraeteICalEvent()
        {
            //Event 생성
            Event newEvent = new Event()
            {
                Summary = Subject,
                Location = Location,
                Start = new EventDateTime()
                {
                    TimeZone = ICalEvent.C_TIMEZONE_SEOUL,
                    DateTime = DateTime.Parse(StartDt)
                },
                End = new EventDateTime()
                {
                    TimeZone = ICalEvent.C_TIMEZONE_SEOUL,
                    DateTime = DateTime.Parse(EndDt)
                }
            };

            //참석자
            newEvent.Attendees = GetAttendees();
            return newEvent;
        }

    }
}
