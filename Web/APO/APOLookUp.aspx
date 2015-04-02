<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" Inherits="Accela.ACA.Web.APO.APOLookUp"
    CodeBehind="APOLookUp.aspx.cs" %>

<%@ Register Src="../Component/RefAPOAddressList.ascx" TagName="RefAPOAddressList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/AddressLookupForm.ascx" TagName="AddressLookupForm" TagPrefix="ACA" %>
<%@ Register Src="~/Component/ParcelLookupForm.ascx" TagName="ParcelLookupForm" TagPrefix="ACA" %>
<%@ Register Src="~/Component/OwnerLookupForm.ascx" TagName="OwnerLookupForm" TagPrefix="ACA" %>
<%@ Register Src="~/Component/RefAddressLookUpList.ascx" TagName="RefAddressLookUpList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/RefParcelLookUpList.ascx" TagName="RefParcelLookUpList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/RefAPOOwnerList.ascx" TagName="OwnerList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/SearchResultInfo.ascx" TagName="SearchResultInfo" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) 
        {
            DoMask($get('<%=this.parcelLookupForm.ParcelNumberClientID %>'));
        }
        function DoMask(self)
        {
            if('True' != '<%=this.IsMasked.ToString() %>') return;
            if(' ' == "<%=this.ParcelIDMask %>") return;
            var mask1 = new AV360Mask("<%=this.ParcelIDMask%>");
            mask1.attach(self);
        }
        function doSomething(e)
        {
            if (!e) var e = window.event;
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();
         }  
         
         var prm = Sys.WebForms.PageRequestManager.getInstance();

         prm.add_initializeRequest(InitializeRequest);
         prm.add_endRequest(EndRequest);
         prm.add_beginRequest(beginRequest);
 
         function InitializeRequest(sender, args)
         {
             document.body.style.cursor = 'wait';
             var btn = document.getElementById('<%=this.btnNewSearch.ClientID %>');

             if(btn)
             {
                btn.style.cursor = 'wait';
             }       
         }

         function EndRequest(sender, args){
             document.body.style.cursor = '';
             var btn = document.getElementById('<%=this.btnNewSearch.ClientID %>');
             if(btn)
             {
                 btn.style.cursor = 'pointer';
             }

             if ($.global.isAdmin)
             {
                 invokeClick($get('<%=ddlSearchType.ClientID %>')); 
             }
             //export file.
             ExportCSV(sender, args)
         }
        
        function beginRequest()
        {
            prm._scrollPosition = null;
        }
        
    </script>

    <div id="MainContent" class="ACA_Content">
        <iframe width="0" height="0" id="iframeExport" style="visibility:hidden" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
        <div id="resUser" runat="server">

            <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/masked.js") %>"></script>

            <div>
                <h1>
                    <ACA:AccelaLabel LabelKey="APO_Search_Label_Title" ID="lblAPO_Search_Label_Title"
                        runat="server"></ACA:AccelaLabel></h1>
            </div>
            <p>
            </p>
        </div>
        <div class="ACA_Page ACA_Page_FontSize">
            <ACA:AccelaLabel LabelKey="APO_Search_Label_Introduce" ID="lblAPO_Search_Label_Introduce"
                runat="server" LabelType="bodyText"></ACA:AccelaLabel>
        </div>
        <div>
            <asp:UpdatePanel ID="updatePanel" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <a name="SearchForm_Start" id="SearchForm_Start"></a>
                    <div class="ACA_InfoTitle ACA_InfoTitle_FontSize">
                        <h1>
                            <%--below div is used to add a calendar control to initial javascript, otherwise an error will raise in Safari broswer--%>
                            <div id="divDate" style="width:0px; height:0px; display:none">
                                <ACA:AccelaCalendarText ID="AccelaCalendarText1" CssClass="ACA_NShot"
                                                runat="server" title="This is a hidden field."></ACA:AccelaCalendarText>
                            </div>  
                            <%--end --%>  
                            <span class="ACA_FLeft" ID="lblSelectedSearchType" runat="server"></span>
                        </h1>
                        <span class="ACA_FRight">
                            <ACA:AccelaDropDownList ID="ddlSearchType" runat="server" OnSelectedIndexChanged="SearchTypeDropdown_IndexChanged"
                                AutoPostBack="true" SourceType="HardCode" ToolTipLabelKey="aca_common_msg_dropdown_changesearchoption_tip">
                            </ACA:AccelaDropDownList>
                        </span>
                    </div>
                    <div id="divInstruction" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize" runat="server">
                            <ACA:AccelaLabel ID="lblSearchInstruction" runat="server" IsNeedEncode="false"></ACA:AccelaLabel>
                    </div>
                    
                    <div id="divSearchPaneltoPressEnter">
                    <!--Look Up By Address begin -->
                    <div id="dvSearchByAddress" runat="server" visible="false">
                         <ACA:AddressLookupForm  ID = "addressLookupForm" runat="Server" />
                    </div>
                    <!--Look Up By Address end -->
                    <!--Look Up By Parcel begin -->
                    <div id="dvSearchByParcel" runat="server" visible="false">
                          <ACA:ParcelLookupForm  ID = "parcelLookupForm" runat="Server" />
                    </div>
                    <!--Look Up By Parcel end -->
                    <!--Look Up By Owner begin -->
                    <div id="dvSearchByOwner" runat="server" visible="false">
                         <ACA:OwnerLookupForm  ID = "ownerLookupForm" runat="Server" />
                    </div>
                    <!--Look Up By Owner end -->
                    <!--Look Up By Permit begin -->
                    <div id="dvSearchByPermit" runat="server" visible="false">
                        <div class="ACA_TabRow">
                            <table role='presentation' class='ACA_TDAlignLeftOrRightTop collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtAPO_Search_by_Permit_PermitNumber" runat="server" LabelKey="APO_Search_by_Permit_PermitNumber"
                                            CssClass="ACA_NLonger" MaxLength="70"></ACA:AccelaTextBox>
                                    </td>
                                    <td>
                                        <ACA:AccelaDropDownList ID="ddlModule" runat="server" LabelKey="apo_search_by_permit_module"
                                            OnSelectedIndexChanged="ModuleDropdown_IndexChanged" AutoPostBack="true">
                                        </ACA:AccelaDropDownList>
                                    </td>
                                    <td>
                                        <ACA:AccelaDropDownList ID="ddlAPO_Search_by_Permit_Permit" runat="server" LabelKey="APO_Search_by_Permit_Permit">
                                        </ACA:AccelaDropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" runat="server" id="dvProjectName">
                            <table role='presentation' class='ACA_TDAlignLeftOrRightTop collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtAPO_Search_by_Permit_ProjectName" runat="server" LabelKey="APO_Search_by_Permit_ProjectName"
                                            CssClass="ACA_NLonger" MaxLength="255"></ACA:AccelaTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow">
                            <table role='presentation' class='ACA_TDAlignLeftOrRightTop collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaCalendarText ID="txtAPO_Search_by_Permit_StartDate" CssClass="ACA_NShot"
                                            runat="server" LabelKey="APO_Search_by_Permit_StartDate"></ACA:AccelaCalendarText>
                                    </td>
                                    <td>
                                        <ACA:AccelaCalendarText ID="txtAPO_Search_by_Permit_EndDate" runat="server" LabelKey="APO_Search_by_Permit_EndDate"
                                            CssClass="ACA_NShot"></ACA:AccelaCalendarText>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <!--Look Up By Permit end-->
                    <!--Look Up By Licensee begin -->
                    <div id="dvSearchByLicensee" runat="server">
                        <div class="ACA_TabRow" id="divLicenseType" runat="server">
                            <table role='presentation' class="ACA_TDAlignLeftOrRightTop collapse_table">
                                <tr>
                                    <td>
                                        <ACA:AccelaDropDownList ID="ddlLicenseType" runat="server" LabelKey="aca_apo_licensee_licenseType">
                                        </ACA:AccelaDropDownList>
                                    </td>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtLicenseNumber" runat="server" LabelKey="aca_apo_licensee_stateLicenseNum"
                                            CssClass="ACA_NLonger" MaxLength="30"></ACA:AccelaTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divContactType" runat="server">
                            <table role='presentation' class='collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaDropDownList ID="ddlContactType" runat="server" LabelKey="aca_apo_licensee_contacttype">
                                        </ACA:AccelaDropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divSSN" runat="server">
                            <table role='presentation' class="ACA_TDAlignLeftOrRightTop collapse_table">
                                <tr>
                                    <td>
                                        <ACA:AccelaSSNText ID="txtSSN" MaxLength="11" LabelKey="aca_apo_licensee_ssn" runat="server"
                                            IsIgnoreValidate="true"></ACA:AccelaSSNText>
                                    </td>
                                    <td>
                                        <ACA:AccelaFeinText ID="txtFEIN" MaxLength="11" IsIgnoreValidate="true" LabelKey="aca_apo_licensee_fein" runat="server"></ACA:AccelaFeinText>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divBusinessName1" runat="server">
                            <table role='presentation' class="ACA_TDAlignLeftOrRightTop collapse_table">
                                <tr>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtBusiName" runat="server" LabelKey="aca_apo_licensee_businessname"
                                            CssClass="ACA_XLong" MaxLength="255"></ACA:AccelaTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divBusinessName2" runat="server">
                            <table role='presentation' class="ACA_TDAlignLeftOrRightTop collapse_table">
                                <tr>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtBusiName2" runat="server" LabelKey="aca_apo_licensee_businessname2"
                                            CssClass="ACA_XLong" MaxLength="255"></ACA:AccelaTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divBusinessLicense" runat="server">
                            <table role='presentation' class="ACA_TDAlignLeftOrRightTop collapse_table">
                                <tr>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtBusiLicense" runat="server" LabelKey="aca_apo_licensee_businesslicense"
                                            CssClass="ACA_NLong" MaxLength="15"></ACA:AccelaTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divCountry" runat="server">
                            <table role='presentation' class='collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaCountryDropDownList ID="ddlCountry" runat="server" LabelKey="aca_apo_licensee_country">
                                        </ACA:AccelaCountryDropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divLicenseName" runat="server">
                            <table role='presentation' class="ACA_TDAlignLeftOrRightTop collapse_table" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtFirstName" runat="server" LabelKey="aca_apo_licensee_firstname"
                                            CssClass="ACA_NLong" MaxLength="70"></ACA:AccelaTextBox>
                                    </td>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtMiddleInitial" runat="server" LabelKey="aca_apo_licensee_middleinitial"
                                            CssClass="ACA_NShot" MaxLength="70"></ACA:AccelaTextBox>
                                    </td>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtLastName" runat="server" LabelKey="aca_apo_licensee_lastname"
                                            CssClass="ACA_NLong" MaxLength="70"></ACA:AccelaTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divAddress1" runat="server">
                            <table role='presentation' class='ACA_TDAlignLeftOrRightTop collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtAddress1" MaxLength="200" runat="server" CssClass="ACA_MLonger"
                                            LabelKey="aca_apo_licensee_address1"></ACA:AccelaTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divAddress2" runat="server">
                            <table role='presentation' class='ACA_TDAlignLeftOrRightTop collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtAddress2" MaxLength="200" runat="server" CssClass="ACA_MLonger"
                                            LabelKey="aca_apo_licensee_address2"></ACA:AccelaTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divAddress3" runat="server">
                            <table role='presentation' class='ACA_TDAlignLeftOrRightTop collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtAddress3" MaxLength="200" runat="server" CssClass="ACA_MLonger"
                                            LabelKey="aca_apo_licensee_address3"></ACA:AccelaTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divState" runat="server">
                            <table role='presentation' class='ACA_TDAlignLeftOrRightTop collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtCity" MaxLength="30" runat="server" CssClass="ACA_NLong"
                                            LabelKey="aca_apo_licensee_city" AutoFillType="City" PositionID="LookUpAPOByLicenseeCity"></ACA:AccelaTextBox>
                                    </td>
                                    <td>
                                        <ACA:AccelaStateControl ID="txtState" runat="server" LabelKey="aca_apo_licensee_state"
                                            AutoFillType="State" PositionID="LookUpAPOByLicenseeState" />
                                    </td>
                                    <td>
                                        <ACA:AccelaZipText ID="txtZipCode" runat="server" IsIgnoreValidate="true" CssClass="ACA_Medium"
                                            LabelKey="aca_apo_licensee_zip"></ACA:AccelaZipText>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divPhone" runat="server">
                            <table role='presentation' class='ACA_TDAlignLeftOrRightTop collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaPhoneText ID="txtPhone1" runat="server" LabelKey="aca_apo_licensee_phone1"
                                            CssClass="ACA_NLong" IsIgnoreValidate="true"></ACA:AccelaPhoneText>
                                    </td>
                                    <td>
                                        <ACA:AccelaPhoneText ID="txtPhone2" runat="server" LabelKey="aca_apo_licensee_phone2"
                                            CssClass="ACA_NLong" IsIgnoreValidate="true"></ACA:AccelaPhoneText>
                                    </td>
                                    <td>
                                        <ACA:AccelaPhoneText ID="txtFax" runat="server" LabelKey="aca_apo_licensee_fax" CssClass="ACA_NLong"
                                            IsIgnoreValidate="true"></ACA:AccelaPhoneText>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_TabRow" id="divContractorLicNO" runat="server">
                            <table role='presentation' class='ACA_TDAlignLeftOrRightTop collapse_table'>
                                <tr>
                                    <td>
                                        <ACA:AccelaNumberText ID="txtContractorLicNO" LabelKey="aca_apo_licensee_contractorlicno"
                                            CssClass="ACA_Small" runat="server" MaxLength="8" IsNeedDot="false" />
                                    </td>
                                    <td>
                                        <ACA:AccelaTextBox ID="txtContractorBusiName" runat="server" LabelKey="aca_apo_licensee_contractorbusiname"
                                            CssClass="ACA_NLonger" MaxLength="65"></ACA:AccelaTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <!--Look Up By Parcel end -->
                    </div>

                    <div class="ACA_TabRow"> &nbsp;</div>
                    <div class="ACA_Row ACA_LiLeft">
                        <div>
                           <ul>                      
                                <li>
                                    <ACA:AccelaButton ID="btnNewSearch" runat="server" OnClick="NewSearchButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" LabelKey="APO_Search_by_Address_SearchButton"></ACA:AccelaButton>
                                </li>
                                <li>
                                    <ACA:AccelaButton ID="btnResetSearch" runat="server" OnClick="ResetSearchButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" CausesValidation="false" LabelKey="aca_aposearch_label_resetsearch"></ACA:AccelaButton>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <a name="PageResult" id="PageResult" href="#PageResult" tabindex="-1"></a>
                    <%--the fellowed is permitlist result--%>
                    <div id="divPageResult" runat="server" visible="false">
                        <div id="divPagePermitListResult" runat="server" visible="false">
                            <ACA:SearchResultInfo ID="RecordSearchResultInfo" runat="server" CountSummaryLabelKey="per_permitList_label_Count_For_Permit" PromptLabelKey="per_permitList_label_Click_Prompt"/>
                            <div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="divResultPermitList" runat="server" visible="true">
                                <ACA:AccelaGridView ID="dgvPermitList" runat="server" HeaderStyle-CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize"
                                    AlternatingRowStyle-CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize" RowStyle-CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize"
                                    AllowPaging="true" AllowSorting="true" ShowCaption="true" Visible="false" AutoGenerateColumns="false"
                                    PagerStyle-HorizontalAlign="center" OnRowCommand="PermitList_RowCommand" OnRowDataBound="PermitList_RowDataBound"
                                    GridViewNumber="60034" ShowExportLink="true" OnGridViewSort="PermitList_GridViewSort" OnGridViewDownload="PermitList_GridViewDownload"
                                    OnPageIndexChanging="PermitList_PageIndexChanging" SummaryKey="gdv_apo_permitlist_summary" CaptionKey="aca_caption_apo_permitlist">
                                    <Columns>
                                        <ACA:AccelaTemplateField AttributeName="lnkDateHeader" ExportDataField="Date" ExportFormat="ShortDate">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                        <ACA:GridViewHeaderLabel ID="lnkDateHeader" runat="server" CommandName="Header" SortExpression="Date"
                                                            LabelKey="APO_Search_by_Permit_PermitResult_Date">
                                                        </ACA:GridViewHeaderLabel>
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaDateLabel ID="lblUpdatedTime" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "Date")%>'></ACA:AccelaDateLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                            <headerstyle Width="100px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkPermitNumberHeader" ExportDataField="PermitNumber">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                        <ACA:GridViewHeaderLabel ID="lnkPermitNumberHeader" runat="server" CommandName="Header"
                                                            SortExpression="PermitNumber" LabelKey="APO_Search_by_Permit_PermitResult_PermitNumber">
                                                        </ACA:GridViewHeaderLabel>
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <strong>
                                                        <ACA:AccelaLabel ID="lblPermitNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PermitNumber") %>'></ACA:AccelaLabel></strong>
                                                    <asp:LinkButton ID="hlPermitNumber" runat="server" CommandName="DisplayAPO"><strong><%# DataBinder.Eval(Container.DataItem, "PermitNumber") %></strong></asp:LinkButton>
                                                    <ACA:AccelaLabel ID="lblCapID1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "capID1") %>'
                                                        Visible="false"></ACA:AccelaLabel>
                                                    <ACA:AccelaLabel ID="lblCapID2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "capID2") %>'
                                                        Visible="false"></ACA:AccelaLabel>
                                                    <ACA:AccelaLabel ID="lblCapID3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "capID3") %>'
                                                        Visible="false"></ACA:AccelaLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                            <headerstyle Width="130px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkPermitTypeHeader" ExportDataField="PermitType">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                        <ACA:GridViewHeaderLabel ID="lnkPermitTypeHeader" runat="server" CommandName="Header"
                                                            SortExpression="PermitType" LabelKey="APO_Search_by_Permit_PermitResult_PermitType">
                                                        </ACA:GridViewHeaderLabel>
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PermitType") %>'></ACA:AccelaLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                            <headerstyle Width="130px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkPermitSearchProjectNameHeader" ExportDataField="ProjectName">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                        <ACA:GridViewHeaderLabel ID="lnkPermitSearchProjectNameHeader" runat="server" CommandName="Header"
                                                            SortExpression="ProjectName" LabelKey="per_permitList_label_permitSearchProjectName">
                                                        </ACA:GridViewHeaderLabel>
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%--use AccelaLabel control instead of asp.Label control, updated by Peter.Pan --%>
                                                    <ACA:AccelaLabel ID="lblProjectName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectName") %>'>
                                                    </ACA:AccelaLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                            <headerstyle Width="130px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkRelatedRecordsHeader" ExportDataField="RelatedRecords">
                                        <HeaderTemplate>
                                            <div class="ACA_CapListStyle ACA_Header_Row">
                                                <ACA:GridViewHeaderLabel ID="lnkRelatedRecordsHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="RelatedRecords"
                                                    LabelKey="per_permitlist_label_relatedrecords"></ACA:GridViewHeaderLabel>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="ACA_CapListStyle">
                                                <asp:Literal ID="litRelatedRecords" runat="server"></asp:Literal>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="110px"/>
                                        <headerstyle Width="110px"/>
                                    </ACA:AccelaTemplateField>
                                    </Columns>
                                </ACA:AccelaGridView>
                            </div>
                        </div>
                        <div id="divLicenseeResult"  runat="server" visible="false">
                            <ACA:SearchResultInfo ID="LPSearchResultInfo" runat="server" CountSummaryLabelKey="per_permitList_label_Count_For_Permit" PromptLabelKey="per_permitList_label_Click_Prompt"/>    
                            <div class="ACA_TabRow" id="divResultLicenseeList"
                                runat="server" visible="true">
                                <ACA:AccelaGridView ID="gdvLicenseeList" GridViewNumber="60095" runat="server" AllowPaging="True"
                                    AllowSorting="true" AutoGenerateColumns="False" PageSize="10"
                                    ShowExportLink="true" ShowCaption="true" PagerStyle-HorizontalAlign="center"
                                    PagerStyle-VerticalAlign="bottom" OnGridViewSort="LicenseeList_GridViewSort"
                                    OnRowCommand="LicenseeList_RowCommand" OnPageIndexChanging="LicenseeList_PageIndexChanging"
                                    OnRowDataBound="LicenseeList_RowDataBound" SummaryKey="gdv_apo_licenselist_summary" CaptionKey="aca_caption_apo_licenselist"
                                    OnGridViewDownload="LicenseeList_GridViewDownload">
                                    <Columns>
                                        <ACA:AccelaTemplateField AttributeName="lnkRefLicenseNbrHeader" ExportDataField="licenseNbr">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                        <ACA:GridViewHeaderLabel ID="lnkRefLicenseNbrHeader" runat="server" CommandName="Header"
                                                            SortExpression="licenseNbr" LabelKey="aca_apo_licenseelist_license_number" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaLinkButton ID="lnkLicenseRefNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"licenseNbr") %>'
                                                        CausesValidation="false" CommandName="selectedLicensee" CommandArgument='<%#Container.DataItemIndex%>'></ACA:AccelaLinkButton>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                            <headerstyle Width="130px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkLicenseTypeHeader" ExportDataField="resLicenseType">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                        <ACA:GridViewHeaderLabel ID="lnkLicenseTypeHeader" runat="server" CommandName="Header"
                                                            SortExpression="licenseType" LabelKey="aca_apo_licenseelist_license_type" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaLabel ID="lblLicenseType" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "resLicenseType")) %>'></ACA:AccelaLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                            <headerstyle Width="130px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkContactTypeHeader" ExportDataField="typeFlag">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                    <ACA:GridViewHeaderLabel ID="lnkContactTypeHeader" runat="server" CommandName="Header"
                                                        SortExpression="typeFlag" LabelKey="aca_apo_licenseelist_license_contacttype"
                                                        CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaLabel ID="lblContactType" runat="server" Text='<%# Accela.ACA.Web.Common.DropDownListBindUtil.GetTypeFlagTextByValue(Convert.ToString(DataBinder.Eval(Container.DataItem, "typeFlag"))) %>'></ACA:AccelaLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                            <headerstyle Width="100px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkSSNHeader" ExportDataField="socialSecurityNumber" ExportFormat="SSN">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                    <ACA:GridViewHeaderLabel ID="lnkSSNHeader" runat="server" CommandName="Header" SortExpression="maskedSsn"
                                                        LabelKey="aca_apo_licenseelist_license_ssn" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaLabel ID="lblSSN" runat="server" Text='<%# Accela.ACA.Common.Util.MaskUtil.FormatSSNShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "socialSecurityNumber"))) %>'></ACA:AccelaLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                            <headerstyle Width="100px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkFEINHeader" ExportDataField="fein" ExportFormat="FEIN">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                    <ACA:GridViewHeaderLabel ID="lnkFEINHeader" runat="server" CommandName="Header" SortExpression="fein"
                                                        LabelKey="aca_apo_licenseelist_license_fein" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaLabel ID="lblFEIN" runat="server" Text='<%#Accela.ACA.Common.Util.MaskUtil.FormatFEINShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "fein")), StandardChoiceUtil.IsEnableFeinMasking())%>'></ACA:AccelaLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                            <headerstyle Width="100px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkBusinessNameHeader" ExportDataField="businessName">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                        <ACA:GridViewHeaderLabel ID="lnkBusinessNameHeader" runat="server" CommandName="Header"
                                                            SortExpression="businessName" LabelKey="aca_apo_licenseelist_license_businessname"
                                                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaLabel ID="lblBusinessName" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "businessName")) %>'></ACA:AccelaLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                            <headerstyle Width="130px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkBusinessLicenseHeader" ExportDataField="businessLicense">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                        <ACA:GridViewHeaderLabel ID="lnkBusinessLicenseHeader" runat="server" CommandName="Header"
                                                            SortExpression="businessLicense" LabelKey="aca_apo_licenseelist_license_businesslicense"
                                                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaLabel ID="lblBusinessLicense" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "businessLicense")) %>'></ACA:AccelaLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="200px" />
                                            <headerstyle Width="200px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkLicenseNameHeader" ExportDataField="contactName">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                        <ACA:GridViewHeaderLabel ID="lnkLicenseNameHeader" runat="server" CommandName="Header"
                                                            SortExpression="contactName" LabelKey="aca_apo_licenseelist_license_name" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaLabel ID="lblLicenseName" runat="server" Text='<%#Convert.ToString(DataBinder.Eval(Container.DataItem, "contactName"))%>'></ACA:AccelaLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                            <headerstyle Width="130px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkAddressHeader" ExportDataField="address1">
                                            <HeaderTemplate>
                                                <div class="ACA_Header_Row">
                                                        <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" SortExpression="address1"
                                                            LabelKey="aca_apo_licenseelist_license_address"></ACA:GridViewHeaderLabel>
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaLabel ID="lblAddress" runat="server" Text='<%#Convert.ToString(DataBinder.Eval(Container.DataItem, "address1"))%>' />
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                            <headerstyle Width="130px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkLicenseExpirationDateHeader" ExportDataField="licenseExpirDate" ExportFormat="ShortDate">
                                            <HeaderTemplate>
                                                <div>
                                                    <ACA:GridViewHeaderLabel ID="lnkLicenseExpirationDateHeader" runat="server" SortExpression="licenseExpirDate"
                                                        LabelKey="aca_apo_licenseelist_license_expirationdate"></ACA:GridViewHeaderLabel>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaDateLabel ID="lblLicExpDate" DateType="ShortDate" runat="server" Text2='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "licenseExpirDate"))%>'></ACA:AccelaDateLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                            <headerstyle Width="100px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkLicenseIssueDateHeader" ExportDataField="licesnseOrigIssueDate" ExportFormat="ShortDate">
                                            <HeaderTemplate>
                                                <div>
                                                    <ACA:GridViewHeaderLabel ID="lnkLicenseIssueDateHeader" runat="server" SortExpression="licesnseOrigIssueDate"
                                                        LabelKey="aca_apo_licenseelist_license_issuedate"></ACA:GridViewHeaderLabel>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaDateLabel ID="lblLicIssueDate" DateType="ShortDate" runat="server" Text2='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "licesnseOrigIssueDate"))%>'>
                                                    </ACA:AccelaDateLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                            <headerstyle Width="100px" />
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkLicenseLastRenewalDateHeader" ExportDataField="lastRenewalDate" ExportFormat="ShortDate">
                                            <HeaderTemplate>
                                                <div>
                                                    <ACA:GridViewHeaderLabel ID="lnkLicenseLastRenewalDateHeader" runat="server" SortExpression="lastRenewalDate"
                                                        LabelKey="aca_apo_licenseelist_license_renewaldate"></ACA:GridViewHeaderLabel>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <ACA:AccelaDateLabel ID="lblLicRenewalDate" DateType="ShortDate" runat="server" Text2='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "lastRenewalDate"))%>'>
                                                    </ACA:AccelaDateLabel>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                            <headerstyle Width="100px" />
                                        </ACA:AccelaTemplateField>
                                    </Columns>
                                </ACA:AccelaGridView>
                            </div>
                        </div>
                        <a name="APOResult" id="APOResult" href="#APOResult" tabindex="-1"></a>
                        <%--the fellowed is APO result list--%>
                        <div id="divAPOResult" runat="server">
                            <ACA:SearchResultInfo ID="APOSearchResultInfo" runat="server" CountSummaryLabelKey="APO_Search_by_Address_ResultCount" PromptLabelKey="per_permitList_label_Click_Prompt"/>
                            <ACA:RefAPOAddressList ID="RefAPOAddressList1" GViewID="60047" runat="server" OnPageIndexChanging="RefAPO_GridViewIndexChanging"
                                OnGridViewSort="RefAPO_GridViewSort"/>
                            <ACA:RefAddressLookUpList ID="RefAddressLookUpList" GViewID="60183" runat="server" ShowExportLink="true" 
                                OnPageIndexChanging="RefAPO_GridViewIndexChanging" OnGridViewSort="RefAPO_GridViewSort"/>
                            <ACA:RefParcelLookUpList ID="RefParcelLookUpList" GViewID="60184" Visible="false" runat="server" ShowExportLink="true"
                                OnPageIndexChanging="RefAPO_GridViewIndexChanging" OnGridViewSort="RefAPO_GridViewSort"/>
                            <ACA:OwnerList ID="RefOwnerLookUpList" Visible="false" runat="server" GridViewNumber="60193" ShowExportLink="true"
                            OnPageIndexChanging="RefAPO_GridViewIndexChanging" OnGridViewSort="RefAPO_GridViewSort"/>
                            <br />                             
                            <asp:UpdatePanel runat="server" ID="UpdatePanelForParcel" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divAssociatedParcel" runat="server" class="ACA_FLeft" style="clear: both;" Visible="False">
                                        <ACA:AccelaLabel LabelKey="aca_apo_label_addresslist_associatedparcel" ID="lblRefParcelList" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                                        <ACA:RefParcelLookUpList ID="AssociatedParcelList" GViewID="60191" IsShowMap="false" runat="server" ShowExportLink="True" 
                                            OnPageIndexChanging="AssociatedParcel_GridViewIndexChanging" OnGridViewSort="AssociatedParcel_GridViewSort"/>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel runat="server" ID="UpdatePanelForAddress" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divAssociatedAddress" runat="server" class="ACA_FLeft" style=" clear: both;" Visible="False">                        
                                        <ACA:AccelaLabel LabelKey="aca_apo_label_parcellist_associatedaddress" ID="lblRefAddressList" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                                        <ACA:RefAddressLookUpList ID="AssociatedAddressList" GViewID="60190" IsShowMap="false" runat="server" ShowExportLink="True" 
                                            OnPageIndexChanging="AssociatedAddress_GridViewIndexChanging" OnGridViewSort="AssociatedAddress_GridViewSort" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel runat="server" ID="UpdatePanelForOwner" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divAssociatedOwner" runat="server" class="ACA_FLeft" Visible="False">                       
                                        <ACA:AccelaLabel LabelKey="aca_apo_label_parcellist_associatedowner" ID="lblParcelAssociatedOwnerList" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                                        <ACA:OwnerList ID="AssociatedOwnerList" runat="server" ShowExportLink="True"  OnPageIndexChanging="AssociatedOwner_GridViewIndexChanging" OnGridViewSort="AssociatedOwner_GridViewSort"/>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel runat="server" ID="UpdateParcelForAPOList" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divAssociatedAPOList" runat="server" class="ACA_FLeft" Visible="False">                       
                                        <ACA:AccelaLabel LabelKey="aca_apo_label_ownerlist_associateddata" ID="lblOwnerAssociatedAPOList" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                                        <ACA:RefAPOAddressList ID="AssociatedAPOList" runat="server" GViewID="60194" IsShowMap="false" ShowExportLink="True"  OnPageIndexChanging="AssociatedAPOList_GridViewIndexChanging" OnGridViewSort="AssociatedAPOList_GridViewSort"/>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <%--dvPageResult--%>
                    
                    <ACA:AccelaInlineScript ID="AccelaInlineScript1" runat="server">
                        <script type="text/javascript">
                            $("#divSearchPaneltoPressEnter").keydown(function (event) {
                                pressEnter2TriggerClick(event, $("#<%=btnNewSearch.ClientID %>"));
                            });
                        </script>
                    </ACA:AccelaInlineScript>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (typeof (myValidationErrorPanel) != "undefined") {
                myValidationErrorPanel.registerIDs4Recheck("<%=ddlSearchType.ClientID %>");
                myValidationErrorPanel.registerIDs4Recheck("<%=btnResetSearch.ClientID %>");
            }
        });

        var prefix = "#ctl00_PlaceHolderMain_";
        if ($.global.isAdmin) {
            HideAllSections();
            $(prefix + "dvSearchByAddress").show();
            $get("<%=ddlSearchType.ClientID %>").SectionInfo = new KeyValuePair();
            $get("<%=lblSearchInstruction.ClientID%>").labelKey = "apo_lookup_instruction_searchbyaddress";
        }

        function HideAllSections() {
            $(prefix + "dvSearchByAddress").hide();
            $(prefix + "dvSearchByParcel").hide();
            $(prefix + "dvSearchByOwner").hide();
            $(prefix + "dvSearchByPermit").hide();
            $(prefix + "dvSearchByLicensee").hide();
        }

        function ChangeType(ddl) {
            HideAllSections();
            var divId;
            var sectionId;
            var pf = '';
            var myArray = ddl.value.split("||");
            var str = myArray[myArray.length - 1];
            $get("<%=lblSelectedSearchType.ClientID%>").innerHTML = ddl[ddl.selectedIndex].text;
            var selectedSection = myArray[0].replace("-", "");
            var lblInstruction = $get("<%=lblSearchInstruction.ClientID%>");

            switch (selectedSection) {
                case '0':
                    divId = "dvSearchByAddress";
                    sectionId = "60011";
                    pf = 'addressLookupForm_';
                    lblInstruction.labelKey = 'apo_lookup_instruction_searchbyaddress';
                    break;
                case '1':
                    divId = "dvSearchByParcel";
                    sectionId = "60012";
                    pf = 'parcelLookupForm_';
                    lblInstruction.labelKey = 'apo_lookup_instruction_searchbyparcel';
                    break;
                case '2':
                    divId = "dvSearchByOwner";
                    sectionId = "60013";
                    pf = 'ownerLookupForm_';
                    lblInstruction.labelKey = 'apo_lookup_instruction_searchbyowner';
                    break;
                case '3':
                    divId = "dvSearchByPermit";
                    sectionId = "60014";
                    lblInstruction.labelKey = 'apo_lookup_instruction_searchbypermit';
                    break;
                case '4':
                    divId = "dvSearchByLicensee";
                    sectionId = "60094";
                    lblInstruction.labelKey = 'apo_lookup_instruction_searchbylicensee';
                    break;
            }
            $(prefix + divId).show();
            ddl.SectionInfo.Add(ddl.parentObj._sectionIdValue, ddl.parentObj._sectionFields);
            var sectionIdValue = "<%= ConfigManager.AgencyCode%>" + "\f" + sectionId + "\fctl00_PlaceHolderMain_" + pf;
            ddl.parentObj._sectionFields = ddl.SectionInfo.GetByKey(sectionIdValue);
            ddl.parentObj._sectionIdValue = sectionIdValue;

            var storedInstruction = ddl.SectionInfo.GetByKey(lblInstruction.labelKey);
            if (!storedInstruction) {
                PageMethods.GetInstructionByKey(lblInstruction.labelKey, function(value) {
                    lblInstruction.innerHTML = value;
                    ddl.SectionInfo.Add(lblInstruction.labelKey, value);
                });
            }
            else {
                lblInstruction.innerHTML = storedInstruction;
            }
            
            parent.parent.LoadProperties(16, ddl.parentObj);
        }
    </script>
</asp:Content>
