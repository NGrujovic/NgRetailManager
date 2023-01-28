using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgRMDataManager.Library.Models
{
    public class UserModel
    {
        public string AuthUserId { get; set; }

        public string FirstName { get; set; }

        public string  LastName { get; set; }
        public string EmailAdress { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
