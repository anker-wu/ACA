/**
* <pre>
* 
*  Accela Citizen Access
*  File: Address.Info.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
*  View address details.
* 
*  Notes:
*      $Id: Address.Info.aspx.cs 278215 2014-08-29 06:37:51Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-05-2009           Dave Brewster           New page added for version 7.0
* 
* </pre>
*/
using System;
using System.Data;
//using System.Configuration;
//using System.Collections;
using System.Collections.Generic;
//using System.Web;
//using System.Web.Security;
// System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
//using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
//using Accela.ACA.WSProxy.WSModel;


/// <summary>
/// 
/// </summary>
public partial class AddressInfo : AccelaPage
{
    private const string HTML_EMPTY = "&nbsp;";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder AddressDetail = new StringBuilder();
    public StringBuilder LegalDescription = new StringBuilder();
    public StringBuilder ParcelList = new StringBuilder();
    public StringBuilder OwnerList = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();

    public string AddressUId = string.Empty;
    public string AddressRefId = string.Empty;
    public string AddressSeqNo = string.Empty;
    public string ResultPageNo = string.Empty;
    public string PageTitle = string.Empty;
    private string SearchMode = string.Empty;

    private RefAddressModel addressModelPK = new RefAddressModel();
    private RefAddressModel addressModel = new RefAddressModel();
    private ParcelModel parcelModal = new ParcelModel();
    ParcelModel[] parcelModels = null;

    // private string divPageText = "<div id=\"pageTextIndented\">";
    private string divTitle = "<div id=\"pageSectionTitle\">";
    private int rowCnt = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("Address.Info.aspx");
        State = GetFieldValue("State", false);
        AddressUId = GetFieldValue("AddressUId", false);
        AddressRefId = GetFieldValue("AddressRefId", false);
        AddressSeqNo = GetFieldValue("AddressSeq", false);
        ResultPageNo = GetFieldValue("ResultPage", false);
        SearchMode = GetFieldValue("SearchMode", false);

        string ownerName = GetFieldValue("OwnerFullName", false);
        string ownerMailAddr = GetFieldValue("OwnerMailAddress", false);
        string ownerUI = GetFieldValue("OwnerUI", false);
        string ownerSeq = GetFieldValue("OwnerSeq", false);
        string ownerNumber = GetFieldValue("OwnerNumber", false);
        string ParcelNo = GetFieldValue("ParcelNumber", false);
        string ParcelSeqNo = GetFieldValue("ParcelSequence", false);
        bool isGlobalSearch = GetFieldValue("GlobalSearch", false) == "true";
        bool isCalledByParcelInfo = GetFieldValue("CalledByParcelInfo", false) == "true";
        bool isCalledByOwnerInfo = GetFieldValue("CalledByOwnerInfo", false) == "true";

        string per_globalsearch_label_apo = LocalGetTextByKey("per_globalsearch_label_apo");
        string per_globalsearch_label_searchresults = LocalGetTextByKey("per_globalsearch_label_searchresults");

