/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IServerConstantBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IServerConstantBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */
namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Defines methods for Server constant BLL.
    /// </summary>
    public interface IServerConstantBll
    {
        #region Methods

        /// <summary>
        /// Gets the constant value from R1SERVER_CONSTANT table by constant name.
        /// If the constant name can't be found, it will return empty string.
        /// </summary>
        /// <param name="constantName">constant name be retrieved.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerId">user id (public group).</param>
        /// <returns>the constant value, return empty string if can't find the value.</returns>
        string GetServerConstantValue(string constantName, string agencyCode, string callerId);

        /// <summary>
        /// Gets the constant value from R1SERVER_CONSTANT table by constant name.
        /// If the constant name can't be found, it will return defaultValue.
        /// </summary>
        /// <param name="constantName">constant name be retrieved.</param>
        /// <param name="defaultValue">If the constant name can't be found,the defaultValue will be return.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerId">user id (public group).</param>
        /// <returns>the constant value, return defaultValue if can't find the value.</returns>
        string GetServerConstantValue(string constantName, string defaultValue, string agencyCode, string callerId);

        /// <summary>
        /// Gets public user group
        /// </summary>
        /// <param name="moduleName">current module name</param>
        /// <param name="agencyCode">current agency code</param>
        /// <param name="callerId">current caller id</param>
        /// <returns>public user group string</returns>
        string GetPublicUserGroup(string moduleName, string agencyCode, string callerId);
        
        #endregion Methods
    }
}