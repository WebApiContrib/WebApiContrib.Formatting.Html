namespace WebApiContrib.Formatting.Html
{
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
}
