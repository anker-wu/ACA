/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ILinkItem.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ILinkItem.cs 239452 2012-12-12 05:56:43Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  10/18/2007    gopal.narra    Initial.
 * </pre>
 */

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// IlinkItem interface for TabItem and LinkItem.
    /// </summary>
    public interface ILinkItem
    {
        /// <summary>
        /// Gets or sets tab order or link item order in the same group.
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// Gets or stes the label/text to be showed to user.
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// Gets or sets the item key according to standard choice key.
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// Gets or set the final url.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Indicates the current link whether can be used by anonymous user.
        /// </summary>
        /// <returns>true-anonymous,false-register.</returns>
        bool IsAnonymousInRoles { get; }

        /// <summary>
        /// Gets or sets the current link module name. 
        /// </summary>
        string Module { get; set; }

        /// <summary>
        /// Gets or sets the all of roles that the current link can be showed.
        /// the value like below: 0 or 0|1 or 1, 0 - citizen user, 1-anonymous user.
        /// </summary>
        string Roles { get; set; }

        /// <summary>
        /// Gets or sets the current link force login. 
        /// the value like below: true or false,true - need force login, false-no need force login.
        /// </summary>
        bool ForceLogin { get; set; }

        /// <summary>
        /// Indicate whether allow select multiple service or single service
        /// true: single service selection only.
        /// false: allow multiple service selecton.
        /// </summary>
        bool SingleServiceOnly { get; set; }

        /// <summary>
        /// Gets or sets the current link can have record type filter settings. 
        /// the value like below: true or false,true - have record type filter settings, false-no record type filter settings.
        /// </summary>
        bool NeedFilter { get; set; }
    }
}
