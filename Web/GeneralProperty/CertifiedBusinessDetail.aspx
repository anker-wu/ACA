<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="CertifiedBusinessDetail.aspx.cs"
    Inherits="Accela.ACA.Web.GeneralProperty.CertifiedBusinessDetail" ValidateRequest="false"%>

<%@ Register Src="~/Component/SectionHeader.ascx" TagName="SectionHeader" TagPrefix="uc1"%>
<%@ Register Src="../Component/CertifiedBusinessGeneralInfo.ascx" TagName="GeneralInfo" TagPrefix="uc1" %>
<%@ Register Src="../Component/CertifiedExperienceList.ascx" TagName="ExperienceList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div id="MainContent" class="ACA_Content">
    <ACA:AccelaLabel ID="lblDetailInstruction" runat="server" LabelKey="aca_certbusiness_label_detail_pageinstruction" LabelType="PageInstruction"/>
    <div id="divNIGPHeader">
        <div id="divTitle" class="ACA_FLeft">
            <h1>
                <ACA:AccelaLabel ID="lblTitle" LabelKey="aca_certbusiness_label_detail_title" runat="server"/>
                <br />
                <ACA:AccelaLabel ID="lblCertifiedBusinessName" runat="server" />
            </h1>
        </div>
        <div id="divPrintBtn" class="ACA_SmButton ACA_SmButton_FontSize ACA_FRight ACA_SmButton_Print ACA_FRight">
            <ACA:AccelaButton ID="btnPrint" runat="server" class="NotShowLoading" LabelKey="aca_certbusiness_label_detail_printbutton" OnClientClick="print_onclick();return false;"></ACA:AccelaButton>
        </div>
        <ACA:AccelaHeightSeparate ID="separate4PrintBtn" runat="server" Height="5"/>
    </div>

    <!--Certified Business General Information Begin-->
    <ACA:AccelaLabel ID="lblCertBusinessGeneralInfoTitle" runat="server" LabelKey="aca_certbusiness_label_detail_details" LabelType="SectionTitle" />
    <uc1:GeneralInfo ID="certifiedBusinessGeneralInfo" runat="server" />
    <!--Certified Business General Information End-->

    <!--Certified Business Experience Begin-->
    <div id="divExperience">
        <div class="ACA_TabRow">&nbsp;</div>
        <uc1:SectionHeader ID="shExperience" TitleLabelKey="aca_certbusiness_label_detail_experience" Collapsible="false" SectionBodyClientID="divExperienceList" runat="server">
        </uc1:SectionHeader>
        <div id="divExperienceList" class="ACA_TabRow_NoScoll ACA_WrodWrap" runat="server">   
            <uc1:ExperienceList ID="experienceList" runat="server"/>
        </div>
    </div>
    <!--Certified Business Experience End-->

    <!--NIGP code Begin-->
    <div>
        <div class="ACA_TabRow">&nbsp;</div>
        <uc1:SectionHeader ID="SH4NIGPcode" TitleLabelKey="aca_certbusiness_label_nigpcode" Collapsible="false" SectionBodyClientID="divNIGPCodeTree" runat="server">
        </uc1:SectionHeader>

        <asp:Repeater ID="rptNigpClass" OnItemDataBound="NigpClassList_ItemDataBound" runat="server">
            <ItemTemplate>
                <div>
                    <div id="div3DigitNigpCode">
                        <h1 class="ACA_Tree_Node">
                            <div id="divNIGPButton" class="ACA_NIGPTree_Button">
                                <a href="javascript:void(0)" class="NotShowLoading" onclick="ShowOrHideSubnode(this)">
                                    <img style="border-width: 0px;" alt="<%=GetTextByKey("img_alt_expand_icon") %>" src="<%=ImageUtil.GetImageURL("plus_expand.gif") %>">       
                                </a>
                            </div>
                            <div id="divNIGPClass" class="ACA_NIGPTree_ThreeDigitalClass">
                                <span style="display: inline-block;"><%#Eval("key")%></span>
                            </div>
                        </h1>
                    </div>
                    <div id="div5DigitNigpCode" class="ACA_Tree_SubNode ACA_Hide">
                        <asp:Repeater ID="rptNigpSubClass" runat="server">
                            <ItemTemplate>
                                <div id="divFiveDigital" class="ACA_NIGPTree_FiveDigitalClass">
                                    <span><%# Container.DataItem %></span>
                                </div>
                                <br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <!--NIGP code End-->
</div> 

<script type="text/javascript">
    var imgCollapsed = '<%=ImageUtil.GetImageURL("minus_collapse.gif") %>';
    var imgExpanded = '<%=ImageUtil.GetImageURL("plus_expand.gif") %>';
    var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>';
    var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';

    function ShowOrHideSubnode(obj) {
        var divSubNode = $(obj).parentsUntil('#div3DigitNigpCode').parent().siblings('#div5DigitNigpCode');

        if (divSubNode.is(':visible')) {
            $(obj).children('img').attr({ src: imgExpanded, alt: altCollapsed });
            divSubNode.hide();
        }
        else {
            $(obj).children('img').attr({ src: imgCollapsed, alt: altExpanded });
            divSubNode.show();
        }
    }

    function print_onclick() {
        var url = "../Print/PrintPage.aspx";
        var a = window.open(url, "_blank", "top=200,left=200,height=550,width=850,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes");
    }

    if (typeof (ExportCSV) != 'undefined') {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequest);
    }

    function EndRequest(sender, args) {
        //export file.
        ExportCSV(sender, args)
    }
</script>
<iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
</asp:Content>
