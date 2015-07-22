using System.Collections.Generic;
using System.Linq;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;
using Sample.Service.IService;

namespace Project.Service.Service
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
