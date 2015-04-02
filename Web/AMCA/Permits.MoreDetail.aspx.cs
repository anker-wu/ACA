/**
* <pre>
* 
*  Accela Citizen Access
*  File: PermitsMoreDetail.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2013
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: Permits.MoreDetail.aspx.cs 125777 2009-04-01 17:42:18Z dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  08/04/2008           Dave Brewster           Added new form for new Permits.View link "More Permit Details".
*  10-09-2008           DWB                     2008 Mobile ACA 6.7.0 interface redesign
*  04/01/2009           Dave Brewster           Modiifed to display AltID instetad of Cap ID, CAP type alias.
*  10-10-2009           Dave Brewster           Added new breadcrumb logic.
* </pre>
*/
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

/// <summary>
/// Permit's View
/// </summary>
public partial class PermitsMoreDetail : AccelaPage
{
    public Permit ThisPermit = new Permit();
    public StringBuilder OutputLinks = new StringBuilder();
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder OwnerDetails = new StringBuilder();
    public StringBuilder LicensedProfessional = new StringBuilder();


    public string Description = string.Empty;
    public string ThisPermitDate = string.Empty;
    public string DisplayNumber = string.Empty;
    public string DisplayType = string.Empty;

    private string Agency = string.Empty;
    private string PermitNo = string.Empty;
    private string PermitType = string.Empty;
    private string SearchBy = string.Empty;
    private string SearchType = string.Empty;
    private string SearchValue = string.Empty;
    private string ViewPermitPageNo = string.Empty;  // ResultPage for "View Permits" breadcrumb link.
    private string InspectionsPageNo = string.Empty; // ResultPage for "View Permits > Inspections" Breadcrumb link
    private string Mode = string.Empty;
    private string AltID = string.Empty;
    private string LicenseEdit_LicensePro_label_homePhone = string.Empty;
    private string LicenseEdit_LicensePro_label_mobile = string.Empty;
    private string LicenseEdit_LicensePro_label_fax = string.Empty;
    private string per_owneredit_phone = string.Empty;
    private string per_owneredit_fax = string.Empty;
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // page validate
        ValidationChecks("Permits.MoreDetail.aspx");

        PermitNo = GetFieldValue("Id", false);
        AltID = GetFieldValue("AltID", false);
        PermitType = GetFieldValue("PermitType", false);
        SearchBy = GetFieldValue("SearchBy", false);
        SearchType = GetFieldValue("SearchType", false);
        SearchValue = GetFieldValue("SearchValue", false);
        ViewPermitPageNo = GetFieldValue("ViewPermitPageNo", false);
        Agency = GetFieldValue("Agency", false);
        
        Mode = GetFieldValue("Mode", false);

        // string mycollection_managepage_label_name = StripHTMLFromLabelText(LabelUtil.GetTextByKey("mycollection_collectionmanagement_collectionname",ModuleName), "Collections");
        LicenseEdit_LicensePro_label_homePhone = StripHTMLFromLabelText(LabelUtil.GetTextByKey("LicenseEdit_LicensePro_label_homePhone", ModuleName), "Phone 1: ");
        LicenseEdit_LicensePro_label_mobile = StripHTMLFromLabelText(LabelUtil.GetTextByKey("LicenseEdit_LicensePro_label_mobile", ModuleName), "Phone 2: ");
        LicenseEdit_LicensePro_label_fax = StripHTMLFromLabelText(LabelUtil.GetTextByKey("LicenseEdit_LicensePro_label_fax", ModuleName), "Fax: ");
        per_owneredit_phone = StripHTMLFromLabelText(LabelUtil.GetTextByKey("per_owneredit_phone", ModuleName), "Phone: ");
        per_owneredit_fax = StripHTMLFromLabelText(LabelUtil.GetTextByKey("per_owneredit_fax", ModuleName), "Fax: ");

         // gets and retrieves Permits for view
        ThisPermit.Number = PermitNo;
        ThisPermit.Type = PermitType;
        ThisPermit.Agency = Agency;
        ThisPermit = MyProxy.PermitRetrieve(ThisPermit);
        ThisPermit.Application = "";
        if (ThisPermit == null)
            this.ThisPermit = new Permit();
        CapModel4WS myCap = AppSession.GetCapModelFromSession(ModuleName);

