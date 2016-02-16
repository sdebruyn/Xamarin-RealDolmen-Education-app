using System;
using Autofac;
using EducationApp.WinPhone.Pages;
using EducationApp.WinPhone.Services;
using GalaSoft.MvvmLight.Views;
using static EducationApp.Constants.Pages;

namespace EducationApp.WinPhone.Utilities
{
    public sealed class PlatformServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var nav = new NavigationService();

            nav.Configure(CategoryDetailsKey, typeof (CategoryPage));
            nav.Configure(SubcategoryDetailsKey, typeof (SubcategoryPage));
            nav.Configure(MainKey, typeof (MainPage));
            nav.Configure(CourseDetailsKey, typeof (CoursePage));
            nav.Configure(SessionsKey, typeof (SessionsPage));
            nav.Configure(SessionKey, typeof (SessionPage));

            builder.RegisterInstance(nav).AsImplementedInterfaces().SingleInstance();


            Type[] types =
            {
                typeof (DialogService),
                typeof (AuthenticationService),
                typeof (LocalizationProvider),
                typeof (DispatcherHelper)
            };
            builder.RegisterTypes(types).AsImplementedInterfaces();
        }
    }
}