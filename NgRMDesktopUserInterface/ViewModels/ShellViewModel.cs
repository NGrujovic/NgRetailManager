using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using NgRMDesktopUserInterface.EventModels;

namespace NgRMDesktopUserInterface.ViewModels
{
    public class ShellViewModel:Conductor<object>, IHandle<LogOnEvent>
    {
        
        private IEventAggregator _events;
        private SalesViewModel _salesVm;
        private SimpleContainer _container;
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVm,
            SimpleContainer container)
        {
            
            _events = events;
            _salesVm = salesVm;
            _container = container;
            // Subscribing shell view to events, telling it to start listening for specific event
            _events.Subscribe(this);


            ActivateItem(_container.GetInstance<LoginViewModel>());

        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVm);
            
        }
    }
}
