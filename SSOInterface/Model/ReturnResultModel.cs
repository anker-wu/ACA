#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ReturnResultModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: ReturnResultModel.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.SSOInterface.Model
{
    /// <summary>
    /// Class Return Result Model
    /// </summary>
    public class ReturnResultModel
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the return entity.
        /// </summary>
        /// <value>The return entity.</value>
        public object ReturnEntity { get; set; }
    }
}
