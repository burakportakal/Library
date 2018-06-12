using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Model;
namespace Library.Web
{
    public static class Factory
    {
        public static IBooks GetBookInstance()
        {
            return new Books();
        }

        public static IAuthors GetAuthorInstance()
        {
            return new Authors();
        }

        public static IReserve GetReserveInstace()
        {
            return new Reserve();
        }

        public static IApplicationUser GetApplicationUserInstance()
        {
            return new ApplicationUser();
        }
    }
}