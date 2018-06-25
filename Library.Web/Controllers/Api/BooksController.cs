using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Library.Model;
using Library.Service;
using Library.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Library.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        private readonly IBookService bookService;
        private readonly IReserveService reserveService;
        private readonly IAuthorService authorService;
        private readonly IBookIdService bookIdService;
        private ApplicationUserManager _userManager;

        public BooksController(IBookService bookService,
            IReserveService reserveService, IAuthorService authorService,IBookIdService bookIdService)
        {
            this.bookService = bookService;
            this.reserveService = reserveService;
            this.authorService = authorService;
            this.bookIdService = bookIdService;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Bütün kitapları getirir.
        /// </summary>
        /// <returns></returns>
        // GET: api/Books
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            var booksList = bookService.GetBooks();
            var viewModel = new List<BookViewModel>();
            foreach (var model in booksList)
            {
                var view = Mapper.Map<Books,BookViewModel>(model);
                view.InLibraryCount = model.BookIds.Count - view.ReservedCount;
                viewModel.Add(view);
            }
            if (viewModel.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, viewModel);
            }
        }

        /// <summary>
        /// Isbn ile istenen kitabın kopyalarına ait listeyi getirir.
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [Route("{isbn}")]
        public HttpResponseMessage Get(string isbn)
        {
            if (String.IsNullOrEmpty(isbn))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new {message = "Hatalı istek"});
            }

            var book = bookService.GetBook(isbn);
            if (book == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Kitap bulunamadı");
            }
           
            var bookViewModel = Mapper.Map<Books, BookViewModel>(book);
            bookViewModel.InLibraryCount = book.BookIds.Count - bookViewModel.ReservedCount;
            var allBookCopies= bookIdService.GetBookIdsByIsbn(isbn);
            var bookIdsViewModel = Mapper.Map<List<BookIds>,List<BookIdsViewModel>>(allBookCopies.ToList());
            
            bookViewModel.BookIds = bookIdsViewModel;
            if (bookViewModel.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, bookViewModel);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, bookViewModel);
            }
        }
     
        /// <summary>
        /// Veritabanına kitap ekler.
        /// Sadece Admin yetkisi ile kullanılabilir.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // POST: api/Books
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Post(CreateBookViewModel value)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.Values.SelectMany(m => m.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorList);
            }

            var bookIsInDb = bookService.GetBook(value.Isbn)!=null;
            if (bookIsInDb)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Isbn daha önce eklenmiş");
            }
            
            List<Authors> authorList = new List<Authors>();
            var book = Mapper.Map<CreateBookViewModel, Books>(value);
            foreach (var author in value.Author)
            {
                var authorObj = (Authors) Factory.GetAuthorInstance(author.AuthorName);
                var isInDb = authorService.GetAuthor(authorObj.AuthorName);
                if (isInDb != null)
                {
                    authorObj = isInDb;
                }
                authorList.Add(authorObj);
            }
            book.Authors = authorList;
            bookService.AddBook(book);
            bookIdService.GenerateBookIdsForBook(book);
            try
            {
                bookService.SaveChanges();
                var response = Request.CreateResponse<Books>(System.Net.HttpStatusCode.Created, book);
                return response;
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        new { exeption = ex.Message, innerException = ex.InnerException.Message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        new { exeption = ex.Message });
                }
            }
        }

        /// <summary>
        /// Kitabı günceller.
        /// Sadece Admin yetkisi ile kullanılabilir.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // PUT: api/Books?={isbn}
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Put(CreateBookViewModel value)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.Values.SelectMany(m => m.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorList);
            }
            var unModifiedBook = bookService.GetBook(value.Isbn);
            if (unModifiedBook ==null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Kitap bulunamadı");
            }
            List<Authors> authorList = new List<Authors>();
            var book = Mapper.Map<CreateBookViewModel, Books>(value);
            foreach (var author in value.Author)
            {
                var authorObj = (Authors)Factory.GetAuthorInstance(author.AuthorName);
                var isInDb = authorService.GetAuthor(authorObj.AuthorName);
                if (isInDb != null)
                {
                    authorObj = isInDb;
                }
                authorList.Add(authorObj);
            }
            unModifiedBook.Authors = authorList;
            unModifiedBook.BookCount = book.BookCount;
            unModifiedBook.BookTitle = book.BookTitle;
            unModifiedBook.Isbn = book.Isbn;
            unModifiedBook.PublishYear = book.PublishYear;
            bookService.UpdateBook(unModifiedBook);
            try
            {
                bookService.SaveChanges();
                return Request.CreateResponse<Books>(System.Net.HttpStatusCode.OK, unModifiedBook);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        new { exeption = ex.Message, innerException = ex.InnerException.Message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        new { exeption = ex.Message });
                }
            }

        }

        /// <summary>
        /// Veritabanından kitabı siler.
        /// Sadece Admin yetkisi ile kullanılabilir.
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        // DELETE: api/Books?isbn={isbn}
        [Authorize(Roles = "Admin")]
        [Route("{isbn}")]
        public HttpResponseMessage Delete(string isbn)
        {
            try
            {
                bookService.DeleteBook(isbn);
                bookService.SaveChanges();
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        /// <summary>
        /// Kullanıcının yaptığı tüm kiralamaları getirir.
        /// </summary>
        /// <returns></returns>
        // GET api/books/reserve
        [Route("Reserve")]
        public HttpResponseMessage GetReserve()
        {
            var reserves = reserveService.GetAllReservesByUserId(User.Identity.GetUserId());
            var viewModel = Mapper.Map<IEnumerable<Reserve>, IEnumerable<ReserveViewModel>>(reserves);
            foreach (var reserve in viewModel)
            {
                var book = bookService.GetBook(reserve.Isbn);
                var authors = Mapper.Map<List<Authors>, List<AuthorViewModel>>(book.Authors.ToList());
                reserve.Authors = authors;
                if (reserve.UserReturnedDate == null && DateTime.Compare(DateTime.Now, reserve.ReturnDate) <= 0)
                {
                    reserve.ReserveState = BookStatus.Reserved;
                }
                else if (reserve.UserReturnedDate == null && DateTime.Compare(DateTime.Now, reserve.ReturnDate) > 0)
                {
                    reserve.ReserveState = BookStatus.Reserved;
                }
                else
                {
                    reserve.ReserveState = BookStatus.Available;
                }
            }

            if (viewModel.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, viewModel);
            }
        }
        /// <summary>
        /// İstenen kitabın kiralama geçmişini getirir.
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [Route("Reserve/{isbn}")]
        public HttpResponseMessage GetReserve(string isbn)
        {
            isbn = isbn.Trim();
            var book = bookService.GetBook(isbn);
            if (book == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "Kitap bulunamadı." });
            }

            var viewModel = Mapper.Map<Books, BookViewModel>(book);
            var bookIds = bookIdService.GetBookIdsByIsbn(isbn);
            var bookIdViewModel = Mapper.Map<IEnumerable<BookIds>, IEnumerable<BookIdsViewModel>>(bookIds).ToList();
            //Her kopyanın ayrı ayrı kiralama geçmişi getiriliyor.
            foreach (var bookId in bookIdViewModel)
            {
                var booksReserveHistory = reserveService.GetBooksCopyReserveHistory(bookId.BookId);
                bookId.Reserves = Mapper.Map<List<Reserve>, List<ReserveViewModel>>(booksReserveHistory.ToList());
            }

            viewModel.BookIds = bookIdViewModel;
            if (viewModel.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, viewModel);
            }
        }
        /// <summary>
        /// İstenen kitabın kopyasının kiralama geçmişini getirir.
        /// </summary>
        /// <param name="isbn"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [Route("Reserve/{isbn}/{bookId}")]
        public HttpResponseMessage GetReserve(string isbn, string bookId)
        {
            if (String.IsNullOrEmpty(isbn) || String.IsNullOrEmpty(bookId))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Hatalı istek" });
            }

            isbn = isbn.Trim();
            bookId = bookId.Trim();
            var book = bookService.GetBook(isbn);
            if (book == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "Kitap bulunamadı." });
            }
            var booksReserveHistory = reserveService.GetBooksCopyReserveHistory(bookId);
            var viewModel = Mapper.Map<List<Reserve>, List<ReserveViewModel>>(booksReserveHistory.ToList());
            if (viewModel.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, viewModel);
            }
        }

        /// <summary>
        /// Kullanıcının kiralağığı kitap sayısı 3'ten az ise istenen kitabın kiralamaya uygun olan rastgele bir kopyasını kiralar.
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        // POST api/books/reserve/{isbn}
        [Route("Reserve/{isbn}")]
        public HttpResponseMessage PostReserve(string isbn)
        {
            #region Kontrol
            if (String.IsNullOrEmpty(isbn))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Hatalı istek!" });
            }

            isbn = isbn.Trim();
            var book = bookService.GetBook(isbn);
            var bookIds = bookIdService.GetBookIdForReserve(isbn);
            //Olmayan kitabı vermeyelim
            if(book == null || bookIds == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "Bu kitaptan kütüphanede bulunamadı!" });
            }
            if (book.BookIds.Count(e=> e.BookStatus==BookStatus.Available) == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    new {message = "Bu kitabın tüm kopyaları kirada!"});
            }

            var userId = User.Identity.GetUserId();
            // Kullanıcı kaç tane kiralık kitaba ve hangi kitaplara sahip.
            var reservedBooks = reserveService.GetBooksUserStillHave(userId);
            //Aynı kitabı iki kere vermeyelim
            var userReservedThisBook = reservedBooks.Any(e => e.BookIds.Isbn == isbn);
            if (userReservedThisBook)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden,new {message = "Bu kitap elinizde olduğu için tekrar kiralayamazsınız!"});
            }
            #endregion
            //Aynı anda 3 ten fazla kitap kiralayamasın.
            if (reservedBooks.Count() < 3)
            {
                bookIds.BookStatus = BookStatus.Reserved;
                bookIdService.UpdateBookId(bookIds);
                var reserve = Factory.GetReserveInstace(userId,bookIds);
                reserveService.AddReserve((Reserve)reserve);
                try
                {
                    reserveService.SaveChanges();
                    var viewModel = Mapper.Map<Reserve, ReserveViewModel>((Reserve)reserve);
                    return Request.CreateResponse<IReserveViewModel>(System.Net.HttpStatusCode.Created, viewModel);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new {exeption = ex.Message, innerException = ex.InnerException.Message});
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new { exeption = ex.Message });
                    }
                }
                
            }
            return Request.CreateResponse(HttpStatusCode.Forbidden, new { message = "Aynı anda sadece 3 kitap kiralayabilirsiniz!" });
        }

        /// <summary>
        /// Daha önce kiralanmış kitabı iade eder.
        /// </summary>
        /// <param name="reserveId"></param>
        /// <returns></returns>
        // PUT api/books/reserve/{reserveId}
        [Route("Reserve/{reserveId}")]
        public HttpResponseMessage PutReserve(int reserveId)
        {
            #region Kontrol
            var reserve = reserveService.GetReserve(reserveId);
            if (reserve == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "Kiralama bulunamadı!" });
            }
            //Başkasının kitabını güncellemesini engeller.
            var reserveObj = reserveService.GetBooksUserStillHave(User.Identity.GetUserId()).FirstOrDefault(e=>e.ReserveId==reserveId);
            if (reserveObj == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "Kiralama bulunamadı!" });
            }
            #endregion
            //Kitabın kopyasının durumunu Available yap.
            var bookId = reserveObj.BookIds;
            bookId.BookStatus = BookStatus.Available;
            bookIdService.UpdateBookId(bookId);
            //Kitapbın reserve durumuna geri dönüş tarihi ekle
            reserveObj.UserReturnedDate=DateTime.Now;
            reserveService.UpdateReserve(reserveObj);

            try
            {
                reserveService.SaveChanges();
                var viewModel = Mapper.Map<Reserve, ReserveViewModel>(reserveObj);
                return Request.CreateResponse<IReserveViewModel>(System.Net.HttpStatusCode.Created, viewModel);

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        new {exeption = ex.Message, innerException = ex.InnerException.Message});
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        new { exeption = ex.Message });
                }
            }
        }
    }
}
