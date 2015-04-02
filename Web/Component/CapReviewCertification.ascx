<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CapReviewCertification.ascx.cs" Inherits="Accela.ACA.Web.Component.CapReviewCertification" %>

<script type="text/javascript">
    $(function() {
         ShowOrHideReviewDate();
    });

    function chbReviewSelected_OnClick() {
        if ($("#<%=termReviewAccept.ClientID%>").attr("checked")) {
            hideMessage();
            $("#divReviewErrorIcon").hide();
        }

        ShowOrHideReviewDate();
    } 

    function chbReviewAccept_OnClick() {
        if ($("#<%=termReviewAccept.ClientID%>").attr("checked")) {
            return true;
        }
        
        showNormalMessage('<%=GetTextByKey("aca_recertification_msg_acceptterms").Replace("'","\\'")%>', 'Error');
        $("#divReviewErrorIcon").show();
        return false;
    }

    function ShowOrHideReviewDate() {
        if(!$.global.isAdmin && !$("#<%=termReviewAccept.ClientID%>").attr("checked")) {
            $("#divReviewDate").hide();
        } else {
            $("#divReviewDate").show();
        }
    }
</script>

<div class="CertificationContent">
    <div class="CertificationContentContainer ACA_ContainerStyle">
        <table role='presentation' cellspacing="0" cellpadding="8">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="lblContent1" LabelKey="aca_recertification_label_content1" runat="server" LabelType="BodyText" />
                    <ACA:AccelaLabel ID="lblContent2" LabelKey="aca_recertification_label_content2" runat="server" LabelType="BodyText" />
                </td>
            </tr>
        </table>
    </div>
    
    <div id="divCheckText" class="CertificationContainer aca_checkbox aca_checkbox_fontsize">
        <div class="ACA_TabRow ACA_SmallError_Icon ACA_Hide" id="divReviewErrorIcon">
            <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_mark_required") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>" />
        </div>
        <div class="CheckContainer">
            <input type="checkbox" id="termReviewAccept" onclick="chbReviewSelected_OnClick(this)" runat="server"/>
            <ACA:AccelaLabel ID="lblAcceptTerms" LabelKey="aca_recertification_label_acceptterms" runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
        </div>
        <div class="DateContainer">
            <table role="presentation" cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td class="DateLabelContainer">
                            <ACA:AccelaLabel ID="lblDateLabel" LabelKey="aca_recertification_label_date" runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
                        </td>
                        <td class="DateValueContainer">
                            <div id="divReviewDate">
                                <ACA:AccelaLabel ID="lblDateValue" runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</div>
