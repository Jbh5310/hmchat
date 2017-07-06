using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICalTest.Model
{
    public class HMFreeBusy
    {
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

        /// <summary>
        /// 타임존. 
        /// </summary>
        public string TimeZone { get; set; } = ICalEvent.C_TIMEZONE_SEOUL;

        /// <summary>
        /// 리스트(사용자는 모두 Google계정이어야 한다.)
        /// </summary>
        public List<string> Person { get; set; } = new List<string>();

        public HMFreeBusy() { }
        public HMFreeBusy(string _StartDt, string _EndDt, params string[] _Person) : base()
        {
            this.StartDt = _StartDt;
            this.EndDt = _EndDt;

            foreach (string attendee in _Person)
            {
                if (this.Person.Contains(attendee))
                    continue;

                Person.Add(attendee);
            }
        }

        public List<FreeBusyRequestItem> PersonList(params string[] _Person)
        {
            List<FreeBusyRequestItem> reqPerson = new List<FreeBusyRequestItem>();

            foreach (string mail in _Person)
            {
                reqPerson.Add(new FreeBusyRequestItem()
                {
                    Id = mail
                });
            }

            return reqPerson;
        }
    }
}
