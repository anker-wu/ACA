<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="Accela.ACA.Web.Account.AccountManager"
    Title="Account Management" ValidateRequest="false" CodeBehind="AccountManager.aspx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.ExpressionBuild" %>

<%@ Register Src="~/Component/AccountView.ascx" TagName="UserInfoView" TagPrefix="ACA" %>
<%@ Register Src="~/Component/RefContactList.ascx" TagName="RefContactList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/TrustAccountList.ascx" TagName="TrustAcctList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/AttachmentEdit.ascx" TagName="AttachmentEdit" TagPrefix="ACA" %>
<%@ Register Src="~/Component/UserLicenseList.ascx" TagName="UserLicenseList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/DelegateUsersView.ascx" TagName="DelegateUsersView" TagPrefix="ACA" %>
<%@ Register Src="~/Component/AgentClerkList.ascx" TagName="AgentClerkList" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/FileUpload.js")%>"></script>
    <script type="text/javascript">
        var NeedAsk = true;

        window.onbeforeunload = function() {
            if (NeedAsk && ('<%=IsEditLogin %>' == "True") && $.global.isAdmin == false) {
                var p = new ProcessLoading();
                p.hideLoading();
                return '<%=GetTextByKey("per_cap_edit_exitAsk").Replace("'","\\'") %>';
            }
        };

        var prm = Sys.WebForms.PageRequestManager.getInstance();
         prm.add_endRequest(EndRequest);

        function EndRequest(sender, args){
            //when attachment upload file saved, no need to hide message.
            if(typeof(sender._postBackSettings.sourceElement) == "undefined"
                || sender._postBackSettings.sourceElement.id.indexOf('<%=attachmentEdit.ClientID %>') < 0){
                hideMessage();
            }
            //export file.
            ExportCSV(sender, args);
        }

         function ShowUserAccountEdit(){
             var url = "UserAccountEdit.aspx";
             var btnId = '<%=btnEditAccount.ClientID %>';
             ACADialog.popup({ url: url, width: 700, height: 550,objectTarget:btnId});
         }
         
         function CreateContactSession() {
            var processShowloading = new ProcessLoading();
            processShowloading.showLoading();
            PageMethods.CreateContactParametersSession('<%= ExpressionType.ReferenceContact.ToString("D")%>','<%=refContactList.ClientID %>',
               '<%=ACAConstant.ContactSectionPosition.AddReferenceContact.ToString("D") %>','<%=ContactProcessType.Add.ToString("D") %>',AddReferenceContact);
        }

         function AddReferenceContact() {
             var btnId = '<%=btnAddContact.ClientID %>';
             var url = '<%=FileUtil.AppendApplicationRoot("People/ContactTypeSelect.aspx") %>?agencyCode=<%=ConfigManager.AgencyCode %>';
             ACADialog.popup({ url: url, width: 400, height: 200, objectTarget: btnId });

             return false;
         }

         function <%=refContactList.ClientID %>_RefreshContactList() {
             __doPostBack('<%=btnAccountContactRefresh.UniqueID %>', null);
         }
         
         function RefreshAttachmentList() {
            var iframe = $("#<%= attachmentEdit.ClientID %>_iframeAttachmentList");

            if (!isNull(iframe[0])) {
                iframe[0].contentWindow.RefreshPage();
            }
        }
    </script>
    <div class="ACA_Content" id="divAccountManager">
        <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>">
            <%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
        <!-- start custom content -->
        <div class="ACA_TabRow" id="divDescription">
            <h1>
                <ACA:AccelaLabel ID="acc_manage_label_accountManage" LabelKey="acc_manage_label_accountManage"
                    runat="server" />
            </h1>
            <p>
                <ACA:AccelaLabel ID="acc_manage_text_accountInfo" LabelKey="acc_manage_text_accountInfo"
                    LabelType="BodyText" runat="server" />
            </p>
        </div>
        <br />
        <ACA:AccelaLabel ID="acc_manage_label_AccountType" LabelKey="acc_manage_label_AccountType"
            runat="server" LabelType="SectionTitle" />
        <p>
            <ACA:AccelaLabel ID="lblAccountType" runat="server" />
        </p>
        <ACA:AccelaHeightSeparate ID="sepHeightForContact" runat="server" Height="10" />
        <div>
            <div id="divAccountBar">
                <div class="ACA_Title_Bar">
                    <div class="ACA_FLeft">
                        <h1>
                            <ACA:AccelaLabel ID="acc_manage_label_accountInfo" LabelKey="acc_manage_label_accountInfo"
                                runat="server" LabelType="SectionTitleWithBar" />
                        </h1>
                    </div>
                    <div class="ACA_FRight">
                        <ACA:AccelaButton ID="btnEditAccount" DivDisableCss="ACA_SmButtonDisable ACA_SmButtonDisable_FontSize"
                            DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" OnClientClick="SetNotAsk();ShowUserAccountEdit();return false;"
                            runat="server" LabelKey="per_permitConfirm_label_editButton" />
                    </div>
                </div>
                <span id='acc_manage_label_accountInfo_sub_label' runat="server" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize">
                </span>
            </div>
            <ACA:UserInfoView ID="accountView" runat="server" />
        </div>
        <!--   User License Begin -->
        <div id="divLicense" runat="server">
            <div id="licenseInfoTitle">
                <div class="ACA_Title_Bar">
                    <div class="ACA_FLeft">
                        <h1>
                            <ACA:AccelaLabel ID="acc_manage_label_licenseInfo" LabelKey="acc_manage_label_licenseInfo"
                                runat="server" LabelType="SectionTitleWithBar" />
                        </h1>
                    </div>
                    <div class="ACA_FRight">
                        <ACA:AccelaButton ID="btnAddLicense" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize"
                            DivDisableCss="ACA_SmButtonDisable ACA_SmButtonDisable_FontSize"
                            runat="server" LabelKey="acc_manage_label_addLicense" OnClientClick="SetNotAsk();"
                            OnClick="AddLicenseButton_Click" />
                    </div>
                </div>
                <span id="acc_manage_label_licenseInfo_sub_label" runat="server" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize" />
            </div>
            <div id="divUserLicenseView" runat="server">
                <asp:UpdatePanel ID="panelLicenseList" UpdateMode="conditional" runat="server">
                    <ContentTemplate>
                        <ACA:UserLicenseList ID="userLicenseList" runat="server" EditState="EDIT" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!--   User License End -->
        <ACA:AccelaHeightSeparate ID="sepLicenseView" runat="server" Height="10" />
        <div>
            <div id="divContactBar">
                <div class="ACA_Title_Bar">
                    <div class="ACA_FLeft">
                        <h1>
                            <ACA:AccelaLabel ID="acc_manage_label_contactInfo" LabelKey="acc_manage_label_contactInfo"
                                runat="server" LabelType="SectionTitleWithBar" />
                        </h1>
                    </div>
                    <div class="ACA_FRight">
                        <ACA:AccelaButton ID="btnAddContact" DivDisableCss="ACA_SmButtonDisable ACA_SmButtonDisable_FontSize"
                            DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" OnClientClick="SetNotAsk();CreateContactSession();return false;"
                            runat="server" LabelKey="aca_account_management_add_contact" />
                    </div>
                </div>
                <span id="acc_manage_label_contactInfo_sub_label" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize"
                    runat="server"></span>
            </div>
            <asp:UpdatePanel ID="panelContactView" UpdateMode="conditional" runat="server">
                <ContentTemplate>
                    <ACA:RefContactList ID="refContactList" GViewID="60145" IsForView="true" ContactSectionPosition="ModifyReferenceContact" runat="server">
                    </ACA:RefContactList>
                    <asp:LinkButton ID="btnAccountContactRefresh" runat="Server" CssClass="ACA_Hide"
                        OnClick="AccountContactRefreshButton_Click" TabIndex="-1"></asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!--    Clerks Begin -->
        <div id="divAgentClerk" runat="server" class="clerksection" visible="false">
            <div>
                <ACA:AccelaSectionTitleBar ID="sectionTitleBar" LabelType="SectionTitleWithBar" LabelKey="aca_authagent_accountmanager_label_clerktitle" ShowType="Show" runat="server" />
            </div>
            <asp:UpdatePanel ID="panelClerkView" UpdateMode="conditional" runat="server">
                <ContentTemplate>
                    <ACA:AgentClerkList ID="agentClerkList" runat="server">
                    </ACA:AgentClerkList>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!--    Clerks End -->
        <ACA:AccelaHeightSeparate ID="sepAttachment" runat="server" Height="10" />
        <!--   Attachment Begin -->
        <div id="divAttachment" runat="server">
            <ACA:AccelaLabel ID="lblAttachmentTitle" runat="server" LabelType="SectionExText"
                LabelKey="per_attachment_Label_attachTitle" />
            <ACA:AttachmentEdit ID="attachmentEdit" IsDetailPage="false" runat="server" IsAccountManagerPage="true" />
        </div>
        <!--   Attachment End -->
        <ACA:AccelaHeightSeparate ID="sepHeightForRefContact" runat="server" Height="10" />
        <div id="divTrustAccount" runat="server">
            <ACA:AccelaLabel ID="lblTrustAccountTitle" LabelKey="per_accountmanage_trustaccount_title"
                runat="server" LabelType="SectionTitle" />
            <ACA:TrustAcctList ID="trustAcctList" runat="server" GViewID="60104" />
        </div>
        <div id="divMyDelegateUserView" runat="server">
            <div class="ACA_Title_Bar">
                <div class="ACA_FLeft">
                    <h1>
                        <ACA:AccelaLabel ID="lblMyProxyUsers" LabelKey="aca_delegate_section_title" runat="server"
                    LabelType="SectionTitleWithBar" />
                    </h1>
                </div>
                <div class="ACA_FRight">
                    <ACA:AccelaButton ID="btnAddDelegateUser" DivDisableCss="ACA_SmButtonDisable ACA_SmButtonDisable_FontSize"
                                DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" OnClientClick="SetNotAsk();showPopupDialog('DelegateManager.aspx?proxyUserPageType=0',this.id);return false;"
                                runat="server" LabelKey="aca_add_delegate" />
                </div>
            </div>
            <span id="lblMyProxyUsers_sub_label" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize"
                    runat="server"></span>
            <ACA:DelegateUsersView ID="myDelegateUsersView" runat="server" />
        </div>    
        <!--Add the base validator to output the basic validation scripts forcely. such as "WebForm_OnSubmit" function.-->
        <asp:CustomValidator ID="cvOnlyForJSOutput" runat="server"></asp:CustomValidator>
    </div>
</asp:Content>