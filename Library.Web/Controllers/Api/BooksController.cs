using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        private ApplicationUserManager _userManager;

        public BooksController(IBookService bookService,
            IReserveService reserveService, IAuthorService authorService )
        {
            this.bookService = bookService;
            this.reserveService = reserveService;
            this.authorService = authorService;
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
        // GET: api/Books
        public IEnumerable<BookViewModel> Get()
        {
            var booksList = bookService.GetBooks();
            var viewModel = new List<BookViewModel>();
            foreach (var model in booksList)
            {
                var view = Mapper.Map<Books,BookViewModel>(model);
                view.Reserves = Mapper.Map<List<Reserve>, List<ReserveBookViewModel>>(model.Reserve.ToList());
                viewModel.Add(view);
            }
            return viewModel;
        }

        // GET: api/Books?isbn={isbn}
        public HttpResponseMessage Get(string isbn)
        {
            var book = bookService.GetBook(isbn);
            if (book != null)
            {
                var view = Mapper.Map<Books, BookViewModel>(book);
                view.Reserves = Mapper.Map<List<Reserve>, List<ReserveBookViewModel>>(book.Reserve.ToList());
                foreach (var temp in view.Reserves)
                {
                    if (temp.UserReturnedDate == null && DateTime.Compare(DateTime.Now, temp.ReturnDate) <=0)
                    {
                        temp.ReserveState = ReserveState.Reserved;
                    }
                    else if (temp.UserReturnedDate == null && DateTime.Compare(DateTime.Now, temp.ReturnDate)>0)
                    {
                        temp.ReserveState = ReserveState.NotReturned;
                    }
                    else
                    {
                        temp.ReserveState = ReserveState.Returned;
                    }
                }
                return Request.CreateResponse(HttpStatusCode.Accepted,view);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        // POST: api/Books
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Post(CreateBookViewModel value)
        {
            List<Authors> authorList = new List<Authors>();
            var book = Mapper.Map<CreateBookViewModel, Books>(value);
            foreach (var author in value.Author)
            {
                var authorObj = (Authors) Factory.GetAuthorInstance();
                authorObj.AuthorName = author.AuthorName;
                var isInDb = authorService.GetAuthor(authorObj.AuthorName);
                if (isInDb != null)
                {
                    authorObj = isInDb;
                }
                authorList.Add(authorObj);
            }
            book.Authors = authorList;
            bookService.AddBook(book);
            try
            {
                bookService.SaveChanges();
                var response = Request.CreateResponse<Books>(System.Net.HttpStatusCode.Created, book);
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // PUT: api/Books?={isbn}
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Put(string isbn, CreateBookViewModel value)
        {
            List<Authors> authorList = new List<Authors>();
            var book = Mapper.Map<CreateBookViewModel, Books>(value);
            foreach (var author in value.Author)
            {
                var authorObj = (Authors)Factory.GetAuthorInstance();
                authorObj.AuthorName = author.AuthorName;
                var isInDb = authorService.GetAuthor(authorObj.AuthorName);
                if (isInDb != null)
                {
                    authorObj = isInDb;
                }
                authorList.Add(authorObj);
            }
            book.Authors = authorList;
            bookService.UpdateBook(book);
            try
            {
                bookService.SaveChanges();
                return Request.CreateResponse<Books>(System.Net.HttpStatusCode.OK, book);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        // DELETE: api/Books?isbn={isbn}
        [Authorize(Roles = "Admin")]
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

        // POST api/books/reserve?isbn={isbn}
        [Route("Reserve")]
        public HttpResponseMessage PostReserve(string isbn)
        {
            var book = bookService.GetBook(isbn);
            if(book == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            if (book.BookCount == 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Bu kitaptan kütüphanede kalmamıştır.");
            var userId = User.Identity.GetUserId();
            // Kullanıcı kaç tane kiralık kitaba sahip.
            var reservedBooks = reserveService.GetBooksUserStillHave(userId);
            //Aynı kitabı iki kere vermeyelim
            var userReservedThisBook = reservedBooks.Any(e => e.Isbn == isbn);

            if (reservedBooks.Count() < 3 && !userReservedThisBook)
            {
                var reserve = Factory.GetReserveInstace(userId,book);
                reserveService.AddReserve((Reserve)reserve);
                book.BookCount = book.BookCount - 1;
                bookService.UpdateBook(book);
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
            return Request.CreateResponse(HttpStatusCode.Forbidden);
        }
        // GET api/books/reserve
        [Route("Reserve")]
        public IEnumerable<ReserveViewModel> GetReserve()
        {
            var reserves = reserveService.GetAllReservesByUserId(User.Identity.GetUserId());
            return Mapper.Map<IEnumerable<Reserve>, IEnumerable<ReserveViewModel>>(reserves);
        }
        // PUT api/books/reserve?isbn={isbn}
        [Route("Reserve")]
        public HttpResponseMessage PutReserve(int reserveId)
        {
            var reserve = reserveService.GetReserve(reserveId);
            if (reserve == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            var reserveObj = reserveService.GetBooksUserStillHave(User.Identity.GetUserId()).FirstOrDefault(e=>e.ReserveId==reserveId);
            if (reserveObj == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var book = reserveObj.Books;
            book.BookCount = book.BookCount + 1;
            bookService.UpdateBook(book);
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
