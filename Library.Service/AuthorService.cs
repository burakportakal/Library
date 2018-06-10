using Library.Data;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Infastructure;

namespace Library.Service
{
    public interface IAuthorService
    {
        IEnumerable<Authors> GetAuthors();
        IEnumerable<Books> GetAuthorsBooks(string authorName);
        Authors GetAuthor(int id);
        Authors GetAuthor(string authorName);
        void AddAuthor(Authors author);
        void SaveAuthor();
    }
    public class AuthorService :IAuthorService
    {
        private readonly IAuthorRepository authorRepository;
        private readonly IUnitOfWork unitOfWork;
        public AuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            this.authorRepository = authorRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Authors> GetAuthors()
        {
            return authorRepository.GetAll();
        }

        public IEnumerable<Books> GetAuthorsBooks(string authorName)
        {
            return authorRepository.GetAuthorByName(authorName).Books;
        }

        public Authors GetAuthor(int id)
        {
            return authorRepository.GetById(id);
        }
        public Authors GetAuthor(string authorName)
        {
            return authorRepository.GetAuthorByName(authorName);
        }

        public void AddAuthor(Authors author)
        {
            authorRepository.Add(author);
        }
        public void SaveAuthor()
        {
           unitOfWork.Commit();
        }
    }
}
