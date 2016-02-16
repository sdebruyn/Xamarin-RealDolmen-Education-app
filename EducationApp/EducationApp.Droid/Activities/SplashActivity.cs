using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using EducationApp.Services;
using JimBobBennett.MvvmLight.AppCompat;
using Microsoft.Practices.ServiceLocation;
using Xamarin;

namespace EducationApp.Droid.Activities
{
    [Activity(Theme = "@style/RDTheme.Splash", MainLauncher = true, NoHistory = true, Immersive = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivityBase
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var startupWork = new Task(() =>
            {
                // initialize everything here
                App.Initialize(ApplicationContext);
                Insights.Initialize(Configuration.InsightsKey, Application.Context);
                ServiceLocator.Current.GetInstance<ICategoryService>().FetchSpeculativelyAsync();
            });

            startupWork.ContinueWith(t => StartActivity(new Intent(Application.Context, typeof (MainActivity))),
                TaskScheduler.FromCurrentSynchronizationContext());
            startupWork.Start();
        }
    }
}