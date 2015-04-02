#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CertifiedBusinessSearchForm.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CertifiedBusinessSearchForm.ascx.cs 187097 2010-12-21 09:43:43Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
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
    /// search certified business.
    /// </summary>
    public partial class CertifiedBusinessSearchForm : FormDesignerBaseControl
    {
        #region Fields

        /// <summary>
        /// The format of the NIGP code display field
        /// </summary>
        private const string NIGP_CODE_DISPLAY_FIELD_FORMAT = "{0} - {1}";

        /// <summary>
        /// NIGP ethnicity search constant string
        /// </summary>
        private const string NIGP_ETHNICITY_SEARCH = "ethnicity_search";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CertifiedBusinessSearchForm class.
        /// </summary>
        public CertifiedBusinessSearchForm()
            : base(GviewID.SearchForCertifiedBusiness)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the zip code's GridView data source
        /// </summary>
        public DataTable ZipCodeDataSource
        {
            get
            {
                if (ViewState["ZipCodeDataSource"] != null)
                {
                    return (DataTable)ViewState["ZipCodeDataSource"];
                }

                return null;
            }

            set
            {
                ViewState["ZipCodeDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Permission
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = "SEARCH_FOR_CERTIFIED_BUSINESS";

                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Gets Certified Items to Enable Ethnicity
        /// </summary>
        protected string CertifiedItems2EnableEthnicity
        {
            get
            {
                IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
                IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CERT_BUSINESS_CERTIFICATION_CATEGORY, false);
                string result = string.Empty;

                // Append the stdItems' key to the result whitch item's value is equal 'yes'
                foreach (ItemValue item in stdItems)
                {
                    string[] strArray = item.Value.ToString().Split(ACAConstant.EQUAL_MARK.ToCharArray());

                    if ((strArray.Length != 2) || (strArray.Length == 2 && strArray[0].Trim().Equals(NIGP_ETHNICITY_SEARCH) && !ValidationUtil.IsNo(strArray[1].Trim())))
                    {
                        result += item.Key + ACAConstant.SPLIT_CHAR4URL1;
                    }
                }

                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Remove(result.Length - 1);
                }

                return result;
            }
        }
        #endregion Properties

        #region Methods

        /// <summary>
        /// Initial the search form by certified business.
        /// </summary>
        public void InitCertifiedBusinessForm()
        {
            txtNIGPKeyword.Text = string.Empty;
            DropDownListBindUtil.BindNigpType(ddlNIGPType);
            DropDownListBindUtil.BindLargestContractExperience(ddlLargestContractExperience);
            DropDownListBindUtil.BindLargestContractValue(ddlLargestContractValue);
            DropDownListBindUtil.BindCertifiedCategory(cbListCertifiedAs);
            DropDownListBindUtil.BindCertifiedBusinessEthnicity(cbListOwnerEthnicity);
            txtZipCode.Text = string.Empty;
            hdZipCode.Value = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtBusinessNamedba.Text = string.Empty;
            txtCertificationDateFrom.SetValue(string.Empty);
            txtCertificationDateTo.SetValue(string.Empty);
            listboxLocation.Items.Clear();
            hdLocation.Value = string.Empty;
            listboxCommodityClass.Items.Clear();
            hdCommodityClassCode.Value = string.Empty;
        }

        /// <summary>
        /// Show the search form info.
        /// </summary>
        /// <param name="searchLicense">The search license information.</param>
        public void ShowLicenseInfo(SearchLicenseModel searchLicense)
        {
            if (searchLicense != null)
            {
                ddlNIGPType.SelectedValue = searchLicense.nigpType;
                txtNIGPKeyword.Text = searchLicense.nigpKeyword;

                string[] searchNigpCodeList = GetFiveDigitNigpCodeAndDesc(searchLicense.nigpCodes);
                BindListBoxItems(listboxCommodityClass, searchNigpCodeList);

                ddlLargestContractExperience.SelectedValue = searchLicense.maximumContractAmountOperator;
                ddlLargestContractValue.SelectedValue = searchLicense.maximumContractAmount;
                BindCheckBoxListItems(cbListCertifiedAs, searchLicense.certications);
                BindCheckBoxListItems(cbListOwnerEthnicity, searchLicense.ethnicities);
                BindListBoxItems(listboxLocation, searchLicense.locations);
                txtCompanyName.Text = searchLicense.businessName;
                txtDescription.Text = searchLicense.comment.StartsWith("%") ? searchLicense.comment.Substring(1) : searchLicense.comment;
                txtBusinessNamedba.Text = searchLicense.busName2;
                txtCertificationDateFrom.Text = I18nDateTimeUtil.FormatToDateStringForUI(searchLicense.certificationDateFrom);
                txtCertificationDateTo.Text = I18nDateTimeUtil.FormatToDateStringForUI(searchLicense.certificationDateTo);

                // set the search zips.
                if (searchLicense.searchZips != null && searchLicense.searchZips.Length > 0)
                {
                    string zips = string.Empty;

                    foreach (string zip in searchLicense.searchZips)
                    {
                        zips += zip + ACAConstant.COMMA_CHAR;
                    }

                    txtZipCode.Text = zips.Substring(0, zips.Length - 1);
                }
            }
        }

        /// <summary>
        /// Get the license list according to user input.
        /// </summary>
        /// <param name="queryFormat">The QueryFormat model.</param>
        /// <param name="needExperience">Need experience or not.</param>
        /// <returns>Return the LicenseModel list.</returns>
        public DataTable SearchLicensee(QueryFormat queryFormat, bool needExperience)
        {
            SearchLicenseModel searchLicense = GetSearchCondition();
            DataTable resultList = LicenseUtil.CreateDataTable4CertBusiness();

            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
            LicenseModel[] refLicenseList = licenseBll.GetLicenseProfessionals(searchLicense, needExperience, queryFormat);

            if (refLicenseList != null && refLicenseList.Length > 0)
            {
                resultList = LicenseUtil.SetDataSource2DataTable4BindList(refLicenseList, needExperience);
            }

            return resultList;
        }

        /// <summary>
        /// Get the search condition.
        /// </summary>
        /// <returns>Return the SearchLicenseModel.</returns>
        public SearchLicenseModel GetSearchCondition()
        {
            SearchLicenseModel searchLicense = new SearchLicenseModel();

            searchLicense.nigpType = ddlNIGPType.SelectedValue;
            searchLicense.nigpKeyword = txtNIGPKeyword.Text.Trim();

            // set the nigp class code
            if (!string.IsNullOrEmpty(hdCommodityClassCode.Value))
            {
                List<string> nigpList = new List<string>();
                string[] nigpCodes = hdCommodityClassCode.Value.Split(new char[] { ACAConstant.SPLIT_CHAR4URL1 }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (string nigpCode in nigpCodes)
                {
                    if (nigpCode.Length >= 5)
                    {
                        nigpList.Add(nigpCode.Substring(0, 5));
                    }
                }
                
                searchLicense.nigpCodes = nigpList.ToArray();
            }

            if (!string.IsNullOrEmpty(ddlLargestContractValue.SelectedValue) && !string.IsNullOrEmpty(ddlLargestContractExperience.SelectedValue))
            {
                // Only comparison operation and value both contain valid value, assign their value to model.
                searchLicense.maximumContractAmountOperator = ddlLargestContractExperience.SelectedValue;
                searchLicense.maximumContractAmount = ddlLargestContractValue.SelectedValue;
            }

            searchLicense.certications = GetCheckedValues(cbListCertifiedAs);
            searchLicense.ethnicities = GetCheckedValues(cbListOwnerEthnicity);

            if (!string.IsNullOrEmpty(hdLocation.Value))
            {
                searchLicense.locations = hdLocation.Value.Split(new char[] { ACAConstant.COMMA_CHAR }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (!string.IsNullOrEmpty(hdZipCode.Value))
            {
                searchLicense.searchZips = hdZipCode.Value.Split(new char[] { ACAConstant.COMMA_CHAR }, StringSplitOptions.RemoveEmptyEntries);
            }

            searchLicense.businessName = txtCompanyName.Text.Trim();
            searchLicense.serviceProviderCode = ACAConstant.AgencyCode;
            searchLicense.acaPermission = ACAConstant.COMMON_Y;

            searchLicense.busName2 = txtBusinessNamedba.Text;
            if (!string.IsNullOrEmpty(txtCertificationDateFrom.GetValue()))
            {
                searchLicense.certificationDateFrom = I18nDateTimeUtil.ParseFromUI(txtCertificationDateFrom.GetValue());
            }

            if (!string.IsNullOrEmpty(txtCertificationDateTo.GetValue()))
            {
                searchLicense.certificationDateTo = I18nDateTimeUtil.ParseFromUI(txtCertificationDateTo.GetValue());
            }

            // Actually it'd append a "%" to the end of comment in AA SQL, so setting a "%" ahead is enough.
            searchLicense.comment = string.IsNullOrEmpty(txtDescription.Text.Trim()) ? string.Empty : "%" + txtDescription.Text.Trim();

            // Filter record which status is enabled.
            searchLicense.auditStatus = ACAConstant.VALID_STATUS;

            return searchLicense;
        }

        /// <summary>
        /// is Check The Date from and Date to
        /// </summary>
        /// <returns>Date from less or equal to Date to return true, or return false</returns>
        public bool CheckDate()
        {
            if (txtCertificationDateFrom.Visible && txtCertificationDateTo.Visible 
                && txtCertificationDateFrom.IsLaterThan(txtCertificationDateTo))
            {
                string msg = GetTextByKey("per_permitList_msg_date_start_end");
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, msg);
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoMessageBar", "scrollIntoView('messageSpan');", true);

                return false;
            }

            return true;
        }

        /// <summary>
        /// It is Comparison Operator
        /// </summary>
        /// <param name="selectedIndex">The selected index.</param>
        /// <returns>true, if it is comparison operator; otherwise return false.</returns>
        public bool IsComparisonOperator(int selectedIndex)
        {
            if ((GeneralInformationSearchType)selectedIndex == GeneralInformationSearchType.Search4CertifiedBusiness)
            {
                string comparisonOperator = ddlLargestContractExperience.SelectedValue;
                if (!string.IsNullOrEmpty(comparisonOperator))
                {
                    ArrayList opList = new ArrayList();
                    opList.Add("<");
                    opList.Add(">");
                    opList.Add("=");
                    opList.Add("<=");
                    opList.Add(">=");
                    opList.Add("!=");
                    opList.Add("<>");

                    return opList.Contains(comparisonOperator);
                }
            }

            return true;
        }

        /// <summary>
        /// Raises the initial event
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // set the validation
            ControlBuildHelper.AddValidationForStandardFields(GviewID.SearchForCertifiedBusiness, ModuleName, Permission, Controls);

            // append the extend control to exists control
            if (!AppSession.IsAdmin)
            {
                string actionAdd = LabelUtil.GetTextByKey("aca_certbusiness_label_drilldown_add", ModuleName);
                string actionRemove = LabelUtil.GetTextByKey("aca_certbusiness_label_drilldown_remove", ModuleName);
                string standardZipCode = LabelUtil.GetTextByKey("aca_certbusiness_label_standardzipcode", ModuleName);
                string addBtnFormat = "<a id='{0}' class=\"ACA_Header_LinkButton NotShowLoading\" onclick=\"{1}('{2}')\" href=\"javascript:void(0)\">{3}</a>";
                string removeBtnFormat = "<a id='{0}' class=\"ACA_Header_LinkButton NotShowLoading\" onclick=\"{1}()\" href=\"javascript:void(0)\">{2}</a>";

                StringBuilder additionBtnHtml = new StringBuilder();
                additionBtnHtml.AppendFormat(addBtnFormat, "lnkAddCommodity", "addCommodityClass", "lnkAddCommodity", actionAdd);
                additionBtnHtml.AppendFormat(removeBtnFormat, "lnkRemoveCommodity", "removeCommodityClass", actionRemove);
                listboxCommodityClass.ExtendControlHtml = additionBtnHtml.ToString();
                additionBtnHtml.Clear();

                additionBtnHtml.AppendFormat(addBtnFormat, "lnkAddLocation", "addLocation", "lnkAddLocation", actionAdd);
                additionBtnHtml.AppendFormat(removeBtnFormat, "lnkRemoveLocation", "removeLocation", actionRemove);
                listboxLocation.ExtendControlHtml = additionBtnHtml.ToString();

                txtZipCode.ExtendControlHtml = string.Format(addBtnFormat, "lnkAddZipCode", "addZipCode", "lnkAddZipCode", standardZipCode);
            }           
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Anti Injected XSS Script
            IList<string> contentArray = new List<string>();
            contentArray.Add(this.hdCommodityClassCode.Value);
            contentArray.Add(this.hdLocation.Value);
            contentArray.Add(this.hdZipCode.Value);

            if (ScriptFilter.IsUnSafeData(contentArray))
            {
                throw new InvalidDataException(this.GetTextByKey("aca_unsafe_data_warning_msg"));
            }

            InitFormDesignerPlaceHolder(phContent);

            if (Page.IsPostBack)
            {
                // set the control's value from previous value that reset in postback.
                string[] commodityClassCodes = hdCommodityClassCode.Value.Split(new char[] { ACAConstant.SPLIT_CHAR4URL1 }, StringSplitOptions.RemoveEmptyEntries);
                BindListBoxItems(listboxCommodityClass, commodityClassCodes);

                string[] locations = hdLocation.Value.Split(new char[] { ACAConstant.COMMA_CHAR }, StringSplitOptions.RemoveEmptyEntries);
                BindListBoxItems(listboxLocation, locations);

                if (!string.IsNullOrEmpty(hdZipCode.Value))
                {
                    txtZipCode.Text = hdZipCode.Value;
                }
            }

            ddlLargestContractValue.Enabled = !string.IsNullOrEmpty(ddlLargestContractExperience.SelectedValue);
        }

        /// <summary>
        /// Set the ListBox's item.
        /// </summary>
        /// <param name="listbox">The ListBox control.</param>
        /// <param name="items">The item value list.</param>
        private void BindListBoxItems(ListBox listbox, string[] items)
        {
            listbox.Items.Clear();

            if (items != null && items.Length > 0)
            {
                foreach (string item in items)
                {
                    listbox.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Bind the CheckBoxList items.
        /// </summary>
        /// <param name="checkboxlist">The CheckBoxList control.</param>
        /// <param name="items">The item value list.</param>
        private void BindCheckBoxListItems(CheckBoxList checkboxlist, string[] items)
        {
            // uncheck list item.
            foreach (ListItem listItem in checkboxlist.Items)
            {
                listItem.Selected = false;
            }

            // set the select item
            foreach (string item in items)
            {
                foreach (ListItem listItem in checkboxlist.Items)
                {
                    if (listItem.Value == item)
                    {
                        listItem.Selected = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get the checked value in CheckBoxList.
        /// </summary>
        /// <param name="checkboxlist">The CheckBoxList control.</param>
        /// <returns>Return the value list which checked.</returns>
        private string[] GetCheckedValues(CheckBoxList checkboxlist)
        {
            List<string> result = new List<string>();

            foreach (ListItem item in checkboxlist.Items)
            {
                if (item.Selected)
                {
                    result.Add(item.Value);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get five digit NIGP code and description list by the five digit code list.
        /// </summary>
        /// <param name="fiveDigitNigpCodes">The five digit NIGP code list.</param>
        /// <returns>Return the five digit NIGP code and description list.</returns>
        private string[] GetFiveDigitNigpCodeAndDesc(string[] fiveDigitNigpCodes)
        {
            List<string> resultList = new List<string>();

            if (fiveDigitNigpCodes != null && fiveDigitNigpCodes.Length > 0)
            {
                IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
                IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CERT_BUSINESS_NIGP_SUBCLASS, false);
            
                // loop get the value from standard choice
                foreach (string code in fiveDigitNigpCodes)
                {
                    foreach (ItemValue item in stdItems)
                    {
                        if (item.Key.Trim().StartsWith(code))
                        {
                            resultList.Add(item.Key.Trim());
                            break;
                        }
                    }
                }
            }

            return resultList.ToArray();
        }
        
        #endregion Methods
    }
}
