<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Cap.WorkLocation" Codebehind="WorkLocation.aspx.cs" %>

<%@ Register Src="~/Component/AddressEdit.ascx" TagName="AddressEdit" TagPrefix="address" %>
<%@ Register src="~/Component/ServiceControl.ascx" tagname="ServiceControl" tagprefix="ACA" %>
<%@ Import Namespace="Accela.ACA.Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <div class="ACA_Content">
        <div id="divAddress" runat="server">
            <div class="ACA_Page ACA_Page_FontSize">
                <ACA:AccelaLabel LabelKey="superAgency_workLocation_text_instructionInfo" ID="superAgency_workLocation_text_instructionInfo"
                    runat="server" LabelType="BodyText"></ACA:AccelaLabel>
            </div>
            <br />
            <div>
                <ACA:AccelaLabel LabelKey="superAgency_workLocation_label_enterWorkLocation" ID="lblWorkLocation"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                <div id="UpdatePanelWorkLocation" runat="server">
                    <address:AddressEdit ID="WorkLocationEdit" runat="server" NeedRunExpression="False" IsWorkLocationPage="true" />
                </div>
            </div>
        </div>
        <div id="divService" runat="server" visible="false">
            <ACA:AccelaLabel LabelKey="aca_wroklocation_label_selectservices" ID="lblSelectServices"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
            <div>
                <ACA:ServiceControl ID="serviceControl" runat="server" />
            </div>
        </div>
    </div>
    
<script type="text/javascript">

    function ShowServiceList(continueButtonID, flag) {
        var divContinue = $get('divContinueButton');

        if (flag) {
            divContinue.style.display = "block";
            disableContinue(continueButtonID, true);
        }
        else {
            divContinue.style.display = "none";
        }
    }

    function disableContinue(continueButtonID, disable) {
        var btnContinue = $('#'+continueButtonID);
        if (disable) {
            btnContinue.parent().addClass("ACA_Button_Text ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
            btnContinue.attr("disabled", "disabled");
            btnContinue.unbind("click").bind("click", function () { return false; });
        }
        else {
            btnContinue.parent().removeClass().addClass("ACA_LgButton  ACA_Button_Text ACA_LgButton_FontSize");
            btnContinue.removeAttr("disabled");
            btnContinue.unbind("click").bind("click", function () {
                var p = new ProcessLoading(); p.showLoading(false);
            });
        }
    }

    function RememberSelectedService(checkBoxItem, continueButtonID, hdnSelectedServicesID, singleSelection) {
        var hdnSelectedServices = $('#' + hdnSelectedServicesID)[0];
        var agencyPlusServiceName = checkBoxItem.parentNode.attributes["AgencyPlusServiceName"].value;
        var splitChar = '<%=ACAConstant.SPLIT_CHAR1 %>';

        if (singleSelection) {
            hdnSelectedServices.value = agencyPlusServiceName;
        }
        else {
            if (checkBoxItem.checked) {
                if (hdnSelectedServices.value.charAt(hdnSelectedServices.length - 1) != splitChar) {
                    hdnSelectedServices.value += splitChar + agencyPlusServiceName + splitChar;
                }
                else {
                    hdnSelectedServices.value += agencyPlusServiceName + splitChar;
                }
            }
            else {
                hdnSelectedServices.value = hdnSelectedServices.value.replace(splitChar + agencyPlusServiceName + splitChar, splitChar);

                if (hdnSelectedServices.value == splitChar) {
                    hdnSelectedServices.value = '';
                }
            }
        }

        disableContinue(continueButtonID, hdnSelectedServices.value == '');
    }

    function CheckSelection(continueButtonID, hdnSelectedServicesID) {
        var hdnSelectedServices = $('#' + hdnSelectedServicesID)[0];
        disableContinue(continueButtonID, hdnSelectedServices.value == '');
    }

    function ShowExpiredNotice(serviceRow, message, isAvailableLic, expiredNoticeClientID) {
        if (typeof (serviceRow) != 'undefined') {
            if (serviceRow.checked) {
                showNotice4Service(expiredNoticeClientID, message);
            }

            // Forbid the invalid service (all of its licenses are unavailable) be selected.
            if (isAvailableLic != 'True') {
                serviceRow.checked = false;
            }
        }
    }

    function showNotice4Service(containerId, msg) {
        //get the container object.
        var container = document.getElementById(containerId);

        if (container == null || typeof (container) == "undefined") {
            return;
        }

        if (container.childNodes.length == 0) {
            showMessage(containerId, msg, "Notice", true, 1);
        }
        else {
            var messageTitle = getText.global_js_showNotice_title + ':<br/>';
            var divNotice = document.getElementById(containerId + '_messages');

            if (typeof (container) != "undefined") {
                var tdMessage = divNotice.getElementsByTagName('td')[2];

                var message0 = tdMessage.childNodes[0].innerHTML;
                tdMessage.childNodes[0].innerHTML = message0.substring(messageTitle.length - 1);

                // insert the message to the top of the notice bar
                var divMessage = document.createElement('div');
                divMessage.style.marginBottom = '10px';
                divMessage.innerHTML = messageTitle + msg;
                tdMessage.insertBefore(divMessage, tdMessage.childNodes[0]);

                // remove the bottom notice if the number of the messages is larger than 4.
                // just keep three messages.
                if (tdMessage.childNodes.length > 3) {
                    tdMessage.removeChild(tdMessage.childNodes[3]);
                }

                tdMessage.childNodes[tdMessage.childNodes.length - 1].removeAttribute("style");
            }
        }
    }

    function SetSingleServiceSelection() {
        var inputlist = document.getElementsByTagName("input");

        //Implement single selection for all radio buttons.
        for (var i = 0; i < inputlist.length; i++) {
            if (inputlist[i].type == "radio") {
                inputlist[i].name = "SingleServiceOnly";
            }
        }
    }    
</script>
</asp:Content>
