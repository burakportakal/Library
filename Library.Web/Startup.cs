using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.Model;
using Library.Web.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Library.Web.Startup))]

namespace Library.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            
            Mapper.Initialize(e =>
            {
                e.CreateMap<Reserve, ReserveViewModel>()
                    .ForMember(id => id.ReserveId,
                        setting => setting.MapFrom(reserve => reserve.ReserveId))
                    .ForMember(reserveDate => reserveDate.ReserveDate,
                        setting => setting.MapFrom(reserve => reserve.DateReserved))
                    .ForMember(returnDate => returnDate.ReturnDate,
                        settings => settings.MapFrom(reserve => reserve.CalculatedReturnDate))
                    .ForMember(isbn => isbn.Isbn,
                        settings => settings.MapFrom(reserve => reserve.BookIds.Isbn))
                    .ForMember(userId => userId.UserId,
                        settings => settings.MapFrom(reserve => reserve.UserId))
                    .ForMember(userReturnedDate => userReturnedDate.UserReturnedDate,
                        settings => settings.MapFrom(reserve => reserve.UserReturnedDate))
                    .ForMember(bookTitle => bookTitle.BookTitle,
                        settings => settings.MapFrom(reserve => reserve.BookIds.Book.BookTitle))
                    .ForMember(state => state.ReserveState,opt => opt.ResolveUsing(ReserveResolver))
                    .ForSourceMember(book => book.User, opt => opt.Ignore())
                    .ForSourceMember(book => book.BookIds, opt => opt.Ignore());


                e.CreateMap<CreateBookViewModel, Books>()
                    .ForMember(dest => dest.Isbn, source =>
                        source.MapFrom(book => book.Isbn))
                    .ForMember(dest => dest.BookTitle, source =>
                        source.MapFrom(book => book.BookTitle))
                    .ForMember(dest => dest.PublishYear, source =>
                        source.MapFrom(book => book.PublishYear))
                    .ForMember(dest => dest.BookCount, source =>
                        source.MapFrom(book => book.Count))
                    .ForMember(dest => dest.Authors, opt => opt.Ignore());

                e.CreateMap<Books, BookViewModel>()
                    .ForMember(dest => dest.BookTitle, source => source.MapFrom(title => title.BookTitle))
                    .ForMember(dest => dest.Count, source => source.MapFrom(title => title.BookCount))
                    .ForMember(dest => dest.Isbn, source => source.MapFrom(title => title.Isbn))
                    .ForMember(dest => dest.PublishYear, source => source.MapFrom(title => title.PublishYear))
                    .ForMember(dest => dest.BookIds,opt=> opt.Ignore())
                    .ForSourceMember(dest => dest.Authors, opt => opt.Ignore());

                e.CreateMap<Authors, AuthorViewModel>()
                    .ForMember(destAuthor => destAuthor.AuthorName,
                        sourceAuthor => sourceAuthor.MapFrom(s => s.AuthorName))
                    .ForSourceMember(author => author.Books, opt => opt.Ignore())
                    .ForSourceMember(author => author.AuthorsId, opt => opt.Ignore());

                 e.CreateMap<Reserve, ReserveBookViewModel>()
                           .ForMember(reserveViewModel => reserveViewModel.UserName,
                               destReserve => destReserve.MapFrom(reserve => reserve.User.UserName))
                           .ForMember(reserveViewModel => reserveViewModel.ReserveDate,
                               destReserve => destReserve.MapFrom(reserve => reserve.DateReserved))
                           .ForMember(reserveViewModel => reserveViewModel.ReserveId,
                               destReserve => destReserve.MapFrom(reserve => reserve.ReserveId))
                           .ForMember(reserveViewModel => reserveViewModel.ReturnDate,
                               destReserve => destReserve.MapFrom(reserve => reserve.CalculatedReturnDate))
                           .ForMember(reserveViewModel => reserveViewModel.UserReturnedDate,
                               destReserve => destReserve.MapFrom(reserve => reserve.UserReturnedDate))
                           .ForMember(dest => dest.ReserveState, opt => opt.Ignore())
                           .ForSourceMember(reserve => reserve.BookIds, opt => opt.Ignore())
                           .ForSourceMember(reserve => reserve.User, opt => opt.Ignore());

                e.CreateMap<BookIds, BookIdsViewModel>()
                    .ForMember(dest => dest.BookId, source => source.MapFrom(data => data.BookId))
                    .ForMember(dest => dest.Status, source => source.MapFrom(data => data.BookStatus))
                    .ForMember(dest => dest.Reserves, opt=> opt.Ignore())
                    .ForSourceMember(source => source.Isbn, opt => opt.Ignore())
                    .ForSourceMember(source => source.Book, opt => opt.Ignore())
                    .ForSourceMember(source => source.Reserves, opt => opt.Ignore())
                    ;
            });
                
            ConfigureAuth(app);
        }

        public static object ReserveResolver(Reserve reserve)
        {
            if (reserve.UserReturnedDate != null)
            {
                return BookStatus.Available;
            }
            else
            {
                return BookStatus.Reserved;
            }
        }
    }
}
