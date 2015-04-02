#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DailyRender.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: DailyRender.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web.UI.WebControls;
using Accela.ACA.Common.Util;
using Accela.Web.Controls.ControlRender;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Class for rendering control in ACA daily
    /// </summary>
    public class DailyRender : IAccelaControlRender
    {
        #region Methods

        /// <summary>
        /// Indicate whether sub label render or not
        /// </summary>
        /// <param name="subLabel">string sub label</param>
        /// <returns>true to render,otherwise not to render</returns>
        public bool IsRenderSubLabel(string subLabel)
        {
            return !string.IsNullOrEmpty(subLabel);
        }

        /// <summary>
        /// OnPreRender event
        /// </summary>
        /// <param name="control">object WebControl</param>
        public void OnPreRender(WebControl control)
        {
            if (control is IAccelaWithExtenderControl)
            {
                ((IAccelaWithExtenderControl)control).InitExtenderControl();
            }

            if (control is IAccelaControl && IsRenderSubLabel((control as IAccelaNonInputControl).GetSubLabel()))
            {
                AccelaWebControlExtender.HelperExtender helpExtender = new AccelaWebControlExtender.HelperExtender();
                helpExtender.ID = control.ID + "_helper_exd";
                helpExtender.BehaviorID = control.ClientID + "_helper_bhv";
                helpExtender.TargetControlID = control.ID;
                helpExtender.IsRTL = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;
                helpExtender.Title = LabelConvertUtil.GetTextByKey("aca_field_helpwindow_title", control);
                helpExtender.CloseTitle = LabelConvertUtil.GetGUITextByKey("aca_common_close");
                control.Controls.Add(helpExtender);
            }

            ControlRenderUtil.RegisterValidationJS(control.Page);
        }

        #endregion Methods
    }
}