<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.UserLicenseView" Codebehind="UserLicenseView.ascx.cs" %>

<%@ import namespace="Accela.ACA.Common.Util" %>

<!--<div id=bottom_links><a href="#" target="_parent">Back to Search</a></div> -->

<script type="text/javascript">
    function ConfirmRemoveLicense()
    {
        if(typeof(SetNotAsk) != 'undefined')
        {
            SetNotAsk();
        }
        
        if(confirm('<%=GetTextByKey("acc_message_confirm_removeLicense").Replace("'","\\'") %>'))
        {
            return true;
        }
        else
        {
            return false;
        }        
    }
</script>

<asp:Repeater ID="rptLicense" runat="server" OnItemDataBound="rptLicense_ItemDataBind">
    <ItemTemplate>
        <div class="ACA_TabRow">
            <div class="ACA_MLonger ACA_FLeft ACA_Page ACA_Page_FontSize">
                <ul>                  
                    <table role='presentation'>
                        <tr>
                            <td colspan="2">
                                <!-- start dynamic content -->
                                <strong>
                                    <%# DataBinder.Eval(Container.DataItem, "license.contactFirstName")%>
                                    &nbsp;
                                    <%# DataBinder.Eval(Container.DataItem, "license.contactLastName")%>
                                </strong>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <%# DataBinder.Eval(Container.DataItem, "license.businessName")%><%# DataBinder.Eval(Container.DataItem, "license.busName2","/{0}")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <%# DataBinder.Eval(Container.DataItem, "license.businessLicense")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <%# DataBinder.Eval(Container.DataItem, "license.address1")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <%# DataBinder.Eval(Container.DataItem, "license.address2")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <%# DataBinder.Eval(Container.DataItem, "license.address3")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <%# DataBinder.Eval(Container.DataItem, "license.city")%>
                                <%# DataBinder.Eval(Container.DataItem, "license.state")%>
                                <%# DataBinder.Eval(Container.DataItem, "license.zip")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <ACA:AccelaLabel ID="acc_userLicenseView_label_home" runat="server" LabelKey="acc_userLicenseView_label_home" />
                                <%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "license.phone1CountryCode") as string, DataBinder.Eval(Container.DataItem, "license.phone1") as string, DataBinder.Eval(Container.DataItem, "license.countryCode") as string) as string%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <ACA:AccelaLabel ID="acc_userLicenseView_label_mobile" runat="server" LabelKey="acc_userLicenseView_label_mobile" />
                                <%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "license.phone2CountryCode") as string, DataBinder.Eval(Container.DataItem, "license.phone2") as string, DataBinder.Eval(Container.DataItem, "license.countryCode") as string) as string%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <ACA:AccelaLabel ID="acc_userLicenseView_label_fax" runat="server" LabelKey="acc_userLicenseView_label_fax" />
                                <%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "license.faxCountryCode") as string, DataBinder.Eval(Container.DataItem, "license.fax") as string, DataBinder.Eval(Container.DataItem, "license.countryCode") as string) as string%>
                            </td>
                        </tr>
                        <tr>
                            <tr>
                            </tr>
                            <!-- end dynamic content -->
                        </tr>
                    </table>                  
                </ul>
            </div>
            <div class="ACA_ConfigInfo ACA_FLeft ACA_Page ACA_Page_FontSize">
                <ul>                  
                    <table role='presentation'>
                        <tr>
                            <td colspan="2">
                                <!-- start dynamic content -->
                                <strong>
                                    <%# Format4LicenseState(Convert.ToString(DataBinder.Eval(Container.DataItem, "license.licState")), " ")%>
                                    <%# I18nStringUtil.GetString(DataBinder.Eval(Container.DataItem, "license.resLicenseType") as string, DataBinder.Eval(Container.DataItem, "license.licenseType") as string)%>
                                </strong>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <%# Format4LicenseState(Convert.ToString(DataBinder.Eval(Container.DataItem, "license.licState")), "-")%>
                                <%# DataBinder.Eval(Container.DataItem, "license.stateLicense")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table role='presentation'>
                                <tr>
                                <td>
                                <ACA:AccelaLabel ID="acc_userLicenseView_label_issued" runat="server" LabelKey="acc_userLicenseView_label_issued" /> 
                                </td>
                                <td>
                                &nbsp;<ACA:AccelaDateLabel id="lblLicenseIssueDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "license.licenseIssueDate")%>'></ACA:AccelaDateLabel>
                                </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table role='presentation'>
                                <tr>
                                <td>
                                <ACA:AccelaLabel ID="acc_userLicenseView_label_expires" runat="server" LabelKey="acc_userLicenseView_label_expires" /> 
                                </td>
                                <td>
                                &nbsp;<ACA:AccelaDateLabel id="lblLicenseExpirationDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "license.licenseExpirationDate")%>'></ACA:AccelaDateLabel>
                                </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <ACA:AccelaLabel ID="acc_userLicenseView_label_insuredMax" runat="server" LabelKey="acc_userLicenseView_label_insuredMax" />
                                <%# I18nNumberUtil.FormatMoneyForUI(DataBinder.Eval(Container.DataItem, "license.insuranceAmount"))%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <ACA:AccelaLabel ID="acc_userLicenseView_label_status" runat="server" LabelKey="acc_userLicenseView_label_status" />
                                <%# GetStatusForI18NDisplay(DataBinder.Eval(Container.DataItem, "status"))%>
                            </td>
                        </tr>
			            <tr>
                            <td colspan="2">
                                <ACA:AccelaLabel ID="lblContractorLicNO" runat="server" LabelKey="acc_userlicenseview_label_contractorlicno" />
                                <%# DataBinder.Eval(Container.DataItem, "license.contrLicNo")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <ACA:AccelaLabel ID="lblContractorBusiName" runat="server" LabelKey="acc_userlicenseview_label_contractorbusiname" />
                                 <%# DataBinder.Eval(Container.DataItem, "license.contLicBusName")%>
                            </td>
                        </tr>
                      </table>
                    <ACA:AccelaHeightSeparate ID="sepHeightForLicenseView" runat="server" Height=25 />
                </ul>
                <!-- end dynamic content -->
            </div>
            <div class="ACA_FLeft ACA_Page ACA_Page_FontSize">
                <ul>
                    <asp:HiddenField ID="hdnLicSeqNbr" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "license.licSeqNbr")%>' />
                    <asp:HiddenField ID="hdnLicType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "license.licenseType")%>' />
                        <ACA:AccelaButton ID="removeLicenseItem" DivEnableCss="ACA_SmButtonForRight ACA_SmButtonForRight_FontSize" DivDisableCss="ACA_SmButtonForRightDisable  ACA_SmButtonForRightDisable_FontSize" OnClick="lbkRemoveLicenseItem_OnClick"
                            LabelKey="lic_licenseInfo_label_remove" runat="server" OnClientClick="return ConfirmRemoveLicense();"
                            CausesValidation="false">
                        </ACA:AccelaButton>
                </ul>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
<div class="ACA_TabRow ACA_Line_Content" id="divLicLine" runat="server" visible="false">&nbsp;</div>
