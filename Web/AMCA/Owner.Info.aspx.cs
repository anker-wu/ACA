/**
* <pre>
* 
*  Accela Citizen Access
*  File: Owner.Info.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  View Parcel Owner details.
* 
*  Notes:
*      $Id: Owner.Info 77905 2009-08-17 12:49:28Z  dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-05-2009           Dave Brewster           New page added for version 7.0
* 
* </pre>
*/
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
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;


/// <summary>
/// 
/// </summary>
public partial class OwnerInfo : AccelaPage
{
    private const string HTML_EMPTY = "&nbsp;";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder OwnerDetail = new StringBuilder();
    public StringBuilder AddressList = new StringBuilder();
    public StringBuilder ParcelList = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();

    public string ParcelNo = string.Empty;
    public string ParcelSeqNo = string.Empty;
    public string ResultPageNo = string.Empty;
    public string PageTitle = string.Empty;
    private string SearchMode = string.Empty;
    private string ownerName = string.Empty;
    private string ownerMailAddr = string.Empty;
    private string ownerUI = string.Empty;
    private string ownerSeq =  string.Empty;
    private string ownerNumber= string.Empty;

    private ParcelModel parcelModal = new ParcelModel();
    private string divPageText = "<div id=\"pageTextIndented\">";
    private string divTitle = "<div id=\"pageSectionTitle\">";
    private int rowCnt = 0;
    private string per_owneredit_phone = string.Empty;
    private string per_owneredit_fax = string.Empty;
 
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("Owner.Info.aspx");
        State = GetFieldValue("State", false);
        ParcelNo = GetFieldValue("ParcelNumber", false);
        ParcelSeqNo = GetFieldValue("ParcelSequence", false);
        ResultPageNo = GetFieldValue("ResultPage", false);
        SearchMode = GetFieldValue("SearchMode", false);
        ownerName = GetFieldValue("OwnerFullName", false);
        ownerMailAddr = GetFieldValue("OwnerMailAddress", false);
        ownerUI = GetFieldValue("OwnerUI", false);
        ownerSeq = GetFieldValue("OwnerSeq", false);
        ownerNumber = GetFieldValue("OwnerNumber", false);
        bool isGlobalSearch = GetFieldValue("GlobalSearch", false) == "true";
        bool isCalledByParcelInfo = GetFieldValue("CalledByParcelInfo", false) == "true";
        bool isCallByAddressInfo = GetFieldValue("CallByAddressInfo", false) == "true";
        string addressRefId = GetFieldValue("AddressRefId", false);
        string addressSeqNo = GetFieldValue("AddressSeq", false);

        iPhonePageTitle = "Owner";
        PageTitle = "<div id=\"pageTitle\">" + ownerName + "</div>"
                  + divTitle + "Owner Details:</div>";

