#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressView.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description: Display work location information
 *
 *  Notes:
 *      $Id: AddressView.ascx.cs 277585 2014-08-18 11:09:47Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for AddressView.
    /// </summary>
    public partial class AddressView : BaseUserControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether this control is displayed in the confirm page.
        /// </summary>
        public bool IsInConfirmPage
        {
            get;
            set;
        }

        #region Methods

        /// <summary>
        /// Get or set address information
        /// </summary>
        /// <param name="address">a AddressModel</param>
        public void Display(AddressModel address)
        {
            Display(address, true);
        }

        /// <summary>
        /// Gets or sets address information
        /// </summary>
        /// <param name="address">a AddressModel</param>
        /// <param name="isShowTemplate">if true,show address template</param>
        public void Display(AddressModel address, bool isShowTemplate)
        {
            if (address != null)
            {
                GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                {
                    permissionLevel = GViewConstant.PERMISSION_APO,
                    permissionValue = GViewConstant.SECTION_ADDRESS
                };

                SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, permission, GviewID.AddressEdit);
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;
                lblAddress.Text = addressBuilderBll.BuildAddressByFormatType(address, models, AddressFormatType.LONG_ADDRESS_WITH_FORMAT);

                if (IsInConfirmPage)
                {
                    lblAddress.CssClass += " font11px color666";
                }

                //We needn't show address template in pages: InspectionScheduleConfirm.aspx and InspectionSchedule.aspx
                if (isShowTemplate)
                {
                    templateView.DisplayAttributes(address.templates);
                }
            }
            else
            {
                lblAddress.Text = string.Empty;
            }
        }

        #endregion Methods
    }
}