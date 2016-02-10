using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.Service
{
    public class DhcpServerService : IDhcpServerService
    {
        private readonly IDhcpServerRepository _dhcpServerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DhcpServerService(IDhcpServerRepository dhcpServerRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dhcpServerRepository = dhcpServerRepository;
        }

        public IPagedList<DhcpServer> GetPageDhcpServer(int pageNumber, int pageSize)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<DhcpServer, bool>> where = x => true;

            var pagedList = _dhcpServerRepository.GetPage(pageInfo, where, x => x.Id);

            return pagedList;
        }

        public DhcpServer GetDhcpServerById(Guid id)
        {
            var server = _dhcpServerRepository.GetById(id);
            if (server == null)
            {
                throw new InvalidIdentifierException($"DhcpServer with id={id} doesn't exist");
            }

            return server;
        }

        public DhcpServer CreateDhcpServer(DhcpServerOption model)
        {
            IPAddress ip;
            if (!IPAddress.TryParse(model.Ip, out ip))
            {
                throw new InvalidIdentifierException($"Invalid IPAdress {model.Ip}");
            }
            var server = new DhcpServer
            {
                Login = model.Login,
                Name = model.Name,
                Password = model.Password,
                Ip = model.Ip
            };
            if (model.VirtualizationType != null)
                server.VirtualizationType = (TypeVirtualization)model.VirtualizationType;

            _dhcpServerRepository.Add(server);
            _unitOfWork.Commit();
            return server;
        }

        public void UpdateDhcpServer(DhcpServerOption model)
        {
            var server = _dhcpServerRepository.GetById(model.Id);
            if (server == null)
            {
                throw new InvalidIdentifierException($"DhcpServer with id={model.Id} doesn't exist");
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                server.Name = model.Name;
            }
            if (!string.IsNullOrEmpty(model.Login))
            {
                server.Login = model.Login;
                server.Password = model.Password;
            }
            if (model.VirtualizationType != null)
            {
                server.VirtualizationType = model.VirtualizationType ?? TypeVirtualization.HyperV;
            }
            if (!string.IsNullOrEmpty(model.Ip))
            {
                IPAddress ip;
                if (!IPAddress.TryParse(model.Ip, out ip))
                {
                    throw new InvalidIdentifierException($"Invalid IPAdress {model.Ip}");
                }
                server.Ip = model.Ip;
            }

            _dhcpServerRepository.Update(server);
            _unitOfWork.Commit();
        }


        public void DeleteDhcpServer(Guid id)
        {
            var server = _dhcpServerRepository.GetById(id);
            if (server == null)
            {
                throw new InvalidIdentifierException($"DhcpServer with id={id} doesn't exist");
            }
            _dhcpServerRepository.Delete(server);
            _unitOfWork.Commit();
        }
    }
}
