<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="RelatedRecords.aspx.cs" Inherits="Accela.ACA.Web.Cap.RelatedRecords" %>
<%@ Register Src="~/Component/CapList.ascx" TagName="CapList" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <script type="text/javascript">
        var CTreeTop = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
        var ETreeTop = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
        var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>';
        var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';
    </script>
    <script type="text/javascript" src="../Scripts/RelatedRecordsTree.js"></script>
    <div class="ACA_SmButton ACA_SmButton_FontSize">
        <ACA:AccelaButton ID="btnChange" CommandArgument="true" OnClick="ChangeButton_Click"
            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" LabelKey="aca_relatedrecord_label_entiretree" runat="server">
        </ACA:AccelaButton>
    </div>
    <ACA:AccelaHeightSeparate ID="heightSeparater" Height="5" runat="server" />
    <asp:Literal ID="litCapTree" runat="server"></asp:Literal>
</asp:Content>