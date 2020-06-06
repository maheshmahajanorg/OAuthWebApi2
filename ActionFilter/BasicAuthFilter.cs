using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace TestApi.ActionFilter
{
    public class BasicAuthFilter :AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                var authToken = actionContext.Request.Headers.Authorization.Parameter;

                // decoding authToken we get decode value in 'Username:Password' format  
                var decodeauthToken = System.Text.Encoding.UTF8.GetString(
                       Convert.FromBase64String(authToken));
                
                //split the user name and password 
                var authUserNamePass = decodeauthToken.Split(':');
                // at 0th postion of array we get username and at 1st we get password  
                
                if (IsAuthorizedUser(authUserNamePass[0], authUserNamePass[1]))
                {
                    // if it web hosting then set principal two places as shown below 
                    // if self hosting then Thread.Current Principa,l can also work
                    
                    HttpContext.Current .User  = new GenericPrincipal(
                    new GenericIdentity(authUserNamePass[0]), null);

                    // setting current principle  
                    Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(authUserNamePass[0]), null); 
                }
                else
                {
                    actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request
                   .CreateResponse(HttpStatusCode.Unauthorized);
            }
            base.OnAuthorization(actionContext);
        }

        public bool IsAuthorizedUser(string UserName , string Password)
        {
            return true;
        }
    }
}