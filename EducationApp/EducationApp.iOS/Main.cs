using Autofac;
using EducationApp.iOS.Utilities;
using EducationApp.Services;
using EducationApp.ViewModels.Utilities;
using Microsoft.Practices.ServiceLocation;
using UIKit;
using Xamarin;

namespace EducationApp.iOS
{
    public class Application
    {
        private static ViewModelLocator _locator;
        private static readonly object LockObject = new object();

        public static ViewModelLocator Locator
        {
            get
            {
                if (_locator == null)
                {
                    lock (LockObject)
                    {
                        RegisterServices();
                        _locator = new ViewModelLocator();
                    }
                }
                return _locator;
            }
        }

        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            Insights.Initialize(Configuration.InsightsKey);
            var locator = Locator; // register services
            ServiceLocator.Current.GetInstance<ICategoryService>().FetchSpeculativelyAsync();
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }

        private static void RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<PlatformServicesModule>();
            ViewModelLocator.RegisterServices(builder);
        }
    }
}