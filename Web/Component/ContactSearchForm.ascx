<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.ContactSearchForm" Codebehind="ContactSearchForm.ascx.cs" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="uc1" %>
<%@ Register Src="~/Component/GenericTemplateEdit.ascx" TagName="GenericTemplate" TagPrefix="ACA" %>
<div class="ACA_TabRow" id="divContactTypeFlag" runat="server">
    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
        <tr>
            <td>
                <ACA:AccelaDropDownList ID="ddlContactTypeFlag" IsFieldRequired="true" Width="160px" onchange="ChangeFieldsRelatedTypeFlag()" runat="server" LabelKey="aca_contactsearchform_label_contacttype_flag"
                    ToolTipLabelKey="aca_common_msg_dropdown_refreshfields_tip"/>
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divName" runat="server">
    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
        <tr>
            <td>
                <ACA:AccelaDropDownList ID="ddlContactType" AutoPostBack="true" OnSelectedIndexChanged="ContactType_SelectedChanged" runat="server" LabelKey="per_contactSearchForm_label_contactType" SourceType="STDandXPolicy"
                    ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip" />
            </td>
            <td>
                <ACA:AccelaDropDownList ID="ddlSalutation" CssClass="ACA_NShot" runat="server" ShowType="showdescription" LabelKey="per_contactSearchForm_label_salutation" />
            </td>
            <td>
                <ACA:AccelaTextBox ID="txtTitle" CssClass="ACA_NShot" MaxLength="255" runat="server"
                    LabelKey="per_contactSearchForm_label_title" />
            </td>
            <td>
                <ACA:AccelaTextBox ID="txtFirstName" CssClass="ACA_NShot" MaxLength="70" runat="server"
                    LabelKey="per_contactSearchForm_label_firstName" />
            </td>
            <td>
                <ACA:AccelaTextBox ID="txtMiddleName" CssClass="ACA_NShot" runat="server" LabelKey="per_contactSearchForm_label_middleName"
                    MaxLength="70" />
            </td>
            <td>
                <ACA:AccelaTextBox ID="txtLastName" CssClass="ACA_NShot" MaxLength="70" runat="server"
                    LabelKey="per_contactSearchForm_label_lastName" />
            </td>
            <td>
                <ACA:AccelaTextBox ID="txtFullName" CssClass="ACA_NShot" MaxLength="220" runat="server"
                    LabelKey="aca_contactsearch_label_fullname" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divIdentity" runat="server">
    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
        <tr>
            <td>
                <ACA:AccelaCalendarText ID="txtBirthDate" CssClass="ACA_NShot" runat="server" LabelKey="per_contactSearchForm_label_birthDate" />
            </td>
            <td  >
                <ACA:AccelaDropDownList ID="ddlGender" CssClass="ACA_NShot" runat="server" LabelKey="per_contactSearchForm_label_gender" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divBusname" runat="server">
    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
        <tr>
            <td>
                <ACA:AccelaTextBox ID="txtOrganizationName" MaxLength="255" CssClass="ACA_XLong" runat="server"
                    LabelKey="per_contactSearchForm_label_organization" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divTradeName" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <ACA:AccelaTextBox ID="txtTradeName" MaxLength="100" CssClass="ACA_XLong" runat="server"
                    LabelKey="per_contactsearchform_label_tradename"></ACA:AccelaTextBox>
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divFein" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <ACA:AccelaSSNText ID="txtSSN" MaxLength="11" LabelKey="per_contactsearchform_label_ssn" runat="server" IsIgnoreValidate="true"></ACA:AccelaSSNText>
            </td>
            <td>
                <ACA:AccelaFeinText ID="txtFein" MaxLength="11" CssClass="ACA_NShot" runat="server"
                    LabelKey="per_contactsearchform_label_fein" IsIgnoreValidate="true"></ACA:AccelaFeinText>
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divAddress1" runat="server">
    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
        <tr>
            <td>
                <ACA:AccelaTextBox ID="txtStreetAdd1" MaxLength="200" CssClass="ACA_XLong" runat="server"
                    LabelKey="per_contactSearchForm_label_addressLine1" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divAddress2" runat="server">
    <table role='presentation'>
        <tr>
            <td>
                <ACA:AccelaTextBox ID="txtStreetAdd2" MaxLength="200" CssClass="ACA_XLong" runat="server"
                    LabelKey="per_contactSearchForm_label_addressLine2" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divAddress3" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <ACA:AccelaTextBox ID="txtStreetAdd3" MaxLength="200" CssClass="ACA_XLong" runat="server"
                    LabelKey="per_contactsearchform_label_addressline3"></ACA:AccelaTextBox>
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divCity" runat="server">
    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
        <tr>
            <td>
                <ACA:AccelaTextBox ID="txtCity" PositionID="SearchByContactCity" AutoFillType="City" runat="server" MaxLength="30" CssClass="ACA_NLong"
                    LabelKey="per_contactSearchForm_label_city" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divState" runat="server">
    <table role='presentation' class="ACA_TDAlignLeftOrRightTop">
        <tr>
            <td>
                <ACA:AccelaStateControl ID="txtState" PositionID="SearchByContactState" AutoFillType="State" LabelKey="per_contactSearchForm_label_state"
                    runat="server" CssClass="ACA_NLong" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divZipText" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <ACA:AccelaZipText ID="txtZip" runat="server" CssClass="ACA_NShot" IsIgnoreValidate="true" LabelKey="per_contactSearchForm_label_zip" />
            </td>
            <td>
                <ACA:AccelaTextBox ID="txtPOBox" MaxLength="40" runat="server" CssClass="ACA_NShot"
                    LabelKey="per_contactSearchForm_label_poBox" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divCountry" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <ACA:AccelaCountryDropDownList ID="ddlCountry" LabelKey="per_contactSearchForm_label_country"
                    runat="server" ShowType="showdescription" IsAlwaysShow="true">
                </ACA:AccelaCountryDropDownList>
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divPhone" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <ACA:AccelaPhoneText ID="txtHomePhone" IsIgnoreValidate="true" runat="server" LabelKey="per_contactSearchForm_label_homePhone" />
            </td>
            <td>
                <ACA:AccelaPhoneText ID="txtWorkPhone" IsIgnoreValidate="true" runat="server" LabelKey="per_contactSearchForm_label_workPhone" />
            </td>
            <td>
                <ACA:AccelaPhoneText ID="txtMobilePhone" IsIgnoreValidate="true" runat="server" LabelKey="per_contactSearchForm_label_mobilePhone" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divFax" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <ACA:AccelaPhoneText ID="txtFax" IsIgnoreValidate="true" runat="server" LabelKey="per_contactSearchForm_label_fax" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divEmail" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <ACA:AccelaTextBox ID="txtEmail" CssClass="ACA_NLong" MaxLength="70" runat="server"
                    LabelKey="per_contactSearchForm_label_email" />
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divPreferredChannelAndRace" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <div class="ACA_FLeft">
                    <ACA:AccelaDropDownList ID="ddlPreferredChannel" CssClass="ACA_NShot" runat="server" LabelKey="aca_contactsearch_label_preferredchannel"></ACA:AccelaDropDownList>
                </div>
                <div class="ACA_FLeft">
                    <ACA:AccelaDropDownList ID="ddlRace" LabelKey="aca_contactsearch_label_race" runat="server" />
                </div>
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divIndentity" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <div class="ACA_FLeft">
                    <ACA:AccelaTextBox ID="txtPassportNumber" LabelKey="aca_contactsearch_label_passport_number" runat="server" />
                </div>
                <div class="ACA_FLeft">
                    <ACA:AccelaTextBox ID="txtDriverLicenseNumber" LabelKey="aca_contactsearch_label_driver_license_number" runat="server" />
                </div>
                <div class="ACA_FLeft">
                    <ACA:AccelaStateControl ID="ddlDriverLicenseState" PositionID="SearchByContactDriverLicenseState" AutoFillType="State" LabelKey="aca_contactsearch_label_driver_license_state" runat="server" />
                </div>
                <div class="ACA_FLeft">
                    <ACA:AccelaTextBox ID="txtStateNumber" LabelKey="aca_contactsearch_label_state_id_number" runat="server" />
                </div>
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divRegion" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <div class="ACA_FLeft">
                    <ACA:AccelaCountryDropDownList ID="ddlBirthplaceCountry" LabelKey="aca_contactsearch_label_country" runat="server" />
                </div>
                <div class="ACA_FLeft">
                    <ACA:AccelaTextBox ID="txtBirthplaceCity" PositionID="SearchByContactBirthplaceCity" AutoFillType="City" LabelKey="aca_contactsearch_label_birthplace_city" runat="server" />
                </div>
                <div class="ACA_FLeft">
                    <ACA:AccelaStateControl ID="ddlBirthplaceState" PositionID="SearchByContactBirthplaceState" AutoFillType="State" LabelKey="aca_contactsearch_label_birthplace_state" runat="server" />
                </div>
            </td>
        </tr>
    </table>
