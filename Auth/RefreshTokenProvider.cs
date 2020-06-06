using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TestApi.Models;

namespace TestApi.Auth
{
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["clientId"]; 
            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }
            
            var refreshTokenId = Guid.NewGuid().ToString("n");
            
            //using (AuthRepository _repo = new AuthRepository())
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("ta:clientRefreshTokenLifeTime");
                var token = new RefreshToken()
                {
                    Id = refreshTokenId,
                    ClientId = clientid,
                    Subject = context.Ticket.Identity.Name,
                    IssuedAt = DateTime.UtcNow,
                    ExpireAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };
                
                context.Ticket.Properties.IssuedUtc = token.IssuedAt;
                context.Ticket.Properties.ExpiresUtc = token.ExpireAt; 
                
                // Serilize the ticket with claims  to store in DB  
                
                token.ProtectedTicket = context.SerializeTicket();
                context.SetToken(refreshTokenId); 
                
                //var result = await _repo.AddRefreshToken(token);
                //if (result)
                //{
                //    context.SetToken(refreshTokenId);
                //}
            }
        }

        /// <summary>
        /// ON Refresh token get the data from refresh token stored in database 
        /// Get the protected ticket and send back 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override  Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            
            var allowedOrigin = context.OwinContext.Get<string>("ta:clientAllowedOrigin");

            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            RefreshToken refreshToken = new RefreshToken();
            // Temp Code 
            //context.DeserializeTicket(refreshToken.ProtectedTicket);
            context.DeserializeTicket("Qv0vaeH70PPm-ttHGsTLYFYaJioXXA4Zs5eFUKDb0ri6n9FCi-i5YGaAb_U-WVZgskNSrp-uTIAgEJXNxUnLmnQfeSa7Y0DrFruSnBT3FAmlOrJgFQcWnIXKmjmsdl7f4GLL6b60ZMr45kIfjOzFk_8peK7Pm1fJR3Ttl78yXzQzzPnXCs6MVJ-2O5-vbU_c4aV8oRmQNl2ACOHcB_JIAjd2gczp5EsRYoeOii4gDru7i-a7WvSKeYYTBc0BfK7IgKxxHQE5sP3DImuRJdJ9bAX1wa-fmaxLts3eCUUfFffgb_QiH5DwVS1Las25mFdHcfcETCcC6MZx9V_W8N2dwTIsRAsO5P6n5ChselTNpHQ");
            return Task.FromResult<object>(null); 

            //string hashedTokenId = Helper.GetHash(context.Token);
            //using (AuthenticationRepository _repo = new AuthenticationRepository())
            //{
            //    var refreshToken = await _repo.FindRefreshToken(hashedTokenId);
            //    if (refreshToken != null)
            //    {
            //        //Get protectedTicket from refreshToken class
            //        context.DeserializeTicket(refreshToken.ProtectedTicket);
            //        var result = await _repo.RemoveRefreshTokenByID(hashedTokenId);
            //    }
            //}
        }
    }
}