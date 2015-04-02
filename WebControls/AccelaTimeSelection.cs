#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaTimeSelection.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 */

#endregion Header

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.Web.Controls.ControlRender;

namespace Accela.Web.Controls
{
    /// <summary>
    ///  Provide a control for time selection 
    /// </summary>
    public class AccelaTimeSelection : AccelaTimeText, IAccelaControl
    {
        #region Fields

        /// <summary>
        /// Dropdown list for time selection control
        /// </summary>
        private AccelaDropDownList ddlAMPM = null;
        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is use24 hours.
        /// </summary>
        /// <value><c>true</c> if this instance is use24 hours; otherwise, <c>false</c>.</value>
        public bool IsUse24Hours
        {
            get
            {
                return !string.IsNullOrEmpty(I18nDateTimeUtil.ShortTimePattern) && I18nDateTimeUtil.ShortTimePattern.IndexOf("h") == -1;
            }
        }

        /// <summary>
        /// Gets the text mask.
        /// </summary>
        public override string Mask
        {
            get
            {
                return I18nDateTimeUtil.ShortTimeMask;                  
            }
        }

        /// <summary>
        /// Gets or sets text
        /// </summary>     
        public string TimeText
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    return string.Empty;
                }

                if (IsUse24Hours)
                {
                    return this.Text;
                }
                else
                {
                    return this.Text + ACAConstant.BLANK + ddlAMPM.SelectedItem.Value;
                }
            }

            set
            {            
                string timeString = value;

                if (IsUse24Hours)
                {
                    this.Text = value;
                }
                else
                {
                    string[] timeStrArr = timeString.Split(' ');

                    if (timeStrArr != null && timeStrArr.Length > 1)
                    {
                        this.Text = timeStrArr[0];

                        if (I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.AMDesignator.Equals(timeStrArr[1], StringComparison.InvariantCultureIgnoreCase))
                        {
                            ddlAMPM.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlAMPM.SelectedIndex = 1;
                        }
                    }
                    else
                    {
                        this.Text = value;

                        if (string.IsNullOrEmpty(value))
                        {
                            ddlAMPM.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets text
        /// </summary>
        [Obsolete("Please use the TimeText property.", true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                base.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets label key of tooltip in AM/PM dropdown list.
        /// </summary>
        public string AmPmTooltipLabelKey
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clear current control value and child
        /// </summary>
        public override void ClearValue()
        {
            this.TimeText = string.Empty;
            if (ddlAMPM != null)
            {
                this.ddlAMPM.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Disable am/pm control to make it readonly.
        /// </summary>
        public override void DisableEdit()
        {
            this.ReadOnly = true;

            if (ddlAMPM != null)
            {
                this.ddlAMPM.Enabled = false;
            }

            // updated CssClass to append readonly css.
            this.CssClass += " " + WebControlConstant.CSS_CLASS_READONLY;
        }

        /// <summary>
        /// Enable am/pm control to make it be editable.
        /// </summary>
        public override void EnableEdit()
        {
            this.ReadOnly = false;

            if (ddlAMPM != null)
            {
                this.ddlAMPM.Enabled = true;
            }

            // Remove readonly style from Css.
            if (this.CssClass != null)
            {
                this.CssClass = this.CssClass.Replace(WebControlConstant.CSS_CLASS_READONLY, string.Empty);
            }
        }

        /// <summary>
        /// Set the am pm at front of time selection.
        /// </summary>
        /// <param name="w">HtmlTextWriter object</param>
        public new void RenderElement(HtmlTextWriter w)
        {
            this.ToolTip = ControlRenderUtil.GetToolTip(this);

            if (!IsUse24Hours)
            {
                w.AddAttribute("role", "presentation");
                w.RenderBeginTag(HtmlTextWriterTag.Table);
                w.RenderBeginTag(HtmlTextWriterTag.Tr);
                w.RenderBeginTag(HtmlTextWriterTag.Td);
                RenderBeginTag(w);
                RenderEndTag(w);
                w.RenderEndTag();
                w.RenderBeginTag(HtmlTextWriterTag.Td);
                w.Write("<div style='width:2px;'></div>");
                w.RenderEndTag();
                w.RenderBeginTag(HtmlTextWriterTag.Td);
                this.RenderChildren(w);
                w.RenderEndTag();
                w.RenderEndTag();
                w.RenderEndTag();
            }
            else
            {
                base.RenderElement(w);                
            }
        }

        /// <summary>
        /// Create a AM/PM control as a part of  time selection.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (!IsUse24Hours)
            {
                ddlAMPM = new AccelaDropDownList();               
                ddlAMPM.ID = this.ID + "_AMPM";
                ddlAMPM.ToolTipLabelKey = AmPmTooltipLabelKey;
                Controls.Add(ddlAMPM);
            }
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.EnsureChildControls();
            
            if (ddlAMPM != null)
            {
                ddlAMPM.Items.Add(new ListItem(LabelConvertUtil.GetTextByKey("common_am_label", this), I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.AMDesignator));
                ddlAMPM.Items.Add(new ListItem(LabelConvertUtil.GetTextByKey("common_pm_label", this), I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.PMDesignator));
                ddlAMPM.SelectedIndex = 0;
            }
            
            if (!IsUse24Hours)
            {
                this.MinValue = "01:00";
                this.MaxValue = "12:59";
            }
            else
            {
                this.MinValue = "00:00";
                this.MaxValue = "23:59";
            }

            this.IsUsedForAMPM = true;
        }

        #endregion Methods
    }
}