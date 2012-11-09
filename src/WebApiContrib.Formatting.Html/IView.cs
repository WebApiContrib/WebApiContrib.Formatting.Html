using System;

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
}
