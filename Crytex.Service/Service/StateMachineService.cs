using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.Extension;
using Crytex.Service.IService;

namespace Crytex.Service.Service
{
    public class StateMachineService : IStateMachineService
    {
        private readonly IStateMachineRepository _stateMachineRepository;
        private readonly IUnitOfWork _unitOfWork;
        public StateMachineService(IStateMachineRepository stateMachineRepository, IUnitOfWork unitOfWork)
        {
            _stateMachineRepository = stateMachineRepository;
            _unitOfWork = unitOfWork;
        }

        public virtual IEnumerable<StateMachine> GetStateByVmId(Guid vmId, int diffInMinutes = 0)
        {
            Expression<Func<StateMachine, bool>> where = s => s.VmId == vmId;
            if (diffInMinutes != 0)
            {
                var previewDate = DateTime.UtcNow.AddMinutes(-diffInMinutes);
                where = where.And(s => s.Date >= previewDate);
            } 

            var stateMachines = this._stateMachineRepository.GetMany(where, s=>s.Date);
            return stateMachines;
        }

        public IEnumerable<StateMachine> GetStateAll()
        {
            throw new NotImplementedException();
        }

        public virtual StateMachine GetStateById(int id)
        {
            var state = this._stateMachineRepository.GetById(id);
            if (state == null)
            {
                throw new InvalidIdentifierException(string.Format("UserVm with id={0} doesnt exist.", id));
            }

            return state;
        }

        public StateMachine CreateState(StateMachine state)
        {
            _stateMachineRepository.Add(state);
            _unitOfWork.Commit();
            return state;
        }

        public virtual StateMachine GetLastVmState(Guid vmId)
        {
            var state = this._stateMachineRepository.GetLastState(vmId);

            return state;
        }
    }
}
