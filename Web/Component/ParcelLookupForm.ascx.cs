﻿﻿#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ParcelLookupForm.acsx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2012
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ParcelLookupForm.ascx.cs  
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
    /// Parcel Lookup Edit
    /// </summary>
    public partial class ParcelLookupForm : FormDesignerBaseControl
    {
        #region Properties
        /// <summary>
        /// Gets control client id
        /// </summary>
        public string UserControlClientID
        {
            get
            {
                return txtAPO_Search_by_Parcel_ParcelNumber.ClientID.Replace("txtAPO_Search_by_Parcel_ParcelNumber", string.Empty);
            }
        }

        /// <summary>
        /// Gets control client id of parcel number
        /// </summary>
        public string ParcelNumberClientID
        {
            get
            {
                return txtAPO_Search_by_Parcel_ParcelNumber.ClientID;
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
                base.Permission.permissionValue = GViewConstant.SECTION_PARCEL;     
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
        /// Initializes a new instance of the ParcelLookupEdit class.
        /// </summary>
        public ParcelLookupForm()
            : base(GviewID.LookUpByParcel)
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
            // Initial the PlaceHolder's properties
            phContent.TemplateControlIDPrefix = ACAConstant.CAP_PARCEL_TEMPLATE_FIELD_PREFIX;
            InitFormDesignerPlaceHolder(phContent);             
        }

        /// <summary>
        /// OnInit event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);           
            ControlBuildHelper.AddValidationForStandardFields(GviewID.LookUpByParcel, ModuleName, Permission, Controls);
        }

        /// <summary>
        /// Get parcel hash table according to user's input
        /// </summary>
        /// <returns>Hashtable</returns>
        public Hashtable GetParcelHashTable()
        {
            Hashtable htParcel = new Hashtable(6);
            htParcel.Add("txtAPO_Search_by_Parcel_ParcelNumber", txtAPO_Search_by_Parcel_ParcelNumber.Text.Trim());
            htParcel.Add("txtAPO_Search_by_Parcel_Lot", txtAPO_Search_by_Parcel_Lot.Text.Trim());
            htParcel.Add("txtAPO_Search_by_Parcel_Block", txtAPO_Search_by_Parcel_Block.Text.Trim());
            htParcel.Add("txtAPO_Search_by_Parcel_Subdivision", txtAPO_Search_by_Parcel_Subdivision.Text.Trim());
            htParcel.Add("ddlAPO_Search_by_Parcel_Status", ddlAPO_Search_by_Parcel_Status.Checked);
            TemplateAttributeModel[] templateAtrributes = templateEdit.GetAttributeModels();
            htParcel.Add("templateAtrributes", templateAtrributes);
            return htParcel;
        }

        /// <summary>
        /// Get parcel by user's input
        /// </summary>
        /// <returns>ParcelModel object</returns>
        public ParcelModel GetParcel()
        {
            ParcelModel parcelModel = new ParcelModel();
            parcelModel.parcelNumber = txtAPO_Search_by_Parcel_ParcelNumber.Text.Trim();

            parcelModel.lot = txtAPO_Search_by_Parcel_Lot.Text.Trim();
            parcelModel.block = txtAPO_Search_by_Parcel_Block.Text.Trim();
            parcelModel.subdivision = txtAPO_Search_by_Parcel_Subdivision.Text.Trim();
            parcelModel.auditStatus = ddlAPO_Search_by_Parcel_Status.Checked ? string.Empty : ACAConstant.VALID_STATUS;
            parcelModel.templates = templateEdit.GetAttributeModels(); 
            return parcelModel;
        }

        /// <summary>
        /// Set parcel value according to hashtable content
        /// </summary>
        /// <param name="Hashtable">htParcel</param>
        public void SetPacelValue(Hashtable htParcel)
        {
            txtAPO_Search_by_Parcel_ParcelNumber.Text = htParcel["txtAPO_Search_by_Parcel_ParcelNumber"].ToString();
            txtAPO_Search_by_Parcel_Lot.Text = htParcel["txtAPO_Search_by_Parcel_Lot"].ToString();
            txtAPO_Search_by_Parcel_Block.Text = htParcel["txtAPO_Search_by_Parcel_Block"].ToString();
            txtAPO_Search_by_Parcel_Subdivision.Text = htParcel["txtAPO_Search_by_Parcel_Subdivision"].ToString();
            ddlAPO_Search_by_Parcel_Status.Checked = (bool)htParcel["ddlAPO_Search_by_Parcel_Status"];
            TemplateAttributeModel[] templateAtrributes = (TemplateAttributeModel[])htParcel["templateAtrributes"];
            templateEdit.DisplayAttributes(templateAtrributes, ACAConstant.CAP_PARCEL_TEMPLATE_FIELD_PREFIX, true);
        }

        /// <summary>
        /// Show parcel template fields.
        /// </summary>         
        public void ShowTemplateFields()
        {
            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            TemplateAttributeModel[] attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_PARCEL, ConfigManager.AgencyCode, ACAConstant.ADMIN_CALLER_ID);
            templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_PARCEL_TEMPLATE_FIELD_PREFIX);
        }

        /// <summary>
        /// Set Parcel status
        /// </summary>
        public void SetParcelStauts()
        {
            ddlAPO_Search_by_Parcel_Status.Checked = false;
        }

        #endregion
    }
}