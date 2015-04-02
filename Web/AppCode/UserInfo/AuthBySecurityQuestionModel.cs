#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AuthBySecurityQuestionModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

namespace Accela.ACA.Web.UserInfo
{
    /// <summary>
    /// The authentication by security question model.
    /// </summary>
    public class AuthBySecurityQuestionModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether enable authentication by security question.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the compulsory security question quantity.
        /// </summary>
        public int CompulsoryQuantity { get; set; }
    }
}