<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.AddressEdit"
    CodeBehind="AddressEdit.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="uc1" %>
<%@ Register Src="~/Component/AddressList.ascx" TagName="AddressList" TagPrefix="uc2" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="uc1" %>
<%@ Register Src="~/Component/ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<ACA:AccelaHideLink ID="hlBegin" runat="server" AltKey="img_alt_form_begin" CssClass="ACA_Hide" />
<asp:UpdatePanel ID="mapPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:ACAMap ID="mapAddress" AGISContext="AddressDetailSpear" GISButtonLabelKey="per_AddressList_label_buttonGISSearch" OnShowOnMap="MapAddress_ShowOnMap" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="panAddress" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hfDisableFormFlag" runat="server" />
        <div id="divAutoFill" class="ACA_TabRow_AutoFill" runat="server">
            <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
                <tr>
                    <td>
                        <ACA:AccelaCheckBox ID="chkAutoFillAddressInfo" runat="server" AutoPostBack="false"  LabelKey="aca_addressedit_label_autofill" />
                    </td>
                    <td>
                        <ACA:AccelaDropDownList ID="ddlAutoFillAddressInfo" runat="server" AutoPostBack="false"
                            Enabled="false" IsHiddenLabel="true" LabelKey="aca_address_label_autofilldropdown" 
                            ToolTipLabelKey="aca_common_msg_dropdown_autofillrecord_tip" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="txtRefAddressId" runat="server"/>
        <asp:HiddenField ID="txtAddressUID" runat="server"/>
        <asp:HiddenField ID="txtSourceSeq" runat="server"/>
        <!--
        0: Default status.
        1: Postback triggered by auto-fill function, so need to re-validate edit form in the EndRequest event.
         -->
        <asp:HiddenField ID="hdfPostbackFlag" Value="0" runat="server" />
        
        <div id="divAddressSearchPanel" runat="server">
        <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
            <ACA:AccelaCountryDropDownList ID="ddlCountry" runat="server" LabelKey="per_workLocationInfo_label_country">
            </ACA:AccelaCountryDropDownList>
            <ACA:AccelaNumberText ID="txtStreetNo" runat="server" IsNeedDot="False" MaxLength="9" LabelKey="per_workLocationInfo_label_streetNo">
            </ACA:AccelaNumberText>
            <ACA:AccelaTextBox ID="txtStartFraction" runat="server" LabelKey="per_workLocationInfo_label_startFraction"
                MaxLength="20">
            </ACA:AccelaTextBox>
            <ACA:AccelaNumberText ID="txtStreetEnd" runat="server" LabelKey="per_workLocationInfo_label_streetEnd"
                IsNeedDot="false" MaxLength="9">
            </ACA:AccelaNumberText>
            <ACA:AccelaTextBox ID="txtEndFraction" runat="server" LabelKey="per_workLocationInfo_label_endFraction"
                MaxLength="20">
            </ACA:AccelaTextBox>
            <ACA:AccelaDropDownList ID="ddlStreetDirection" runat="server" LabelKey="per_workLocationInfo_label_direction">
            </ACA:AccelaDropDownList>
            <ACA:AccelaTextBox ID="txtPrefix" runat="server" LabelKey="per_workLocationInfo_label_prefix"
                MaxLength="20">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtStreetName" runat="server" LabelKey="per_workLocationInfo_label_streetName"
                MaxLength="40">
            </ACA:AccelaTextBox>
            <ACA:AccelaDropDownList ID="ddlStreetSuffix" runat="server" LabelKey="per_workLocationInfo_label_streetType">
            </ACA:AccelaDropDownList>
            <ACA:AccelaDropDownList ID="ddlStreetSuffixDirection" runat="server" LabelKey="per_workLocationInfo_label_streetSuffixDirection">
            </ACA:AccelaDropDownList>
            <ACA:AccelaDropDownList ID="ddlUnitType" runat="server" LabelKey="per_workLocationInfo_label_unitType">
            </ACA:AccelaDropDownList>
            <ACA:AccelaTextBox ID="txtUnitNo" runat="server" LabelKey="per_workLocationInfo_label_unitNo"
                MaxLength="10">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtUnitEnd" runat="server" LabelKey="per_workLocationInfo_label_unitEnd"
                MaxLength="10">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtSecondaryRoad" runat="server" LabelKey="per_workLocationInfo_label_secondaryRoad"
                MaxLength="100">
            </ACA:AccelaTextBox>
            <ACA:AccelaNumberText ID="txtSecondaryRoadNo" runat="server" LabelKey="per_workLocationInfo_label_secondaryRoadNo"
                IsNeedDot="false" MaxLength="20">
            </ACA:AccelaNumberText>
            <ACA:AccelaTextBox ID="txtNeighborhoodP" runat="server" LabelKey="per_workLocationInfo_label_neighborhoodP"
                MaxLength="6">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtNeighborhood" runat="server" LabelKey="per_workLocationInfo_label_neighborhood"
                MaxLength="30">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4"
                LabelKey="per_workLocationInfo_label_description" MaxLength="255" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaNumberText ID="txtDistance" runat="server" LabelKey="per_workLocationInfo_label_distance"
                MaxLength="9" DecimalDigitsLength="3" Validate="MaxLength;DecimalDigitsLength">
            </ACA:AccelaNumberText>
            <ACA:AccelaNumberText ID="txtXCoordinator" runat="server" LabelKey="per_workLocationInfo_label_xcoordinator"
                IsNeedNegative="true" MaxLength="20" DecimalDigitsLength="8" Validate="MaxLength;DecimalDigitsLength">
            </ACA:AccelaNumberText>
            <ACA:AccelaNumberText ID="txtYCoordinator" runat="server" LabelKey="per_workLocationInfo_label_ycoordinator"
                IsNeedNegative="true" MaxLength="20" DecimalDigitsLength="8" Validate="MaxLength;DecimalDigitsLength">
            </ACA:AccelaNumberText>
            <ACA:AccelaTextBox ID="txtInspectionDP" runat="server" LabelKey="per_workLocationInfo_label_inspectionDP"
                MaxLength="6">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtInspectionD" runat="server" LabelKey="per_workLocationInfo_label_inspectionD"
                MaxLength="255">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtCity" PositionID="SpearFormAddressCity" AutoFillType="City"
                runat="server" LabelKey="per_workLocationInfo_label_city" MaxLength="32">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtCounty" runat="server" LabelKey="per_workLocationInfo_label_county"
                MaxLength="30">
            </ACA:AccelaTextBox>
            <ACA:AccelaStateControl ID="txtState" PositionID="SpearFormAddressState" runat="server"
                AutoFillType="State" LabelKey="per_workLocationInfo_label_state"></ACA:AccelaStateControl>
            <ACA:AccelaZipText ID="txtZip" runat="server" LabelKey="per_workLocationInfo_label_zip">
            </ACA:AccelaZipText>
            <ACA:AccelaTextBox ID="txtStreetAddress" runat="server" TextMode="MultiLine" Rows="4"
                LabelKey="per_workLocationInfo_label_streetAddress" MaxLength="1024" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAddressLine1" runat="server" LabelKey="per_workLocationInfo_label_addressLine1"
                MaxLength="200" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAddressLine2" runat="server" LabelKey="per_workLocationInfo_label_addressLine2"
                MaxLength="200" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtLevelPrefix" runat="server" LabelKey="aca_worklocationinfo_label_levelprefix"
                MaxLength="20" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtLevelNbrStart" runat="server" LabelKey="aca_worklocationinfo_label_levelnumberstart"
                MaxLength="20" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtLevelNbrEnd" runat="server" LabelKey="aca_worklocationinfo_label_levelnumberend"
                MaxLength="20" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtHouseAlphaStart" runat="server" LabelKey="aca_worklocationinfo_label_housealphastart"
                MaxLength="20" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtHouseAlphaEnd" runat="server" LabelKey="aca_worklocationinfo_label_housealphaend"
                MaxLength="20" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaRangeNumberText ID="txtStreetNo4Search" runat="server" IsNeedDot="false" MaxLength="9"
                LabelKey="per_workLocationInfo_label_streetNo" ToolTipLabelKeyRangeFrom="aca_capedit_label_streetnofrom"
                ToolTipLabelKeyRangeTo="aca_capedit_label_streetnoto">
            </ACA:AccelaRangeNumberText>
            <ACA:AccelaRangeNumberText ID="txtStreetEnd4Search" runat="server" LabelKey="per_workLocationInfo_label_streetEnd"
                ToolTipLabelKeyRangeFrom="aca_capedit_label_streetendfrom" ToolTipLabelKeyRangeTo="aca_capedit_label_streetendto"
                IsNeedDot="false" MaxLength="9">
            </ACA:AccelaRangeNumberText>
            <uc1:TemplateEdit ID="templateEdit" runat="server" />
        </ACA:AccelaFormDesignerPlaceHolder>
        </div>

        <div class="ACA_TabRow ACA_LiLeft addressedit_button_section">
            <ul>
                <span class="ACA_Error_Indicator" style="margin-top: 4px;" runat="server" id="imgErrorIcon"
                    visible="true">
                    <img class="ACA_Error_Indicator_In_Same_Row" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>"
                        src="<%=ImageUtil.GetImageURL("error_16.gif") %>" />
                </span>
                <li runat="server" id="liSearchButton">
                    <ACA:AccelaButton ID="btnSearch" runat="server" LabelKey="per_permitList_label_buttonSearchEdit"
                        OnClick="SearchButton_Click" CausesValidation="false" OnClientClick="return AfterAddressButtonClick();"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                    </ACA:AccelaButton>
                </li>
                <li>
                    <ACA:AccelaButton ID="btnClearAddress" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                        DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" runat="server"
                        LabelKey="per_permitList_label_buttonClear" OnClick="ClearAddressButton_Click" CausesValidation="false"
                        OnClientClick="AfterAddressButtonClick()">
                    </ACA:AccelaButton>
                </li>
            </ul>
        </div>
        <uc2:AddressList ID="ucAddressList" runat="server" OnPageIndexChanging="AddressList_GridViewIndexChanging" OnGridViewSort="AddressList_GridViewSort" />
        <a id="lnkAddressResult"></a>
        <!-- begin condition notice -->
        <div id="divAddressConditionMessage" class="ACA_Hide">
        </div>
        <div id="divAddressConditionInfo" class="ACA_Hide">
            <ACA:AccelaHeightSeparate ID="sepLineForCondition" runat="server" Height="10" />
            <ACA:AccelaLabel ID="lblAddressConditionTitle" LabelKey="aca_permit_address_conditions_title" runat="server" LabelType="SectionTitle"/>
            <div id="divAddressConditionList">
            </div>
        </div>
        <div id="divAddressConditon">
            <uc1:Conditions ID="ucConditon" runat="server" />
        </div>
        <!-- end condition notice -->
        <asp:HiddenField ID="hfDuplicateAddressKeys"  runat="server"/>
        <asp:LinkButton ID="btnPostback" runat="Server" OnClick="PostbackButton_Click" CssClass="ACA_Hide" TabIndex="-1"></asp:LinkButton>
        <asp:LinkButton ID="btnUpdateAddressAndAssociates" runat="Server" OnClick="UpdateAddressAndAssociatesButton_Click" CssClass="ACA_Hide" TabIndex="-1"></asp:LinkButton>
        <ACA:AccelaInlineScript ID="AccelaInlineScript1" runat="server">
            <script type="text/javascript">
                $("#<%=divAddressSearchPanel.ClientID %>").keydown(function (event) {
                    pressEnter2TriggerClick(event, $("#<%=btnSearch.ClientID %>"));
                });
            </script>
        </ACA:AccelaInlineScript>
    </ContentTemplate>
