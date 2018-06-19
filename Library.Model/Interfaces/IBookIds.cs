using System.Collections.Generic;

namespace Library.Model
{
    public interface IBookIds
    {
        Books Book { get; set; }
        string BookId { get; set; }
        BookStatus BookStatus { get; set; }
        string Isbn { get; set; }
        ICollection<Reserve> Reserves { get; set; }
    }
}