#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AuthorizedServiceSettingModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

namespace Accela.ACA.Web.AuthorizedAgent
{
    /// <summary>
    /// The authorized service setting's model.
    /// </summary>
    public class AuthorizedServiceSettingModel
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the name of the cap type filter.
        /// </summary>
        /// <value>
        /// The name of the cap type filter.
        /// </value>
        public string CapTypeFilterName { get; set; }
    }
}