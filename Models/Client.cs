using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApi.Models
{
    public class Client
    {
        public string  ClientId { get; set; }
        public string ClientSecret { get; set; }

        public bool IsActive { get; set; } = true;
        public string AllowedOrigin { get; set; } = "*";

        public string RefreshTokenLifeTime { get; set; } = "10";
    }
}