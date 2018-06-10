using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Authors
    {
        public int AuthorsId { get; set; }
        public string AuthorName { get; set; }
        public ICollection<Books> Books { get; set; }
    }
}
