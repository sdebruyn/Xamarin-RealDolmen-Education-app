using System;
using Autofac;
using EducationApp.iOS.Services;
using EducationApp.iOS.ViewControllers;
using GalaSoft.MvvmLight.Views;
using NavigationService = EducationApp.iOS.Services.NavigationService;

namespace EducationApp.iOS.Utilities
{
    public class PlatformServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var nav = new NavigationService();

            nav.Configure(TabBarItemType.Courses, typeof (MainViewController), Constants.Pages.MainKey, true);
            nav.Configure(Constants.Pages.CategoryDetailsKey, typeof (CategoryViewController));
            nav.Configure(Constants.Pages.SubcategoryDetailsKey, typeof (SubcategoryViewController));
            nav.Configure(TabBarItemType.Profile, typeof (ProfileViewController));
            nav.Configure(Constants.Pages.CourseDetailsKey, typeof (CourseTabViewController));
            nav.Configure(TabBarItemType.Schedule, typeof (SessionViewController));
            nav.Configure(Constants.Pages.SessionKey, typeof (SessionViewController));
            nav.Configure(TabBarItemType.Contact, typeof (ContactViewController));
            nav.Configure(TabBarItemType.Feedback, typeof (FeedbackViewController));
            nav.Configure(TabBarItemType.Subscribe, typeof (SubscribeViewController));
            builder.RegisterInstance(nav).AsImplementedInterfaces().SingleInstance().AsSelf();

            builder.RegisterType<AuthenticationService>().AsImplementedInterfaces().SingleInstance();

            Type[] types =
            {
                typeof (DialogService),
                typeof (DispatcherHelper),
                typeof (LocalizationProvider)
            };
            builder.RegisterTypes(types).AsImplementedInterfaces();
        }
    }
}