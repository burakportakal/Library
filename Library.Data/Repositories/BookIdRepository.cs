using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Infastructure;
using Library.Model;

namespace Library.Data
{
    public class BookIdRepository: RepositoryBase<BookIds>,IBookIdRepository
    {
        public BookIdRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public BookIds GetById(string bookId)
        {
            return DbContext.BookIds.Find(bookId);
        }

        public BookIds GetABookId(string isbn)
        {
            return DbContext.BookIds
                .FirstOrDefault(e => e.Isbn == isbn && e.BookStatus == BookStatus.Available);
        }

        public IEnumerable<BookIds> GetAvailableBooks(string isbn)
        {
            return DbContext.BookIds.Where(e => e.Isbn==isbn && e.BookStatus == BookStatus.Available);
        }

        public IEnumerable<BookIds> GetReservedBooks(string isbn)
        {
            return DbContext.BookIds.Where(e => e.Isbn==isbn && e.BookStatus == BookStatus.Reserved);
        }

        public IEnumerable<BookIds> GetAll(string isbn)
        {
            return DbContext.BookIds.Where(e => e.Isbn == isbn);
        }
    }
    public interface IBookIdRepository : IRepository<BookIds>
    {
        BookIds GetById(string bookId);
        BookIds GetABookId(string isbn);
        IEnumerable<BookIds> GetAvailableBooks(string isbn);
        IEnumerable<BookIds> GetReservedBooks(string isbn);
        IEnumerable<BookIds> GetAll(string isbn);
    }
}