        if (myCap.applicantModel != null && myCap.applicantModel.people != null)
        {
            ThisPermit.Application = GetContactFullName(myCap.applicantModel.people);
        }

        Dictionary<string, UserRolePrivilegeModel> sectionPermissions = this.GetSectionPermissions(ModuleName);
        bool isLPVisible = this.GetSectionVisibility(CapDetailSectionType.LICENSED_PROFESSIONAL.ToString(), sectionPermissions);
        bool isOwnerVisible = this.GetSectionVisibility(CapDetailSectionType.OWNER.ToString(), sectionPermissions);

		//standard choiceutil is "N" ,invisible owner detail infomation.
        if (StandardChoiceUtil.IsDisplayOwnerSection())
        {
            if (isOwnerVisible && myCap.ownerModel != null)
            {
                OwnerDetails.Append(FormatOwner4Detail(myCap.ownerModel, ModuleName));
            }
        }

        if (isLPVisible && myCap.licenseProfessionalList != null)
        {
            ReOrderLicenseList(myCap);
            OwnerDetails.Append(FormatLicenseModel4Basic(TempModelConvert.ConvertToLicenseProfessionalModelList(myCap.licenseProfessionalList)));
        }

        if (MyProxy.OnErrorReturn)
        {  // Proxy Exception 
            ErrorMessage.Append(MyProxy.ExceptionMessage);
        }
        if (ThisPermit.Alias.ToString() == string.Empty)
        {
            DisplayNumber = ThisPermit.Number;
        }
        else
        {
            DisplayNumber = ThisPermit.Alias;
        }
        if (ThisPermit.TypeAlias.ToString() == string.Empty)
        {
            DisplayType = ThisPermit.Type.ToString();
        }
        else
        {
            DisplayType = ThisPermit.TypeAlias.ToString();
        }

        DateTime ThisDate;
        if (DateTime.TryParse(ThisPermit.Date, out ThisDate))
        {
            ThisPermitDate = ThisDate.ToString("MM/dd/yy hh:mm tt");
        }
        else
        {
            ThisPermitDate = ThisPermit.Date;
        }

        // Links

        string CollectionModuleName = string.Empty;
        string SearchMode = Mode;
        string ResultPage = ViewPermitPageNo;
        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&SearchBy=" + GetFieldValue("SearchBy", false));
        sbWork.Append("&SearchType=" + GetFieldValue("SearchType", false));
        sbWork.Append("&Mode=" + Mode.ToString());
        sbWork.Append("&Module=" + ModuleName.ToString());
        sbWork.Append("&Agency=" + GetFieldValue("Agency", false));
        sbWork.Append("&Id=" + GetFieldValue("Id", false));
        sbWork.Append("&PermitType=" + PermitType);
        sbWork.Append("&AltID=" + GetFieldValue("AltID", false));
        sbWork.Append("&ViewPermitPageNo" + GetFieldValue("ViewPermitPageNo", false));
        if (SearchMode == "MyCollections")
        {
            sbWork.Append("&CollectionId=" + GetFieldValue("CollectionId", false));
            sbWork.Append("&CollectionModuleName=" + GetFieldValue("CollectionModuleName",false));
        }
        if (ThisPermit.Desc.Length > 100)
        {
            Description = ThisPermit.Desc.Substring(0, 95) + HTML.PresentLink("View.Details.aspx?State=" + State + sbWork.ToString() + "&Type=Permit", "...");
        }
        else
        {
            Description = ThisPermit.Desc;
        }
        iPhonePageTitle = DisplayNumber;
        if (isiPhone == true)
        {
            if (iPhoneTitleHasBeenClipped == false)
            {
                DisplayNumber = string.Empty;
            }
        }
        else
        {

            DisplayNumber = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div><hr />"; 
        }


