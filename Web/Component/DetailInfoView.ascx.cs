#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DetailInfoView.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DetailInfoView.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.VO;
using Accela.ACA.WSProxy;

/// <summary>
/// the class for DetailInfoView.
/// </summary>
public partial class DetailInfoView : BaseUserControl
{
    #region Methods

    /// <summary>
    /// display detail information accord by Object Additional Info
    /// </summary>
    /// <param name="detailInfo">Additional Info</param>
    public void Display(AddtionalInfo detailInfo)
    {
        if (AppSession.IsAdmin)
        {
            tblContent.Visible = false;

            return;
        }

        if (detailInfo == null)
        {
            return;
        }

        InitialAllField();

        GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
        {
            permissionLevel = GViewConstant.SECTION_ADDITIONAL_INFO
        };

        SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, permission, GviewID.DetailInformation);
        IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

        if (!string.IsNullOrEmpty(detailInfo.ApplicationName)
            && gviewBll.IsFieldVisible(models, "txtApplicationName"))
        {
            lblApplicationName.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("capDescriptionEdit_label_AppName"), detailInfo.ApplicationName);
        }

        if (!string.IsNullOrEmpty(detailInfo.GeneralDesc)
            && gviewBll.IsFieldVisible(models, "txtDescriptionGeneral"))
        {
            lblnote.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("capDescriptionEdit_label_note"), detailInfo.GeneralDesc);
        }

        if (!string.IsNullOrEmpty(detailInfo.DetailedDesc)
            && gviewBll.IsFieldVisible(models, "txtDescriptionDetail"))
        {
            lblDetaileDescription.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_permitReg_label_descriptionDetailEdit"), detailInfo.DetailedDesc);
        }

        ModelUIFormat.HiddenEmptyRow(tbDetailInfoView);
    }

    /// <summary>
    /// if not initial all field the view will display previous value.
    /// </summary>
    private void InitialAllField()
    {
        lblApplicationName.Text = string.Empty;
        lblnote.Text = string.Empty;
        lblDetaileDescription.Text = string.Empty;
    }

    #endregion Methods
}