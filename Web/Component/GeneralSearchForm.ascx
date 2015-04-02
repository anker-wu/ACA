<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneralSearchForm.ascx.cs"
    Inherits="Accela.ACA.Web.Component.GeneralSearchForm" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="uc1" %>
<ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
    <ACA:AccelaDropDownList ID="ddlGSSubAgency" runat="server" LabelKey="per_permitList_label_agency"
        OnSelectedIndexChanged="GSSubAgency_IndexChanged" ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip" />
    <ACA:AccelaTextBox ID="txtGSPermitNumber" runat="server" LabelKey="per_permitList_label_permitNum"
         MaxLength="22"></ACA:AccelaTextBox>
    <ACA:AccelaDropDownList ID="ddlGSPermitType" runat="server" LabelKey="per_permitList_label_permitType_ForLabel"
        OnSelectedIndexChanged="GSPermitType_IndexChange" ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip">
    </ACA:AccelaDropDownList>
    <ACA:AccelaTextBox ID="txtGSProjectName" runat="server" LabelKey="per_permitList_label_projectName"
         MaxLength="255"></ACA:AccelaTextBox>
    <ACA:AccelaDropDownList ID="ddlGSCapStatus" runat="server" LabelKey="aca_caphome_capstatus" />
    <ACA:AccelaCalendarText ID="txtGSStartDate"  runat="server" LabelKey="per_permitList_label_startDate"></ACA:AccelaCalendarText>
    <ACA:AccelaCalendarText ID="txtGSEndDate" runat="server" LabelKey="per_permitList_label_endDate"
        ></ACA:AccelaCalendarText>
    <ACA:AccelaDropDownList ID="ddlGSLicenseType" runat="server" LabelKey="per_permitList_label_licenseType">
    </ACA:AccelaDropDownList>
    <ACA:AccelaTextBox ID="txtGSLicenseNumber" runat="server" LabelKey="per_permitList_label_stateLicenseNum"
         MaxLength="30"></ACA:AccelaTextBox>
    <ACA:AccelaDropDownList ID="ddlGSContactType" runat="server" LabelKey="general_license_contacttype">
    </ACA:AccelaDropDownList>
    <ACA:AccelaSSNText ID="txtGSSSN" MaxLength="11" LabelKey="general_license_ssn" runat="server"
        IsIgnoreValidate="true"></ACA:AccelaSSNText>
    <ACA:AccelaFeinText ID="txtGSFEIN" MaxLength="11" LabelKey="general_license_fein"
        IsIgnoreValidate="true" runat="server"></ACA:AccelaFeinText>
    <ACA:AccelaTextBox ID="txtGSFirstName" runat="server" LabelKey="per_permitList_label_firstName"
         MaxLength="70"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSLastName" runat="server" LabelKey="per_permitList_label_lastName"
         MaxLength="70"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSBusiName" runat="server" LabelKey="per_permitList_label_business"
         MaxLength="255"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSBusiName2" runat="server" LabelKey="per_permitList_label_business2"
         MaxLength="255"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSBusiLicense" runat="server" LabelKey="per_permitlist_label_businesslicense"
         MaxLength="15"></ACA:AccelaTextBox>
    <ACA:AccelaCountryDropDownList ID="ddlGSCountry" runat="server" LabelKey="per_permitList_label_Country">
    </ACA:AccelaCountryDropDownList>
    <ACA:AccelaRangeNumberText ID="txtGSNumber" runat="server" LabelKey="per_permitList_label_StreetNo"
         ToolTipLabelKeyRangeFrom="aca_recordsearch_label_streetnofrom" ToolTipLabelKeyRangeTo="aca_recordsearch_label_streetnoto"
         MaxLength="9" IsNeedDot="false"></ACA:AccelaRangeNumberText>
    <ACA:AccelaTextBox ID="txtGSStartFraction" runat="server" LabelKey="per_permitList_label_StartFraction"
         MaxLength="20"></ACA:AccelaTextBox>
    <ACA:AccelaRangeNumberText ID="txtGSStreetEnd" runat="server" LabelKey="per_permitList_label_StreetEnd"
         ToolTipLabelKeyRangeFrom="aca_recordsearch_label_streetendfrom" ToolTipLabelKeyRangeTo="aca_recordsearch_label_streetendto"
         IsNeedDot="false" MaxLength="9"></ACA:AccelaRangeNumberText>
    <ACA:AccelaTextBox ID="txtGSEndFraction" runat="server" LabelKey="per_permitList_label_EndFraction"
         MaxLength="20"></ACA:AccelaTextBox>
    <ACA:AccelaDropDownList ID="ddlGSDirection" runat="server" LabelKey="per_permitList_label_direction">
    </ACA:AccelaDropDownList>
    <ACA:AccelaTextBox ID="txtGSPrefix" runat="server" LabelKey="per_permitList_label_Prefix"
         MaxLength="6"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSStreetName" runat="server" LabelKey="per_permitList_label_street"
         MaxLength="40"></ACA:AccelaTextBox>
    <ACA:AccelaDropDownList ID="ddlGSStreetSuffix" runat="server" LabelKey="per_permitList_label_streetType">
    </ACA:AccelaDropDownList>
    <ACA:AccelaDropDownList ID="ddlGSStreetSuffixDirection" runat="server" LabelKey="per_permitList_label_StreetSuffixDirection">
    </ACA:AccelaDropDownList>
    <ACA:AccelaDropDownList ID="ddlGSUnitType" runat="server" LabelKey="per_permitList_label_unitType">
    </ACA:AccelaDropDownList>
    <ACA:AccelaTextBox ID="txtGSUnitNo" runat="server" LabelKey="per_permitList_label_unit"
         MaxLength="21"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSUnitEnd" runat="server" LabelKey="per_permitList_label_UnitEnd"
         MaxLength="10"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSParcelNo" runat="server" LabelKey="per_permList_label_parcel"
         MaxLength="24"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtLot" runat="server" LabelKey="per_permitList_label_Lot"
         MaxLength="40"></ACA:AccelaTextBox>
    <ACA:AccelaDropDownList ID="ddlSubdivision" runat="server" LabelKey="per_permitList_label_Subdivision">
    </ACA:AccelaDropDownList>
    <ACA:AccelaTextBox ID="txtGSSecondaryRoad" runat="server" LabelKey="per_permitList_label_SecondaryRoad"
         MaxLength="100"></ACA:AccelaTextBox>
    <ACA:AccelaNumberText ID="txtGSSecondaryRoadNo" runat="server" LabelKey="per_permitList_label_SecondaryRoadNo"
         IsNeedDot="false" MaxLength="20"></ACA:AccelaNumberText>
    <ACA:AccelaTextBox ID="txtGSNeighborhoodP" runat="server" LabelKey="per_permitList_label_NeighborhoodP"
         MaxLength="6"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSNeighborhood" runat="server" LabelKey="per_permitList_label_Neighborhood"
         MaxLength="30"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSDescription" runat="server" TextMode="MultiLine" Rows="4"
        LabelKey="per_permitList_Label_description"  MaxLength="255"
        Validate="MaxLength"></ACA:AccelaTextBox>
    <ACA:AccelaNumberText ID="txtGSDistance" runat="server" LabelKey="per_permitList_label_Distance"
         MaxLength="9" DecimalDigitsLength="3" Validate="MaxLength;DecimalDigitsLength"></ACA:AccelaNumberText>
    <ACA:AccelaNumberText ID="txtGSXCoordinator" runat="server" LabelKey="per_permitList_label_Xcoordinator"
         IsNeedNegative="true" MaxLength="18" DecimalDigitsLength="8"
        Validate="MaxLength;DecimalDigitsLength"></ACA:AccelaNumberText>
    <ACA:AccelaNumberText ID="txtGSYCoordinator" runat="server" LabelKey="per_permitList_label_Ycoordinator"
         IsNeedNegative="true" MaxLength="18" DecimalDigitsLength="8"
        Validate="MaxLength;DecimalDigitsLength"></ACA:AccelaNumberText>
    <ACA:AccelaTextBox ID="txtGSInspectionDP" runat="server" LabelKey="per_permitList_label_InspectionDP"
         MaxLength="6"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSInspectionD" runat="server" LabelKey="per_permitList_label_InspectionD"
         MaxLength="255"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSCity" PositionID="GeneralSearchCity" AutoFillType="City"
        runat="server" LabelKey="per_permitList_label_city"  MaxLength="30"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSCounty" runat="server" LabelKey="per_permitList_label_County"
         MaxLength="30"></ACA:AccelaTextBox>
    <ACA:AccelaStateControl ID="ddlGSState" PositionID="GeneralSearchState" AutoFillType="State"
        LabelKey="per_permitList_label_state" runat="server" />
    <ACA:AccelaZipText ID="txtGSAppZipSearchPermit"  runat="server"
        IsIgnoreValidate="true" LabelKey="per_permitList_label_zip"></ACA:AccelaZipText>
    <ACA:AccelaTextBox ID="txtGSStreetAddress" TextMode="MultiLine" Rows="4" runat="server"
         LabelKey="per_permitList_label_StreetFlag" MaxLength="1024"
        Validate="MaxLength"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSAddressLine1" runat="server" LabelKey="per_permitList_label_AddressLine1"
         MaxLength="200"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtGSAddressLine2" runat="server" LabelKey="per_permitList_label_AddressLine2"
         MaxLength="200"></ACA:AccelaTextBox>

    <ACA:AccelaTextBox ID="txtLevelPrefix" runat="server" LabelKey="aca_generalsearch_label_levelprefix"
        MaxLength="20" Validate="MaxLength">
    </ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtLevelNbrStart" runat="server" LabelKey="aca_generalsearch_label_levelnumberstart"
        MaxLength="20" Validate="MaxLength">
    </ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtLevelNbrEnd" runat="server" LabelKey="aca_generalsearch_label_levelnumberend"
        MaxLength="20" Validate="MaxLength">
    </ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtHouseAlphaStart" runat="server" LabelKey="aca_generalsearch_label_housealphastart"
        MaxLength="20" Validate="MaxLength">
    </ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtHouseAlphaEnd" runat="server" LabelKey="aca_generalsearch_label_housealphaend"
        MaxLength="20" Validate="MaxLength">
    </ACA:AccelaTextBox>


    <uc1:TemplateEdit ID="templateSearch" IsForSearch="true" runat="server" />
</ACA:AccelaFormDesignerPlaceHolder>