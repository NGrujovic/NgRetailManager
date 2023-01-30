using NgRMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgRMDesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}