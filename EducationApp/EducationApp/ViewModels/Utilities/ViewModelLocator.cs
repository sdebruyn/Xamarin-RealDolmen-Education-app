using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using EducationApp.Services.Default;
using EducationApp.Services.Fakes.Utilities;
using EducationApp.Services.Web.Utilities;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.ViewModels.Utilities
{
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    public class ViewModelLocator
    {
        private CategoryViewModel _categoryViewModel;
        private CourseViewModel _courseViewModel;
        private MainViewModel _mainViewModel;
        private SessionViewModel _sessionViewModel;
        private SubcategoryViewModel _subcategoryViewModel;

        static ViewModelLocator()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                RegisterServices(registerFakes: true);
            }
        }

        public CategoryViewModel CategoryViewModel
            => _categoryViewModel ?? (_categoryViewModel = ServiceLocator.Current.GetInstance<CategoryViewModel>());

        public SessionViewModel SessionViewModel
            => _sessionViewModel ?? (_sessionViewModel = ServiceLocator.Current.GetInstance<SessionViewModel>());

        public SubcategoryViewModel SubcategoryViewModel
            =>
                _subcategoryViewModel ??
                (_subcategoryViewModel = ServiceLocator.Current.GetInstance<SubcategoryViewModel>());

        public CourseViewModel CourseViewModel
            => _courseViewModel ?? (_courseViewModel = ServiceLocator.Current.GetInstance<CourseViewModel>());

        public MainViewModel MainViewModel
            => _mainViewModel ?? (_mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>());

        public static void RegisterServices(ContainerBuilder registrations = null, bool registerFakes = false)
        {
            var builder = new ContainerBuilder();

            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic || registerFakes)
            {
                builder.RegisterModule<FakeServiceModule>();
                builder.RegisterType<DebugLogger>().AsImplementedInterfaces();
                builder.RegisterType<FakePlatformActionService>().AsImplementedInterfaces();
            }
            else
            {
                builder.RegisterModule<WebServiceModule>();
                builder.RegisterType<XamarinLogger>().AsImplementedInterfaces();
                builder.RegisterType<PlatformActionService>().AsImplementedInterfaces();
            }
            builder.RegisterModule<ViewModelModule>();

            var container = builder.Build();
            registrations?.Update(container);

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }
    }
}