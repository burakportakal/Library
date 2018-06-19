using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class BookIds : IBookIds
    {
        public string BookId { get; set; }
        public string Isbn { get; set; }
        public BookStatus BookStatus { get; set; }
        public virtual Books Book { get; set; }
        public ICollection<Reserve> Reserves { get; set; }

        public BookIds()
        {
            BookStatus = BookStatus.Available;
        }
    }
    public enum BookStatus
    {
        Reserved,
        Available
    }
}
