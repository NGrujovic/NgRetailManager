using Caliburn.Micro;
using NgRMDesktopUI.Library.Api;
using NgRMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NgRMDesktopUserInterface.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        private readonly IUserEndpoint _userEndpoint;
        private BindingList<ApplicationUserModel> _users; 

        public BindingList<ApplicationUserModel> Users { get
            {
                return _users;
            }

            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }
        public UserDisplayViewModel(StatusInfoViewModel status,IWindowManager window,IUserEndpoint userEndpoint)
        {
            _status = status;
            _window = window;
            _userEndpoint = userEndpoint;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                //Settings of message box that i show

                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMod = ResizeMode.NoResize;
                settings.Title = "System Error";


                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unauthorized Access", "You do not have premission to interact with the sales form.");
                    _window.ShowDialog(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    _window.ShowDialog(_status, null, settings);
                }

                TryClose();

            }
        }

        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAllUsers();

            Users = new BindingList<ApplicationUserModel>(userList);
        }
    }
}
