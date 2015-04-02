#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaFeinText.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  AccelaFeinText is used for social security information.
 * 
 *  Notes:
 * $Id: AccelaFeinText.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a text box to input Fein.
    /// </summary>
    public class AccelaFeinText : AccelaMaskText, IAccelaControl
    {
        #region Fields

        /// <summary>
        /// indicates validate Fein format or not.
        /// </summary>
        private bool _isIgnoreValidate;

        /// <summary>
        /// is client always editable,default value is false;
        /// </summary>
        private bool _isAlwaysEditable = false;

        /// <summary>
        /// The converter used to convert the mask of AA(standard choice) to ACA's
        /// </summary>
        private MaskConverter _maskConverter = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether validate Fein format or not.
        /// </summary>
        public bool IsIgnoreValidate
        {
            get
            {
                string maskWithNoDefaultValue = MaskUtil.GetMaskFromAA(BizDomainConstant.STD_MASKS_ITEM_FEIN);

                if (string.IsNullOrEmpty(maskWithNoDefaultValue))
                {
                    _isIgnoreValidate = true;
                }

                return _isIgnoreValidate;
            }

            set
            {
                _isIgnoreValidate = value;
            }
        }

        /// <summary>
        /// Gets the text mask.
        /// </summary>
        public override string Mask
        {
            get
            {
                return _maskConverter.Mask;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is always editable in client.
        /// </summary>
        public new bool IsAlwaysEditable
        {
            get
            {
                return _isAlwaysEditable;
            }

            set
            {
                _isAlwaysEditable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value for validating the illegal character in the SSN field.
        /// </summary>
        public override string ValidationExpression
        {
            get
            {
                return _maskConverter.ValidationExpression;
            }
        }

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

        /// <summary>
        /// override ClearMaskOnLostFocus
        /// </summary>
        /// <returns>Clear mask when lost focus or not</returns>
        protected override bool ClearMaskOnLostFocus()
        {
            return false;
        }

        /// <summary>
        /// override ExpressionValidator
        /// </summary>
        /// <returns>RegularExpressionValidator object</returns>
        protected override RegularExpressionValidator ExpressionValidator()
        {
            if (!IsIgnoreValidate)
            {
                //set the mask to validation message, to guide the user input.
                AddionalExpressionValidateMessage = string.Format(
                    LabelConvertUtil.GetGlobalTextByKey("aca_fein_invalidmessage"), 
                    _maskConverter.MaskFormat);

                return base.ExpressionValidator();
            }
            else
            {
                return null;
            }
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
                writer.AddAttribute("onblur", "ClearSplitChar(this, false)");
            }
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            // Initialize the mask
            _maskConverter = new MaskConverter(MaskUtil.FEINMaskFromAA, Filtered);
            ShowStarCharacter = _maskConverter.ShowStarCharacter;

            base.OnInit(e);
        }

        #endregion Methods
    }
}