</asp:UpdatePanel>
<ACA:AccelaHideLink ID="hlEnd" runat="server" AltKey="aca_common_msg_formend" CssClass="ACA_Hide" />
<ACA:AccelaHeightSeparate ID="sepLineForAddress" runat="server" Height="25" />

<script type="text/javascript"> 
    
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(<%=ClientID %>_EndRequest);
    prm.add_pageLoaded(<%=ClientID %>_PageLoaded);

    function <%=ClientID %>_EndRequest(sender, args)
    {
        if($get('<%=hdfPostbackFlag.ClientID %>').value == '1') 
        {
            //Postback triggered by auto-fill function, so need to re-validate edit form in the EndRequest event.
            $get('<%=hdfPostbackFlag.ClientID%>').value = '0';
            ShowAddressCondition();
        }
    }

    function <%=ClientID %>_PageLoaded(sender, args)
    {
        if($get('<%=hfDisableFormFlag.ClientID %>').value == 'Y') 
        {
            disableAddressForm(true);
        }
    }

    function ddlAutoFillAddressChanged()
    {
        var ddl = document.getElementById("<%=ddlAutoFillAddressInfo.ClientID%>"); 
        FillWithMyAddressInfo(ddl.value,FillWithAddressInfo);
    }

    function IsShowAddressLockCondition(value) {
        var isShowLockCondition = false;
        
        if (typeof(value) == "string" && value != "") {
            isShowLockCondition = true;
        }
        
        return isShowLockCondition;
    }
    
    var IsShowAddressCondition = false;
        
    function showDetailAddressCondition(a)
    { 
        $get("divAddressConditionInfo").className = IsShowAddressCondition? "ACA_Hide" : "ACA_Show";
        a.innerHTML = IsShowAddressCondition ? '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>':'<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
        IsShowAddressCondition = !IsShowAddressCondition;
    }
    
    function GetAddressConditionInfo(value) {
        if (IsShowAddressLockCondition(value)) {
            PageMethods.GetAddressCondtionMessage(value,moduleName,CallbackAddressConditionInfo);
            $get("divAddressConditon").className = "ACA_Hide";
        }
    }

    function CallbackAddressConditionInfo(result) {
        var divAddressConditionMessage = document.getElementById("divAddressConditionMessage");
        var spanConditionPaddingTop = document.getElementById("spanConditionPaddingTop");
        if (result =='')
        {
            if (spanConditionPaddingTop)
                spanConditionPaddingTop.className = "ACA_Hide";
            if(divAddressConditionMessage)
                divAddressConditionMessage.className = "ACA_Hide";
            document.getElementById("divAddressConditionInfo").className = "ACA_Hide";
        }
        else
        {
            if (spanConditionPaddingTop)
                spanConditionPaddingTop.className = "ACA_Show";
            if(divAddressConditionMessage)
                divAddressConditionMessage.className = "ACA_Show";
        } 
        if(divAddressConditionMessage)
            divAddressConditionMessage.innerHTML = result;
    } 
    
    function chkAutoFillAddressChanged()
    {
        var chk=document.getElementById("<%=chkAutoFillAddressInfo.ClientID%>");
        var ddl = document.getElementById("<%=ddlAutoFillAddressInfo.ClientID%>");

        if(chk != null && ddl != null)
        {
            if(chk.checked)
            {
                FillWithMyAddressInfo(ddl.value,FillWithAddressInfo);
            }
            
            ddl.disabled = !chk.checked;  
        }
    }

    function disableAddressForm(isDisable)
    {
        var chkAutoFill = document.getElementById('<%=chkAutoFillAddressInfo.ClientID %>');        
        var ddlAutoFill = document.getElementById("<%=ddlAutoFillAddressInfo.ClientID%>"); 
        var form = document.getElementById("<%=panAddress.ClientID %>"); 
        disableForm(form,chkAutoFill,ddlAutoFill,isDisable);
    }
    
    function FillWithAddressInfo(info) {
        if (myValidationErrorPanel) {
            myValidationErrorPanel.clearErrors();
        }

        var splitChar = "";
        if (info == '') {
            return;
        }

        var index = info.indexOf(splitChar);
        var normalField;
        var template;
        var templateJson;

        if (index > -1) {
            normalField = info.substring(0, index);
            template = info.substring(index + 1);
            templateJson = eval('(' + template + ')');
        } else {
            normalField = info;
        }

        var json = eval('(' + normalField + ')');
        var countryCtrl = $('#<% = ddlCountry.ClientID %>');

        var isCountryChanged = false;
        var countrySettingJason = '';
        
        if (!IsTrue(json.IsLocked))
        {
            SetValueById('<%=txtStreetNo.ClientID%>',json.houseNumberStart);
            SetValueById('<%=ddlStreetDirection.ClientID%>',json.streetDirection);
            SetValueById('<%=ddlStreetSuffix.ClientID%>',json.streetSuffix);
            SetValueById('<%=ddlUnitType.ClientID%>',json.unitType);
            SetValueById('<%=txtStreetName.ClientID%>',JsonDecode(json.streetName));
            SetValueById('<%=txtCity.ClientID%>',json.city);
            SetValueById('<%=txtZip.ClientID%>',json.zip);
            SetValueById('<%=txtUnitNo.ClientID%>',json.unitStart);
            SetValueById('<%=txtCounty.ClientID%>',json.county);
            SetValueById('<%=txtRefAddressId.ClientID%>',json.refAddressId);
            SetValueById('<%=txtAddressUID.ClientID%>',json.UID);
            SetValueById('<%=txtPrefix.ClientID%>',json.streetPrefix);
            SetValueById('<%=txtStreetEnd.ClientID%>',json.houseNumberEnd);
            SetValueById('<%=txtUnitEnd.ClientID%>',json.unitEnd);
            SetValueById('<%=txtState.ClientID%>',json.state);
            SetValueById('<%=txtStartFraction.ClientID%>',json.houseFractionStart);
            SetValueById('<%=txtEndFraction.ClientID%>',json.houseFractionEnd);
            SetValueById('<%=ddlStreetSuffixDirection.ClientID%>',json.streetSuffixdirection);
            SetValueById('<%=txtDescription.ClientID%>',JsonDecode(json.addressDescription));
            SetValueById('<%=txtDistance.ClientID%>',json.distance);
            SetValueById('<%=txtSecondaryRoad.ClientID%>',json.secondaryRoad);
            SetValueById('<%=txtSecondaryRoadNo.ClientID%>',json.secondaryRoadNumber);
            SetValueById('<%=txtInspectionD.ClientID%>',json.inspectionDistrict);
            SetValueById('<%=txtInspectionDP.ClientID%>',json.inspectionDistrictPrefix);
            SetValueById('<%=txtNeighborhoodP.ClientID%>',json.neighberhoodPrefix);
            SetValueById('<%=txtNeighborhood.ClientID%>',json.neighborhood);
            SetValueById('<%=txtXCoordinator.ClientID%>',json.XCoordinator);
            SetValueById('<%=txtYCoordinator.ClientID%>',json.YCoordinator);
            SetValueById('<%=txtStreetAddress.ClientID%>',JsonDecode(json.fullAddress));
            SetValueById('<%=txtAddressLine1.ClientID%>',JsonDecode(json.addressLine1));
            SetValueById('<%=txtAddressLine2.ClientID%>',JsonDecode(json.addressLine2));

            SetValueById('<%=txtLevelPrefix.ClientID%>',JsonDecode(json.levelPrefix));
            SetValueById('<%=txtLevelNbrStart.ClientID%>',JsonDecode(json.levelNumberStart));
            SetValueById('<%=txtLevelNbrEnd.ClientID%>',JsonDecode(json.levelNumberEnd));
            SetValueById('<%=txtHouseAlphaStart.ClientID%>',JsonDecode(json.houseAlphaStart));
            SetValueById('<%=txtHouseAlphaEnd.ClientID%>',JsonDecode(json.houseAlphaEnd));

            SetValueById('<%=hfDuplicateAddressKeys.ClientID%>',"");
            
            SetValueById('<%=txtSourceSeq.ClientID%>',json.sourceNumber);
            
            if(typeof(templateJson)!="undefined" &&  typeof(<%=templateEdit.ClientID %>_SetTemplateValue) == "function")
            {
                <%=templateEdit.ClientID %>_SetTemplateValue(templateJson);
            }
            
            if (countryCtrl.val() != json.countryCode) {
                if (countryCtrl.is(':visible')) {
                    countryCtrl.val(json.countryCode);
                }

                countrySettingJason = '[{"countryClientID":"' + '<% = ddlCountry.ClientID %>' + '" , "countryCode":"' + json.countryCode + '", "state":"' + json.state + '", "zip" : "' + json.zip + '"}]';
                isCountryChanged = true;
            }
        }

        var isSectionValidate = "<%=IsValidate%>" == "True";
        var isDisable = isSectionValidate && !IsTrue(json.IsLocked);
        if (isDisable) {
            disableAddressForm(isDisable);
            $get('<%=hfDisableFormFlag.ClientID%>').value = "Y";
        }

        if (isCountryChanged) {
            $get('<%=hdfPostbackFlag.ClientID%>').value = '1';

            //If auto-fill changed Country field, need send a postback to apply the regional settings.
            __doPostBack('<%=panAddress.UniqueID %>', '<%=ACAConstant.ACA_COUNTRY_AUTOFILL_FLAG %>' + countrySettingJason);
        }
        else
        {
            ShowAddressCondition();
        }
    }

    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%=btnSearch.ClientID %>");
        myValidationErrorPanel.registerIDs4Recheck("<%=btnClearAddress.ClientID %>");
        myValidationErrorPanel.registerIDs4Recheck("<%=ucAddressList.ClientID %>");
    }


    function <%=CONTROL_VALUE_VALIDATE_FUNCTION %>()
    {
        var result;
        eval('result = typeof(<%=TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION %>) != "undefined";');
        if(result)
        {
            eval('result = <%=TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION %>();');
            if(!result)return false;
        }
    
        var preFix = '<%=btnSearch.ClientID.Replace("btnSearch",string.Empty) %>';
        var control =  $get(preFix + 'ddlStreetDirection');
        var streetDirection = GetValue(control).trim();
        control =  $get(preFix + 'ddlStreetSuffix'); 
        var streetSuffix = GetValue(control).trim();
        control =  $get(preFix + 'ddlUnitType'); 
        var unitType = GetValue(control).trim();
        control =  $get(preFix + 'txtUnitNo'); 
        var unit = GetValue(control).trim();
        control =  $get(preFix + 'txtCounty'); 
        var county = GetValue(control).trim();
        control =  $get(preFix + 'txtStreetNo'); 
        var txtStreetNo = GetValue(control).trim();
        control =  $get(preFix + 'txtStreetName'); 
        var txtStreetName = GetValue(control).trim();
        control =  $get(preFix + 'txtZip'); 
        var txtZip = GetValue(control).trim();
    
        control =  $get(preFix + 'txtPrefix'); 
        var txtPrefix = GetValue(control).trim();
        control =  $get(preFix + 'txtStreetEnd'); 
        var txtStreetEnd = GetValue(control).trim();
        control =  $get(preFix + 'txtUnitEnd'); 
        var txtUnitEnd = GetValue(control).trim();
    
        control =  $get(preFix + 'ddlCountry'); 
        var ddlCountry = GetValue(control).trim();
    
        control =  $get(preFix + 'txtStartFraction'); 
        var txtStartFraction = GetValue(control).trim();
        control =  $get(preFix + 'txtEndFraction'); 
        var txtEndFraction = GetValue(control).trim();
    
//    control =  $get(preFix + 'ddlAddressTypeFlag'); 
//    var ddlAddressTypeFlag = GetValue(control).trim();
    
        control =  $get(preFix + 'ddlStreetSuffixDirection'); 
        var ddlStreetSuffixDirection = GetValue(control).trim();
        control =  $get(preFix + 'txtDescription'); 
        var txtDescription = GetValue(control).trim();
        control =  $get(preFix + 'txtDistance'); 
        var txtDistance = GetValue(control).trim();
    
        control =  $get(preFix + 'txtSecondaryRoad'); 
        var txtSecondaryRoad = GetValue(control).trim();
        control =  $get(preFix + 'txtSecondaryRoadNo'); 
        var txtSecondaryRoadNo = GetValue(control).trim();
    
        control =  $get(preFix + 'txtInspectionDP'); 
        var txtInspectionDP = GetValue(control).trim();
        control =  $get(preFix + 'txtInspectionD'); 
        var txtInspectionD = GetValue(control).trim();
    
        control =  $get(preFix + 'txtNeighborhoodP'); 
        var txtNeighborhoodP = GetValue(control).trim();
        control =  $get(preFix + 'txtNeighborhood'); 
        var txtNeighborhood = GetValue(control).trim();
    
        control =  $get(preFix + 'txtXCoordinator'); 
        var txtXCoordinator = GetValue(control).trim();
        control =  $get(preFix + 'txtYCoordinator'); 
        var txtYCoordinator = GetValue(control).trim();
    
        control =  $get(preFix + 'txtStreetAddress'); 
        var txtStreetAddress = GetValue(control).trim();
   
        control =  $get(preFix + 'txtState_State1'); 
        var ddlAppState = GetValue(control).trim();  
        control =  $get(preFix + 'txtCity'); 
        var txtCity = GetValue(control).trim();
    
//    control =  $get(preFix + 'ddlAddressType'); 
//    var ddlAddressType = GetValue(control).trim();
    
        control =  $get(preFix + 'txtAddressLine1'); 
        var txtAddressLine1 = GetValue(control).trim();
        control =  $get(preFix + 'txtAddressLine2'); 
        var txtAddressLine2 = GetValue(control).trim();


        control =  $get(preFix + 'txtLevelPrefix'); 
        var txtLevelPrefix = GetValue(control).trim();
        control =  $get(preFix + 'txtLevelNbrStart'); 
        var txtLevelNbrStart = GetValue(control).trim();
        control =  $get(preFix + 'txtLevelNbrEnd'); 
        var txtLevelNbrEnd = GetValue(control).trim();
        control =  $get(preFix + 'txtHouseAlphaStart'); 
        var txtHouseAlphaStart = GetValue(control).trim();
        control =  $get(preFix + 'txtHouseAlphaEnd'); 
        var txtHouseAlphaEnd = GetValue(control).trim();


        return streetDirection == '' && ddlAppState == '' && streetSuffix == '' && unitType == '' && unit == '' && county == '' && txtStreetNo == '' && txtStreetName == '' && txtZip == ''
            && txtPrefix == '' && txtStreetEnd == '' && txtUnitEnd == '' && ddlCountry == '' && txtCity == ''
            && txtStartFraction == '' && txtEndFraction == '' && ddlStreetSuffixDirection == '' && txtDescription == '' && txtDistance == ''
            && txtSecondaryRoad == '' && txtSecondaryRoadNo == '' && txtInspectionDP == '' && txtInspectionD == '' && txtNeighborhoodP == '' && txtNeighborhood == ''
            && txtXCoordinator == '' && txtYCoordinator == '' && txtStreetAddress == '' && txtAddressLine1 == '' && txtAddressLine2 == ''
            && txtLevelPrefix == '' && txtLevelNbrStart == '' && txtLevelNbrEnd == '' && txtHouseAlphaStart == '' && txtHouseAlphaEnd == '';
    }

    function OpenGIS(url)
    {
        window.open(url, "_blank","top=20,left=20,height=650px,width=750px,status=no,toolbar=0,menubar=no,location=no,scrollbars=yes");
    }  

    function FillGISAddress(info)
    {
        var clientIdPrefix = '<%=txtStreetNo.ClientID.Replace("txtStreetNo","") %>';
        var reg = /_/igm;
        var id= clientIdPrefix.replace(reg,'$');//.replace('_','$');
        SetNotAskForSPEAR();
        __doPostBack(id + 'btnPostback',info);
    }

    function AfterAddressButtonClick() 
    {
        SetNotAskForSPEAR();
        ResetIsShowSAddressConditionFlg();
        var submitPermitted = myValidationErrorPanel.isValidationPassExceptRequiredField('<%=ClientID %>');
        
        if (!submitPermitted) {            
            return false;
        }
        
        if(typeof (myValidationErrorPanel) != "undefined" && '<%=IsWorkLocationPage %>' == 'False') {
            myValidationErrorPanel.clearCurrentSectionErrors('<%=ClientID %>');
        }

        return true;
    }
    
    var IsShowaddressCondition = false;
    var initialAddressConditionStatus="0";
    
    function ResetIsShowSAddressConditionFlg() 
    {
        IsShowaddressCondition = false;
    }
    
    function showMoreaddressCondition(div,a)
    {
        if(div=='undefined') return;
        if(initialAddressConditionStatus=="0"){
            IsShowaddressCondition=false;
            initialAddressConditionStatus="1";
        }
        
        $get(div).style.display= IsShowaddressCondition?'none': '';
        $get(a).innerHTML = IsShowaddressCondition?'<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>':'<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
        IsShowaddressCondition = !IsShowaddressCondition;
    }
    
    //Call function is defined in Conditions.cs at line 346.
    function EndaddressRequest(linkId,divConditions)
    {
        var lnk = document.getElementById('linkId');
        if (lnk != null) 
        {
            if (IsShowaddressCondition) 
            {
                lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
                $get(divConditions).style.display=  '';
            }
            else 
            {
                lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>';
                $get(divConditions).style.display=  'none';
            }
        }
    }

    function ShowAddressCondition()
    {
        // show condition notice
        var ddl = document.getElementById("<%=ddlAutoFillAddressInfo.ClientID%>");
        GetAddressConditionInfo(ddl.value);

        if(typeof(GetAddressCondition) != "undefined") {
            GetAddressCondition(ddl.value); 
        }
    }

    // Open the page of associated parcel and owner infomation for address.
    function <%=ClientID %>_OpenAddressSearchResult() {
        var url = '<%= Page.ResolveUrl("~/APO/AddressSearchResult.aspx") %>';
        url += '?<%= ACAConstant.MODULE_NAME + "=" + ModuleName %>';
        url += '&<%= UrlConstant.AgencyCode + "=" + ConfigManager.AgencyCode %>';

        var popWidth = 800;
        var popHeight = 250;

        if (window.location.href.indexOf("isPopup") > -1) {
            popWidth = 780;
            popHeight = 1020;
        }

        ACADialog.popup({ url: url, width: popWidth, height: popHeight, objectTarget: '<%= btnSearch.ClientID %>' });

        return false;
    }

    // update after select an Address and its associates.
    function <%=UpdateAddressAndAssociatesFunctionName %>() {
        window.setTimeout(function() {
            delayShowLoading();
        }, 100);

        __doPostBack('<%=btnUpdateAddressAndAssociates.UniqueID %>', '');
    }
</script>