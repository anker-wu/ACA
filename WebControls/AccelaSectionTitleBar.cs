#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaSectionTitleBar.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.UI;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;

namespace Accela.Web.Controls
{
    /// <summary>
    /// the tool bar show type
    /// </summary>
    public enum ToolBarControlShowType
    {
        /// <summary>
        /// No tool bar title
        /// </summary>
        Hidden = 0,

        /// <summary>
        /// Show the daily and Admin
        /// </summary>
        Show,

        /// <summary>
        /// Show only admin
        /// </summary>
        OnlyAdmin,
    }

    /// <summary>
    /// the title label with the title bar
    /// </summary>
    public class AccelaSectionTitleBar : AccelaNonInputCompositeControl
    {
        #region fields

        /// <summary>
        /// The toolBar controls, show in the left
        /// </summary>
        private List<Control> _toolBarControl;

        /// <summary>
        /// section id value
        /// </summary>
        private string _sectionId = string.Empty;

        /// <summary>
        /// label type
        /// </summary>
        private LabelType _labelType = LabelType.SectionExText;

        /// <summary>
        /// the tool bar show type, default show only admin
        /// </summary>
        private ToolBarControlShowType _showType = ToolBarControlShowType.OnlyAdmin;

        /// <summary>
        /// The custom text field
        /// </summary>
        private string _text = string.Empty;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is show bar.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is show bar; otherwise, <c>false</c>.
        /// </value>
        public ToolBarControlShowType ShowType
        {
            get
            {
                return this._showType;
            }

            set
            {
                this._showType = value;
            }
        }

        /// <summary>
        /// Gets or sets section ID, if current label is section label,this property identify the id of the section
        /// </summary>
        public string SectionID
        {
            get
            {
                return this._sectionId;
            }

            set
            {
                this._sectionId = value;
            }
        }

        /// <summary>
        /// Gets or sets a control's ClientID.
        /// The value of this control contains a section's permission value.
        /// In Admin, section property grid will get the section permission value from this control.
        /// </summary>
        public string PermissionValueId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is display super agency text.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is display super agency text; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisplaySuperAgencyText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                string text = string.Empty;

                if (!string.IsNullOrEmpty(_text))
                {
                    text = _text;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.LabelKey))
                    {
                        if (this.IsDisplaySuperAgencyText && !AccelaControlRender.IsAdminRender(this))
                        {
                            text = LabelConvertUtil.GetSuperAgencyTextByKey(this.LabelKey, this);
                        }
                        else
                        {
                            text = LabelConvertUtil.GetTextByKey(this.LabelKey, this.GetModuleName(), this);
                        }
                    }
                }

                return text;
            }

            set
            {
                _text = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the label.
        /// </summary>
        /// <value>The type of the label.</value>
        public LabelType LabelType
        {
            get
            {
                return this._labelType;
            }

            set
            {
                this._labelType = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add ToolBar control to the Section Tool Bar.
        /// </summary>
        /// <param name="control">The control.</param>
        public void AddToolBarControls(Control control)
        {
            if (control == null)
            {
                return;
            }

            if (this._toolBarControl == null)
            {
                this._toolBarControl = new List<Control>();
            }

            this._toolBarControl.Add(control);

            if (!this.Controls.Contains(control))
            {
                this.Controls.Add(control);
            }
        }

        /// <summary>
        /// initial daily extender control
        /// </summary>
        public override void InitExtenderControl()
        {
            //not need extender
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            IAccelaControlRender render = ObjectFactory.GetObject(typeof(IAccelaControlRender), AccelaControlRender.IsAdminRender(this)) as IAccelaControlRender;
            render.OnPreRender(this);

            base.OnPreRender(e);
        }

        /// <summary>
        /// Displays the label on the client.
        /// </summary>
        /// <param name="w">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter w)
        {
            if (this.DesignMode)
            {
                return;
            }

            w.Write("<div class=\"ACA_Title_Bar\"><span class=\"ACA_FLeft\"><h1>");
            w.Write("<span ID=\"" + this.ClientID + "\">" + this.Text + "</span>");
            w.Write("</h1></span><span class=\"ACA_FRight\">");
            bool showToolbarControl = false;

            switch (this._showType)
            {
                case ToolBarControlShowType.Show:
                    showToolbarControl = true;
                    break;

                case ToolBarControlShowType.Hidden:
                    showToolbarControl = false;
                    break;

                case ToolBarControlShowType.OnlyAdmin:
                    if (AccelaControlRender.IsAdminRender(this))
                    {
                        showToolbarControl = true;
                    }

                    break;
            }

            if (showToolbarControl && _toolBarControl != null)
            {
                foreach (Control webControl in this._toolBarControl)
                {
                    if (webControl is IAccelaControl)
                    {
                        AccelaControlRender.Render(w, webControl as IAccelaControl);
                    }
                    else
                    {
                        webControl.RenderControl(w);
                    }
                }
            }

            w.Write("</span></div>");

            string subLabel = ScriptFilter.FilterScript(this.GetSubLabel(), false);
            string subLabelCls = string.IsNullOrEmpty(subLabel) ? "ACA_Hide" : "ACA_Section_Instruction ACA_Section_Instruction_FontSize";
            string subLabelHtml = string.Format("<div id='{0}_sub_label' class='{1}'>{2}</div>", this.ClientID, subLabelCls, subLabel);
            w.Write(subLabelHtml);

            this.RenderChildren(w);
        }

        /// <summary>
        /// Outputs the content of a server control's children to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the rendered content.</param>
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            if (_toolBarControl == null)
            {
                return;
            }

            foreach (Control control in this.Controls)
            {
                //if the children control not in _toolBarControl need to render.
                if (!this._toolBarControl.Contains(control))
                {
                    control.RenderControl(writer);
                }
            }
        }

        #endregion
    }
}