using Caliburn.Micro;
using NgRMDesktopUserInterface.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NgRMDesktopUI.Library.Api;
using NgRMDesktopUserInterface.EventModels;

namespace NgRMDesktopUserInterface.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private IEventAggregator _events;
        
        private IAPIHelper _apiHelper;
        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }
        public string UserName
        {
            get { return _userName; }
            set { 
                _userName = value; 
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        //private bool _isErrorVsbl;

        public bool IsErrorVsbl
        {
            get {
                bool output = false;
                    if (ErrorMessage?.Length > 0)
                    {
                    output = true; }
                return output;
                }

            
        }


        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVsbl);
                 }
        }


        public string Password
        {
            get { return _password; }
            set {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool output = false;
                if (Password?.Length > 0 && UserName?.Length > 0)
                {
                    output = true;

                }
                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";
                var result = await _apiHelper.Authenticate(UserName, Password);

                //capture more information about user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                await _events.PublishOnUIThreadAsync(new LogOnEvent());
                
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }



    }
}
