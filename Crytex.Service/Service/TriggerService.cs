using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Model.Models.Notifications;
using Crytex.Service.Extension;
using Crytex.Service.IService;
using PagedList;

namespace Crytex.Service.Service
{
    public class TriggerService : ITriggerService
    {
        private readonly ITriggerRepository _triggerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TriggerService(ITriggerRepository triggerRepo, IUnitOfWork unitOfWork)
        {
            this._triggerRepository = triggerRepo;
            this._unitOfWork = unitOfWork;
        }


        public Trigger CreateTrigger(Trigger trigger)
        {
            _triggerRepository.Add(trigger);
            _unitOfWork.Commit();

            return trigger;
        }

        public void InitStandartTriggersForUser(string userId)
        {
            var triggerCreateVm = new Trigger
            {
                UserId = userId,
                ThresholdValueSecond = null,
                Type = TriggerType.EndTask,
                ThresholdValue = Convert.ToDouble(TypeTask.CreateVm)
            };

            var triggerUpdateVm = new Trigger
            {
                UserId = userId,
                ThresholdValueSecond = null,
                Type = TriggerType.EndTask,
                ThresholdValue = Convert.ToDouble(TypeTask.UpdateVm)
            };

            _triggerRepository.Add(triggerCreateVm);
            _triggerRepository.Add(triggerUpdateVm);

            _unitOfWork.Commit();
        }

        public void UpdateTrigger(Trigger trigger)
        {
            var triggerToUpdate = _triggerRepository.GetById(trigger.Id);

            if (triggerToUpdate == null)
            {
                throw new InvalidIdentifierException(string.Format("Trigger width Id={0} doesn't exists", trigger.Id));
            }

            triggerToUpdate.Type = trigger.Type;
            triggerToUpdate.UserId= trigger.UserId;
            triggerToUpdate.ThresholdValue = trigger.ThresholdValue;

            _triggerRepository.Update(triggerToUpdate);
            _unitOfWork.Commit();

        }

        public void DeleteTriggerById(Guid triggerId)
        {
            var triggerToDelete = _triggerRepository.GetById(triggerId);

            if (triggerToDelete == null)
            {
                throw new InvalidIdentifierException(string.Format("Trigger width Id={0} doesn't exists", triggerId));
            }

            _triggerRepository.Delete(triggerToDelete);
            _unitOfWork.Commit();
        }

        public Trigger GetTriggerById(Guid triggerId)
        {
            var trigger = _triggerRepository.GetById(triggerId);
            if (trigger == null)
            {
                throw new InvalidIdentifierException(string.Format("Trigger width Id={0} doesn't exists", triggerId));
            }

            return trigger;
        }

        public List<Trigger> GetUserTriggers(string userId, TriggerType? type = null)
        {
            Expression<Func<Trigger, bool>> where = x => x.UserId == userId;
            if (type != null)
            {
                where = where.And(t => t.Type == type);
            }
            var triggers = this._triggerRepository.GetMany(where);

            return triggers;
        }

        public Trigger GetUserTrigger(string userId, TriggerType type, double value)
        {
            var trigger = this._triggerRepository.Get(t => t.UserId == userId && t.Type == type && t.ThresholdValue == value);

            return trigger;
        }

        public IPagedList<Trigger> GetPage(int pageNumber, int pageSize, string userId = null)

        {
            Expression<Func<Trigger, bool>> where = x => true;
            if (!string.IsNullOrEmpty(userId))
            {
                where.And(t => t.UserId == userId);
            }
            var page = new PageInfo(pageNumber, pageSize);
            var triggers = this._triggerRepository.GetPage(page, where, (x => x.Id));
           
            return triggers;
        }
    }
}
