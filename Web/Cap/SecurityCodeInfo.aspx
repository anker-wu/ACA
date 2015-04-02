<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Cap_SecurityCodeInfo" Codebehind="SecurityCodeInfo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head id="Head1" runat="server">
    <title><%=LabelUtil.GetGlobalTextByKey("aca_securitycodeinfo_label_title|tip")%></title>
</head>
<body>
    <form id="form1" runat="server" class="ACA_Page_NoScrollBar">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="ACA_RightContent">
            <p>
                <br />
            </p>
            <div class="ACA_InfoTitle ACA_InfoTitle_FontSize">
                <div class="ACA_Label ACA_Label_FontSize">
                    <ACA:AccelaLabel LabelKey="securityCodeInfo_label_rowTitle" ID="securityCodeInfo_label_rowTitle"
                        runat="server" />
                </div>
            </div>
            <ACA:AccelaLabel LabelKey="securityCodeInfo_text_communication" ID="securityCodeInfo_text_communication"
                runat="server" />
            <br />
            <ACA:AccelaLabel LabelKey="securityCodeInfo_text_visa" ID="securityCodeInfo_text_visa"
                runat="server" />
            <br />
            <div class="ACA_Security_Vmd_Icon" title="Security code location for Visa, MasterCard and Discover Card">
                <img alt="<%=GetTextByKey("img_alt_security_vmd_icon") %>" src="<%=ImageUtil.GetImageURL("cc_security_code_vmd.gif") %>" />
            </div>
            <br />
            <br />
            <ACA:AccelaLabel LabelKey="securityCodeInfo_text_express" ID="securityCodeInfo_text_express"
                runat="server" />
            <br />
            <div class="ACA_Security_Amex_Icon" title="Security code location for Visa, MasterCard and Discover Card">
                <img alt="<%=GetTextByKey("img_alt_security_amex_icon") %>" src="<%=ImageUtil.GetImageURL("cc_security_code_amex.gif") %>" />
            </div>
            <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
        </div>
    </form>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>
