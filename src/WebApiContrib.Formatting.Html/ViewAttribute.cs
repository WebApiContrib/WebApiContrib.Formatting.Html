using System;

namespace WebApiContrib.Formatting.Html
{
    /// <summary>
    /// Attribute to override the default view name for a model type. By default the
    /// view name is assumed to equal the model class name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited= true)] 
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
}
