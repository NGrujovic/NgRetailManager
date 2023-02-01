using NgRMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace NgRMDesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}