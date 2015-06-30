using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiContrib.Formatting.Html
{
    /// <summary>
    /// Represents an abstract view.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// The data to be presented by the view.
        /// </summary>
        object Model { get; }

        /// <summary>
        /// Optional explicit definition of data type for <see cref="Model"/>. When specified,
        /// this type must be public and have a type name that matches the C# and VB language
        /// rules for identifiers. It should not be set if the model is an anonymous type or
        /// a compiler-generated iterator (enumerable or enumerator) type.
        /// </summary>
        /// <remarks>
        /// This property can be used by view engines to create strong-typed view templates. 
        /// </remarks>
        Type ModelType { get; }

        /// <summary>
        /// The view name, used to resolve the view template definition.
        /// </summary>
        string ViewName { get; }
    }

    /// <summary>
    /// Interface for view locators. A view locator loads unparsed view templates 
    /// for abstract views.
    /// </summary>
    public interface IViewLocator
    {
        /// <summary>
        /// Retrieves unparsed view template for an abstract view definition.
        /// </summary>
        /// <param name="siteRootPath">Physical or virtual root path for resolution of view templates.</param>
        /// <param name="view">An abstract view definition.</param>
        /// <returns>The unparsed view template for <paramref name="view"/>.</returns>
        string GetView(string siteRootPath, IView view);
    }

    /// <summary>
    /// Interface for view template parsers. A view template 
    /// parser transforms abstract views into concrete views
    /// based using view templates.
    /// </summary>
    public interface IViewParser
    {
        /// <summary>
        /// Generates concrete view for a given abstract view.
        /// </summary>
        /// <param name="view">The abstract view.</param>
        /// <param name="viewTemplate">The view template to be used to generate the concrete view.</param>
        /// <param name="encoding">The text encoding of the concrete view.</param>
        /// <returns>Byte representation of the generated concrete view.</returns>
        byte[] ParseView(IView view, string viewTemplate, Encoding encoding);
    }

    /// <summary>
    /// Attribute to override the default view name for a model type. By default the
    /// view name is assumed to equal the model class name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ViewAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="ViewAttribute"/> instance.
        /// </summary>
        /// <param name="viewName">The view name, used to resolve view template definitions.</param>
        public ViewAttribute(string viewName)
        {
            if (string.IsNullOrWhiteSpace(viewName))
                throw new ArgumentException("Argument viewName can't be null or empty", "viewName");

            ViewName = viewName;
        }

        /// <summary>
        /// The view name, used to resolve view template definitions.
        /// </summary>
        public string ViewName { get; private set; }
    }

    namespace Formatting
    {
        /// <summary>
        /// A <see cref="MediaTypeFormatter"/> that produces HTML content.
        /// </summary>
        public class HtmlMediaTypeViewFormatter : MediaTypeFormatter
        {
            private readonly string _siteRootPath;
            private readonly IViewLocator _viewLocator;
            private readonly IViewParser _viewParser;

            /// <summary>
            /// Creates a new <see cref="HtmlMediaTypeViewFormatter"/> instance.
            /// </summary>
            /// <param name="siteRootPath">Physical or virtual root path for resolution of view templates.</param>
            /// <param name="viewLocator">The view locator to use for retrieval of view templates.</param>
            /// <param name="viewParser">The view parser to use for generation of views from view templates.</param>
            public HtmlMediaTypeViewFormatter(string siteRootPath = null, IViewLocator viewLocator = null, IViewParser viewParser = null)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xhtml"));
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xhtml+xml"));

                SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
                SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));

                _viewLocator = viewLocator;
                _viewParser = viewParser;
                _siteRootPath = siteRootPath;
            }

            /// <summary>
            /// Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/> can serializean object of the specified type.
            /// </summary>
            /// <param name="type">The type to serialize.</param>
            /// <returns>
            /// true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/> can serialize the type; otherwise, false.
            /// </returns>
            public override bool CanWriteType(Type type)
            {
                return true;
            }

            /// <summary>
            /// Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/> can deserializean object of the specified type.
            /// </summary>
            /// <param name="type">The type to deserialize.</param>
            /// <returns>
            /// true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/> can deserialize the type; otherwise, false.
            /// </returns>
            public override bool CanReadType(Type type)
            {
                return false;
            }

            /// <summary>
            /// Asynchronously deserializes an object of the specified type.
            /// </summary>
            /// <param name="type">The type of the object to deserialize.</param>
            /// <param name="readStream">The <see cref="T:System.IO.Stream"/> to read.</param>
            /// <param name="content">The <see cref="T:System.Net.Http.HttpContent"/>, if available. It may be null.</param>
            /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger"/> to log events to.</param>
            /// <returns>
            /// A <see cref="T:System.Threading.Tasks.Task"/> whose result will be an object of the given type.
            /// </returns>
            /// <exception cref="T:System.NotSupportedException">Derived types need to support reading.</exception>
            public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Returns a specialized instance of the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/> that can format a response for the given parameters.
            /// </summary>
            /// <param name="type">The type to format.</param>
            /// <param name="request">The request.</param>
            /// <param name="mediaType">The media type.</param>
            /// <returns>
            /// Returns <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter"/>.
            /// </returns>
            public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
            {
                if (_viewLocator == null || _viewParser == null)
                {
                    var config = request.GetConfiguration();
                    if (config != null)
                    {
                        var resolver = config.DependencyResolver;
                        var viewLocator = _viewLocator ?? (IViewLocator)resolver.GetService(typeof(IViewLocator));
                        var viewParser = _viewParser ?? (IViewParser)resolver.GetService(typeof(IViewParser));
                        return new HtmlMediaTypeViewFormatter(_siteRootPath, viewLocator, viewParser);
                    }
                }

                return base.GetPerRequestFormatterInstance(type, request, mediaType);
            }

            /// <summary>
            /// Asynchronously writes an object of the specified type.
            /// </summary>
            /// <param name="type">The type of the object to write.</param>
            /// <param name="value">The object value to write.  It may be null.</param>
            /// <param name="writeStream">The <see cref="T:System.IO.Stream"/> to which to write.</param>
            /// <param name="content">The <see cref="T:System.Net.Http.HttpContent"/> if available. It may be null.</param>
            /// <param name="transportContext">The <see cref="T:System.Net.TransportContext"/> if available. It may be null.</param>
            /// <returns>
            /// A <see cref="T:System.Threading.Tasks.Task"/> that will perform the write.
            /// </returns>
            /// <exception cref="T:System.NotSupportedException">Derived types need to support writing.</exception>
            public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
            {
                var encoding = SelectCharacterEncoding(content.Headers);
                var parsedView = ParseView(type, value, encoding);

                writeStream.Write(parsedView, 0, parsedView.Length);
                writeStream.Flush();

                return Task.FromResult<object>(null);
            }

            private byte[] ParseView(Type type, object model, Encoding encoding)
            {
                var view = GetView(model, type);
                var viewTemplate = ViewLocator.GetView(_siteRootPath, view);
                return ViewParser.ParseView(view, viewTemplate, encoding);
            }

            private IViewLocator ViewLocator
            {
                get
                {
                    if (_viewLocator != null)
                        return _viewLocator;

                    if (GlobalViews.DefaultViewLocator != null)
                        return GlobalViews.DefaultViewLocator;

                    throw new ConfigurationErrorsException("No ViewLocator is specified");
                }
            }

            private IViewParser ViewParser
            {
                get
                {
                    if (_viewParser != null)
                        return _viewParser;

                    if (GlobalViews.DefaultViewParser != null)
                        return GlobalViews.DefaultViewParser;

                    throw new ConfigurationErrorsException("No ViewParser is specified");
                }
            }

            private static IView GetView(object model, Type type)
            {
                if (model is IView)
                    return model as IView;

                var modelType = model != null ? model.GetType() : type;

                string viewName = modelType.GetCustomAttributes(typeof(ViewAttribute), true)
                    .Cast<ViewAttribute>()
                    .Select(a => a.ViewName)
                    .FirstOrDefault();

                if (viewName == null)
                    viewName = GlobalViews.Views.ContainsKey(modelType) ?
                               GlobalViews.Views[modelType] :
                               modelType.Name;

                return new View(viewName, model, modelType);
            }

            /// <summary>
            /// Represents default implementation of <see cref="IView"/> interface.
            /// </summary>
            private sealed class View : IView
            {
                private readonly string _viewName;
                private readonly object _model;
                private readonly Type _modelType;

                /// <summary>
                /// Creates a new <see cref="View"/> instance.
                /// </summary>
                /// <param name="viewName">The view name, used to resolve the view template definition.</param>
                /// <param name="model">The data to be presented by the view.</param>
                public View(string viewName, object model)
                    : this(viewName, model, null)
                {
                }

                /// <summary>
                /// Creates a new <see cref="View"/> instance.
                /// </summary>
                /// <param name="viewName">The view name, used to resolve the view template definition.</param>
                /// <param name="model">The data to be presented by the view.</param>
                /// <param name="modelType">Optional explicit definition of data type for <see cref="Model"/>. 
                /// </param>
                /// <exception cref="ArgumentException"><paramref name="modelType"/> must be public.</exception>
                public View(string viewName, object model, Type modelType)
                {
                    _model = model;
                    _viewName = viewName;
                    _modelType = modelType ?? (model != null ? model.GetType() : null);
                }

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
            }
        }

        /// <summary>
        /// Configuration extensions for rendering HTML responses.
        /// </summary>
        public static class Extensions
        {
            /// <summary>
            /// Retrieves the <see cref="System.Net.Http.Formatting.MediaTypeFormatter"/> registered to handle HTML.
            /// </summary>
            /// <param name="formatters">The <see cref="MediaTypeFormatterCollection"/>.</param>
            /// <returns>The HtmlMediaTypeFormatter registered to handle requests for HTML.</returns>
            public static MediaTypeFormatter GetHtmlFormatter(this MediaTypeFormatterCollection formatters)
            {
                return formatters
                    .Select(formatter => System.Web.Http.Services.Decorator.GetInner<MediaTypeFormatter>(formatter))
                    .OfType<Formatting.HtmlMediaTypeViewFormatter>()
                    .SingleOrDefault();
            }
        }
    }

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
            var response = RequestMessage.CreateResponse(HttpStatusCode.OK, this, Formatting.Extensions.GetHtmlFormatter(config.Formatters));
            return Task.FromResult(response);
        }
    }

    /// <summary>
    /// Configuration extensions for rendering HTML responses.
    /// </summary>
    public static class Extensions
    {
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