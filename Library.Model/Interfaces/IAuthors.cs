using System.Collections.Generic;

namespace Library.Model
{
    public interface IAuthors
    {
        string AuthorName { get; set; }
        int AuthorsId { get; set; }
        ICollection<Books> Books { get; set; }
    }
}