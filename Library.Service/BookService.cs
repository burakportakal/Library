using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data;
using Library.Data.Infastructure;
using Library.Model;

namespace Library.Service
{
    public interface IBookService : IService
    {
        IEnumerable<Books> GetBooks();
        IEnumerable<Authors> GetBooksAuthors(string isbn);
        Books GetBook(string isbn);
        void AddBook(Books book);
        void UpdateBook(Books book);
        void DeleteBook(string isbn );
    }
    public class BookService : IBookService
    {
        private readonly IBooksRepository booksRepository;
        private readonly IUnitOfWork unitOfWork;
        public BookService(IBooksRepository booksRepository, IUnitOfWork unitOfWork)
        {
            this.booksRepository = booksRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Books> GetBooks()
        {
            return booksRepository.GetAll();
        }

        public IEnumerable<Authors> GetBooksAuthors(string isbn)
        {
            return booksRepository.GetById(isbn).Authors;
        }
        public Books GetBook(string isbn)
        {
            return booksRepository.GetById(isbn);
        }

        public void AddBook(Books book)
        {
            booksRepository.Add(book);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public void UpdateBook(Books book)
        {
            booksRepository.Update(book);
        }

        public void DeleteBook(string isbn)
        {
            booksRepository.Delete(isbn);
        }
    }
}
