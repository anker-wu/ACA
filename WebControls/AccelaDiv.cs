#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaDiv.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaDiv.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI.WebControls;

using Accela.ACA.Common;

namespace Accela.Web.Controls
{
    #region Enumerations

    /// <summary>
    /// The div type
    /// </summary>
    public enum DivType
    {
        /// <summary>
        /// the normal div
        /// </summary>
        Normal = 0,

        /// <summary>
        /// the div which contain the standard component
        /// </summary>
        StandardComponent = 1,

        /// <summary>
        /// the div which contain the customize component
        /// </summary>
        CustomizeComponent = 2
    }

    #endregion Enumerations

    /// <summary>
    /// Provide a div control
    /// </summary>
    public class AccelaDiv : Panel, IAccelaNonInputControl
    {
        #region Fields

        /// <summary>
        /// whether display label,default value is true.
        /// </summary>
        private bool _isDisplayLabel = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether control label span is or isn't display
        /// </summary>
        public bool IsDisplayLabel
        {
            get
            {
                return _isDisplayLabel;
            }

            set
            {
                _isDisplayLabel = value;
            }
        }

        /// <summary>
        /// Gets or sets label key
        /// </summary>
        public string LabelKey
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the div type.
        /// </summary>
        /// <value>The div type.</value>
        public DivType DivType
        {
            get; 
            
            set;
        }

        /// <summary>
        /// Gets or sets section ID.
        /// If the current div is the component div, it identify the component use PK resID 
        /// </summary>
        public string SectionID
        {
            get;
            
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// get default label
        /// </summary>
        /// <returns>default label</returns>
        public string GetDefaultLabel()
        {
            return string.Empty;
        }

        /// <summary>
        /// get default language text
        /// </summary>
        /// <returns>default language text</returns>
        public string GetDefaultLanguageText()
        {
            return string.Empty;
        }

        /// <summary>
        /// Get Default Language Sub Label of control.
        /// </summary>
        /// <returns>Default language sub label of control</returns>
        public string GetDefaultLanguageSubLabel()
        {
            return string.Empty;
        }

        /// <summary>
        /// Get Default Sub Label of control.
        /// </summary>
        /// <returns>Default sub label of control</returns>
        public string GetDefaultSubLabel()
        {
            return string.Empty;
        }

        /// <summary>
        /// Get Sub Label of control.
        /// </summary>
        /// <returns>Default sub label of control</returns>
        public string GetSubLabel()
        {
            return string.Empty;
        }

        /// <summary>
        /// Initial extender control
        /// </summary>
        public void InitExtenderControl()
        {
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), AccelaControlRender.IsAdminRender(this));
            render.OnPreRender(this);

            base.OnPreRender(e);
        }

        /// <summary>
        /// Displays the div on the client.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<div id=\"{0}\"", this.ClientID);
            this.Attributes.Render(writer);
            writer.Write(">");
            RenderChildren(writer);
            writer.Write("</div>");
        }

        #endregion Methods
    }
}