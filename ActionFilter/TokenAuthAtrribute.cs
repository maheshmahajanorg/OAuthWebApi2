using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TestApi.ActionFilter
{
    public class TokenAuthAtrribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        { 
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(actionContext);
            } 
            else
            {
                actionContext.Response = actionContext.Request
                     .CreateResponse(HttpStatusCode.Unauthorized);
            } 
        }
    }
}