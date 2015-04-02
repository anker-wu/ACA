#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LabelService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LabelService.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.CustomizeAPI;
using Accela.ACA.Web.Common;

namespace Accela.ACA.ComponentService.Service
{
    /// <summary>
    /// This class provide the ability to operate label key
    /// </summary>
    public class LabelService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the LabelService class.
        /// </summary>
        /// <param name="context">User Context</param>
        public LabelService(UserContext context) : base(context)
        {
        }

        /// <summary>
        /// Get text by label key
        /// </summary>
        /// <param name="labelKey">Label Key</param>
        /// <returns>the label text</returns>
        public string GetLabelTextByKey(string labelKey)
        {
            return LabelUtil.GetGlobalTextByKey(labelKey); 
        }
    }
}
