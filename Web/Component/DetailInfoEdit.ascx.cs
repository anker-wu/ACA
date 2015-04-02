#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DetailInfoEdit.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DetailInfoEdit.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.Web.VO;
using Accela.ACA.WSProxy;

/// <summary>
/// the class for DetailInfoEdit.
/// </summary>
public partial class DetailInfoEdit : FormDesignerBaseControl
{
    #region Fields

    /// <summary>
    /// long view id
    /// </summary>
    private const long VIEW_ID = 5028;

    /// <summary>
    /// Is new addition information.
    /// </summary>
    private bool _isNewAddtionalInfor = false;

    /// <summary>
    /// Indicate the the detail information form is editable or not.
    /// </summary>
    private bool _isEditable = true;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="DetailInfoEdit"/> class.
    /// </summary>
    public DetailInfoEdit()
        : base(GviewID.DetailInformation)
    {
    }

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the additional information is new
    /// </summary>
    public bool IsNewAddtionalInfor
    {
        get
        {
            return _isNewAddtionalInfor;
        }

        set
        {
            _isNewAddtionalInfor = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether edit the form.
    /// </summary>
    public bool IsEditable
    {
        get
        {
            return _isEditable;
        }

        set
        {
            _isEditable = value;
        }
    }

    /// <summary>
    /// Gets or sets focus click id.
    /// </summary>
    public string SkippingToParentClickID
    {
        get
        {
            return ViewState["SkippingToParentClickID"] as string;
        }

        set
        {
            ViewState["SkippingToParentClickID"] = value;
        }
    }

    /// <summary>
    /// Gets or sets Permission.
    /// </summary>
    protected override GFilterScreenPermissionModel4WS Permission
    {
        get
        {
            base.Permission = new GFilterScreenPermissionModel4WS();
            base.Permission.permissionLevel = GViewConstant.SECTION_DETAIL;
            return base.Permission;
        }
        
        set
        {
            base.Permission = value;
        }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Display additional information to edit area control 
    /// </summary>
    /// <param name="addtionalInfo">AdditionalInfo object to be presented to control.</param>
    public void DisplayAddtionalInfo(AddtionalInfo addtionalInfo)
    {
        if (IsNewAddtionalInfor)
        {
            txtApplicationName.Text = string.Empty;
            txtDescriptionDetail.Text = string.Empty;
            txtDescriptionGeneral.Text = string.Empty;
        }
        else
        {
            txtApplicationName.Text = addtionalInfo.ApplicationName;
            txtDescriptionDetail.Text = addtionalInfo.DetailedDesc;
            txtDescriptionGeneral.Text = addtionalInfo.GeneralDesc;
        }

        if (!AppSession.IsAdmin)
        {
            DisableDetailInfoForm(IsEditable);
        }
    }

    /// <summary>
    /// Gets the additional information. JobValue property is only for UI presentation.
    /// if want to get the original job value,need to call JobValueModel.estimatedValue. 
    /// </summary>
    /// <param name="jobValueModel">job value model.</param>
    /// <returns>AdditionalInfo object.</returns>
    public AddtionalInfo GetAddtionalInfo(BValuatnModel4WS jobValueModel)
    {
        AddtionalInfo model = new AddtionalInfo();
        model.JobValueModel = jobValueModel;
        model.ApplicationName = txtApplicationName.Text.Trim();
        model.GeneralDesc = txtDescriptionGeneral.Text.Trim();
        model.DetailedDesc = txtDescriptionDetail.Text.Trim();

        return model;
    }

    /// <summary>
    /// Initial control. The general and detail information whether need to be marked as required should according to ACA config standard choice.
    /// </summary>
    /// <param name="e">EventArgs e</param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        ControlBuildHelper.AddValidationForStandardFields(GviewID.DetailInformation, ModuleName, Permission, Controls);
    }

    /// <summary>
    /// Page load event method. 
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">EventArgs e</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        hlEnd.NextControlClientID = SkippingToParentClickID;
        InitFormDesignerPlaceHolder(phContent);
    }

    /// <summary>
    /// when set the IsEditable false in aca admin disable the contact form.
    /// </summary>
    /// <param name="isEditable">IsEditable Flag</param>
    private void DisableDetailInfoForm(bool isEditable)
    {
        if (!isEditable)
        {
            DisableEdit(this, null);
        }
    }

    #endregion Methods
}
