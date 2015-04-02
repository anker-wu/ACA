#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaMaskText.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaMaskText.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI.WebControls;

using Accela.ACA.Common;

using AjaxControlToolkit;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a mask box to input masked text
    /// </summary>
    public class AccelaMaskText : AccelaTextBox
    {
        #region Fields

        /// <summary>
        /// accept negative
        /// </summary>
        private MaskedEditShowSymbol _acceptNegative = MaskedEditShowSymbol.None;

        /// <summary>
        /// is auto complete or not
        /// </summary>
        private bool _autoComplete;

        /// <summary>
        /// display money
        /// </summary>
        private MaskedEditShowSymbol _displayMoney;

        /// <summary>
        /// input direction
        /// </summary>
        private MaskedEditInputDirection _inputDirection = MaskedEditInputDirection.LeftToRight;

        /// <summary>
        /// is clear mask or not
        /// </summary>
        private bool _isClearMask;

        /// <summary>
        /// mask string .
        /// </summary>
        private string _mask;

        /// <summary>
        /// mask type.
        /// </summary>
        private MaskedEditType _maskType;

        /// <summary>
        /// ajax mask control filtered property
        /// </summary>
        private string _filtered;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the filtered.
        /// </summary>
        /// <value>The filtered.</value>
        public string Filtered
        {
            get
            {
                return _filtered;
            }

            set
            {
                _filtered = value;
            }
        }

        /// <summary>
        /// Gets or sets accept negative. Accepts the negative sign (-).
        ///     None - do not show the negative sign
        ///     Left - show the negative sign on the left of the mask
        ///     Right - show the negative sign on the right of the mask 
        /// </summary>
        public MaskedEditShowSymbol AcceptNegative
        {
            get
            {
                return _acceptNegative;
            }

            set
            {
                _acceptNegative = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to auto complete
        /// </summary>
        public virtual bool AutoComplete
        {
            get
            {
                return _autoComplete;
            }

            set
            {
                _autoComplete = value;
            }
        }

        /// <summary>
        /// Gets or sets DisplayMoney. How the currency symbol is displayed. The default value is None.
        /// None - do not show the currency symbol
        /// Left - show the currency symbol on the left of the mask
        /// Right - show the currency symbol on the right of the mask 
        /// </summary>
        public MaskedEditShowSymbol DisplayMoney
        {
            get
            {
                return _displayMoney;
            }

            set
            {
                _displayMoney = value;
            }
        }

        /// <summary>
        /// Gets or sets input direction
        /// </summary>
        public MaskedEditInputDirection InputDirection
        {
            get
            {
                return _inputDirection;
            }

            set
            {
                _inputDirection = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to clear the mask in the control when lost focus.
        /// </summary>
        public bool IsClearMask
        {
            get
            {
                return _isClearMask;
            }

            set
            {
                _isClearMask = value;
            }
        }

        /// <summary>
        /// Gets or sets Mask
        /// </summary>
        public virtual string Mask
        {
            get
            {
                return _mask;
            }

            set
            {
                _mask = value;
            }
        }

        /// <summary>
        /// Gets or sets Mask Type
        /// </summary>
        public virtual MaskedEditType MaskType
        {
            get
            {
                return _maskType;
            }

            set
            {
                _maskType = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show star character.
        /// </summary>
        public bool ShowStarCharacter
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Whether Clear Mask when Lost Focus.
        /// </summary>
        /// <returns>return true if it need to clear,otherwise return false</returns>
        protected virtual bool ClearMaskOnLostFocus()
        {
            return IsClearMask;
        }

        /// <summary>
        /// Get invalid value message
        /// </summary>
        /// <returns>invalid value message</returns>
        protected virtual string GetInvalidValueMessage()
        {
            //return "Invalid";
            return LabelConvertUtil.GetTextByKey("ACA_AccelaMaskText_InvalidValueMessage", GetModuleName());
        }

        /// <summary>
        /// override LengthValidator
        /// </summary>
        /// <returns>TextLengthValidator object</returns>
        protected override TextLengthValidator LengthValidator()
        {
            TextLengthValidator validator = base.LengthValidator();
            if (validator != null)
            {
                validator.Mask = Mask;
            }

            return validator;
        }

        /// <summary>
        /// Masked validator.
        /// </summary>
        /// <returns>MaskedEditValidator object</returns>
        protected virtual Tuple<MaskedEditExtender, MaskedEditValidator> MaskedValidator()
        {
            MaskedEditExtender maskedEditExtender = new MaskedEditExtender();
            maskedEditExtender.CultureName = ACA.Common.Util.I18nCultureUtil.UserPreferredCulture; //(*****I18nStamp:Culture*****)
            maskedEditExtender.ID = ID + "_ext";
            maskedEditExtender.TargetControlID = ID;
            maskedEditExtender.Mask = Mask;
            maskedEditExtender.MessageValidatorTip = true;
            maskedEditExtender.OnFocusCssClass = "MaskedEditFocus";
            maskedEditExtender.OnInvalidCssClass = "MaskedEditError";
            maskedEditExtender.MaskType = MaskType;
            maskedEditExtender.DisplayMoney = DisplayMoney;
            maskedEditExtender.AcceptNegative = AcceptNegative;
            maskedEditExtender.InputDirection = InputDirection;
            maskedEditExtender.ClearMaskOnLostFocus = ClearMaskOnLostFocus();
            maskedEditExtender.PromptCharacter = " ";
            maskedEditExtender.AutoComplete = AutoComplete;
            maskedEditExtender.Filtered = _filtered;
            maskedEditExtender.ShowStarCharacter = ShowStarCharacter;

            Controls.Add(maskedEditExtender);

            MaskedEditValidator maskedVad = new MaskedEditValidator();
            maskedVad.ID = this.ClientID + "_maskedit_vad";
            maskedVad.ControlToValidate = ID;
            maskedVad.ControlExtender = maskedEditExtender.ID;
            maskedVad.IsValidEmpty = true;
            maskedVad.EmptyValueMessage = string.Empty;
            maskedVad.InvalidValueMessage = GetInvalidValueMessage().Replace("'", "\\'");
            maskedVad.ValidationGroup = ValidationGroup;
            maskedVad.Display = ValidatorDisplay.None;
            maskedVad.TooltipMessage = TooltipMessage;

            Controls.Add(maskedVad);
            CreateValidatorCallbackExtender(maskedVad.ID);

            var result = new Tuple<MaskedEditExtender, MaskedEditValidator>(maskedEditExtender, maskedVad);
            return result;
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (!AccelaControlRender.IsAdminRender(this))
            {
                MaskedValidator();
            }

            if (string.IsNullOrEmpty(CssClass))
            {
                CssClass = "maskedfields";
            }
            else
            {
                CssClass += " maskedfields";
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Gets the current runtime page's module name.
        /// if current page doesn't belong to any module, returns string.Empty
        /// </summary>
        /// <returns>module name.</returns>
        protected string GetModuleName()
        {
            if (this.Page is IPage)
            {
                return (Page as IPage).GetModuleName();
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Methods
    }
}