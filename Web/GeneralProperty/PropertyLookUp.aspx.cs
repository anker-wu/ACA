#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProviderLookUp.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PropertyLookUp.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.AppCode.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.GeneralProperty
{
    /// <summary>
    /// the class for education look up.
    /// </summary>
    public partial class PropertyLookUp : BasePage
    {
        #region Fields

        /// <summary>
        /// The url parameter isLicensee.
        /// </summary>
        private const string URLPARA_IS_LICENSEE = "isLicensee";

        /// <summary>
        /// The url parameter isFoodFacility.
        /// </summary>
        private const string URLPARA_IS_FOOD_FACILITY = "isFoodFacility";

        /// <summary>
        /// The url parameter isCertBusiness.
        /// </summary>
        private const string URLPARA_IS_CERT_BUSINESS = "isCertBusiness";

        /// <summary>
        /// The url parameter isSearchDocument.
        /// </summary>
        private const string URLPARA_IS_SEARCH_DOCUMENT = "isSearchDocument";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets provider query info.
        /// </summary>
        private EducationQueryInfo EducationQueryInfo
        {
            get
            {
                if (Session[SessionConstant.SESSION_EDUCATION_QUERYINFO] == null)
                {
                    Session[SessionConstant.SESSION_EDUCATION_QUERYINFO] = new EducationQueryInfo();
                }

                return (EducationQueryInfo)Session[SessionConstant.SESSION_EDUCATION_QUERYINFO];
            }

            set
            {
                Session[SessionConstant.SESSION_EDUCATION_QUERYINFO] = value;
            }
        }

        #endregion Properties

        #region Public/Protected Methods

        /// <summary>
        /// Get instruction by label key
        /// </summary>
        /// <param name="labelKey">label key string.</param>
        /// <returns>instruction value</returns>
        [System.Web.Services.WebMethod(Description = "Get value by label key", EnableSession = true)]
        public static string GetInstructionByKey(string labelKey)
        {
            return GetStaticTextByKey(labelKey);
        }

        /// <summary>
        /// On Initial event method
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            DialogUtil.RegisterScriptForDialog(this.Page);

            certBusinessList.GridViewDownloadAll += CertBusinessList_GridViewDownloadAll;
            refProviderList.GridViewDownloadAll += RefProviderList_GridViewDownloadAll;
            refLicenseeList.GridViewDownloadAll += RefLicenseeList_GridViewDownloadAll;
            refFoodFacilityList.GridViewDownloadAll += RefFoodFacilityList_GridViewDownloadAll;
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //set the education page no cache.
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (!IsPostBack)
            {
                DropDownListBindUtil.BindEducationSearchType(ddlSearchType);

                //if only 1 option, display value as lable. hide dropdown.
                if (!AppSession.IsAdmin && ddlSearchType.Items.Count == 1)
                {
                    ddlSearchType.Visible = false;
                }

                SetDefaultType();

                InitAllUI();

                if (AppSession.IsAdmin)
                {
                    DisplayResult4Admin();
                }
                else
                {
                    ReloadData();
                }
            }

            if (!IsPostBack)
            {
                SetDivVisible();
            }

            refProviderList.PageIndexChanging += new GridViewPageEventHandler(GV_PageIndexChanging);
            refProviderList.GridViewSort += new GridViewSortedEventHandler(GV_GridViewSort);
            certBusinessList.PageIndexChanging += new GridViewPageEventHandler(GV_PageIndexChanging);
            certBusinessList.GridViewSort += new GridViewSortedEventHandler(GV_GridViewSort);
            refEducationList.PageIndexChanging += new GridViewPageEventHandler(GV_PageIndexChanging);
            refEducationList.GridViewSort += new GridViewSortedEventHandler(GV_GridViewSort);
            refContinuingEducationList.PageIndexChanging += new GridViewPageEventHandler(GV_PageIndexChanging);
            refContinuingEducationList.GridViewSort += new GridViewSortedEventHandler(GV_GridViewSort);
            refExaminationList.PageIndexChanging += new GridViewPageEventHandler(GV_PageIndexChanging);
            refExaminationList.GridViewSort += new GridViewSortedEventHandler(GV_GridViewSort);
            refLicenseeList.PageIndexChanging += new GridViewPageEventHandler(GV_PageIndexChanging);
            refLicenseeList.GridViewSort += new GridViewSortedEventHandler(GV_GridViewSort);
            refFoodFacilityList.PageIndexChanging += new GridViewPageEventHandler(GV_PageIndexChanging);
            refFoodFacilityList.GridViewSort += new GridViewSortedEventHandler(GV_GridViewSort);

            if (AppSession.IsAdmin)
            {
                divLicenseeSearchForm.Visible = true;
                divEduRelationSearchForm.Visible = true;
                divRefExamSearchForm.Visible = true;
                divFoodFacilitySearchForm.Visible = true;
                divCertBusinessSearchForm.Visible = true;
                divDocumentSearchForm.Visible = true;
                ddlSearchType.AutoPostBack = false;
                ddlSearchType.Attributes.Add("onchange", "ChangeType(this)");
            }
        }

        /// <summary>
        /// grid view page index event method.
        /// </summary>
        /// <param name="sender">An sender object that contains the event sender.</param>
        /// <param name="arg">A System.CommonEventArgs object containing the event data.</param>
        protected void GV_PageIndexChanging(object sender, GridViewPageEventArgs arg)
        {
            if (sender != null && arg != null && EducationQueryInfo != null)
            {
                AccelaGridView gdv = (AccelaGridView)sender;
                RefResultType resultType = GetGVType(gdv.GridViewNumber);
                EducationQueryInfo.ResultPageIndex = UpdateOrCreateHashTable(EducationQueryInfo.ResultPageIndex, resultType, arg.NewPageIndex);
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(EducationQueryInfo.SearchType.ToString());

                if (arg.NewPageIndex > pageInfo.EndPage)
                {
                    SearchProcess(arg.NewPageIndex, pageInfo.SortExpression);
                }
            }
        }

        /// <summary>
        /// grid view sorted event to record the latest sort expression.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void GV_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (sender != null && e != null && EducationQueryInfo != null)
            {
                AccelaGridView gdv = (AccelaGridView)sender;
                RefResultType resultType = GetGVType(gdv.GridViewNumber);
                EducationQueryInfo.ResultSortExpression = UpdateOrCreateHashTable(EducationQueryInfo.ResultSortExpression, resultType, e.GridViewSortExpression);
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(EducationQueryInfo.SearchType.ToString());
                pageInfo.SortExpression = e.GridViewSortExpression;
            }
        }

        /// <summary>
        /// Raises "new search" button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void NewSearchButton_Click(object sender, EventArgs e)
        {
            MessageUtil.HideMessageByControl(Page);

            //Comparison Operator in SearchByCertifiedBusiness.
            if (!certifiedBusinessSearchForm.IsComparisonOperator(SelectedIndex()))
            {
                MessageUtil.ShowMessageByControl(this.Page, MessageType.Error, GetTextByKey("aca_invalid_operator_nigp"));
                return;
            }

            if (!certifiedBusinessSearchForm.CheckDate())
            {
                return;
            }

            refProviderList.DataSource = null;
            refLicenseeList.DataSource = null;
            refFoodFacilityList.DataSource = null;
            certBusinessList.DataSource = null;
            certBusinessList.InitialShortListAction();
            Page.FocusElement(btnNewSearch.ClientID);
            SearchProcess(0, null, true);
        }

        /// <summary>
        /// Raises "Reset search" button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ResetSearchButton_Click(object sender, EventArgs e)
        {
            MessageUtil.HideMessageByControl(Page);
            InitAllUI();
            certBusinessList.InitialShortListAction();

            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoSearchForm_Start", "scrollIntoView('SearchForm_Start');", true);
        }

        /// <summary>
        /// Dropdown list command
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void SearchTypeDropDown_IndexChanged(object sender, EventArgs e)
        {
            InitAllUI();
            SetDivVisible();

            if (!AppSession.IsAdmin)
            {
                educationRelationSearchForm.SetCurrentCityAndState();
                refExamSearchForm.SetCurrentCityAndState();
                refLicenseeSearchForm.SetCurrentCityAndState();
                refFoodFacilitySearchForm.SetCurrentCityAndState();
            }
        }

        /// <summary>
        /// Override the function, identify login with the parameter
        /// which is licensee, food facility or certified business.
        /// </summary>
        /// <param name="url">The request url.</param>
        /// <returns>Return true or false show that need force login or not.</returns>
        protected override bool IsFeatureForceLogin(string url)
        {
            string parameter = string.Empty;

            if (!IsPostBack)
            {
                // The first load, get the parameter from url.
                if (Request[URLPARA_IS_LICENSEE] != null)
                {
                    parameter = URLPARA_IS_LICENSEE;
                }
                else if (Request[URLPARA_IS_FOOD_FACILITY] != null)
                {
                    parameter = URLPARA_IS_FOOD_FACILITY;
                }
                else if (Request[URLPARA_IS_CERT_BUSINESS] != null)
                {
                    parameter = URLPARA_IS_CERT_BUSINESS;
                }
                else if (Request[URLPARA_IS_SEARCH_DOCUMENT] != null)
                {
                    parameter = URLPARA_IS_SEARCH_DOCUMENT;
                }
            }
            else
            {
                // When postback, get the parameter from the dropdownlist item.
                switch ((GeneralInformationSearchType)SelectedIndex())
                {
                    case GeneralInformationSearchType.Search4Licensee:
                        parameter = URLPARA_IS_LICENSEE;
                        break;
                    case GeneralInformationSearchType.Search4FoodFacilityInspection:
                        parameter = URLPARA_IS_FOOD_FACILITY;
                        break;
                    case GeneralInformationSearchType.Search4CertifiedBusiness:
                        parameter = URLPARA_IS_CERT_BUSINESS;
                        break;
                    case GeneralInformationSearchType.Search4Document:
                        parameter = URLPARA_IS_SEARCH_DOCUMENT;
                        break;
                    case GeneralInformationSearchType.Search4Provider:
                    case GeneralInformationSearchType.Search4EduAndExam:
                    default:
                        // do nothing.
                        break;
                }
            }

            IBizDomainBll bizDomain = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            return bizDomain.IsForceLogin(ModuleName, url, parameter);
        }

        #endregion Public/Protected Methods

        #region Private Methods

        /// <summary>
        /// get education type dropdown list selected index.
        /// </summary>
        /// <returns>dropdown selected index</returns>
        private int SelectedIndex()
        {
            int index = 0;
            string selectedValue = ddlSearchType.SelectedValue;

            if (!string.IsNullOrEmpty(selectedValue))
            {
                if (selectedValue.IndexOf("||") != -1)
                {
                    // remove "||"
                    selectedValue = selectedValue.Substring(0, selectedValue.IndexOf("||"));
                }

                string strIndex = selectedValue.Replace("-", string.Empty);

                if (ValidationUtil.IsInt(strIndex))
                {
                    index = int.Parse(strIndex);
                }
            }
            
            return index;
        }

        /// <summary>
        /// Valid Selected Index Number.
        /// </summary>
        /// <param name="index">dropdown list selected index</param>
        /// <returns>true / false</returns>
        private bool ValidSelectedIndex(int index)
        {
            bool isValid = false;
            Array array  = Enum.GetValues(typeof(GeneralInformationSearchType));

            if (array != null)
            {
                if (index >= 0 && index < array.Length)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Get grid view type
        /// </summary>
        /// <param name="gridViewNumber">grid view number</param>
        /// <returns>education search result.</returns>
        private RefResultType GetGVType(string gridViewNumber)
        {
            RefResultType resultType = RefResultType.Provider;

            if (!string.IsNullOrEmpty(gridViewNumber))
            {
                switch (gridViewNumber)
                {
                    case GviewID.ProviderList:
                        resultType = RefResultType.Provider;
                        break;
                    case GviewID.EducationList:
                        resultType = RefResultType.Education;
                        break;
                    case GviewID.ContinuingEducationList:
                        resultType = RefResultType.ContEdu;
                        break;
                    case GviewID.ExaminationList:
                        resultType = RefResultType.Examination;
                        break;
                    case GviewID.LicenseeList:
                        resultType = RefResultType.Licensee;
                        break;
                    case GviewID.FoodFacilityList:
                        resultType = RefResultType.FoodFacility;
                        break;
                    case GviewID.CertifiedBusinessList:
                        resultType = RefResultType.CertifiedBusiness;
                        break;
                }
            }

            return resultType;
        }
        
        /// <summary>
        /// Update or Create hash table.
        /// </summary>
        /// <param name="ht">hash table.</param>
        /// <param name="tableType">ref result table type</param>
        /// <param name="value">the value data.</param>
        /// <returns>hashTable result</returns>
        private Hashtable UpdateOrCreateHashTable(Hashtable ht, RefResultType tableType, object value)
        {
            Hashtable htResult = ht;

            if (htResult == null)
            {
                htResult = new Hashtable();
            }

            if (htResult.Contains(tableType))
            {
                htResult[tableType] = value;
            }
            else
            {
                htResult.Add(tableType, value);
            }

            return htResult;
        }

        /// <summary>
        /// Display page element by different search type
        /// </summary>
        private void SetDivVisible()
        {
            string prefix = string.Empty;
            string sectionId = string.Empty;
            int index = SelectedIndex();

            //if invalidate selected index number.
            if (!ValidSelectedIndex(index))
            {
                return;
            }

            string instructionKey = string.Empty;

            // set the search form visible
            divEduRelationSearchForm.Visible = false;
            divRefExamSearchForm.Visible = false;
            divLicenseeSearchForm.Visible = false;
            divFoodFacilitySearchForm.Visible = false;
            divCertBusinessSearchForm.Visible = false;
            divDocumentSearchForm.Visible = false;
            btnNewSearch.Visible = true;
            btnResetSearch.Visible = true;

            switch ((GeneralInformationSearchType)index)
            {
                case GeneralInformationSearchType.Search4Provider:
                    divEduRelationSearchForm.Visible = true;
                    prefix = educationRelationSearchForm.ContactIDPrefix;
                    sectionId = GviewID.SearchForProvider;
                    instructionKey = "per_propertylookup_instruction_searchforprovider";
                    break;
                case GeneralInformationSearchType.Search4EduAndExam:
                    divRefExamSearchForm.Visible = true;
                    prefix = refExamSearchForm.ContactIDPrefix;
                    sectionId = GviewID.SearchForEducationAndExam;
                    instructionKey = "per_propertylookup_instruction_searchforeducation";                    
                    break;
                case GeneralInformationSearchType.Search4Licensee:
                    divLicenseeSearchForm.Visible = true;
                    prefix = refLicenseeSearchForm.ClientID + ACAConstant.SPLIT_CHAR5;
                    sectionId = GviewID.SearchForLicensee;
                    instructionKey = "per_propertylookup_instruction_searchforlicensee";
                    break;
                case GeneralInformationSearchType.Search4FoodFacilityInspection:
                    divFoodFacilitySearchForm.Visible = true;
                    prefix = refFoodFacilitySearchForm.ClientID + ACAConstant.SPLIT_CHAR5;
                    sectionId = GviewID.SearchForFoodFacility;
                    instructionKey = "aca_foodfacility_label_searchinstruction";
                    break;
                case GeneralInformationSearchType.Search4CertifiedBusiness:
                    divCertBusinessSearchForm.Visible = true;
                    prefix = certifiedBusinessSearchForm.ClientID + ACAConstant.SPLIT_CHAR5;
                    sectionId = GviewID.SearchForCertifiedBusiness;
                    instructionKey = "aca_certbusiness_label_searchinstruction";
                    break;
                case GeneralInformationSearchType.Search4Document:
                    divDocumentSearchForm.Visible = true;
                    btnNewSearch.Visible = AppSession.IsAdmin;
                    btnResetSearch.Visible = AppSession.IsAdmin;

                    if (!AppSession.IsAdmin)
                    {
                        documentSearchForm.ShowMap();
                    }

                    prefix = documentSearchForm.ClientID + ACAConstant.SPLIT_CHAR5;
                    instructionKey = "aca_searchdocument_label_searchinstruction";
                    break;
            }

            lblSearchInstruction.Text = GetTextByKey(instructionKey);

            if (!AppSession.IsAdmin)
            {
                divInstruction.Visible = !string.IsNullOrEmpty(lblSearchInstruction.Text);
            }

            if ((GeneralInformationSearchType)index == GeneralInformationSearchType.Search4Document)
            {
                ddlSearchType.SectionID = string.Format("{0}" + ACAConstant.SPLIT_CHAR + "{1}", ConfigManager.AgencyCode, prefix);
            }
            else
            {
                ddlSearchType.SectionID = string.Format("{0}" + ACAConstant.SPLIT_CHAR + "{1}" + ACAConstant.SPLIT_CHAR + "{2}", ConfigManager.AgencyCode, sectionId, prefix);
            }

            ddlSearchType.StdCategory = ACAConstant.EDUCATION_LOOKUP;
        }

        /// <summary>
        /// Display Result Data List
        /// </summary>
        /// <param name="educationSearchType">education search type</param>
        /// <param name="htResultCollection">date table result array</param>
        /// <param name="htPageIndex">the list page index.</param>
        /// <param name="htSort">the list page sort.</param>
        /// <param name="isSearch">is search.</param>
        private void DisplayResultList(GeneralInformationSearchType educationSearchType, Hashtable htResultCollection, Hashtable htPageIndex, Hashtable htSort, bool isSearch = false)
        {
            InitSearchRestult();

            if (htResultCollection == null || htResultCollection.Count == 0)
            {
                noResultMessage.Show(MessageType.Notice, "per_educationlookup_label_error_noresult", MessageSeperationType.Both);
                return;
            }

            switch (educationSearchType)
            {
                case GeneralInformationSearchType.Search4Provider:
                    DisplayProviderResultList(htResultCollection, htPageIndex, htSort, isSearch);

                    break;
                case GeneralInformationSearchType.Search4EduAndExam:
                    DisplayLicenseCertification(htResultCollection, htPageIndex, htSort, isSearch);

                    break;
                case GeneralInformationSearchType.Search4Licensee:
                    DisplayLicenseeList(htResultCollection, htPageIndex, htSort, isSearch);

                    break;
                case GeneralInformationSearchType.Search4FoodFacilityInspection:
                    DisplayFoodFacilityList(htResultCollection, htPageIndex, htSort, isSearch);

                    break;
                case GeneralInformationSearchType.Search4CertifiedBusiness:
                    DisplayCertifiedBusinessList(htResultCollection, htPageIndex, htSort, isSearch);

                    break;
            }
        }

        /// <summary>
        /// Display license certification.
        /// </summary>
        /// <param name="htResultCollection">The result collection.</param>
        /// <param name="htPageIndex">The page index.</param>
        /// <param name="htSort">The sort.</param>
        /// <param name="isSearch">Is search</param>
        private void DisplayLicenseCertification(Hashtable htResultCollection, Hashtable htPageIndex, Hashtable htSort, bool isSearch = false)
        {
            DisplayEducationResultList(htResultCollection, htPageIndex, htSort);
            DisplayContEduResultList(htResultCollection, htPageIndex, htSort);
            DisplayExaminationResultList(htResultCollection, htPageIndex, htSort);

            int eduAndExamCount = GetEduAndExamCount(htResultCollection);

            if (isSearch)
            {
                RedirectToEduAndExamDetail(htResultCollection);
            }
            else
            {
                if (eduAndExamCount == 1)
                {
                    divHrforResult.Visible = false;
                    divEducationResult.Visible = false;
                    divContEduResult.Visible = false;
                    divExamResult.Visible = false;
                }
            }

            if (eduAndExamCount == 0)
            {
                noResultMessage.Show(MessageType.Notice, "per_educationlookup_label_error_noresult", MessageSeperationType.Both);
            }
        }

        /// <summary>
        /// Display examination search result.
        /// </summary>
        /// <param name="htResultCollection">hashTable for search result</param>
        /// <param name="htPageIndex">hashTable for the list page index.</param>
        /// <param name="htSort">hashTable for the list page sort.</param>
        private void DisplayExaminationResultList(Hashtable htResultCollection, Hashtable htPageIndex, Hashtable htSort)
        {
            if (htResultCollection.Contains(RefResultType.Examination))
            {
                DataTable dtExam = htResultCollection[RefResultType.Examination] as DataTable;

                if (dtExam != null && (dtExam.Rows.Count > 0 || AppSession.IsAdmin))
                {
                    refExaminationList.DataSource = dtExam;
                    int index = htPageIndex != null && htPageIndex.Contains(RefResultType.Examination) ? (int)htPageIndex[RefResultType.Examination] : 0;
                    string sort = htSort != null && htSort.Contains(RefResultType.Examination) ? (string)htSort[RefResultType.Examination] : string.Empty;
                    refExaminationList.BindExaminationList(index, sort);
                    lblExamContEduResult.Text = dtExam.Rows.Count.ToString();
                    divExamResult.Visible = true;
                }
                else
                {
                    divExamResult.Visible = false;
                }
            }
        }

        /// <summary>
        /// Display continuing education search result.
        /// </summary>
        /// <param name="htResultCollection">hashTable for search result</param>
        /// <param name="htPageIndex">hashTable for the list page index.</param>
        /// <param name="htSort">hashTable for the list page sort.</param>
        private void DisplayContEduResultList(Hashtable htResultCollection, Hashtable htPageIndex, Hashtable htSort)
        {
            if (htResultCollection.Contains(RefResultType.ContEdu))
            {
                DataTable dtContEdu = htResultCollection[RefResultType.ContEdu] as DataTable;

                if (dtContEdu != null && (dtContEdu.Rows.Count > 0 || AppSession.IsAdmin))
                {
                    refContinuingEducationList.DataSource = dtContEdu;
                    int index = htPageIndex != null && htPageIndex.Contains(RefResultType.ContEdu) ? (int)htPageIndex[RefResultType.ContEdu] : 0;
                    string sort = htSort != null && htSort.Contains(RefResultType.ContEdu) ? (string)htSort[RefResultType.ContEdu] : string.Empty;
                    refContinuingEducationList.BindContEduList(index, sort);
                    lblCountNumContEduResult.Text = dtContEdu.Rows.Count.ToString();
                    divContEduResult.Visible = true;
                }
                else
                {
                    divContEduResult.Visible = false;
                }
            }
        }

        /// <summary>
        /// Display education search result.
        /// </summary>
        /// <param name="htResultCollection">hashTable for search result</param>
        /// <param name="htPageIndex">hashTable for the list page index.</param>
        /// <param name="htSort">hashTable for the list page sort.</param>
        private void DisplayEducationResultList(Hashtable htResultCollection, Hashtable htPageIndex, Hashtable htSort)
        {
            if (htResultCollection.Contains(RefResultType.Education))
            {
                DataTable dtEducation = htResultCollection[RefResultType.Education] as DataTable;

                if (dtEducation != null && (dtEducation.Rows.Count > 0 || AppSession.IsAdmin))
                {
                    refEducationList.DataSource = dtEducation;
                    int index = htPageIndex != null && htPageIndex.Contains(RefResultType.Education) ? (int)htPageIndex[RefResultType.Education] : 0;
                    string sort = htSort != null && htSort.Contains(RefResultType.Education) ? (string)htSort[RefResultType.Education] : string.Empty;
                    refEducationList.BindEducationList(index, sort);

                    lblCountEducationResult.Text = dtEducation.Rows.Count.ToString();
                    lblCountEducationCount.Visible = true;
                    divEducationResult.Visible = true;
                }
                else
                {
                    divEducationResult.Visible = false;
                }
            }
        }

        /// <summary>
        /// Display provider search result.
        /// </summary>
        /// <param name="htResultCollection">hashTable for search result</param>
        /// <param name="htPageIndex">hashTable for the list page index.</param>
        /// <param name="htSort">hashTable for the list page sort.</param>
        /// <param name="isSearch">is search.</param>
        private void DisplayProviderResultList(Hashtable htResultCollection, Hashtable htPageIndex, Hashtable htSort, bool isSearch = false)
        {
            if (htResultCollection.Contains(RefResultType.Provider))
            {
                DataTable dtProvider = htResultCollection[RefResultType.Provider] as DataTable;
                int index = htPageIndex != null && htPageIndex.Contains(RefResultType.Provider) ? (int)htPageIndex[RefResultType.Provider] : 0;
                string sort = htSort != null && htSort.Contains(RefResultType.Provider) ? (string)htSort[RefResultType.Provider] : string.Empty;

                if (dtProvider != null && (dtProvider.Rows.Count > 0 || AppSession.IsAdmin))
                {
                    if (dtProvider.Rows.Count == 1)
                    {
                        if (isSearch)
                        {
                            RedirectToProviderDetail(htResultCollection);
                            return;
                        }

                        divProviderResult.Visible = false;
                        divHrforResult.Visible = false;
                    }
                    else
                    {
                        refProviderList.DataSource = dtProvider;
                        refProviderList.BindProviderList(index, sort);
                        lblCountNumResult.Text = refProviderList.CountSummary;
                        lblCountProviderCount.Visible = true;
                        divProviderResult.Visible = true;
                        divHrforResult.Visible = true;
                    }
                }
                else
                {
                    noResultMessage.Show(MessageType.Notice, "per_educationlookup_label_error_noresult", MessageSeperationType.Both);
                }
            }
        }

        /// <summary>
        /// Display license search result.
        /// </summary>
        /// <param name="htResultCollection">hashTable for search result</param>
        /// <param name="htPageIndex">hashTable for the list page index.</param>
        /// <param name="htSort">hashTable for the list page sort.</param>
        /// <param name="isSearch">is search.</param>
        private void DisplayLicenseeList(Hashtable htResultCollection, Hashtable htPageIndex, Hashtable htSort, bool isSearch = false)
        {
            if (htResultCollection.Contains(RefResultType.Licensee))
            {
                IList<LicenseModel4WS> licenseeList = (IList<LicenseModel4WS>)htResultCollection[RefResultType.Licensee];

                if (licenseeList != null && (licenseeList.Count > 0 || AppSession.IsAdmin))
                {
                    if (licenseeList.Count == 1)
                    {
                        if (isSearch)
                        {
                            RedirectToLicenseDetail(htResultCollection);
                            return;
                        }

                        divHrforResult.Visible = false;
                        divRefLicenseeResult.Visible = false;
                    }
                    else
                    {
                        refLicenseeList.DataSource = licenseeList;
                        int index = htPageIndex != null && htPageIndex.Contains(RefResultType.Licensee) ? (int)htPageIndex[RefResultType.Licensee] : 0;
                        string sort = htSort != null && htSort.Contains(RefResultType.Licensee) ? (string)htSort[RefResultType.Licensee] : string.Empty;
                        refLicenseeList.BindLicenseeList(index, sort);
                        lblLicenseeResult.Text = refLicenseeList.CountSummary;
                        divRefLicenseeResult.Visible = true;
                        divHrforResult.Visible = true;
                    }
                }
                else
                {
                    noResultMessage.Show(MessageType.Notice, "per_educationlookup_label_error_noresult", MessageSeperationType.Both);
                }
            }
        }

        /// <summary>
        /// Display food facility search result.
        /// </summary>
        /// <param name="htResultCollection">hashTable for search result</param>
        /// <param name="htPageIndex">hashTable for the list page index.</param>
        /// <param name="htSort">hashTable for the list page sort.</param>
        /// <param name="isSearch">is search.</param>
        private void DisplayFoodFacilityList(Hashtable htResultCollection, Hashtable htPageIndex, Hashtable htSort, bool isSearch = false)
        {
            if (htResultCollection.Contains(RefResultType.FoodFacility))
            {
                IList<LicenseModel4WS> licenseeList = (IList<LicenseModel4WS>)htResultCollection[RefResultType.FoodFacility];

                if (licenseeList != null && (licenseeList.Count > 0 || AppSession.IsAdmin))
                {
                    if (licenseeList.Count == 1)
                    {
                        if (isSearch)
                        {
                            RedirectToFoodFacilityInspectionDetail(htResultCollection);
                            return;
                        }

                        divHrforResult.Visible = false;
                        divFoodFacilityResult.Visible = false;
                    }
                    else
                    {
                        refFoodFacilityList.DataSource = licenseeList;
                        int index = htPageIndex != null && htPageIndex.Contains(RefResultType.FoodFacility) ? (int)htPageIndex[RefResultType.FoodFacility] : 0;
                        string sort = htSort != null && htSort.Contains(RefResultType.FoodFacility) ? (string)htSort[RefResultType.FoodFacility] : string.Empty;
                        refFoodFacilityList.BindLicenseeList(index, sort);
                        lblFoodFacilityResult.Text = refFoodFacilityList.CountSummary;
                        divFoodFacilityResult.Visible = true;
                        divHrforResult.Visible = true;
                    }
                }
                else
                {
                    noResultMessage.Show(MessageType.Notice, "per_educationlookup_label_error_noresult", MessageSeperationType.Both);
                }
            }
        }

        /// <summary>
        /// Display food facility search result.
        /// </summary>
        /// <param name="htResultCollection">hashTable for search result</param>
        /// <param name="htPageIndex">hashTable for the list page index.</param>
        /// <param name="htSort">hashTable for the list page sort.</param>
        /// <param name="isSearch">is search.</param>
        private void DisplayCertifiedBusinessList(Hashtable htResultCollection, Hashtable htPageIndex, Hashtable htSort, bool isSearch = false)
        {
            if (htResultCollection.Contains(RefResultType.CertifiedBusiness))
            {
                DataTable licenseeList = (DataTable)htResultCollection[RefResultType.CertifiedBusiness];

                if (licenseeList != null && (licenseeList.Rows.Count > 0 || AppSession.IsAdmin))
                {
                    if (licenseeList.Rows.Count == 1)
                    {
                        if (isSearch)
                        {
                            RedirectToCertifiedBusinessDetail(htResultCollection);
                            return;
                        }

                        divHrforResult.Visible = false;
                        divCertBusinessResult.Visible = false;
                    }
                    else
                    {
                        certBusinessList.DataSource = licenseeList;
                        int index = htPageIndex != null && htPageIndex.Contains(RefResultType.CertifiedBusiness) ? (int)htPageIndex[RefResultType.CertifiedBusiness] : 0;
                        string sort = htSort != null && htSort.Contains(RefResultType.CertifiedBusiness) ? (string)htSort[RefResultType.CertifiedBusiness] : string.Empty;
                        certBusinessList.BindLicenseeList(index, sort);
                        lblCertBusinessCount.Text = certBusinessList.CountSummary;
                        divCertBusinessResult.Visible = true;
                        divHrforResult.Visible = true;
                    }
                }
                else
                {
                    noResultMessage.Show(MessageType.Notice, "per_educationlookup_label_error_noresult", MessageSeperationType.Both);
                }
            }
        }

        /// <summary>
        /// IE Back load data
        /// </summary>
        private void ReloadData()
        {
            EducationQueryInfo educationQueryInfo = (EducationQueryInfo)Session[SessionConstant.SESSION_EDUCATION_QUERYINFO];

            if (educationQueryInfo == null)
            {
                return;
            }

            //1. Setting search type.
            ddlSearchType.SelectedValue = Convert.ToString((int)educationQueryInfo.SearchType);
            lblSelectedSearchType.InnerText = ddlSearchType.SelectedItem != null ? ddlSearchType.SelectedItem.Text : string.Empty;

            switch (educationQueryInfo.SearchType)
            {
                case GeneralInformationSearchType.Search4Provider:
                    EducationSearchModel educationSearchModel = (EducationSearchModel)educationQueryInfo.SearchModel;
                    educationRelationSearchForm.ShowProviderInfo(educationSearchModel);

                    break;
                case GeneralInformationSearchType.Search4EduAndExam:
                    EducationSearchModel provider4EduandExam = (EducationSearchModel)educationQueryInfo.SearchModel;
                    refExamSearchForm.ShowProviderInfo(provider4EduandExam);

                    break;
                case GeneralInformationSearchType.Search4Licensee:
                    Hashtable userInputData = (Hashtable)educationQueryInfo.SearchModel;
                    refLicenseeSearchForm.ShowLicenseInfo(userInputData);

                    break;
                case GeneralInformationSearchType.Search4FoodFacilityInspection:
                    Hashtable foodFacilityData = (Hashtable)educationQueryInfo.SearchModel;
                    refFoodFacilitySearchForm.ShowLicenseInfo(foodFacilityData);

                    break;
                case GeneralInformationSearchType.Search4CertifiedBusiness:
                    SearchLicenseModel certBusinessData = (SearchLicenseModel)educationQueryInfo.SearchModel;
                    certifiedBusinessSearchForm.ShowLicenseInfo(certBusinessData);

                    break;
            }

            //3. Show search result.
            DisplayResultList(educationQueryInfo.SearchType, educationQueryInfo.SearchResultCollection, educationQueryInfo.ResultPageIndex, educationQueryInfo.ResultSortExpression);
        }

        /// <summary>
        /// Initial the all UI
        /// </summary>
        private void InitAllUI()
        {
            if (ddlSearchType.Items.Count > 0)
            {
                lblSelectedSearchType.InnerText = ddlSearchType.SelectedItem.Text;
            }

            InitSearchRestult();

            educationRelationSearchForm.InitProviderForm();
            refExamSearchForm.InitProviderForm();
            refLicenseeSearchForm.InitLicenseForm();
            refFoodFacilitySearchForm.InitLicenseForm();
            certifiedBusinessSearchForm.InitCertifiedBusinessForm();
        }

        /// <summary>
        /// Initial the search result section
        /// </summary>
        private void InitSearchRestult()
        {
            noResultMessage.Hide();

            divHrforResult.Visible = false;
            divProviderResult.Visible = false;
            divEducationResult.Visible = false;
            divContEduResult.Visible = false;
            divExamResult.Visible = false;
            divRefLicenseeResult.Visible = false;
            divFoodFacilityResult.Visible = false;
            divCertBusinessResult.Visible = false;
            divDocumentSearchForm.Visible = false;
        }

        /// <summary>
        /// Save provider query info.
        /// </summary>
        /// <param name="serchType">search type</param>
        /// <param name="obj">search form model</param>
        /// <param name="searchResultCollection">search results</param>
        /// <param name="htResultPageIndex">result page index</param>
        /// <param name="htResultSortExpression">result sort expression.</param>
        private void SaveProviderQueryInfo(GeneralInformationSearchType serchType, object obj, Hashtable searchResultCollection, Hashtable htResultPageIndex, Hashtable htResultSortExpression)
        {
            if (EducationQueryInfo != null)
            {
                EducationQueryInfo.SearchType = serchType;
                EducationQueryInfo.SearchModel = obj;
                EducationQueryInfo.SearchResultCollection = searchResultCollection;
                EducationQueryInfo.ResultPageIndex = htResultPageIndex;
                EducationQueryInfo.ResultSortExpression = htResultSortExpression;
            }
        }

        /// <summary>
        /// search by licensee.
        /// </summary>
        /// <param name="queryFormat">The QueryFormat model.</param>
        /// <returns>license list</returns>
        private IList<LicenseModel4WS> SearchByLicensee(QueryFormat queryFormat)
        {
            IList<LicenseModel4WS> licenseModels = refLicenseeSearchForm.SearchLicensee(queryFormat);

            return licenseModels;
        }

        /// <summary>
        /// search by food facility.
        /// </summary>
        /// <param name="queryFormat">The QueryFormat model.</param>
        /// <returns>food facility list</returns>
        private IList<LicenseModel4WS> SearchByFoodFacility(QueryFormat queryFormat)
        {
            IList<LicenseModel4WS> licenseModels = refFoodFacilitySearchForm.SearchLicensee(queryFormat);

            return licenseModels;
        }

        /// <summary>
        /// search by certified business.
        /// </summary>
        /// <param name="queryFormat">The QueryFormat model.</param>
        /// <returns>certified business list</returns>
        private DataTable SearchByCertBusiness(QueryFormat queryFormat)
        {
            DataTable dataSource = certifiedBusinessSearchForm.SearchLicensee(queryFormat, true);

            return dataSource;
        }

        /// <summary>
        /// display dummy result for aca admin.
        /// </summary>
        private void DisplayResult4Admin()
        {
            DataTable dtEmpty = new DataTable();
            Hashtable dtSearchResult = new Hashtable();
            IList<LicenseModel4WS> emptyList = new List<LicenseModel4WS>();

            dtSearchResult = UpdateOrCreateHashTable(dtSearchResult, RefResultType.Provider, dtEmpty);
            dtSearchResult = UpdateOrCreateHashTable(dtSearchResult, RefResultType.Education, dtEmpty);
            dtSearchResult = UpdateOrCreateHashTable(dtSearchResult, RefResultType.ContEdu, dtEmpty);
            dtSearchResult = UpdateOrCreateHashTable(dtSearchResult, RefResultType.Examination, dtEmpty);
            dtSearchResult = UpdateOrCreateHashTable(dtSearchResult, RefResultType.Licensee, emptyList);
            dtSearchResult = UpdateOrCreateHashTable(dtSearchResult, RefResultType.FoodFacility, emptyList);
            dtSearchResult = UpdateOrCreateHashTable(dtSearchResult, RefResultType.CertifiedBusiness, dtEmpty);

            DisplayProviderResultList(dtSearchResult, null, null);
            DisplayEducationResultList(dtSearchResult, null, null);
            DisplayContEduResultList(dtSearchResult, null, null);
            DisplayExaminationResultList(dtSearchResult, null, null);
            DisplayLicenseeList(dtSearchResult, null, null);
            DisplayFoodFacilityList(dtSearchResult, null, null);
            DisplayCertifiedBusinessList(dtSearchResult, null, null);
        }

        /// <summary>
        /// Set look up default type.
        /// </summary>
        private void SetDefaultType()
        {
            string isLicensee = Request[URLPARA_IS_LICENSEE] == null ? ACAConstant.COMMON_N : Request[URLPARA_IS_LICENSEE].ToString();
            string isFoodFacility = Request[URLPARA_IS_FOOD_FACILITY] == null ? ACAConstant.COMMON_N : Request[URLPARA_IS_FOOD_FACILITY].ToString();
            string isCertBusiness = Request[URLPARA_IS_CERT_BUSINESS] == null ? ACAConstant.COMMON_N : Request[URLPARA_IS_CERT_BUSINESS].ToString();
            string isSearchDocument = Request[URLPARA_IS_SEARCH_DOCUMENT] == null ? ACAConstant.COMMON_N : Request[URLPARA_IS_SEARCH_DOCUMENT].ToString();

            if (ddlSearchType.Items != null && ddlSearchType.Items.Count > 0)
            {
                GeneralInformationSearchType defaultEnumValue = GeneralInformationSearchType.Search4Provider;

                if (ACAConstant.COMMON_Y.Equals(isLicensee, StringComparison.InvariantCultureIgnoreCase))
                {
                    defaultEnumValue = GeneralInformationSearchType.Search4Licensee;
                }
                else if (ACAConstant.COMMON_Y.Equals(isFoodFacility, StringComparison.InvariantCultureIgnoreCase))
                {
                    defaultEnumValue = GeneralInformationSearchType.Search4FoodFacilityInspection;
                }
                else if (ACAConstant.COMMON_Y.Equals(isCertBusiness, StringComparison.InvariantCultureIgnoreCase))
                {
                    defaultEnumValue = GeneralInformationSearchType.Search4CertifiedBusiness;
                }
                else if (ACAConstant.COMMON_Y.Equals(isSearchDocument, StringComparison.InvariantCultureIgnoreCase))
                {
                    defaultEnumValue = GeneralInformationSearchType.Search4Document;
                }

                string defaultValue = ((int)defaultEnumValue).ToString();
                string defaultWithDisableValue = ACAConstant.SPLIT_CHAR4 + defaultValue;

                // set the default value
                for (int i = 0; i < ddlSearchType.Items.Count; i++)
                {
                    if (ddlSearchType.Items[i].Value.StartsWith(defaultValue))
                    {
                        ddlSearchType.Items[i].Selected = true;
                        break;
                    }

                    // in admin, when the dropdownlist is set disablee or invisible, it should show. And the item value is changed to the format invisible.
                    if (AppSession.IsAdmin && ddlSearchType.Items[i].Value.StartsWith(defaultWithDisableValue))
                    {
                        ddlSearchType.Items[i].Selected = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// The search operation.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <param name="isSearch">is search.</param>
        private void SearchProcess(int currentPageIndex, string sortExpression, bool isSearch = false)
        {
            //1.Get search form info.            
            object obj = null;

            //2. Search provider list by search form info.
            int selectedIndex = SelectedIndex();

            //if invalidate selected index number.
            if (!ValidSelectedIndex(selectedIndex))
            {
                return;
            }
       
            GeneralInformationSearchType educationSearchType = (GeneralInformationSearchType)selectedIndex;
            Hashtable htResultCollection = new Hashtable();
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(educationSearchType.ToString());
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            QueryFormat queryFormat = null;

            //3. Get search result.
            switch (educationSearchType)
            {
                case GeneralInformationSearchType.Search4Provider:
                    pageInfo.CustomPageSize = refProviderList.PageSize;
                    queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                    DataTable dtProvider = GetProviderByQueryFormat(queryFormat, out obj).DataSource;
                    dtProvider = PaginationUtil.MergeDataSource<DataTable>(refProviderList.DataSource, dtProvider, pageInfo);
                    htResultCollection = UpdateOrCreateHashTable(htResultCollection, RefResultType.Provider, dtProvider);
                    break;
                case GeneralInformationSearchType.Search4EduAndExam:
                    //1. Get education list.
                    EducationSearchModel refExamSearchModel = refExamSearchForm.GetProviderModel();
                    obj = refExamSearchModel;
                    IRefEducationBll refEducationBll = (IRefEducationBll)ObjectFactory.GetObject(typeof(IRefEducationBll));
                    RefEducationModel4WS[] educationInfoModels = refEducationBll.GetRefEducationListByProvider(refExamSearchModel.providerModel4WS, refExamSearchModel.capTypeModel);
                    DataTable dtEducation = refEducationList.ConvertEducationListToDataTable(educationInfoModels);
                    htResultCollection = UpdateOrCreateHashTable(htResultCollection, RefResultType.Education, dtEducation);

                    //2. Get continuing education list.
                    IRefContinuingEducationBll refContEduBll = (IRefContinuingEducationBll)ObjectFactory.GetObject(typeof(IRefContinuingEducationBll));
                    RefContinuingEducationModel4WS[] refContEduModels = refContEduBll.GetRefContEducationListByProvider(refExamSearchModel.providerModel4WS, refExamSearchModel.capTypeModel);
                    DataTable dtContEdu = refContinuingEducationList.ConvertListToDataTable(refContEduModels);
                    htResultCollection = UpdateOrCreateHashTable(htResultCollection, RefResultType.ContEdu, dtContEdu);

                    //3. Get examination list.
                    IRefExaminationBll refExamBll = (IRefExaminationBll)ObjectFactory.GetObject(typeof(IRefExaminationBll));
                    RefExaminationModel4WS[] refExamModels = refExamBll.GetRefExaminationListByProvider(refExamSearchModel.providerModel4WS, refExamSearchModel.capTypeModel);
                    DataTable dtExam = refExaminationList.ConvertListToDataTable(refExamModels);
                    htResultCollection = UpdateOrCreateHashTable(htResultCollection, RefResultType.Examination, dtExam);

                    break;
                case GeneralInformationSearchType.Search4Licensee:
                    pageInfo.CustomPageSize = refLicenseeList.PageSize;
                    queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                    IList<LicenseModel4WS> licenseModels = SearchByLicensee(queryFormat);
                    licenseModels = PaginationUtil.MergeDataSource<IList<LicenseModel4WS>>(refLicenseeList.DataSource, licenseModels, pageInfo);
                    htResultCollection = UpdateOrCreateHashTable(htResultCollection, RefResultType.Licensee, licenseModels);
                    obj = refLicenseeSearchForm.GetSearchCondition();

                    break;
                case GeneralInformationSearchType.Search4FoodFacilityInspection:
                    pageInfo.CustomPageSize = refFoodFacilityList.PageSize;
                    queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                    IList<LicenseModel4WS> license4FoodFacility = SearchByFoodFacility(queryFormat);
                    license4FoodFacility = PaginationUtil.MergeDataSource<IList<LicenseModel4WS>>(refFoodFacilityList.DataSource, license4FoodFacility, pageInfo);
                    htResultCollection = UpdateOrCreateHashTable(htResultCollection, RefResultType.FoodFacility, license4FoodFacility);
                    obj = refFoodFacilitySearchForm.GetSearchCondition();

                    break;
                case GeneralInformationSearchType.Search4CertifiedBusiness:
                    pageInfo.CustomPageSize = certBusinessList.PageSize;
                    queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                    DataTable certBusinessModels = SearchByCertBusiness(queryFormat);
                    certBusinessModels = PaginationUtil.MergeDataSource<DataTable>(certBusinessList.DataSource, certBusinessModels, pageInfo);
                    htResultCollection = UpdateOrCreateHashTable(htResultCollection, RefResultType.CertifiedBusiness, certBusinessModels);
                    obj = certifiedBusinessSearchForm.GetSearchCondition();

                    break;
            }

            //4. Save search form infor.
            SaveProviderQueryInfo(educationSearchType, obj, htResultCollection, null, null);

            //5. Bind education list to UI.
            DisplayResultList(educationSearchType, htResultCollection, null, null, isSearch);

            //6.Scroll into UI view.
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GoToPageResult", "scrollIntoView('PageResult');", true);
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the certBusinessList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void CertBusinessList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetCertBusinessWithExperienceByQueryFormat);
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the refProviderList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void RefProviderList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetProviderByQueryFormat);
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the refLicenseeList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void RefLicenseeList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetLicenseeByQueryFormat, LicenseeExportFormat);
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the refFoodFacilityList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void RefFoodFacilityList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetFoodFacilityByQueryFormat, LicenseeExportFormat);
        }

        /// <summary>
        /// Get the provider by query format.
        /// </summary>
        /// <param name="queryFormat">The QueryFormat model.</param>
        /// <param name="obj">The out object.</param>
        /// <returns>download result model that contains provider list</returns>
        private DownloadResultModel GetProviderByQueryFormat(QueryFormat queryFormat, out object obj)
        {
            EducationSearchModel educationSearchModel = educationRelationSearchForm.GetProviderModel();
            obj = educationSearchModel;
            IProviderBll providerBll = ObjectFactory.GetObject<IProviderBll>();
            ProviderModel4WS[] providerModels = providerBll.GetProviderList(educationSearchModel.providerModel4WS, queryFormat);
            DataTable result = refProviderList.ConvertProviderListToDataTable(providerModels);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = 0;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get the provider by query format.
        /// </summary>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>download result model that contains provider list</returns>
        private DownloadResultModel GetProviderByQueryFormat(QueryFormat queryFormat)
        {
            object obj = null;
            return GetProviderByQueryFormat(queryFormat, out obj);
        }

        /// <summary>
        /// Get the license by query format.
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <returns>download result model that contains license data table</returns>
        private DownloadResultModel GetLicenseeByQueryFormat(QueryFormat queryFormat)
        {
            DataTable result = refLicenseeList.ConvertLicenseeListToDataTable(SearchByLicensee(queryFormat));

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = 0;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get the food facility by query format.
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <returns>download result model that contains food facility data table</returns>
        private DownloadResultModel GetFoodFacilityByQueryFormat(QueryFormat queryFormat)
        {
            DataTable result = refLicenseeList.ConvertLicenseeListToDataTable(SearchByFoodFacility(queryFormat));

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = 0;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get the certified business by query format, contain the experience record.
        /// </summary>
        /// <param name="queryFormat">The QueryFormat model.</param>
        /// <returns>download result model that contains certified business list</returns>
        private DownloadResultModel GetCertBusinessWithExperienceByQueryFormat(QueryFormat queryFormat)
        {
            DataTable result = certifiedBusinessSearchForm.SearchLicensee(queryFormat, true);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = 0;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get license field's format mapping
        /// </summary>
        /// <param name="dataRow">The grid view row</param>
        /// <param name="visibleColumns">The visible columns.</param>
        /// <returns>license field's format mapping dictionary</returns>
        private Dictionary<string, string> LicenseeExportFormat(DataRow dataRow, List<string> visibleColumns)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string colTypeFlag = ColumnConstant.RefLicenseProfessional.TypeFlag.ToString();
            string colAddress = ColumnConstant.RefLicenseProfessional.FullAddress.ToString();

            if (dataRow != null)
            {
                if (visibleColumns.Contains(colTypeFlag))
                {
                    result.Add(colTypeFlag, DropDownListBindUtil.GetTypeFlagTextByValue(dataRow[colTypeFlag].ToString()));
                }

                if (visibleColumns.Contains(colAddress))
                {
                    LicenseModel4WS license = new LicenseModel4WS();
                    license.countryCode = dataRow[ColumnConstant.RefLicenseProfessional.CountryCode.ToString()].ToString();
                    license.address1 = dataRow[ColumnConstant.RefLicenseProfessional.Address1.ToString()].ToString();
                    license.address2 = dataRow[ColumnConstant.RefLicenseProfessional.Address2.ToString()].ToString();
                    license.address3 = dataRow[ColumnConstant.RefLicenseProfessional.Address3.ToString()].ToString();
                    license.resState = dataRow[ColumnConstant.RefLicenseProfessional.ResState.ToString()].ToString();
                    license.state = dataRow[ColumnConstant.RefLicenseProfessional.State.ToString()].ToString();
                    license.city = dataRow[ColumnConstant.RefLicenseProfessional.City.ToString()].ToString();
                    license.zip = dataRow[ColumnConstant.RefLicenseProfessional.Zip.ToString()].ToString();

                    result.Add(colAddress, LicenseUtil.GetAddressDetail4License(license));
                }
            }

            return result;
        }

        /// <summary>
        /// get the education and examination count
        /// </summary>
        /// <param name="htResultCollection">The result collection.</param>
        /// <returns>The education and examination count.</returns>
        private int GetEduAndExamCount(Hashtable htResultCollection)
        {
            int count = 0;

            if (htResultCollection.Contains(RefResultType.Education))
            {
                DataTable dtEducation = htResultCollection[RefResultType.Education] as DataTable;
                count += dtEducation != null ? dtEducation.Rows.Count : 0;
            }

            if (htResultCollection.Contains(RefResultType.ContEdu))
            {
                DataTable dtContEdu = htResultCollection[RefResultType.ContEdu] as DataTable;
                count += dtContEdu != null ? dtContEdu.Rows.Count : 0;
            }

            if (htResultCollection.Contains(RefResultType.Examination))
            {
                DataTable dtExam = htResultCollection[RefResultType.Examination] as DataTable;
                count += dtExam != null ? dtExam.Rows.Count : 0;
            }

            return count;
        }

        #region Navigate To detail page

        /// <summary>
        /// Navigate To Provider detail page.
        /// </summary>
        /// <param name="htResultCollection">The result collection.</param>
        private void RedirectToProviderDetail(Hashtable htResultCollection)
        {
            if (htResultCollection.Contains(RefResultType.Provider))
            {
                DataTable dt = htResultCollection[RefResultType.Provider] as DataTable;
                string providerPkNbr = dt.Rows[0]["ProviderPKNbr"].ToString();
                string url = FileUtil.AppendApplicationRoot(string.Format("GeneralProperty/ProviderDetail.aspx?providerPKNbr={0}", providerPkNbr));

                Response.Redirect(url);
            }
        }

        /// <summary>
        ///  Navigate To Education/Examinations  detail page.
        /// </summary>
        /// <param name="htResultCollection">The result collection.</param>
        private void RedirectToEduAndExamDetail(Hashtable htResultCollection)
        {
            int educationRecordCount = 0;
            int contEduRecordCount = 0;
            int examRecordCount = 0;
            DataTable dtEducation = null;
            DataTable dtContEdu = null;
            DataTable dtExam = null;

            if (htResultCollection.Contains(RefResultType.Education))
            {
                dtEducation = htResultCollection[RefResultType.Education] as DataTable;
                educationRecordCount = dtEducation != null ? dtEducation.Rows.Count : 0;
            }

            if (htResultCollection.Contains(RefResultType.ContEdu))
            {
                dtContEdu = htResultCollection[RefResultType.ContEdu] as DataTable;
                contEduRecordCount = dtContEdu != null ? dtContEdu.Rows.Count : 0;
            }

            if (htResultCollection.Contains(RefResultType.Examination))
            {
                dtExam = htResultCollection[RefResultType.Examination] as DataTable;
                examRecordCount = dtExam != null ? dtExam.Rows.Count : 0;
            }

            bool isNeedRedirect = educationRecordCount + contEduRecordCount + examRecordCount == 1;

            //if the search result only has one,then redirect to the detail info
            if (isNeedRedirect)
            {
                string url = string.Empty;
                if (educationRecordCount == 1)
                {
                    string educationNumber = dtEducation.Rows[0]["Number"].ToString();
                    url = FileUtil.AppendApplicationRoot(string.Format("GeneralProperty/EducationDetail.aspx?refEducationNbr={0}", educationNumber));
                }

                if (contEduRecordCount == 1)
                {
                    string contEduNbr = dtContEdu.Rows[0]["ContEduNbr"].ToString();
                    url = FileUtil.AppendApplicationRoot(string.Format("GeneralProperty/ContinuingEducationDetail.aspx?refContEduNbr={0}", contEduNbr));
                }

                if (examRecordCount == 1)
                {
                    string refExamNbr = dtExam.Rows[0]["refExamNbr"].ToString();
                    url = FileUtil.AppendApplicationRoot(string.Format("GeneralProperty/ExaminationDetail.aspx?refExamNbr={0}", refExamNbr));
                }

                if (!string.IsNullOrEmpty(url))
                {
                    Response.Redirect(url);
                }
            }
            else
            {
                divHrforResult.Visible = true;
            }
        }

        /// <summary>
        /// Navigate To License detail page.
        /// </summary>
        /// <param name="htResultCollection">The result collection.</param>
        private void RedirectToLicenseDetail(Hashtable htResultCollection)
        {
            if (htResultCollection.Contains(RefResultType.Licensee))
            {
                IList<LicenseModel4WS> licenseViewList = (IList<LicenseModel4WS>)htResultCollection[RefResultType.Licensee];
                LicenseModel4WS license = licenseViewList[0];
                string licenseNumber = Convert.ToString(license.stateLicense);
                string licenseType = Convert.ToString(license.licenseType);
                string url = FileUtil.AppendApplicationRoot(string.Format("GeneralProperty/LicenseeDetail.aspx?LicenseeNumber={0}&LicenseeType={1}", licenseNumber, licenseType));
                
                Response.Redirect(url);
            }
        }

        /// <summary>
        ///  Navigate To FoodFacilityInspection detail page.
        /// </summary>
        /// <param name="htResultCollection">The result collection.</param>
        private void RedirectToFoodFacilityInspectionDetail(Hashtable htResultCollection)
        {
            if (htResultCollection.Contains(RefResultType.FoodFacility))
            {
                IList<LicenseModel4WS> licenseeList = (IList<LicenseModel4WS>)htResultCollection[RefResultType.FoodFacility];
                LicenseModel4WS license = licenseeList[0];
                string licenseNumber = Convert.ToString(license.stateLicense);
                string licenseType = Convert.ToString(license.licenseType);
                string url = FileUtil.AppendApplicationRoot(string.Format("GeneralProperty/FoodFacilityInspectionDetail.aspx?LicenseeNumber={0}&LicenseeType={1}", licenseNumber, licenseType));
                
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Navigate To CertifiedBusiness detail page.
        /// </summary>
        /// <param name="htResultCollection">The result collection.</param>
        private void RedirectToCertifiedBusinessDetail(Hashtable htResultCollection)
        {
            if (htResultCollection.Contains(RefResultType.CertifiedBusiness))
            {
                DataTable dt = (DataTable)htResultCollection[RefResultType.CertifiedBusiness];
                string businessName = dt.Rows[0]["businessName"] != null ? dt.Rows[0]["businessName"].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(businessName))
                {
                    string[] arrArgs = new string[4];
                    arrArgs[0] = dt.Rows[0]["seqNumber"] != null ? dt.Rows[0]["seqNumber"].ToString() : string.Empty;
                    arrArgs[1] = dt.Rows[0]["stateLicense"] != null ? dt.Rows[0]["stateLicense"].ToString() : string.Empty;
                    arrArgs[2] = dt.Rows[0]["licenseType"] != null ? dt.Rows[0]["licenseType"].ToString() : string.Empty;
                    arrArgs[3] = dt.Rows[0]["agencyCode"] != null ? dt.Rows[0]["agencyCode"].ToString() : string.Empty;
                    string url = FileUtil.AppendApplicationRoot(string.Format(
                        "GeneralProperty/CertifiedBusinessDetail.aspx?licenseSeqNbr={0}&stateLicense={1}&licenseType={2}&{3}={4}",
                        arrArgs[0],
                        arrArgs[1],
                        arrArgs[2],
                        UrlConstant.AgencyCode,
                        arrArgs[3]));

                    Response.Redirect(url);
                }
            }
        }

        #endregion

        #endregion Private Methods
    }
}
