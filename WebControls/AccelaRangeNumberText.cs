#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaRangeNumberText.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: 
 *
 *  Notes:
 * $Id: AccelaRangeNumberText.cs 238998 2012-12-04 09:10:00Z ACHIEVO\alan.hu $.
 *  Revision History
 *  Date,            Who,        What
 *  Dec 4, 2012      Alan Hu     Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;

[assembly: WebResource("Accela.Web.Controls.AccelaRangeNumberText.js", "text/javascript")]

namespace Accela.Web.Controls
{
    /// <summary>
    /// Class AccelaRangeNumberText.
    /// </summary>
    public class AccelaRangeNumberText : AccelaCompositeControl
    {
        #region Fields

        /// <summary>
        /// The text field control of range from.
        /// </summary>
        private AccelaNumberText _ctlRangeFrom = new AccelaNumberText();

        /// <summary>
        /// The text field control of range to.
        /// </summary>
        private AccelaNumberText _ctlRangeTo = new AccelaNumberText();

        /// <summary>
        /// is need dot or not.
        /// </summary>
        private bool _isNeedDot = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the max length
        /// </summary>
        public int MaxLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of range from text box.
        /// </summary>
        public string TextFrom
        {
            get
            {
                return _ctlRangeFrom.Text;
            }

            set
            {
                _ctlRangeFrom.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of range to text box.
        /// </summary>
        public string TextTo
        {
            get
            {
                return _ctlRangeTo.Text;
            }

            set
            {
                _ctlRangeTo.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is need dot.
        /// </summary>
        /// <value><c>true</c> if this instance is need dot; otherwise, <c>false</c>.</value>
        public bool IsNeedDot
        {
            get
            {
                return _isNeedDot;
            }

            set
            {
                _isNeedDot = value;
            }
        }

        /// <summary>
        /// Gets or sets the tool tip label key range from.
        /// </summary>
        /// <value>The tool tip label key range from.</value>
        public string ToolTipLabelKeyRangeFrom
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tool tip label key range automatic.
        /// </summary>
        /// <value>The tool tip label key range automatic.</value>
        public string ToolTipLabelKeyRangeTo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the watermark range from.
        /// </summary>
        /// <value>The watermark range from.</value>
        private string WatermarkRangeFrom
        {
            get
            {
                string watermark = LabelConvertUtil.GetTextByKey(LabelKey + WebControlConstant.LABEL_KEY_WATERMARK1_SUFFIX, this);
                return string.IsNullOrEmpty(watermark) ? string.Empty : System.Web.HttpUtility.HtmlDecode(watermark);
            }
        }

        /// <summary>
        /// Gets the watermark range to.
        /// </summary>
        private string WatermarkRangeTo
        {
            get
            {
                string watermark = LabelConvertUtil.GetTextByKey(LabelKey + WebControlConstant.LABEL_KEY_WATERMARK2_SUFFIX, this);
                return string.IsNullOrEmpty(watermark) ? string.Empty : System.Web.HttpUtility.HtmlDecode(watermark);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initial extender controls.
        /// </summary>
        public override void InitExtenderControl()
        {
            // this method need not implement.
        }

        /// <summary>
        /// Clear each textbox's text value.
        /// </summary>
        public override void ClearValue()
        {
            _ctlRangeFrom.Text = string.Empty;
            _ctlRangeTo.Text = string.Empty;
        }

        /// <summary>
        /// Disable edit the text box controls.
        /// </summary>
        public override void DisableEdit()
        {
            _ctlRangeFrom.DisableEdit();
            _ctlRangeTo.DisableEdit();
        }

        /// <summary>
        /// Enable edit the text box controls.
        /// </summary>
        public override void EnableEdit()
        {
            _ctlRangeFrom.EnableEdit();
            _ctlRangeTo.EnableEdit();
        }

        /// <summary>
        /// Get current control's value
        /// </summary>
        /// <returns>Control value.</returns>
        public override string GetValue()
        {
            if (string.IsNullOrEmpty(TextFrom) || string.IsNullOrEmpty(TextTo))
            {
                return !string.IsNullOrEmpty(TextFrom) ? TextFrom : TextTo;
            }

            return string.Format("{0}{1}{2}", TextFrom, ACAConstant.SPLIT_CHAR, TextTo);
        }

        /// <summary>
        /// if to hide the required indicate
        /// </summary>
        /// <returns>Hide the required indicate or not</returns>
        public override bool IsHideRequireIndicate()
        {
            return false;
        }

        /// <summary>
        /// Check whether this control is required input.
        /// </summary>
        /// <returns>Whether this control is required input</returns>
        public override bool IsRequired()
        {
            return IsFieldRequired;
        }

        /// <summary>
        /// Sets the value of control fields
        /// </summary>
        /// <param name="value">control value.</param>
        public override void SetValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            string[] values = value.Split(ACAConstant.SPLIT_CHAR);

            if (value.Length > 1)
            {
                _ctlRangeFrom.Text = values[0];
                _ctlRangeTo.Text = values[1];
            }
            else if (value.Length == 1)
            {
                _ctlRangeFrom.Text = values[0];
                _ctlRangeTo.Text = values[0];
            }
        }

        /// <summary>
        /// Create children controls
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            base.CreateChildControls();
            
            Attributes.Add("class", "RangeNumberText_Control_Layout");

            Literal literal = new Literal();
            literal.Text = "<div class=\"RangeNumberText_SubControl1_Layout\">";
            Controls.Add(literal);

            _ctlRangeFrom.ID = string.Format("{0}0", CHILD_CONTROL_ID_PREFIX);
            _ctlRangeFrom.WatermarkText = WatermarkRangeFrom;
            _ctlRangeFrom.Width = Unit.Percentage(100);
            _ctlRangeFrom.IsNeedDot = IsNeedDot;
            _ctlRangeFrom.MaxLength = MaxLength;
            Controls.Add(_ctlRangeFrom);

            literal = new Literal();
            literal.Text = "</div>";
            literal.Text += "<span class=\"RangeNumberText_Split_Layout\"> - </span>";
            literal.Text += "<div class=\"RangeNumberText_SubControl2_Layout\">";
            Controls.Add(literal);

            _ctlRangeTo.ID = string.Format("{0}1", CHILD_CONTROL_ID_PREFIX);
            _ctlRangeTo.WatermarkText = WatermarkRangeTo;
            _ctlRangeTo.Width = Unit.Percentage(100);
            _ctlRangeTo.IsNeedDot = IsNeedDot;
            _ctlRangeTo.MaxLength = MaxLength;
            Controls.Add(_ctlRangeTo);

            literal = new Literal();
            literal.Text = "</div>";
            Controls.Add(literal);

            ChildControlsCreated = true;
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            bool isAdmin = AccelaControlRender.IsAdminRender(this);

            if (isAdmin)
            {
                // The following attributes are only used to control the Admin Console display
                Attributes.Add("watermarkText1", WatermarkRangeFrom);
                Attributes.Add("watermarkText2", WatermarkRangeTo);
                Attributes.Add("watermarkCssClass", "watermark");
            }
            else
            {
                InitValidatorExtender();
            }

            if (!isAdmin && Page != null)
            {
                // Register the client scripts resource.
                ScriptManager.RegisterClientScriptResource(Page, GetType(), "Accela.Web.Controls.AccelaRangeNumberText.js");

                // Update the tooptip for the Range Number Text From/To fields.
                string numberFromTitle = GetSubChildTooltip(ToolTipLabelKeyRangeFrom, _ctlRangeFrom.WatermarkText);
                string numberToTitle = GetSubChildTooltip(ToolTipLabelKeyRangeTo, _ctlRangeTo.WatermarkText);

                string script = string.Format(
                    @"(function(){{ var fromTitle = DecodeHTMLTag('{0}');
                                                      var toTitle= DecodeHTMLTag('{1}');
                                                      $('#{2}').attr('title', fromTitle);
                                                      $('#{3}').attr('title', toTitle);}})();",
                                        ScriptFilter.EncodeHtml(numberFromTitle),
                                        ScriptFilter.EncodeHtml(numberToTitle),
                                        _ctlRangeFrom.ClientID,
                                        _ctlRangeTo.ClientID);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), ClientID + "UpdateSubChildTooltip", script, true);
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Initial validator extender control
        /// </summary>
        private void InitValidatorExtender()
        {
            // Create the custom validator controls for requried field.
            if (IsRequired())
            {
                CreateRequiredValidate("requiredValidFrom", "AccelaRangeNumberText.validatorForRequired", _ctlRangeFrom);
                CreateRequiredValidate("requiredValidTo", "AccelaRangeNumberText.validatorForRequired", _ctlRangeTo);
            }

            //Create custom validator controls for range text
            CreateRangeValidate("customValidFrom", _ctlRangeFrom);
            CreateRangeValidate("customValidTo", _ctlRangeTo);
        }

        /// <summary>
        /// Create range text values validate
        /// </summary>
        /// <param name="id">The new validate control's id</param>
        /// <param name="validateControl">The target control to be validate</param>
        private void CreateRangeValidate(string id, AccelaNumberText validateControl)
        {
            string errMessage = LabelConvertUtil.GetGlobalTextByKey("aca_rangesearch_msg_fromgreaterthanto");

            CustomValidator customValidator = new CustomValidator();
            customValidator.ID = id;
            customValidator.ControlToValidate = validateControl.ID;
            customValidator.Display = ValidatorDisplay.None;
            customValidator.ClientValidationFunction = "AccelaRangeNumberText.validatorForRangeValue";
            customValidator.ErrorMessage = System.Web.HttpUtility.HtmlEncode(string.Format(errMessage, WatermarkRangeFrom, WatermarkRangeTo));
            Controls.Add(customValidator);
            CreateValidatorCallbackExtender(customValidator.ID, validateControl.ClientID);
        }

        /// <summary>
        /// Get the sub child's tooltip.
        /// </summary>
        /// <param name="tooltipLabelKey">The tooltip label key.</param>
        /// <param name="watermark">The watermark</param>
        /// <returns>The tooltip.</returns>
        private string GetSubChildTooltip(string tooltipLabelKey, string watermark)
        {
            string moduleName = GetModuleName();
            string fieldName = LabelConvertUtil.GetTextByKey(LabelKey, moduleName).TrimEnd();

            if (fieldName.EndsWith(":"))
            {
                fieldName = fieldName.Substring(0, fieldName.Length - 1);
            }

            string result = fieldName;

            if (IsFieldRequired)
            {
                result += ACAConstant.BLANK + LabelConvertUtil.GetGlobalTextByKey("aca_required_field");
            }

            if (!string.IsNullOrEmpty(watermark))
            {
                result += ACAConstant.BLANK + watermark;
            }
            else
            {
                result += ACAConstant.BLANK + LabelConvertUtil.GetTextByKey(tooltipLabelKey, moduleName);
            }

            return result;
        }

        #endregion Methods
    }
}
