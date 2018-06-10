using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Infastructure;
using Library.Model;

namespace Library.Data
{
    public class AuthorRepository : RepositoryBase<Authors>, IAuthorRepository
    {
        public AuthorRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Authors GetAuthorByName(string authorName)
        {
            return DbContext.Authors.FirstOrDefault(a => a.AuthorName == authorName);
        }
    }

    public interface IAuthorRepository : IRepository<Authors>
    {
        Authors GetAuthorByName(string authorName);
    }
}

