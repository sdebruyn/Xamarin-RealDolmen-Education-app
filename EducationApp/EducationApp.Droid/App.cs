using Android.Content;
using Autofac;
using EducationApp.Droid.Utilities;
using EducationApp.ViewModels.Utilities;

namespace EducationApp.Droid
{
    public sealed class App
    {
        private static readonly object LockObject = new object();

        public static ViewModelLocator Locator { get; private set; }

        public static void Initialize(Context context)
        {
            lock (LockObject)
            {
                if (Locator == null)
                {
                    var builder = new ContainerBuilder();
                    builder.RegisterInstance(context);
                    builder.RegisterModule<PlatformServicesModule>();
                    ViewModelLocator.RegisterServices(builder);
                    Locator = new ViewModelLocator();
                }
            }
        }
    }
}