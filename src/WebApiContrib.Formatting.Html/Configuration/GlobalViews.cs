using System;
using System.Collections.Generic;
using WebApiContrib.Formatting.Html.Locators;
using WebApiContrib.Formatting.Html.ViewParsers;

namespace WebApiContrib.Formatting.Html.Configuration
{
    using WebApiContrib.Formatting.Html.Formatters;

    /// <summary>
    /// Global configuration of <see cref="HtmlMediaTypeViewFormatter"/> instances.
    /// </summary>
    public static class GlobalViews
    {
        static GlobalViews()
        {
            Views = new Dictionary<Type, string>();
        }

        /// <summary>
        /// User-defined mappings from model types to view names.
        /// </summary>
        public static IDictionary<Type, string> Views { get; private set; }

        /// <summary>
        /// Default <see cref="IViewLocator"/> to be used for retrieval of view templates.
        /// </summary>
        public static IViewLocator DefaultViewLocator { get; set; }

        /// <summary>
        /// Default <see cref="IViewParser"/> to be used for generation of views from view templates.
        /// </summary>
        public static IViewParser DefaultViewParser { get; set; }
    }
}
