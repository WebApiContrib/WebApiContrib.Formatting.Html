using System;

namespace WebApiContrib.Formatting.Html
{
    /// <summary>
    /// Represents default implementation of <see cref="IView"/> interface.
    /// </summary>
    public class View : IView
    {
        /// <summary>
        /// Creates a new <see cref="View"/> instance.
        /// </summary>
        /// <param name="viewName">The view name, used to resolve the view template definition.</param>
        /// <param name="model">The data to be presented by the view.</param>
        public View(string viewName, object model) : this(viewName, model, null)
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
            Model = model;
            ViewName = viewName;
            ModelType = modelType ?? (model != null ? model.GetType() : null);
        }

        /// <summary>
        /// The data to be presented by the view.
        /// </summary>
        public object Model { get; private set; }

        /// <summary>
        /// The view name, used to resolve the view template definition.
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        /// Optional explicit definition of data type for <see cref="Model"/>. When specified,
        /// this type must be public and have a type name that matches the C# and VB language
        /// rules for identifiers. It should not be set if the model is an anonymous type or
        /// a compiler-generated iterator (enumerable or enumerator) type.
        /// </summary>
        public Type ModelType { get; private set; }
    }
}
