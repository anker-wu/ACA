<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DependentCapTypeList.ascx.cs" Inherits="Accela.ACA.Web.Component.DependentCapTypeList" %>
<%@ Import Namespace="Accela.ACA.BLL.Cap" %>
<%@ Import Namespace="Accela.ACA.WSProxy" %>

<asp:Repeater ID="rptDependentCapType" runat="server" OnItemCommand="DependentCapType_ItemCommand">
    <HeaderTemplate>
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="10" />
        <table role='presentation' class="dependent_cap_type" border="0" width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:<%=IsMutipleRecord?"2%":"0px" %>;"></td>
                <td class="desc">
                    <div class="Header_h4 ACA_FLeftForStyle">
                        <ACA:AccelaLabel ID="lblDependentCapType" runat="server" LabelKey="aca_label_receiptpage_dependentcaptype" />
                    </div>
                </td>
            </tr>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td style="width:<%=IsMutipleRecord?"2%":"0px" %>;"></td>
            <td class="item">
                <ACA:AccelaLinkButton ID="btnGoApplyPage" CssClass="NotShowLoading font11px" CommandName="GoToApply" 
                    CommandArgument='<%#string.Format("{0}/{1}/{2}/{3},{4},{5}", Eval("group"), Eval("type"), Eval("subType"), Eval("category"), Eval("alias"), Eval("moduleName")) %>' 
                    Text='<%# CAPHelper.GetAliasOrCapTypeLabel(Container.DataItem as CapTypeModel) %>' runat="server"/>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>