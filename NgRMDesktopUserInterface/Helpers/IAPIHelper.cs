using NgRMDesktopUserInterface.Models;
using System.Threading.Tasks;

namespace NgRMDesktopUserInterface.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}