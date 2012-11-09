using System.Text;

namespace WebApiContrib.Formatting.Html.ViewParsers
{
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
}
