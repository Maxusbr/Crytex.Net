using Crytex.Data.IRepository;
using Crytex.Model.Models.WebHosting;
using Crytex.Service.IService;
using System.Collections.Generic;
using PagedList;
using System;
using Crytex.Data.Infrastructure;
using System.Linq.Expressions;

namespace Crytex.Service.Service
{
    public class WebHostingTariffService : IWebHostingTariffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostingTariffRepository _webHostingTariffRepository;

        public WebHostingTariffService(IWebHostingTariffRepository webHostingTariffRepository, IUnitOfWork unitOfWork)
        {
            this._webHostingTariffRepository = webHostingTariffRepository;
            this._unitOfWork = unitOfWork;
        }

        public WebHostingTariff Create(WebHostingTariff tariff)
        {
            tariff.CreateDate = DateTime.UtcNow;
            tariff.LastUpdateDate = null;

            this._webHostingTariffRepository.Add(tariff);
            this._unitOfWork.Commit();

            return tariff;
        }

        public IPagedList<WebHostingTariff> GetPage(int pageNumber, int pageSize)
        {
            if(pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("pageNumber and pageSize must be greater than zero");
            }

            var pageInfo = new PageInfo(pageNumber, pageSize);
            var page = this._webHostingTariffRepository.GetPage(pageInfo, x => true, x => x.CreateDate);

            return page;
        }
    }
}
