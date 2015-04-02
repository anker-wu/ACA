<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="FoodFacilityGeneralInfo.ascx.cs" Inherits="Accela.ACA.Web.Component.FoodFacilityGeneralInfo" %>
<%@ Register Src="LicenseeGeneralInfoLPTemplate.ascx" TagName="LicenseeGeneralInfoLPTemplate" TagPrefix="uc1" %>
<%@ Register Src="LicenseeGeneralInfoLPPeopleInfoTable.ascx" TagName="LicenseeGeneralInfoLPPeopleInfoTable" TagPrefix="uc1" %>

<div class="ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize" runat="Server">
    <div id="divFoodFacilityFields" runat="server">
        <div class="ACA_TwoColumnTable" id="divType" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblType" LabelKey="aca_foodfacilitydetail_label_detailtype" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblTypeValue" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divContactType" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblContactType" LabelKey="aca_foodfacilitydetail_label_detailcontacttype" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblContactTypeValue" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
	    <div class="ACA_TwoColumnTable" id="divLicenseeNumber" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblLicenseeNumber" LabelKey="aca_foodfacilitydetail_label_detaillicenseenumber" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblLicenseeNumberValue" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divContactName" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblContactName" LabelKey="aca_foodfacilitydetail_label_detailcontactname" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblContactNameValue" IsNeedEncode="false" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divLicenseState" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblLicenseState" LabelKey="aca_foodfacilitydetail_label_detaillicensestate" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>                    
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblLicenseStateValue" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divAddress" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblAddress" LabelKey="aca_foodfacilitydetail_label_detailaddress" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblAddressValue" IsNeedEncode="false" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divBoard" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblBoard" LabelKey="aca_foodfacilitydetail_label_detailboard" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblBoardValue" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divTelephone1" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblTelephone1" LabelKey="aca_foodfacilitydetail_label_detailtelephone1" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblTelephone1Value" IsNeedEncode="false" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divTelephone2" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblTelephone2" LabelKey="aca_foodfacilitydetail_label_detailtelephone2" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblTelephone2Value" IsNeedEncode="false" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divBusinessName" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblBusinessName" LabelKey="aca_foodfacilitydetail_label_detailbusinessname" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblBusinessNameValue" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divFax" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblFax" LabelKey="aca_foodfacilitydetail_label_detailfax" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblFaxValue" IsNeedEncode="false" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divBusinessNumber" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblBusinessNumber" LabelKey="aca_foodfacilitydetail_label_detailbusinessnumber" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblBusinessNumberValue" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divEmail" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblEmail" LabelKey="aca_foodfacilitydetail_label_detailemail" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaLabel ID="lblEmailValue" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divBusinessExpirationDate" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblBusinessExpirationDate" LabelKey="aca_foodfacilitydetail_label_detailbusinessexpirationdate" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaDateLabel ID="lblBusinessExpirationDateValue" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate" runat="server" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divIssueDate" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblIssueDate" LabelKey="aca_foodfacilitydetail_label_detailissuedate" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaDateLabel ID="lblIssueDateValue" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_TwoColumnTable" id="divExpirationDate" runat="server">
            <table role='presentation'>
                <tr>
                    <td class="ACA_Nowrap ACA_Table_Align_Top"> 
                        <ACA:AccelaLabel ID="lblExpirationDate" LabelKey="aca_foodfacilitydetail_label_detailexpirationdate" class="ACA_RefEducation_Font" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_Table_Align_Top">
                        <p class='break-word'>
                            <ACA:AccelaDateLabel ID="lblExpirationDateValue" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate" />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    
    <br />
    <table class="ACA_FullWidthTable NoBorder" role='presentation'>
        <tr>
            <td>
                <ACA:AccelaHeightSeparate ID="hsLPTemplate" Height="10" runat="server" />
                <uc1:LicenseeGeneralInfoLPTemplate ID="licenseeGeneralInfoLPTemplate" runat="server" />
            </td>
        </tr>
    </table>

    <table class="ACA_FullWidthTable font9px NoBorder" role='presentation'>
        <tr>
            <td>
                <ACA:AccelaHeightSeparate ID="hsLPPeopleInfoTable" Height="10" runat="server" />
                <uc1:LicenseeGeneralInfoLPPeopleInfoTable ID="licenseeGeneralInfoLPPeopleInfoTable"
                    runat="server" />
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        void function() {
            var elements = $("p").filter(".break-word");
            var element, i;
            for (i = 0; element = elements[i]; i++) {
                breakWord(element);
            }
        } ();

        AutoSetHeight();

        function AutoSetHeight() {
            if ($.browser.msie && $.browser.version != 8.0) {
                return;
            }

            var containId = '#<%= divFoodFacilityFields.ClientID %>';

            $(containId).children('.ACA_TwoColumnTable').each(function (i) {
                if (i % 2 != 0) {
                    var prevHeight = parseInt($(this).prev('.ACA_TwoColumnTable').height());
                    var thisHeight = parseInt($(this).height());
                    var maxHeight = Math.max(prevHeight, thisHeight);

                    $(this).height(maxHeight);
                    $(this).prev('.ACA_TwoColumnTable').height(maxHeight);
                }
            });
        }
    </script>

</div>
