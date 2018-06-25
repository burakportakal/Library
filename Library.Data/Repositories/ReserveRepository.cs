using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public IEnumerable<Reserve> GetUsersAllReservesByUserId(string userId)
        {
            return DbContext.Reserve.Where(r => r.UserId == userId);
        }

        public IEnumerable<Reserve> GetUsersNotReturnedBooksByUserId(string userId)
        {
            var reservedBooks = DbContext.Reserve
                .Where(r => r.UserId == userId)
                .Where(d => d.UserReturnedDate == null);
            return reservedBooks;
        }

        public IEnumerable<Reserve> GetBooksCopyReserveHistory(string bookId)
        {
            var reserveHistoryForBook = DbContext.Reserve.Where(e => e.BookId == bookId).Include(e=> e.User);
            return reserveHistoryForBook;
        }

        public IEnumerable<Reserve> GetBooksAllReserves(string isbn)
        {
            return DbContext.Reserve.Where(e => e.BookIds.Isbn == isbn);
           
        }
    }

    public interface IReserveRepository : IRepository<Reserve>
    {
        IEnumerable<Reserve> GetUsersAllReservesByUserId(string userId);
        IEnumerable<Reserve> GetUsersNotReturnedBooksByUserId(string userId);
        IEnumerable<Reserve> GetBooksCopyReserveHistory(string bookId);
        IEnumerable<Reserve> GetBooksAllReserves(string isbn);
    }

}