</div>
<div class="ACA_TabRow" id="divBusinessNameAndDeceasedDate" runat="server">
    <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <div class="ACA_FLeft">
                    <ACA:AccelaTextBox ID="txtBusinessName2" LabelKey="aca_contactsearch_label_business_name2" runat="server"></ACA:AccelaTextBox>
                </div>
                <div class="ACA_FLeft">
                    <ACA:AccelaCalendarText ID="txtDeceasedDate" LabelKey="aca_contactsearch_label_deceased_date" runat="server" />
                </div>
                <div class="ACA_FLeft">
                    <ACA:AccelaTextBox ID="txtSuffix" MaxLength="10" runat="server" LabelKey="aca_contact_label_suffix"></ACA:AccelaTextBox>
                </div>
            </td>
        </tr>
    </table>
</div>
<div id="divLoadContactTemplate" runat="server" class="ACA_TabRow ACA_Link_Text ACA_Link_Text_FontSize">
    <asp:LinkButton ID="lnkLoadContactTemplate" runat="server" CausesValidation="false"  OnClick="LoadContactTemplate_OnClick">
        <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_expand_icon") %>" src="<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>" CssClass="ACA_XShoter ACA_Content_Collapse"/>
        <ACA:AccelaLabel ID="lblLoadContactTemplate" runat="server" LabelKey="aca_contactsearchfrom_expand"/>
    </asp:LinkButton>
    <br />
