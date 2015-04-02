<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParcelLookupForm.ascx.cs" Inherits="Accela.ACA.Web.Component.ParcelLookupForm" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="uc1" %>
<asp:UpdatePanel ID="ParcelLookupEditPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Parcel_ParcelNumber" runat="server" LabelKey="APO_Search_by_Parcel_ParcelNumber"
                CssClass="ACA_NLonger" MaxLength="24"></ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Parcel_Lot" runat="server" LabelKey="APO_Search_by_Parcel_Lot"
                CssClass="ACA_NLonger" MaxLength="40"></ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Parcel_Block" runat="server" LabelKey="APO_Search_by_Parcel_Block"
                CssClass="ACA_NShot" MaxLength="15"></ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAPO_Search_by_Parcel_Subdivision" runat="server" LabelKey="APO_Search_by_Parcel_Subdivision"
                CssClass="ACA_NShot" MaxLength="40"></ACA:AccelaTextBox>
            <ACA:AccelaCheckBox ID="ddlAPO_Search_by_Parcel_Status" runat="server" LabelKey="aca_searchbyparcel_label_status" />
            <uc1:TemplateEdit ID="templateEdit" IsForSearch="true" runat="server"/>
        </ACA:AccelaFormDesignerPlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>
