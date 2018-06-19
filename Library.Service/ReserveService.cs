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
    public interface IReserveService
    {
        IEnumerable<Reserve> GetReserves();
        Reserve GetReserve(int id);
        Reserve GetReserveByUserId(string isbn,string userId);
        IEnumerable<Reserve> GetAllReservesByUserId(string userId);
        /// <summary>
        /// Get list of books user not returned.
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <returns></returns>
        IEnumerable<Reserve> GetBooksUserStillHave(string userId);

        IEnumerable<Reserve> GetBooksCopyReserveHistory(string bookId);
        IEnumerable<Reserve> GetBooksReserveHistory(string isbn);
        void UpdateReserve(Reserve reserve);
        void AddReserve(Reserve reserve);
        void SaveChanges();

    }
    public class ReserveService: IReserveService
    {
        private readonly IReserveRepository reserveRepository;
        private readonly IUnitOfWork unitOfWork;

        public ReserveService(IReserveRepository reserveReposityory, IUnitOfWork unitOfWork)
        {
            this.reserveRepository = reserveReposityory;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Reserve> GetReserves()
        {
            return reserveRepository.GetAll();
        }

        public Reserve GetReserve(int id)
        {
            return reserveRepository.GetById(id);
        }

        public Reserve GetReserveByUserId(string isbn ,string userId)
        {
            return reserveRepository.GetAll().FirstOrDefault(i => (i.BookIds.Isbn == isbn && i.UserId == userId));
        }

        public IEnumerable<Reserve> GetAllReservesByUserId(string userId)
        {
            return reserveRepository.GetUsersAllReservesByUserId(userId);
        }

        public IEnumerable<Reserve> GetBooksUserStillHave(string userId)
        {
            return reserveRepository.GetUsersNotReturnedBooksByUserId(userId);
        }

        public IEnumerable<Reserve> GetBooksCopyReserveHistory(string bookId)
        {
            return reserveRepository.GetBooksCopyReserveHistory(bookId);
        }

        public IEnumerable<Reserve> GetBooksReserveHistory(string isbn)
        {
            return reserveRepository.GetBooksAllReserves(isbn);
        }

        public void UpdateReserve(Reserve reserve)
        {
            reserveRepository.Update(reserve);
        }
        public void AddReserve(Reserve reserve)
        {
            var isUserHasPesmission = reserveRepository.GetUsersNotReturnedBooksByUserId(reserve.UserId).Count() <= 3;
            if (isUserHasPesmission)
            {
                reserveRepository.Add(reserve);
            }
            else
            {
                throw new Exception("User has more then three books to reserve");
            }
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }
    }
}
