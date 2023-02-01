using Caliburn.Micro;
using NgRMDesktopUI.Library.Api;
using NgRMDesktopUI.Library.Helpers;
using NgRMDesktopUI.Library.Models;
using NgRMDesktopUserInterface.Helpers;
using NgRMDesktopUserInterface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NgRMDesktopUserInterface
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }

        protected override void Configure()
        {
            //TODO:create class for container configuration
            _container.Instance(_container)
                .PerRequest<ISaleEndpoint,SaleEndpoint>()
                .PerRequest<IProductEndpoint, ProductEndpoint>();
                


            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IConfigHelper,ConfigHelper>()
                .Singleton<IAPIHelper, APIHelper>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));

            //Getting every type in app(at start of) where type is class and name ends with view model, put it to list,than go through every
            //class in there and go to container and registerPerRequest view models service name and implementation
            // in _container.RegistrationPerRequest service = interface, key=name, implementation=viewModel, i passed both to interface and to
            // implementation viewModel sinec i dont want to create interface just for that right now
            //This is called reflection
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
            
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }


        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
