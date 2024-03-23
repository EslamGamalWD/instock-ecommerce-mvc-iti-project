using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Instock.Helper
{
    [HtmlTargetElement("li", Attributes = "active-when")]
    public class ActiveTage : TagHelper
    {
        public string? ActiveWhen { get; set; }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContextData { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ActiveWhen))
                return;

            var currentController = ViewContextData?.RouteData.Values["controller"]?.ToString();

            if (currentController != null && currentController.Equals(ActiveWhen, System.StringComparison.OrdinalIgnoreCase))
            {
                if (output.Attributes.ContainsName("class"))
                {
                    output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
                }
                else
                {
                    output.Attributes.SetAttribute("class", $"active");
                }
            }
        }
    }
}
