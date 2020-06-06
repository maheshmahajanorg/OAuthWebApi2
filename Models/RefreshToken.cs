using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApi.Models
{
    public class RefreshToken
    {
        public string  Id { get; set; }
        public string ClientId { get; set; }
        public string Subject { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpireAt { get; set; }

        public string ProtectedTicket { get; set; } 
    }
}