        IRefAddressBll addressBll = (IRefAddressBll)ObjectFactory.GetObject(typeof(IRefAddressBll));
        try
        {
            addressModelPK.sourceNumber = StringUtil.ToInt(AddressSeqNo);
            addressModelPK.refAddressId = StringUtil.ToLong(AddressRefId);
            addressModelPK.UID = AddressUId;
            addressModel = addressBll.GetAddressByPK(ConfigManager.AgencyCode, addressModelPK);
            if (addressModel == null)
            {
                // the RefId adn SeqNo are switched in the GlobalSearchManager.cs routine that builds the
                // search results.
                addressModel = new RefAddressModel();
                addressModelPK.sourceNumber = StringUtil.ToInt(AddressRefId);
                addressModelPK.UID = AddressUId;
                addressModelPK.refAddressId = StringUtil.ToLong(AddressSeqNo);
                addressModel = addressBll.GetAddressByPK(ConfigManager.AgencyCode, addressModelPK);
            }
            iPhonePageTitle = "Address";
            PageTitle = "<div id=\"pageTitle\">" + GenerateAddressString(addressModel) + "</div>";

            if (addressModel != null)
            {
                addressModelPK.UID = (addressModel.UID != null) ? addressModel.UID : string.Empty;
                ParcelList.Append(divTitle + "<b>Parcels:</b></div>");
                LoadParcelView();
				
				//if standard choice is "N", not display owner section.
                if (StandardChoiceUtil.IsDisplayOwnerSection())
                {
                    OwnerList.Append(divTitle + "<b>Owners:</b></div>");
                    LoadOwnerView();
                }
            }
            // test error trapping
            // throw new Exception("just testing");
        }
        catch (Exception ex)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(ex.Message);
            ErrorMessage.Append(ErrorFormatEnd);
        }
        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&SearchMode=" + SearchMode.ToString());
        sbWork.Append("&ResultPage=" + ResultPageNo.ToString());
        sbWork.Append("&AddressRefId=" + AddressRefId);
        sbWork.Append("&AddressSeq=" + AddressSeqNo);
        sbWork.Append("&AddressUId=" + AddressUId);
        sbWork.Append("&GlobalSearch=true");
        string addressText = GenerateAddressString(addressModel);
        if (isCalledByParcelInfo == true)
        {
            sbWork.Append("&CalledByParcelInfo=true");
            sbWork.Append("&ParcelNumber=" + ParcelNo);
            sbWork.Append("&ParcelSequence=" + ParcelSeqNo);
        }
        else // called by Owner Info
        {
            sbWork.Append("&CalledByOwnerInfo=true");
            sbWork.Append("&OwnerFullName=" + ownerName);
            sbWork.Append("&OwnerMailAddress=" + ownerMailAddr);
            sbWork.Append("&OwnerUI=" + ownerUI);
            sbWork.Append("&OwnerSeq=" + ownerSeq);
            sbWork.Append("&OwnerNumber=" + ownerNumber);
        }
        if (addressText.Length > 20)
        {
            addressText = addressText.Substring(0, 18) + "...";
        }
        Breadcrumbs = BreadCrumbHelper("Address.Info.aspx", sbWork, addressText, breadCrumbIndex, isElipseLink, false, false, false);
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
    }

    /// <summary>
    /// Fill data for owner object
    /// </summary>
    private void LoadParcelView()
    {
        try
        {
            string callID = AppSession.User.PublicUserId;
            parcelModels = ParcelService.getParcelListByRefAddressPK(ConfigManager.AgencyCode, addressModelPK, callID, null);

            string linkHTML = "<a class=\"pageListLinkBold\" href=\"Parcel.Info.aspx?State=" + State;

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
                    sbWork.Append("&CalledByAddressInfo=true");
                    sbWork.Append("&AddressRefId=" + AddressRefId);
                    sbWork.Append("&AddressSeq=" + AddressSeqNo);
                    sbWork.Append("&AddressUId=" + AddressUId);
                    sbWork.Append("\">");
                    sbWork.Append(parcelModel.parcelNumber.ToString());
                    // ParcelList.Append(CreateListCell(sbWork.ToString(), rowCnt, filteredList.Count).ToString());
                    ParcelList.Append(MyProxy.CreateListCell(string.Empty, sbWork.ToString(), rowCnt - 1, rowCnt - 1, filteredList.Count, 0, 100, isiPhone, false).ToString());
                }
            }
            // test error trapping
            // throw new Exception("just testing");
        }
        catch  (Exception ex)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(ex.Message);
            ErrorMessage.Append(ErrorFormatEnd);
        }
    }

    /// <summary>
    /// Fill data for owner object
    /// </summary>
    private void LoadOwnerView()
    {
        if (parcelModels == null)
        {
            return;
        }
        try
        {
            SearchResultModel searchResult = APOService.getOwnerListByParcelPKs(ConfigManager.AgencyCode, parcelModels, false, null);
            OwnerModel[] ownerModels = ObjectConvertUtil.ConvertObjectArray2EntityArray<OwnerModel>(searchResult.resultList);

            string linkHTML = "<a  class=\"pageListLinkBold\" href=\"Owner.Info.aspx?State=" + State;
            if (ownerModels != null)
            {
                // Filter out null and duplicate entries
                List<OwnerModel> filteredList = new List<OwnerModel>();
                StringBuilder sbWork = new StringBuilder();
                rowCnt = 0;
                foreach (OwnerModel ownerModel in ownerModels)
                {
                    if (ownerModel == null || ownerModel.ownerFullName == null)
                    {
                        continue;
                    }
                    rowCnt++;
                    StringBuilder sbDupCheck = new StringBuilder();

                    sbDupCheck.Append("&OwnerUI=" + ownerModel.UID);
                    sbDupCheck.Append("&OwnerSeq=" + ownerModel.sourceSeqNumber);
                    sbDupCheck.Append("&OwnerNumber=" + ownerModel.ownerNumber);
                    sbDupCheck.Append("&SearchMode=" + SearchMode);
                    if (!sbWork.ToString().Contains(sbDupCheck.ToString()))
                    {
                        sbWork.Append("|" + sbDupCheck + "|");
                        filteredList.Add(ownerModel);
                    }
                }
                rowCnt = 0;
                foreach (OwnerModel ownerModel in filteredList)
                {
                    rowCnt++;
                    sbWork = new StringBuilder();
                    sbWork.Append(linkHTML);
                    sbWork.Append("&OwnerFullName=" + ownerModel.ownerFullName);
                    sbWork.Append("&OwnerMailAddress=" + ownerModel.mailAddress);
                    sbWork.Append("&OwnerUI=" + ownerModel.UID);
                    sbWork.Append("&OwnerSeq=" + ownerModel.sourceSeqNumber);
                    sbWork.Append("&OwnerNumber=" + ownerModel.ownerNumber);
                    sbWork.Append("&SearchMode=" + SearchMode);
                    sbWork.Append("&ResultPage=" + ResultPageNo);
                    sbWork.Append("&GlobalSearch=true");
                    sbWork.Append("&CalledByAddressInfo=true");
                    sbWork.Append("&AddressRefId=" + AddressRefId);
                    sbWork.Append("&AddressSeq=" + AddressSeqNo);
                    sbWork.Append("&AddressUId=" + AddressUId);
                    sbWork.Append("\">");
                    sbWork.Append(ownerModel.ownerFullName.ToString());
                    // OwnerList.Append(CreateListCell(sbWork.ToString(), rowCnt, filteredList.Count).ToString());
                    OwnerList.Append(MyProxy.CreateListCell(string.Empty, sbWork.ToString(), rowCnt - 1, rowCnt - 1, filteredList.Count, 0, 100, isiPhone, true).ToString());
                }
            }
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
    /// Gets an instance of ParcelWebServiceService.
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
    private APOWebServiceService APOService
    {
        get
        {
            return WSFactory.Instance.GetWebService<APOWebServiceService>();
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
