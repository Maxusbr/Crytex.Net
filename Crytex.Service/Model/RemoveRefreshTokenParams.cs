using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.Model
{
    public class RemoveRefreshTokenParams
    {
        public String UserName { get; set; }
        public String OriginToken { get; set; }
        public String RefreshToken { get; set; }
    }
}
