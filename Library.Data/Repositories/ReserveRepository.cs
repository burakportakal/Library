using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Infastructure;
using Library.Model;

namespace Library.Data
{
    public class ReserveRepository :RepositoryBase<Reserve>, IReserveRepository
    {
        public ReserveRepository(IDbFactory dbFactory) : base(dbFactory) { }
        public List<Reserve> GetUsersAllReservesByUserId(string userId)
        {
            return DbContext.Reserve.Where(r => r.UserId == userId).ToList();
        }

        public List<Reserve> GetUsersNotReturnedBooksByUserId(string userId)
        {
            var reservedBooks = DbContext.Reserve
                .Where(r => r.UserId == userId)
                .Where(d => d.UserReturnedDate == null).ToList();
            return reservedBooks;
        }
    }

    public interface IReserveRepository : IRepository<Reserve>
    {
        List<Reserve> GetUsersAllReservesByUserId(string userId);
        List<Reserve> GetUsersNotReturnedBooksByUserId(string userId);
    }

}
