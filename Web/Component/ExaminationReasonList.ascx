<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExaminationReasonList.ascx.cs" Inherits="Accela.ACA.Web.Component.ExaminationReasonList" %>

<ACA:AccelaGridView ID="gvReasonExamination" CssClass="PopUpInspectionRow" runat="server" role="presentation" ShowHeader="false"
        ShowHeaderWhenEmpty="false" AlternatingRowStyle-CssClass="InspectionTypeRow" RowStyle-CssClass="InspectionTypeRow" 
        AllowPaging="True" PagerStyle-VerticalAlign="bottom" AutoGenerateCheckBoxColumn="false" OnRowDataBound="ReasonExamination_RowDataBound"
            OnPageIndexChanging="ReasonExamination_PageIndexChanging" IsAutoWidth="true">
    <Columns>
        <ACA:AccelaTemplateField>
            <ItemTemplate>
                <ACA:AccelaRadioButton ID="rdReasonExamination" Enabled='<%# DataBinder.Eval(Container.DataItem, "Enabled")%>' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReasonString")%>' value='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                        Checked='<%# DataBinder.Eval(Container.DataItem, "Selected")%>' />
            </ItemTemplate>
        </ACA:AccelaTemplateField>
    </Columns>
</ACA:AccelaGridView>
<asp:HiddenField ID="hfSelectedReason" runat="server" />