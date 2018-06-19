using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Infastructure;
using Library.Model;
using Microsoft.AspNet.Identity;

namespace Library.Data
{
    // I will use UserManager instead of this.
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>
    {
        public ApplicationUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }

    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
    }
}
