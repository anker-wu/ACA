<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CertifiedBusinessGeneralInfo.ascx.cs" Inherits="Accela.ACA.Web.Component.CertifiedBusinessGeneralInfo" %>

<div id="divGerneralInfo" class="ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize" runat="Server">
    <table role='presentation' id="tbProviderDetail" runat="server" class="ACA_InsContent">
        <tr class="ACA_Table_Align_Top">
            <td>
                <table role='presentation' border="0" cellspacing="0" cellpadding="0" class="ACA_FullWidthTable">
                    <tr>
                        <td class="ACA_Table_Align_Top">
                            <div>
                                <table role='presentation'>
                                    <tr>
                                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                                            <p>
                                                <ACA:AccelaLabel ID="lblCertifiedBusinessName" LabelKey="aca_certbusiness_label_detail_certifiedbusinessname" Font-Bold="true" runat="server" />
                                            </p>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="ACA_Table_Align_Top">
                                            <p class='break-word'>
                                                <ACA:AccelaLabel ID="lblCertifiedBusinessNameValue" runat="server" />
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Table_Align_Top">
                            <div>
                                <table role='presentation'>
                                    <tr>
                                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                                            <p>
                                                <ACA:AccelaLabel ID="lblBusinessNameDba" LabelKey="aca_certbusiness_label_detail_businessnamedba" Font-Bold="true" runat="server" />
                                            </p>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="ACA_Table_Align_Top">
                                            <p class='break-word'>
                                                <ACA:AccelaLabel ID="lblBusinessNameDbaValue" runat="server" />
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Table_Align_Top">
                            <div>
                                <table role='presentation'>
                                    <tr>
                                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                                            <p>
                                                <ACA:AccelaLabel ID="lblBusinessDescription" LabelKey="aca_certbusiness_label_detail_businessdescription" Font-Bold="true" runat="server" />
                                            </p>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="ACA_Table_Align_Top">
                                            <p class='break-word'>
                                                <ACA:AccelaLabel ID="lblBusinessDescriptionValue" runat="server" /></p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Table_Align_Top">
                            <div>
                                <table role='presentation'>
                                    <tr>
                                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                                            <p>
                                                <ACA:AccelaLabel ID="lblAddress" LabelKey="aca_certbusiness_label_detail_address" Font-Bold="true" runat="server" />
                                            </p>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="ACA_Table_Align_Top">
                                            <p class='break-word'>
                                                <ACA:AccelaLabel ID="lblAddressValue" runat="server" IsNeedEncode="false"/></p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Table_Align_Top">
                            <div>
                                <table role='presentation'>
                                    <tr>
                                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                                            <p>
                                                <ACA:AccelaLabel ID="lblContact" LabelKey="aca_certbusiness_label_detail_contact" Font-Bold="true" runat="server" />
                                            </p>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="ACA_Table_Align_Top">
                                            <p class='break-word'>
                                                <ACA:AccelaLabel ID="lblContactValue" runat="server" /></p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Table_Align_Top">
                            <div>
                                <table role='presentation'>
                                    <tr>
                                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                                            <p>
                                                <ACA:AccelaLabel ID="lblTelphone" LabelKey="aca_certbusiness_label_detail_phone" Font-Bold="true" runat="server" />
                                            </p>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="ACA_Table_Align_Top">
                                            <p class='break-word'>
                                                <ACA:AccelaLabel IsNeedEncode="false" ID="lblTelphoneValue" runat="server" /></p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Table_Align_Top">
                            <div>
                                <table role='presentation'>
                                    <tr>
                                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                                            <p>
                                                <ACA:AccelaLabel ID="lblFax" LabelKey="aca_certbusiness_label_detail_fax" Font-Bold="true" runat="server" />
                                            </p>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="ACA_Table_Align_Top">
                                            <p class='break-word'>
                                                <ACA:AccelaLabel ID="lblFaxValue" IsNeedEncode="false" runat="server" /></p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                            <div>
                                <table role='presentation'>
                                    <tr>
                                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                                            <p>
                                                <ACA:AccelaLabel ID="lblEmail" LabelKey="aca_certbusiness_label_detail_email" Font-Bold="true" runat="server" />
                                            </p>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="ACA_Table_Align_Top">
                                            <p class='break-word'>
                                                <ACA:AccelaDateLabel ID="lblEmailValue" runat="server" /></p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Table_Align_Top">
                            <div>
                                <table role='presentation'>
                                    <tr>
                                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                                                <ACA:AccelaLabel ID="lblWebSite" LabelKey="aca_certbusiness_label_detail_website" Font-Bold="true" runat="server" />
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="ACA_Table_Align_Top">
                                            <p class='break-word'>
                                                <ACA:AccelaDateLabel ID="lblWebSiteValue" runat="server" /></p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ACA_Table_Align_Top">
                            <div>
                                <table role='presentation'>
                                    <tr>
                                        <td class="ACA_Table_Align_Top ACA_Nowrap">
                                                <ACA:AccelaLabel ID="lblEstablishmentDate" LabelKey="aca_certbusiness_label_detail_establishmentdate"
                                                    Font-Bold="true" runat="server" />
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="ACA_Table_Align_Top">
                                            <p class='break-word'>
                                                <ACA:AccelaDateLabel ID="lblEstablishmentDateValue" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize"
                                                    DateType="ShortDate" /></p>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>        
    </table>
