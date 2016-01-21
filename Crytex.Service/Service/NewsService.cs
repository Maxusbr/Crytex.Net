using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Model.Models;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Service.Extension;
using Crytex.Service.IService;
using PagedList;

namespace Crytex.Service.Service
{
    public class NewsService: INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NewsService(INewsRepository repository, IUnitOfWork unitOfWork)
        {
            _newsRepository = repository;
            _unitOfWork = unitOfWork;
        }
        public News CreateNews(News news)
        {
            _newsRepository.Add(news);
            _unitOfWork.Commit();
            return news;
        }

        public void UpdateNews(News news)
        {
            var newsToUpdate = _newsRepository.GetById(news.Id);

            if (newsToUpdate == null)
            {
                throw new InvalidIdentifierException($"News width Id={news.Id} doesn't exists");
            }

            newsToUpdate.Title = news.Title;
            newsToUpdate.UserId = news.UserId;
            newsToUpdate.Body = news.Body;
            newsToUpdate.CreateTime = news.CreateTime;

            _newsRepository.Update(newsToUpdate);
            _unitOfWork.Commit();
        }

        public void DeleteNewsById(Guid newsid)
        {
            var newsToUpdate = _newsRepository.GetById(newsid);

            if (newsToUpdate == null)
            {
                throw new InvalidIdentifierException($"News width Id={newsid} doesn't exists");
            }
            _newsRepository.Delete(newsToUpdate);
            _unitOfWork.Commit();
        }

        public News GetNewsById(Guid newsid)
        {
            var newsToUpdate = _newsRepository.GetById(newsid);

            if (newsToUpdate == null)
            {
                throw new InvalidIdentifierException($"News width Id={newsid} doesn't exists");
            }
            return newsToUpdate;
        }

        public IPagedList<News> GetPage(int pageNumber, int pageSize)
        {
            Expression<Func<News, bool>> where = x => true;

            var page = new PageInfo(pageNumber, pageSize);
            var triggers = this._newsRepository.GetPage(page, where, (x => x.CreateTime), reverse:true);

            return triggers;
        }

        public IEnumerable<News> GetAll()
        {
            var news = this._newsRepository.GetAll();
            return news;
        }
    }
}
