<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Cap.CapHome" ValidateRequest="false" Codebehind="CapHome.aspx.cs" %>
<%@ Import Namespace="Accela.ACA.Common"%>
<%@ Import Namespace="Accela.ACA.Web.Common"%>
<%@ Register Src="~/Component/ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<%@ Register Src="~/Component/CapList.ascx" TagName="PermitList" TagPrefix="uc1" %>
<%@ Register src="~/Component/CAPs2MyCollection.ascx" TagName="CAPs2MyCollection" TagPrefix="uc4" %>
<%@ Register Src="~/Component/ASISearchForm.ascx" TagName="ASISearchForm" TagPrefix="uc5" %>
<%@ Register Src="~/Component/ContactList.ascx" TagName="ContactList" TagPrefix="uc6" %>
<%@ Register Src="~/Component/ContactSearchForm.ascx" TagName="ContactSearchForm" TagPrefix="uc7" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<%@ Register Src="~/Component/SearchByAddressForm.ascx" TagName="SearchByAddressForm" TagPrefix="ACA" %>
<%@ Register Src="~/Component/GeneralSearchForm.ascx" TagName="GeneralSearchForm" TagPrefix="ACA" %>
<%@ Register Src="~/Component/SearchResultInfo.ascx" TagName="SearchResultInfo" TagPrefix="ACA" %>
<%@ Register Src="~/Component/ASITSearchForm.ascx" TagName="ASITSearchForm" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
<div class="ACA_Area_CapHome">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <script type="text/javascript" src="../Scripts/MyCollectionMethods.js"></script>
    <script type="text/javascript" src="../Scripts/ShoppingCartMethods.js"></script>
<script language="javascript" type="text/javascript">
    //add for ASI drill down.
    var splitChar = "<%=ACAConstant.SPLIT_CHAR%>";
    var defaultSelectText = "<%=WebConstant.DropDownDefaultText%>";
