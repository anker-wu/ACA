<%@ Control Inherits="Accela.ACA.Web.Component.ContactAddressEdit" Language="C#" AutoEventWireup="true"
CodeBehind="ContactAddressEdit.ascx.cs" ClassName="ContactAddressEdit"%>
<%@ Import Namespace="Accela.ACA.Common" %>
<div id="divContactAddressEdit">
<asp:UpdatePanel ID="contactAddressEditPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <ACA:AccelaHeightSeparate ID="sepForContactAddressTitle" runat="server" Height="5" />
            <ACA:AccelaLabel ID="lblMsgBar" runat="server" Visible="false"/>
            <div id="divDeactiveContactAddress" visible="false" runat="server">
                <ACA:AccelaLabel ID="lblReplaceContactAddressHint" runat="server" LabelKey="aca_contactaddress_label_replace_hint" CssClass="ACA_Label ACA_Label_FontSize"/>
                <ACA:AccelaCalendarText ID="txtDeactivatedContactAddressEndDate" LabelKey="aca_contactaddress_label_deactivate_enddate" runat="server"></ACA:AccelaCalendarText>
                <ACA:AccelaLabel ID="lblReplaceContactAddressTitle" runat="server" LabelKey="aca_contactaddress_label_replace_title" CssClass="ACA_Label ACA_Label_FontSize"/>
            </div>
            <!--The "validateFlag" field not displayed in UI and just used in Contact Address Expression for Contact Address Validation.-->
            <ACA:AccelaTextBox ID="validateFlag" IsHidden="True" runat="server" ToolTipLabelKey="aca_contactaddress_label_validate"/>
            <ACA:AccelaFormDesignerPlaceHolder ID="fdphContentAddress" runat="server">
                <ACA:AccelaDropDownList ID="ddlAddressType" runat="server" LabelKey="aca_contactaddress_label_addresstype"></ACA:AccelaDropDownList>
                <ACA:AccelaCalendarText ID="txtStartDate" runat="server" LabelKey="aca_contactaddress_label_startdate"></ACA:AccelaCalendarText>
                <ACA:AccelaCalendarText ID="txtEndDate" runat="server" LabelKey="aca_contactaddress_label_enddate"></ACA:AccelaCalendarText>
                <ACA:AccelaTextBox ID="txtRecipient" runat="server" LabelKey="aca_contactaddress_label_recipient" MaxLength="220"></ACA:AccelaTextBox>
                <ACA:AccelaTextBox ID="txtFullAddress" runat="server" LabelKey="aca_contactaddress_label_fulladdress" MaxLength="1024"></ACA:AccelaTextBox>
                <ACA:AccelaTextBox ID="txtAddressLine1" runat="server" LabelKey="aca_contactaddress_label_addressline1" MaxLength="200"></ACA:AccelaTextBox>
                <ACA:AccelaTextBox ID="txtAddressLine2" runat="server" LabelKey="aca_contactaddress_label_addressline2" MaxLength="200"></ACA:AccelaTextBox>
                <ACA:AccelaTextBox ID="txtAddressLine3" runat="server" LabelKey="aca_contactaddress_label_addressline3" MaxLength="200"></ACA:AccelaTextBox>
                <ACA:AccelaNumberText ID="txtStreetStart" runat="server" LabelKey="aca_contactaddress_label_streetstart" MaxLength="9"></ACA:AccelaNumberText>
                <ACA:AccelaNumberText ID="txtStreetEnd" runat="server" LabelKey="aca_contactaddress_label_streetend" MaxLength="9"></ACA:AccelaNumberText>
                <ACA:AccelaDropDownList ID="ddlStreetDirection" runat="server" LabelKey="aca_contactaddress_label_direction"></ACA:AccelaDropDownList>
                <ACA:AccelaTextBox ID="txtPrefix" runat="server" LabelKey="aca_contactaddress_label_prefix" MaxLength="20"></ACA:AccelaTextBox>
                <ACA:AccelaTextBox ID="txtStreetName" runat="server" LabelKey="aca_contactaddress_label_streetname" MaxLength="40"></ACA:AccelaTextBox>
                <ACA:AccelaDropDownList ID="ddlStreetType" runat="server" LabelKey="aca_contactaddress_label_streettype"></ACA:AccelaDropDownList>
                <ACA:AccelaDropDownList ID="ddlUnitType" runat="server" LabelKey="aca_contactaddress_label_unittype"></ACA:AccelaDropDownList>
                <ACA:AccelaTextBox ID="txtUnitStart" runat="server" LabelKey="aca_contactaddress_label_unitstart" MaxLength="10"></ACA:AccelaTextBox>
                <ACA:AccelaTextBox ID="txtUnitEnd" runat="server" LabelKey="aca_contactaddress_label_unitend" MaxLength="10"></ACA:AccelaTextBox>
                <ACA:AccelaDropDownList ID="ddlStreetSuffixDirection" runat="server" LabelKey="aca_contactaddress_label_streetsuffixdirection"></ACA:AccelaDropDownList>
                <ACA:AccelaCountryDropDownList ID="ddlCountry" runat="server" LabelKey="aca_contactaddress_label_country" ShowType="showdescription" IsAlwaysShow="true">
                </ACA:AccelaCountryDropDownList>
                <ACA:AccelaTextBox ID="txtCity" PositionID="SpearFormAddressCity" AutoFillType="City"
                    runat="server" LabelKey="aca_contactaddress_label_city" MaxLength="32"></ACA:AccelaTextBox>
                <ACA:AccelaStateControl ID="txtState" PositionID="SpearFormAddressState" runat="server"
                    AutoFillType="State" LabelKey="aca_contactaddress_label_state"></ACA:AccelaStateControl>
                <ACA:AccelaZipText ID="txtZip" runat="server" LabelKey="aca_contactaddress_label_zip"></ACA:AccelaZipText>
                <ACA:AccelaPhoneText ID="txtPhone" runat="server" LabelKey="aca_contactaddress_label_phone"></ACA:AccelaPhoneText>
                <ACA:AccelaPhoneText ID="txtFax" runat="server" LabelKey="aca_contactaddress_label_fax"></ACA:AccelaPhoneText>
                <ACA:AccelaCheckBox ID="ckbPrimary" LabelKey="aca_contactaddress_label_primary" runat="server" />
                <ACA:AccelaCalendarText ID="caEndDate" LabelKey="aca_contactaddress_label_enddate" runat="server"></ACA:AccelaCalendarText>
                <ACA:AccelaDropDownList ID="ddlStatus" LabelKey="aca_contactaddress_label_status" runat="server"></ACA:AccelaDropDownList>
                <ACA:AccelaTextBox ID="txtLevelPrefix" runat="server" LabelKey="aca_contactaddress_label_levelprefix" MaxLength="20" />
                <ACA:AccelaTextBox ID="txtLevelNbrStart" runat="server" LabelKey="aca_contactaddress_label_levelnumberstart" MaxLength="20" />
                <ACA:AccelaTextBox ID="txtLevelNbrEnd" runat="server" LabelKey="aca_contactaddress_label_levelnumberend" MaxLength="20" />
                <ACA:AccelaTextBox ID="txtHouseAlphaStart" runat="server" LabelKey="aca_contactaddress_label_housealphastart" MaxLength="20" />
                <ACA:AccelaTextBox ID="txtHouseAlphaEnd" runat="server" LabelKey="aca_contactaddress_label_housealphaend" MaxLength="20" />
            </ACA:AccelaFormDesignerPlaceHolder>
            <ACA:AccelaHeightSeparate ID="sepForSearch" runat="server" Height="5" />
        <asp:HiddenField ID="hdfFormStatus" runat="server" />
        <asp:HiddenField ID="hdfDisableContactForm" runat="server" />
        <asp:HiddenField ID="hdfDisableFormByDeactivate" runat="server" />
        <asp:HiddenField ID="hdfDeactivateFlag" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
</div>
<script type="text/javascript">
    var <%=ClientID %>_ContactAddressFormControlIDs;
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    var imgCollapsed = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
    var imgExpanded = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
    var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>';
    var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';

    AddValidationSectionID('<%=ClientID %>');

    function <%=ClientID %>_ButtonClientClick() {
        SetNotAskForSPEAR();
        SetCurrentValidationSectionID('<%=ClientID %>');
    }

    function <%=ClientID %>_PageLoaded(sender, args) {
        //Prepare the excluded fields for Contact edit from validation.
        <%=ClientID %>_ContactAddressFormControlIDs = [];
        $("input[id^='<%=ClientID %>'],select[id^='<%=ClientID %>']").each(function() {
            <%=ClientID %>_ContactAddressFormControlIDs.push(this.id);
        });
    }

    prm.add_pageLoaded(<%=ClientID %>_PageLoaded);
</script>
