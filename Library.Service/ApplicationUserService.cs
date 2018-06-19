using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data;
using Library.Data.Infastructure;
using Library.Model;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;

namespace Library.Service
{
    public interface IApplicationUserService
    {
    }
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IUnitOfWork unitOfWork;
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository, IUnitOfWork unitOfWork)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.unitOfWork = unitOfWork;
        }
    }

}
