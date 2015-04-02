#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseeDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseeDetail.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Accela.ACA.BLL;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.GeneralProperty
{
    /// <summary>
    /// Licensee detail information
    /// </summary>
    public partial class LicenseeDetail : BasePage
    {
        #region Fields

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(LicenseeDetail));

        /// <summary>
        /// the cap type permissions
        /// </summary>
        private CapTypePermissionModel[] _capTypePermissions;

        /// <summary>
        /// the cap model
        /// </summary>
        private CapModel4WS _capModel;

        #endregion

        #region Methods

        /// <summary>
        /// page load function
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string collapseline = policyBll.GetPolicyValueForData4AsKey(BizDomainConstant.STD_COLLAPSE_LINES, ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode, PageID);
            licenseeGeneralInfo.TemplateCollapseLine = collapseline;

            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    DisplayPageForAdmin();
                }
                else
                {
                    LicenseModel4WS selectedLicenseeModel = GetLicensee();

                    SetCurrentCAPByLicense(selectedLicenseeModel);

                    SetPermissionByCAP();

                    DisplayLicenseeInfo(selectedLicenseeModel);
                }              
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            ////when user trigger this Gridview pageIndexChanged event, permitList.pageIndex will be set to zero
            int currentPageIndex = 0;
            dgvPermitList.InitialExport(false);
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(dgvPermitList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;

            ////when sorting gridview, we use GridViewDataSource as the data, so ,needn't pass the first parameters
            dgvPermitList.BindCapList(null, currentPageIndex, e.GridViewSortExpression);
        }

        /// <summary>
        /// response permit grid view page index changing event to record the latest page index.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void DgvPL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvPermitList.InitialExport(false);
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(dgvPermitList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                DisplayRelatedCaps((LicenseModel4WS)pageInfo.SearchCriterias[0], e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// Display a empty page in ACA admin side.
        /// </summary>
        private void DisplayPageForAdmin()
        {         
            ucConditon.HideCondition();
            ucConditon.AdminDataBound();
            DisplayRelatedCaps(new LicenseModel4WS(), 0, string.Empty);
            CapModel4WS capModel = new CapModel4WS();
            DisplayEducations(capModel);
            DisplayContEducations(capModel);
            DisplayExamination(capModel);
            DisplayAttachment(null);
        }

        /// <summary>
        /// get licensee with parameters in url.
        /// </summary>
        /// <returns>License Models</returns>
        private LicenseModel4WS GetLicensee()
        {
            LicenseModel4WS selectedLicenseeModel = null;

            ILicenseBLL licenseeBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
            LicenseModel4WS licenseeModel = new LicenseModel4WS();
            licenseeModel.serviceProviderCode = ConfigManager.AgencyCode;
            licenseeModel.stateLicense = Request["licenseeNumber"] == null ? string.Empty : Request["LicenseeNumber"].ToString(); //"09ACH-00000-00209";
            licenseeModel.licenseType = Request["licenseeType"] == null ? string.Empty : Request["LicenseeType"].ToString(); //"Architect";
            
            try
            {
                selectedLicenseeModel = licenseeBll.GetLicense(licenseeModel, false);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
            }

            return selectedLicenseeModel;
        }

        /// <summary>
        /// Display all information associated to current licensee.
        /// </summary>
        /// <param name="selectedLicenseeModel">licensee model</param>
        private void DisplayLicenseeInfo(LicenseModel4WS selectedLicenseeModel)
        {
            if (selectedLicenseeModel != null)
            {
                StringBuilder sbProperty = new StringBuilder();
                sbProperty.Append(I18nStringUtil.GetString(selectedLicenseeModel.resLicenseType, selectedLicenseeModel.licenseType));
                sbProperty.Append(ACAConstant.BLANK);

                if (StandardChoiceUtil.IsDisplayLicenseState())
                {
                    sbProperty.Append(I18nStringUtil.GetString(selectedLicenseeModel.resLicState, selectedLicenseeModel.licState));
                    sbProperty.Append(string.IsNullOrEmpty(I18nStringUtil.GetString(selectedLicenseeModel.resLicState, selectedLicenseeModel.licState)) ? string.Empty : ACAConstant.SPLIT_CHAR4);
                }

                sbProperty.Append(selectedLicenseeModel.stateLicense);

                lblPropertyInfo.Text = sbProperty.ToString();                

                DisplayConditions(selectedLicenseeModel.licSeqNbr);
                licenseeGeneralInfo.Display(selectedLicenseeModel);

                // judge whether display related caps list.
                if (IsSectionEnabled(LicenseVerificationSectionType.RELATED_RECORDS.ToString()))
                {
                    DisplayRelatedCaps(selectedLicenseeModel, 0, string.Empty);
                }                

                // judge whether display attachmen list.
                if (IsSectionEnabled(LicenseVerificationSectionType.PUBLIC_DOCUMENTS.ToString()))
                {                     
                    DisplayAttachment(_capModel);                     
                }                 

                // judge whether display education list. 
                if (IsSectionEnabled(LicenseVerificationSectionType.EDUCATION.ToString())) 
                {                     
                    DisplayEducations(_capModel);                     
                }

                // judge whether continue education list. 
                if (IsSectionEnabled(LicenseVerificationSectionType.CONTINUE_EDUCATION.ToString()))
                {                     
                    DisplayContEducations(_capModel);                     
                }

                // judge whether display examination list. 
                if (IsSectionEnabled(LicenseVerificationSectionType.EXAMINATION.ToString()))
                {                     
                    DisplayExamination(_capModel);                     
                }
            }
        }

        /// <summary>
        /// Display Attachment
        /// </summary>
        /// <param name="capModel">cap model.</param>
        private void DisplayAttachment(CapModel4WS capModel)
        {
            divAttachment.Visible = true;
            string moduleName = string.Empty;

            if (capModel != null)
            {
                moduleName = capModel.moduleName;
                AppSession.SetCapModelToSession(moduleName, capModel);
            }

            Literal lc = new Literal();

            string src = string.Format("../FileUpload/AttachmentsList.aspx?readonly=true&module={0}&isLicenseeDetailPage=true&{1}={2}", ScriptFilter.AntiXssUrlEncode(moduleName), UrlConstant.AgencyCode, ConfigManager.AgencyCode);
            string iframeContent = string.Format(LabelUtil.GetGlobalTextByKey("iframe_nonsupport_message"), src);
            lc.Text = string.Format("<iframe id=\"iframeAttachmentList\" src=\"{0}\" frameborder=\"0\" width=\"100%;\" height=\"auto;\" scrolling=\"no\" title=\"{1}\">{2}</iframe>", src, GetTextByKey("iframe_licensedetail_attachmentlist_title"), iframeContent);

            //For support opera to Tab between iframe content and outside of iframe.
            bool isOpera = Request.UserAgent.IndexOf("opera", 0, StringComparison.OrdinalIgnoreCase) != -1;

            if (isOpera)
            {
                lc.Text = string.Format("<a id='hlAttachmentListBegin' href='javascript:void(0);' class='NotShowLoading' tabindex='0'></a>{0}<a id='hlAttachmentListEnd' href='javascript:void(0);' class='NotShowLoading' tabindex='0'></a>", lc.Text);
            }

            phAttachment.Controls.Add(lc);
        }

        /// <summary>
        /// Gets and displays Licensee's condition.
        /// </summary>
        /// <param name="licenseeSeqNum">licensee sequence number</param>
        private void DisplayConditions(string licenseeSeqNum)
        {
            if (!string.IsNullOrEmpty(licenseeSeqNum))
            {
                divCondition.Visible = true;
                ucConditon.HideCondition();
                ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                LicenseModel4WS licenseModel =
                    licenseBll.GetLicenseCondition(ConfigManager.AgencyCode, long.Parse(licenseeSeqNum), AppSession.User.PublicUserId);

                if (licenseModel != null && licenseModel.noticeConditions != null && licenseModel.noticeConditions.Length > 0)
                {
                    ucConditon.IsShowCondition(licenseModel.noticeConditions, licenseModel.hightestCondition, ConditionType.License);
                }
            }
        }

        /// <summary>
        /// Show educations in CAP detail page.
        /// </summary>
        /// <param name="capModel">CAP model contains Education model list.</param>
        private void DisplayEducations(CapModel4WS capModel)
        {
            divEducation.Visible = true;

            IList<EducationModel4WS> educationViewList = null;

            if (!AppSession.IsAdmin && capModel != null)
            {
                educationViewList = ObjectConvertUtil.ConvertArrayToList(capModel.educationList);
            }

            educationList.GridViewDataSource = educationViewList;
            educationList.EducationSectionPosition = EducationOrExamSectionPosition.CapDetail;
            educationList.BindEducations();
        }

        /// <summary>
        /// Display continuing education list.
        /// </summary>
        /// <param name="capModel">cap model which contains continuing education models</param>
        private void DisplayContEducations(CapModel4WS capModel)
        {
            divContEducation.Visible = true;

            // bind continuing education summary information.
            contEducationSummaryList.IsCapDetailPage = true;
            ContinuingEducationModel4WS[] contEducationArray = capModel == null ? null : capModel.contEducationList;
            CapTypeModel capType = capModel == null ? null : capModel.capType;
            contEducationSummaryList.BindSummaryContEducation(contEducationArray, capType);

            // Convert array to list.
            List<ContinuingEducationModel4WS> contEducations = new List<ContinuingEducationModel4WS>();

            //if capModel.contEducation isn't null, it need convert to IList<ContinuingEducationModel4WS>.
            if (capModel != null && capModel.contEducationList != null)
            {
                contEducations.AddRange(capModel.contEducationList);
            }

            // bind continuing education list.
            contEducationList.GridViewDataSource = contEducations;
            contEducationList.ContEducationSectionPosition = EducationOrExamSectionPosition.CapDetail;
            contEducationList.BindContEducations();
        }

        /// <summary>
        /// Show Examinations in CAP detail Page.
        /// </summary>
        /// <param name="capModel">CAP model contains Examination model List</param>
        private void DisplayExamination(CapModel4WS capModel)
        {
            divExamination.Visible = true;

            IList<ExaminationModel> examinations = new List<ExaminationModel>();

            if (!AppSession.IsAdmin && capModel != null && capModel.examinationList != null)
            {
                examinations = ObjectConvertUtil.ConvertArrayToList(capModel.examinationList);
            }

            ExaminationList.DataSource = examinations;
            ExaminationList.ExaminationSectionPosition = EducationOrExamSectionPosition.CapDetail;
            ExaminationList.Bind();
        }

        /// <summary>
        /// Gets and displays related caps
        /// </summary>
        /// <param name="licensee">licensee model</param>
        /// <param name="currentPageIndex">the current page index.</param>
        /// <param name="sortExpression">sort expression</param>
        private void DisplayRelatedCaps(LicenseModel4WS licensee, int currentPageIndex, string sortExpression)
        {
            List<string> enabledModuleList = GetEnabledModuleList();
            licensee.serviceProviderCode = ConfigManager.AgencyCode;
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(dgvPermitList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = dgvPermitList.PageSize;
            pageInfo.SearchCriterias = new object[] { licensee };
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            DataTable capList = capBll.GetRelatedCapsByRefLP(licensee, enabledModuleList, queryFormat);
            capList = PaginationUtil.MergeDataSource<DataTable>(dgvPermitList.GridViewDataSource, capList, pageInfo);
            dgvPermitList.IsForLicensee = true;
            dgvPermitList.InitialExport(false);
            divRelatedPermits.Visible = true;

            if (!AppSession.IsAdmin && capList != null && capList.Rows.Count > 0)
            {
                dvPromptForLicense.Visible = true;                
                dvResult.Visible = true;
                lblResult.Text = string.Format("<table role='presentation'><tr><td>{0},</td><td>{1}</td><td>{2}</td></tr></table>", licensee.licenseType, this.GetTextByKey("ACA_CapHome_LicenseNumberText"), licensee.stateLicense);
                dgvPermitList.BindDataToPermitList(capList, 0, string.Empty);
            }
            else
            {
                if (AppSession.IsAdmin)
                {
                    dvPromptForLicense.Visible = true;
                }

                dgvPermitList.BindDataToPermitList(new DataTable(), 0, string.Empty);
            }
        }

        /// <summary>
        /// get current agency's active modules' name.
        /// </summary>
        /// <returns>modules' name list</returns>
        private List<string> GetEnabledModuleList()
        {
            Dictionary<string, string> allEnabledModuleList = TabUtil.GetAllEnableModules(false);
            List<string> enableModules = new List<string>();

            if (allEnabledModuleList != null)
            {
                foreach (string key in allEnabledModuleList.Keys)
                {
                    enableModules.Add(key);
                }
            }

            return enableModules;
        }

        /// <summary>
        /// whether or not display this section according to section name and model name
        /// </summary>
        /// <param name="sectionName">section name</param>
        /// <returns>true or false</returns>
        private bool IsSectionEnabled(string sectionName)
        {
            bool isSectionEnabled = false;

            if (sectionName == LicenseVerificationSectionType.EDUCATION.ToString() || sectionName == LicenseVerificationSectionType.CONTINUE_EDUCATION.ToString() || sectionName == LicenseVerificationSectionType.EXAMINATION.ToString())
            {
                if (IsSectionExist(sectionName) && GetSectionEntityPermission(sectionName) == ACAConstant.ROLE_HASPERMISSION)
                {                    
                    isSectionEnabled = true;                    
                }
            }
            else
            {
                if (IsSectionExist(sectionName) && GetSectionEntityPermission(sectionName) == ACAConstant.ROLE_NOPERMISSION)
                {
                    isSectionEnabled = false;   
                }
                else
                {
                    isSectionEnabled = true;
                }
            }
             
            return isSectionEnabled;
        }

        /// <summary>
        /// get Section entity
        /// </summary>
        /// <param name="sectionName">section name</param>
        /// <returns>true or false</returns>
        private bool IsSectionExist(string sectionName)
        {
            bool isSectionExist = false;
            if (_capTypePermissions != null)
            {
                for (int i = 0; i < _capTypePermissions.Length; i++)
                {
                    if (_capTypePermissions[i].entityKey1 != null && _capTypePermissions[i].entityKey1.Trim() == sectionName)
                    {
                        isSectionExist = true;
                        break;
                    }
                }
            }

            return isSectionExist;
        }

        /// <summary>
        /// get section entity permission
        /// </summary>
        /// <param name="sectionName">section name</param>
        /// <returns>true or false</returns>
        private string GetSectionEntityPermission(string sectionName)
        {
            string entityPermission = string.Empty;
            if (_capTypePermissions != null)
            {
                for (int i = 0; i < _capTypePermissions.Length; i++)
                {
                    if (_capTypePermissions[i].entityKey1 != null && _capTypePermissions[i].entityKey1.Trim() == sectionName)
                    {
                        entityPermission = _capTypePermissions[i].entityPermission;
                        break;
                    }
                }
            }

            return entityPermission;
        }

        /// <summary>
        /// set the current cap values by license
        /// </summary>
        /// <param name="selectedLicenseeModel">licensee model</param>
        private void SetCurrentCAPByLicense(LicenseModel4WS selectedLicenseeModel)
        {
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            CapIDModel4WS capIDModel = new CapIDModel4WS();
            capIDModel.serviceProviderCode = ConfigManager.AgencyCode;
            capIDModel.customID = string.IsNullOrEmpty(selectedLicenseeModel.stateLicense) ? string.Empty : selectedLicenseeModel.stateLicense;
            _capModel = capBll.GetCapViewByAltID(capIDModel);
        }

        /// <summary>
        /// set the cap type permission values for the current cap model
        /// </summary>
        private void SetPermissionByCAP()
        {
            if (_capModel != null && _capModel.capType != null)
            {
                ICapTypePermissionBll capTypePermissionBll = (ICapTypePermissionBll)ObjectFactory.GetObject(typeof(ICapTypePermissionBll));
                CapTypePermissionModel capTypePermission = new CapTypePermissionModel();
                capTypePermission.serviceProviderCode = ConfigManager.AgencyCode;
                capTypePermission.controllerType = ControllerType.LICENSEVERIFICATION.ToString();
                capTypePermission.entityType = EntityType.SECTIONTYPE.ToString();
                capTypePermission.category = _capModel.capType.category;
                capTypePermission.group = _capModel.capType.group;
                capTypePermission.type = _capModel.capType.type;
                capTypePermission.subType = _capModel.capType.subType;
                capTypePermission.moduleName = _capModel.moduleName;
                _capTypePermissions = capTypePermissionBll.GetCapTypePermissions(ConfigManager.AgencyCode, capTypePermission);
            }
        }

        #endregion
    }
}
