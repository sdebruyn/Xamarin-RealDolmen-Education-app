using Android.App;
using Android.Content.PM;
using Android.OS;
using EducationApp.Services;
using GalaSoft.MvvmLight.Views;
using JimBobBennett.MvvmLight.AppCompat;
using Microsoft.Practices.ServiceLocation;
using Xamarin;

namespace EducationApp.Droid.Activities
{
    [Activity(Theme = "@style/RDTheme.Splash", MainLauncher = true, NoHistory = true, Immersive = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public sealed class SplashActivity : AppCompatActivityBase
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            App.Initialize(ApplicationContext);
            Insights.Initialize(Configuration.InsightsKey, Application.Context);
            ServiceLocator.Current.GetInstance<ICategoryService>().FetchSpeculativelyAsync();
        }

        protected override void OnResume()
        {
            base.OnResume();
            ServiceLocator.Current.GetInstance<INavigationService>().NavigateTo(Constants.Pages.MainKey);
        }
    }
}