using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Instock.Helper
{
    [HtmlTargetElement("a", Attributes = "active-when")]
    public class ActiveTagHelper : TagHelper
    {
        public string? ActiveWhen { get; set; }
        
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContextData { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ActiveWhen) || ViewContextData == null)
                return;
            
            var currentController = ViewContextData.RouteData.Values["controller"]?.ToString();
            
            if (currentController != null && string.Equals(currentController, ActiveWhen, System.StringComparison.OrdinalIgnoreCase))
            {
                if (output.Attributes.ContainsName("class"))
                {
                    var existingClasses = output.Attributes["class"].Value.ToString();
                    var newClasses = $"{existingClasses.Trim()} active";
                    output.Attributes.SetAttribute("class", newClasses);
                }
                else
                {
                    output.Attributes.SetAttribute("class", "active");
                }
            }
        }
    }
}
