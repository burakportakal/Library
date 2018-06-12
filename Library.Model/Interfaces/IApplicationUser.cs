using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Library.Model
{
    public interface IApplicationUser
    {
        Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType);
        string Email { get; set; }
        string UserName { get; set; }
        string Id { get; set; }
        string PasswordHash { get; set; }
    }
}