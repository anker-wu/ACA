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
 *      $Id: CapDescriptionView.ascx.cs 277634 2014-08-19 08:16:23Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Globalization;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.VO;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// CapDescriptionView user control to present additional information.
    /// </summary>
    public partial class CapDescriptionView : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// long view id
        /// </summary>
        private const long VIEW_ID = 5027;

        #endregion Fields

        #region Methods

        /// <summary>
        /// display detail information accord by Object Additional Info
        /// </summary>
        /// <param name="detailInfo">Additional Info</param>
        public void Display(AddtionalInfo detailInfo)
        {
            if (AppSession.IsAdmin)
            {
                divContent.Visible = false;

                return;
            }

            if (detailInfo == null)
            {
                return;
            }

            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.SECTION_CAPDESCRIPTION
            };

            SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, permission, GviewID.CAPDescriptionEdit);
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            if (gviewBll.IsFieldVisible(models, "txtJobValue"))
            {
                this.lbljobValue.Text = ModelUIFormat.FormatDataForJobValue(detailInfo.JobValue);
            }

            if (gviewBll.IsFieldVisible(models, "txtHousingUnit"))
            {
                this.lblHouseUnit.Text = ScriptFilter.FilterScript(detailInfo.HousingUnit);
            }

            if (gviewBll.IsFieldVisible(models, "txtBuildingsNumbers"))
            {
                this.lblBuildNumber.Text = ScriptFilter.FilterScript(detailInfo.BuildingNumber);
            }

            if (gviewBll.IsFieldVisible(models, "chkPublicOwned"))
            {
                this.lblPublicOwner.Text = ModelUIFormat.FormatYNLabel(detailInfo.PublicOwner);
            }

            if (gviewBll.IsFieldVisible(models, "ddlConstrucType"))
            {
                this.lblConstructType.Text = ScriptFilter.FilterScript(CapUtil.GetOneConstuctionTypeDescription(detailInfo.ConstructionType, ConfigManager.AgencyCode));
            }

            divJob.Visible = !string.IsNullOrEmpty(lbljobValue.Text);
            lblHouseUnit.Visible = !string.IsNullOrEmpty(lblHouseUnit.Text);
            tdHouseUnitLabel.Visible = lblHouseUnit.Visible;
            tdHouseUnitBox.Visible = lblHouseUnit.Visible;
            divSpearFrom.Visible = lblHouseUnit.Visible;
            tdSpearFrom.Visible = lblHouseUnit.Visible;
            capDescriptionView_label_houseUnit.Visible = lblHouseUnit.Visible;
            lblBuildNumber.Visible = !string.IsNullOrEmpty(lblBuildNumber.Text);
            capDescriptionView_label_buildNumber.Visible = lblBuildNumber.Visible;
            divHourse.Visible = lblHouseUnit.Visible || lblBuildNumber.Visible;
            divOwner.Visible = !string.IsNullOrEmpty(lblPublicOwner.Text);
            divConstructType.Visible = !string.IsNullOrEmpty(lblConstructType.Text);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                capDescriptionView_label_jobValue.Text = string.Format(GetTextByKey("capDescriptionEdit_label_jobValue"), I18nNumberUtil.CurrencySymbol);
                capDescriptionView_label_publicOwner.Text = GetTextByKey("capDescriptionEdit_label_publicOwner") + ":";
            }
        }

        #endregion Methods
    }
}