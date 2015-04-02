<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" 
Inherits="Accela.ACA.Web.MyCollection.MyCollectionDetail" ValidateRequest="false" Codebehind="MyCollectionDetail.aspx.cs" %> 

<%@ Register src="~/Component/MyCollectionSummary.ascx" tagname="MyCollectionSummary" tagprefix="ACA" %> 
<%@ Register src="~/Component/MyCollectionCAPs.ascx" tagname="MyCollectionCAPs" tagprefix="ACA" %>
<%@ Register src="~/Component/CapList.ascx" TagName="CapList" TagPrefix="ACA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" Runat="Server">  
 <%
    Response.Buffer = true;
    Response.Expires = 0;
    Response.CacheControl = "no-cache";
    Response.AddHeader("Pragma", "No-Cache");
%>
 <script type="text/javascript" src="../Scripts/MyCollectionMethods.js"></script>
 
<asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<div>
    <div class="ACA_Content">
         <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_collection_detail" LabelType="PageInstruction"
                runat="server" />
        <div>  
            <ACA:MyCollectionSummary ID="SummaryInformation" runat="server" />
        </div>
        <div>
           <asp:DataList ID="dlCAPsGroupByModule" runat="server" OnItemDataBound="CAPsGroupByModule_ItemDataBound" role='presentation'>
                <ItemTemplate> 
                   <ACA:MyCollectionCAPs ID="CAPsForMyCollection" Windowless="true" runat="server" /> 
                </ItemTemplate>
            </asp:DataList>
        </div> 
    </div>
</div>
<div id="divAdded" class="ACA_Loading_Message ACA_SmLabel ACA_SmLabel_FontSize">
</div>
<ACA:CapList ID="capList" ShowPermitAddress="true" runat="server" Windowless="true" GViewID="60033" Visible="False"/>
<script type="text/javascript">
    if (typeof (ExportCSV) != 'undefined') {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(ExportCSV);
    }
</script>
</ContentTemplate>
</asp:UpdatePanel>
<iframe width="0" height="0" id="iframeExport" style="display:none;" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
</asp:Content>