using Sitecore.Data;
using Assignment.Foundation.Dictionary.Repositories;

namespace Assignment.Foundation.Alerts
{
  public static class AlertTexts
  {
    public static string InvalidDataSourceTemplate(ID templateId) => string.Format(DictionaryPhraseRepository.Current.Get(ErrorDictionaryPaths.InvalidDatasource, "Data source isn't set or has wrong template. Template {0} is required"), templateId);

    public static string InvalidDataSourceTemplateFriendlyMessage => DictionaryPhraseRepository.Current.Get(ErrorDictionaryPaths.InvalidDatasourceTemplate, "There was a problem with the associated content item, please associate the correct content item with the component");

    public static string InvalidDataSource => DictionaryPhraseRepository.Current.Get(ErrorDictionaryPaths.InvalidDatasourceUnknownTemplate, "Data source isn't set or has wrong template");


  }
}