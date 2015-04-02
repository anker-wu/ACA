<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" Inherits="Accela.ACA.Web.GeneralProperty.LicenseeDetail"
    ValidateRequest="false" CodeBehind="LicenseeDetail.aspx.cs" %>

<%@ Register Src="../Component/LicenseeGeneralInfo.ascx" TagName="GeneralInfo" TagPrefix="uc1" %>
<%@ Register Src="~/Component/CapList.ascx" TagName="PermitList" TagPrefix="uc1" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="uc2" %>
<%@ Register Src="~/Component/EducationList.ascx" TagName="EducationList" TagPrefix="uc3" %>
<%@ Register Src="~/Component/ExaminationList.ascx" TagName="ExaminationList" TagPrefix="uc5" %>
<%@ Register Src="~/Component/ContinuingEducationList.ascx" TagName="ContEducationList"
    TagPrefix="uc6" %>
<%@ Register Src="~/Component/ContinuingEducationSummaryList.ascx" TagName="ContEducationSummaryList"
    TagPrefix="uc7" %>
<%@ Register Src="~/Component/SectionHeader.ascx" TagName="SectionHeader" TagPrefix="uc1"%>
    
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">

    <script type="text/javascript">
       var CTreeTop='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_collapsed.gif") %>';
       var ETreeMiddle='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("plus_expand.gif") %>';
       var ETreeTop='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_expanded.gif") %>';
       var CTreeMiddle='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("minus_collapse.gif") %>';
       var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>';
       var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';

         function ExpandAttachmentSection() {
             var iframe = $("[id$=iframeAttachmentList]")[0];

             if (typeof (iframe) == "undefined" || iframe == null || iframe.contentWindow.document.body == null) {
                 return;
             }

             // set the attachmentlist iframe's width to its container.
             iframe.width = $('#<%= divAttachment.ClientID %>').width();
             var collapse = '<%=GetTextByKey("inspection_resultcommentlist_collapselink") %>'; 
             var readMore = '<%=GetTextByKey("inspection_resultcommentlist_readmorelink") %>';
             iframe.contentWindow.RebuildEllipsis({readMore:readMore,collapse:collapse,containerId:iframe.id});
             var iframeBody = iframe.contentWindow.document.compatMode == 'BackCompat'
                ?  iframe.contentWindow.document.body
                :  iframe.contentWindow.document.documentElement;

             /*
             Attachment list horizontal scroll will be hidden by iframe's wrong-height when adding more columns in ACA Admin immediately.
             So hard-code for admin.
             */ 
             iframe.height = $.global.isAdmin ? 62 : iframeBody.scrollHeight;
         }
     
        function popUpDetailDialog(pageUrl, objectTargetID) {
            var popupDialogWidth = 680;
            var popupDialogHeight = 600;
            ACADialog.popup({ url: pageUrl, width: popupDialogWidth, height: popupDialogHeight, objectTarget: objectTargetID });
        }

    </script>

    <iframe width="0" height="0" id="iframeExport" style="visibility: hidden" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
    <div id="MainContent" class="ACA_Content">
         <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_licensee_detail" LabelType="PageInstruction"
                runat="server" />
        <h1>
            <ACA:AccelaLabel ID="lblTitle" runat="server" LabelKey="per_licensee_label_title" />
            <br />
            <ACA:AccelaLabel ID="lblPropertyInfo" runat="server" />
        </h1>
        <br />
        <div id="divCondition" runat="server" visible="false">
            <asp:UpdatePanel ID="upConditions" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:Conditions ID="ucConditon" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <ACA:AccelaHeightSeparate ID="sepForCondition" runat="server" Height="10"></ACA:AccelaHeightSeparate>
        <!--Licensee General Information Begin-->
        <div>
        <asp:UpdatePanel ID="upGeneralInfo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <uc1:GeneralInfo ID="licenseeGeneralInfo" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <!--Licenseee General Information End-->
    
        <!--Related Permits Begin-->
        <div runat="server" id="divRelatedPermits" visible="false">   
            <div class="ACA_TabRow">&nbsp;</div>
            <uc1:SectionHeader runat="server" ID="shRelatedPermit" TitleLabelKey="per_licensee_label_relatedpermit" Collapsible="true"
                SectionBodyClientID="divRelatedPermitList">
            </uc1:SectionHeader>

            <div id="divRelatedPermitList" style="display: none">
                <asp:UpdatePanel ID="updatePanelRelatedPermits" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap" runat="server" id="dvPromptForLicense" visible="false">
                            <ACA:AccelaLabel ID="lblPromptForLicense" runat="server" CssClass="font13px" LabelKey="per_permitList_label_Permit_By_License"></ACA:AccelaLabel>
                        </div>
                        <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap" id="dvResult" visible="false" runat="server"
                            style="height: auto;">
                            <ACA:AccelaLabel ID="lblResult" IsNeedEncode="false" CssClass="font13px" runat="server" Style="font-weight: bold;" />
                        </div>
                        <uc1:PermitList ID="dgvPermitList" runat="server" OnGridViewSort="PermitList_GridViewSort"
                                OnPageIndexChanging="DgvPL_PageIndexChanging" ShowPermitAddress="true" GViewID="60103"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!--Related Permits End-->
       
        <!--Education Begin-->
        <div id="divEducation" runat="server" visible="false"> 
            <div class="ACA_TabRow">&nbsp;</div>
            <uc1:SectionHeader runat="server" ID="shEducation" TitleLabelKey="per_detail_education_section_name" Collapsible="true"
                SectionBodyClientID="divEducationList">
            </uc1:SectionHeader>
            <div id="divEducationList" style="display: none;">
                <asp:UpdatePanel ID="updatePanelEducation" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc3:EducationList ID="educationList" GViewID="60124" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!--Education End-->
        
        <!--Continuing Education Begin-->
        <div id="divContEducation" runat="server" visible="false">
            <div class="ACA_TabRow">&nbsp;</div>
            <uc1:SectionHeader runat="server" ID="shContEducation" TitleLabelKey="continuing_education_capdetail_section_name" Collapsible="true"
                SectionBodyClientID="divContEducationList">
            </uc1:SectionHeader>
            <div id="divContEducationList" style="display: none;">
                <asp:UpdatePanel ID="updatePanelContEducation" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc7:ContEducationSummaryList ID="contEducationSummaryList" runat="server" />
                        <br />
                        <uc6:ContEducationList ID="contEducationList" GViewID="60125" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!--Continuing Education End-->
       
        <!--Examination Begin-->
        <div id="divExamination" runat="server" visible="false">
            <div class="ACA_TabRow">&nbsp;</div>
            <uc1:SectionHeader runat="server" ID="shExamination" TitleLabelKey="examination_title" Collapsible="true"
                SectionBodyClientID="divExaminationList">
            </uc1:SectionHeader>
            <div id="divExaminationList" style="display: none;">
                <asp:UpdatePanel ID="updatePanelExamination" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc5:ExaminationList ID="ExaminationList" runat="server" ExaminationSectionPosition="CapDetail" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!--Examination End-->
        <div class="ACA_TabRow">
            &nbsp;</div>
        <!--Attachment Begin-->
        <div id="divAttachment" runat="server" visible="false">          
            <uc1:SectionHeader runat="server" ID="shAttachment" TitleLabelKey="attachment_title" Collapsible="true"
                OnClientExpanded="ExpandAttachmentSection();" SectionBodyClientID="divAttachmentList">
            </uc1:SectionHeader>

            <div id="divAttachmentList" style="display: none;" class="ACA_TabRow_NoScoll ACA_WrodWrap">
                <div class="ACA_TabRow">
                   <ACA:AccelaLabel ID="AccelaLabel3" LabelKey="attachment_label_attachList" runat="server" CssClass="ACA_TabRow_Italic" LabelType="SubSectionText"></ACA:AccelaLabel>
                </div>
                <asp:PlaceHolder ID="phAttachment" runat="server"></asp:PlaceHolder>
            </div>
        </div>
        <!--Attachment End-->
    </div>

    <script type="text/javascript">
    var IsShowlicenseCondition = false;
    var initialLicenseConditionStatus="0";

    function showMorelicenseCondition(div,a)
    {
        if(div=='undefined') return;
        if(initialLicenseConditionStatus=="0"){
            IsShowlicenseCondition=false;
            initialLicenseConditionStatus="1";
        }
        $get(div).style.display= IsShowlicenseCondition?'none': '';
        $get(a).innerHTML = IsShowlicenseCondition?'<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>':'<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
        IsShowlicenseCondition = !IsShowlicenseCondition;
    }

    function EndlicenseRequest(linkId, divConditions) {
      
        var lnk = document.getElementById(linkId);       
        if (lnk != null) 
        {
            if (IsShowlicenseCondition) 
            {
                lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>'
                $get(divConditions).style.display=  '';
            }
            else 
            {
                lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>'                
                $get(divConditions).style.display=  'none';
            }
        }        
    }
    </script>

</asp:Content>
