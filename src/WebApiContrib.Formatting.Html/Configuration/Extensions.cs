using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using WebApiContrib.Formatting.Html.Formatting;

namespace WebApiContrib.Formatting.Html
{
    /// <summary>
    /// Configuration extensions for rendering HTML responses.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Retrieves the <see cref="MediaTypeFormatter"/> registered to handle HTML.
        /// </summary>
        /// <param name="formatters">The <see cref="MediaTypeFormatterCollection"/>.</param>
        /// <returns>The <see cref="HtmlMediaTypeFormatter"/> registered to handle requests for HTML.</returns>
        public static HtmlMediaTypeViewFormatter GetHtmlFormatter(this MediaTypeFormatterCollection formatters)
        {
            return formatters.OfType<HtmlMediaTypeViewFormatter>().SingleOrDefault();
        }

        /// <summary>
        /// Creates a <see cref="ViewResult"/> using the current <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="requestMessage">The <see cref="HttpRequestMessage"/>.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <param name="model">The model to render into the view.</param>
        /// <returns>The <see cref="ViewResult"/>.</returns>
        public static IHttpActionResult View(this HttpRequestMessage requestMessage, string viewName, object model)
        {
            return new ViewResult(requestMessage, viewName, model);
        }

        /// <summary>
        /// Creates a <see cref="ViewResult"/> using the current <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="requestMessage">The <see cref="HttpRequestMessage"/>.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <param name="model">The model to render into the view.</param>
        /// <param name="modelType">The type of the <paramref name="model"/>.</param>
        /// <returns>The <see cref="ViewResult"/>.</returns>
        public static IHttpActionResult View(this HttpRequestMessage requestMessage, string viewName, object model, Type modelType)
        {
            return new ViewResult(requestMessage, viewName, model, modelType);
        }

        /// <summary>
        /// Creates a <see cref="ViewResult"/> using the current <see cref="ApiController"/>.
        /// </summary>
        /// <param name="controller">The <see cref="ApiController"/>.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <param name="model">The model to render into the view.</param>
        /// <returns>The <see cref="ViewResult"/>.</returns>
        public static IHttpActionResult View(this ApiController controller, string viewName, object model)
        {
            return new ViewResult(controller.Request, viewName, model);
        }

        /// <summary>
        /// Creates a <see cref="ViewResult"/> using the current <see cref="ApiController"/>.
        /// </summary>
        /// <param name="controller">The <see cref="ApiController"/>.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <param name="model">The model to render into the view.</param>
        /// <param name="modelType">The type of the <paramref name="model"/>.</param>
        /// <returns>The <see cref="ViewResult"/>.</returns>
        public static IHttpActionResult View(this ApiController controller, string viewName, object model, Type modelType)
        {
            return new ViewResult(controller.Request, viewName, model, modelType);
        }
    }
}
