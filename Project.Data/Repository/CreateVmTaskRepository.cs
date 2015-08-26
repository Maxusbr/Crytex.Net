using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;
using PagedList;

namespace Project.Data.Repository
{
    public class CreateVmTaskRepository : RepositoryBase<CreateVmTask>, ICreateVmTaskRepository
    {
        public CreateVmTaskRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IPagedList<CreateVmTask> GetPageWithContents(Page page)
        {
            var dbContext = this.DataContext;
            var tasksQuery = dbContext.CreateVmTasks.OrderBy(task=>task.CreationDate).GetPage(page);
            var tasksWithTemplatesQuery = tasksQuery.Join(dbContext.ServerTemplates, task => task.ServerTemplateId, temp => temp.Id, (task, template) => new { Task = task, Template = template });
            var finalQuery = tasksWithTemplatesQuery.Join(dbContext.Files, twt => twt.Template.ImageFileId, f => f.Id, (twt, f) => new { Task = twt.Task, Template = twt.Template, ImageFile = f });
            var result = finalQuery.ToList();

            var tasks = new List<CreateVmTask>();
            foreach (var resItem in result)
            {
                var task = resItem.Task;
                task.ServerTemplate = resItem.Template;
                task.ServerTemplate.ImageFileDescriptor = resItem.ImageFile;
                tasks.Add(task);
            }

            var total = tasks.Count();
            var tasksPagedList = new StaticPagedList<CreateVmTask>(tasks, page.PageNumber, page.PageSize, total);

            return tasksPagedList;
        }


        public CreateVmTask GetByIdWithContents(int id)
        {
            var queryResult = this.DataContext.CreateVmTasks.Where(task => task.Id == id)
                .Join(this.DataContext.ServerTemplates, task => task.ServerTemplateId, temp => temp.Id, (task, temp) => new { Task = task, Template = temp })
                .Join(this.DataContext.Files, twt => twt.Template.ImageFileId, f => f.Id, (twt, f) => new { Template = twt.Template, Task = twt.Task, File = f })
                .SingleOrDefault();
            
            CreateVmTask resultTask = null;
            if(queryResult != null){
                resultTask = queryResult.Task;
                resultTask.ServerTemplate = queryResult.Template;
                resultTask.ServerTemplate.ImageFileDescriptor = queryResult.File;
            }

            return resultTask;
        }
    }
}
