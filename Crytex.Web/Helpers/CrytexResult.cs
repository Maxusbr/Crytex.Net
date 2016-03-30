using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace Crytex.Web.Helpers
{
    public class CrytexResult : IHttpActionResult
    {
        private ServerTypesResult typeResult;
        private object data;

        public CrytexResult(ServerTypesResult typeResult)
        {
            this.typeResult = typeResult;
        
        }
        public CrytexResult(ServerTypesResult typeResult, object data)
        {
            this.typeResult = typeResult;
            this.data = data;
        }

        Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = GetHttpResponseMessage();
            return Task.FromResult(response);
        }

        public HttpResponseMessage GetHttpResponseMessage()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new Dictionary<String, Object>()
                {
                    {
                       "error",  Enum.GetName(typeof(ServerTypesResult), typeResult)
                    },
                      {
                       "errorEnum",   typeResult
                    },
                     {
                       "data", data
                    }
                }))
            };

            return responseMessage;
        }
    }

}