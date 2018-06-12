using Library.Data.Infastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model;

namespace Library.Data
{
    public class BooksRepository : RepositoryBase<Books>, IBooksRepository
    {
        public BooksRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Books GetById(string isbn)
        {
            return DbContext.Books.Include(e => e.Authors).FirstOrDefault(i => i.Isbn == isbn);
        }

        public override IEnumerable<Books> GetAll()
        {
           
            return DbContext.Books.Include(e => e.Authors);
        }

        public void Delete(string isbn)
        {
            var book = DbContext.Books.Find(isbn);
            DbContext.Books.Remove(book);
        }
    }

    public interface IBooksRepository : IRepository<Books>
    {
        Books GetById(string isbn);
        void Delete(string isbn);
    }
}
