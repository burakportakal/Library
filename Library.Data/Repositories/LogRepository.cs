using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Infastructure;
using Library.Model;

namespace Library.Data
{
    public class LogRepository :RepositoryBase<Log>,ILogRepository
    {
        public LogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override Log GetById(int id)
        {
           return DbContext.Log.Find(id);
        }

        public override void Delete(Log log)
        {
            DbContext.Log.Remove(log);
        }

        public IEnumerable<Log> GetByUri(string uri)
        {
            return DbContext.Log.Where(e => e.RequestUri == uri);
        }
    }
    public interface ILogRepository : IRepository<Log>
    {
        Log GetById(int id);
        void Delete(Log log);
        IEnumerable<Log> GetByUri(string uri);
    }
}
