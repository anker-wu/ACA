<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.RefEducationDetail" Codebehind="RefEducationDetail.ascx.cs" %>
<div class="ACA_SmLabel ACA_SmLabel_FontSize">
    <div class="ACA_ConfigInfo ACA_FLeft">
        <table role='presentation' id ="educationDetail" runat="server">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="lblNameTitle" runat="server" CssClass="fontbold" LabelKey="per_refeducationdetail_label_name" Visible="false" />
                    <ACA:AccelaLabel ID="lblName" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <ACA:AccelaLabel ID="lblDegreeTitle" runat="server" CssClass="fontbold" LabelKey="per_refeducationdetail_label_degree"  Visible="false"/>
                    <ACA:AccelaLabel ID="lblDegree" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <ACA:AccelaLabel ID="lblCommentTitle" runat="server" CssClass="fontbold" LabelKey="per_refeducationdetail_label_comment" Visible="false" />
                    <ACA:AccelaLabel ID="lblComment" runat="server" />
                </td>
            </tr>
        </table>
        <br />
    </div>
</div>
