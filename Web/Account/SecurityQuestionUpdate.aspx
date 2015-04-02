<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="SecurityQuestionUpdate.aspx.cs" Inherits="Accela.ACA.Web.Account.SecurityQuestionUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <%
        Response.Expires = -1;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.AddHeader("pragma", "no-cache");
        Response.AddHeader("Cache-Control", "no-cache");
        Response.CacheControl = "no-cache";
        Response.Cache.SetNoStore();
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    %>
    <script type="text/javascript">
        window.history.forward();

        $(document).ready(function () {
            $(".MultipleControl_Vertical .subTable").css('width', '286px');
        });
    </script>

    <div class="ACA_Content">
        <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_securityquestionupdate_label_pageinstruction" LabelType="PageInstruction" runat="server" />
        <ACA:AccelaLabel ID="lblTitle" LabelKey="aca_securityquestionupdate_label_sectiontitle" runat="server" LabelType="SectionTitle" />
        <div class="ACA_TabRow SecurityQuestionUpdate">
            <ACA:AccelaTextBox runat="server" MinLength="4" MaxLength="32" ID="txbUserID" CssClass="UserName ACA_ReadOnly"
                ReadOnly="true" Validate="required" LabelKey="aca_securityquestionupdate_label_username" />
            <table role='presentation' class="QuestionAndAnswers">
                <tr>
                    <td style="width:50%">
                        <ACA:AccelaMultipleControl ValidationIgnoreCase="True" ID="ddlQuestionForDaily" runat="server" ControlWidth="28em" Visible="false" LabelKey="aca_securityquestionupdate_label_question" ChildControlSubLabel="aca_multiplesecurityquestion_label_subquestion" ChildControlSubLabelFullName="aca_multiplesecurityquestion_label_subquestionfullname" IsFieldRequired="true" ChildControlType="Text">
                            <DuplicateValidate NeedValidate="True" MessageLabelKey="aca_securityquestion_edit_msg_duplicate" />                            
                            <TextBoxSet MaxLength="150" TrimValue="True"/>
                        </ACA:AccelaMultipleControl>
                        <ACA:AccelaTextBox ID="txtQuestionForAdmin" CssClass="QuestionForAdmin" runat="server" Visible="False" LabelKey="aca_securityquestionupdate_label_question" IsFieldRequired="true" ShowType="ShowDescription"></ACA:AccelaTextBox>
                    </td>
                    <td>
                        <ACA:AccelaMultipleControl ID="txbAnswerForDaily" runat="server" ControlWidth="28em" Visible="false" LabelKey="aca_securityquestionupdate_label_answer" ChildControlSubLabel="aca_multiplesecurityquestion_label_subanswer" ChildControlSubLabelFullName="aca_multiplesecurityquestion_label_subanswerfullname" IsFieldRequired="true" ChildControlType="Text">
                            <TextBoxSet MaxLength="100"></TextBoxSet>
                        </ACA:AccelaMultipleControl>
                        <ACA:AccelaTextBox ID="txbAnswerForAdmin" CssClass="AnswerForAdmin" runat="server" Visible="False" LabelKey="aca_securityquestionupdate_label_answer" MaxLength="20" IsFieldRequired="true"></ACA:AccelaTextBox>
                    </td>
                </tr>
            </table>
            <ACA:AccelaButton ID="btnSubmit" runat="server" CausesValidation="true" LabelKey="aca_securityquestionupdate_label_submit" 
                DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" OnClick="SubmitButton_Click"></ACA:AccelaButton>
        </div>
    </div>
</asp:Content>