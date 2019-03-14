using Assignment.Foundation.Dictionary.Models;
using Sitecore.Sites;

namespace Assignment.Foundation.Dictionary.Repositories
{
  public interface IDictionaryRepository
  {
    Models.Dictionary Get(SiteContext site);
  }
}