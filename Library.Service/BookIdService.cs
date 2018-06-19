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
    public interface IBookIdService
    {
        IEnumerable<BookIds> GetBookIdsByIsbn(string isbn);
        IEnumerable<BookIds> GetAvailableBooksByIsbn(string isbn);
        IEnumerable<BookIds> GetReservedBooksByIsbn(string isbn);
        BookIds GetBookIdForReserve(string isbn);
        void GenerateBookIdsForBook(Books book);
        void AddBookId(BookIds book);
        void SaveChanges();
        void UpdateBookId(BookIds bookId);
        void DeleteBookIds(BookIds bookId);

    }
    public class BookIdService: IBookIdService
    {
        private readonly IBookIdRepository bookidsRepository;
        private readonly IReserveRepository reserveRepository;
        private readonly IUnitOfWork unitOfWork;
        public BookIdService(IBookIdRepository bookidsRepository,IReserveRepository reserveRepository, IUnitOfWork unitOfWork)
        {
            this.bookidsRepository = bookidsRepository;
            this.unitOfWork = unitOfWork;
            this.reserveRepository = reserveRepository;
        }
        public IEnumerable<BookIds> GetBookIdsByIsbn(string isbn)
        {
            return bookidsRepository.GetAll(isbn);
        }

        public IEnumerable<BookIds> GetAvailableBooksByIsbn(string isbn)
        {
            return bookidsRepository.GetAvailableBooks(isbn);
        }

        public IEnumerable<BookIds> GetReservedBooksByIsbn(string isbn)
        {
            return bookidsRepository.GetReservedBooks(isbn);
        }

        public BookIds GetBookIdForReserve(string isbn)
        {
            return bookidsRepository.GetABookId(isbn);
        }

        public void GenerateBookIdsForBook(Books book)
        {
            for (int i = 0; i < book.BookCount; i++)
            {
                var newBookId =ModelFactory.GetBookIdsInstance(book.Isbn);
                AddBookId((BookIds)newBookId);
            }
        }

        public void AddBookId(BookIds book)
        {
            bookidsRepository.Add(book);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public void UpdateBookId(BookIds bookId)
        {
            bookidsRepository.Update(bookId);
        }

        public void DeleteBookIds(BookIds bookId)
        {
           bookidsRepository.Delete(bookId);
        }
    }
}
