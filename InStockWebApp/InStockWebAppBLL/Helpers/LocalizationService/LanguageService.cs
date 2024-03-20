using Microsoft.Extensions.Localization;
using System.Reflection;

namespace InStockWebAppBLL.Helpers.LocalizationService
{
    /// <summary>
    /// Dummy class to group shared resources
    /// </summary>
    public class SharedResource { }
    public class LanguageService
    {
        private readonly IStringLocalizer _localizer;
        public LanguageService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name); // §REVIEW_DJE: "SharedResource" or "ShareResource"
        }
        public LocalizedString Getkey(string key)
        {
            return _localizer[key];
        }
    }
}