        string per_globalsearch_label_searchresults = LocalGetTextByKey("per_globalsearch_label_searchresults");
        string per_globalsearch_label_apo = LocalGetTextByKey("per_globalsearch_label_apo");
        per_owneredit_phone = StripHTMLFromLabelText(LabelUtil.GetTextByKey("per_owneredit_phone", null), "Phone: ");
        per_owneredit_fax = StripHTMLFromLabelText(LabelUtil.GetTextByKey("per_owneredit_fax", null), "Fax: ");

        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&SearchMode=" + SearchMode.ToString());
        sbWork.Append("&ResultPage=" + ResultPageNo.ToString());
        sbWork.Append("&GlobalSearch=true");
        sbWork.Append("&OwnerFullName=" + ownerName);
        sbWork.Append("&OwnerMailAddress=" + ownerMailAddr);
        sbWork.Append("&OwnerUI=" + ownerUI);
        sbWork.Append("&OwnerSeq=" + ownerSeq);
        sbWork.Append("&OwnerNumber=" + ownerNumber);
        if (isCalledByParcelInfo == true)
        {
            sbWork.Append("&ParcelNumber=" + ParcelNo.ToString());
            sbWork.Append("&ParcelSequence=" + ParcelSeqNo.ToString());
            sbWork.Append("&CalledByParcelInfo=true");
        }
        else // called by AddressInfo
        {
            sbWork.Append("&AddressRefId=" + addressRefId);
            sbWork.Append("&AddressSeq=" + addressSeqNo);
            sbWork.Append("&CalledByAddressinfo=true");
        }
        Breadcrumbs = BreadCrumbHelper("Owner.Info.aspx", sbWork, ownerName, breadCrumbIndex, isElipseLink, false, false, false);
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));

        try
        {
            IOwnerBll ownerBll = (IOwnerBll)ObjectFactory.GetObject(typeof(IOwnerBll));
            OwnerModel ownerPK = new OwnerModel();
            ownerPK.UID = ownerUI;
            ownerPK.sourceSeqNumber = long.Parse(ownerSeq);
            ownerPK.ownerNumber = long.Parse(ownerNumber);
            ownerPK = ownerBll.GetOwnerByPK(ConfigManager.AgencyCode, ownerPK);

            OwnerDetail.Append(divPageText);
            if (ownerPK.ownerFullName != null && ownerPK.ownerFullName.Length > 0)
            {
                OwnerDetail.Append(ownerPK.ownerFullName + "<br>");
            }

            string ownerMailAddress = PeopleUtil.BuildOwnerMailAddressString(ownerPK);

            if (string.IsNullOrEmpty(ownerMailAddress))
            {
                OwnerDetail.Append(ownerMailAddress + "<br>");
            }
            if (ownerPK.phone != null && ownerPK.phone.Length > 0)
            {
                OwnerDetail.Append(per_owneredit_phone);
                OwnerDetail.Append(FormatPhoneShow(ownerPK.phoneCountryCode, ownerPK.phone, ownerPK.mailCountry) + "<br>");
            }
            if (ownerPK.fax != null && ownerPK.fax.Length > 0)
            {
                OwnerDetail.Append(per_owneredit_fax);
                OwnerDetail.Append(FormatPhoneShow(ownerPK.faxCountryCode, ownerPK.fax, ownerPK.mailCountry) + "<br>");
            }
            if (ownerPK.email != null && ownerPK.email.Length > 0)
            {
                OwnerDetail.Append(ownerPK.email + "<br>");
            }

            OwnerDetail.Append("</div>");
            LoadAddressView();
            LoadParcelView();

            // test error trapping
            // throw new Exception("just testing");

        }
        catch (Exception ex)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(ex.Message);
            ErrorMessage.Append(ErrorFormatEnd);
        }

    }

    /// <summary>
    /// fill data for address object
    /// </summary>
    private void LoadAddressView()
    {
        try
        {
            if (parcelModal == null)
            {
                return;
            }
            OwnerModel ownerPK = new OwnerModel();
            ownerPK.sourceSeqNumber = StringUtil.ToLong(ownerSeq);
            ownerPK.UID = ownerUI;
            ownerPK.ownerNumber = StringUtil.ToLong(ownerNumber);

            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            parcelInfo.ownerModel = ownerPK;

            // APOBll apoBLL = new APOBll();
            SearchResultModel result = OwnerService.getAPOList(ConfigManager.AgencyCode, parcelInfo, null, false);
            ParcelInfoModel[] parcelInfoList = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);
            string linkHTML = "<a class=\"pageListLinkBold\" href=\"Address.Info.aspx?State=" + State;

            AddressList.Append(divTitle + "Address Details:</div>");

            if (parcelInfoList != null)
            {
                // Filter out null and duplicate entries
                List<ParcelInfoModel> filteredList = new List<ParcelInfoModel>();
                StringBuilder sbWork = new StringBuilder();
                rowCnt = 0;
                foreach (ParcelInfoModel parcelInfoModel in parcelInfoList)
                {
                    if (parcelInfoModel == null || parcelInfoModel.RAddressModel == null)
                    {
                        continue;
                    }
                    rowCnt++;
                    StringBuilder sbDupCheck = new StringBuilder();

                    sbDupCheck.Append("&AddressUI=" + parcelInfoModel.RAddressModel.UID);
                    sbDupCheck.Append("&AddressSeq=" + parcelInfoModel.RAddressModel.sourceNumber);
                    sbDupCheck.Append("&AddressRefId=" + parcelInfoModel.RAddressModel.refAddressId);
                    sbDupCheck.Append("&AddressText=" + parcelInfoModel.RAddressModel.addressDescription);
                    if (!sbWork.ToString().Contains(sbDupCheck.ToString()))
                    {
                        sbWork.Append("|" + sbDupCheck + "|");
                        filteredList.Add(parcelInfoModel);
                    }
                    if (rowCnt > 99)
                    {
                        break;
                    }
                }
                rowCnt = 0;
                foreach (ParcelInfoModel parcelInfoModel in filteredList)
                {
                    rowCnt++;
                    sbWork = new StringBuilder();
                    sbWork.Append(linkHTML);
                    sbWork.Append("&SearchMode=" + SearchMode);
                    sbWork.Append("&ResultPage=" + ResultPageNo);
                    sbWork.Append("&GlobalSearch=true");
                    sbWork.Append("&AddressUI=" + parcelInfoModel.RAddressModel.UID);
                    sbWork.Append("&AddressSeq=" + parcelInfoModel.RAddressModel.sourceNumber);
                    sbWork.Append("&AddressRefId=" + parcelInfoModel.RAddressModel.refAddressId);
                    sbWork.Append("&AddressText=" + parcelInfoModel.RAddressModel.addressDescription);
                    sbWork.Append("&CalledByOwnerInfo=true");
                    sbWork.Append("&OwnerFullName=" + ownerName);
                    sbWork.Append("&OwnerMailAddress=" + ownerMailAddr);
                    sbWork.Append("&OwnerUI=" + ownerUI);
                    sbWork.Append("&OwnerSeq=" + ownerSeq);
                    sbWork.Append("&OwnerNumber=" + ownerNumber);
                    sbWork.Append("\">");
                    sbWork.Append(GenerateAddressString(parcelInfoModel.RAddressModel));
                    AddressList.Append(MyProxy.CreateListCell(string.Empty, sbWork.ToString(), rowCnt - 1, rowCnt - 1, filteredList.Count, 0, 100, isiPhone, true).ToString());
                }
                // test error trapping
                // throw new Exception("just testing");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(ex.Message);
            ErrorMessage.Append(ErrorFormatEnd);
        }
    }

    /// <summary>
    /// Fill data for owner object
    /// </summary>
    private void LoadParcelView()
    {
        try
        {
            OwnerModel ownerPK = new OwnerModel();
            ownerPK.sourceSeqNumber = StringUtil.ToLong(ownerSeq);
            ownerPK.UID = ownerUI;
            ownerPK.ownerNumber = StringUtil.ToLong(ownerNumber);
            string callID = AppSession.User.UserID;

            ParcelModel[] parcelModels = ParcelService.getParcelListByOwnerPK(ConfigManager.AgencyCode, ownerPK, callID, null);

            string linkHTML = "<a class=\"pageListLinkBold\" href=\"Parcel.Info.aspx?State=" + State;

            ParcelList.Append(divTitle + "<b>Parcels:</b></div>");

            if (parcelModels != null)
            {
                // Filter out null and duplicate entries
                List<ParcelModel> filteredList = new List<ParcelModel>();
                StringBuilder sbWork = new StringBuilder();
                rowCnt = 0;
                foreach (ParcelModel parcelModel in parcelModels)
                {
                    if (parcelModel == null || parcelModel.parcelNumber == null)
                    {
                        continue;
                    }
                    rowCnt++;
                    StringBuilder sbDupCheck = new StringBuilder();

                    sbDupCheck.Append("&ParcelNumber=" + parcelModel.parcelNumber);
                    sbDupCheck.Append("&ParcelSequence=" + parcelModel.sourceSeqNumber);
                    sbDupCheck.Append("&SearchMode=" + SearchMode);
                    if (!sbWork.ToString().Contains(sbDupCheck.ToString()))
                    {
                        sbWork.Append("|" + sbDupCheck + "|");
                        filteredList.Add(parcelModel);
                    }
                    if (rowCnt > 99)
                    {
                        break;
                    }
                }
                rowCnt = 0;
                foreach (ParcelModel parcelModel in filteredList)
                {
                    rowCnt++;
                    sbWork = new StringBuilder();
                    sbWork.Append(linkHTML);
                    sbWork.Append("&ParcelNumber=" + parcelModel.parcelNumber);
                    sbWork.Append("&ParcelSequence=" + parcelModel.sourceSeqNumber);
                    sbWork.Append("&SearchMode=" + SearchMode);
                    sbWork.Append("&ResultPage=" + ResultPageNo);
                    sbWork.Append("&GlobalSearch=true");
                    sbWork.Append("&CalledByOwnerInfo=true");
                    sbWork.Append("&OwnerFullName=" + ownerName);
                    sbWork.Append("&OwnerMailAddress=" + ownerMailAddr);
                    sbWork.Append("&OwnerUI=" + ownerUI);
                    sbWork.Append("&OwnerSeq=" + ownerSeq);
                    sbWork.Append("&OwnerNumber=" + ownerNumber);
                    sbWork.Append("\">");
                    sbWork.Append(parcelModel.parcelNumber.ToString());
                    ParcelList.Append(MyProxy.CreateListCell(string.Empty, sbWork.ToString(), rowCnt - 1, rowCnt - 1, filteredList.Count, 0, 100, isiPhone, true).ToString());
                }
                // test error trapping
                // throw new Exception("just testing");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(ex.Message);
            ErrorMessage.Append(ErrorFormatEnd);
        }
    }

    /// <summary>
    /// Gets an instance of OwnerWebServiceService.
    /// </summary>
    private ParcelWebServiceService ParcelService
    {
        get
        {
            return WSFactory.Instance.GetWebService<ParcelWebServiceService>();
        }
    }

    /// <summary>
    /// Gets an instance of OwnerWebServiceService.
    /// </summary>
    private OwnerWebServiceService OwnerService
    {
        get
        {
            return WSFactory.Instance.GetWebService<OwnerWebServiceService>();
        }
    }

    /// <summary>
    /// Generate Address string from a specified RefAddressModel model.
    /// </summary>
    /// <param name="refAddressModel">a RefAddressModel model.</param>
    /// <returns>an assembled address string</returns>
    private string GenerateAddressString(RefAddressModel refAddressModel)
    {
        IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;
        return addressBuilderBll.BuildAddressByFormatType(refAddressModel, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
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