</div>
                            
<div id="divExpandCollapse" class="ACA_Link_Text ACA_Link_Text_FontSize ACA_HyperLink" runat="server">
    <a id="btnContactTemplateCollapse" href="javascript:void(0);" onclick='ExpandCollapseContactTemplate();' class="NotShowLoading" title="<%=GetTitleByKey("img_alt_collapse_icon","aca_contactsearchfrom_collapse") %>">
        <img id="imgContactTemplateCollapse" class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_collapse_icon") %>" src="<%=ImageUtil.GetImageURL("caret_expanded.gif") %>" />
        <ACA:AccelaLabel ID="lblContactTemplateExpand" runat="server" LabelKey="aca_contactsearchfrom_expand" CssClass="ACA_Hide" />
        <ACA:AccelaLabel ID="lblContactTemplateCollapse" runat="server" LabelKey="aca_contactsearchfrom_collapse"/>
    </a>
</div> 
<div id ="divNoContactTemplate" runat="server" visible="false" class="ACA_TabRow ACA_CapDetail_NoRecord font12px">
&nbsp;&nbsp;&nbsp;<ACA:AccelaLabel ID="lblNoContactResult" runat="server" LabelKey="no_contact_message" />
</div>                            
<div id="divContactTemplate" runat="server">
    <uc1:TemplateEdit ID="templateEdit" IsForSearch="true" runat="server" />
    <ACA:GenericTemplate ID="genericTemplate" IsHideTemplateTable="True" runat="server" />
</div>
<asp:HiddenField ID="hfContactExpanded" runat="server"/>

 <ACA:AccelaInlineScript runat="server" ID="acelaInlineScript1">
    <script type="text/javascript">
                 
        function ChangeFieldsRelatedTypeFlag() {
            if (!$.global.isAdmin) {           
                SetFieldToDisabled('<%=txtOrganizationName.ClientID %>', false);             
                SetFieldToDisabled('<%=txtTradeName.ClientID %>', false);
                SetFieldToDisabled('<%=txtFein.ClientID %>', false);
                SetFieldToDisabled('<%=txtFirstName.ClientID %>', false);
                SetFieldToDisabled('<%=txtLastName.ClientID %>', false);
                SetFieldToDisabled('<%=txtMiddleName.ClientID %>', false);
                SetFieldToDisabled('<%=txtTitle.ClientID %>', false);
                SetFieldToDisabled('<%=txtSSN.ClientID %>', false);

                var contactTypeFlag = $get('<%=ddlContactTypeFlag.ClientID %>');

                if (contactTypeFlag != null && contactTypeFlag.selectedIndex != null && contactTypeFlag.selectedIndex > -1) {
                    var selectContactTypeFlag = contactTypeFlag[contactTypeFlag.selectedIndex].value;

                    if (selectContactTypeFlag.toLowerCase() == "individual") {
                        SetFieldToDisabled('<%=txtOrganizationName.ClientID %>', true);
                        $('#<%=txtOrganizationName.ClientID %>').val("");
                        SetFieldToDisabled('<%=txtTradeName.ClientID %>', true);
                        $('#<%=txtTradeName.ClientID %>').val("");
                        SetFieldToDisabled('<%=txtFein.ClientID %>', true);
                        $('#<%=txtFein.ClientID %>').val("");
                    }

                    if (selectContactTypeFlag.toLowerCase() == "organization") {
                        SetFieldToDisabled('<%=txtFirstName.ClientID %>', true);
                        $('#<%=txtFirstName.ClientID %>').val("");
                        SetFieldToDisabled('<%=txtLastName.ClientID %>', true);
                        $('#<%=txtLastName.ClientID %>').val("");
                        SetFieldToDisabled('<%=txtMiddleName.ClientID %>', true);
                        $('#<%=txtMiddleName.ClientID %>').val("");
                        SetFieldToDisabled('<%=txtSSN.ClientID %>', true);
                        $('#<%=txtSSN.ClientID %>').val("");
                        SetFieldToDisabled('<%=txtTitle.ClientID %>', true);
                        $('#<%=txtTitle.ClientID %>').val("");
                    }
                }
            }
        }

        ChangeFieldsRelatedTypeFlag();     
    </script>
 </ACA:AccelaInlineScript>