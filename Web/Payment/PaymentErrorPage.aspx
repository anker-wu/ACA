<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Payment.Payment_PaymentErrorPage" MasterPageFile="~/Default.master" Codebehind="PaymentErrorPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <div class="ACA_Content">
        <div class="ACA_Message_Error ACA_Message_Error_FontSize" id="divErrorMessage" runat="server">
            <table role='presentation'>
                <tr>
                    <td width="30px">
                        <div class="ACA_Error_Icon">
                            <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_24.gif") %>"/>
                        </div>
                    </td>
                    <td>
                        <ACA:AccelaLabel ID="lblErrorMessage" IsNeedEncode="false" LabelType="bodyText" runat="server" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
