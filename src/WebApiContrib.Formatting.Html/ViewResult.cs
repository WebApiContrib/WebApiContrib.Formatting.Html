using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiContrib.Formatting.Html.Formatters;

namespace WebApiContrib.Formatting.Html
{
    /// <summary>
    /// Represents default implementation of <see cref="IView"/> interface.
    /// </summary>
    public class ViewResult : IView, IHttpActionResult
    {
        private readonly HttpRequestMessage _requestMessage;
        private readonly string _viewName;
        private readonly object _model;
        private readonly Type _modelType;

        /// <summary>
        /// Creates a new <see cref="ViewResult"/> instance.
        /// </summary>
        /// <param name="request">The current request message.</param>
        /// <param name="viewName">The view name, used to resolve the view template definition.</param>
        /// <param name="model">The data to be presented by the view.</param>
        public ViewResult(HttpRequestMessage request, string viewName, object model) :
            this(request, viewName, model, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ViewResult"/> instance.
        /// </summary>
        /// <param name="request">The current request message.</param>
        /// <param name="viewName">The view name, used to resolve the view template definition.</param>
        /// <param name="model">The data to be presented by the view.</param>
        /// <param name="modelType">Optional explicit definition of data type for <see cref="Model"/>. 
        /// </param>
        /// <exception cref="ArgumentException"><paramref name="modelType"/> must be public.</exception>
        public ViewResult(HttpRequestMessage request, string viewName, object model, Type modelType)
        {
            _requestMessage = request;
            _model = model;
            _viewName = viewName;
            _modelType = modelType ?? (model != null ? model.GetType() : null);
        }

        /// <summary>
        /// The current request message.
        /// </summary>
        public HttpRequestMessage RequestMessage { get { return _requestMessage; } }

        /// <summary>
        /// The data to be presented by the view.
        /// </summary>
        public object Model { get { return _model; } }

        /// <summary>
        /// The view name, used to resolve the view template definition.
        /// </summary>
        public string ViewName { get { return _viewName; } }

        /// <summary>
        /// Optional explicit definition of data type for <see cref="Model"/>. When specified,
        /// this type must be public and have a type name that matches the C# and VB language
        /// rules for identifiers. It should not be set if the model is an anonymous type or
        /// a compiler-generated iterator (enumerable or enumerator) type.
        /// </summary>
        public Type ModelType { get { return _modelType; } }

        /// <summary>
        /// Executes this <see cref="IHttpActionResult"/> to retrieve a <see cref="HttpResponseMessage"/> from the view data.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> representing the view data.</returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var config = RequestMessage.GetConfiguration();
            var response = RequestMessage.CreateResponse(HttpStatusCode.OK, this, config.Formatters.GetHtmlFormatter());
            return Task.FromResult(response);
        }
    }
}
