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
 *      $Id: CapDescriptionEdit.ascx.cs 277634 2014-08-19 08:16:23Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.Web.VO;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// CapDescriptionEdit user control for edit.
    /// </summary>
    public partial class CapDescriptionEdit : FormDesignerBaseControl
    {
        #region Fields

        /// <summary>
        /// long view id
        /// </summary>
        private const long VIEW_ID = 5028;

        /// <summary>
        /// is new additional info.
        /// </summary>
        private bool _isNewAddtionalInfor = false;

        /// <summary>
        /// indicate the the additional information form is editable or not.
        /// </summary>
        private bool _isEditable = true;

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="CapDescriptionEdit"/> class.
        /// </summary>
        public CapDescriptionEdit()
            : base(GviewID.CAPDescriptionEdit)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether current control whether only show fee relevant fields.
        /// the default value is false.
        /// </summary>
        public bool IsOnlyShowFeeField
        {
            get
            {
                return (string)ViewState["OnlyShowFeeField"] == "1" ? true : false;
            }

            set
            {
                ViewState["OnlyShowFeeField"] = value ? "1" : "0";
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether additional information is new.
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
                base.Permission.permissionLevel = GViewConstant.SECTION_CAPDESCRIPTION;
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
        /// <param name="additionalInfo">AdditionalInfo object to be presented to control.</param>
        public void DisplayAddtionalInfo(AddtionalInfo additionalInfo)
        {
            if (IsOnlyShowFeeField)
            {
                DisplayFeeRelevatFields(additionalInfo);
            }
            else
            {
                DisplayAllFields(additionalInfo);
            }

            if (!AppSession.IsAdmin)
            {
                DisableAddtionalInfoForm(IsEditable);
                txtJobValue.Text = txtJobValue.Text == ACAConstant.NONASSIGN_NUMBER ? ACAConstant.NONASSIGN_NUMBER : ModelUIFormat.FormatDataForJobValue(txtJobValue.GetInvariantDoubleText());
            }
        }

        /// <summary>
        /// Gets the additional information. JobValue property is only for UI presentation.
        /// if want to get the original job value,need to call JobValueModel.estimatedValue. 
        /// </summary>
        /// <param name="jobValueModel">job Value Model</param>
        /// <returns>AdditionalInfo object.</returns>
        public AddtionalInfo GetAdditionalInfo(BValuatnModel4WS jobValueModel)
        {
            AddtionalInfo model = new AddtionalInfo();
            model.JobValueModel = jobValueModel;
            model.HousingUnit = txtHousingUnit.Text;
            model.BuildingNumber = txtBuildingsNumbers.Text;
            model.PublicOwner = chkPublicOwned.Visible ? (chkPublicOwned.Checked ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N) : string.Empty;
            string constuctionType = ddlConstrucType.SelectedValue;
            int i = ddlConstrucType.SelectedValue.IndexOf('-');

            if (ddlConstrucType.SelectedValue.IndexOf('-') != -1)
            {
                constuctionType = ddlConstrucType.SelectedValue.Substring(0, i);
            }

            model.ConstructionType = constuctionType;
            double? nullableJobValue = txtJobValue.DoubleValue;
            model.JobValue = nullableJobValue == null ? string.Empty : I18nNumberUtil.FormatNumberForWebService(nullableJobValue.Value);

            return model;
        }

        /// <summary>
        /// Initial control. The general and detail information whether need to be marked as required should according to ACA config standard choice.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!IsOnlyShowFeeField)
            {
                DropDownListBindUtil.BindConstructType(ddlConstrucType);
            }

            ControlBuildHelper.AddValidationForStandardFields(GviewID.CAPDescriptionEdit, ModuleName, Permission, Controls);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                txtJobValue.Label = string.Format(GetTextByKey("capDescriptionEdit_label_jobValue"), I18nNumberUtil.CurrencySymbol);
            }

            hlEnd.NextControlClientID = SkippingToParentClickID;
            InitFormDesignerPlaceHolder(phContent);
        }

        /// <summary>
        /// Display all fields and value.
        /// </summary>
        /// <param name="additionalInfo">AdditionalInfo object to be presented to control.</param>
        private void DisplayAllFields(AddtionalInfo additionalInfo)
        {
            if (IsNewAddtionalInfor)
            {
                txtJobValue.Text = txtJobValue.Visible ? StandardChoiceUtil.GetDefaultJobValue() : ACAConstant.NONASSIGN_NUMBER;
                txtBuildingsNumbers.Text = string.Empty;
                txtHousingUnit.Text = string.Empty;
                chkPublicOwned.Checked = false;
                DropDownListBindUtil.SetSelectedToFirstItem(ddlConstrucType);
            }
            else if (!AppSession.IsAdmin)
            {
                txtJobValue.Text = I18nNumberUtil.ConvertMoneyFromWebServiceToInput(additionalInfo.JobValue);
                txtBuildingsNumbers.Text = additionalInfo.BuildingNumber;
                txtHousingUnit.Text = additionalInfo.HousingUnit;
                chkPublicOwned.Checked = additionalInfo.PublicOwner == ACAConstant.COMMON_Y;
                DropDownListBindUtil.SetSelectedValue(ddlConstrucType, additionalInfo.ConstructionType);
            }
        }

        /// <summary>
        /// Display fee relevant fields for obtain fee estimate.
        /// </summary>
        /// <param name="addtionalInfo">AdditionalInfo object to be presented to control.</param>
        private void DisplayFeeRelevatFields(AddtionalInfo addtionalInfo)
        {
            txtJobValue.Visible = true;

            if (IsNewAddtionalInfor)
            {
                string currentJobValueString = txtJobValue.Visible ? StandardChoiceUtil.GetDefaultJobValue() : ACAConstant.NONASSIGN_NUMBER;
                txtJobValue.Text = I18nNumberUtil.ConvertMoneyFromWebServiceToInput(currentJobValueString);
            }
            else
            {
                txtJobValue.Text = I18nNumberUtil.ConvertMoneyFromWebServiceToInput(addtionalInfo.JobValue);
            }
        }

        /// <summary>
        /// when set the IsEditable false in aca admin disable the contact form.
        /// </summary>
        /// <param name="isEditable">IsEditable Flag</param>
        private void DisableAddtionalInfoForm(bool isEditable)
        {
            if (!isEditable)
            {
                DisableEdit(this, null);
            }
        }

        #endregion Methods
    }
}
