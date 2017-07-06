using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICalTest.Model
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
        
    }
}
