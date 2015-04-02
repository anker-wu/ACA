#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapClone.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapClone.aspx.cs 185151 2010-11-24 06:17:54Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

using Accela.ACA.BLL;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// cap clone class.
    /// </summary>
    public partial class CapClone : PopupDialogBasePage
    {
        /// <summary>
        /// check box id prefix.
        /// </summary>
        private const string CHECKBOX_ID_PREFIX = "chk";

        /// <summary>
        /// clone component prefix.
        /// </summary>
        private const string CLONE_COMPONENT_PREFIX = "clone";

        /// <summary>
        /// Gets or sets the page flow model list.
        /// </summary>
        private PageFlowComponentModel[] PageFlowComponents
        {
            get
            {
                return ViewState["pageFlows"] as PageFlowComponentModel[];
            }

            set
            {
                ViewState["pageFlows"] = value;
            }
        }

        /// <summary>
        /// Gets the source cap id for clone.
        /// </summary>
        private CapIDModel SourceCapID
        {
            get
            {
                CapIDModel capID = new CapIDModel();
                capID.ID1 = Request.QueryString[ACAConstant.CAP_ID_1].ToString();
                capID.ID2 = Request.QueryString[ACAConstant.CAP_ID_2].ToString();
                capID.ID3 = Request.QueryString[ACAConstant.CAP_ID_3].ToString();
                capID.serviceProviderCode = Request.QueryString[UrlConstant.AgencyCode].ToString();
                return capID;
            }
        }

        /// <summary>
        /// Gets the module name.
        /// </summary>
        private string ModuleName
        {
            get
            {
                string moduleName = Request.QueryString[ACAConstant.MODULE_NAME];
                return moduleName;
            }
        }
        
        /// <summary>
        /// page load function.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetMasterPageProperties();

                if (AppSession.IsAdmin)
                {
                    EnableCloneComponents4Admin(this.tbComponents);
                }
                else
                {
                    EnableAvailableCloneComponents();
                }
            }
        }

        /// <summary>
        /// Raises the clone button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void CloneRecordButton_Click(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                return;
            }

            ArrayList components = new ArrayList();
            string[] cloneComponents = GetCloneComponents(this.tbComponents, components);
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(Accela.ACA.BLL.Cap.ICapBll));
            CapIDModel[] clonedCapIDs = capBll.CloneRecords(SourceCapID, cloneComponents, AppSession.User.PublicUserId);

            if (clonedCapIDs != null && clonedCapIDs.Length > 0)
            {
                //Super cap don't support clone.
                CapWithConditionModel4WS capWithConditionModel4WS = capBll.GetCapViewBySingle(TempModelConvert.Add4WSForCapIDModel(clonedCapIDs[0]), AppSession.User.UserSeqNum, ACAConstant.COMMON_N, false);
                CapModel4WS capModel4WS = capWithConditionModel4WS.capModel;
                FilterAPOTemplates(capModel4WS);

                // The license record in licenseProfessionalModel is included in licenseProfessionalList, so set the capModel4WS.licenseProfessionalModel to null.
                capModel4WS.licenseProfessionalModel = null;

                AppSession.SetCapModelToSession(ModuleName, capModel4WS);

                string url = BuilderUrl();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "CloneRecord", url, true);
            }
        }

        /// <summary>
        /// judge a component can be cloned or not.
        /// </summary>
        /// <param name="cloneComponentName">current component name.</param>
        /// <param name="sectionPermissions">section permission in record detail.</param>
        /// <param name="cap">The cap model.</param>
        /// <returns>true or false.</returns>
        private static bool IsCloneSectionVisibility(string cloneComponentName, Dictionary<string, UserRolePrivilegeModel> sectionPermissions, CapModel4WS cap)
        {
            CloneRecordComponent pageFlowComponent = (CloneRecordComponent)Enum.Parse(typeof(CloneRecordComponent), cloneComponentName);

            string capDetailSection = CloneRecordUtil.Mapping2CapDetailSection(pageFlowComponent);
            bool isSectionVisible = CapUtil.GetSectionVisibility(capDetailSection, sectionPermissions, cap);

            if (cloneComponentName.Equals(CloneRecordComponent.cloneContacts.ToString()) && !isSectionVisible)
            {
                isSectionVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.APPLICANT.ToString(), sectionPermissions, cap);
            }

            return isSectionVisible;
        }

        /// <summary>
        /// convert page flow component's name to clone record's component name.
        /// </summary>
        /// <param name="componentId">page flow component id.</param>
        /// <returns>true or false.</returns>
        private static string ConvertComponentName4PageFlow2CloneRecord(long componentId)
        {
            string cloneRecordComponent = string.Empty;

            switch ((PageFlowComponent)componentId)
            {
                case PageFlowComponent.ADDRESS:
                    cloneRecordComponent = CloneRecordComponent.cloneAddressList.ToString();
                    break;
                case PageFlowComponent.PARCEL:
                    cloneRecordComponent = CloneRecordComponent.cloneParcelList.ToString();
                    break;
                case PageFlowComponent.OWNER:
                    cloneRecordComponent = CloneRecordComponent.cloneOwnerList.ToString();
                    break;
                case PageFlowComponent.ASI:
                    cloneRecordComponent = CloneRecordComponent.cloneAppSpecificInfo.ToString();
                    break;
                case PageFlowComponent.ASI_TABLE:
                    cloneRecordComponent = CloneRecordComponent.cloneAppSpecificInfoTable.ToString();
                    break;
                case PageFlowComponent.EDUCATION:
                    cloneRecordComponent = CloneRecordComponent.cloneEducation.ToString();
                    break;
                case PageFlowComponent.CONTINUING_EDUCATION:
                    cloneRecordComponent = CloneRecordComponent.cloneContEducation.ToString();
                    break;
                case PageFlowComponent.EXAMINATION:
                    cloneRecordComponent = CloneRecordComponent.cloneExamination.ToString();
                    break;
                case PageFlowComponent.DETAIL_INFORMATION:
                    cloneRecordComponent = CloneRecordComponent.cloneDetailInformation.ToString();
                    break;
                case PageFlowComponent.ADDITIONAL_INFORMATION:
                    cloneRecordComponent = CloneRecordComponent.cloneAdditionInfo.ToString();
                    break;
                case PageFlowComponent.VALUATION_CALCULATOR:
                    cloneRecordComponent = CloneRecordComponent.cloneValuationCalculator.ToString();
                    break;
                case PageFlowComponent.APPLICANT:
                case PageFlowComponent.CONTACT_1:
                case PageFlowComponent.CONTACT_2:
                case PageFlowComponent.CONTACT_3:
                case PageFlowComponent.CONTACT_LIST:
                    cloneRecordComponent = CloneRecordComponent.cloneContacts.ToString();
                    break;
                case PageFlowComponent.LICENSED_PROFESSIONAL:
                case PageFlowComponent.LICENSED_PROFESSIONAL_LIST:
                    cloneRecordComponent = CloneRecordComponent.cloneProfessional.ToString();
                    break;
                case PageFlowComponent.ATTACHMENT:
                case PageFlowComponent.CUSTOM_COMPONENT:
                    break;
                case PageFlowComponent.ASSETS:
                    cloneRecordComponent = CloneRecordComponent.cloneAssetList.ToString();
                    break;
                default:
                    break;
            }

            return cloneRecordComponent;
        }

        /// <summary>
        /// Filter APO template fields by current agency.(Only copy current agency's template fields)
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        private void FilterAPOTemplates(CapModel4WS capModel)
        {
            //Get template fields from daily side.
            CapUtil.FillCapModelTemplateValue(capModel);
            string agencyCode = capModel.capID.serviceProviderCode;

            if (!ConfigManager.SuperAgencyCode.Equals(agencyCode, StringComparison.OrdinalIgnoreCase))
            {
                if (capModel.addressModel != null && capModel.addressModel.addressId != 0)
                {
                    List<string> attributeNames = GetRefAttributeNames(TemplateType.CAP_ADDRESS, agencyCode);

                    if (attributeNames != null && attributeNames.Count > 0)
                    {
                        IEnumerable<TemplateAttributeModel> addressTemplates = capModel.addressModel.templates.Where(t => attributeNames.Contains(t.attributeName));

                        if (addressTemplates != null)
                        {
                            capModel.addressModel.templates = addressTemplates.ToArray();
                        }
                    }
                }

                if (capModel.parcelModel != null && capModel.parcelModel.parcelModel != null &&
                    !string.IsNullOrEmpty(capModel.parcelModel.parcelModel.parcelNumber))
                {
                    List<string> attributeNames = GetRefAttributeNames(TemplateType.CAP_PARCEL, agencyCode);

                    if (attributeNames != null && attributeNames.Count > 0)
                    {
                        IEnumerable<TemplateAttributeModel> parcelTemplates = capModel.parcelModel.parcelModel.templates.Where(t => attributeNames.Contains(t.attributeName));

                        if (parcelTemplates != null)
                        {
                            capModel.parcelModel.parcelModel.templates = parcelTemplates.ToArray();
                        }
                    }
                }

                if (capModel.ownerModel != null && capModel.ownerModel.ownerNumber != null)
                {
                    List<string> attributeNames = GetRefAttributeNames(TemplateType.CAP_OWNER, agencyCode);

                    if (attributeNames != null && attributeNames.Count > 0)
                    {
                        IEnumerable<TemplateAttributeModel> ownerTemplates = capModel.ownerModel.templates.Where(t => attributeNames.Contains(t.attributeName));

                        if (ownerTemplates != null)
                        {
                            capModel.ownerModel.templates = ownerTemplates.ToArray();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get RefAttributes names.
        /// </summary>
        /// <param name="type">the template type</param>
        /// <param name="agencyCode">the agency code.</param>
        /// <returns>ref attribute names.</returns>
        private List<string> GetRefAttributeNames(TemplateType type, string agencyCode)
        {
            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            TemplateAttributeModel[] capTemplates = templateBll.GetAPOTemplateAttributes(type, agencyCode, AppSession.User.PublicUserId);
            List<string> attributeNames = new List<string>();

            if (capTemplates != null && capTemplates.Length > 0)
            {
                attributeNames = capTemplates.Select(o => o.attributeName).ToList();
            }

            return attributeNames;
        }

        /// <summary>
        /// Get all components which can be cloned.
        /// </summary>
        /// <param name="capID">cap id model.</param>
        /// <param name="moduleName">module name which current cap belong to.</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>string array.</returns>
        private string[] GetAvailableSections(CapIDModel capID, string moduleName, string callerID)
        {
            IPageflowBll pageflowBll = (IPageflowBll)ObjectFactory.GetObject(typeof(IPageflowBll));
            PageFlowComponents = pageflowBll.GetPageFlowComponentsByCapID(capID, callerID);
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(Accela.ACA.BLL.Cap.ICapBll));

            if (StandardChoiceUtil.IsSuperAgency() && (PageFlowComponents == null || PageFlowComponents.Length == 0))
            {
                CapModel4WS childCap = capBll.GetCapByPK(TempModelConvert.Add4WSForCapIDModel(capID));

                if (!string.IsNullOrEmpty(childCap.refID))
                {
                    string[] parentCap = childCap.refID.Split('-');
                    CapIDModel parentCapID = new CapIDModel();
                    parentCapID.serviceProviderCode = parentCap[0];
                    parentCapID.ID1 = parentCap[1];
                    parentCapID.ID2 = parentCap[2];
                    parentCapID.ID3 = parentCap[3];
                    PageFlowComponents = pageflowBll.GetPageFlowComponentsByCapID(parentCapID, callerID);
                }
            }

            ArrayList components = new ArrayList();

            if (PageFlowComponents != null && PageFlowComponents.Length > 0)
            {
                // Get section permissions
                string agencyCode = string.Empty;

                if (capID != null)
                {
                    agencyCode = capID.serviceProviderCode;
                }

                Dictionary<string, UserRolePrivilegeModel> sectionPermissions = CapUtil.GetSectionPermissions(agencyCode, moduleName);

                // Get Cap Detail Information
                CapModel4WS cap = AppSession.GetCapModelFromSession(moduleName);

                if (cap == null || !cap.capID.Equals(TempModelConvert.Add4WSForCapIDModel(capID)))
                {
                    bool isSuperCap = CapUtil.IsSuperCAP(TempModelConvert.Add4WSForCapIDModel(capID));
                    CapWithConditionModel4WS capWithConditionModel4WS = capBll.GetCapViewBySingle(TempModelConvert.Add4WSForCapIDModel(capID), AppSession.User.UserSeqNum, ACAConstant.COMMON_N, isSuperCap);
                    cap = capWithConditionModel4WS.capModel;
                }

                foreach (PageFlowComponentModel pageFlow in PageFlowComponents)
                {
                    if (pageFlow.componentID == (long)PageFlowComponent.ATTACHMENT 
                        || pageFlow.componentID == (long)PageFlowComponent.CUSTOM_COMPONENT 
                        || pageFlow.componentID == (long)PageFlowComponent.CONDITION_ATTACHMENT)
                    {
                        continue;
                    }

                    string cloneComponentName = ConvertComponentName4PageFlow2CloneRecord(pageFlow.componentID);

                    bool isVisibility = IsCloneSectionVisibility(cloneComponentName, sectionPermissions, cap);

                    if (isVisibility && !components.Contains(cloneComponentName))
                    {
                        components.Add(cloneComponentName);
                    }
                }
            }

            return (string[])components.ToArray(typeof(string));
        }

        /// <summary>
        /// Set master page properties.
        /// </summary>
        private void SetMasterPageProperties()
        {
            Dialog master = this.Master as Dialog;
            
            master.PageTitleVisible = true;
            master.SetTitleLabelKey("aca_capclone_label_title");
        }

        /// <summary>
        /// display components.
        /// </summary>
        private void EnableAvailableCloneComponents()
        {
            AccelaCheckBox chkComponent = null;

            string[] availableComponentNames = GetAvailableSections(SourceCapID, ModuleName, AppSession.User.PublicUserId);

            if (availableComponentNames != null && availableComponentNames.Length > 0)
            {
                foreach (string componentName in availableComponentNames)
                {
                    //componentName.Substring(5) : remove five characters("clone").
                    string controlID = CHECKBOX_ID_PREFIX + componentName.Substring(5);

                    Control ctl = this.tbComponents.FindControl(controlID);

                    if (ctl != null && ctl is AccelaCheckBox)
                    {
                        chkComponent = (AccelaCheckBox)ctl;
                        chkComponent.Enabled = true;
                        chkComponent.Checked = true;
                        chkComponent.Attributes.Add("onclick", "EableDisableCloneButton();");
                    }
                }
            }
            else
            {
                this.lnkCloneRecord.Enabled = false;
            }
        }

        /// <summary>
        /// get need clone components.
        /// </summary>
        /// <param name="parentControl">parent control.</param>
        /// <param name="components">need clone components.</param>
        /// <returns>clone components</returns>
        private string[] GetCloneComponents(Control parentControl, ArrayList components)
        {
            foreach (Control childControl in parentControl.Controls)
            {
                if (childControl is AccelaCheckBox && ((AccelaCheckBox)childControl).Checked)
                {
                    //childControl.ID.Remove(0, 3): remove 3 characters("chk") from control id.
                    string cloneComponent = CLONE_COMPONENT_PREFIX + childControl.ID.Remove(0, 3);
                    components.Add(cloneComponent);
                }

                if (childControl.HasControls())
                {
                    GetCloneComponents(childControl, components);
                }
            }

            return (string[])components.ToArray(typeof(string));
        }

        /// <summary>
        /// enable clone components for admin side.
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        private void EnableCloneComponents4Admin(Control parentControl)
        {
            foreach (Control childControl in parentControl.Controls)
            {
                if (childControl is AccelaCheckBox)
                {
                    ((AccelaCheckBox)childControl).Enabled = true;
                }

                if (childControl.HasControls())
                {
                    EnableCloneComponents4Admin(childControl);
                }
            }
        }

        /// <summary>
        /// Build the apply record url for clone record
        /// </summary>
        /// <returns>url string</returns>
        private string BuilderUrl()
        {
            string agencyCode = SourceCapID.serviceProviderCode;
            string pageFlowGroupCode = string.Empty;

            if (PageFlowComponents != null && PageFlowComponents.Length > 0 && !string.IsNullOrEmpty(PageFlowComponents[0].servProvCode))
            {
                agencyCode = PageFlowComponents[0].servProvCode;
                pageFlowGroupCode = PageFlowComponents[0].groupCode;
            }

            List<LinkItem> links = TabUtil.GetCreationLinkItemList(ModuleName, false);
            LinkItem creationItem = links[0];

            ICapTypeFilterBll capTypFiltereBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
            string filterName = capTypFiltereBll.GetCapTypeFilterByLabelKey(ConfigManager.AgencyCode, creationItem.Module, creationItem.Label);
            string url = TabUtil.RebuildUrl(creationItem.Url, creationItem.Module, filterName);
            
            if (url.IndexOf("?") >= 0)
            {
                url += "&" + ACAConstant.IS_CLONE_RECORD + "=" + ACAConstant.COMMON_TRUE;
            }
            else
            {
                url += "?" + ACAConstant.IS_CLONE_RECORD + "=" + ACAConstant.COMMON_TRUE;
            }

            if (StandardChoiceUtil.IsSuperAgency())
            {
                url += "&" + ACAConstant.IS_SUBAGENCY_CAP + "=" + ACAConstant.COMMON_Y;
                url += "&" + UrlConstant.AgencyCode + "=" + agencyCode;
            }

            url += "&" + UrlConstant.PAGEFLOW_GROUP_CODE + "=" + Server.UrlEncode(pageFlowGroupCode);
            StringBuilder sb = new StringBuilder();
            sb.Append("parent.window.location.href = '").Append(FileUtil.AppendApplicationRoot(url)).Append("';");

            return sb.ToString();
        }
    }
}