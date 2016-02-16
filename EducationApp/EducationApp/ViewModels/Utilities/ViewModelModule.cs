using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace EducationApp.ViewModels.Utilities
{
    public class ViewModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.GetTypeInfo().IsSubclassOf(typeof (ViewModelBase)));
            builder.RegisterType<MainViewModel>().SingleInstance();
        }
    }
}