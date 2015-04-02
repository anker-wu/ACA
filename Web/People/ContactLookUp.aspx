<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="ContactLookUp.aspx.cs" Inherits="Accela.ACA.Web.People.ContactLookUp" %>
<%@ Register Src="~/Component/ContactInfo.ascx" TagName="ContactInfo" TagPrefix="uc1" %>
<%@ Register Src="~/Component/ContactSearchList.ascx" TagName="ContactSearchList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/ContactAddressSearchList.ascx" TagName="ContactAddressSearchList" TagPrefix="ACA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <script src="../Scripts/global.js" type="text/javascript"></script>
    <script src="../Scripts/DisableForm.js" type="text/javascript"></script>
    <script src="../Scripts/Expression.js" type="text/javascript"></script>
    <script type="text/javascript">
        function PopupClose(refContactSeqNbr) {
            var parentFocusObj = parent.ACADialog.focusObject;
            parent.ACADialog.close();
            SetParentLastFocus(parentFocusObj);
            RefreshContactList(refContactSeqNbr);
        }
        
        function RefreshContactList(refContactSeqNbr) {
            if (<%= IsMultipleContact.ToString().ToLower() %>) {
                parent.<%=ParentID %>_RefreshContactList(false, refContactSeqNbr, false);
            } else {
                parent.<%=ParentID %>_Refresh(false, refContactSeqNbr, false);
            }
        }

        with(Sys.WebForms.PageRequestManager.getInstance()){
            add_pageLoaded(function() {
                var radioButtons = $("input[type=radio][id*=CB_]");
                var radioSelected = radioButtons.filter(":checked").length > 0;
                
                SetWizardButtonDisable("<%= btnContinueContact.ClientID %>", !(<%=AppSession.IsAdmin.ToString().ToLower() %> || radioSelected));
                
                radioButtons.bind("click", function() {
                    SetWizardButtonDisable("<%= btnContinueContact.ClientID %>", false);
                }); 

                $("#divContactLookUpCriteriaInput").keydown(function(event) {
                    pressEnter2TriggerClick(event, $("#<%=btnSearch.ClientID %>"));
                });
            });
        }
    </script>
    <div id="divContactLookUp" class="ContactLookUp">
        <ACA:AccelaLabel ID="lblContactTitle" LabelKey="aca_contactlookup_label_title" runat="server" Visible="false" LabelType="SectionExText" />
        <div id="divContactLookUpCriteria" class="ContactLookUpCriteria" runat="server">
            <div id="divContactLookUpCriteriaInput" class="ContactLookUpCriteriaInput">
                <uc1:ContactInfo ID="ucContactInfo" runat="server" NeedRunExpression="False" IsForSearch="True" IsShowContactType="True" />
            </div>
            <div id="divContactLookUpCriteriaButton" class="ContactLookUpCriteriaButton ACA_Row ACA_LiLeft">
                <ul>
                    <li>
                        <ACA:AccelaButton ID="btnSearch" runat="server" LabelKey="aca_contactedit_label_lookup" CausesValidation="False"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            OnClick="SearchButton_Click">
                        </ACA:AccelaButton>
                    </li>
                    <li id="liClearBtn">
                        <ACA:AccelaButton ID="btnClear" runat="server" LabelKey="aca_contactedit_label_clearbutton"
                            OnClick="ClearButton_Click" CausesValidation="false"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                        </ACA:AccelaButton>
                    </li>
                    <li>
                        <ACA:AccelaLinkButton ID="btnSearchFormCancel" runat="server" LabelKey="aca_contactlookup_label_canceledit" CausesValidation="false"
                            OnClientClick="parent.ACADialog.close();return false;" CssClass="ACA_LinkButton"/>
                    </li>
                </ul>
            </div>
        </div>
        <div id="divContactList" class="ContactList ACA_Row" runat="server">
            <div id="divContactListBack" class="ContactListBack ACA_TabRow">
                <ACA:AccelaLinkButton ID="btnBack" runat="server" CausesValidation="false" LabelKey="aca_contactlookup_label_backcontactlist"
                    CssClass="ACA_LinkButton" OnClick="BackButton_Click">
                </ACA:AccelaLinkButton>
            </div>
            <div id="divContactListHints" class="ContactListHints ACA_TabRow">
                <ACA:AccelaLabel ID="lblContactListHints" LabelKey="aca_contactlookup_label_contactlisthints" LabelType="BodyText" runat="server" />
            </div>
            <div id="divContactListResults" class="ContactListResults ACA_TabRow">
                <ACA:ContactSearchList OnPageIndexChanging="ContactList_PageIndexChanging" ID="contactSearchList" runat="server" />
            </div>
            <div id="divContactListButton" class="ContactListButton ACA_TabRow ACA_LiLeft">
                <ul>
                    <li>
                        <ACA:AccelaButton ID="btnContinueContact" runat="server" LabelKey="aca_contactlookup_label_continue" CausesValidation="False"
                            OnClick="ContinueContactButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"/>
                    </li>
                    <li>
                        <ACA:AccelaLinkButton ID="btnCancelContactList" runat="server" CausesValidation="false" LabelKey="aca_contactlookup_label_cancelselectcontact"
                            OnClientClick="parent.ACADialog.close();return false;" CssClass="ACA_LinkButton"/>
                    </li>
                </ul>
            </div>
        </div>
        <div id="divContactAddressList" class="contact_address_selection ACA_Row" runat="server">
            <div id="divContactType" runat="server">
                <div class="ACA_TabRow contact_name">
                    <ACA:AccelaLabel ID="lblContactFullName" LabelType="BodyText" runat="server" />
                </div>
                <div class="ACA_TabRow">
                    <ACA:AccelaDropDownList ID="ddlContactType" OnSelectedIndexChanged="DdlContactType_SelectedIndexChanged" AutoPostBack="true" CssClass="contact_type_field" 
                         Visible="False" Required="True" LayoutType="Horizontal" runat="server" LabelKey="per_appInfoEdit_label_contactType" ToolTipLabelKey="aca_common_msg_dropdown_changedatafilter_tip" />
                    <ACA:AccelaLabel ID="lblContactType" CssClass="contact_type_label" LabelType="BodyText" runat="server" />
                </div>
            </div>
            <div id="divAddressList" runat="server">
                <div class="ContactAddressListHints ACA_TabRow">
                    <ACA:AccelaLabel ID="lblContactAddressListHints" LabelKey="aca_contactlookup_label_contactaddresslisthints" LabelType="BodyText" runat="server" />
                </div>
                <div id="divContactAddressListResults" class="ContactAddressListResults ACA_TabRow">
                    <ACA:ContactAddressSearchList ID="contactAddressSearchList" runat="server"></ACA:ContactAddressSearchList>
                </div>
            </div>
            <div id="divContactAddressListButton" class="ContactAddressListButton ACA_Row ACA_LiLeft" runat="server">
                <ul>
                    <li>
                        <ACA:AccelaButton ID="btnContinueContactAddress" runat="server" LabelKey="aca_contactlookup_label_continuecontactaddress"
                            OnClick="ContinueContactAddressButton_Click"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"/>
                    </li>
                    <li>
                        <ACA:AccelaLinkButton ID="btnCancelContactAddress" runat="server" LabelKey="aca_contactlookup_label_cancelcontactaddress"
                            CausesValidation="false" OnClientClick="parent.ACADialog.close();return false;" CssClass="ACA_LinkButton"/>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
