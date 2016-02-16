using System;
using Autofac;
using EducationApp.Droid.Activities;
using EducationApp.Droid.Services;
using JimBobBennett.MvvmLight.AppCompat;

namespace EducationApp.Droid.Utilities
{
    public class PlatformServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var nav = new AppCompatNavigationService();

            nav.Configure(Constants.Pages.CategoryDetailsKey, typeof (CategoryActivity));
            nav.Configure(Constants.Pages.MainKey, typeof (MainActivity));
            nav.Configure(Constants.Pages.SubcategoryDetailsKey, typeof (SubcategoryActivity));
            nav.Configure(Constants.Pages.CourseDetailsKey, typeof (CourseActivity));
            nav.Configure(Constants.Pages.SessionKey, typeof (SessionActivity));

            builder.RegisterInstance(nav).AsImplementedInterfaces().AsSelf().SingleInstance();

            Type[] types =
            {
                typeof (AppCompatDialogService),
                typeof (LocalizationProvider),
                typeof (DispatcherHelper)
            };
            builder.RegisterTypes(types).AsImplementedInterfaces();
            builder.RegisterType<AuthenticationService>().AsImplementedInterfaces().SingleInstance();
        }
    }
}