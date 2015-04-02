/**
* <pre>
* 
*  Accela Citizen Access
*  File: Parcel.Info.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
*  View Parcel details.
* 
*  Notes:
*      $Id: Parcel.Info 77905 2009-08-17 12:49:28Z  dave.brewster $.
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
using Accela.ACA.Common.Util;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;


/// <summary>
/// 
/// </summary>
public partial class ParcelInfo : AccelaPage
{
    private const string HTML_EMPTY = "&nbsp;";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder ParcelDetail = new StringBuilder();
    public StringBuilder LegalDescription = new StringBuilder();
    public StringBuilder AddressList = new StringBuilder();
    public StringBuilder OwnerList = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();

    public string ParcelNo = string.Empty;
    public string ParcelSeqNo = string.Empty;
    public string ResultPageNo = string.Empty;
    public string PageTitle = string.Empty;
    private string SearchMode = string.Empty;

    private ParcelModel parcelModal = new ParcelModel();
    private ParcelModel parcelPK = new ParcelModel();
    private string divPageText = "<div id=\"pageTextIndented\">";
    private string divTitle = "<div id=\"pageSectionTitle\">";
    private int rowCnt = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("Parcel.Info.aspx");
        State = GetFieldValue("State", false);
        ParcelNo = GetFieldValue("ParcelNumber", false);
        ParcelSeqNo = GetFieldValue("ParcelSequence", false);
        string ownerName = GetFieldValue("OwnerFullName", false);
        string ownerMailAddr = GetFieldValue("OwnerMailAddress", false);
        string ownerUI = GetFieldValue("OwnerUI", false);
        string ownerSeq = GetFieldValue("OwnerSeq", false);
        string ownerNumber = GetFieldValue("OwnerNumber", false);
        string addressRefId = GetFieldValue("AddressRefU=Id", false);
        string addressSeqNo = GetFieldValue("AddressSeq", false);
        ResultPageNo = GetFieldValue("ResultPage", false);
        SearchMode = GetFieldValue("SearchMode", false);
        bool isGlobalSearch = GetFieldValue("GlobalSearch", false) == "true";
        bool isCalledByOwnerInfo = GetFieldValue("CalledByOwnerInfo", false) == "true";
        bool isCallByAddressInfo = GetFieldValue("CallByAddressInfo", false) == "true";

        iPhonePageTitle = ParcelNo;
        if (isiPhone == true)
        {
            PageTitle = divTitle + "Parcel Details:</div>";
        }
        else
        {
            PageTitle = "<div id=\"pageTitle\">" + ParcelNo + "</div>"
                      + divTitle + "Parcel Details:</div>";
        }

        string per_globalsearch_label_apo = LocalGetTextByKey("per_globalsearch_label_apo");
        string per_globalsearch_label_searchresults = LocalGetTextByKey("per_globalsearch_label_searchresults");

        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&SearchMode=" + SearchMode.ToString());
        sbWork.Append("&ParcelNumber=" + ParcelNo.ToString());
        sbWork.Append("&ParcelSequence=" + ParcelSeqNo.ToString());
        sbWork.Append("&ResultPage=" + ResultPageNo.ToString());
        sbWork.Append("&GlobalSearch=true");
        if (isCallByAddressInfo == true)
        {
            sbWork.Append("&CalledByAddressInfo=true");
            sbWork.Append("&AddressRefId=" + addressRefId);
            sbWork.Append("&AddressSeq=" + addressSeqNo) ;
        }
        else if (isCalledByOwnerInfo == true)
        {
            sbWork.Append("&CalledByOwnerInfo=true");
            sbWork.Append("&OwnerFullName=" + ownerName);
            sbWork.Append("&OwnerMailAddress=" + ownerMailAddr);
            sbWork.Append("&OwnerUI=" + ownerUI);
            sbWork.Append("&OwnerSeq=" + ownerSeq);
            sbWork.Append("&OwnerNumber=" + ownerNumber);
        }
        Breadcrumbs = BreadCrumbHelper("Parcel.Info.aspx", sbWork, ParcelNo, breadCrumbIndex, isElipseLink, false, false, false);
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));

        try
        {
            parcelPK.UID = AppSession.User.PublicUserId;
            parcelPK.parcelNumber = ParcelNo;
            parcelPK.sourceSeqNumber = StringUtil.ToLong(ParcelSeqNo);

            IParcelBll parcelBll = (IParcelBll)ObjectFactory.GetObject(typeof(IParcelBll));
            parcelModal = parcelBll.GetParcelByPK(ConfigManager.AgencyCode, parcelPK);

            if (parcelModal != null && parcelModal.parcelNumber != null)
            {
                ParcelDetail.Append(divPageText);
                ParcelDetail.Append("<b>Number: </b>"+  parcelModal.parcelNumber.ToString());
                ParcelDetail.Append((parcelModal.lot == null) ? string.Empty : "; <b>Lot: </b>" + HTML_EMPTY + parcelModal.lot.ToString());
                ParcelDetail.Append((parcelModal.block == null) ? string.Empty : "; <br><b>Block: </b>" + HTML_EMPTY + parcelModal.block.ToString());
                ParcelDetail.Append((parcelModal.subdivision == null) ? string.Empty : "; <br><b>Subdivision: </b>" + HTML_EMPTY + parcelModal.subdivision.ToString());
                ParcelDetail.Append((parcelModal.book == null) ? string.Empty : "; <br><b>Book: </b>" +  HTML_EMPTY + parcelModal.book.ToString());
                ParcelDetail.Append((parcelModal.page == null) ? string.Empty : "; <br><b>Page: </b>" + HTML_EMPTY + parcelModal.page.ToString());
                ParcelDetail.Append("</div>");

                if (parcelModal.legalDesc != null)
                {
                    LegalDescription.Append(divTitle);
                    LegalDescription.Append("Legal Description:");
                    LegalDescription.Append("</div>");
                    LegalDescription.Append(divPageText);
                    LegalDescription.Append(parcelModal.legalDesc.ToString());
                    LegalDescription.Append("</div>");
                }
                if (parcelModal.tract != null)
                {
                    LegalDescription.Append(divTitle);
                    LegalDescription.Append("Tract:");
                    LegalDescription.Append("</div>");
                    LegalDescription.Append(divPageText);
                    LegalDescription.Append(parcelModal.tract.ToString());
                    LegalDescription.Append("</div>");
                }
                LoadAddressView();

				//if standard choice is "N" not display owner view.
                if (StandardChoiceUtil.IsDisplayOwnerSection())
                {
                    LoadOwnerView();
                }                
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
    /// fill data for address object
    /// </summary>
    private void LoadAddressView()
    {
        if (parcelModal == null)
        {
            return;
        }
        try
        {
            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            parcelInfo.parcelModel = parcelPK;

            SearchResultModel result = ParcelService.getAPOList(ConfigManager.AgencyCode, parcelInfo, true, null, false);
            ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);
            string linkHTML = "<a class=\"pageListLinkBold\" href=\"Address.Info.aspx?State=" + State;

            AddressList.Append(divTitle + "Address Details:</div>");

            if (parcelInfos != null)
            {
                // Filter out null and duplicate entries
                List<ParcelInfoModel> filteredList = new List<ParcelInfoModel>();
                StringBuilder sbWork = new StringBuilder();
                rowCnt = 0;
                foreach (ParcelInfoModel parcelInfoModel in parcelInfos)
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
                }
                rowCnt = 0;
                foreach (ParcelInfoModel parcelInfoModel in filteredList)
                {
                    rowCnt++;
                    sbWork = new StringBuilder();
                    sbWork.Append(linkHTML);
                    sbWork.Append("&AddressUI=" + parcelInfoModel.RAddressModel.UID);
                    sbWork.Append("&AddressSeq=" + parcelInfoModel.RAddressModel.sourceNumber);
                    sbWork.Append("&AddressRefId=" + parcelInfoModel.RAddressModel.refAddressId);
                    sbWork.Append("&AddressText=" + parcelInfoModel.RAddressModel.addressDescription);
                    sbWork.Append("&ParcelNumber=" + ParcelNo.ToString());
                    sbWork.Append("&ParcelSequence=" + ParcelSeqNo.ToString());
                    sbWork.Append("&SearchMode=" + SearchMode);
                    sbWork.Append("&ResultPage=" + ResultPageNo);
                    sbWork.Append("&GlobalSearch=true");
                    sbWork.Append("&CalledByParcelInfo=true");
                    sbWork.Append("\">");
                    sbWork.Append(GenerateAddressString(parcelInfoModel.RAddressModel));
                    // AddressList.Append(CreateListCell(sbWork.ToString(), rowCnt, filteredList.Count).ToString());
                    AddressList.Append(MyProxy.CreateListCell(string.Empty, sbWork.ToString(), rowCnt - 1, rowCnt - 1, filteredList.Count, 0, 100, isiPhone, true).ToString());
                }
                // test error trapping
                // throw new Exception("just testing 2");
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
    private void LoadOwnerView()
    {
        if (parcelModal == null)
        {
            return;
        }
        try
        {
            ParcelModel[] parcelPKArray = new ParcelModel[1];
            parcelPKArray[0] = parcelModal;
            SearchResultModel searchResult = APOService.getOwnerListByParcelPKs(ConfigManager.AgencyCode, parcelPKArray, false, null);
            OwnerModel[] ownerModels = ObjectConvertUtil.ConvertObjectArray2EntityArray<OwnerModel>(searchResult.resultList);

            string linkHTML = "<a  class=\"pageListLinkBold\" href=\"Owner.Info.aspx?State=" + State;
            OwnerList.Append(divTitle + "Owners:</div>");
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
                    if (ownerModel == null || ownerModel.ownerFullName == null)
                    {
                        continue;
                    }
                    rowCnt++;
                    sbWork = new StringBuilder();
                    sbWork.Append(linkHTML);
                    sbWork.Append("&OwnerUI=" + ownerModel.UID);
                    sbWork.Append("&OwnerSeq=" + ownerModel.sourceSeqNumber);
                    sbWork.Append("&OwnerNumber=" + ownerModel.ownerNumber);
                    sbWork.Append("&SearchMode=" + SearchMode);
                    sbWork.Append("&OwnerFullName=" + ownerModel.ownerFullName);
                    sbWork.Append("&OwnerMailAddress=" + ownerModel.mailAddress);
                    sbWork.Append("&ParcelNumber=" + ParcelNo.ToString());
                    sbWork.Append("&ParcelSequence=" + ParcelSeqNo.ToString());
                    sbWork.Append("&ResultPage=" + ResultPageNo);
                    sbWork.Append("&GlobalSearch=true");
                    sbWork.Append("&CalledByParcelInfo=true");
                    sbWork.Append("\">");
                    sbWork.Append(ownerModel.ownerFullName.ToString());
                    // OwnerList.Append(CreateListCell(sbWork.ToString(), rowCnt, filteredList.Count).ToString());
                    OwnerList.Append(MyProxy.CreateListCell(string.Empty, sbWork.ToString(), rowCnt - 1, rowCnt - 1, filteredList.Count, 0, 100, isiPhone, true).ToString());
                }
            }
            // test error trapping
            // throw new Exception("just testing 3");
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
