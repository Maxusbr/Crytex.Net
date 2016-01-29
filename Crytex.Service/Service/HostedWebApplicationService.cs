using System;
using Crytex.Service.IService;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.WebHostingModels;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;

namespace Crytex.Service.Service
{
    class HostedWebApplicationService : IHostedWebApplicationService
    {
        private readonly IHostedWebApplicationRepository _hostedWebApplicationRepository;
        private readonly ITaskV2Service _taskService;
        private readonly IUnitOfWork _unitOfWork;

        public HostedWebApplicationService(IHostedWebApplicationRepository hostedWebApplicationRepository, ITaskV2Service taskService,
            IUnitOfWork unitOfWork)
        {
            this._hostedWebApplicationRepository = hostedWebApplicationRepository;
            this._taskService = taskService;
            this._unitOfWork = unitOfWork;
        }

        public virtual HostedWebApplication GetHostedWebApplicationById(Guid id)
        {
            var app = this._hostedWebApplicationRepository.Get(a => a.Id == id, a => a.WebHosting);

            if(app == null)
            {
                throw new InvalidIdentifierException($"HostedWebApplication with id={id.ToString()} doesnt exist.");
            }

            return app;
        }

        public void StartApplication(Guid appId)
        {
            var app = this.GetHostedWebApplicationById(appId);
            if(app.Status == WebApplicationStatus.Started || app.Status == WebApplicationStatus.StartRequested 
                || app.Status == WebApplicationStatus.RestartRequested)
            {
                return;
            }

            if(app.Status == WebApplicationStatus.Creating 
                || app.Status == WebApplicationStatus.StopRequested)
            {
                throw new TaskOperationException($"Cannot start app with status {app.Status.ToString()}");
            }

            var startAppTask = new TaskV2
            {
                TypeTask = TypeTask.StartWebApp,
                UserId = app.WebHosting.UserId,
            };
            var taskOptions = new WebApplicationTaskOptions
            {
                HostedWedApplicationId = appId
            };
            this._taskService.CreateTask(startAppTask, taskOptions);

            app.Status = WebApplicationStatus.StartRequested;
            this._hostedWebApplicationRepository.Update(app);
            this._unitOfWork.Commit();
        }

        public void RestartApplication(Guid appId)
        {
            var app = this.GetHostedWebApplicationById(appId);
            if (app.Status == WebApplicationStatus.RestartRequested)
            {
                return;
            }

            if (app.Status == WebApplicationStatus.Creating
                || app.Status == WebApplicationStatus.StartRequested
                || app.Status == WebApplicationStatus.StopRequested
                || app.Status == WebApplicationStatus.Stop)
            {
                throw new TaskOperationException($"Cannot start app with status {app.Status.ToString()}");
            }

            var startAppTask = new TaskV2
            {
                TypeTask = TypeTask.RestartWebApp,
                UserId = app.WebHosting.UserId,
            };
            var taskOptions = new WebApplicationTaskOptions
            {
                HostedWedApplicationId = appId
            };
            this._taskService.CreateTask(startAppTask, taskOptions);

            app.Status = WebApplicationStatus.RestartRequested;
            this._hostedWebApplicationRepository.Update(app);
            this._unitOfWork.Commit();
        }

        public void StopApplication(Guid appId)
        {
            var app = this.GetHostedWebApplicationById(appId);
            if(app.Status == WebApplicationStatus.Stop || app.Status == WebApplicationStatus.StopRequested)
            {
                return;
            }

            if(app.Status == WebApplicationStatus.Creating 
                || app.Status == WebApplicationStatus.StartRequested
                || app.Status == WebApplicationStatus.RestartRequested)
            {
                throw new TaskOperationException($"Cannot start app with status {app.Status.ToString()}");
            }

            var startAppTask = new TaskV2
            {
                TypeTask = TypeTask.StopWebApp,
                UserId = app.WebHosting.UserId,
            };
            var taskOptions = new WebApplicationTaskOptions
            {
                HostedWedApplicationId = appId
            };
            this._taskService.CreateTask(startAppTask, taskOptions);

            app.Status = WebApplicationStatus.StopRequested;
            this._hostedWebApplicationRepository.Update(app);
            this._unitOfWork.Commit();
        }
    }
}
