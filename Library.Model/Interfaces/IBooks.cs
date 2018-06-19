using System;
using System.Collections.Generic;

namespace Library.Model
{
    public interface IBooks
    {
        ICollection<Authors> Authors { get; set; }
        int BookCount { get; set; }
        string BookTitle { get; set; }
        string Isbn { get; set; }
        string PublishYear { get; set; }
        ICollection<BookIds> BookIds { get; set; }
    }
}