/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SupervisorList.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 *  SupervisorList user control.
 * 
 *  Notes:
 * $Id: SupervisorList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// supervisor list
    /// </summary>
    public partial class SupervisorList : BaseUserControl
    {
        /// <summary>
        ///  licensed professional interface
        /// </summary>
        private ILicenseProfessionalBll licenseProBll = (ILicenseProfessionalBll)ObjectFactory.GetObject(typeof(ILicenseProfessionalBll));

        /// <summary>
        /// template interface
        /// </summary>
        private ITemplateBll _templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));

        /// <summary>
        /// Gets or sets a value indicating whether event in Detail page.
        /// </summary>
        public bool IsSaveForDetailPage
        {
            get 
            {
                if (ViewState["IsSaveForDetailPage"] != null)
                {
                    return (bool)ViewState["IsSaveForDetailPage"];
                }

                return false;
            }

            set
            {
                ViewState["IsSaveForDetailPage"] = value;
            }
        }

        /// <summary>
        /// Gets or sets license professional model list for display.
        /// </summary>
        private LicenseProfessionalModel[] LicenseProModels4Display
        {
            get
            {
                if (ViewState["LicenseProModels4Display"] != null)
                {
                    return (LicenseProfessionalModel[])ViewState["LicenseProModels4Display"];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                ViewState["LicenseProModels4Display"] = value;
            }
        }

        /// <summary>
        /// Gets or sets license professional model list for save.
        /// </summary>
        private LicenseProfessionalModel[] LicenseProModels4Save
        {
            get
            {
                if (ViewState["LicenseProModels4Save"] != null)
                {
                    return (LicenseProfessionalModel[])ViewState["LicenseProModels4Save"];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                ViewState["LicenseProModels4Save"] = value;
            }
        }

        /// <summary>
        /// Load supervisor for each LP.
        /// </summary>
        /// <param name="capIdModel">CAP id model</param>
        public void DisplaySupervisor4EachLP(CapIDModel4WS capIdModel)
        {
            // Get selected license professional model list from reference side. 
            LicenseProfessionalModel[] licProModels = licenseProBll.GetLPListByCapID(ConfigManager.AgencyCode, capIdModel, AppSession.User.PublicUserId);

            DataTable dtSupervisor = ConstructSupervisorDataSource(licProModels, capIdModel);

            dlAttributesList4EMSE.DataSource = dtSupervisor;
            dlAttributesList4EMSE.DataBind();
        }
        
        /// <summary>
        /// Check required field whether have value.
        /// </summary>
        /// <param name="licenseProModels">license professional model</param>
        /// <returns>true indicate have required field no value;false indicate all required field have value</returns>
        public bool CheckRequiredField4Save(LicenseProfessionalModel[] licenseProModels)
        {
            bool checkRequiredField = false;

            if (licenseProModels == null)
            {
                return checkRequiredField;
            }

            foreach (LicenseProfessionalModel licenseProModel in licenseProModels)
            {
                for (int i = 0; i < licenseProModel.templateAttributes.Length; i++)
                {
                    if (!string.IsNullOrEmpty(licenseProModel.templateAttributes[i].attributeScriptCode) &&
                        ACAConstant.COMMON_Y.Equals(licenseProModel.templateAttributes[i].attributeValueReqFlag, StringComparison.InvariantCulture) &&
                        string.IsNullOrEmpty(licenseProModel.templateAttributes[i].attributeValue))
                    {
                        checkRequiredField = true;
                    }
                }
            }

            return checkRequiredField;
        }

        /// <summary>
        /// Supervisor page load event.
        /// </summary>
        /// <param name="sender">page object.</param>
        /// <param name="e">page arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnCancel.Attributes.Add("onclick", string.Format("ExitAddForm_{0}(this);return false;", btnCancel.ClientID));

                // if save supervisor in cap detail page, don't set OnClientClick event to save button.
                if (IsSaveForDetailPage)
                {
                    btnSave.OnClientClick = string.Empty;
                }
            }
        }

        /// <summary>
        /// EMSE dropdown data bound.
        /// </summary>
        /// <param name="sender">Data list event object.</param>
        /// <param name="e">Data list event arguments.</param>
        protected void AttributesList4EMSE_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // license professional order.
            int orderIndex = 0;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                SupervisorEdit supervisorEdit = e.Item.FindControl("SupervisorList") as SupervisorEdit;
                AccelaLabel lblLicenseType = e.Item.FindControl("lblLicenseType") as AccelaLabel;
                AccelaLabel lblLicenseNbr = e.Item.FindControl("lblLicenseNbr") as AccelaLabel;
                AccelaLabel lblAgencyCode = e.Item.FindControl("lblAgencyCode") as AccelaLabel;
                string scriptName4Supervisor = "Templete_" + "License" + orderIndex.ToString() + "_CheckControlValueValidate";

                LicenseProfessionalModel licProModel = (LicenseProfessionalModel)drv["LicProModel"];
                lblAgencyCode.Text = drv["AgencyCode"].ToString();
                lblLicenseNbr.Text = drv["LicenseNbr"].ToString();
                lblLicenseType.Text = string.IsNullOrEmpty(licProModel.resLicenseType) ? drv["LicenseType"].ToString() : licProModel.resLicenseType;
                supervisorEdit.ScriptName4Supervisor = scriptName4Supervisor;
                supervisorEdit.DisplayEMSEAttributes(licProModel.templateAttributes);

                orderIndex++;
            }
        }

        /// <summary>
        /// Save supervisor for each license professional.
        /// </summary>
        /// <param name="sender">Save event object.</param>
        /// <param name="e">Save event arguments.</param>
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int licenseProListCount = dlAttributesList4EMSE.Items.Count;

            if (licenseProListCount == 0)
            {
                return;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            LicenseProfessionalModel[] licenseProModels4Save = LicenseProModels4Save;

            for (int i = 0; i < licenseProListCount; i++)
            {
                SupervisorEdit supervisorEdit = dlAttributesList4EMSE.Items[i].FindControl("SupervisorList") as SupervisorEdit;
                SetAttributesValue2LicProModel(LicenseProModels4Display[i], supervisorEdit);
            }

            // Set attributes to license professional models which it's used for save.
            SetAttributes2LicenseProModels(LicenseProModels4Display, licenseProModels4Save);

            if (CheckRequiredField4Save(LicenseProModels4Display))
            {
                ScriptManager.RegisterStartupScript(updatePanel, btnSave.GetType(), "alert", "window.setTimeout(\"alert('" + GetTextByKey("supervisor_detailpage_save_message").Replace("'", "\\'") + "')\",0);", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(updatePanel, GetType(), "HiddenPopWindow", string.Format("CloseAddForm_{0}(this)", btnSave.ClientID), true);
                licenseProBll.CreateOrUpdateLicenseProfessionals(ConfigManager.AgencyCode, licenseProModels4Save, AppSession.User.PublicUserId);
            }
        }

        /// <summary>
        /// Construct Supervisor data source for each LP.
        /// </summary>
        /// <param name="licProModellist">LicenseProfessionalModel array</param>
        /// <param name="capIdModel">CAP id model</param>
        /// <returns>Supervisor data table</returns>
        private DataTable ConstructSupervisorDataSource(LicenseProfessionalModel[] licProModellist, CapIDModel4WS capIdModel)
        {
            LicenseProfessionalModel[] licProModels = FilterLicenseModelByAttribute(licProModellist);

            if (licProModels == null)
            {
                return null;
            }

            LicenseProModels4Save = licProModels;

            LicenseProfessionalModel[] filteredLPs = FilterDuplicateLP(licProModels);
            LicenseProModels4Display = filteredLPs;

            DataTable dtSupervisor = new DataTable();
            dtSupervisor.Columns.Add(new DataColumn("LicenseType", typeof(string)));
            dtSupervisor.Columns.Add(new DataColumn("LicenseNbr", typeof(string)));
            dtSupervisor.Columns.Add(new DataColumn("AgencyCode", typeof(string)));
            dtSupervisor.Columns.Add(new DataColumn("LicProModel", typeof(LicenseProfessionalModel)));

            foreach (LicenseProfessionalModel licProModel in filteredLPs)
            {
                dtSupervisor.Rows.Add(licProModel.licenseType, licProModel.licenseNbr, licProModel.agencyCode, licProModel);  
            }

            return dtSupervisor;
        }

        /// <summary>
        /// Filter license profession model with the same license type and license number.
        /// </summary>
        /// <param name="licenseProModels">Licensed professional models</param>
        /// <returns>Licensed professional models after filter by license type and license number.</returns>
        private LicenseProfessionalModel[] FilterDuplicateLP(LicenseProfessionalModel[] licenseProModels)
        {
            if (licenseProModels == null || licenseProModels.Length == 0)
            {
                return null;
            }

            ArrayList filteredLPs = new ArrayList();

            foreach (LicenseProfessionalModel licensePro in licenseProModels)
            {
                bool isExist = false;

                foreach (LicenseProfessionalModel lp in filteredLPs)
                {
                    if (licensePro.agencyCode.Equals(lp.agencyCode, StringComparison.InvariantCultureIgnoreCase) &&
                        licensePro.licenseType.Equals(lp.licenseType, StringComparison.InvariantCultureIgnoreCase) &&
                         licensePro.licenseNbr.Equals(lp.licenseNbr, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    filteredLPs.Add(licensePro);
                }
            }

            LicenseProfessionalModel[] lpList = (LicenseProfessionalModel[])filteredLPs.ToArray(typeof(LicenseProfessionalModel));

            return lpList;
        }

        /// <summary>
        /// Filter License Professional model by EMSE attributes, only return the license with EMSE attributes.
        /// </summary>
        /// <param name="licProModels">The licensed professionals need to be filtered</param>
        /// <returns>The licenses with EMSE attributes</returns>
        private LicenseProfessionalModel[] FilterLicenseModelByAttribute(LicenseProfessionalModel[] licProModels)
        {
            if (licProModels == null)
            {
                return null;
            }

            ArrayList licenseModelList = new ArrayList(); 

            foreach (LicenseProfessionalModel license in licProModels)
            {
                if (license.templateAttributes != null && CapUtil.IsContainsEMSEAttribute(license.templateAttributes))
                {
                    licenseModelList.Add(license);
                }
            }

            LicenseProfessionalModel[] licenseProModels = null;

            if (licenseModelList.Count > 0)
            {
                licenseProModels = (LicenseProfessionalModel[])licenseModelList.ToArray(typeof(LicenseProfessionalModel));
            }

            return licenseProModels;
        }
       
        /// <summary>
        /// Set attributes license professional models from display license professional models to save license professional models.
        /// </summary>
        /// <param name="licProModels4Show">Display license professional models.</param>
        /// <param name="licProModels4Save">Save license professional models.</param>
        private void SetAttributes2LicenseProModels(LicenseProfessionalModel[] licProModels4Show, LicenseProfessionalModel[] licProModels4Save)
        {
            foreach (LicenseProfessionalModel licProModel4Save in licProModels4Save)
            {
                foreach (LicenseProfessionalModel licProModel4Show in licProModels4Show)
                {
                    if (licProModel4Save.agencyCode.Equals(licProModel4Show.agencyCode, StringComparison.InvariantCultureIgnoreCase) && 
                        licProModel4Save.licenseType.Equals(licProModel4Show.licenseType, StringComparison.InvariantCultureIgnoreCase) &&
                        licProModel4Save.licenseNbr.Equals(licProModel4Show.licenseNbr, StringComparison.InvariantCultureIgnoreCase))
                    {
                        licProModel4Save.templateAttributes = licProModel4Show.templateAttributes;
                    }
                }
            }
        }

        /// <summary>
        /// Set EMSE attributes value to license professional model.
        /// </summary>
        /// <param name="liceseProModel">Licensed Professional Model</param>
        /// <param name="supervisorEdit">SupervisorEdit user control</param>
        private void SetAttributesValue2LicProModel(LicenseProfessionalModel liceseProModel, SupervisorEdit supervisorEdit)
        {
            liceseProModel.templateAttributes = supervisorEdit.GetEMSEAttributeModels();
        }

        /// <summary>
        /// Check required field whether have value in daily side.
        /// </summary>
        /// <param name="licenseProModels">License professional models</param>
        /// <returns>True indicate existing no value required field;False indicate don't existing</returns>
        private bool CheckRequiredFieldNoValue4DailySide(LicenseProfessionalModel[] licenseProModels)
        {
            bool isRequiredFieldNoValue4DailySide = false;

            foreach (LicenseProfessionalModel licenseProModel in licenseProModels)
            {
                if (licenseProModel.templateAttributes == null || licenseProModel.templateAttributes.Length == 0)
                {
                    continue;
                }

                for (int i = 0; i < licenseProModel.templateAttributes.Length; i++)
                {
                    if (!string.IsNullOrEmpty(licenseProModel.templateAttributes[i].attributeScriptCode) &&
                           ACAConstant.COMMON_Y.Equals(licenseProModel.templateAttributes[i].attributeValueReqFlag, StringComparison.InvariantCulture) &&
                           string.IsNullOrEmpty(licenseProModel.templateAttributes[i].attributeValue))
                    {
                        isRequiredFieldNoValue4DailySide = true;
                        break;
                    }
                }
            }

            return isRequiredFieldNoValue4DailySide;
        }
    }
}
