using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBot.Models
{
    public class GoogleToken
    {
        public string AccessToken { get; set; }
        public long? ExpiresInSeconds { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Issued { get; set; }
    }
}
