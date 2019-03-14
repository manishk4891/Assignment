namespace Assignment.Foundation.Multisite.Providers
{
    using System.Linq;
    using Sitecore.Configuration;
    using Sitecore.Data.Items;
    using Assignment.Foundation.Multisite.Providers;
    using Assignment.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Web;

    public class SiteSettingsProvider : ISiteSettingsProvider
    {
        private readonly SiteContext siteContext;

        public SiteSettingsProvider() : this(new SiteContext())
        {
        }

        public SiteSettingsProvider(SiteContext siteContext)
        {
            this.siteContext = siteContext;
        }

        public static string SettingsRootName => Settings.GetSetting("Multisite.SettingsRootName", "settings");
        public static string GlobalFolderName => Settings.GetSetting("Multisite.GlobalFolder", "global");

        public virtual Item GetSetting(Item contextItem, string settingsType, string setting)
        {
            var settingsRootItem = this.GetSettingsRoot(contextItem, settingsType);
            var settingItem = settingsRootItem?.Children.FirstOrDefault(i => i.Key.Equals(setting.ToLower()));
            return settingItem;
        }

        private Item GetSettingsRoot(Item contextItem, string settingsName)
        {
            var currentDefinition = this.siteContext.GetSiteDefinition(contextItem);
            if (currentDefinition?.Item == null)
            {
                return null;
            }

            var definitionItem = currentDefinition.Item;
            var settingsFolder = definitionItem.Children[SettingsRootName];
            var settingsRootItem = settingsFolder?.Children.FirstOrDefault(i => i.IsDerived(Templates.SiteSettings.ID) && i.Key.Equals(settingsName.ToLower()));
            return settingsRootItem;
        }

        /// <summary>
        /// Get Site Settings Root item e.g. /sitecore/content/Assignment/Settings
        /// </summary>
        /// <param name="contextItem"></param>
        /// <returns></returns>
        public Item GetWebsiteSettingsRoot(Item contextItem)
        {
            var currentDefinition = this.siteContext.GetSiteDefinition(contextItem);
            if (currentDefinition?.Item == null)
            {
                return null;
            }

            var definitionItem = currentDefinition.Item;
            var settingsFolder = definitionItem.Children[SettingsRootName];            
            return settingsFolder;
        }

        /// <summary>
        /// Get Site Global Folder item e.g. /sitecore/content/Assignment/Global
        /// </summary>
        /// <param name="contextItem"></param>
        /// <returns></returns>
        public Item GetWebsiteGlobalFolder(Item contextItem)
        {
            var currentDefinition = this.siteContext.GetSiteDefinition(contextItem);
            if (currentDefinition?.Item == null)
            {
                return null;
            }

            var definitionItem = currentDefinition.Item;
            var globalFolder = definitionItem.Children[GlobalFolderName];
            return globalFolder;
        }

    }
}