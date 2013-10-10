using System.Linq;
using System.Net.Http.Formatting;
using WebApiContrib.Formatting.Html.Formatters;

namespace WebApiContrib.Formatting.Html
{
    public static class Extensions
    {
        public static HtmlMediaTypeViewFormatter GetHtmlFormatter(this MediaTypeFormatterCollection formatters)
        {
            return formatters.OfType<HtmlMediaTypeViewFormatter>().SingleOrDefault();
        }
    }
}
