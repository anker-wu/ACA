/**
* <pre>
* 
*  Accela Citizen Access
*  File: License.Info.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2011
* 
*  Description:
*  View License details.
* 
*  Notes:
*      $Id: License.Info.aspx.cs 237981 2012-11-15 08:58:28Z ACHIEVO\alan.hu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-17-2009           Dave Brewster           New page added for version 7.0
*  01-24-2011           Dave Brewster           Updated for 7.1 relase
* </pre>
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;


/// <summary>
/// 
/// </summary>
public partial class LicenseInfo : AccelaPage
{
    private const string HTML_EMPTY = "&nbsp;";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder LicenseDetails = new StringBuilder();
    public StringBuilder BusinessDetails = new StringBuilder();
    public StringBuilder ContactDetails = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder PageTitle = new StringBuilder();

    private string LicenseEdit_LicensePro_label_homePhone = string.Empty;
    private string LicenseEdit_LicensePro_label_mobile = string.Empty;
    private string LicenseEdit_LicensePro_label_fax = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("License.Info.aspx");
        string resultPageNo = GetFieldValue("ResultPageNo", false);
        string licenseNumber = GetFieldValue("LicenseNumber", false);
        string licenseType = GetFieldValue("LicenseType", false);
        string SearchMode = GetFieldValue("SearchMode", false);
        bool isGlobalSearch = GetFieldValue("GlobalSearch", false) == "true";
        LicenseEdit_LicensePro_label_homePhone = StripHTMLFromLabelText(LabelUtil.GetTextByKey("LicenseEdit_LicensePro_label_homePhone", ModuleName), "Phone 1: ");
        LicenseEdit_LicensePro_label_mobile = StripHTMLFromLabelText(LabelUtil.GetTextByKey("LicenseEdit_LicensePro_label_mobile", ModuleName), "Phone 2: ");
        LicenseEdit_LicensePro_label_fax = StripHTMLFromLabelText(LabelUtil.GetTextByKey("LicenseEdit_LicensePro_label_fax", ModuleName), "Fax: ");

        LicenseModel4WS searchParamModel = new LicenseModel4WS();
        searchParamModel.serviceProviderCode = ConfigManager.AgencyCode;
        searchParamModel.stateLicense = licenseNumber;
        searchParamModel.licenseType = licenseType;
        iPhonePageTitle = licenseNumber;
        try
        {
            DateTime dateWork;
            CapModel4WS capModel = null;
            LicenseProfessionalModel4WS licPro = null;

            if (isGlobalSearch == false)
            {
                capModel = AppSession.GetCapModelFromSession(ModuleName);
                licPro = capModel.licenseProfessionalModel;
                for (int aRow = 0; aRow < capModel.licenseProfessionalList.Length; aRow++)
                {
                    if (capModel.licenseProfessionalList[aRow].licenseNbr == licenseNumber
                        && capModel.licenseProfessionalList[aRow].licenseType == licenseType)
                    {
                        licPro = capModel.licenseProfessionalList[aRow];
                        break;
                    }
                }
            }
            ProviderModel4WS provider = new ProviderModel4WS();
            ILicenseBLL licenseeBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
            LicenseModel4WS[] resultArray = licenseeBll.GetLicenseList(searchParamModel, provider, false, null);
            bool useLicPro = (resultArray == null || resultArray.Length == 0);

            if (licPro == null && (resultArray == null || resultArray.Length == 0))
            {
                ErrorMessage.Append("No license information is on file for License Number: " + licenseNumber.ToString());
            }
            else
            {
                string titleTextDiv = "<div id=\"pageSectionTitle\">";
                string normalTextDiv = "<div id=\"pageTextIndented\">";

                if (useLicPro == false)
                {
                    LicenseModel4WS aLicense = resultArray[0];
                    if (aLicense.contactFirstName != null || aLicense.contactLastName != null || aLicense.contactMiddleName != null)
                    {
                        PageTitle.Append(titleTextDiv);
                        PageTitle.Append(aLicense.contactFirstName != null ? aLicense.contactFirstName + " " : string.Empty);
                        PageTitle.Append(aLicense.contactMiddleName != null ? aLicense.contactMiddleName + " " : string.Empty);
                        PageTitle.Append(aLicense.contactLastName != null ? aLicense.contactLastName + " " : string.Empty);
                        PageTitle.Append("</div>");
                    }
                    else if (aLicense.contLicBusName != null)
                    {
                        PageTitle.Append(titleTextDiv);
                        PageTitle.Append(aLicense.contLicBusName);
                        PageTitle.Append("</div>");
                    }
                    LicenseDetails.Append(normalTextDiv);
                    LicenseDetails.Append(licenseNumber + " (");
                    LicenseDetails.Append(licenseType + ")");
                    LicenseDetails.Append("</div>");

                    string newLine = "";

                    LicenseDetails.Append(normalTextDiv);
                    if (aLicense.licenseBoard != null && aLicense.licenseBoard != string.Empty)
                    {
                        LicenseDetails.Append(aLicense.licState != null && aLicense.licState != string.Empty ? aLicense.licState.ToString() + ", " : string.Empty);
                        LicenseDetails.Append(aLicense.licenseBoard != null && aLicense.licenseBoard != string.Empty ? aLicense.licenseBoard.ToString() : string.Empty);
                        newLine = "<br>";
                    }
                    if (aLicense.licenseIssueDate != null)
                    {
                        if (DateTime.TryParse(aLicense.licenseIssueDate, out dateWork) == true)
                        {
                            LicenseDetails.Append(newLine);
                            newLine = "<br>";
                            LicenseDetails.Append("Issued: ");
                            LicenseDetails.Append(dateWork.ToString("MM/dd/yyyy"));
                        }
                    }
                    if (aLicense.licenseExpirationDate != null)
                    {
                        if (DateTime.TryParse(aLicense.licenseExpirationDate, out dateWork) == true)
                        {
                            LicenseDetails.Append(newLine);
                            newLine = "<br>";
                            LicenseDetails.Append("Expires: ");
                            LicenseDetails.Append(dateWork.ToString("MM/dd/yyyy"));
                        }
                    }
                    LicenseDetails.Append("</div>");
                    BusinessDetails.Append(titleTextDiv);
                    BusinessDetails.Append("Business Information");
                    BusinessDetails.Append("</div>");

                    newLine = "";
                    BusinessDetails.Append(normalTextDiv);
                    if (aLicense.businessName != null && aLicense.businessName != string.Empty)
                    {
                        BusinessDetails.Append(aLicense.businessName);
                        newLine = "<br>";
                    }
                    if (aLicense.businessLicense != null && aLicense.businessLicense != string.Empty)
                    {
                        BusinessDetails.Append(newLine);
                        BusinessDetails.Append(aLicense.businessLicense);
                        newLine = "<br>";
                    }
                    if (aLicense.businessLicExpDate != null)
                    {
                        if (DateTime.TryParse(aLicense.businessLicExpDate, out dateWork) == true)
                        {
                            BusinessDetails.Append(newLine);
                            newLine = "<br>";
                            BusinessDetails.Append("Expires: ");
                            BusinessDetails.Append(dateWork.ToString("MM/dd/yyyy"));
                        }
                    }
                    BusinessDetails.Append(newLine + aLicense.insuranceCo);
                    if (aLicense.insuranceExpDate != null)
                    {
                        if (DateTime.TryParse(aLicense.insuranceExpDate, out dateWork) == true)
                        {
                            BusinessDetails.Append(newLine);
                            newLine = "<br>";
                            BusinessDetails.Append("Expires: ");
                            BusinessDetails.Append(dateWork.ToString("MM/dd/yyyy"));
                        }
                    }
                    BusinessDetails.Append("</div>");

                    ContactDetails.Append(titleTextDiv);
                    ContactDetails.Append("Contact Information");
                    ContactDetails.Append("</div>");

                    newLine = "";
                    ContactDetails.Append(normalTextDiv);
                    if (aLicense.address1 != null && aLicense.address1 != string.Empty)
                    {
                        ContactDetails.Append(aLicense.address1);
                        newLine = "<br>";
                    }
                    if (aLicense.address2 != null && aLicense.address2 != string.Empty)
                    {
                        ContactDetails.Append(newLine + aLicense.address2);
                        newLine = "<br>";
                    }
                    if (aLicense.address3 != null && aLicense.address3 != string.Empty)
                    {
                        ContactDetails.Append(newLine + aLicense.address3);
                        newLine = "<br>";
                    }
                    if ((aLicense.city != null && aLicense.city != string.Empty)
                        || (aLicense.state != null && aLicense.state != string.Empty)
                        || (aLicense.zip != null && aLicense.zip != string.Empty))
                    {
                        string comma = string.Empty;
                        string dash = string.Empty;
                        if (aLicense.city != null && aLicense.city != string.Empty)
                        {
                            ContactDetails.Append(newLine + aLicense.city);
                            comma = ", ";
                            newLine = "<br>";
                        }
                        if (aLicense.state != null && aLicense.state != string.Empty)
                        {
                            ContactDetails.Append(comma + I18nUtil.DisplayStateForI18N(aLicense.state, aLicense.countryCode));
                            dash = " - ";
                            newLine = "<br>";
                        }
                        if (aLicense.zip != null && aLicense.zip != string.Empty)
                        {
                            ContactDetails.Append(dash + this.FormatZipShow(aLicense.zip, aLicense.countryCode));
                            newLine = "<br>";
                        }
                    }
                    ContactDetails.Append(newLine);
                    string faxPadding = string.Empty;
                    string emailPadding = string.Empty;
                    string phoneLabel = "Phone: ";
                    if (aLicense.phone1 != null && aLicense.phone1.Trim() != string.Empty)
                    {
                        ContactDetails.Append(newLine);
                        newLine = "<br>";
                        ContactDetails.Append("Phone 1: ");
                        phoneLabel = "Phone 2: ";
                        string phone1 = FormatPhoneShow(aLicense.phone1CountryCode, aLicense.phone1, aLicense.countryCode);
                        ContactDetails.Append(phone1);
                        faxPadding = " ";
                        emailPadding = " ";
                    }
                    if (aLicense.phone2 != null && aLicense.phone2.Trim() != string.Empty)
                    {
                        ContactDetails.Append(newLine);
                        newLine = "<br>";
                        ContactDetails.Append(phoneLabel);
                        string phone1 = FormatPhoneShow(aLicense.phone2CountryCode, aLicense.phone2, aLicense.countryCode);
                        ContactDetails.Append(phone1);
                        faxPadding = " "; 
                        emailPadding = " "; 
                    }
                    if (aLicense.fax != null && aLicense.fax.Trim() != string.Empty)
                    {
                        ContactDetails.Append(newLine);
                        newLine = "<br>";
                        ContactDetails.Append("Fax" + faxPadding + ": ");
                        string fax = FormatPhoneShow(aLicense.faxCountryCode, aLicense.fax, aLicense.countryCode);
                        ContactDetails.Append(fax);
                    }
                    ContactDetails.Append(aLicense.emailAddress != null ? newLine + "email" + emailPadding + ": " + aLicense.emailAddress : string.Empty);
                    ContactDetails.Append("</div>");
                }
                else
                {
                    if (licPro.contactFirstName != null || licPro.contactLastName != null || licPro.contactMiddleName != null)
                    {
                        PageTitle.Append(titleTextDiv);
                        PageTitle.Append(licPro.contactFirstName != null ? licPro.contactFirstName + " " : string.Empty);
                        PageTitle.Append(licPro.contactMiddleName != null ? licPro.contactMiddleName + " " : string.Empty);
                        PageTitle.Append(licPro.contactLastName != null ? licPro.contactLastName + " " : string.Empty);
                        PageTitle.Append("</div>");
                    }
                    else if (licPro.contLicBusName != null)
                    {
                        PageTitle.Append(titleTextDiv);
                        PageTitle.Append(licPro.contLicBusName);
                        PageTitle.Append("</div>");
                    }
                    LicenseDetails.Append(normalTextDiv);
                    LicenseDetails.Append(licenseNumber + " (");
                    LicenseDetails.Append(licenseType + ")");
                    LicenseDetails.Append("</div>");

                    string newLine = "";

                    LicenseDetails.Append(normalTextDiv);
                    LicenseDetails.Append("</div>");


                    BusinessDetails.Append(titleTextDiv);
                    BusinessDetails.Append("Business Information");
                    BusinessDetails.Append("</div>");

                    newLine = "";
                    BusinessDetails.Append(normalTextDiv);
                    if (licPro.businessName != null && licPro.businessName != string.Empty)
                    {
                        BusinessDetails.Append(licPro.businessName);
                        newLine = "<br>";
                    }
                    if (licPro.businessLicense != null && licPro.businessLicense != string.Empty)
                    {
                        BusinessDetails.Append(newLine);
                        BusinessDetails.Append(licPro.businessLicense);
                        newLine = "<br>";
                    }
                    BusinessDetails.Append("</div>");

                    ContactDetails.Append(titleTextDiv);
                    ContactDetails.Append("Contact Information");
                    ContactDetails.Append("</div>");

                    newLine = "";
                    ContactDetails.Append(normalTextDiv);
                    if (licPro.address1 != null && licPro.address1 != string.Empty)
                    {
                        ContactDetails.Append(licPro.address1);
                        newLine = "<br>";
                    }
                    if (licPro.address2 != null && licPro.address2 != string.Empty)
                    {
                        ContactDetails.Append(newLine + licPro.address2);
                        newLine = "<br>";
                    }
                    if (licPro.address3 != null && licPro.address3 != string.Empty)
                    {
                        ContactDetails.Append(newLine + licPro.address3);
                        newLine = "<br>";
                    }
                    if ((licPro.city != null && licPro.city != string.Empty)
                        || (licPro.state != null && licPro.state != string.Empty)
                        || (licPro.zip != null && licPro.zip != string.Empty))
                    {
                        string comma = string.Empty;
                        string dash = string.Empty;
                        if (licPro.city != null && licPro.city != string.Empty)
                        {
                            ContactDetails.Append(newLine + licPro.city);
                            comma = ", ";
                            newLine = "<br>";
                        }
                        if (licPro.state != null && licPro.state != string.Empty)
                        {
                            ContactDetails.Append(comma + licPro.state);
                            dash = " - ";
                            newLine = "<br>";
                        }
                        if (licPro.zip != null && licPro.zip != string.Empty)
                        {
                            ContactDetails.Append(dash + this.FormatZipShow(licPro.zip, licPro.countryCode));
                            newLine = "<br>";
                        }
                    }
                    ContactDetails.Append(newLine);
                    string faxPadding = string.Empty;
                    string emailPadding = string.Empty;
                    // string phoneLabel = "Phone: ";
                    if (licPro.phone1 != null && licPro.phone1.Trim() != string.Empty)
                    {
                        ContactDetails.Append(newLine);
                        newLine = "<br>";
                        ContactDetails.Append(LicenseEdit_LicensePro_label_homePhone);
                        // phoneLabel = "Phone 2: ";
                        string phone1 = FormatPhoneShow(licPro.phone1CountryCode, licPro.phone1, licPro.countryCode);
                        ContactDetails.Append(phone1);
                        faxPadding = " ";
                        emailPadding = " ";
                    }
                    if (licPro.phone2 != null && licPro.phone2.Trim() != string.Empty)
                    {
                        ContactDetails.Append(newLine);
                        newLine = "<br>";
                        ContactDetails.Append(LicenseEdit_LicensePro_label_mobile);
                        string phone2 = FormatPhoneShow(licPro.phone2CountryCode, licPro.phone2, licPro.countryCode);
                        ContactDetails.Append(phone2);
                        faxPadding = " "; 
                        emailPadding = " ";
                    }
                    if (licPro.fax != null && licPro.fax.Trim() != string.Empty)
                    {
                        ContactDetails.Append(newLine);
                        newLine = "<br>";
                        ContactDetails.Append(LicenseEdit_LicensePro_label_fax);
                        string fax = FormatPhoneShow(licPro.faxCountryCode, licPro.fax, licPro.countryCode);
                        ContactDetails.Append(fax);
                    }
                    ContactDetails.Append(licPro.email != null ? newLine + "Email" + emailPadding + ": " + licPro.email : string.Empty);
                    ContactDetails.Append("</div>");
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(ex.Message);
            ErrorMessage.Append(ErrorFormatEnd);
        }
        // string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        // bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder sbWork = new StringBuilder();
        Breadcrumbs = BreadCrumbHelper("License.Info.aspx", sbWork, "Dummy Breadcrumb", null, false , false, false, false);
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
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
}
