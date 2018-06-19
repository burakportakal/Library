using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model;

namespace Library.Service
{
    internal static class ModelFactory
    {
        public static IBookIds GetBookIdsInstance(string isbn)
        {
            return new BookIds
            {
                BookId = Guid.NewGuid().ToString(),
                Isbn = isbn
            };
        }
    }
}