</script>
<script type="text/javascript" src="../Scripts/DrillDown.js"></script>

    <script type="text/javascript" language="javascript">
        function disableValidate(ascxId) {
            if (Page_Validators) {
                for (var i = Page_Validators.length - 1; i >= 0; i--) {
                    if (Page_Validators[i].id.indexOf(ascxId) != -1) {
                        Array.removeAt(Page_Validators,i);
                    }
                }
            }
        }
        
        function doSomething(e)
        {
            if (!e) var e = window.event;
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();
         }  
         
         var prm = Sys.WebForms.PageRequestManager.getInstance();

         prm.add_initializeRequest(InitializeRequest);
         prm.add_beginRequest(beginRequest);
         prm.add_endRequest(EndRequest);
 
         function InitializeRequest(sender, args)
         {
             document.body.style.cursor = 'wait';
             var btn = document.getElementById('ctl00_PlaceHolderMain_btnNewSearch');
             if(btn)
             {
                btn.style.cursor = 'wait';
             }   
         }
         
         function EndRequest(sender, args){
             document.body.style.cursor = '';
             var btn = document.getElementById('ctl00_PlaceHolderMain_btnNewSearch');
             if(btn)
             {
                btn.style.cursor = 'pointer';
             }                
             
             //export file.
             ExportCSV(sender, args);
         }
         
        function beginRequest()
        {
            prm._scrollPosition = null;
        } 

       function Add2Collection(e, gridObj) 
       {
            addCollectionClickElement = gridObj;
            var isRightOrLeft = '<%=IsRightToLeft %>';
            window.document.getElementById("<%=hfGridId.ClientID %>").value = gridObj.id;
            var obj = e.srcElement? e.srcElement : e.target;
            clearNameDefaultValue();
            var divAddForm  = document.getElementById("divForPermitsCollection");
            
            if(divAddForm == null)
            {
                return;
            }
            
            divAddForm.style.display = "block";
            
            if(isRightOrLeft == "True")
            {
                divAddForm.style.left = (getElementLeft(obj)-divAddForm.offsetWidth + obj.offsetWidth) + "px"; 
            }
            else
            {
                 divAddForm.style.left = getElementLeft(obj) + "px";
            } 
            divAddForm.style.top = (getElementTop(obj)+obj.offsetHeight) + "px";
            setAddedMessagePosition(obj, isRightOrLeft);
            var rdoButton = document.getElementById('<%=addForMyPermits.RDOExistCollectionID%>');
            if (rdoButton == null) {
                rdoButton = document.getElementById('<%=addForMyPermits.RDONewCollectionID%>');
            }

            if (rdoButton != null) {
                rdoButton.focus();
            }
        }
        
        function HiddenAddForm()
        {
            var divAddForm  = window.document.getElementById("divForPermitsCollection");
            if(divAddForm!=null && divAddForm !='undefined')
            {
                divAddForm.style.display = 'none';
            }
        }
        
        //In search by General Search, expand or collapse the ASI form.
        function ExpandCollapsePermitASI(isExpandedASI)
        {
           var imgPermitExpandCollapse = document.getElementById("imgPermitExpandCollapse");
           var lblPermitCollapse = document.getElementById('<%=lblPermitCollapse.ClientID %>');
           var lblPermitExpand = document.getElementById('<%=lblPermitExpand.ClientID %>');
           var divPermitASI = document.getElementById('<%=divASIPermit.ClientID %>');
           var hfASIExpandedFlag = document.getElementById('<%=hfASIExpanded.ClientID %>');
           var lnkPermitExpandCollapse = document.getElementById("lnkPermitExpandCollapse");

           ExpandCollapseASI(isExpandedASI, divPermitASI, lblPermitCollapse, lblPermitExpand, imgPermitExpandCollapse, hfASIExpandedFlag, lnkPermitExpandCollapse);
        }
        
        function ExpandCollapseContactTemplate(isExpandedContact)
        {
            var contactSearchFormID = '<%=contactSearchForm.ClientID %>';
            var divContactTemplateID = contactSearchFormID + "_divContactTemplate";
            var lblContactTemplateCollapseID = contactSearchFormID + "_lblContactTemplateCollapse"; 
            var lblContactTemplateExpandID = contactSearchFormID + "_lblContactTemplateExpand";   
            var divNoContactTemplateID = contactSearchFormID + "_divNoContactTemplate";      
            var imgContactTemplateCollapse = document.getElementById("imgContactTemplateCollapse");
            var lnkContactTemplate = document.getElementById("lnkContactTemplate");
            var divContactTemplate = $get(divContactTemplateID);
            var divNoContactTemplate = $get(divNoContactTemplateID);
            
            if(isExpandedContact == '<%=ACAConstant.COMMON_N%>')
            {
                divNoContactTemplate.className = 'ACA_TabRow ACA_CapDetail_NoRecord font12px';
                $get(lblContactTemplateCollapseID).className = divContactTemplate.className = 'ACA_Hide';
                $get(lblContactTemplateExpandID).className = '';
                Collapsed(imgContactTemplateCollapse, imgCollapsed, altExpanded);
                AddTitle(lnkContactTemplate, altExpanded, $get(lblContactTemplateExpandID));
                $get(contactSearchFormID + "_hfContactExpanded").value = '<%=ACAConstant.COMMON_N%>'; 
            }            
            else if(divContactTemplate.className == 'ACA_Hide'){
                if(divNoContactTemplate!=null)
                {
                    divNoContactTemplate.className = 'ACA_TabRow ACA_CapDetail_NoRecord font12px';
                }
                $get(lblContactTemplateCollapseID).className = divContactTemplate.className = '';
                $get(lblContactTemplateExpandID).className = 'ACA_Hide';
                Expanded(imgContactTemplateCollapse, imgExpanded, altCollapsed);
                AddTitle(lnkContactTemplate, altCollapsed, $get(lblContactTemplateCollapseID));
                $get(contactSearchFormID + "_hfContactExpanded").value = '<%=ACAConstant.COMMON_Y%>';
            }
            else{
                if(divNoContactTemplate!=null)
                {
                    divNoContactTemplate.className = 'ACA_Hide';
                }
                $get(lblContactTemplateCollapseID).className = divContactTemplate.className = 'ACA_Hide';
                $get(lblContactTemplateExpandID).className = '';
                Collapsed(imgContactTemplateCollapse, imgCollapsed, altExpanded);
                AddTitle(lnkContactTemplate, altExpanded, $get(lblContactTemplateExpandID));
                $get(contactSearchFormID + "_hfContactExpanded").value = '<%=ACAConstant.COMMON_N%>';
            }
        }
        
        //In search by General Search, expand or collapse the ASI form.
        function ExpandCollapseGSASI(isExpandedASI)
        {
           var imgGSExpandCollapse = document.getElementById("imgGSExpandCollapse");
           var lblGSCollapse = document.getElementById('<%=lblGSCollapse.ClientID %>');
           var lblGSExpand = document.getElementById('<%=lblGSExpand.ClientID %>');
           var divGSASI = document.getElementById('<%=divGSASI.ClientID %>');
           var hfASIExpandedFlag = document.getElementById('<%=hfASIExpanded.ClientID %>');
           var lnkExpandCollapseGS = document.getElementById("lnkExpandCollapseGS");

           ExpandCollapseASI(isExpandedASI, divGSASI, lblGSCollapse, lblGSExpand, imgGSExpandCollapse, hfASIExpandedFlag, lnkExpandCollapseGS);
        }
        
        //To expand or collapse ASI search form.
        function ExpandCollapseASI(isExpandedASI, divASI, lblCollapse, lblExpand, imgExpandCollapse, hfASIExpandedFlag, lnkExpandCollapse)
        {
           if (isExpandedASI != null && isExpandedASI == '<%=ACAConstant.COMMON_N%>')
           {
               CollapseASI(divASI, lblCollapse, lblExpand, imgExpandCollapse, lnkExpandCollapse);
           }
           else if (divASI != null)
           {
               if (divASI.className == 'ACA_Hide')
               {
                   ExpandASI(divASI, lblCollapse, lblExpand, imgExpandCollapse, lnkExpandCollapse);
                   hfASIExpandedFlag.value = '<%=ACAConstant.COMMON_Y%>';
               }
               else
               {
                   CollapseASI(divASI, lblCollapse, lblExpand, imgExpandCollapse, lnkExpandCollapse);
                   hfASIExpandedFlag.value = '<%=ACAConstant.COMMON_N%>';
               }
           }
        }

        var imgCollapsed = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
        var imgExpanded = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
        var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon") %>'; 
        var altExpanded = '<%=GetTextByKey("img_alt_expand_icon") %>'; 
        //To expand ASI search form.
        function ExpandASI(divASI, lblCollapse, lblExpand, imgExpandCollapse, lnkExpandCollapse)
        {
            if (divASI ==  null || lblCollapse == null || lblExpand == null || imgExpandCollapse == null)
            {
                return;
            }
            
            lblCollapse.className = divASI.className = '';
            lblExpand.className = 'ACA_Hide';
            Expanded(imgExpandCollapse, imgExpanded, altCollapsed);
            AddTitle(lnkExpandCollapse, altCollapsed, lblCollapse);
        }
        
        //To collapse ASI search form.
        function CollapseASI(divASI, lblCollapse, lblExpand, imgExpandCollapse, lnkExpandCollapse) 
        {
            if (divASI ==  null || lblCollapse == null || lblExpand == null || imgExpandCollapse == null)
            {
                return;
            }
            
            lblCollapse.className = divASI.className = 'ACA_Hide';
            lblExpand.className = '';
            Collapsed(imgExpandCollapse, imgCollapsed, altExpanded);
            AddTitle(lnkExpandCollapse, altExpanded, lblExpand);
        }

        function FocusSearchButton() {
            if ($get("<%=btnNewSearch.ClientID %>") != null) {
                $get("<%=btnNewSearch.ClientID %>").focus();
            }
        }
    </script>
    
   <table role='presentation' cellspacing="0"><tr><td><iframe width="0" height="0" id="iframeExport" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
    <div id="divForPermitsCollection" class="ACA_Add2CollectionForm">
        <uc4:CAPs2MyCollection ID="addForMyPermits" runat="server" /> 
    </div>
    <div id="MainContent" class="ACA_Content caphome">
        <div id="divMyPermitListSection" class="ACA_Section mypermitlist">
             <div id="divMyPermitListSectionHeader" class="ACA_SectionHeader" runat="server">
                <div id="divMyRecordListSection4Admin" class="ACA_SectionHeader" runat="server">
                    <ACA:AccelaLabel LabelKey="per_permitList_label_history" runat="server" LabelType="SectionTitle"/>
                </div>
                <div id="divMyRecordListSection4Daily" class="ACA_InfoTitle ACA_InfoTitle_FontSize" runat="server">
                    <h1>
                        <span class="ACA_FLeft" ID="lblPermitListHistory" runat="server"></span>
                    </h1>
                	<span class="ACA_FRight">
	                    <ACA:AccelaDropDownList ID="ddlDataFilter" runat="server" OnSelectedIndexChanged="DataFilterDropDown_IndexChanged" AutoPostBack="true" ToolTipLabelKey="aca_common_msg_dropdown_changedatafilter_tip">
	                    </ACA:AccelaDropDownList>
	                </span>
                </div>
                <div class="section_instruction_record_home">
                    <span ID="spanRecordSectionInstruction" class="ACA_Section_Instruction_FontSize" runat="server"/>
                </div>
                <div id="divMyPermitList" class="ACA_Row" runat="server">
                    <uc1:PermitList ID="PermitList" ShowPermitAddress="true" runat="server" GViewID="60033"/>
                </div>
            </div>
        </div>
        <div id="divSearchSectionInstruction" class="ACA_SectionBody" runat="server">
            <h1 runat="server" ID="h1Permit">
                <ACA:AccelaLabel LabelKey="per_permitList_label_permit" ID="lblPermitListTitle"
                    runat="server"></ACA:AccelaLabel>
            </h1>
            <div class="ACA_Page ACA_Page_FontSize introduce">
                <ACA:AccelaLabel LabelKey="per_permitList_text_introduce" ID="lblPermitListIntroduce"
                    runat="server" LabelType="BodyText"></ACA:AccelaLabel> 
            </div>
        </div>
        <div id="searchSection" class="ACA_Section searchsection" runat="server">
            <div class="ACA_SectionHeader">
            <asp:UpdatePanel ID="updatePanalLabel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <a name="SearchForm_Start" id="SearchForm_Start"></a>
                <div class="ACA_InfoTitle ACA_InfoTitle_FontSize">
                    <h1>                                
                        <span class="ACA_FLeft" ID="lblSelectedSearchType" runat="server"></span>
                    </h1>
                    <span class="ACA_FRight">
                        <ACA:AccelaDropDownList ID="ddlSearchType" runat="server" OnSelectedIndexChanged="SearchTypeDropDown_IndexChanged"
                            AutoPostBack="true" SourceType="HardCode" ToolTipLabelKey="aca_common_msg_dropdown_changesearchoption_tip">
                        </ACA:AccelaDropDownList>
                    </span>
                </div>
                <div id="divInstruction" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize" runat="server">
                    <ACA:AccelaLabel ID="lblSearchInstruction" runat="server" IsNeedEncode="false"></ACA:AccelaLabel>
                </div>
             
            <div class="ACA_SectionBody">
                <div id="divSearchPanel">
                    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table role='presentation' class="ACA_FRight">
                                <tr>
                                    <td>
                                        <div id="mypermitFlag" runat="server">
                                            <ACA:AccelaCheckBox ID="chkSearch" runat="server" LabelKey="per_permitList_label_searchPermitOnly" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <ACA:AccelaCheckBox ID="chkCrossModuleSearch" runat="server" LabelKey="per_permitList_label_searchPermitAll" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            
                            <div id="divSearchPaneltoPressEnter">
                            <div id="dvGenearlSearch" runat="server" visible="true">
                                <div class="ACA_TabRow">
                                    <ACA:GeneralSearchForm ID="generalSearchForm" OnSubAgencyChanged="GeneralSearch_SubAgencyChanged" OnPermitTypeChanged="GeneralSearchForm_PermitTypeChanged" runat="server"></ACA:GeneralSearchForm>
                                </div>
                                <div id="divGSLoadASI" runat="server" class="ACA_TabRow ACA_Link_Text ACA_Link_Text_FontSize">
                                    <asp:LinkButton ID="btnGSLoadASI" runat="server" CausesValidation="false" OnClick="GSLoadASIButton_OnClick"  >
                                        <img id="imgGSLoadASI" class="caphome_search_icon" runat="server"/>
                                        <ACA:AccelaLabel ID="lblGSLoadASI" runat="server" LabelKey="per_permitList_label_permit_expandASI" />
                                    </asp:LinkButton> 
                                    <br />
                                    <ACA:AccelaLabel ID="lblGSNoASIMsg" runat="server" CssClass="ACA_Sub_Label ACA_Sub_Label_FontSize ACA_Message_Label" LabelKey="per_permitList_label_permit_noASI_message" />
                                </div>
                                <div id="divGSExpandCollapseASI" runat="server" class="ACA_Link_Text ACA_Link_Text_FontSize ACA_HyperLink">
                                    <a id="btnGSExpandCollapse" href="javascript:void(0);" onclick="ExpandCollapseGSASI();" class="NotShowLoading" title="<%=GetTitleByKey("img_alt_collapse_icon","per_permitList_label_permit_collapseASI") %>">
                                        <img id="imgGSExpandCollapse" class="caphome_search_icon" alt="<%=GetTextByKey("img_alt_collapse_icon") %>" src="<%=ImageUtil.GetImageURL("caret_expanded.gif") %>"/>
                                        <ACA:AccelaLabel ID="lblGSExpand" runat="server" LabelKey="per_permitList_label_permit_expandASI" CssClass="ACA_Hide" />
                                        <ACA:AccelaLabel ID="lblGSCollapse" runat="server" LabelKey="per_permitList_label_permit_collapseASI" />
                                    </a>
                                </div>
                                <div id="divGSASI" runat="server">
                                    <div id="divGSNoASIResult" runat="server" visible="false" class="caphome_asitsearchform_norecord">
                                        <ACA:AccelaLabel ID="lblNoASIResult" runat="server" LabelKey="asi_searchForm_label_notResult" />
                                    </div>
                                    <div id="divGSASITResult" runat="server" class="caphome_asitsearchform">
                                        <uc5:ASISearchForm ID="asiGSForm" runat="server" />
                                        <ACA:ASITSearchForm ID="asitGSForm" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField ID="hfASIExpanded" runat="server"/>
                            <div class="ACA_Hide">
                                <!-- Datetime input control, add for avoid bug of Safari4, please don't delete it!! -->
                                <ACA:AccelaCalendarText ID="txtHiddenDate" runat="server" title="This is a hidden field." />
                            </div>
                            <div id="dvSearchForPermit" runat="server" visible="false">
                                <div class="ACA_TabRow" id="divPermit" runat="server">
                                    <table role='presentation' class="ACA_TDAlignLeftOrRightTopNoBorder">
                                        <tr>
                                            <td>
                                                 <ACA:AccelaDropDownList ID="ddlPermitSubAgency" runat ="server" LabelKey="per_permitList_label_agency" OnSelectedIndexChanged="PermitSubAgencyDropDown_IndexChanged"/>
                                            </td>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtPermitNumber" runat="server" LabelKey="per_permitList_label_permitNum"
                                                    CssClass="ACA_NLonger" MaxLength="22"></ACA:AccelaTextBox></td>
                                            <td>
                                                <ACA:AccelaDropDownList ID="ddlPermitType" runat="server" LabelKey="per_permitList_label_permitType_ForLabel" OnSelectedIndexChanged="PermitTypeDropDown_IndexChanged">
                                                </ACA:AccelaDropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="ACA_TabRow" runat="server" id="dvProjectName">
                                    <table role='presentation' class="ACA_TDAlignLeftOrRightTopNoBorder">
                                        <tr>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtProjectName" runat="server" LabelKey="per_permitList_label_projectName"
                                                    CssClass="ACA_NLonger" MaxLength="255"></ACA:AccelaTextBox>
                                             </td>                                         
                                             <td>
                                                 <ACA:AccelaDropDownList ID="ddlCapStatus" runat ="server" LabelKey="aca_caphome_capstatus"/>
                                             </td>
                                         </tr>
                                     </table>
                                </div>
                                <div class="ACA_TabRow" id="divPermitDate" runat="server">
                                    <table role='presentation' class="ACA_TDAlignLeftOrRightTopNoBorder">
                                        <tr>
                                            <td>
                                                <ACA:AccelaCalendarText ID="txtStartDate" CssClass="ACA_NShot" runat="server" LabelKey="per_permitList_label_startDate">

                                                </ACA:AccelaCalendarText></td>
                                            <td>
                                                <ACA:AccelaCalendarText ID="txtEndDate" runat="server" LabelKey="per_permitList_label_endDate"
                                                    CssClass="ACA_NShot"></ACA:AccelaCalendarText>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divPermitLoadASI" runat="server" class="ACA_TabRow ACA_Link_Text ACA_Link_Text_FontSize">
                                    <asp:LinkButton ID="lnkPermitLoadASI" runat="server" CausesValidation="false"  OnClick="PermitLoadASIButton_OnClick">
                                        <img id="imgPermitLoadASI" class="caphome_search_icon" runat="server"/>
                                        <ACA:AccelaLabel ID="lblPermitLoadASI" runat="server" LabelKey="per_permitList_label_GS_expandASI" />
                                    </asp:LinkButton>
                                    <br />
                                    <ACA:AccelaLabel ID="lblPermitNoASIMsg" runat="server" CssClass="ACA_Sub_Label ACA_Sub_Label_FontSize ACA_Message_Label" LabelKey="per_permitList_label_GS_noASI_message"/>
                                </div>
                                <div id="divPermitExpandCollapseASI" class="ACA_Link_Text ACA_Link_Text_FontSize ACA_HyperLink" runat="server">
                                    <a id="btnPermitExpandCollapse" href="javascript:void(0);" class="NotShowLoading" onclick="ExpandCollapsePermitASI();" title="<%=GetTitleByKey("img_alt_collapse_icon","per_permitList_label_GS_collapseASI") %>">
                                        <img id="imgPermitExpandCollapse" class="caphome_search_icon" alt="<%=GetTextByKey("img_alt_collapse_icon") %>" src="<%=ImageUtil.GetImageURL("caret_expanded.gif") %>"/>
                                        <ACA:AccelaLabel ID="lblPermitExpand" runat="server" LabelKey="per_permitList_label_GS_expandASI" CssClass="ACA_Hide" />
                                        <ACA:AccelaLabel ID="lblPermitCollapse" runat="server" LabelKey="per_permitList_label_GS_collapseASI" />
                                    </a>
                                </div>
                                <div id="divASIPermit" runat="server">
                                    <div id="divPermitNoASIResult" runat="server" visible="false" class="caphome_asitsearchform_norecord">
                                        <ACA:AccelaLabel ID="lblPermitNoASIResult" runat="server" LabelKey="asi_searchForm_label_notResult" />
                                    </div>
                                    <div id="divPermitASITResult" runat="server" class="caphome_asitsearchform">
                                        <uc5:ASISearchForm ID="asiPermitForm" runat="server" />
                                        <ACA:ASITSearchForm ID="asitPermitForm" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div id="dvSearchForLicensed" runat="server" visible="true">
                                <div class="ACA_TabRow" id="divLicense" runat="server">
                                    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
                                        <tr>
                                            <td>
                                                <ACA:AccelaDropDownList ID="ddlLicenseType" runat="server" LabelKey="per_permitList_label_licenseType">
                                                </ACA:AccelaDropDownList></td>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtLicenseNumber" runat="server" LabelKey="per_permitList_label_stateLicenseNum"
                                                    CssClass="ACA_NLonger" MaxLength="30"></ACA:AccelaTextBox></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="ACA_TabRow" id="divContactType" runat="server">
                                    <table role='presentation'>
                                        <tr>
                                            <td>
                                            <ACA:AccelaDropDownList ID="ddlContactType" runat="server" LabelKey="per_permitList_contacttype"></ACA:AccelaDropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="ACA_TabRow" id="divSSN" runat="server">
                                    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
                                        <tr>
                                            <td><ACA:AccelaSSNText ID="txtSSN" MaxLength="11" LabelKey="per_permitList_ssn" runat="server" IsIgnoreValidate="true"></ACA:AccelaSSNText></td>
                                            <td><ACA:AccelaFeinText ID="txtFEIN" MaxLength="11" LabelKey="per_permitList_fein" IsIgnoreValidate="true" runat="server"></ACA:AccelaFeinText></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="ACA_TabRow" id="divLicenseName" runat="server">
                                    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
                                        <tr>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtFirstName" runat="server" LabelKey="per_permitList_label_firstName"
                                                    CssClass="ACA_NShot" MaxLength="70"></ACA:AccelaTextBox></td>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtLastName" runat="server" LabelKey="per_permitList_label_lastName"
                                                    CssClass="ACA_NShot" MaxLength="70"></ACA:AccelaTextBox></td>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtBusiName" runat="server" LabelKey="per_permitList_label_business"
                                                    CssClass="ACA_XLong" MaxLength="255"></ACA:AccelaTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="ACA_TabRow" id="divLicenseName2" runat="server">
                                    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
                                        <tr>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtBusiName2" runat="server" LabelKey="per_permitList_label_business2"
                                                    CssClass="ACA_XLong" MaxLength="255"></ACA:AccelaTextBox>
                                            </td> 
                                       </tr> 
                                    </table> 
                                </div>
                                <div class="ACA_TabRow" id="divBusiLicense" runat="server">
                                    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
                                        <tr>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtBusiLicense" runat="server" LabelKey="per_permitlist_label_businesslicense"
                                                    CssClass="ACA_NLong" MaxLength="15"></ACA:AccelaTextBox>
                                            </td> 
                                       </tr> 
                                    </table> 
                                </div>
                                <div class="ACA_TabRow" id="divLicenseContractor" runat="server">
                                     <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
                                        <tr>
                                             <td>
                                                <ACA:AccelaNumberText ID="txtContractorLicNO" runat="server"  LabelKey="per_permitList_label_contractorlicno"
                                                    CssClass="ACA_Small" MaxLength="8" Validate="MaxLength" IsNeedDot="false" />
                                            </td>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtContractorBusiName" runat="server" LabelKey="per_permitList_label_contractorbusiname"
                                                    CssClass="ACA_NLonger" MaxLength="255"></ACA:AccelaTextBox>
                                            </td>
                                        </tr>
                                   </table>
                                </div>
                            </div>                        
                            <div id="dvSearchForAddress" runat="server" visible="true">
                                <div class="ACA_TabRow">
                                    <ACA:SearchByAddressForm ID="searchByAddress" runat="server" />
                                </div>
                            </div>
                            <div id="dvSearchForTradeName" runat="server" visible="true">
                                <div class="ACA_TabRow" id="dvTradeName1" runat="server" >
                                    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
                                        <tr>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtEnglishTradeName" runat="server" LabelKey="per_permitList_label_englishTradeName"
                                                    CssClass="ACA_NLonger" MaxLength="65"></ACA:AccelaTextBox>
                                            </td>
                                            <td>
                                                <ACA:AccelaTextBox ID="txtArabicTradeName" runat="server" LabelKey="per_permitList_label_arabicTradeName"
                                                    CssClass="ACA_NLonger" MaxLength="65"></ACA:AccelaTextBox>
                                            </td>
                                            <td>
                                                <ACA:AccelaDropDownList ID="ddlTradeNameRecordStatus" runat="server" LabelKey="aca_caphome_searchtradename_label_recordstatus">
                                                </ACA:AccelaDropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                 </div>
                                 <div class="ACA_TabRow" id="dvTradeName2" runat="server">
                                    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
                                        <tr>
                                            <td>
                                                <ACA:AccelaDropDownList ID="ddlTradeNameType" runat="server" LabelKey="per_permitList_label_tradeNameType">
                                                </ACA:AccelaDropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="dvSearchForContact" runat="server" visible="true">
                                <uc7:ContactSearchForm ID="contactSearchForm" runat="server"/>
                            </div>  
                            </div>

                            <ACA:AccelaHeightSeparate ID="sepLineForButton" runat="server" Height="35" />
                            <div class="ACA_Row ACA_LiLeft">
                                <table role='presentation' cellpadding="0" cellspacing="0" border="0">
                                    <tr valign="bottom">
                                        <td>
                                            <div>
                                                <ACA:AccelaButton ID="btnNewSearch" runat="server" OnClick="NewSearchButton_Click"  CausesValidation="true" LabelKey="per_permitList_label_buttonSearch" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"></ACA:AccelaButton>
                                            </div>
                                         </td>
                                         <td class="button_space">&nbsp;</td>
                                         <td>
                                            <div>
                                                <ACA:AccelaButton ID="btnResetSearch" runat="server" OnClick="ResetSearchButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" CausesValidation="false" LabelKey="aca_modulesearch_label_resetsearch"></ACA:AccelaButton>
                                            </div>
                                         </td>
                                    </tr>
                                </table>
                            </div>
                            <ACA:SearchResultInfo ID="AssociatedSearchResultInfo" runat="server" CountSummaryLabelKey="per_permitList_label_Count_For_Address" PromptLabelKey="per_permitList_label_Click_Prompt"/>
                            <div id="dvAddressResult" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server" visible="false">
                                <ACA:AccelaGridView ID="dgvForAddress" GridViewNumber="60032" runat="server" AutoGenerateColumns="false"
                                    Visible="false" ShowCaption="true" AllowSorting="true" AllowPaging="true"  PagerStyle-HorizontalAlign="center"
                                    CaptionCssClass="ACA_SmLabel ACA_SmLabel_FontSize" OnRowCommand="ForAddressGridView_RowCommand" OnGridViewSort="GridView_GridViewSort"
                                    OnPageIndexChanging="GridView_PageIndexChanging" ShowExportLink="true" SummaryKey="gdv_caphome_addresslist_summary" CaptionKey="aca_caption_caphome_addresslist"
                                    OnGridViewDownload="ForAddressGridView_GridViewDownloadAll">
                                    <Columns>
                                        <ACA:AccelaTemplateField AttributeName="lnkAddressHeader" ExportDataField="Address">
                                            <headertemplate>
                                                <div>
                                                <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" SortExpression="Address" LabelKey="per_permitList_label_addressListAddress" >
                                                </ACA:GridViewHeaderLabel> 
                                                </div>
                                            </headertemplate>
                                            <itemtemplate>
                                                <div class="ACA_FLeft">
                                                    <asp:LinkButton ID="lbAddress" runat="server" CommandName="showCAPs" Visible="true"
                                                        CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text='<%#DataBinder.Eval(Container.DataItem, "Address")%>'
                                                        Font-Bold="true">
                                                    </asp:LinkButton>
                                                     <ACA:AccelaLabel ID="lblAddressRowIndex" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "RowIndex")%>' Visible="false" />
                                                     <ACA:AccelaLabel ID="lblAddress" IsNeedEncode="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Address")%>' Visible="false" />
                                                     <ACA:AccelaLabel ID="lblAgency" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AgencyCode")%>' Visible="false" />
                                                </div>
                                            </itemtemplate>
                                            <ItemStyle Width="478px"/>
                                            <headerstyle Width="478px"/>
                                        </ACA:AccelaTemplateField>
                                    </Columns>
                                </ACA:AccelaGridView>
                            </div>
                            <div id="dvTradeNameResult" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server" visible="false">
                                <ACA:AccelaGridView ID="dgvTradeName" GridViewNumber="60056" runat="server" AutoGenerateColumns="false"
                                    Visible="false" ShowCaption="true" AllowSorting="true" AllowPaging="true"              
                                    CaptionCssClass="ACA_SmLabel ACA_SmLabel_FontSize" OnRowCommand="TradeNameGridView_RowCommand" OnRowDataBound="TradeNameGridView_RowDataBound"
                                    OnGridViewSort="GridView_GridViewSort" ShowExportLink="true" OnPageIndexChanging="GridView_PageIndexChanging" PagerStyle-HorizontalAlign="center" 
                                    SummaryKey="gdv_caphome_tradelist_summary" CaptionKey="aca_caption_caphome_tradelist"
                                    OnGridViewDownload="TradeNameGridView_GridViewDownloadAll">
                                    <Columns>
                                    <ACA:AccelaTemplateField AttributeName="lnkTradeNameNumHeader" ExportDataField="LicenseNumber">
                                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkTradeNameNumHeader" runat="server" SortExpression="LicenseNumber" 
                                                    LabelKey="per_permitList_label_listTradeNameNumber" ></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                                        <itemtemplate>
                                            <div>
                                                <asp:HyperLink ID="hlTradeNameNum" runat ="server" ><strong><ACA:AccelaLabel ID="lblLicenseSeqNbr" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseNumber") %>' /></strong></asp:HyperLink>
                                            </div>
                                        </itemtemplate>
                                        <ItemStyle Width="128px"/>
                                        <headerstyle Width="128px"/>
                                    </ACA:AccelaTemplateField>
                                    <ACA:AccelaTemplateField AttributeName="lnkTradeNameTypeHeader" ExportDataField="LicenseType">
                                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkTradeNameTypeHeader" runat="server" SortExpression="LicenseType" 
                                                    LabelKey="per_permitList_label_listTradeNameType" ></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                                        <itemtemplate>
                                            <div>
                                                <ACA:AccelaLabel  ID="lblTradeNameType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseType") %>' />
                                            </div>
                                        </itemtemplate>
                                        <ItemStyle Width="80px"/>
                                        <headerstyle Width="80px"/>
                                    </ACA:AccelaTemplateField>
                                    <ACA:AccelaTemplateField AttributeName="lnkEnglishTradeNameHeader" ExportDataField="EnglishName">
                                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkEnglishTradeNameHeader" runat="server" SortExpression="EnglishName" 
                                                    LabelKey="per_permitList_label_listEnglishTradeName"></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                                        <itemtemplate>
                                            <div>
                                                <ACA:AccelaLabel  ID="lblEnglishTradeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EnglishName") %>' />
                                            </div>
                                        </itemtemplate>
                                        <ItemStyle Width="128px"/>
                                        <headerstyle Width="128px"/>
                                    </ACA:AccelaTemplateField>
                                    <ACA:AccelaTemplateField AttributeName="lnkArabicTradeNameHeader" ExportDataField="ArabicName">
                                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkArabicTradeNameHeader" runat="server" SortExpression="ArabicName" 
                                                    LabelKey="per_permitList_label_listArabicTradeName"></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                                        <itemtemplate>
                                            <div>
                                                <ACA:AccelaLabel  ID="lblArabicTradeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ArabicName") %>' />
                                            </div>
                                        </itemtemplate>
                                        <ItemStyle Width="128px"/>
                                        <headerstyle Width="128px"/>
                                    </ACA:AccelaTemplateField>
                                    <ACA:AccelaTemplateField AttributeName="lnkExpirationDateHeader" ExportDataField="LicenseExpirationDate" ExportFormat="ShortDate">
                                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkExpirationDateHeader" runat="server" SortExpression="LicenseExpirationDate" 
                                                    LabelKey="per_permitList_label_listExpirationDate"></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                                        <itemtemplate>
                                            <div >
                                                <ACA:AccelaDateLabel id="lblExpirationDate" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "LicenseExpirationDate")%>'></ACA:AccelaDateLabel>
                                            </div>
                                        </itemtemplate>
                                        <ItemStyle Width="80px"/>
                                        <headerstyle Width="80px"/>
                                    </ACA:AccelaTemplateField>
                                     <ACA:AccelaTemplateField AttributeName="lnkStatusHeader" ExportDataField="LicenseStatus">
                                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkStatusHeader" runat="server" SortExpression="LicenseStatus" 
                                                    LabelKey="per_permitList_label_listTradeNameStatus"></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                                        <itemtemplate>
                                            <div>
                                                <ACA:AccelaLabel  ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseStatus") %>' />
                                            </div>
                                        </itemtemplate>
                                        <ItemStyle Width="128px"/>
                                        <headerstyle Width="128px"/>
                                    </ACA:AccelaTemplateField>
                                    </Columns>
                                </ACA:AccelaGridView>
                            </div>
                            <div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="dvLicenseResult" runat="server" visible="false">
                                <ACA:AccelaGridView ID="dgvLicense" GridViewNumber="60031" runat="server" ShowExportLink="true" 
                                    AllowPaging="true" AllowSorting="true" ShowCaption="true" Visible="false" AutoGenerateColumns="false"
                                    OnRowCommand="LicenseGridView_RowCommand" OnRowDataBound="LicenseGridView_RowDataBound"
                                    OnGridViewSort="GridView_GridViewSort" OnPageIndexChanging="GridView_PageIndexChanging" PagerStyle-HorizontalAlign="center" 
                                    SummaryKey="gdv_caphome_licenselist_summary" CaptionKey="aca_caption_caphome_licenselist"
                                    OnGridViewDownload="LicenseGridView_GridViewDownloadAll">
                                    <Columns>
                                        <ACA:AccelaTemplateField AttributeName="lnkLicenseNumberHeader" ExportDataField="LicenseNumber">
                                            <headertemplate>
                                              <div class="ACA_CapListStyle">
                                                <ACA:GridViewHeaderLabel ID="lnkLicenseNumberHeader" runat="server" CommandName="Header" SortExpression="LicenseNumber" 
                                                    LabelKey="per_permitList_label_licenseListLicenseNumber" ></ACA:GridViewHeaderLabel>
                                              </div>
                                            </headertemplate>
                                            <itemtemplate>
                                            <div class="ACA_CapListStyle">
                                                <asp:LinkButton ID="lbLicense" runat="server" CommandName="showCAPs" Visible="true" commandargument="<%#((GridViewRow)Container).RowIndex%>" >
                                                    <strong>
                                                        <%#Accela.ACA.Common.Common.ScriptFilter.FilterScriptEx(DataBinder.Eval(Container.DataItem, "LicenseNumber"))%>
                                                 </strong>                                        
                                                </asp:LinkButton>                                             
                                                 <ACA:AccelaLabel ID="lblLicenseID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "licenseNumber")%>' Visible="false" />
                                                 <ACA:AccelaLabel ID="lblLicenseType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "licenseType")%>' Visible="false" />
                                                 <ACA:AccelaLabel ID="lblLicenseTypeText" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "licenseTypeText")%>' Visible="false" />
                                                 <ACA:AccelaLabel ID="lblAgencyCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AgencyCode")%>' Visible="false" />                                      
                                            </div>
                                        </itemtemplate>
                                            <ItemStyle Width="160px"/>
                                            <headerstyle Width="160px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkLicenseTypeHeader" ExportDataField="LicenseType">
                                            <headertemplate>
                                              <div class="ACA_CapListStyle">
                                                <ACA:GridViewHeaderLabel ID="lnkLicenseTypeHeader" runat="server" SortExpression="LicenseType" 
                                                    LabelKey="per_permitList_label_licenseListLicenseType" ></ACA:GridViewHeaderLabel>
                                              </div>  
                                            </headertemplate>
                                            <itemtemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaLabel ID="lblLicType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "LicenseTypeText")%>' />
                                            </div>
                                        </itemtemplate>
                                            <ItemStyle Width="100px"/>
                                            <headerstyle Width="100px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkContactTypeHeader" ExportDataField="ContactType">
                                            <headertemplate>
                                                   <div class="ACA_CapListStyle">
                                                        <ACA:GridViewHeaderLabel ID="lnkContactTypeHeader" runat="server" 
                                                            CommandName="Header" SortExpression="ContactType" 
                                                            LabelKey="caphome_licenselist_contacttype"  
                                                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                   </div>
                                            </headertemplate>
                                            <itemtemplate>
                                                        <div class="ACA_CapListStyle">
                                                            <ACA:AccelaLabel ID="lblContactType" runat="server" Text='<%#DropDownListBindUtil.GetTypeFlagTextByValue(Convert.ToString(DataBinder.Eval(Container.DataItem, "ContactType"))) %>'></ACA:AccelaLabel>
                                                        </div>
                                           </itemtemplate>
                                           <ItemStyle Width="100px"/>
                                           <headerstyle Width="100px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkSSNHeader" ExportDataField="MaskedSSN">
                                            <headertemplate>
                                                    <div class="ACA_CapListStyle">
                                                        <ACA:GridViewHeaderLabel ID="lnkSSNHeader" runat="server" 
                                                            CommandName="Header" SortExpression="MaskedSSN" 
                                                            LabelKey="caphome_licenselist_ssn"  
                                                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                    </div>
                                            </headertemplate>
                                            <itemtemplate>
                                                        <div class="ACA_CapListStyle">
                                                            <ACA:AccelaLabel ID="lblSSN" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaskedSSN") %>'></ACA:AccelaLabel>
                                                        </div>
                                           </itemtemplate>
                                           <ItemStyle Width="100px"/>
                                           <headerstyle Width="100px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkFEINHeader" ExportDataField="MaskedFEIN">
                                            <headertemplate>
                                                    <div class="ACA_CapListStyle">
                                                        <ACA:GridViewHeaderLabel ID="lnkFEINHeader" runat="server" 
                                                            CommandName="Header" SortExpression="FEIN" 
                                                            LabelKey="caphome_licenselist_fein"  
                                                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                                                    </div>
                                            </headertemplate>
                                            <itemtemplate>
                                                        <div class="ACA_CapListStyle">
                                                            <ACA:AccelaLabel ID="lblFEIN" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaskedFEIN") %>'></ACA:AccelaLabel>
                                                        </div>
                                           </itemtemplate>
                                           <ItemStyle Width="100px"/>
                                           <headerstyle Width="100px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkContractNameHeader" ExportDataField="ContractName">
                                            <headertemplate>
                                              <div class="ACA_CapListStyle">
                                                <ACA:GridViewHeaderLabel ID="lnkContractNameHeader" runat="server" SortExpression="ContractName" 
                                                    LabelKey="per_permitList_label_licenseListContractName" ></ACA:GridViewHeaderLabel> 
                                              </div> 
                                            </headertemplate>
                                            <itemtemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaLabel ID="lblContractName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ContractName")%>' />
                                            </div>
                                        </itemtemplate>
                                            <ItemStyle Width="160px"/>
                                            <headerstyle Width="160px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkBusinessNameHeader" ExportDataField="BusinessName">
                                            <headertemplate>
                                              <div class="ACA_CapListStyle">
                                                <ACA:GridViewHeaderLabel ID="lnkBusinessNameHeader" runat="server" SortExpression="BusinessName" 
                                                    LabelKey="per_permitlist_label_licenselistbusinessname" ></ACA:GridViewHeaderLabel> 
                                              </div> 
                                            </headertemplate>
                                            <itemtemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaLabel ID="lblBusinessName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "BusinessName")%>' />
                                            </div>
                                        </itemtemplate>
                                            <ItemStyle Width="100px"/>
                                            <headerstyle Width="100px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkBusinessLicenseHeader" ExportDataField="BusinessLicense">
                                            <headertemplate>
                                              <div class="ACA_CapListStyle">
                                                <ACA:GridViewHeaderLabel ID="lnkBusinessLicenseHeader" runat="server" SortExpression="BusinessLicense" 
                                                    LabelKey="per_permitlist_label_licenselistbusinesslicense" ></ACA:GridViewHeaderLabel> 
                                              </div> 
                                            </headertemplate>
                                            <itemtemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaLabel ID="lblBusinessLicense" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "BusinessLicense")%>' />
                                            </div>
                                        </itemtemplate>
                                            <ItemStyle Width="200px"/>
                                            <headerstyle Width="200px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkLicenseIssueDateHeader" ExportDataField="LicenseIssueDate" ExportFormat="ShortDate">
                                            <headertemplate>
                                              <div class="ACA_CapListStyle">
                                                <ACA:GridViewHeaderLabel ID="lnkLicenseIssueDateHeader" runat="server" SortExpression="LicenseIssueDate" 
                                                    LabelKey="per_permitList_label_licenseListLicenseIssueDate" ></ACA:GridViewHeaderLabel> 
                                              </div>  
                                            </headertemplate>
                                            <itemtemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaDateLabel id="lblLicIssueDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "LicenseIssueDate")%>'></ACA:AccelaDateLabel>
                                            </div>
                                        </itemtemplate>
                                            <ItemStyle Width="160px"/>
                                            <headerstyle Width="160px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkLicenseExpirationDateHeader" ExportDataField="LicenseExpirationDate" ExportFormat="ShortDate">
                                            <headertemplate>
                                              <div class="ACA_CapListStyle"> 
                                                <ACA:GridViewHeaderLabel ID="lnkLicenseExpirationDateHeader" runat="server" SortExpression="LicenseExpirationDate" 
                                                    LabelKey="per_permitList_label_licenseListLicenseExpirationDate" ></ACA:GridViewHeaderLabel> 
                                              </div> 
                                            </headertemplate>
                                            <itemtemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaDateLabel id="lblLicExpDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "LicenseExpirationDate")%>'></ACA:AccelaDateLabel>
                                            </div>
                                        </itemtemplate>
                                            <ItemStyle Width="160px"/>
                                            <headerstyle Width="160px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkPhoneHeader" ExportDataField="Phone">
                                            <headertemplate>
                                              <div class="ACA_CapListStyle">
                                                <ACA:GridViewHeaderLabel ID="lnkPhoneHeader" runat="server" SortExpression="Phone" 
                                                    LabelKey="per_permitList_label_licenseListPhone" ></ACA:GridViewHeaderLabel> 
                                              </div> 
                                            </headertemplate>
                                            <itemtemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaLabel ID="lblPhone" IsNeedEncode="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Phone")%>' />
                                            </div>
                                        </itemtemplate>
                                            <ItemStyle Width="100px"/>
                                            <headerstyle Width="100px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkFaxHeader" ExportDataField="Fax">
                                            <headertemplate>
                                              <div class="ACA_CapListStyle"> 
                                                <ACA:GridViewHeaderLabel ID="lnkFaxHeader" runat="server" SortExpression="Fax" 
                                                    LabelKey="per_permitList_label_licenseListFax" ></ACA:GridViewHeaderLabel> 
                                              </div>
                                            </headertemplate>
                                            <itemtemplate>                                    
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaLabel ID="lblFax" IsNeedEncode="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Fax")%>' />
                                            </div>
                                        </itemtemplate>
                                            <ItemStyle Width="100px"/>
                                            <headerstyle Width="100px"/>
                                        </ACA:AccelaTemplateField>
                                        <ACA:AccelaTemplateField AttributeName="lnkAddress1Header" ExportDataField="Address1">
                                            <headertemplate>
                                              <div class="ACA_CapListStyle">
                                                <ACA:GridViewHeaderLabel ID="lnkAddress1Header" runat="server" SortExpression="Address1" 
                                                    LabelKey="per_permitList_label_licenseListAddress1" ></ACA:GridViewHeaderLabel>
                                              </div> 
                                            </headertemplate>
                                            <itemtemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaLabel ID="lblAddress1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Address1")%>' />
                                            </div>
                                        </itemtemplate>
                                            <ItemStyle Width="100px"/>
                                            <headerstyle Width="100px"/>
                                        </ACA:AccelaTemplateField>
                                    </Columns>
                                </ACA:AccelaGridView>
                            </div>
                            <div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="divContactResult" runat="server" visible="false">
                                    <uc6:ContactList ID="contactList" Location="SearchResult" GViewID ="60066" runat="server"/>
                            </div>
                            
                            <ACA:AccelaInlineScript runat="server">
                                <script type="text/javascript">
                                    var btnNewSearchId = '<%=btnNewSearch.UniqueID %>';
                                    $("#divSearchPaneltoPressEnter").keydown(function (e) {
                                        var srcElement = e.srcElement ? e.srcElement : e.target;
                                        var activeElement = document.activeElement;
                                        if (activeElement == null || activeElement == "undefined" || (activeElement.tagName != "A" && srcElement.id != "<%= ddlSearchType.ClientID %>")) {
                                            if (e.keyCode == "13" && srcElement.type != "textarea" && $.global.isAdmin == false) {
                                                // disable addForMyPermits validate 
                                                disableValidate('addForMyPermits');
                                            }
                                        }

                                        pressEnter2TriggerClick(e, $("#<%=btnNewSearch.ClientID %>"));
                                    });

                                    $('#' + btnNewSearchId).click(function (e) {
                                        // disable addForMyPermits validate
                                        disableValidate('addForMyPermits');
                                    });
                                </script>
                            </ACA:AccelaInlineScript>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlSearchType" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                            <ACA:SearchResultInfo ID="RecordSearchResultInfo" runat="server" CountSummaryLabelKey="per_permitList_label_Count_For_Permit" PromptLabelKey="per_permitList_label_Click_Prompt1"/>
                            <div class="ACA_TabRow ACA_Line_Content" runat="server" id="divSeparateLine" visible="false">
                                <hr style="border-width: 0px" />
                            </div>
                            <div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="divPermitList" runat="server" visible="false">
                                <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap" runat="server" id="dvPromptForAddress" visible="false">
                                    <ACA:AccelaLabel ID="lblPromptForAddress" runat="server" CssClass="SearchResultPromptInfo font13px" LabelKey="per_permitList_label_Permit_By_Address"></ACA:AccelaLabel>
                                </div>
                                <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap" runat="server" id="dvPromptForLicense" visible="false">
                                    <ACA:AccelaLabel ID="lblPromptForLicense" runat="server" CssClass="SearchResultPromptInfo font13px" LabelKey="per_permitList_label_Permit_By_License"></ACA:AccelaLabel>
                                </div>
                                <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap" runat="server" id="dvPromptForContact" visible="false">
                                    <ACA:AccelaLabel ID="lblPromptForContact" runat="server" CssClass="SearchResultPromptInfo font13px" LabelKey="per_permitList_label_permit_by_contact"/>
                                </div>
                                <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap SearchResultSelectedPrompt" id="dvResult" visible="false" runat="server">
                                    <ACA:AccelaLabel ID="lblResult" IsNeedEncode="false" CssClass="font13px" runat="server"/>
                                </div>
                            </div>
                        <div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="dvSearchList" runat="server" visible="false">
                            <uc1:PermitList ID="dgvPermitList" runat="server" OnGridViewSort="PLGridView_GridViewSort"
                                 OnPageIndexChanging="PLGridView_PageIndexChanging" ShowPermitAddress="true" GViewID="60033"/>
                        </div> 
                    </contenttemplate>  
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnNewSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnResetSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlSearchType" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="dgvForAddress" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="dgvTradeName" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="dgvLicense" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="contactList" EventName="ContactSelected" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
               </ContentTemplate>
            </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </td></tr>
    </table>
    <a name="PageResult" id="PageResult" href="#PageResult" tabindex="-1"></a>
    <div id="divAdded"class="ACA_Loading_Message ACA_SmLabel ACA_SmLabel_FontSize">
    </div>
    <asp:HiddenField ID="hfGridId" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            if (typeof (myValidationErrorPanel) != "undefined") {
                myValidationErrorPanel.registerIDs4Recheck("<%=ddlSearchType.ClientID %>");
                myValidationErrorPanel.registerIDs4Recheck("<%=btnResetSearch.ClientID %>");
            }
        });

        var prefix = "#ctl00_PlaceHolderMain_";
        if($.global.isAdmin)
        {
            HideAllSections();
            $(prefix + "dvGenearlSearch").show();
            $get("<%=ddlSearchType.ClientID %>").SectionInfo = new KeyValuePair();
            $get("<%=lblSearchInstruction.ClientID%>").labelKey = "per_permitlist_instruction_generalsearch";
        }

        function HideAllSections() {
            $(prefix + "dvGenearlSearch").hide();
            $(prefix + "dvSearchForPermit").hide();
            $(prefix + "dvSearchForLicensed").hide();
            $(prefix + "dvSearchForAddress").hide();
            $(prefix + "dvSearchForTradeName").hide();
            $(prefix + "dvSearchForContact").hide();
        }
        
        function ChangeType(ddl)
        {
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
                    divId = "dvGenearlSearch";
                    sectionId = "60007";
                    lblInstruction.labelKey = 'per_permitlist_instruction_generalsearch';
                    pf = "generalSearchForm_";
                    break;
                case '1':
                    divId = "dvSearchForAddress";
                    sectionId = "60010";
                    lblInstruction.labelKey = 'per_permitlist_instruction_searchbyaddress';
                    pf = "searchByAddress_";
                    break;
                case '2':
                    divId = "dvSearchForLicensed";
                    sectionId = "60009";
                    lblInstruction.labelKey = 'per_permitlist_instruction_searchbylicense';
                    break;
                case '3':
                    divId = "dvSearchForPermit";
                    sectionId = "60008";
                    lblInstruction.labelKey = 'per_permitlist_instruction_searchbypermit';
                    break;
                case '4':
                    divId = "dvSearchForTradeName";
                    sectionId = "60057";
                    lblInstruction.labelKey = 'per_permitlist_instruction_searchbytradename';
                    break;
                case '5':
                    divId = "dvSearchForContact";
                    sectionId = "60065";
                    pf = 'contactSearchForm_';
                    lblInstruction.labelKey = 'per_permitlist_instruction_searchbycontact';
                    break;
            }
            $(prefix + divId).show();
            ddl.SectionInfo.Add(ddl.parentObj._sectionIdValue, ddl.parentObj._sectionFields);
            var sectionIdValue = "<%= ModuleName%>" + "\f" + sectionId + "\fctl00_PlaceHolderMain_" + pf;
            ddl.parentObj._sectionFields = ddl.SectionInfo.GetByKey(sectionIdValue);
            ddl.parentObj._sectionIdValue = sectionIdValue;

            var storedInstruction = ddl.SectionInfo.GetByKey(lblInstruction.labelKey);
            if (!storedInstruction) {
                PageMethods.GetInstructionByKey(lblInstruction.labelKey, "<%= ModuleName%>", function(value) {
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
</div>
</asp:Content>

