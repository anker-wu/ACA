#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: APOPopupBasePage.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: Provide the base page for APO popup page inherit.
 *
 *  Notes:
 *      $Id: APOPopupBasePage.cs 170366 2014-06-19 05:34:25Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Web.Common;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Provide the base page for APO popup page.
    /// </summary>
    public class APOPopupBasePage : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// contact session parameter
        /// </summary>
        private APOSessionParameterModel _apoSessionParameter;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the contact session parameter
        /// </summary>
        protected APOSessionParameterModel APOSessionParameter
        {
            get
            {
                if (AppSession.IsAdmin)
                {
                    _apoSessionParameter = new APOSessionParameterModel();
                }

                if (_apoSessionParameter == null)
                {
                    _apoSessionParameter = AppSession.GetAPOSessionParameter();
                }

                return _apoSessionParameter;
            }
        }

        /// <summary>
        /// Gets the callback function name.
        /// </summary>
        protected string CallbackFunctionName
        {
            get
            {
                return APOSessionParameter.CallbackFunctionName;
            }
        }

        #endregion Properties
    }
}