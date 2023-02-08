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
        private string _selectedUserName;
        private ApplicationUserModel applicationUserModel;
        private BindingList<string> _userRoles = new BindingList<string>();
        private BindingList<string> _avaliableRoles = new BindingList<string>();
        private string _selectedAvaliableRole;
        private string _selectedUserRole;



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
        public UserDisplayViewModel(StatusInfoViewModel status, IWindowManager window, IUserEndpoint userEndpoint)
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
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    await _window.ShowDialogAsync(_status, null, settings);
                }

                TryCloseAsync();

            }
        }
       

        public ApplicationUserModel SelectedUser
        {
            get { return applicationUserModel; }
            set {
                applicationUserModel = value;
                SelectedUserName = value.Email;
                UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => CanRemoveSelectedRole);
                NotifyOfPropertyChange(() => CanRemoveSelectedRole);
                LoadRoles();
                //Load Roles should be avaited but for now i left it like this 
                // it works but it will be fixed later for better usage

            }
        }

       
        

        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);

                
            }
        }
        //SelectedRoleToRemove
        
        

        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set { 
                _selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
                NotifyOfPropertyChange(() => CanRemoveSelectedRole);
            }
        }

        

        public string SelectedAvaliableRole
        {
            get { return _selectedAvaliableRole; }
            set
            {
                _selectedAvaliableRole = value;
                NotifyOfPropertyChange(() => SelectedAvaliableRole);
                NotifyOfPropertyChange(() => CanAddSelectedRole);
            }
        }

        public BindingList<string> UserRoles
        {
            get { return _userRoles; }
            set {
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
                NotifyOfPropertyChange(() => CanRemoveSelectedRole);
                NotifyOfPropertyChange(() => CanAddSelectedRole);

            }
        }

        

        public BindingList<string> AvaliableRoles
        {
            get { return _avaliableRoles; }
            set
            {
                _avaliableRoles = value;
                NotifyOfPropertyChange(() => AvaliableRoles);

            }
        }


        private async Task LoadRoles()
        {
            var roles = await _userEndpoint.GetAllRoles();
            foreach(var role in roles)
            {
                if (UserRoles.IndexOf(role.Value) < 0)
                {
                    AvaliableRoles.Add(role.Value);
                }
            }
        }
        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAllUsers();

            Users = new BindingList<ApplicationUserModel>(userList);
        }
        

        public bool CanRemoveSelectedRole
        {
            get {
                bool output = false;
                if(SelectedUserRole != null && SelectedUser != null)
                {
                    output = true;
                }
                return output; }
            
        }

        public bool CanAddSelectedRole
        {
            get
            {
                bool output = false;
                if (SelectedAvaliableRole != null && SelectedUser != null)
                {
                    output = true;
                }
                return output;
            }

        }

        public async Task AddSelectedRole()
        {
            try
            {
                await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvaliableRole);

                UserRoles.Add(SelectedAvaliableRole);
                AvaliableRoles.Remove(SelectedAvaliableRole);

            }
            catch (Exception)
            {
                //TODO:Message box for not successfull adding

                throw;
            }

            
        }


        public async Task RemoveSelectedRole()
        {
            try
            {
                await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);

                AvaliableRoles.Add(SelectedUserRole);
                UserRoles.Remove(SelectedUserRole);
                
            }
            catch (Exception)
            {
                //TODO:Message box for not successfull removing
                throw;
            }
        }

    }
}
