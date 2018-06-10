using Library.Data.Infastructure;
using System;
using System.Collections.Generic;
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
            return DbContext.Books.FirstOrDefault(i => i.Isbn == isbn);
        }
    }

    public interface IBooksRepository : IRepository<Books>
    {
        Books GetById(string isbn);
    }
}
