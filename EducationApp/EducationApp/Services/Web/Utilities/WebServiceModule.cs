using System;
using Autofac;

namespace EducationApp.Services.Web.Utilities
{
    public class WebServiceModule : Module
    {
        /// <exception cref="ArgumentNullException">Configuration of <see cref="Configuration.ApiUri" /> has an incorrect value.</exception>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace.EndsWith(nameof(Web))).AsImplementedInterfaces().SingleInstance();
        }
    }
}