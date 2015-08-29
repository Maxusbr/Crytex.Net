using PagedList;
using Crytex.Model.Models;
using System.Collections.Generic;

namespace Crytex.Service.IService
{
    public interface IHelpDeskRequestService
    {
        HelpDeskRequest CreateNew(string summary, string details, string userId);

        void Update(HelpDeskRequest request);

        HelpDeskRequest GeById(int id);

        void DeleteById(int id);

        IPagedList<HelpDeskRequest> GetPage(int pageNumber, int pageSize);

        IEnumerable<HelpDeskRequestComment> GetCommentsByRequestId(int id);

        HelpDeskRequestComment CreateComment(int requestId, string comment, string userId);

        void DeleteCommentById(int id);

        void UpdateComment(int commentId, string comment);
    }
}
