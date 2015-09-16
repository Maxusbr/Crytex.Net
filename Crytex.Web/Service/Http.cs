using System.Web;
using Crytex.Core.Service;

namespace Crytex.Web.Service
{
    public class Http : IHttp
    {
        private HttpRequest _request;
        public Http(HttpRequest request)
        {
            this._request = request;
        }

        public string UserIp
        {
            get { return this._request.UserHostAddress; }
        }

        public string RequestPath
        {
            get { return this._request.Path; }
        }
    }
}