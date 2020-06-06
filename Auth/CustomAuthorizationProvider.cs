using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TestApi.Models;

namespace TestApi.Auth
{
    public class CustomAuthorizationProvider : OAuthAuthorizationServerProvider
    {
        ///<summary>
        ///   Validate Client Authentication method is used to validate the  client 
        ///   Not required if the application is used by single client 
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        //public override  async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        //{
        //    context.Validated();
        //}


        ///Validate client
        public override  async  Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty; 
            
            // The TryGetBasicCredentials method checks the Authorization header and
            // Return the ClientId and clientSecret
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.SetError("invalid_client", "Client credentials could not be retrieved through the Authorization header.");
                context.Rejected();
                return;
            } 
            Client client = new Client() {ClientId = clientId   ,  ClientSecret = clientSecret };
            //Check the existence of by calling the ValidateClient method
            //ClientMaster client = (new ClientMasterRepository()).ValidateClient(clientId, clientSecret);
            
            if (client != null)
            {
                if (!client.IsActive)
                {
                    context.SetError("invalid_client", "Client is inactive.");
                    context.Rejected();
                    //  return Task.FromResult<object>(null);
                }

                // Client has been verified.
                context.OwinContext.Set("ta:clientId", clientId);
                context.OwinContext.Set<Client>("ta:client", client);
                context.OwinContext.Set<string>("ta:clientAllowedOrigin", client.AllowedOrigin);
                context.OwinContext.Set<string>("ta:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
                context.Validated();
                //return Task.FromResult<object>(null);

                //context.OwinContext.Set<ClientMaster>("oauth:client", client)
                //Client has been verified. 
                //context.OwinContext.Set("oauth:clientId", clientId);
                //context.Validated(clientId);
            }
            else
            {
                // Client could not be validated.
                context.SetError("invalid_client", "Client credentials are invalid.");
                context.Rejected();
            }
            context.Validated();
        }


        /// <summary>
        /// Generate the token based on the user name and password  
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //  Grant resource specific to client 
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            Client client = context.OwinContext.Get<Client>("ta:client");
            string allowedOrgin   = context.OwinContext.Get<string>("ta:clientAllowedOrigin");
            string refreshTokenLifeTime = context.OwinContext.Get<string>("ta:clientRefreshTokenLifeTime");
            if (allowedOrgin == null)
            {
                allowedOrgin = "*";
            }

            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrgin });

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            if (context.UserName == "admin" && context.Password == "admin")
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                identity.AddClaim(new Claim("UserId", "admin"));
                identity.AddClaim(new Claim("Username", "admin"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "admin"));
                identity.AddClaim(new Claim("Email", "Email"));
                identity.AddClaim(new Claim("Feature", "002"));
                context.Validated(identity);
            } 
            else if (context.UserName == "user" && context.Password == "user")
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim("UserId", "user"));
                identity.AddClaim(new Claim("Username", "user"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "user"));
                identity.AddClaim(new Claim("Email", "Email"));
                identity.AddClaim(new Claim("Feature", "001"));
                //Send extra properties with token 
                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "clientId", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    {
                        "userName", context.UserName
                    }
                }); 

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
                //if just need to create ticket 
                //context.Validated(identity);
            }
            else
            {
                context.SetError("Invalid_Grant", "Invalid username and password");
                return;
            }
        }

        /// <summary>
        /// To addition properties in token response
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns> 
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
        
        
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            string ClientId = context.OwinContext.Get<string>("ta:clientId");
            string newClientid = context.ClientId; 
            if(newClientid != ClientId)
            {
                context.SetError("Invalid_ClientId", "Refresh token passed is for different client");
                return Task.FromResult<object>(null);
            }
            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);
            return Task.FromResult<object>(null);
        }
        //public override async  Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        //{
        //    var identity = new ClaimsIdentity(context.Options.AuthenticationType); 
        //    if(context.UserName  == "admin" &&  context.Password =="admin")
        //    {
        //        identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
        //        identity.AddClaim(new Claim("UserId", "admin"));
        //        identity.AddClaim(new Claim("Username", "admin"));
        //        identity.AddClaim(new Claim(ClaimTypes.Name, "admin"));
        //        identity.AddClaim(new Claim("Feature", "002"));
        //        context.Validated(identity);
        //    } 
        //    else if (context.UserName == "user" && context.Password == "user")
        //    {

        //        identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
        //        identity.AddClaim(new Claim("UserId", "user"));
        //        identity.AddClaim(new Claim("Username", "user"));
        //        identity.AddClaim(new Claim(ClaimTypes.Name, "user"));
        //        identity.AddClaim(new Claim("Feature", "001"));

        //        //Send extra properties with token 
        //        var props = new AuthenticationProperties(new Dictionary<string, string>
        //        {
        //            {
        //                "oauth:clientId", (context.ClientId == null) ? string.Empty : context.ClientId
        //            },
        //            {
        //                "userName", context.UserName
        //            }
        //        }); 

        //        var ticket = new AuthenticationTicket(identity, props);
        //        context.Validated(ticket); 

        //        //if just need to create ticket 
        //        //context.Validated(identity);
        //    }
        //    else
        //    {
        //        context.SetError("Invalid_Grant", "Invalid username and password");
        //        return;
        //    }
        //}
    }
}