/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ValidatorCallbackExtender.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ValidatorCallbackExtender.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//// (c) Copyright Microsoft Corporation.
//// This source is subject to the Microsoft Permissive License.
//// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
//// All other rights reserved.

using System.ComponentModel;
using System.Web.UI;

[assembly: System.Web.UI.WebResource("Accela.Web.Controls.ValidatorCallback.ValidatorCallbackBehavior.js", "text/javascript")]

namespace AjaxControlToolkit
{
    /// <summary>
    /// Extender class for validator call back
    /// </summary>
    [Designer("AjaxControlToolkit.ValidatorCallbackDesigner, AjaxControlToolkit")]
    [RequiredScript(typeof(CommonToolkitScripts))]
    [RequiredScript(typeof(PopupExtender))]
    [TargetControlType(typeof(IValidator))]
    [ClientScriptResource("AjaxControlToolkit.ValidatorCallbackBehavior", "Accela.Web.Controls.ValidatorCallback.ValidatorCallbackBehavior.js")]
    public class ValidatorCallbackExtender : ExtenderControlBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets call back control id
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("callbackControlID")]
        public string CallbackControlID
        {
            get
            {
                Control callbackControl = FindControlHelper(GetPropertyValue("CallbackControlID", string.Empty));
                return callbackControl.ClientID;
            }

            set
            {
                SetPropertyValue("CallbackControlID", value);
            }
        }

        /// <summary>
        /// Gets or sets the current validate composite control child id or single control id.
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("currentValidateControlClientID")]
        public string CurrentValidateControlClientID
        {
            get
            {
                return GetPropertyValue("CurrentValidateControlClientID", string.Empty);
            }

            set
            {
                SetPropertyValue("CurrentValidateControlClientID", value);
            }
        }

        /// <summary>
        /// Gets or sets call back fail function
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("callbackFailFunction")]
        public string CallbackFailFunction
        {
            get
            {
                return GetPropertyValue("CallbackFailFunction", string.Empty);
            }

            set
            {
                SetPropertyValue("CallbackFailFunction", value);
            }
        }

        /// <summary>
        /// Gets or sets check control value validate function
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("checkControlValueValidateFunction")]
        public string CheckControlValueValidateFunction
        {
            get
            {
                return GetPropertyValue("CheckControlValueValidateFunction", string.Empty);
            }

            set
            {
                SetPropertyValue("CheckControlValueValidateFunction", value);
            }
        }

        /// <summary>
        /// Gets or sets high light CSS
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("highlightCssClass")]
        public string HighlightCssClass
        {
            get
            {
                return GetPropertyValue("HighlightCssClass", string.Empty);
            }

            set
            {
                SetPropertyValue("HighlightCssClass", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether high light CSS
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("validationByHiddenTextBox")]
        public bool ValidationByHiddenTextBox
        {
            get
            {
                return GetPropertyValue("ValidationByHiddenTextBox", false);
            }

            set
            {
                SetPropertyValue("ValidationByHiddenTextBox", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether high light CSS
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("validationIgnoreCase")]
        public bool ValidationIgnoreCase
        {
            get
            {
                return GetPropertyValue("ValidationIgnoreCase", false);
            }

            set
            {
                SetPropertyValue("ValidationIgnoreCase", value);
            }
        }

        #endregion Properties
    }
}