        Breadcrumbs = BreadCrumbHelper("Permits.MoreDetail.aspx", sbWork, "More Permit Details", breadCrumbIndex, isElipseLink, false, false, true);
        // Breadcrumbs.Append("<br>");
        OutputLinks = new StringBuilder();
        OutputLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
    }
    /// <summary>
    /// Get Field value from Form, Request, or QueryString.
    /// </summary>
    /// <param name="FieldName"> Field name </param>
    /// <param name="IsRequired"> Indcates if the user must enterer data into the field or not</param>
    /// <returns>String</returns>
    // DWB - 07-18-2008 - Added new funciton.
    private string GetFieldValue(string FieldName, bool IsRequired)
    {
        string TheValue = string.Empty;
        try
        {
            TheValue = (Request.QueryString[FieldName] != null)
                   ? Request.QueryString[FieldName] : ((Request[FieldName] != null)
                   ? Request.Form[FieldName].ToString() : string.Empty);
        }
        catch
        {
            TheValue = string.Empty;
        }
        if (IsRequired == true && TheValue.ToString() == "")
        {
            // IsRequiredDataValid = false;
        }
        return TheValue;
    }
    /// <summary>
    /// Construct contact full name which consist of first name, middle name and last name
    /// </summary>
    /// <param name="people">the PeopleModel4WS instance</param>
    /// <returns>the contact full name</returns>
    private string GetContactFullName(PeopleModel4WS people)
    {
        if (people == null)
        {
            return string.Empty;
        }

        string[] fullName = {people.firstName, people.middleName, people.lastName};
        string contactFullName = DataUtil.ConcatStringWithSplitChar(fullName,ACAConstant.BLANK);

        return contactFullName;
    }

    #region code copied from ACA for the 7.1 release cycle
    /// <summary>
    /// select one license to change order to top when primary license is "Y".
    /// </summary>
    private void ReOrderLicenseList(CapModel4WS CapModel)
    {
        if (CapModel.licenseProfessionalList != null && CapModel.licenseProfessionalList.Length > 0)
        {
            LicenseProfessionalModel changeLocationLS;
            for (int i = 0; i < CapModel.licenseProfessionalList.Length; i++)
            {
                if (CapModel.licenseProfessionalList[i].printFlag == ACAConstant.COMMON_Y)
                {
                    changeLocationLS = TempModelConvert.ConvertToLicenseProfessionalModel(CapModel.licenseProfessionalList[i]);
                    CapModel.licenseProfessionalList[i] = CapModel.licenseProfessionalList[0];
                    CapModel.licenseProfessionalList[0] = TempModelConvert.ConvertToLicenseProfessionalModel4WS(changeLocationLS);
                    break;
                }
            }
        }
    }
    /// <summary>
    /// Display full name and address in cap detail page.
    /// </summary>
    /// <param name="owner">Owner model come from spare form.</param>
    /// <param name="moduleName">current module name</param>
    /// <param name="lines">the account of display lines</param>
    /// <returns>owner detail information display in cap detail</returns>
    public string FormatOwner4Detail(RefOwnerModel owner, string moduleName)
    {
        StringBuilder buf = new StringBuilder();
        if (owner != null)
        {
            StringBuilder contant = new StringBuilder();
            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.PERMISSION_APO,
                permissionValue = GViewConstant.SECTION_OWNER
            };
            SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(moduleName, permission,GviewID.OwnerEdit);
            // contant.Append("<table role='presentation' border='0' cellpadding='0' cellspacing='0'>");

            StringBuilder tmpContant1 = new StringBuilder();
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            if (!string.IsNullOrEmpty(owner.ownerTitle) && gviewBll.IsFieldVisible(models, "txtTitle"))
            {
                tmpContant1.Append(ScriptFilter.FilterScript(owner.ownerTitle) + ACAConstant.BLANK);
            }

            if (!string.IsNullOrEmpty(owner.ownerFullName) && gviewBll.IsFieldVisible(models, "txtName"))
            {
                tmpContant1.Append(ScriptFilter.FilterScript(owner.ownerFullName));
            }

            if (!string.IsNullOrEmpty(tmpContant1.ToString()))
            {
                contant.Append(tmpContant1);
                contant.Append("<br>");
            }

            if (!string.IsNullOrEmpty(owner.mailAddress1) && gviewBll.IsFieldVisible(models, "txtAddress1"))
            {
                contant.Append(ScriptFilter.FilterScript(owner.mailAddress1));
                contant.Append("<br>");
            }

            if (!string.IsNullOrEmpty(owner.mailAddress2) && gviewBll.IsFieldVisible(models, "txtAddress2"))
            {
                contant.Append(ScriptFilter.FilterScript(owner.mailAddress2));
                contant.Append("<br>");
            }

            if (!string.IsNullOrEmpty(owner.mailAddress3) && gviewBll.IsFieldVisible(models, "txtAddress3"))
            {
                contant.Append(ScriptFilter.FilterScript(owner.mailAddress3));
                contant.Append("<br>");
            }

            StringBuilder tmpContant2 = new StringBuilder();

            if (!string.IsNullOrEmpty(owner.mailCity) && gviewBll.IsFieldVisible(models, "txtCity"))
            {
                tmpContant2.Append(ScriptFilter.FilterScript(owner.mailCity) + ACAConstant.BLANK);
            }

            if (!string.IsNullOrEmpty(owner.mailState) && gviewBll.IsFieldVisible(models, "ddlAppState"))
            {
                tmpContant2.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(owner.mailState, owner.mailCountry)) + ACAConstant.BLANK);
            }

            if (!string.IsNullOrEmpty(owner.mailZip) && gviewBll.IsFieldVisible(models, "txtZip"))
            {
                tmpContant2.Append(ModelUIFormat.FormatZipShow(owner.mailZip, owner.mailCountry) + ACAConstant.BLANK);
            }

            if (!string.IsNullOrEmpty(tmpContant2.ToString()))
            {
                contant.Append(tmpContant2);
                contant.Append("<br>");
            }

            if (!string.IsNullOrEmpty(owner.mailCountry) && gviewBll.IsFieldVisible(models, "ddlCountry"))
            {
                contant.Append(ScriptFilter.FilterScript(StandardChoiceUtil.GetCountryByKey(owner.mailCountry)));
                contant.Append("<br>");
            }

            if (!string.IsNullOrEmpty(owner.phone) &&
                   gviewBll.IsFieldVisible(models, "txtPhone"))
            {
                string phone = ModelUIFormat.FormatPhoneShow(owner.phoneCountryCode, owner.phone, owner.mailCountry);
                contant.Append(per_owneredit_phone);
                contant.Append(StripHTMLFromLabelText(phone, phone));
                contant.Append("<br>");
            }

            if (!string.IsNullOrEmpty(owner.fax) &&
                gviewBll.IsFieldVisible(models, "txtFax"))
            {
                string fax = ModelUIFormat.FormatPhoneShow(owner.faxCountryCode, owner.fax, owner.mailCountry);
                contant.Append(per_owneredit_fax);
                contant.Append(StripHTMLFromLabelText(fax, fax));
                contant.Append("<br>");
            }

            if (!string.IsNullOrEmpty(owner.email) && gviewBll.IsFieldVisible(models, "txtEmail"))
            {
                // contant.Append("Email: ");
                contant.Append(ScriptFilter.FilterScript(StripHTMLFromLabelText(owner.email, owner.email)));
                contant.Append("<br>");
            }

            buf.Append(contant);
            return "<div id=\"pageSectionTitle\">Owner: <br><span id=\"pageLineText\">" + buf.ToString() + "</span></div>";
       }
        else
        {
             return string.Empty;
        }
    }

    /// <summary>
    /// Gets the section permissions for the specified module.
    /// </summary>
    /// <param name="moduleName">The name of the module to get section permissions </param>
    /// <returns>returns all section permissions of this module. returns empty Dictionary instance if no section permissions</returns>
    private Dictionary<string, UserRolePrivilegeModel> GetSectionPermissions(string moduleName)
    {
        Dictionary<string, UserRolePrivilegeModel> sectionPermissions = new Dictionary<string, UserRolePrivilegeModel>();

        IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
        XPolicyModel[] xpolicys = xPolicyBll.GetPolicyListByCategory(
            BizDomainConstant.STD_ITEM_CAPDETAIL_SECTIONROLES,
            ACAConstant.LEVEL_TYPE_MODULE,
            moduleName);

        if (xpolicys == null)
        {
            return sectionPermissions;
        }

        // find general section permissions
        foreach (XPolicyModel xpolicy in xpolicys)
        {
            if (EntityType.GENERAL.ToString().Equals(xpolicy.data3, StringComparison.InvariantCultureIgnoreCase))
            {
                if ((!string.IsNullOrEmpty(xpolicy.data2) && xpolicy.data2.Length < 5) ||
                    string.IsNullOrEmpty(xpolicy.data4))
                {
                    throw new ACAException("Invalid section permissions");
                }

                if (sectionPermissions.ContainsKey(xpolicy.data4))
                {
                    continue;
                }

                UserRolePrivilegeModel userRole = null;
                if (!string.IsNullOrEmpty(xpolicy.data2))
                {
                    //userRole = UserRoleUtil.ConvertToUserRolePrivilegeModel(xpolicy.data2);
                    var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
                    userRole = userRoleBll.ConvertToUserRolePrivilegeModel(xpolicy.data2);
                }

                this.FillInLicenseTypesInfo(xpolicys, xpolicy.data4, userRole);
                sectionPermissions.Add(xpolicy.data4, userRole);
            }
        }

        return sectionPermissions;
    }

    /// <summary>
    /// Finds license type setting information for the specified section in the given XPolicyModel4WS array and fills 
    /// found license types into the specified 
    /// </summary>
    /// <param name="xpolicys">xpolicys in which to find matching licence types for the section</param>
    /// <param name="sectionName">the name of the section for which to find license types</param>
    /// <param name="sectionPermission">UserRolePrivilegeModel4WS instance to be filled in</param>
    private void FillInLicenseTypesInfo(XPolicyModel[] xpolicys, string sectionName, UserRolePrivilegeModel sectionPermission)
    {
        // find related license types
        foreach (XPolicyModel xpolicy in xpolicys)
        {
            if (EntityType.LICENSETYPE.ToString().Equals(xpolicy.data3, StringComparison.InvariantCultureIgnoreCase) &&
                sectionName.Equals(xpolicy.data4, StringComparison.InvariantCultureIgnoreCase))
            {
                if (!string.IsNullOrEmpty(xpolicy.data2))
                {
                    sectionPermission.licenseTypeRuleArray
                        = xpolicy.data2.Split(new string[] { ACAConstant.SPLIT_DOUBLE_VERTICAL }, StringSplitOptions.RemoveEmptyEntries);
                }

                break;
            }
        }
    }

    /// <summary>
    /// Gets a boolean value represpenting whether the given section needs to display for the current user according
    /// to the given section permissions.
    /// Note:
    /// For compatibility with previous version, in this feature, the following sections:
    ///   1.¡°Education¡±, 
    ///   2.¡°Continuing Education¡±, 
    ///   3.¡°Examination¡±
    /// by default will be NOT visible for any users (except Admin), and the other sections will be visible for any users
    /// </summary>
    /// <param name="section">The section to determine whether to display </param>
    /// <param name="sectionPermissions">The section permissions used to determine the specified whether to display</param>
    /// <returns>true if the specified section need to display; otherwise, false</returns>
    /// <remarks>by default, all sections is visible for any user unless administor explicitly configures permissions for sections</remarks>
    private bool GetSectionVisibility(string section, Dictionary<string, UserRolePrivilegeModel> sectionPermissions)
    {
        bool isSectionVisible = true;

        // The default situation that no section permission is set
        if (sectionPermissions == null || sectionPermissions.Count == 0)
        {
            isSectionVisible = GetDefaultVisibility(section);
        }
        else
        {
            UserRolePrivilegeModel findedPermission = null;
            if (sectionPermissions.ContainsKey(section))
            {
                findedPermission = sectionPermissions[section];
            }

            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();

            if (findedPermission != null)
            {
                isSectionVisible = proxyUserRoleBll.HasReadOnlyPermission(AppSession.GetCapModelFromSession(this.ModuleName), findedPermission);
            }
            else
            {
                // The default situation that no section permission is set for the specified section
                isSectionVisible = GetDefaultVisibility(section);
            }
        }

        return isSectionVisible;
    }
    /// <summary>
    /// Get the default section permission for the specified section.
    /// </summary>
    /// <param name="section">The section to get the default section permission</param>
    /// <returns>true if the default section permission is visible; otherwise, false</returns>
    /// <remarks>For compatibility with previous version, in this feature, the following sections:
    ///   1.¡°Education¡±, 
    ///   2.¡°Continuing Education¡±, 
    ///   3.¡°Examination¡±
    /// by default will be NOT visible for any users (except Admin), and the other sections will be visible for any users
    /// </remarks>
    private bool GetDefaultVisibility(string section)
    {
        bool visible = true;

        if (CapDetailSectionType.EDUCATION.ToString().Equals(section, StringComparison.InvariantCultureIgnoreCase) ||
            CapDetailSectionType.CONTINUING_EDUCATION.ToString().Equals(section, StringComparison.InvariantCultureIgnoreCase) ||
            CapDetailSectionType.EXAMINATION.ToString().Equals(section, StringComparison.InvariantCultureIgnoreCase))
        {
            visible = false;
        }

        return visible;
    }

    /// <summary>
    /// Get the formatted value for "Licensed Professional"
    /// </summary>
    /// <param name="licenses">LicenseProfessionalModel array</param>
    ////// <param name="moduleName">module name</param>
    ////// <param name="lines">line number</param>
    ////// <param name="isDisplayTempleteField">true to display template field</param>
    /// <returns>formatted value for licensed professional</returns>
    public  string FormatLicenseModel4Basic(LicenseProfessionalModel[] licenses)
    {
        StringBuilder breakline = new StringBuilder();
        StringBuilder buf = new StringBuilder();

        if (licenses == null ||
            licenses.Length == 0)
        {
             return string.Empty;
        }

        int index = -1;

        for (int i = 0; i < licenses.Length; i++)
        {
            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel =GviewID.CapDetail,
                permissionValue = GViewConstant.SECTION_LICENSE

            };
            SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, permission, GviewID.LicenseEdit);

            StringBuilder contant = new StringBuilder();
            LicenseProfessionalModel licenseProfessional = licenses[i];

            if (licenseProfessional.licenseNbr == null)
            {
                continue;
            }
            else
            {
                index++;
            }

            
            bool isExistName = false;

            if (index > 0)
            {
                if (index == 1)
                {
                    StringBuilder temp = new StringBuilder();
                    temp.Append(index.ToString() + ") " + buf.ToString());
                    buf = new StringBuilder();
                    buf.Append(temp.ToString());
                }
                buf.Append("<br>");
                buf.Append((index + 1).ToString() + ") ");
            }

            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            if (!string.IsNullOrEmpty(licenseProfessional.contactFirstName) && gviewBll.IsFieldVisible(models, "txtFirstName"))
            {
                isExistName = true;
                contant.Append(ScriptFilter.FilterScript(licenseProfessional.contactFirstName) + " ");
            }

            if (!string.IsNullOrEmpty(licenseProfessional.contactMiddleName) && gviewBll.IsFieldVisible(models, "txtMiddleName"))
            {
                isExistName = true;
                contant.Append(ScriptFilter.FilterScript(licenseProfessional.contactMiddleName) + " ");
            }

            if (!string.IsNullOrEmpty(licenseProfessional.contactLastName) && gviewBll.IsFieldVisible(models, "txtLastName"))
            {
                isExistName = true;
                contant.Append(ScriptFilter.FilterScript(licenseProfessional.contactLastName));
            }

            if (isExistName)
            {
                buf.Append("<b>");
                buf.Append(contant.ToString());
                buf.Append("</b><br>");
                if (licenseProfessional.birthDate != null && gviewBll.IsFieldVisible(models, "txtBirthDate"))
                {
                    string bd = I18nDateTimeUtil.FormatToDateStringForUI(licenseProfessional.birthDate.Value);
                    buf.Append(bd);
                    buf.Append("<br>");
                }

                if (!string.IsNullOrEmpty(licenseProfessional.gender) && gviewBll.IsFieldVisible(models, "radioListGender"))
                {
                    string gender = StandardChoiceUtil.GetGenderByKey(licenseProfessional.gender);
                    buf.Append(gender);
                    buf.Append("<br>");
                }
            }

            if (!string.IsNullOrEmpty(licenseProfessional.businessName) &&
                gviewBll.IsFieldVisible(models, "txtBusName"))
            {
                if (!isExistName)
                {
                    buf.Append("<b>");
                }
                buf.Append(ScriptFilter.FilterScript(licenseProfessional.businessName));

                if (!string.IsNullOrEmpty(licenseProfessional.busName2) &&
                    gviewBll.IsFieldVisible(models, "txtBusName2"))
                {
                    buf.Append(ACAConstant.SLASH + ScriptFilter.FilterScript(licenseProfessional.busName2));
                }
                if (!isExistName)
                {
                    buf.Append("</b>");
                }
               buf.Append("<br>");
            }

            if (!string.IsNullOrEmpty(licenseProfessional.businessLicense) && gviewBll.IsFieldVisible(models, "txtBusLicense"))
            {
                buf.Append(ScriptFilter.FilterScript(licenseProfessional.businessLicense));
                buf.Append("<br>");
            }

            if (!string.IsNullOrEmpty(licenseProfessional.address1) && gviewBll.IsFieldVisible(models, "txtAddress1"))
            {
                buf.Append(ScriptFilter.FilterScript(licenseProfessional.address1));
                buf.Append("<br>");
            }

            if (!string.IsNullOrEmpty(licenseProfessional.address2) &&
                gviewBll.IsFieldVisible(models, "txtAddress2"))
            {
                buf.Append(ScriptFilter.FilterScript(licenseProfessional.address2));
                buf.Append("<br>");
            }

            if (!string.IsNullOrEmpty(licenseProfessional.address3) &&
                gviewBll.IsFieldVisible(models, "txtAddress3"))
            {
                buf.Append(ScriptFilter.FilterScript(licenseProfessional.address3));
                buf.Append("<br>");
            }
            bool addBreadkToCityStateZipLine = false;
            if (!string.IsNullOrEmpty(licenseProfessional.city) && gviewBll.IsFieldVisible(models, "txtCity"))
            {
                addBreadkToCityStateZipLine = true;
                buf.Append(ScriptFilter.FilterScript(licenseProfessional.city));
                buf.Append(ACAConstant.COMMA_BLANK);
            }

            if (!string.IsNullOrEmpty(licenseProfessional.state) && gviewBll.IsFieldVisible(models, "txtState"))
            {
                addBreadkToCityStateZipLine = true;
                buf.Append(I18nUtil.DisplayStateForI18N(licenseProfessional.state, licenseProfessional.countryCode));
                buf.Append(ACAConstant.COMMA_BLANK);
            }

            if (!string.IsNullOrEmpty(licenseProfessional.zip) && gviewBll.IsFieldVisible(models, "txtZipCode"))
            {
                addBreadkToCityStateZipLine = true;
                buf.Append(FormatZipShow(licenseProfessional.zip, licenseProfessional.countryCode));
            }
            if (addBreadkToCityStateZipLine)
            {
                buf.Append("<br>");
            }
            if (!string.IsNullOrEmpty(licenseProfessional.countryCode) &&
                StandardChoiceUtil.IsCountryCodeEnabled() && gviewBll.IsFieldVisible(models, "ddlCountry"))
            {
                string country = StandardChoiceUtil.GetCountryByKey(licenseProfessional.countryCode);
                buf.Append(country);
                buf.Append("<br>");
            }

            if (!string.IsNullOrEmpty(licenseProfessional.postOfficeBox) && gviewBll.IsFieldVisible(models, "txtPOBox"))
            {
                buf.Append(ScriptFilter.FilterScript(licenseProfessional.postOfficeBox));
                buf.Append("<br>");
            }

            LicenseModel4WS licenseModel = new LicenseModel4WS();

            // Get Reference License Model based on License Type & Number to display license state
            if (!string.IsNullOrEmpty(licenseProfessional.licenseNbr) &&
               !string.IsNullOrEmpty(licenseProfessional.licenseType))
            {
                LicenseModel4WS license = new LicenseModel4WS();

                license.serviceProviderCode = ConfigManager.AgencyCode;
                license.licenseType = licenseProfessional.licenseType;
                license.stateLicense = licenseProfessional.licenseNbr;

                ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                licenseModel = licenseBll.GetLicenseByStateLicNbr(license);
            }

            if (gviewBll.IsFieldVisible(models, "txtHomePhone"))
            {
                if (!string.IsNullOrEmpty(licenseProfessional.phone1))
                {
                    string phone1 = FormatPhoneShow(licenseProfessional.phone1CountryCode, licenseProfessional.phone1, licenseProfessional.countryCode);
                    buf.Append(LicenseEdit_LicensePro_label_homePhone);
                    buf.Append(phone1);
                    // buf.Append(I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("LicenseEdit_LicensePro_label_homePhone", ModuleName), phone1));
                    buf.Append("<br>");
                }
            }

            if (gviewBll.IsFieldVisible(models, "txtMobilePhone"))
            {
                if (!string.IsNullOrEmpty(licenseProfessional.phone2))
                {
                    string phone2 = FormatPhoneShow(licenseProfessional.phone2CountryCode, licenseProfessional.phone2, licenseProfessional.countryCode);
                    buf.Append(LicenseEdit_LicensePro_label_mobile);
                    buf.Append(phone2);
                    //buf.Append(I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("LicenseEdit_LicensePro_label_mobile", ModuleName), phone2));
                    buf.Append("<br>");
                }
            }

            if (gviewBll.IsFieldVisible(models, "txtFax"))
            {
                if (!string.IsNullOrEmpty(licenseProfessional.fax))
                {
                    string fax = FormatPhoneShow(licenseProfessional.faxCountryCode, licenseProfessional.fax, licenseProfessional.countryCode);
                    buf.Append(LicenseEdit_LicensePro_label_fax);
                    buf.Append(fax);
                    //buf.Append(I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("LicenseEdit_LicensePro_label_fax", ModuleName), fax));
                    buf.Append("<br>");
                }
            }
            bool addLineBreakAfterLicneseInfo = false;
            if (!string.IsNullOrEmpty(licenseProfessional.resLicenseType) && gviewBll.IsFieldVisible(models, "ddlLicenseType"))
            {
                if (licenseModel != null && !string.IsNullOrEmpty(licenseModel.licState) && StandardChoiceUtil.IsDisplayLicenseState())
                {
                    buf.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModel.licState, licenseModel.countryCode)));
                    buf.Append(" ");
                }

                buf.Append(ScriptFilter.FilterScript(licenseProfessional.resLicenseType));
                buf.Append(" ");
                addLineBreakAfterLicneseInfo = true;
            }

            if (!string.IsNullOrEmpty(licenseProfessional.licenseNbr) && gviewBll.IsFieldVisible(models, "txtLicenseNum"))
            {

                if (licenseModel != null && !string.IsNullOrEmpty(licenseModel.licState) && StandardChoiceUtil.IsDisplayLicenseState())
                {
                    buf.Append(ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModel.licState, licenseModel.countryCode)));
                    buf.Append("-");
                }

                buf.Append(" ");
                buf.Append(ScriptFilter.FilterScript(licenseProfessional.licenseNbr));
                addLineBreakAfterLicneseInfo = true;
            }

            if (addLineBreakAfterLicneseInfo)
            {
                buf.Append("<br>");
            }
            if (!string.IsNullOrEmpty(licenseProfessional.typeFlag) && gviewBll.IsFieldVisible(models, "ddlContactType"))
            {
                string strTypeFlag = ScriptFilter.FilterScript(DropDownListBindUtil.GetTypeFlagTextByValue(licenseProfessional.typeFlag));

                buf.Append(strTypeFlag);
                buf.Append("<br>");
            }

            if (gviewBll.IsFieldVisible(models, "txtContractorLicNO"))
            {
                if (!string.IsNullOrEmpty(licenseProfessional.contrLicNo))
                {
                    buf.Append(I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("licenseedit_licensepro_label_contractorlicno", ModuleName), licenseProfessional.contrLicNo));
                    buf.Append("<br>");
                }
            }

            if (gviewBll.IsFieldVisible(models, "txtContractorBusiName"))
            {
                if (!string.IsNullOrEmpty(licenseProfessional.contLicBusName))
                {
                    buf.Append(I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("licenseedit_licensepro_label_contractorbusiname", ModuleName), licenseProfessional.contLicBusName));
                    buf.Append("<br>");
                }
            }
 //            buf.Append("<br>");
        } //end for

        if (buf.ToString() != string.Empty)
        {
            return "<div id=\"pageSectionTitle\">Licensed Professional: <br><span id=\"pageLineText\">" + buf.ToString() + "</span></div>";
        }
        return string.Empty;
    }

    #endregion
}
