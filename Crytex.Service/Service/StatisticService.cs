using System;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Service.IService;
using System.Collections.Generic;
using Crytex.Model.Models;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Crytex.Core.Extension;
using Crytex.Service.Extension;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.Service
{
    class StatisticService : IStatisticService
    {
        private IStatisticRepository _statisticRepository;
        private IUnitOfWork _unitOfWork;

        private IUserVmRepository _userVmRepository;
        private IApplicationUserRepository _applicationUserRepository;
        private ITaskV2Repository _taskV2Repository;

        public StatisticService(IUnitOfWork unitOfWork, 
            IStatisticRepository statisticRepository, 
            IUserVmRepository userVmRepository,
            IApplicationUserRepository applicationUserRepository,
            ITaskV2Repository taskV2Repository)
        {
            this._unitOfWork = unitOfWork;
            this._statisticRepository = statisticRepository;
            this._userVmRepository = userVmRepository;
            this._applicationUserRepository = applicationUserRepository;
            this._taskV2Repository = taskV2Repository;
        }

        public IEnumerable<Statistic> GetAllStatistics()
        {
            var statistic = this._statisticRepository.GetAll();
            return statistic;
        }

        public StatisticSummary GetSummary()
        {
            var statistic = new StatisticSummary();
            statistic.NumberUsers = _applicationUserRepository.CountUsers(u => true);
            statistic.NumberActiveUsers = _applicationUserRepository.CountUsers(u => u.UserVms.Count > 0);
            statistic.NumberMachine = _userVmRepository.CountUserVms(m => true);
            statistic.NumberRunningMachine = _userVmRepository.CountUserVms(m => m.Status == StatusVM.Enable);
            DateTime today = DateTime.UtcNow.TrimDate(TimeSpan.TicksPerDay);
            DateTime tomorrow = today.AddDays(1);
            statistic.CreatedMachinesToday = _taskV2Repository.CountTaskV2(t => t.TypeTask == TypeTask.CreateVm && t.CompletedAt >= today && t.CompletedAt <= tomorrow);

            return statistic;
        }

        public IPagedList<Statistic> GetAllPageStatistics(int pageNumber, int pageSize, StatisticType? type = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var page = new PageInfo(pageNumber, pageSize);
            var minDate = dateFrom ?? DateTime.MinValue;
            var maxDate = dateTo ?? DateTime.MaxValue;

            Expression<Func<Statistic, bool>> where = x => true;

            switch (type)
            {
                case null:
                    @where = x => true;
                    break;
                case StatisticType.NumberRunningMachine:
                    @where = x => x.Type == TypeStatistic.NumberRunningMachine;
                    break;
                case StatisticType.NumberStoppedMachine:
                    @where = x => x.Type == TypeStatistic.NumberStoppedMachine;
                    break;
                case StatisticType.AverageDelayStartEndTasksInPeriod:
                    @where = x => x.Type == TypeStatistic.AverageDelayStartEndTasksInPeriod;
                    break;
                case StatisticType.NumberTasks:
                    @where = x => x.Type == TypeStatistic.NumberTasks;
                    break;
                case StatisticType.NumberTasksCompletedDuringPeriod:
                    @where = x => x.Type == TypeStatistic.NumberTasksCompletedDuringPeriod;
                    break;
                case StatisticType.NumberUsers:
                    @where = x => x.Type == TypeStatistic.NumberUsers;
                    break;
                case StatisticType.UsersWithLeastOneRunningMachine:
                    @where = x => x.Type == TypeStatistic.UsersWithLeastOneRunningMachine;
                    break;
            }

            where = where.And(x => x.Date >= minDate && x.Date <= maxDate);

            var pageList = _statisticRepository.GetPage(page, where, s => s.Date);
            return pageList;
        }

        public Statistic GetStatisticById(int id)
        {
            var statistic = this._statisticRepository.GetById(id);

            if (statistic == null)
            {
                throw new InvalidIdentifierException(string.Format("Region with Id={0} doesn't exists", id));
            }

            return statistic;
        }
        public Statistic CreateStatistic(Statistic statistic)
        {
            this._statisticRepository.Add(statistic);
            this._unitOfWork.Commit();

            return statistic;
        }

        public void UpdateStatistic(Statistic updatedStatistic)
        {
            var statistic = this._statisticRepository.GetById(updatedStatistic.Id);

            if (statistic == null)
            {
                throw new InvalidIdentifierException(string.Format("Region width Id={0} doesn't exists", updatedStatistic.Id));
            }

            statistic.Date = DateTime.UtcNow;
            statistic.Type = updatedStatistic.Type;
            statistic.Value = updatedStatistic.Value;

            this._statisticRepository.Update(statistic);
            this._unitOfWork.Commit();
        }

        public void DeleteStatisticById(int id)
        {
            var statistic = this._statisticRepository.GetById(id);

            if (statistic == null)
            {
                throw new InvalidIdentifierException(string.Format("Region with Id={0} doesn't exists", id));
            }

            this._statisticRepository.Delete(statistic);
            this._unitOfWork.Commit();
            
        }

        public void CalculateStatistic(TypeStatistic type)
        {
            switch (type)
            {
                case TypeStatistic.NumberUsers:
                    CalculateNumberUsers();
                    break;
                case TypeStatistic.NumberTasks:
                    CalculateNumberTasks();
                    break;
                case TypeStatistic.AverageDelayStartEndTasksInPeriod:
                    CalculateAverageDelayStartEndTasksInPeriod(10);
                    break;
                case TypeStatistic.NumberTasksCompletedDuringPeriod:
                    CalculateNumberTasksCompletedDuringPeriod(1);
                    break;
                case TypeStatistic.UsersWithLeastOneRunningMachine:
                    CalculateUsersWithLeastOneRunningMachine();
                    break;
                case TypeStatistic.NumberRunningMachine:
                    CalculateNumberRunningMachine();
                    break;
                case TypeStatistic.NumberStoppedMachine:
                    CalculateNumberStoppedMachine();
                    break;
            }
        }

        private void CalculateNumberUsers()
        {
            int count = _applicationUserRepository.CountUsers(u => true);
            var statisticStoppedMachine = new Statistic
            {
                Type = TypeStatistic.NumberUsers,
                Value = count,
                Date = DateTime.UtcNow
            };

            _statisticRepository.Add(statisticStoppedMachine);
            _unitOfWork.Commit();
        }
        private void CalculateNumberTasks()
        {
            int count = _taskV2Repository.CountTaskV2(x=>x.StartedAt == null);
            var statisticNumberTasks = new Statistic
            {
                Type = TypeStatistic.NumberTasks,
                Value = count,
                Date = DateTime.UtcNow
            };

            _statisticRepository.Add(statisticNumberTasks);
            _unitOfWork.Commit();
        }
        private void CalculateAverageDelayStartEndTasksInPeriod(double minutes)
        {
            var timeStart = DateTime.UtcNow.AddMinutes(-(minutes)).TrimDate(TimeSpan.TicksPerMinute);
            var timeEnd = DateTime.UtcNow.TrimDate(TimeSpan.TicksPerMinute);
            var tasks = _taskV2Repository.GetMany(x => x.CompletedAt >= timeStart && x.CompletedAt <= timeEnd).ToList();
            int avarageMinutes = 0;

            if (tasks.Any())
            {
                var totalCount = tasks.Where(t=>t.CompletedAt.HasValue).Select(s=> new {diff = s.CompletedAt.Value - s.CreatedAt}).Select(d=>d.diff.TotalMinutes).Sum();
                avarageMinutes = Convert.ToInt32(totalCount / tasks.Count());
            }

            var statisticAverageDelayStartEndTasksInPeriod = new Statistic
            {
                Type = TypeStatistic.AverageDelayStartEndTasksInPeriod,
                Value = avarageMinutes,
                Date = timeEnd
            };

            _statisticRepository.Add(statisticAverageDelayStartEndTasksInPeriod);
            _unitOfWork.Commit();
        }
        private void CalculateNumberTasksCompletedDuringPeriod(double hours)
        {
            
            var timeStart = DateTime.UtcNow.AddHours(-(hours)).TrimDate(TimeSpan.TicksPerHour);
            var timeEnd = DateTime.UtcNow.TrimDate(TimeSpan.TicksPerHour);

            int count = _taskV2Repository.CountTaskV2(x=>x.CompletedAt >= timeStart && x.CompletedAt <= timeEnd);
            var statisticNumberTasksCompletedDuringPeriod = new Statistic
            {
                Type = TypeStatistic.NumberTasksCompletedDuringPeriod,
                Value = count,
                Date = timeEnd
            };

            _statisticRepository.Add(statisticNumberTasksCompletedDuringPeriod);
            _unitOfWork.Commit();
        }
        private void CalculateUsersWithLeastOneRunningMachine()
        {
            int count = _applicationUserRepository.CountUsers(u=>u.UserVms.Count > 0);
            var statisticUsersWithLeastOneRunningMachine = new Statistic
            {
                Type = TypeStatistic.UsersWithLeastOneRunningMachine,
                Value = count,
                Date = DateTime.UtcNow
            };

            _statisticRepository.Add(statisticUsersWithLeastOneRunningMachine);
            _unitOfWork.Commit();
        }
        private void CalculateNumberRunningMachine()
        {
            int count = _userVmRepository.CountUserVms(v => v.Status == StatusVM.Enable);
            var statisticRunningMachine = new Statistic
            {
                Type = TypeStatistic.NumberRunningMachine,
                Value = count,
                Date = DateTime.UtcNow
            };

            _statisticRepository.Add(statisticRunningMachine);
            _unitOfWork.Commit();
        }
        private void CalculateNumberStoppedMachine()
        {
            int count = _userVmRepository.CountUserVms(v => v.Status == StatusVM.Disable);
            var statisticStoppedMachine = new Statistic
            {
                Type = TypeStatistic.NumberStoppedMachine,
                Value = count,
                Date = DateTime.UtcNow
            };

            _statisticRepository.Add(statisticStoppedMachine);
            _unitOfWork.Commit();
        }


    }
}
