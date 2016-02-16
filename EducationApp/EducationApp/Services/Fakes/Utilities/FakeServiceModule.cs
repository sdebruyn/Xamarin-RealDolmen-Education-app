using Autofac;

namespace EducationApp.Services.Fakes.Utilities
{
    public class FakeServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(
                    t =>
                        t.Namespace.EndsWith(nameof(Fakes)))
                .AsImplementedInterfaces();
        }
    }
}