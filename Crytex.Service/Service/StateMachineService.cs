using System.Collections.Generic;
using System.Linq;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
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

        public IEnumerable<StateMachine> GetStateAll()
        {
            return this._stateMachineRepository.GetAll();
        }

        public StateMachine GetStateById(int id)
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
    }
}
