/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IPage.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  Defined an interface for common page.
 *  Notes:
 *      $Id: IPage.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
namespace Accela.ACA.Common
{
    /// <summary>
    /// Defined an interface for common page.
    /// module name only can be got from page, so base page class should implement this interface.
    /// Accela web control will use this interface to get module name parameter or necessary parameters, 
    /// </summary>
    public interface IPage
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether the controls in current page whether need to be rendered as admin mode.
        /// true - All controls will be rendered as daily mode.
        /// false- All controls will be rendered as admin mode
        /// </summary>
        bool IsControlRenderAsAdmin
        {
            get;
        }

        /// <summary>
        /// Gets the current page's element id.
        /// </summary>
        string PageID
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the current runtime page's module name.
        /// if current page doesn't belong to any module,returns String.Empty.
        /// </summary>
        /// <returns>module name.</returns>
        string GetModuleName();

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key, return String.Empty.</returns>
        string GetTextByKey(string key);

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>The text(label message) according the key.if can't find the key, return String.Empty.</returns>
        string GetTextByKey(string key, string moduleName);

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key, return String.Empty.</returns>
        string GetSuperAgencyTextByKey(string key);

        /// <summary>
        /// Construct title value with image alt value with blank. 
        /// </summary>
        /// <param name="alt">The image alt label key</param>
        /// <param name="title">The title label key</param>
        /// <returns>title value</returns>
        string GetTitleByKey(string alt, string title);

        #endregion Methods
    }
}