using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using NgRMDesktopUI.Library.Api;
using NgRMDesktopUI.Library.Models;
using NgRMDesktopUserInterface.EventModels;

namespace NgRMDesktopUserInterface.ViewModels
{
    public class ShellViewModel:Conductor<object>, IHandle<LogOnEvent>
    {
        
        private IEventAggregator _events;

        
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;
      
        public ShellViewModel(IEventAggregator events,ILoggedInUserModel user, IAPIHelper apiHelper)
        {
            
            _events = events;

            _user = user;
            // Subscribing shell view to events, telling it to start listening for specific event
            _events.SubscribeOnPublishedThread(this);
            _apiHelper = apiHelper;

            ActivateItemAsync(IoC.Get<LoginViewModel>());

        }
        
        
        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (string.IsNullOrEmpty(_user.Token) == false)
                {
                    output = true;
                }
                return output;
            }
        }
        
        public void ExitApplication()
        {
            TryCloseAsync();
            
        }
        public async Task LogOut()
        {
            //Clears loggedInUserModel
            _user.ResetUserModel();

            //Clears token that is in header of api
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);

        }
        public async Task UserManagment()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
