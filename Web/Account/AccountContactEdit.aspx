<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    CodeBehind="AccountContactEdit.aspx.cs" Inherits="Accela.ACA.Web.Account.AccountContactEdit" %>
<%@ Import Namespace="Accela.ACA.Common" %>

<%@ Register Src="~/Component/ContactInfo.ascx" TagName="ContactInfo" TagPrefix="uc1" %>
<%@ Register TagPrefix="ACA" TagName="EducationEdit" Src="~/Component/EducationEdit.ascx" %>
<%@ Register TagPrefix="ACA" TagName="ContinuingEducationEdit" Src="~/Component/ContinuingEducationEdit.ascx" %>
<%@ Register TagPrefix="ACA" TagName="ExaminationEdit" Src="~/Component/ExaminationEdit.ascx" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagPrefix="ACA" TagName="MessageBar" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<script src="../Scripts/Expression.js" type="text/javascript"></script>
<script src="../Scripts/GeneralNameList.js" type="text/javascript"></script>
<script type="text/javascript">
    function PopupClose() {
        ReloadPage();
    }

    function ReloadPage() {
        window.location.href = '<%= ReloadPage %>';
        ShowLoading();
    }

    function popUpDetailDialog(pageUrl, objectTargetID) {
        var popupDialogWidth = 680;
        var popupDialogHeight = 600;
        ACADialog.popup({ url: pageUrl, width: popupDialogWidth, height: popupDialogHeight, objectTarget: objectTargetID });
    }
    
    function changeContactType() {
        var p = new ProcessLoading();
        p.showLoading();
        PageMethods.EditContactByTypeDisabled(function (info) {
            url = '<%= Page.ResolveUrl("~/People/ContactTypeSelect.aspx") %>';
            url += '?<%= ACAConstant.MODULE_NAME + "=" + ModuleName %>';
            url += '&<%= UrlConstant.AgencyCode + "=" + ConfigManager.AgencyCode %>';
            ACADialog.popup({ url: url, width: 400, height: 200, objectTarget: '<%=MessageBar.ClientID%>' });
        });
    }

    function updateContactType(contactType) {
        var p = new ProcessLoading();
        p.showLoading();
        __doPostBack('<%=btnEditContactType.UniqueID %>', contactType);
    }
</script>
    <div class="ACA_Content">
        <asp:UpdatePanel runat="server" ID="pnlEditContactType">
            <ContentTemplate>
                <div class="Body">
                    <ACA:MessageBar runat="server" ID="MessageBar" />
                    <div class="ACA_TabRow">
                        <h1>
                            <ACA:AccelaLabel ID="lblTitle" LabelKey="acc_manage_label_accountManage" runat="server" />
                        </h1>
                        <h1>
                            <ACA:AccelaLabel ID="lblSubTitle" LabelKey="aca_account_manage_label_contact_detail_title" runat="server" Visible="false" />
                        </h1>
                    </div>
                    <br />
                    <div class="AccountContactMainForm">
                        <p>
                            <ACA:AccelaLabel ID="acc_manage_text_accountInfo" LabelKey="aca_account_manage_label_contact_detail_desc" LabelType="BodyText" runat="server" Visible="false" />
                        </p>
                        <br />
                        <table role='presentation'>
                            <tr>
                                <td>
                                    <div>
                                        <ACA:AccelaButton ID="lnkCreateAmendment" EnableRecordTypeFilter="true" LabelKey="aca_account_management_contact_amendment" runat="server" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                                            Visible="false" OnClick="CreateAmendmentButton_Click" CausesValidation="false">
                                        </ACA:AccelaButton>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <ACA:AccelaHeightSeparate ID="divSeparate" Height="5" runat="server"></ACA:AccelaHeightSeparate>
                        <div>
                            <uc1:ContactInfo ID="ucContactInfo" IsMultipleContact="True" runat="server" />
                        </div>
                        <br/>
                        <div class="ACA_Row ACA_LiLeft Footer">
                            <ul>
                                <li>
                                    <ACA:AccelaButton ID="btnSave" LabelKey="aca_accountcontactedit_label_save" OnClick="SaveButton_Click" OnClientClick='return SubmitEP(this);' runat="server"
                                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"></ACA:AccelaButton>
                                </li>
                                <li>
                                    <ACA:AccelaLinkButton ID="lnkBack" CausesValidation="False" LabelKey="aca_accountcontactedit_label_back"
                                        OnClientClick="ReloadPage();return false;" runat="server" CssClass="ACA_LinkButton" />
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <ACA:AccelaLinkButton runat="server" Visible="False" ID="btnEditContactType" OnClick="UpdateContactType_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="divEduExamCEInfo" runat="server" Visible="False" class="Account_CertificationList">
            <ACA:AccelaLabel ID="lblEducationTitle" LabelKey="aca_contact_educationlist_label_section_name" LabelType="SectionTitle" runat="server" />
            <div>
                <ACA:EducationEdit ID="educationEdit" IsEditable="True" EducationSectionPosition="AccountContactEdit" runat="server"></ACA:EducationEdit>
            </div>
            <div class="Section">
                <ACA:AccelaLabel ID="lblExaminationTitle" LabelKey="aca_contact_examinationlist_label_section_name" LabelType="SectionTitle" runat="server" />
                <div>
                    <ACA:ExaminationEdit ID="examinationEdit" IsEditable="True" ExaminationSectionPosition="AccountContactEdit" runat="server"></ACA:ExaminationEdit>
                </div>
            </div>
            <div class="Section">
                <ACA:AccelaLabel ID="lblContEducationTitle" LabelKey="aca_contact_conteducationlist_label_section_name" LabelType="SectionTitle" runat="server" />
                <div>
                    <ACA:ContinuingEducationEdit ID="continuingEducationEdit" IsEditable="True" ContEducationSectionPosition="AccountContactEdit" runat="server"></ACA:ContinuingEducationEdit>
                </div>
            </div>
            <div class="ACA_Row ACA_LiLeft Footer">
                <ul>
                    <li>
                        <ACA:AccelaLinkButton ID="btnBack2" CausesValidation="False" LabelKey="aca_accountcontactedit_label_back"
                            OnClientClick="ReloadPage();return false;" runat="server" CssClass="ACA_LinkButton"/>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
