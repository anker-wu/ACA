#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaNonInputCompositeControl.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description: The base class of those non input composite controls.
 *
 *  Notes:
 * $Id: AccelaNonInputCompositeControl.cs 238998 2012-12-04 09:10:00Z ACHIEVO\alan.hu $.
 *  Revision History
 *  Date,            Who,        What
 *  Dec 4, 2012      Alan Hu     Initial.
 * </pre>
 */

#endregion Header

using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;

namespace Accela.Web.Controls
{
    /// <summary>
    /// The base class of those non input composite controls. such as Label/Button/Section Header...
    /// </summary>
    public abstract class AccelaNonInputCompositeControl : CompositeControl, IAccelaNonInputControl
    {
        #region Fields

        /// <summary>
        /// Local field for <see cref="IsDisplayLabel"/> property.
        /// </summary>
        private bool _isDisplayLabel = true;

        /// <summary>
        /// Local field for <see cref="SubLabel"/> property.
        /// </summary>
        private string _subLabel;

        #endregion

        #region Properties(Implemented from IAccelaNonInputControl)

        /// <summary>
        /// Gets or sets a value indicating whether need to display the control label.
        /// </summary>
        public virtual bool IsDisplayLabel
        {
            get
            {
                return _isDisplayLabel;
            }

            set
            {
                _isDisplayLabel = value;
            }
        }

        /// <summary>
        /// Gets or sets the control label key.
        /// </summary>
        public virtual string LabelKey
        {
            get;
            set;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the control's instructions.
        /// </summary>
        public virtual string SubLabel
        {
            get
            {
                return _subLabel;
            }

            set
            {
                _subLabel = ScriptFilter.FilterScript(value, false);
            }
        }

        #endregion

        #region Methods(Implemented from IAccelaNonInputControl)

        /// <summary>
        /// Gets the original label for current language.
        /// </summary>
        /// <returns>string of label.</returns>
        public virtual string GetDefaultLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return ScriptFilter.FilterScript(LabelConvertUtil.GetGUITextByKey(LabelKey), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the customized label for default language, the default language is defined in I18n settings.
        /// </summary>
        /// <returns>string of label.</returns>
        public virtual string GetDefaultLanguageText()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return ScriptFilter.FilterScript(LabelConvertUtil.GetDefaultLanguageTextByKey(LabelKey, GetModuleName()), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the original sub label for current language.
        /// </summary>
        /// <returns>string of sub label.</returns>
        public virtual string GetDefaultSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return ScriptFilter.FilterScript(LabelConvertUtil.GetGUITextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the customized sub label for default language, the default language is defined in I18n settings.
        /// </summary>
        /// <returns>string of sub label.</returns>
        public virtual string GetDefaultLanguageSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return ScriptFilter.FilterScript(LabelConvertUtil.GetDefaultLanguageGlobalTextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets customized sub label for current language.
        /// </summary>
        /// <returns>string of sub label.</returns>
        public virtual string GetSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey) && string.IsNullOrEmpty(SubLabel))
            {
                SubLabel = LabelConvertUtil.GetTextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX, this);
            }

            return SubLabel;
        }

        /// <summary>
        /// Initial the control extenders. 
        /// </summary>
        public abstract void InitExtenderControl();

        #endregion

        #region Methods

        /// <summary>
        /// Gets current module name.
        /// </summary>
        /// <returns>string of module.</returns>
        protected virtual string GetModuleName()
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

        #endregion
    }
}