#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SearchVEMap.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 *
 *  Description:
 *
 *  Notes:
 *      $Id: SearchVEMap.ascx.cs 132295 2009-05-25 02:05:22Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ablity to operation SearchVEMap.
    /// </summary>
    public partial class SearchVEMap : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// address information.
        /// </summary>
        private string _addressInfo = string.Empty;

        /// <summary>
        /// bind VEMapArgs list.
        /// </summary>
        private IList<VEMapArgs> _bindList;

        /// <summary>
        /// not address number.
        /// </summary>
        private int _noAddressNumber = 0;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets bind list.
        /// </summary>
        public IList<VEMapArgs> BindList
        {
            set
            {
                _bindList = value;
                BindToControl(value);

                if (IsPostBack)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "VEMapKeyS", "setPostBackInfoS('" + ScriptFilter.FilterJSChar(_addressInfo) + "','" + _noAddressNumber + "');searchedS=false;FindS()", true);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether need to display Map on page.
        /// </summary>
        protected bool IsShowMap
        {
            get
            {
                // Admin mode, Hidden the MAP because the slow to load VE scripts from the third site.
                if (AppSession.IsAdmin)
                {
                    return false;
                }

                return StandardChoiceUtil.IsShowMap(this.ModuleName);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// parse and bind key value list to control
        /// format:adress:{permitnumber&capid1_capid2_capid3&permittype|...};...
        /// </summary>
        /// <param name="permitsByPerAddress">permits by PerAddress</param>
        private void BindToControl(IList<VEMapArgs> permitsByPerAddress)
        {
            if (permitsByPerAddress == null || permitsByPerAddress.Count == 0)
            {
                this.hiddenAddressS.Attributes.Add("NoAddressNumber", _noAddressNumber.ToString());
                return;
            }

            _noAddressNumber = 0;

            StringBuilder sbPermits = new StringBuilder();
            bool isFirst = true;

            foreach (VEMapArgs item in permitsByPerAddress)
            {
                if (item.Address == String.Empty)
                {
                    //_addressInfo = string.Concat(_addressInfo, item.Address, ":", BuildPermitsString(item.Permits));

                    // All permits without address
                    _noAddressNumber = item.Permits.Count;
                }
                else
                {
                    if (!isFirst)
                    {
                        // Address info or push pin information is separated by ACAConstant.SPLIT_CHAR1
                        sbPermits.Append(ACAConstant.SPLIT_CHAR1);
                    }
                    else
                    {
                        isFirst = false;
                    }

                    // eg: address1 SPLIT_CHAR2 permit11 SPLIT_CHAR3 permit12 SPLIT_CHAR1 address2 SPLIT_CHAR2 permit11 SPLIT_CHAR3 permit12
                    sbPermits.Append(item.Address);
                    sbPermits.Append(ACAConstant.SPLIT_CHAR2); // separates address infor and many permits
                    sbPermits.Append(BuildPermitsString(item.Permits));

                    //_addressInfo = String.Concat(_addressInfo, ACAConstant.SPLIT_CHAR1, item.Address, ":", BuildPermitsString(item.Permits));
                }
            }

            this.hiddenAddressS.Value = sbPermits.ToString();
            _addressInfo = sbPermits.ToString();
            this.hiddenAddressS.Attributes.Add("NoAddressNumber", _noAddressNumber.ToString());
        }

        /// <summary>
        /// get the number and type string of permit collection
        /// </summary>
        /// <param name="permitInfos">permit Infos</param>
        /// <returns>build permit string.</returns>
        private string BuildPermitsString(IList<KeyValuePair<string, string>> permitInfos)
        {
            bool isFirst = true;

            StringBuilder sbPermits = new StringBuilder();

            foreach (KeyValuePair<string, string> item in permitInfos)
            {
                if (!isFirst)
                {
                    // separate permits by ACAConstant.SPLIT_CHAR3
                    sbPermits.Append(ACAConstant.SPLIT_CHAR3);
                }
                else
                {
                    isFirst = false;
                }

                sbPermits.Append(item.Value);
            }

            return sbPermits.ToString();
        }

        #endregion Methods
    }
}