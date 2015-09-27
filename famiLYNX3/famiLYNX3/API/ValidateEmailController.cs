using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace famiLYNX3.API
{
    [RoutePrefix("API/ValidateEmail")]
    public class ValidateEmailController : ApiController
    {
        [Route("validate", Name = "validateEmail")]
        public IRestResponse Get(string email) {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               "pubkey-4589ea7c99231f892c2304bcb754c08c");
            RestRequest request = new RestRequest();
            request.Resource = "/address/validate";
            request.AddParameter("address", email);
            var testVar = client.Execute(request);
            return client.Execute(request);
        }

    }
}
