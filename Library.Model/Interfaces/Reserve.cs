using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Reserve : IReserve
    {
        public int ReserveId { get; set; }
        public string UserId { get; set; }
        public string Isbn { get; set; }
        public DateTime DateReserved { get; set; }
        public DateTime CalculatedReturnDate { get; set; }
        public DateTime? UserReturnedDate { get; set; }
        public virtual Books Books { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Reserve()
        {
            DateReserved = DateTime.Now;
            CalculatedReturnDate = DateReserved.AddDays(7);
        }
    }
}
