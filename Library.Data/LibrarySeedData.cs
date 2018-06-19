using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Library.Data
{
    public class LibrarySeedData : DropCreateDatabaseIfModelChanges<LibraryEntities>
    {
        protected override void Seed(LibraryEntities context)
        {
            //AOXzk3nA3Vobf7vpv703kJt9iMD76UzV6lwaCHufQ4HPo/3N0F2HJphqoclambmXmw== burak1234
            try
            {

                var userIds = new Dictionary<string, string>();
                var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "burak.portakal@outlook.com",
                    PasswordHash = "AOXzk3nA3Vobf7vpv703kJt9iMD76UzV6lwaCHufQ4HPo/3N0F2HJphqoclambmXmw==",
                    Email = "burak.portakal@outlook.com",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                      UserName = "burak.portakal2@outlook.com",
                      PasswordHash = "AOXzk3nA3Vobf7vpv703kJt9iMD76UzV6lwaCHufQ4HPo/3N0F2HJphqoclambmXmw==",
                      Email = "burak.portakal2@outlook.com",
                      SecurityStamp = Guid.NewGuid().ToString()
                }
            };

                foreach (var user in users)
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    userIds.Add(user.Email, user.Id);
                }
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };
                manager.Create(role);

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                userManager.AddToRole(userIds["burak.portakal@outlook.com"], "Admin");
                context.SaveChanges();

                var books = new List<Books>
            {
                new Books
                {
                    BookTitle = "Gölgeler",
                    Isbn = "9786050952544",
                    Authors = new List<Authors>
                    {
                        new Authors
                        {
                            AuthorName = "Zülfü Livaneli"
                        }
                    },
                    PublishYear = "2018",
                    BookCount = 10
                },
                new Books
                {
                    BookTitle = "Sabahattin Ali Üç Roman",
                    Isbn = "9789750842429",
                    Authors = new List<Authors>
                    {
                        new Authors
                        {
                            AuthorName = "Sabahattin Ali"
                        }
                    },
                    PublishYear = "2018",
                    BookCount = 10
                },
                new Books
                {
                    BookTitle = "Sen Gittin Ya Ben Çok Güzelleştim",
                    Isbn = "9786053114253",
                    Authors = new List<Authors>
                    {
                        new Authors
                        {
                            AuthorName = "Nilgün Bodur"
                        }
                    },
                    PublishYear = "2018",
                    BookCount = 10
                },

            };
                books.ForEach(book => context.Books.Add(book));
                context.SaveChanges();
                foreach (var book in books)
                {
                    for (int i = 0; i < book.BookCount; i++)
                    {
                        var newBookId = new BookIds
                        {
                            BookId = Guid.NewGuid().ToString(),
                            Isbn = book.Isbn
                        };
                        context.BookIds.Add(newBookId);
                    }
                }

                context.SaveChanges();
            }
            catch
            {

             
            }
        }
    }
}
