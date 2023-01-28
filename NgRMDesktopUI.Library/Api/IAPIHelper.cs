
using NgRMDesktopUserInterface.Models;
using System.Threading.Tasks;

namespace NgRMDesktopUI.Library.Api
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}