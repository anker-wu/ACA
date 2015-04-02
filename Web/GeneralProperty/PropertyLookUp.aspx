<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.GeneralProperty.PropertyLookUp" ValidateRequest="false" Codebehind="PropertyLookUp.aspx.cs" %>

<%@ Register Src="../Component/EducationRelationSearchForm.ascx" TagName="EducationRelationSearchForm"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/RefProviderList.ascx" TagName="RefProviderList" TagPrefix="uc1" %>
<%@ Register Src="../Component/RefEducationList.ascx" TagName="RefEducationList"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/RefContinuingEducationList.ascx" TagName="RefContinuingEducationList"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/RefExaminationList.ascx" TagName="RefExaminationList"
    TagPrefix="uc1" %>
<%@ Register Src="~/Component/RefLicenseeSearchForm.ascx" TagName="RefLicenseeSearchForm"
    TagPrefix="uc1" %>
<%@ Register Src="~/Component/RefLicensedProfessionalList.ascx" TagName="RefLicensedProfessionalList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/CertifiedBusinessList.ascx" TagName="CertifiedBusinessList"
    TagPrefix="uc1" %>
<%@ Register Src="~/Component/DocumentSearchForm.ascx" TagName="DocumentSearchForm"
    TagPrefix="uc1" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<%@ Register Src="~/Component/CertifiedBusinessSearchForm.ascx" TagName="CertifiedBusinessSearchForm" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">

    <script language="javascript" type="text/javascript">
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
                 var ddl = $get('<%=ddlSearchType.ClientID %>'); 
                 ddl.click(); 
             }
             
             //export file.
             ExportCSV(sender, args);
         }
        
        function beginRequest()
        {
            prm._scrollPosition = null;
        }
    </script>

    <div class="ACA_Content">
        <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
        <h1>
            <ACA:AccelaLabel ID="lblSearchTypeTitle" runat="server" LabelKey="per_educationlookup_label_title" /></h1>
        <div class="ACA_Page ACA_Page_FontSize">
            <ACA:AccelaLabel ID="lblSearchDescription" runat="server" IsNeedEncode="false" LabelType="bodyText"
                LabelKey="per_educationlookup_label_des" />
        </div>
        <div>
            <asp:UpdatePanel ID="updatePanel" runat="server">
                <ContentTemplate>
                 <a name="SearchForm_Start" id="SearchForm_Start"></a>
                    <div class="ACA_InfoTitle ACA_InfoTitle_FontSize">
                        <h1>
                            <span class="ACA_FLeft" ID="lblSelectedSearchType" runat="server"></span>
                        </h1>
                        <span class="ACA_FRight">
                            <ACA:AccelaDropDownList ID="ddlSearchType" runat="server" OnSelectedIndexChanged="SearchTypeDropDown_IndexChanged"
                                AutoPostBack="true" SourceType="HardCode" ToolTipLabelKey="aca_common_msg_dropdown_changesearchoption_tip" />
                        </span>
                    </div>
                    <div id="divInstruction" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize" runat="server">
                            <ACA:AccelaLabel ID="lblSearchInstruction" runat="server" IsNeedEncode="false"></ACA:AccelaLabel>
                        </div>

                    <div id="divSearchPaneltoPressEnter" class="ACA_TabRow">
                        <div id="divLicenseeSearchForm" runat="server" visible="false">
                            <uc1:RefLicenseeSearchForm ID="refLicenseeSearchForm" SearchType="Search4Licensee" runat="server" />
                        </div>
                        <div id="divFoodFacilitySearchForm" runat="server" visible="false">
                            <uc1:RefLicenseeSearchForm ID="refFoodFacilitySearchForm" SearchType="Search4FoodFacilityInspection" runat="server" />
                        </div>
                        <div id="divCertBusinessSearchForm" runat="server" visible="false">
                            <uc1:CertifiedBusinessSearchForm ID="certifiedBusinessSearchForm" runat="server" />
						</div>
						<div id="divEduRelationSearchForm" runat="server" visible="false">
                        	<uc1:EducationRelationSearchForm ID="educationRelationSearchForm" SearchType="Search4Provider" runat="server" />
						</div>
                        <div id="divRefExamSearchForm" runat="server" visible="false">
                        	<uc1:EducationRelationSearchForm ID="refExamSearchForm" SearchType="Search4EduAndExam" runat="server" />
						</div>
                        <div id="divDocumentSearchForm" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server" visible="false">
                            <uc1:DocumentSearchForm ID="documentSearchForm" runat="server" />
                        </div>
                    </div>

                    <div class="ACA_TabRow">  &nbsp;</div>
                    <div class="ACA_Row ACA_LiLeft">
                        <div>
                            <ul>                      
                                <li>
                                    <ACA:AccelaButton ID="btnNewSearch" runat="server" OnClick="NewSearchButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" LabelKey="per_educationlookup_label_buttonSearch"></ACA:AccelaButton>
                                </li>
                                <li>
                                    <ACA:AccelaButton ID="btnResetSearch" runat="server" OnClick="ResetSearchButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" CausesValidation="false" LabelKey="aca_generalsearch_label_resetsearch"></ACA:AccelaButton>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div id="divHrforResult" class="ACA_TabRow ACA_Line_Content" runat="server" visible="false">
                        &nbsp;
                    </div>
                    <div class="ACA_Row">
                        <uc1:MessageBar ID = "noResultMessage" runat="Server" />
                    </div>
                    <a name="PageResult" id="PageResult" href="#PageResult" tabindex="-1"></a>
                    <div id="divProviderResult" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server" visible="false">
                        <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                            <ACA:AccelaLabel ID="lblCountNumResult" runat="server" CssClass="ACA_RefEducation_Font font13px" />
                            <ACA:AccelaLabel ID="lblCountProviderCount" CssClass="ACA_RefEducation_Font font13px" runat="server"
                                LabelKey="per_educationlookup_label_providercount" />
                        </div>
                        <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                            <ACA:AccelaLabel ID="lblClickPrompt" runat="server" CssClass="font13px" LabelKey="per_educationlookup_label_click_prompt" />
                        </div>
                        <uc1:RefProviderList ID="refProviderList" runat="server" />
                    </div>
                    <div id="divEducationResult" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server" visible="false">
                        <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                            <ACA:AccelaLabel ID="lblCountEducationResult" runat="server" CssClass="ACA_RefEducation_Font font13px" />
                            <ACA:AccelaLabel ID="lblCountEducationCount" CssClass="ACA_RefEducation_Font font13px" runat="server"
                                LabelKey="per_educationlookup_label_educationcount" />
                        </div>
                        <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                                <ACA:AccelaLabel ID="lblClickEduPrompt" runat="server" CssClass="font13px" LabelKey="per_educationlookup_education_prompt" />
                        </div>
                        <uc1:RefEducationList ID="refEducationList" runat="server" />
                    </div>
                    <div id="divContEduResult" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server" visible="false">
                        <div class="ACA_TabRow">
                            <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                                <ACA:AccelaLabel ID="lblCountNumContEduResult" runat="server" CssClass="ACA_RefEducation_Font font13px" />
                                <ACA:AccelaLabel ID="lblCountCoutEduCount" CssClass="ACA_RefEducation_Font font13px" runat="server"
                                    LabelKey="per_educationlookup_label_contedu_count"/>
                            </div>
                            <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                                <ACA:AccelaLabel ID="lblClickContEduPrompt" runat="server" CssClass="font13px" LabelKey="per_educationlookup_label_contedu_promt"/>
                            </div>
                        </div>
                        <uc1:RefContinuingEducationList ID="refContinuingEducationList" runat="server" />
                    </div>
                    <div id="divExamResult" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server" visible="false">
                        <div class="ACA_TabRow">
                            <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                                <ACA:AccelaLabel ID="lblExamContEduResult" runat="server" CssClass="ACA_RefEducation_Font font13px" />
                                <ACA:AccelaLabel ID="lblExamEdurCount" CssClass="ACA_RefEducation_Font font13px" runat="server" LabelKey="per_educationlookup_label_examcount" />
                            </div>
                            <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                                <ACA:AccelaLabel ID="lblClickExamPrompt" CssClass="font13px" runat="server" LabelKey="per_educationlookup_label_exam_promt" />
                            </div>
                        </div>
                        <uc1:RefExaminationList ID="refExaminationList" runat="server" />
                    </div>
                    <div id="divRefLicenseeResult" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server" visible="false">
                            <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                               <ACA:AccelaLabel ID="lblLicenseeResult" CssClass="ACA_RefEducation_Font font13px" runat="server"/>
                               <ACA:AccelaLabel ID="lblLicenseeCount" CssClass="ACA_RefEducation_Font font13px" runat="server"
                                 LabelKey="per_propertylookup_licenseecount" />
                            </div>
                            <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                                <ACA:AccelaLabel ID="lblLicenseePrompt" CssClass="font13px" runat="server" LabelKey="per_propertylookup_licensepromt" />
                            </div>
                             <ACA:RefLicensedProfessionalList ID="refLicenseeList" SearchType="Search4Licensee" runat="server" GViewID="60087" />
                    </div>
                    <div id="divFoodFacilityResult" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server" visible="false">
                            <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                               <ACA:AccelaLabel ID="lblFoodFacilityResult" CssClass="ACA_RefEducation_Font font13px" runat="server"/>
                               <ACA:AccelaLabel ID="lblFoodFacilityCount" CssClass="ACA_RefEducation_Font font13px" runat="server" LabelKey="aca_foodfacility_label_searchcount" />
                            </div>
                            <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                                <ACA:AccelaLabel ID="lblFoodFacilityPrompt" CssClass="font13px" runat="server" LabelKey="aca_foodfacility_label_searchprompt" />
                            </div>
                             <ACA:RefLicensedProfessionalList ID="refFoodFacilityList" SearchType="Search4FoodFacilityInspection" runat="server" GViewID="60116" />
                    </div>
                    <div id="divCertBusinessResult" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server" visible="false">
                        <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                            <ACA:AccelaLabel ID="lblCertBusinessCount" CssClass="ACA_RefEducation_Font font13px" runat="server" />
                            <ACA:AccelaLabel ID="lblCertBusinessResult" CssClass="ACA_RefEducation_Font font13px" runat="server" LabelKey="aca_certbusinesslist_label_searchcount" />
                        </div>
                        <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                            <ACA:AccelaLabel ID="lblCertBusinessPrompt" runat="server" CssClass="font13px" LabelKey="aca_certbusinesslist_label_instruction" />
                        </div>
                        <uc1:CertifiedBusinessList ID="certBusinessList" runat="server" />
                    </div>
                    
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
            }
        });

        var prefix = "#ctl00_PlaceHolderMain_";
        if ($.global.isAdmin) {
            HideAllSections();
            var lblSearchInstruction = $get("<%=lblSearchInstruction.ClientID%>");

            if('<%=Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(Request["isLicensee"])%>' == 'Y')
            {
                $(prefix + "divLicenseeSearchForm").show();
                lblSearchInstruction.labelKey = "per_propertylookup_instruction_searchforlicensee";
            }
            else if ('<%=Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(Request["isFoodFacility"])%>' == 'Y') {
                $(prefix + "divFoodFacilitySearchForm").show();
                lblSearchInstruction.labelKey = "aca_foodfacility_label_searchinstruction";
            }
            else if ('<%=Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(Request["isCertBusiness"])%>' == 'Y') {
                $(prefix + "divCertBusinessSearchForm").show();
                lblSearchInstruction.labelKey = "aca_certbusiness_label_searchinstruction";
            }
            else if ('<%=Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(Request["isSearchDocument"])%>' == 'Y') {
                $(prefix + "divDocumentSearchForm").show();
                lblSearchInstruction.labelKey = "aca_searchdocument_label_searchinstruction";
                $('#<%=btnNewSearch.ClientID %>').hide();
            }
            else
            {
                $(prefix + "divEduRelationSearchForm").show();
                lblSearchInstruction.labelKey = "per_propertylookup_instruction_searchforprovider";
                
            }
            
            $get("<%=ddlSearchType.ClientID %>").SectionInfo = new KeyValuePair();
        }

        function HideAllSections() {
            $(prefix + "divLicenseeSearchForm").hide();
            $(prefix + "divEduRelationSearchForm").hide();
            $(prefix + "divRefExamSearchForm").hide();
            $(prefix + "divFoodFacilitySearchForm").hide();
            $(prefix + "divCertBusinessSearchForm").hide();
            $(prefix + "divDocumentSearchForm").hide();
        }

        function ChangeType(ddl) {
            HideAllSections();
            var divId;
            var sectionId;
            var elementPrefix;
            var myArray = ddl.value.split("||");
            var str = myArray[myArray.length - 1];
            $get("<%=lblSelectedSearchType.ClientID%>").innerHTML = ddl[ddl.selectedIndex].text;
            var selectedSection = myArray[0].replace("-", "");
            var lblInstruction = $get("<%=lblSearchInstruction.ClientID%>");
            $('#<%=btnNewSearch.ClientID %>').show();
            $('#<%=btnResetSearch.ClientID %>').show();
            
            switch (selectedSection) {
                case '0':
                    divId = "divEduRelationSearchForm";
                    sectionId = "60075";
                    elementPrefix = "<%=educationRelationSearchForm.ClientID %>_";
                    lblInstruction.labelKey = 'per_propertylookup_instruction_searchforprovider';
                    break;
                case '1':
                    divId = "divRefExamSearchForm";
                    sectionId = "60076";
                    elementPrefix = "<%=refExamSearchForm.ClientID %>_";
                    lblInstruction.labelKey = 'per_propertylookup_instruction_searchforeducation';                   
                    break;
                case '2':
                    divId = "divLicenseeSearchForm";
                    sectionId = "60086";
                    elementPrefix = "<%=refLicenseeSearchForm.ClientID %>_";
                    lblInstruction.labelKey = 'per_propertylookup_instruction_searchforlicensee';
                    break;
                case '3':
                    divId = "divFoodFacilitySearchForm";
                    sectionId = "60115";
                    elementPrefix = "<%=refFoodFacilitySearchForm.ClientID %>_";
                    lblInstruction.labelKey = 'aca_foodfacility_label_searchinstruction';
                    break;
                case '4':
                    divId = "divCertBusinessSearchForm";
                    sectionId = "60119";
                    elementPrefix = "<%=certifiedBusinessSearchForm.ClientID %>_";
                    lblInstruction.labelKey = 'aca_certbusiness_label_searchinstruction';
                    break;
                case '5':
                    divId = "divDocumentSearchForm";
                    elementPrefix = "<%=documentSearchForm.ClientID %>_";
                    lblInstruction.labelKey = 'aca_searchdocument_label_searchinstruction';
                    $('#<%=btnNewSearch.ClientID %>').hide();
                    $('#<%=btnResetSearch.ClientID %>').hide();
                    break;
            }

            var storedInstruction = ddl.SectionInfo.GetByKey(lblInstruction.labelKey);
            if (!storedInstruction) {
                PageMethods.GetInstructionByKey(lblInstruction.labelKey, function(value)
                {
                   lblInstruction.innerHTML = value;
                   ddl.SectionInfo.Add(lblInstruction.labelKey, value);
                });
            }
            else
            {
               lblInstruction.innerHTML = storedInstruction;
            }
            
            $(prefix + divId).show();
            ddl.SectionInfo.Add(ddl.parentObj._sectionIdValue, ddl.parentObj._sectionFields);
            var sectionIdValue = "<%= ConfigManager.AgencyCode%>" + "\f" + sectionId + "\f" + elementPrefix;

            if (selectedSection == "5") {
                sectionIdValue = "<%= ConfigManager.AgencyCode%>" + "\f" + elementPrefix;
            }

            ddl.parentObj._sectionFields = ddl.SectionInfo.GetByKey(sectionIdValue);
            ddl.parentObj._sectionIdValue = sectionIdValue;

            parent.parent.LoadProperties(16, ddl.parentObj);
        }
    </script>
</asp:Content>