</div>

<div id="tbMoreDetail" runat="server">
    <table role='presentation' border="0" class="ACA_FullWidthTable" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="3" style="height: 24px">
                    <h1><a href="javascript:void(0);" class="NotShowLoading" onclick='ControlDisplay($get("TRMoreDetail"),$get("imgMoreDetail"),false,$get("lnkMoreDetail"),$get("<%=lblMoreDetail.ClientID %>"))'
                     title="<%=GetTitleByKey("img_alt_collapse_icon", "aca_certbusiness_label_detail_moredetail") %>" id="lnkMoreDetail">
                    <img class="SectionTitleArrow" alt="<%=GetTextByKey("img_alt_collapse_icon") %>"
                        src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_expanded.gif") %>"
                        id="imgMoreDetail" /></a><ACA:AccelaLabel ID="lblMoreDetail" runat="server" IsDisplayLabel="True"
                            IsNeedEncode="False" LabelType="LabelText" LabelKey="aca_certbusiness_label_detail_moredetail" /></h1>
            </td>
        </tr>
        <tr id="TRMoreDetail" style="display: block;">
            <td class="moredetail_td">
                &nbsp;</td>
            <td colspan="2" style="height: 24px">
                <div class="ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize">
                    <table role='presentation'>
                        <tr>
                            <td class="ACA_Table_Align_Top ACA_Nowrap">
                                <ACA:AccelaLabel ID="lblCertifiedAs" LabelKey="aca_certbusiness_label_detail_certifiedas" Font-Bold="true" runat="server" />
                            </td>
                            <td>&nbsp;</td>
                            <td class="ACA_Table_Align_Top">
                                <p class='break-word'>
                                    <ACA:AccelaLabel ID="lblCertifiedAsValue" runat="server" />
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_Table_Align_Top ACA_Nowrap">
                                <ACA:AccelaLabel ID="lblValidThru" LabelKey="aca_certbusiness_label_detail_validthru" Font-Bold="true" runat="server" />
                            </td>
                            <td>&nbsp;</td>
                            <td class="ACA_Table_Align_Top">
                                <p class='break-word'>
                                    <ACA:AccelaLabel ID="lblValidThruValue" runat="server" />
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_Table_Align_Top ACA_Nowrap">
                                <ACA:AccelaLabel ID="lblOwnerEthnicity" LabelKey="aca_certbusiness_label_detail_ownerethnicity" Font-Bold="true" runat="server" />
                            </td>
                            <td>&nbsp;</td>
                            <td class="ACA_Table_Align_Top">
                                <p class='break-word'>
                                    <ACA:AccelaLabel ID="lblOwnerEthnicityValue" runat="server" />
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_Table_Align_Top ACA_Nowrap">
                                <ACA:AccelaLabel ID="lblRegion" LabelKey="aca_certbusiness_label_detail_region" Font-Bold="true" runat="server" />
                            </td>
                            <td>&nbsp;</td>
                            <td class="ACA_Table_Align_Top">
                                <p class='break-word'>
                                    <ACA:AccelaLabel ID="lblRegionValue" runat="server" />
                                </p>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>

<script language="javascript" type="text/javascript">
    var CTreeTop = '<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_collapsed.gif") %>';
    var ETreeTop = '<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_expanded.gif") %>';
    var CTreeMiddle = '<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("minus_collapse.gif") %>';
    var ETreeMiddle = '<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("plus_expand.gif") %>';
    var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'", "\\'") %>';
    var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'", "\\'") %>';
    function ControlDisplay(obj, imgObj, isLeefNode, lnkObj, lblValue) {
        if (obj.style.display == 'block' || obj.style.display == "" || obj.style.display == "table-row") {
            if (!isLeefNode) {
                Collapsed(imgObj, CTreeTop, altExpanded);
                AddTitle(lnkObj, altExpanded, lblValue);
            }
            else {
                Collapsed(imgObj, ETreeMiddle, altExpanded);
                AddTitle(lnkObj, altExpanded, lblValue);
            }

            obj.style.display = 'none';
        }
        else {
            if (!isLeefNode) {
                Expanded(imgObj, ETreeTop, altCollapsed);
                AddTitle(lnkObj, altCollapsed, lblValue);
            }
            else {
                Expanded(imgObj, CTreeMiddle, altCollapsed);
                AddTitle(lnkObj, altCollapsed, lblValue);
            }
            if (document.all)
                obj.style.display = 'block';
            else
                obj.style.display = 'table-row';
        }
    }
</script>
