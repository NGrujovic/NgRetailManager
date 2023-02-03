﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using NgRMDesktopUI.Library.Models;
using NgRMDesktopUserInterface.EventModels;

namespace NgRMDesktopUserInterface.ViewModels
{
    public class ShellViewModel:Conductor<object>, IHandle<LogOnEvent>
    {
        
        private IEventAggregator _events;
        private SalesViewModel _salesVm;
        private ILoggedInUserModel _user;
      
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVm,ILoggedInUserModel user)
        {
            
            _events = events;
            _salesVm = salesVm;
            _user = user;
            // Subscribing shell view to events, telling it to start listening for specific event
            _events.Subscribe(this);


            ActivateItem(IoC.Get<LoginViewModel>());

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
        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVm);
            NotifyOfPropertyChange(() => IsLoggedIn);
            
        }
        public void ExitApplication()
        {
            TryClose();
        }
        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);

        }
    }
}
