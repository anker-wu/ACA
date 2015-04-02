#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ParcelView.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// ParcelView user control, presents the specified parcel information to UI.
    /// this control should aggregate TemplateView control.
    /// </summary>
    public partial class ParcelView : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Presents the CapParcelModel information to control.
        /// </summary>
        /// <param name="capParcel">An CapParcelModel data to be presented.</param>
        public void DisplayParcel(CapParcelModel capParcel)
        {
            if (capParcel == null)
            {
                return;
            }

            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.PERMISSION_APO,
                permissionValue = GViewConstant.SECTION_PARCEL
            };

            ParcelModel parcel = capParcel.parcelModel;
            SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, permission, GviewID.ParcelEdit);
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            txtParcelNo.Text = ScriptFilter.FilterScript(parcel.parcelNumber);
            divParcelNo.Visible = !string.IsNullOrEmpty(txtParcelNo.Text)
                && gviewBll.IsFieldVisible(models, "txtParcelNo");

            txtLot.Text = ScriptFilter.FilterScript(parcel.lot);
            divLot.Visible = !string.IsNullOrEmpty(txtLot.Text)
                && gviewBll.IsFieldVisible(models, "txtLot");

            txtBlock.Text = ScriptFilter.FilterScript(parcel.block);
            divBlock.Visible = !string.IsNullOrEmpty(txtBlock.Text)
                && gviewBll.IsFieldVisible(models, "txtBlock");

            txtSubdivision.Text = ScriptFilter.FilterScript(I18nStringUtil.GetString(parcel.resSubdivision, parcel.subdivision));
            divSubdivision.Visible = !string.IsNullOrEmpty(txtSubdivision.Text)
                && gviewBll.IsFieldVisible(models, "ddlSubdivision");

            txtBook.Text = ScriptFilter.FilterScript(parcel.book);
            divBook.Visible = !string.IsNullOrEmpty(txtBook.Text)
                && gviewBll.IsFieldVisible(models, "txtBook");

            txtPage.Text = ScriptFilter.FilterScript(parcel.page);
            divPage.Visible = !string.IsNullOrEmpty(txtPage.Text)
                && gviewBll.IsFieldVisible(models, "txtPage");

            txtTract.Text = ScriptFilter.FilterScript(parcel.tract);
            divTract.Visible = !string.IsNullOrEmpty(txtTract.Text)
                && gviewBll.IsFieldVisible(models, "txtTract");

            txtLegalDescription.Text = ScriptFilter.FilterScript(parcel.legalDesc);
            divLegalDescription.Visible = !string.IsNullOrEmpty(txtLegalDescription.Text)
                && gviewBll.IsFieldVisible(models, "txtLegalDescription");

            txtParcelArea.Text = ScriptFilter.FilterScript(Convert.ToString(parcel.parcelArea));
            divParcelArea.Visible = !string.IsNullOrEmpty(txtParcelArea.Text)
                && gviewBll.IsFieldVisible(models, "txtParcelArea");

            txtLandValue.Text = ScriptFilter.FilterScript(Convert.ToString(parcel.landValue));
            divLandValue.Visible = !string.IsNullOrEmpty(txtLandValue.Text)
                && gviewBll.IsFieldVisible(models, "txtLandValue");

            txtImprovedValue.Text = ScriptFilter.FilterScript(Convert.ToString(parcel.improvedValue));
            divImprovedValue.Visible = !string.IsNullOrEmpty(txtImprovedValue.Text)
                && gviewBll.IsFieldVisible(models, "txtImprovedValue");

            txtExceptionValue.Text = ScriptFilter.FilterScript(Convert.ToString(parcel.exemptValue));
            divExceptionValue.Visible = !string.IsNullOrEmpty(txtExceptionValue.Text)
                && gviewBll.IsFieldVisible(models, "txtExceptionValue");

            templateView.DisplayAttributes(parcel.templates);
            if (parcel.templates != null)
            {
                divTemplateView.Visible = true;
            }
            else
            {
                divTemplateView.Visible = false;
            }
        }

        #endregion Methods
    }
}