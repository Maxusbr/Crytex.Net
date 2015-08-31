using System.Collections.Generic;
using System.Linq;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Sample.Service.IService;

namespace Crytex.Service.Service
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _focusRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MessageService(IMessageRepository focusRepository, IUnitOfWork unitOfWork)
        {
            _focusRepository = focusRepository;
            _unitOfWork = unitOfWork;
        }

        public void LogMessage(string message)
        {
            _focusRepository.Add(new Message() { Body = message});
            _unitOfWork.Commit();
        }

        public List<Message> GetAll()
        {
            return _focusRepository.GetAll().ToList();
        }
    }
}
