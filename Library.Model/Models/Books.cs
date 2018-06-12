using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Books : IBooks
    {
        public string Isbn { get; set; }
        public string BookTitle { get; set; }
        public string PublishYear { get; set; }
        public int BookCount { get; set; }
        public ICollection<Authors> Authors { get; set; }
        public ICollection<Reserve> Reserve { get; set; }
    }
}
