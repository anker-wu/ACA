﻿﻿#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: OwnerLookupForm.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: OwnerLookupForm.aspx.cs 
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
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Owner Lookup Edit
    /// </summary>
    public partial class OwnerLookupForm : FormDesignerBaseControl
    {
        #region Properties
        /// <summary>
        /// Gets owner phone client id
        /// </summary>
        public string OwnerPhoneID
        {
            get
            {
                return txtOwnerPhone.ClientID;
            }
        }

        /// <summary>
        /// Gets owner fax client id
        /// </summary>
        public string OwnerFaxID
        {
            get
            {
                return txtOwnerFax.ClientID;
            }
        }

        /// <summary>
        /// Get user control client id
        /// </summary>
        public string UserControlClientID
        {
            get
            {
                return txtAPO_Search_by_Owner_OwnerName.ClientID.Replace("txtAPO_Search_by_Owner_OwnerName", string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets Address Attributes
        /// </summary>
        private Hashtable AddressAttributes
        {
            get
            {
                if (ViewState["AddressAttributes"] == null)
                {
                    ViewState["AddressAttributes"] = new Hashtable();
                }

                return (Hashtable)ViewState["AddressAttributes"];
            }

            set
            {
                ViewState["AddressAttributes"] = value;
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
                base.Permission.permissionLevel = GViewConstant.PERMISSION_APO;
                base.Permission.permissionValue = GViewConstant.SECTION_OWNER;  
                return base.Permission;
            }
            set
            {
                base.Permission = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the OwnerLookupEdit class.
        /// </summary>
        public OwnerLookupForm()
            : base(GviewID.LookUpByOwner)
        {
        }
        /// <summary>
        /// override PreRender event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);             
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
            phContent.TemplateControlIDPrefix = ACAConstant.CAP_OWNER_TEMPLATE_FIELD_PREFIX;
            InitFormDesignerPlaceHolder(phContent);
        }

        /// <summary>
        /// OnInit event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ControlBuildHelper.AddValidationForStandardFields(GviewID.LookUpByOwner, ModuleName, Permission, Controls);
           
            ddlAPOOwnerCountry.BindItems();
            ddlAPOOwnerCountry.SetCountryControls(txtAPO_Search_by_Owner_Zip, ddlAPO_Search_by_Owner_State, txtOwnerPhone, txtOwnerFax);
        }
        /// <summary>
        /// Get owner hash table according to user's input
        /// </summary>
        /// <returns>Hashtable</returns>
        public Hashtable GetOwnerHashTable()
        {
            Hashtable htOwner = new Hashtable(15);
            htOwner.Add("txtAPO_Search_by_Owner_OwnerName", txtAPO_Search_by_Owner_OwnerName.Text.Trim());
            htOwner.Add("txtAPO_Search_by_Owner_AddressLine1", txtAPO_Search_by_Owner_AddressLine1.Text.Trim());
            htOwner.Add("txtAPO_Search_by_Owner_AddressLine2", txtAPO_Search_by_Owner_AddressLine2.Text.Trim());
            htOwner.Add("txtAPO_Search_by_Owner_AddressLine3", txtAPO_Search_by_Owner_AddressLine3.Text.Trim());
            htOwner.Add("txtAPO_Search_by_Owner_Zip", txtAPO_Search_by_Owner_Zip.GetZip(ddlAPOOwnerCountry.SelectedValue.Trim()));
            htOwner.Add("txtAPO_Search_by_Owner_City", txtAPO_Search_by_Owner_City.Text.Trim());
            htOwner.Add("ddlAPO_Search_by_Owner_State", ddlAPO_Search_by_Owner_State.Text.Trim());
            htOwner.Add("ddlAPOOwnerCountry", ddlAPOOwnerCountry.SelectedValue.Trim());
            htOwner.Add("txtOwnerPhone", txtOwnerPhone.GetPhone(ControlUtil.GetControlValue(ddlAPOOwnerCountry)));
            htOwner.Add("txtOwnerPhoneIDD", txtOwnerPhone.CountryCodeText.Trim());
            htOwner.Add("txtOwnerFax", txtOwnerFax.GetPhone(ControlUtil.GetControlValue(ddlAPOOwnerCountry)));
            htOwner.Add("txtOwnerFaxIDD", txtOwnerFax.CountryCodeText.Trim());
            htOwner.Add("txtOwnerEmail", txtOwnerEmail.Text.Trim());
            htOwner.Add("ddlAPO_Search_by_Owner_Status", ddlAPO_Search_by_Owner_Status.Checked);
            TemplateAttributeModel[] templateAtrributes = templateEdit.GetAttributeModels();
            htOwner.Add("templateAtrributes", templateAtrributes);
            return htOwner;
        }

        /// <summary>
        /// Get owner by user's input
        /// </summary>
        /// <returns>OwnerModel object</returns>
        public OwnerCompModel GetOwner()
        {
            OwnerCompModel ownerModel = new OwnerCompModel();
            ownerModel.ownerFullName = txtAPO_Search_by_Owner_OwnerName.Text.Trim();
            ownerModel.address1 = txtAPO_Search_by_Owner_AddressLine1.Text.Trim();
            ownerModel.address2 = txtAPO_Search_by_Owner_AddressLine2.Text.Trim();
            ownerModel.address3 = txtAPO_Search_by_Owner_AddressLine3.Text.Trim();
            ownerModel.zip = txtAPO_Search_by_Owner_Zip.GetZip(ddlAPOOwnerCountry.SelectedValue.Trim());
            ownerModel.city = txtAPO_Search_by_Owner_City.Text.Trim();
            ownerModel.state = ddlAPO_Search_by_Owner_State.Text.Trim();
            ownerModel.country = ddlAPOOwnerCountry.SelectedValue.Trim();
            ownerModel.fax = txtOwnerFax.GetPhone(ControlUtil.GetControlValue(ddlAPOOwnerCountry));
            ownerModel.faxCountryCode = txtOwnerFax.CountryCodeText.Trim();
            ownerModel.phone = txtOwnerPhone.GetPhone(ControlUtil.GetControlValue(ddlAPOOwnerCountry));
            ownerModel.phoneCountryCode = txtOwnerPhone.CountryCodeText.Trim();
            ownerModel.email = txtOwnerEmail.Text.Trim();
            ownerModel.auditStatus = ddlAPO_Search_by_Owner_Status.Checked ? string.Empty : ACAConstant.VALID_STATUS;
            ownerModel.templates = templateEdit.GetAttributeModels();
            return ownerModel;
        }

        /// <summary>
        /// Set owner value according to hashtable content
        /// </summary>
        /// <param name="htOwner">Hashtable</param>
        public void SetOwnerValue(Hashtable htOwner)
        {
            DropDownListBindUtil.SetCountrySelectedValue(ddlAPOOwnerCountry, htOwner["ddlAPOOwnerCountry"].ToString(), false, true, true);
            txtAPO_Search_by_Owner_OwnerName.Text = htOwner["txtAPO_Search_by_Owner_OwnerName"].ToString();
            txtAPO_Search_by_Owner_AddressLine1.Text = htOwner["txtAPO_Search_by_Owner_AddressLine1"].ToString();
            txtAPO_Search_by_Owner_AddressLine2.Text = htOwner["txtAPO_Search_by_Owner_AddressLine2"].ToString();
            txtAPO_Search_by_Owner_AddressLine3.Text = htOwner["txtAPO_Search_by_Owner_AddressLine3"].ToString();
            txtAPO_Search_by_Owner_Zip.Text = ModelUIFormat.FormatZipShow(htOwner["txtAPO_Search_by_Owner_Zip"].ToString(), htOwner["ddlAPOOwnerCountry"].ToString());
            txtAPO_Search_by_Owner_City.Text = htOwner["txtAPO_Search_by_Owner_City"].ToString();
            ddlAPO_Search_by_Owner_State.Text = htOwner["ddlAPO_Search_by_Owner_State"].ToString();

            txtOwnerPhone.Text = ModelUIFormat.FormatPhone4EditPage(htOwner["txtOwnerPhone"].ToString(), htOwner["ddlAPOOwnerCountry"].ToString());
            txtOwnerPhone.CountryCodeText = htOwner["txtOwnerPhoneIDD"].ToString();
            txtOwnerFax.Text = ModelUIFormat.FormatPhone4EditPage(htOwner["txtOwnerFax"].ToString(), htOwner["ddlAPOOwnerCountry"].ToString());
            txtOwnerFax.CountryCodeText = htOwner["txtOwnerFaxIDD"].ToString();
            txtOwnerEmail.Text = htOwner["txtOwnerEmail"].ToString();
            ddlAPO_Search_by_Owner_Status.Checked = (bool)htOwner["ddlAPO_Search_by_Owner_Status"];
            TemplateAttributeModel[] templateAtrributes = (TemplateAttributeModel[])htOwner["templateAtrributes"];
            templateEdit.DisplayAttributes(templateAtrributes, ACAConstant.CAP_OWNER_TEMPLATE_FIELD_PREFIX, true);
        }

        /// <summary>
        /// Set the value of city and state
        /// </summary>
        public void SetCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentState(ddlAPO_Search_by_Owner_State, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentCity(txtAPO_Search_by_Owner_City, ModuleName);
        }

        /// <summary>
        /// Set drop down list of APO owner country
        /// </summary>
        /// <param name="RelevantOwnerPhoneIDs">string</param>
        public void SetAPOOwnerCountry(string RelevantOwnerPhoneIDs)
        {
            ddlAPOOwnerCountry.RelevantControlIDs = RelevantOwnerPhoneIDs;
            ddlAPOOwnerCountry.RegisterScripts();
        }

        /// <summary>
        /// Show owner template fields.
        /// </summary>        
        public void ShowTemplateFields()
        {
            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            TemplateAttributeModel[] attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_OWNER, ConfigManager.AgencyCode, ACAConstant.ADMIN_CALLER_ID);
            templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_OWNER_TEMPLATE_FIELD_PREFIX);
        }

        /// <summary>
        /// Set owner status
        /// </summary>
        public void SetOwnerStatus()
        {
            ddlAPO_Search_by_Owner_Status.Checked = false;
        }

        /// <summary>
        /// Clears the regional setting.
        /// </summary>
        public void ClearRegionalSetting()
        {
            ControlUtil.ClearRegionalSetting(ddlAPOOwnerCountry, true, string.Empty, null, string.Empty);
        }

        /// <summary>
        /// Applies the regional setting.
        /// </summary>
        /// <param name="getDefault">if set to <c>true</c> [get default].</param>
        public void ApplyRegionalSetting(bool getDefault)
        {
            ControlUtil.ApplyRegionalSetting(IsPostBack, true, true, getDefault, ddlAPOOwnerCountry);
        }

        #endregion
    }
}