<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.RefExaminationDetail" Codebehind="RefExaminationDetail.ascx.cs" %>
<div class="ACA_SmLabel ACA_SmLabel_FontSize">
    <div class="ACA_ConfigInfo ACA_FLeft">
        <table role='presentation'>
            <tr>
                <td>
                    <ACA:AccelaLabel ID="lblExamNameTitle" runat="server" CssClass="fontbold" LabelKey="per_refexaminationdetail_exam_name" Visible="false" />
                    <ACA:AccelaLabel ID="lblExamName" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <ACA:AccelaLabel ID="lblCommentTitle" runat="server" CssClass="fontbold" LabelKey="per_refexaminationdetail_comments" Visible="false" />
                    <ACA:AccelaLabel ID="lblComment" runat="server" />
                </td>
            </tr>
        </table>
        <br />
    </div>
</div>
