﻿﻿#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressLookupForm.acsx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AddressLookupForm.ascx.cs  
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.WSProxy;


namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Address Lookup Edit
    /// </summary>
    public partial class AddressLookupForm : FormDesignerBaseControl
    {
        #region Properties
        /// <summary>
        /// Gets user control client id
        /// </summary>
        public string UserControlClientID
        {
            get
            {
                return txtAPO_Search_by_Address_StreetNumber.ClientID.Replace("txtAPO_Search_by_Address_StreetNumber", string.Empty);
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the AddressLookupEdit class.
        /// </summary>
        public AddressLookupForm()
            : base(GviewID.LookUpByAddress)
        {
        }

        #endregion Constructors

        #region Method
        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyRegionalSetting(false);
            // Initial the PlaceHolder's properties
            phContent.TemplateControlIDPrefix = ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX;
            InitFormDesignerPlaceHolder(phContent);
        }
        /// <summary>
        /// Get address hash table according to user's input
        /// </summary>
        /// <returns>Hashtable</returns>
        public Hashtable GetAddress()
        {
            Hashtable htAddress = new Hashtable(12);
            htAddress.Add("txtAPO_Search_by_Address_StreetNumberFrom", txtAPO_Search_by_Address_StreetNumber.TextFrom.Trim());
            htAddress.Add("txtAPO_Search_by_Address_StreetNumberTo", txtAPO_Search_by_Address_StreetNumber.TextTo.Trim());
            htAddress.Add("ddlAPO_Search_by_Address_Direction", ddlAPO_Search_by_Address_Direction.SelectedValue.Trim());
            htAddress.Add("txtAPO_Search_by_Address_StreetName", txtAPO_Search_by_Address_StreetName.Text.Trim());
            htAddress.Add("ddlAPO_Search_by_Address_UnitType", ddlAPO_Search_by_Address_UnitType.SelectedValue.Trim());
            htAddress.Add("txtAPO_Search_by_Address_UnitNo", txtAPO_Search_by_Address_UnitNo.Text.Trim());
            htAddress.Add("txtAPO_Search_by_Address_City", txtAPO_Search_by_Address_City.Text.Trim());
            htAddress.Add("ddlAPO_Search_by_Address_State", ddlAPO_Search_by_Address_State.Text.Trim());
            htAddress.Add("txtAPO_Search_by_Address_Zip", txtAPO_Search_by_Address_Zip.GetZip(ddlAPOAddressCountry.SelectedValue.Trim()));
            htAddress.Add("ddlStreetSuffix", ddlStreetSuffix.SelectedValue.Trim());
            htAddress.Add("ddlStreetSuffixDirection", ddlStreetSuffixDirection.SelectedValue.Trim());
            htAddress.Add("ddlAPOAddressCountry", ddlAPOAddressCountry.SelectedValue.Trim());
            htAddress.Add("ddlAPO_Search_by_Address_Status", ddlAPO_Search_by_Address_Status.Checked);
            TemplateAttributeModel[] templateAtrributes = templateEdit.GetAttributeModels();
            htAddress.Add("templateAtrributes", templateAtrributes);

            htAddress.Add("txtLevelPrefix", txtLevelPrefix.Text.Trim());
            htAddress.Add("txtLevelNbrStart", txtLevelNbrStart.Text.Trim());
            htAddress.Add("txtLevelNbrEnd", txtLevelNbrEnd.Text.Trim());
            htAddress.Add("txtHouseAlphaStart", txtHouseAlphaStart.Text.Trim());
            htAddress.Add("txtHouseAlphaEnd", txtHouseAlphaEnd.Text.Trim());
            htAddress.Add("txtStartFraction", txtStartFraction.Text.Trim());
            htAddress.Add("txtEndFraction", txtEndFraction.Text.Trim());

            return htAddress;
        }

        /// <summary>
        /// Gets or sets Permission.
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = GViewConstant.PERMISSION_APO;
                base.Permission.permissionValue = GViewConstant.SECTION_ADDRESS;
                return base.Permission;
            }
            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// OnInit event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);                          
            ControlBuildHelper.AddValidationForStandardFields(GviewID.LookUpByAddress, ModuleName, Permission, Controls);

            ddlAPOAddressCountry.BindItems();
            ddlAPOAddressCountry.SetCountryControls(txtAPO_Search_by_Address_Zip, ddlAPO_Search_by_Address_State, null);
        }

        /// <summary>
        /// Get ref address model by user input
        /// </summary>
        /// <returns>RefAddressModel object</returns>
        public RefAddressModel GetRefAddress()
        {
            RefAddressModel addressModel = new RefAddressModel();
            addressModel.streetDirection = ddlAPO_Search_by_Address_Direction.SelectedValue.Trim();
            addressModel.streetName = txtAPO_Search_by_Address_StreetName.Text.Trim();
            addressModel.unitType = ddlAPO_Search_by_Address_UnitType.SelectedValue.Trim();
            addressModel.unitStart = txtAPO_Search_by_Address_UnitNo.Text.Trim();
            addressModel.city = txtAPO_Search_by_Address_City.Text.Trim();
            addressModel.state = ddlAPO_Search_by_Address_State.Text.Trim();
            addressModel.zip = txtAPO_Search_by_Address_Zip.GetZip(ddlAPOAddressCountry.SelectedValue.Trim());
            addressModel.streetSuffix = ddlStreetSuffix.SelectedValue.Trim();
            addressModel.streetSuffixdirection = ddlStreetSuffixDirection.SelectedValue.Trim();
            addressModel.countryCode = ddlAPOAddressCountry.SelectedValue.Trim();
            addressModel.auditStatus = ddlAPO_Search_by_Address_Status.Checked ? string.Empty : ACAConstant.VALID_STATUS;
            addressModel.templates = templateEdit.GetAttributeModels();

            addressModel.levelPrefix = txtLevelPrefix.Text.Trim();
            addressModel.levelNumberStart = txtLevelNbrStart.Text.Trim();
            addressModel.levelNumberEnd = txtLevelNbrEnd.Text.Trim();
            addressModel.houseNumberAlphaStart = txtHouseAlphaStart.Text.Trim();
            addressModel.houseNumberAlphaEnd = txtHouseAlphaEnd.Text.Trim();
            addressModel.houseFractionStart = txtStartFraction.Text.Trim();
            addressModel.houseFractionEnd = txtEndFraction.Text.Trim();

            // If only one of range search parameter inputed, than do not use range search. And use only one parameter to fix search.
            Range<int?> streetNoStartRange = Range<int>.GetRangeValue(
                                                            txtAPO_Search_by_Address_StreetNumber.TextFrom,
                                                            txtAPO_Search_by_Address_StreetNumber.TextTo);

            addressModel.houseNumberStart = streetNoStartRange.SingleValue;
            addressModel.houseNumberStartFrom = streetNoStartRange.LowerBound;
            addressModel.houseNumberStartTo = streetNoStartRange.UpperBound;

            return addressModel;
        }

        /// <summary>
        /// Set address value according to hashtable content
        /// </summary>
        /// <param name="htAddress">Hashtable</param>
        public void SetAddressValue(Hashtable htAddress)
        {
            DropDownListBindUtil.SetCountrySelectedValue(ddlAPOAddressCountry, htAddress["ddlAPOAddressCountry"].ToString(), false, true, true);
            txtAPO_Search_by_Address_StreetNumber.TextFrom = htAddress["txtAPO_Search_by_Address_StreetNumberFrom"].ToString();
            txtAPO_Search_by_Address_StreetNumber.TextTo = htAddress["txtAPO_Search_by_Address_StreetNumberTo"].ToString();
            ddlAPO_Search_by_Address_Direction.SelectedValue = htAddress["ddlAPO_Search_by_Address_Direction"].ToString();
            txtAPO_Search_by_Address_StreetName.Text = htAddress["txtAPO_Search_by_Address_StreetName"].ToString();
            ddlAPO_Search_by_Address_UnitType.SelectedValue = htAddress["ddlAPO_Search_by_Address_UnitType"].ToString();
            txtAPO_Search_by_Address_UnitNo.Text = htAddress["txtAPO_Search_by_Address_UnitNo"].ToString();
            txtAPO_Search_by_Address_City.Text = htAddress["txtAPO_Search_by_Address_City"].ToString();
            ddlAPO_Search_by_Address_State.Text = htAddress["ddlAPO_Search_by_Address_State"].ToString();
            txtAPO_Search_by_Address_Zip.Text = ModelUIFormat.FormatZipShow(htAddress["txtAPO_Search_by_Address_Zip"].ToString(), htAddress["ddlAPOAddressCountry"].ToString());
            ddlStreetSuffix.SelectedValue = htAddress["ddlStreetSuffix"].ToString();
            ddlStreetSuffixDirection.SelectedValue = htAddress["ddlStreetSuffixDirection"].ToString();
            ddlAPO_Search_by_Address_Status.Checked = (bool)htAddress["ddlAPO_Search_by_Address_Status"];
            TemplateAttributeModel[] templateAtrributes = (TemplateAttributeModel[])htAddress["templateAtrributes"];

            txtLevelPrefix.Text = htAddress["txtLevelPrefix"].ToString();
            txtLevelNbrStart.Text = htAddress["txtLevelNbrStart"].ToString();
            txtLevelNbrEnd.Text = htAddress["txtLevelNbrEnd"].ToString();
            txtHouseAlphaStart.Text = htAddress["txtHouseAlphaStart"].ToString();
            txtHouseAlphaEnd.Text = htAddress["txtHouseAlphaEnd"].ToString();
            txtStartFraction.Text = htAddress["txtStartFraction"].ToString();
            txtEndFraction.Text = htAddress["txtEndFraction"].ToString();

            templateEdit.DisplayAttributes(templateAtrributes, ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX, true);
        }

        /// <summary>
        /// Set data of drop down list
        /// </summary>
        public void BindAllDropdownList()
        {
            DropDownListBindUtil.BindStreetDirection(ddlAPO_Search_by_Address_Direction);
            DropDownListBindUtil.BindUnitType(ddlAPO_Search_by_Address_UnitType);
            DropDownListBindUtil.BindStreetSuffix(ddlStreetSuffix);
            DropDownListBindUtil.BindStreetDirection(ddlStreetSuffixDirection);
        }

        /// <summary>
        /// Set the value of city and state
        /// </summary>
        public void SetCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtAPO_Search_by_Address_City, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(ddlAPO_Search_by_Address_State, ModuleName);
        }

        /// <summary>
        /// Set address status
        /// </summary>
        public void SetAddessStatus()
        {
            ddlAPO_Search_by_Address_Status.Checked = false;
        }

        /// <summary>
        /// Show address template fields.
        /// </summary>         
        public void ShowTemplateFields()
        {
            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            TemplateAttributeModel[] attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_ADDRESS, ConfigManager.AgencyCode, ACAConstant.ADMIN_CALLER_ID);
            templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX);
        }

        /// <summary>
        /// Clears the regional setting.
        /// </summary>
        public void ClearRegionalSetting()
        {
            ControlUtil.ClearRegionalSetting(ddlAPOAddressCountry, true, string.Empty, null, string.Empty);
        }

        /// <summary>
        /// Applies the regional setting.
        /// </summary>
        /// <param name="getDefault">if set to <c>true</c> [get default].</param>
        public void ApplyRegionalSetting(bool getDefault)
        {
            ControlUtil.ApplyRegionalSetting(IsPostBack, true, true, getDefault, ddlAPOAddressCountry);
        }

        #endregion
    }
}