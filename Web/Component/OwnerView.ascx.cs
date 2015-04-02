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
 *      $Id: OwnerView.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
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
    /// OwnerView user control, presents the specified owner information to UI.
    /// this control should aggregate TemplateView control.
    /// </summary>
    public partial class OwnerView : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Presents the OwnerModel information to control.
        /// </summary>
        /// <param name="owner">An OwnerModel data to be presented.</param>
        public void DisplayOwner(RefOwnerModel owner)
        {
            if (owner == null)
            {
                return;
            }

            InitialAllField();
            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.PERMISSION_APO,
                permissionValue = GViewConstant.SECTION_OWNER
            };

            SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, permission, GviewID.OwnerEdit);
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            if (!string.IsNullOrEmpty(owner.ownerTitle)
                && gviewBll.IsFieldVisible(models, "txtTitle"))
            {
                lblOwnerTitle.Text = owner.ownerTitle + ACAConstant.BLANK;
            }

            if (!string.IsNullOrEmpty(owner.ownerFullName)
                && gviewBll.IsFieldVisible(models, "txtName"))
            {
                lblOwnerName.Text = owner.ownerFullName;
            }

            if (!string.IsNullOrEmpty(owner.mailAddress1)
                && gviewBll.IsFieldVisible(models, "txtAddress1"))
            {
                lblAddress1.Text = owner.mailAddress1 + ACAConstant.BLANK;
            }

            if (!string.IsNullOrEmpty(owner.mailAddress2)
                && gviewBll.IsFieldVisible(models, "txtAddress2"))
            {
                lblAddress2.Text = owner.mailAddress2 + ACAConstant.BLANK;
            }

            if (!string.IsNullOrEmpty(owner.mailAddress3)
                && gviewBll.IsFieldVisible(models, "txtAddress3"))
            {
                lblAddress3.Text = owner.mailAddress3;
            }

            if (!string.IsNullOrEmpty(owner.mailCity)
                && gviewBll.IsFieldVisible(models, "txtCity"))
            {
                lblCity.Text = owner.mailCity;
            }

            if (!string.IsNullOrEmpty(owner.mailZip)
                && gviewBll.IsFieldVisible(models, "txtZip"))
            {
                lblZip.Text = ModelUIFormat.FormatZipShow(owner.mailZip, owner.mailCountry, false);
            }

            if (!string.IsNullOrEmpty(owner.mailState)
                && gviewBll.IsFieldVisible(models, "ddlAppState"))
            {
                lblState.Text = owner.mailState + ACAConstant.BLANK;
            }

            if (!string.IsNullOrEmpty(owner.mailCountry)
                && gviewBll.IsFieldVisible(models, "ddlCountry"))
            {
                IblCountry.Text = StandardChoiceUtil.GetCountryByKey(owner.mailCountry);
            }

            if (!string.IsNullOrEmpty(owner.phone)
                && gviewBll.IsFieldVisible(models, "txtPhone"))
            {
                string temp = ModelUIFormat.FormatPhoneShow(owner.phoneCountryCode, owner.phone, owner.mailCountry);
                IblPhone.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_owneredit_phone"), temp);
            }

            if (!string.IsNullOrEmpty(owner.fax)
                && gviewBll.IsFieldVisible(models, "txtFax"))
            {
                string temp = ModelUIFormat.FormatPhoneShow(owner.faxCountryCode, owner.fax, owner.mailCountry);
                IblFax.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_owneredit_fax"), temp);
            }

            if (!string.IsNullOrEmpty(owner.email)
                && gviewBll.IsFieldVisible(models, "txtEmail"))
            {
                IblEmail.Text = owner.email;
            }

            ModelUIFormat.HiddenEmptyRow(tbOwnerDetail);

            templateView.DisplayAttributes(owner.templates);
        }

        /// <summary>
        /// if not initial all field the view will display previous value.
        /// </summary>
        private void InitialAllField()
        {
            lblOwnerTitle.Text = string.Empty;
            lblOwnerName.Text = string.Empty;
            lblAddress1.Text = string.Empty;
            lblAddress2.Text = string.Empty;
            lblAddress3.Text = string.Empty;
            lblCity.Text = string.Empty;
            lblZip.Text = string.Empty;
            lblState.Text = string.Empty;
            IblCountry.Text = string.Empty;
            IblPhone.Text = string.Empty;
            IblFax.Text = string.Empty;
            IblEmail.Text = string.Empty;
        }

        #endregion Methods
    }
}