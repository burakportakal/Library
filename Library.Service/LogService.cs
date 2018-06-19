using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data;
using Library.Data.Infastructure;
using Library.Model;

namespace Library.Service
{
    public class LogService:ILogService
    {
        private readonly ILogRepository logRepository;
        private readonly IUnitOfWork unitOfWork;

        public LogService(ILogRepository logRepository, IUnitOfWork unitOfWork)
        {
            this.logRepository = logRepository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Log> GetAllLogs()
        {
            return logRepository.GetAll();
        }

        public IEnumerable<Log> GetLogsByUri(string uri)
        {
            return logRepository.GetByUri(uri);
        }

        public void AddLog(Log log)
        {
            logRepository.Add(log);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }
    }

    public interface ILogService
    {
        IEnumerable<Log> GetAllLogs();
        IEnumerable<Log> GetLogsByUri(string uri);
        void AddLog(Log log);
        void SaveChanges();
    }
    
}
