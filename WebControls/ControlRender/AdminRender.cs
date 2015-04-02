#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminRender.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AdminRender.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.Web.Controls.ControlRender;
using AccelaWebControlExtender;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Class for rendering Accela control class in admin
    /// </summary>
    public class AdminRender : IAccelaControlRender
    {
        #region Methods

        /// <summary>
        /// Indicate whether sub label render or not
        /// </summary>
        /// <param name="subLabel">sub label string</param>
        /// <returns>true to render,otherwise not to render</returns>
        public bool IsRenderSubLabel(string subLabel)
        {
            return true;
        }

        /// <summary>
        /// OnPreRender event
        /// </summary>
        /// <param name="control">object WebControl</param>
        public void OnPreRender(WebControl control)
        {
            if ((control is AccelaLabel
                    && string.IsNullOrEmpty(((AccelaLabel)control).LabelKey)
                    && ((AccelaLabel)control).LabelType != LabelType.HidableLabel
                    && string.IsNullOrEmpty(((AccelaLabel)control).Text))
                || (control is AccelaNumberText 
                    && ((AccelaNumberText)control).Is4CountryCode)
                || (control is AccelaTextBox 
                    && string.IsNullOrEmpty(((AccelaTextBox)control).LabelKey))
                || (control is AccelaDropDownList 
                    && string.IsNullOrEmpty(((AccelaDropDownList)control).LabelKey) 
                    && ((AccelaDropDownList)control).SourceType != DropDownListDataSourceType.HardCode)
                || (control is AccelaNameValueLabel && string.IsNullOrEmpty((control as IAccelaNonInputControl).LabelKey))
                || (control is AccelaCheckBoxList && string.IsNullOrEmpty(((AccelaCheckBoxList)control).LabelKey)))
            {
                return;
            }

            if (control is AccelaLinkButton)
            {
                AccelaLinkButton lb = (AccelaLinkButton)control;
                lb.OnClientClick = string.Empty;
            }

            if (control is AccelaButton)
            {
                AccelaButton btn = (AccelaButton)control;
                btn.OnClientClick = string.Empty;
            }

            if (!IsContainExtender(control))
            {
                ControlRender.ControlRenderUtil.CreateDesinSupportExtender(control);
            }

            ControlRenderUtil.RegisterValidationJS(control.Page);
        }

        /// <summary>
        /// Determines whether the specified control contain extender.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>Return true if the specified control contain extender; otherwise false.</returns>
        private bool IsContainExtender(WebControl control)
        {
            if (control is AccelaLabel)
            {
                AccelaLabel label = control as AccelaLabel;

                foreach (Control child in label.Children)
                {
                    if (child is AccelaWebControlExtenderExtender)
                    {
                        return true;
                    }
                }
            }
            else if (control is AccelaLinkButton)
            {
                AccelaLinkButton label = control as AccelaLinkButton;

                foreach (Control child in label.Children)
                {
                    if (child is AccelaWebControlExtenderExtender)
                    {
                        return true;
                    }
                }
            }

            if (control == null || !control.HasControls())
            {
                return false;
            }
         
            foreach (Control child in control.Controls)
            {
                if (child is AccelaWebControlExtenderExtender)
                {
                    return true;
                }
            }
            
            return false;
        }

        #endregion Methods
    }
}