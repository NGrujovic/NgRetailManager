using NgRMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgRMDesktopUI.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<ApplicationUserModel>> GetAllUsers();
    }
}