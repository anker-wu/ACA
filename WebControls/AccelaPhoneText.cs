#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaPhoneText.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaPhoneText.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
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
    /// provide a phone box to input phone
    /// </summary>
    public class AccelaPhoneText : AccelaMaskText, IAccelaControl
    {
        #region Fields

        /// <summary>
        /// CSS class string
        /// </summary>
        private string _cssClass = null;

        /// <summary>
        /// text for country code
        /// </summary>
        private AccelaNumberText txtCountryCode = null;

        /// <summary>
        /// indicates validate phone format or not.
        /// </summary>
        private bool _isIgnoreValidate;

        /// <summary>
        /// The phone mask
        /// </summary>
        private string _phoneMask = string.Empty;

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaPhoneText"/> class.
        /// </summary>
        public AccelaPhoneText()
        {
            IsAlwaysEditable = false;
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether validate phone format or not.
        /// </summary>
        public bool IsIgnoreValidate
        {
            get
            {
                return _isIgnoreValidate;
            }

            set
            {
                _isIgnoreValidate = value;
            }
        }

        /// <summary>
        /// Gets the country code clientID.
        /// </summary>
        public string CountryCodeClientID
        {
            get
            {
                if (this.txtCountryCode != null)
                {
                    return this.txtCountryCode.ClientID;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets the country code text.
        /// </summary>
        public string CountryCodeText
        {
            get
            {
                if (this.txtCountryCode != null)
                {
                    return this.txtCountryCode.Text;
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                if (this.txtCountryCode != null)
                {
                    this.txtCountryCode.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets CSS class
        /// </summary>
        public override string CssClass
        {
            get
            {
                if (string.IsNullOrEmpty(_cssClass))
                {
                    if (ControlConfigureProvider.IsCountryCodeEnabled())
                    {
                        _cssClass = "ACA_NMLong";
                    }
                    else
                    {
                        _cssClass = "ACA_Medium";
                    }

                    if (this.ReadOnly)
                    {
                        _cssClass += " " + WebControlConstant.CSS_CLASS_READONLY;
                    }
                }

                return _cssClass;
            }

            set
            {
                _cssClass = value;
            }
        }

        /// <summary>
        /// Gets the text mask.
        /// </summary>
        public override string Mask
        {
            get
            {
                return _phoneMask;
            }

            set
            {
                _phoneMask = value;
            }
        }

        /// <summary>
        /// Gets or sets maximum length of the text value of this element.
        /// </summary>
        public int PhoneMaxLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the control is always editable in client.
        /// </summary>
        public new bool IsAlwaysEditable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether focus is set to the control specified by the System.Web.UI.WebControls.BaseValidator.ControlToValidate property when validation fails.
        /// </summary>
        public override bool SetFocusOnError
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clear current control value and child
        /// </summary>
        public override void ClearValue()
        {
            this.Text = string.Empty;
            this.CountryCodeText = string.Empty;
        }

        /// <summary>
        /// Disable current control to make it readonly.
        /// </summary>
        public override void DisableEdit()
        {
            this.ReadOnly = true;

            if (txtCountryCode != null)
            {
                this.txtCountryCode.ReadOnly = true;
            }

            // Change PhoneTextBox's style
            ChangeCssClass();

            if (txtCountryCode != null)
            {
                this.txtCountryCode.CssClass = this.CssClass;
            }
        }

        /// <summary>
        /// Enable current control to make it be editable.
        /// </summary>
        public override void EnableEdit()
        {
            this.ReadOnly = false;

            if (txtCountryCode != null)
            {
                this.txtCountryCode.ReadOnly = false;
            }

            // Change PhoneTextBox's style
            ChangeCssClass();

            if (txtCountryCode != null)
            {
                this.txtCountryCode.CssClass = this.CssClass;
            }
        }

        /// <summary>
        /// Set the country code control at front of phone number.
        /// </summary>
        /// <param name="w">HtmlTextWriter object</param>
        public new void RenderElement(HtmlTextWriter w)
        {
            if (ControlConfigureProvider.IsCountryCodeEnabled())
            {
                //text box's title need add water mark value.
                string[] titleValue = { ControlRenderUtil.GetToolTip(this), WatermarkText };
                this.ToolTip = DataUtil.ConcatStringWithSplitChar(titleValue, ACAConstant.BLANK);
                bool isRight2Left = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;

                w.Write("<div class='ACA_PhoneNumberLTR'>");
                w.Write("<div style='float:left;width:1.5em;'>");
                w.Write("(<span class='ACA_CountryCode_Sign'>+</span></div><div class='ACA_CountryCode'>");
                this.RenderChildren(w);
                w.Write("&nbsp;)</div>");
                w.Write("<div style='float:left;direction:");
                w.Write(isRight2Left ? "rtl" : "ltr");
                w.Write(";'>");
                RenderBeginTag(w);
                RenderEndTag(w);
                w.Write("</div><div style='clear:both;'></div></div>");
            }
            else
            {
                base.RenderElement(w);
            }
        }

        /// <summary>
        /// Set the min length for phone control
        /// </summary>
        /// <param name="minLength">min length.</param>
        public void SetClientMinLength(int minLength)
        {
            this.MinLength = minLength;
        }

        /// <summary>
        /// override AddAttributesToRender
        /// </summary>
        /// <param name="writer">HtmlTextWriter object</param>
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            if (!AccelaControlRender.IsAdminRender(this))
            {
                writer.AddAttribute("onblur", "ClearSplitChar(this, true)");
            }

            writer.AddAttribute("class", CssClass);
        }

        /// <summary>
        /// override ClearMaskOnLostFocus
        /// </summary>
        /// <returns>Clear mask when lost focus or not</returns>
        protected override bool ClearMaskOnLostFocus()
        {
            return false;
        }

        /// <summary>
        /// Create a Country Code control as a part of  phone number.the default value should be enabled.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (ControlConfigureProvider.IsCountryCodeEnabled())
            {
                txtCountryCode = new AccelaNumberText();
                txtCountryCode.Is4CountryCode = true;
                txtCountryCode.ID = this.ID + "_IDD";
                txtCountryCode.IsNeedDot = false;
                txtCountryCode.MaxLength = 3;
                txtCountryCode.Attributes.Add("style", "width:2.5em;");
                txtCountryCode.ToolTip = LabelConvertUtil.GetGlobalTextByKey("aca_phone_countrycode");
                txtCountryCode.FieldAlignment = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft
                                                    ? FieldAlignment.RTL
                                                    : FieldAlignment.LTR;
                Controls.Add(txtCountryCode);
            }
        }

        /// <summary>
        /// override ExpressionValidator
        /// </summary>
        /// <returns>RegularExpressionValidator object</returns>
        protected override RegularExpressionValidator ExpressionValidator()
        {
            if (!_isIgnoreValidate)
            {
                return base.ExpressionValidator();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// override LengthValidator
        /// </summary>
        /// <returns>TextLengthValidator object</returns>
        protected override TextLengthValidator LengthValidator()
        {
            if (!_isIgnoreValidate)
            {
                return base.LengthValidator();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.ValidChars = "-";
            this.CustomValidationMessageKey = "ACA_AccelaMaskText_InvalidValueMessage";
            HiddenField hfphoneMask = new HiddenField();
            hfphoneMask.ID = this.ID + "_phoneMask";
            hfphoneMask.Value = Mask;

            if (!this.Validate.ToLowerInvariant().Contains("maxlength"))
            {
                this.Validate += ";maxlength";
            }

            this.PhoneMaxLength = 40;

            Controls.Add(hfphoneMask);

            base.OnInit(e);
            this.EnsureChildControls();
        }

        /// <summary>
        /// Change CSS Style when disable or enable phone textbox
        /// </summary>
        private void ChangeCssClass()
        {
            this.CssClass = ControlConfigureProvider.IsCountryCodeEnabled() ? "ACA_NMLong" : "ACA_Medium";

            if (this.ReadOnly)
            {
                this.CssClass += " " + WebControlConstant.CSS_CLASS_READONLY;
            }

            // Store CSS into ViewState
            this.CssClassForEdit = this.CssClass;
        }

        #endregion Methods
    }
}