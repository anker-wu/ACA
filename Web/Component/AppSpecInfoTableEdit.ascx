<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppSpecInfoTableEdit.ascx.cs" Inherits="Accela.ACA.Web.Component.AppSpecInfoTableEdit" %>
 
<div class="ACA_TabRow ACA_ASI_Container">
    <ACA:AccelaLabel ID="labSectionName" runat="server" LabelType="SectionTitle" Visible="False" />
    <asp:DataList runat="server" ID="dlAppInfoTable" OnItemDataBound="TableList_ItemDataBound" Width="100%" role='presentation'>
        <ItemTemplate>
            <asp:UpdatePanel ID="tableEditPanel" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="ACA_ASI_Container">
                        <asp:PlaceHolder ID="phAppInfoTable" EnableTheming="true" runat="server" ></asp:PlaceHolder>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ItemTemplate>
    </asp:DataList>
</div>