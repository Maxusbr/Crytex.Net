using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Service.IService
{
    public interface INewsService
    {
        News CreateNews(News trigger);

        void UpdateNews(News trigger);

        void DeleteNewsById(Guid id);

        News GetNewsById(Guid id);

        IPagedList<News> GetPage(int pageNumber, int pageSize);

        IEnumerable<News> GetAll();
    }
}
