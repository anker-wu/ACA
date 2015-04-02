<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InspectionWizardInputLocation.aspx.cs" 
    MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.Inspection.InspectionWizardInputLocation" %>
<%@ Register Src="~/Component/AddressView.ascx" TagName="AddressView" TagPrefix="uc1" %>
<%@ Import Namespace="Accela.ACA.Web.Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <div class="font11px">
            <asp:HiddenField ID="hdContactContent" runat="server"></asp:HiddenField>
            <div>
                
                <div class="ACA_TabRow_Italic font12px">
                    <ACA:AccelaLabel ID="lblInpectionType" LabelKey="aca_inspection_type_label" runat="server"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="5" runat="server" />
                <div class="ACA_TabRow ACA_Popup_Title">
                    <ACA:AccelaLabel ID="lblLocationContact" LabelKey="aca_inspection_location_contact_label" runat="server"></ACA:AccelaLabel>
                </div>
                <div class="ACA_Section_Instruction ACA_Page_Instruction_FontSize">
                    <ACA:AccelaLabel ID="lblLocationContactContext" LabelKey="aca_inspection_location_contact_context" runat="server"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="15" runat="server" />        
                <div class="InspectionLocationSubTitle">
                    <ACA:AccelaLabel ID="lblLocation" LabelKey="aca_inspection_location_label" runat="server"></ACA:AccelaLabel>
                </div>
                <div id="divLocationContent" runat="server">
                    <uc1:AddressView ID="addressView" runat="server" />
                </div>
                <div id="divLocationContentEmpty" class="ACA_TabRow_Italic" runat="server" visible="false">
                    <ACA:AccelaLabel ID="lblLocationEmpty" LabelKey="aca_inspection_location_empty_label" runat="server"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="20" runat="server" />        
                <div class="InspectionLocationSubTitle">
                    <ACA:AccelaLabel ID="lblContact" LabelKey="aca_inspection_contact_label" runat="server"></ACA:AccelaLabel>
                </div>
                <div id="divContactContent" runat="server" class="ACA_FLeft">
                    <div id="divContactName"><%= ContactName %></div>
                    <div id="divContactPhone"><%= ContactPhone %></div>
                </div>
                <div id="divContactContentEmpty" class="ACA_TabRow_Italic" runat="server" visible="false">
                    <ACA:AccelaLabel ID="lblContactContentEmpty" LabelKey="aca_inspection_contact_empty_label" runat="server"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate4" Height="10" runat="server" />
                <!-- the change contact div -->
                <div id="divContactChangeLink" runat="server">
                    <table ID="tblCollapsesOrExpandChangeContact" runat="server" role='presentation' border='0' cellspacing='0' cellpadding='0'>
                        <tr>
                            <td class="ACA_LinkButton nav_bar_separator">
                                <a href="javascript:void(0);" class="nav_more_arrow NotShowLoading" onclick="showHideChangeContact()" title="<%=GetTitleByKey("aca_inspection_contact_link_changecontact","") %>">
                                    <ACA:AccelaLabel ID="lblActions" LabelKey="aca_inspection_contact_link_changecontact" runat="server"></ACA:AccelaLabel>
                                    <img alt="" border="0" src="<%=ImageUtil.GetImageURL("caret_arrow.gif") %>" />
                                </a>
                            </td>
                        </tr>
                    </table>  
                    <div id="divChangeContact" class="SmallPopUpDlg ACA_Hide" runat="server">
                    <table role="presentation"><tr><td>
                        <ACA:AccelaRadioButton ID="rbContactSelect" CssClass="fontbold" GroupName="rbContact" LabelKey="aca_inspection_contact_label_select" runat="server"  onclick="switchContactInputOrSelect()"/>
                        <div id="divChangeContactSelect" class="InspectionChangeContactItem" runat="server">
                            <ACA:AccelaDropDownList ID="ddlContactList" runat="server"></ACA:AccelaDropDownList>
                            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate5" Height="15" runat="server" />
                        </div>
                        </td></tr>
                        <tr><td>
                        <ACA:AccelaRadioButton ID="rbContactSpecify" CssClass="fontbold" GroupName="rbContact" LabelKey="aca_inspection_contact_label_specify" runat="server" onclick="switchContactInputOrSelect()" Type="SectionExRadio"/>
                        <div id="divChangeContactSpecify" class="InspectionChangeContactItem" runat="server">     
                            <table role="presentation" class="font9px">
                                <tbody>
                                    <tr>
                                        <td>
                                            <ACA:AccelaTextBox ID="txtFirstName" Width="12em" MaxLength="70" LabelKey="per_peoplelist_firstname" runat="server"></ACA:AccelaTextBox>
                                        </td>
                                        <td>
                                            <ACA:AccelaTextBox ID="txtMiddleName" Width="12em" MaxLength="70" LabelKey="per_peoplelist_middlename" runat="server"></ACA:AccelaTextBox>
                                        </td>
                                        <td>
                                            <ACA:AccelaTextBox ID="txtLastName" Width="12em" MaxLength="70" Labelkey="per_peoplelist_lastname" runat="server"></ACA:AccelaTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td colspan="3">
                                        <ACA:AccelaPhoneText ID="txtPhoneNumber" LabelKey="aca_inspection_contact_label_phonenumber" runat="server"></ACA:AccelaPhoneText>
                                    </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        </td></tr></table>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate6" Height="20" runat="server" />
                        <div runat="server" ID="divChangeContactButton" class="ACA_TabRow">
                        <table role='presentation'>
                            <tr valign="bottom">
                                <td>
                                    <div class="ACA_LgButton ACA_LgButton_FontSize">
                                        <ACA:AccelaButton ID="lnkContactChange" OnClick="ContactChangeButton_Click" LabelKey="aca_inspection_contact_link_change" runat="server"/>
                                    </div>
                                </td>
                                <td class="PopupButtonSpace">&nbsp;</td>
                                <td>
                                    <div class="ACA_LinkButton">
                                        <ACA:AccelaLinkButton ID="lnkContactCancel" OnClientClick="showHideChangeContact(); return false;" LabelKey="ACA_Inspection_Action_Cancel" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        </div>
                    </div>
                </div>
            </div>
            
            <ACA:AccelaHeightSeparate ID="spAboveButton" Height="40" Visible="true" runat="server" />            
            <!-- button list -->
            <div class="ACA_TabRow">
            <table role='presentation'>
                <tr valign="bottom">
                    <td>
                        <div class="ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaButton ID="lnkContinue" OnClientClick="locateValidationError(false)" OnClick="ContinueButton_Click" LabelKey="aca_inspection_action_continue" runat="server"/>
                        </div>
                     </td>
                     <td id="tdBackSpace" runat="server" class="PopupButtonSpace">&nbsp;</td>
                     <td id="tdBack" runat="server">
                        <div class="ACA_LinkButton">
                            <ACA:AccelaLinkButton ID="lnkBack" CausesValidation="False" OnClick="BackButton_Click" LabelKey="aca_inspection_action_back" runat="server" />
                        </div>
                     </td>
                     <td class="PopupButtonSpace">&nbsp;</td>
                     <td>
                        <div class="ACA_LinkButton">
                            <ACA:AccelaLinkButton ID="lnkCancel" OnClientClick="parent.ACADialog.close(); return false;" LabelKey="ACA_Inspection_Action_Cancel" runat="server" />
                        </div>
                     </td>
                </tr>
            </table>
            </div>
    </div>
    
    <script type="text/javascript">
        $("#lnkBeginFocus", getParentDocument()).focus();
        var space = 0;
        var spAboveButtonHeight = 0;
        var rbContactSelect = $("#<%=rbContactSelect.ClientID %>");
        var ddlContactList = $("#<%=ddlContactList.ClientID %>");
        var rbContactSpecify = $("#<%=rbContactSpecify.ClientID %>");
        var txtFirstName = $("#<%=txtFirstName.ClientID %>");
        var txtMiddleName = $("#<%=txtMiddleName.ClientID %>");
        var txtLastName = $("#<%=txtLastName.ClientID %>");
        var txtPhoneNumber = $("#<%=txtPhoneNumber.ClientID %>");
        var txtPhoneIDD = $("#<%=txtPhoneNumber.ClientID %>_IDD");
        var collapsibleContactEditForm = new CollapsibleElement("<%=divChangeContact.ClientID %>");

        function switchContactInputOrSelect(currentObject) {
            setFieldStatusForContactInputOrSelect();
            CheckAndResetValidator();
        }

        function CheckAndResetValidator() {
            if (typeof (Page_Validators) != "undefined" && typeof (Page_IsValid) != "undefined" && !Page_IsValid) {
                for (var i = 0; i < Page_Validators.length; i++) {
                    var validator = Page_Validators[i];
                    if (!validator.isvalid) {
                        var currentElement = $("#" + validator.controltovalidate);
                        if (currentElement) {
                            currentElement.val("");
                            ValidatorValidate(validator);
                            Page_IsValid = true;
                        }
                    }
                }
            }
        }

        function showHideChangeContact() {
            if ($.global.isAdmin) {
                return false;
            }
        
            //when show the action div, don't need pop up the query window.
            CheckAndSetNoAsk();
            
            var divChangeContact = $('#<%= divChangeContact.ClientID %>');
            var divInspectionPage = $('#divInspectionPage');
            var spAboveButton = $('#<%= spAboveButton.ClientID %>');
            var height = divInspectionPage.height();

            if (spAboveButtonHeight == 0)
            {
                spAboveButtonHeight = spAboveButton.height();
            }

            if (collapsibleContactEditForm.isCollapsed) {
                collapsibleContactEditForm.expand();

                var contactBottom = divChangeContact.position().top + divChangeContact.height();

                if (contactBottom > height) {
                    space = contactBottom - height + 20;
                    height = height + space;
                }
                else {
                    space = 0;
                }

                setFieldStatusForContactInputOrSelect();
            }
            else {
                collapsibleContactEditForm.collapse();

                if (typeof (myValidationErrorPanel) != "undefined") {
                    myValidationErrorPanel.clearErrors();
                }   

                // the div height value must get in the display status.
                height = height - space;

                $('#<%=tblCollapsesOrExpandChangeContact.ClientID %> a').focus();
            }

            divInspectionPage.height(height);
        }

        function locateValidationError(isToBypassValidation) {
            // Do not validate those requried fields when click 'Continue' button. 
            // But if do not display default contact and input new one, then need validate.
            if ("<%=DisplayDefaultContact4Inspection %>".toLowerCase() == "true" || "<%=divContactChangeLink.Visible %>".toLowerCase() == "false") {
                if (typeof(Page_Validators) != "undefined" && Page_Validators.length > 0) {
                    var index = 0;
                    var validators = new Array();

                    for (var i = 0; i < Page_Validators.length; i++) {
                        if (!(Page_Validators[i].controltovalidate == "<%=txtFirstName.ClientID %>") &&
                            !(Page_Validators[i].controltovalidate == "<%=txtLastName.ClientID %>") &&
                            !(Page_Validators[i].controltovalidate == "<%=txtMiddleName.ClientID %>") &&
                            !(Page_Validators[i].controltovalidate == "<%=txtPhoneNumber.ClientID %>")) {
                            validators[index] = Page_Validators[i];
                            index++;
                        }
                    }

                    Page_Validators = validators;
                }
            }

            if (typeof (Page_ClientValidate) == "function" && !Page_ClientValidate()) {
                if (isToBypassValidation) {
                    Page_ValidationActive = false;
                }
                else {
                    collapsibleContactEditForm.expand();
                }
            }
        }
        
        //Set the field status if select Contact Input or Select option.
        function setFieldStatusForContactInputOrSelect() {
            var disabledString = "disabled";
            var group1Disabled = !$(rbContactSelect).attr("checked");
            var group2Disabled = !group1Disabled;

            if (group1Disabled) {
                $(ddlContactList).attr(disabledString, disabledString);
            }
            else {
                $(ddlContactList).removeAttr(disabledString);
            }

            if (group2Disabled) {              
                $(txtFirstName).attr(disabledString, disabledString);
                $(txtMiddleName).attr(disabledString, disabledString);
                $(txtLastName).attr(disabledString, disabledString);
                $(txtPhoneNumber).attr(disabledString, disabledString);
                $(txtPhoneIDD).attr(disabledString, disabledString);
            }
            else {
                $(txtFirstName).removeAttr(disabledString);
                $(txtMiddleName).removeAttr(disabledString);
                $(txtLastName).removeAttr(disabledString);
                $(txtPhoneNumber).removeAttr(disabledString);
                $(txtPhoneIDD).removeAttr(disabledString);

                //to avoid causing section 508 input box focus issue
                if ($.browser.msie && $.browser.version == "7.0") {
                    $(txtFirstName).focus();
                    rbContactSpecify.focus();
                }
            }
        }

    </script>
</asp:Content>