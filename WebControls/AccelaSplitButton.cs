#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaSplitButton.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaSplitButton.cs 201304 2011-08-10 07:04:19Z ACHIEVO\alan.hu $.
 *  Revision History
 *  Date,            Who,           What
 *  Jul 6, 2011      Wallance Zhang Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Accela Split button.
    /// </summary>
    public class AccelaSplitButton : AccelaButton
    {
        #region Fields

        /// <summary>
        /// The drop button of the split button.
        /// </summary>
        private AccelaButton _dropBtn;

        /// <summary>
        /// The menu of the split button.
        /// </summary>
        private HtmlGenericControl _divMenu;

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the AccelaSplitButton class.
        /// </summary>
        public AccelaSplitButton()
        {
            _dropBtn = new AccelaButton();
            _divMenu = new HtmlGenericControl();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the menu items.
        /// </summary>
        public IList<ActionViewModel> MenuItems
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this button including menu items.
        /// </summary>
        private bool HaveMenuItems
        {
            get
            {
                return MenuItems != null && MenuItems.Count > 0;
            }
        }

        /// <summary>
        /// Gets the CSS class for Main button.
        /// </summary>
        private string MainButtonCSS
        {
            get
            {
                string cssClass = "ACA_SmButton ACA_SmButton_FontSize";

                if (HaveMenuItems)
                {
                    cssClass = "mainbtn mainbtn_fontsize";
                }

                return cssClass;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Construct the child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            if (HaveMenuItems)
            {
                _divMenu = new HtmlGenericControl("div");
                _divMenu.ID = ClientID + "divMenu";
                _divMenu.Attributes.Add("class", "splitbutton_menu");

                _dropBtn = new AccelaButton();
                _dropBtn.ID = "btnDrop";
                _dropBtn.Attributes.Add("href", "javascript:void(0);");
                _dropBtn.CssClass = "NotShowLoading";
                _dropBtn.OnClientClick = "SplitButton_ShowMenu('" + _divMenu.ID + "','" + ClientID + "_container');return false;";
                _dropBtn.Attributes.Add("onmousemove", "SplitButton_MenuFocus('" + _divMenu.ID + "')");
                _dropBtn.Attributes.Add("onmouseout", "SplitButton_MenuBlur('" + _divMenu.ID + "')");

                Controls.Add(_dropBtn);
            }
        }

        /// <summary>
        /// Handle the pre-render event to register script resource.
        /// </summary>
        /// <param name="e">event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RegisterScripts();
        }

        /// <summary>
        /// Render the split button.
        /// </summary>
        /// <param name="w">html writer object.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter w)
        {
            //Render split button.
            if (HaveMenuItems)
            {
                w.Write("<div id='" + ClientID + "_container' class='splitbutton'>");
            }

            if (I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft)
            {
                //RTL
                if (HaveMenuItems)
                {
                    w.Write("<div class='dropbtn'>");
                    _dropBtn.RenderControl(w);
                    w.Write("</div>");
                }

                w.Write("<div class='" + MainButtonCSS + "'>");
                base.Render(w);
                w.Write("</div>");
            }
            else
            {
                //LTR
                w.Write("<div class='" + MainButtonCSS + "'>");
                base.Render(w);
                w.Write("</div>");

                if (HaveMenuItems)
                {
                    w.Write("<div class='dropbtn'>");
                    _dropBtn.RenderControl(w);
                    w.Write("</div>");
                }
            }

            if (HaveMenuItems)
            {
                w.Write("</div>");

                // Contruct and render menu items.
                ConstructMenuItem(_divMenu);
                _divMenu.RenderControl(w);
            }
        }

        /// <summary>
        /// Construct the menu items.
        /// </summary>
        /// <param name="menuContainer">Menu container.</param>
        private void ConstructMenuItem(Control menuContainer)
        {
            foreach (ActionViewModel actionModel in MenuItems)
            {
                AccelaLinkButton menuItm = new AccelaLinkButton();
                menuItm.Text = ScriptFilter.AntiXssHtmlEncode(actionModel.ActionLabel);

                if (!string.IsNullOrWhiteSpace(actionModel.ClientEvent))
                {
                    /* Notice: SplitButton_HideMenu must execute before other event. 
                     * Because of other event possible 'return false' lead to the splitbutton menu can't hide automatic.
                     */
                    menuItm.OnClientClick = "SplitButton_HideMenu('" + _divMenu.ID + "'); " + actionModel.ClientEvent;
                }

                menuItm.Attributes.Add("onfocus", "SplitButton_MenuFocus('" + _divMenu.ID + "')");
                menuItm.Attributes.Add("onblur", "SplitButton_MenuBlur('" + _divMenu.ID + "')");
                menuItm.Attributes.Add("onmousemove", "SplitButton_MenuFocus('" + _divMenu.ID + "')");
                menuItm.Attributes.Add("onmouseout", "SplitButton_MenuBlur('" + _divMenu.ID + "')");
                menuItm.Attributes.Add("href", "javascript:void(0);");

                HtmlGenericControl divChild = new HtmlGenericControl("div");
                divChild.Attributes.Add("class", "splitbutton_menuitem");
                divChild.Controls.Add(menuItm);
                menuContainer.Controls.Add(divChild);
            }
        }

        /// <summary>
        /// Register the script resource.
        /// </summary>
        private void RegisterScripts()
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("AccelaSplitButton"))
            {
                ScriptManager.RegisterClientScriptInclude(this, typeof(Page), "AccelaSplitButton", GetScriptUrl());
            }
        }

        /// <summary>
        /// Get script url.
        /// </summary>
        /// <returns>script url</returns>
        private string GetScriptUrl()
        {
            string appRoot = HttpContext.Current.Request.ApplicationPath;

            if (!appRoot.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
            {
                appRoot += "/";
            }

            return appRoot + "Scripts/AccelaSplitButton.js";
        }

        #endregion Methods
    }
}