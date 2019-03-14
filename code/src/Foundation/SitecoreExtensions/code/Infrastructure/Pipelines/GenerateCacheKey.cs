namespace Assignment.Foundation.SitecoreExtensions.Infrastructure.Pipelines
{
    using Sitecore.Mvc.Pipelines.Response.RenderRendering;
    using Sitecore.Mvc.Presentation;
    using System;

    public class GenerateCacheKey : Sitecore.Mvc.Pipelines.Response.RenderRendering.GenerateCacheKey
    {
        protected override string GenerateKey(Rendering rendering, RenderRenderingArgs args)
        {
            var cacheKey = base.GenerateKey(rendering, args);
            var caching = rendering.Caching;
            
            if (caching.VaryByData && rendering.RenderingItem.ID == Templates.RelatedNewsRendering.ID)
            {
                var renderingDataSource = args.Rendering.DataSource;
                if (string.IsNullOrEmpty(renderingDataSource))
                {
                    cacheKey += args.Rendering.Item.ID;
                }
            }

            return cacheKey;
        }

    }
}