/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ServerTemplate.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ServerTemplate.cs 131439 2009-05-19 11:07:26Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls.Navigation
{
    /// <summary>
    /// ServerTemplate class
    /// </summary>
    [ToolboxItem(false)]
    [ParseChildren(true)]
    [PersistChildren(false)]
    public sealed class ServerTemplate : WebControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets template
        /// </summary>
        [Browsable(false), TemplateContainer(typeof(ServerTemplateContainer)), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ITemplate Template { get; set; }

        #endregion Properties
    }
}
