namespace Assignment.Feature.News.Infrastructure.Pipelines
{
    using System.Web.Routing;
    using Assignment.Feature.News;
    using Assignment.Foundation.DependencyInjection;
    using Sitecore.Pipelines;
    using Sitecore;

    [Service]
    public class InitializeRoutes
    {
        public void Process(PipelineArgs args)
        {
            if (!Context.IsUnitTesting)
            {
                RouteConfig.RegisterRoutes(RouteTable.Routes);
            }
        }
    }
}