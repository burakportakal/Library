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

        void UpdateReserve(Reserve reserve);
        void AddReserve(Reserve reserve);
        void SaveChanges();

    }
    public class ReserveService: IReserveService
    {
        private readonly IReserveRepository reserveReposityory;
        private readonly IUnitOfWork unitOfWork;

        public ReserveService(IReserveRepository reserveReposityory, IUnitOfWork unitOfWork)
        {
            this.reserveReposityory = reserveReposityory;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Reserve> GetReserves()
        {
            return reserveReposityory.GetAll();
        }

        public Reserve GetReserve(int id)
        {
            return reserveReposityory.GetById(id);
        }

        public Reserve GetReserveByUserId(string isbn ,string userId)
        {
            return reserveReposityory.GetAll().FirstOrDefault(i => (i.Isbn == isbn && i.UserId == userId));
        }

        public IEnumerable<Reserve> GetAllReservesByUserId(string userId)
        {
            return reserveReposityory.GetUsersAllReservesByUserId(userId);
        }

        public IEnumerable<Reserve> GetBooksUserStillHave(string userId)
        {
            return reserveReposityory.GetUsersNotReturnedBooksByUserId(userId);
        }

        public void UpdateReserve(Reserve reserve)
        {
            reserveReposityory.Update(reserve);
        }
        public void AddReserve(Reserve reserve)
        {
            var isUserHasPesmission = reserveReposityory.GetUsersNotReturnedBooksByUserId(reserve.UserId).Count <= 3;
            if (isUserHasPesmission)
            {
                reserveReposityory.Add(reserve);
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
