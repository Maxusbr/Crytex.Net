using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IDhcpServerService
    {
        IPagedList<DhcpServer> GetPageDhcpServer(int pageNumber, int pageSize);
        DhcpServer GetDhcpServerById(Guid id);
        DhcpServer CreateDhcpServer(DhcpServerOption server);
        void UpdateDhcpServer(DhcpServerOption server);
        void DeleteDhcpServer(Guid id);
    }
}
