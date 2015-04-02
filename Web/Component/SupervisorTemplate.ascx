<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupervisorTemplate.ascx.cs" Inherits="Accela.ACA.Web.Component.SupervisorTemplate" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="uc1" %>
 <div> 
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
       <div >
           <asp:DataList ID="dlAttributesList4EMSE" runat="server" OnItemDataBound="AttributesList4EMSE_ItemDataBound" role='presentation'>
                <ItemTemplate> 
                    <h1> 
                        <ACA:AccelaLabel ID="lblLicenseAgency" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "agencyCode")) %>'></ACA:AccelaLabel>&nbsp;
                    </h1>
                    <div >
                        <uc1:TemplateEdit ID="SupervisorList"  runat="server" />
                    </div> 
                </ItemTemplate>
            </asp:DataList>
        </div> 
    </ContentTemplate>
    </asp:UpdatePanel>
</div>