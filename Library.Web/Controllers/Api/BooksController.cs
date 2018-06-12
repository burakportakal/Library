using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Library.Model;
using Library.Service;
using Library.Web.Models;

namespace Library.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        private readonly IBookService bookService;
        private readonly IReserveService reserveService;
        private readonly IAuthorService authorService;

        public BooksController(IBookService bookService,
            IReserveService reserveService, IAuthorService authorService )
        {
            this.bookService = bookService;
            this.reserveService = reserveService;
            this.authorService = authorService;
        }
        // GET: api/Books
        public IEnumerable<Books> Get()
        {
            return bookService.GetBooks();
        }

        // GET: api/Books/5
        public HttpResponseMessage Get(string isbn)
        {
            var book = bookService.GetBook(isbn);
            if (book != null)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted,book);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/Books
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Post(CreateBookViewModel value)
        {
            List<Authors> authorList = new List<Authors>();
            var book = (Books) Factory.GetBookInstance();
            book.Isbn = value.Isbn;
            book.BookCount = value.Count;
            book.BookTitle = value.BookTitle;
            book.PublishYear = value.PublishYear;
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
                bookService.SaveBook();
                var response = Request.CreateResponse<Books>(System.Net.HttpStatusCode.Created, book);
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
           
        }

        // PUT: api/Books/5
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Put(string isbn, CreateBookViewModel value)
        {
            List<Authors> authorList = new List<Authors>();
            var book = (Books)Factory.GetBookInstance();
            book.Isbn = value.Isbn;
            book.BookCount = value.Count;
            book.BookTitle = value.BookTitle;
            book.PublishYear = value.PublishYear;
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
                bookService.SaveBook();
                return Request.CreateResponse<Books>(System.Net.HttpStatusCode.OK, book);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        // DELETE: api/Books/5
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage Delete(string isbn)
        {
            try
            {
                bookService.DeleteBook(isbn);
                bookService.SaveBook();
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        // POST api/books/reserve
        [Route("reserve")]
        public async Task<IHttpActionResult> Reserve(string isbn)
        {
            var user = applicationUserService
            return Ok();
        }
    }
}
