using System;

namespace NgRMDesktopUI.Library.Models
{
    public interface ILoggedInUserModel
    {
        string AuthUserId { get; set; }
        DateTime CreatedDate { get; set; }
        string EmailAdress { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Token { get; set; }
        void ResetUserModel();
    }
}