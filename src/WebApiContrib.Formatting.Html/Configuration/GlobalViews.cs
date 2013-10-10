using System;
using System.Collections.Generic;
using WebApiContrib.Formatting.Html.Formatting;

namespace WebApiContrib.Formatting.Html
{
    /// <summary>
    /// Global configuration of <see cref="HtmlMediaTypeViewFormatter"/> instances.
    /// </summary>
    public static class GlobalViews
    {
        private static readonly IDictionary<Type, string> _views = new Dictionary<Type, string>();

        /// <summary>
        /// User-defined mappings from model types to view names.
        /// </summary>
        public static IDictionary<Type, string> Views { get { return _views; } }

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
