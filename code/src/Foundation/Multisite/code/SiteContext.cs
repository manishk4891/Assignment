namespace Assignment.Foundation.Multisite
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Assignment.Foundation.Multisite.Providers;
  using Assignment.Foundation.Multisite;
    using Sitecore;
  public class SiteContext
  {
    private readonly ISiteDefinitionsProvider siteDefinitionsProvider;

    public SiteContext() : this(new SiteDefinitionsProvider())
    {    
    }

    public SiteContext(ISiteDefinitionsProvider siteDefinitionsProvider)
    {
      this.siteDefinitionsProvider = siteDefinitionsProvider;
    }

    public virtual SiteDefinition GetSiteDefinition([NotNull]Item item)
    {
      Assert.ArgumentNotNull(item, nameof(item));

      return this.siteDefinitionsProvider.GetContextSiteDefinition(item);
    }
  }
}