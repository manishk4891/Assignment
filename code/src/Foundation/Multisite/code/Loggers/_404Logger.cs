using log4net;
using Sitecore.Diagnostics;

namespace Assignment.Foundation.Multisite.Loggers
{
    public static class _404Logger
    {
        public static ILog Log => LogManager.GetLogger("Assignment.Foundation.Multisite._404Logger") ?? LoggerFactory.GetLogger(typeof(_404Logger));
    }
}