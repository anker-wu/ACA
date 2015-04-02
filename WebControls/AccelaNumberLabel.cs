#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaNumberLabel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaNumberText.cs 206995 2011-11-09 01:09:44Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common.Util;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Number label type.
    /// </summary>
    public enum NumberLabelTypeEnum
    {
        /// <summary>
        /// Numeric type
        /// </summary>
        Number,

        /// <summary>
        /// Money type
        /// </summary>
        Money
    }

    /// <summary>
    /// Accela Numeric or Currency Label
    /// </summary>
    public class AccelaNumberLabel : AccelaLabel
    {
        /// <summary>
        /// Gets or sets the type of the label.
        /// </summary>
        /// <value>The type of the label.</value>
        public NumberLabelTypeEnum NumberLabelType
        {
            get; 
            set;
        }

        /// <summary>
        /// Sets the numeric text.
        /// </summary>
        /// <value>The numeric text.</value>
        public object NumericText
        {
            set
            {
                Text = ConvertToString(value);
            }
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>the format numeric string</returns>
        private string ConvertToString(object value)
        {
            string result = string.Empty;

            if (NumberLabelType == NumberLabelTypeEnum.Money)
            {
                result = I18nNumberUtil.FormatMoneyForUI(value);
            }
            else if (NumberLabelType == NumberLabelTypeEnum.Number)
            {
                result = I18nNumberUtil.FormatNumberForUI(value);
            }

            if (string.IsNullOrEmpty(result))
            {
                result = value.ToString();
            }

            return result;
        }
    }
}
