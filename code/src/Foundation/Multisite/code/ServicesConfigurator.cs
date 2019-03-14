namespace Assignment.Foundation.Multisite
{
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Sitecore.Abstractions;
    using Sitecore.DependencyInjection;
    using Assignment.Foundation.Multisite.Placeholders;

    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Singleton<BasePlaceholderCacheManager, SiteSpecificPlaceholderCacheManager>());
        }
    }
}