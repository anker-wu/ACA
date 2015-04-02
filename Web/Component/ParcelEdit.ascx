<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ParcelEdit"
    CodeBehind="ParcelEdit.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="~/Component/TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="ucl" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="uc1" %>
<%@ Register Src="~/Component/ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<ACA:AccelaHideLink ID="hlBegin" runat="server" CssClass="ACA_Hide" AltKey="img_alt_form_begin" />
<asp:UpdatePanel ID="mapPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:ACAMap ID="mapParcel" AGISContext="ParcelDetailSpear" GISButtonLabelKey="per_AddressList_label_buttonGISSearch"
            OnShowOnMap="MapParcel_ShowOnMap" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="panParcel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hfDisableFormFlag" runat="server" />
        <div id="divAutoFill" class="ACA_TabRow_AutoFill" runat="server">
            <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
                <tr>
                    <td>
                        <ACA:AccelaCheckBox ID="chkAutoFillParcelInfo" runat="server" AutoPostBack="false"
                            LabelKey="aca_parceledit_label_autofill" />
                    </td>
                    <td>
                        <ACA:AccelaDropDownList ID="ddlAutoFillParcelInfo" runat="server" AutoPostBack="false"
                            Enabled="false" IsHiddenLabel="true" LabelKey="aca_parcel_label_autofilldropdown"
                            ToolTipLabelKey="aca_common_msg_dropdown_autofillrecord_tip" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="txtRefParcelNo" runat="server" />
        <asp:HiddenField ID="txtSourceSeq" runat="server" />
        <asp:HiddenField ID="txtParcelUID" runat="server" />

        <div id="divParcelSearchPanel" runat="server">
        <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
            <ACA:AccelaTextBox ID="txtParcelNo" CssClass="ACA_NLonger" runat="server" MaxLength="24"
                LabelKey="per_parcel_label_parcelNo" IsDBRequired="true">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtLot" runat="server" LabelKey="per_parcel_label_lot" MaxLength="40">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtBlock" runat="server" LabelKey="per_parcel_label_block"
                MaxLength="15">
            </ACA:AccelaTextBox>
            <ACA:AccelaDropDownList ID="ddlSubdivision" runat="server" LabelKey="per_parcel_label_subdivision">
            </ACA:AccelaDropDownList>
            <ACA:AccelaTextBox ID="txtBook" runat="server" LabelKey="per_parcel_label_book" MaxLength="8">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtPage" runat="server" LabelKey="per_parcel_label_page" MaxLength="8">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtTract" runat="server" LabelKey="per_parcel_label_tract"
                TextMode="MultiLine" Rows="6" MaxLength="80" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtLegalDescription" runat="server" LabelKey="per_parcel_label_legalDescription"
                TextMode="MultiLine" Rows="6" MaxLength="2000" Validate="MaxLength">
            </ACA:AccelaTextBox>
            <ACA:AccelaNumberText ID="txtParcelArea" runat="server" LabelKey="per_parcel_label_parcelArea"
                MaxLength="13" MinimumValue="0" MaximumValue="9999999999.99" Validate="MinimumValue;MaximumValue">
            </ACA:AccelaNumberText>
            <ACA:AccelaNumberText ID="txtLandValue" runat="server" LabelKey="per_parcel_label_landValue"
                MaxLength="13" MinimumValue="0" MaximumValue="9999999999.99" Validate="MinimumValue;MaximumValue">
            </ACA:AccelaNumberText>
            <ACA:AccelaNumberText ID="txtImprovedValue" runat="server" LabelKey="per_parcel_label_improvedValue"
                MaxLength="13" MinimumValue="0" MaximumValue="9999999999.99" Validate="MinimumValue;MaximumValue">
            </ACA:AccelaNumberText>
            <ACA:AccelaNumberText ID="txtExceptionValue" runat="server" LabelKey="per_parcel_label_exceptionValue"
                MaxLength="13" MinimumValue="0" MaximumValue="9999999999.99" Validate="MinimumValue;MaximumValue">
            </ACA:AccelaNumberText>
            <ucl:TemplateEdit ID="templateEdit" runat="server" />
        </ACA:AccelaFormDesignerPlaceHolder>
        </div>

        <div style="margin-top: 10px">
        </div>
        <div class="ACA_Row ACA_LiLeft">
            <ul>
                <span class="ACA_Error_Indicator" style="margin-top: 4px;" runat="server" id="imgErrorIcon"
                    visible="true">
                    <img class="ACA_Error_Indicator_In_Same_Row" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>"
                        src="<%=ImageUtil.GetImageURL("error_16.gif") %>" />
                </span>
                <li runat="server" id="liSearchButton">
                    <ACA:AccelaButton ID="btnSearch" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                        DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" runat="server"
                        LabelKey="per_parcel_button_search" OnClick="SearchButton_Click" CausesValidation="false"
                        OnClientClick="return AfterParcelButtonClick();">
                    </ACA:AccelaButton>
                </li>
                <li>
                    <ACA:AccelaButton ID="btnClear" runat="server" LabelKey="per_parcel_button_clear"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                        OnClick="ClearButton_Click" CausesValidation="false" OnClientClick="AfterParcelButtonClick()">
                    </ACA:AccelaButton>
                </li>
            </ul>
        </div>
        <a id="lnkParcelResult"></a>
        <!-- begin condition notice -->
        <div id="divParcelConditionMessage" class="ACA_Hide">
        </div>
        <div id="divParcelConditionInfo" class="ACA_Hide">
            <ACA:AccelaHeightSeparate ID="sepLineForCondition" runat="server" Height="10" />
            <ACA:AccelaLabel ID="lblParcelConditionTitle" LabelKey="aca_permit_parcel_conditions_title"
                runat="server" LabelType="SectionTitle" />
            <div id="divParcelConditionList">
            </div>
        </div>
        <div id="divParcelConditon">
            <uc1:Conditions ID="ucConditon" runat="server" />
        </div>
        <!-- end condition notice -->
        <asp:HiddenField ID="hfDuplicateParcelKeys" runat="server" />
        <asp:LinkButton ID="btnPostback" runat="Server" OnClick="PostbackButton_Click" CssClass="ACA_Hide"
            TabIndex="-1"></asp:LinkButton>
        <asp:LinkButton ID="btnUpdateParcelAndAssociates" runat="Server" OnClick="UpdateParcelAndAssociatesButton_Click" CssClass="ACA_Hide" TabIndex="-1"></asp:LinkButton>
        <ACA:AccelaInlineScript ID="AccelaInlineScript1" runat="server">
            <script type="text/javascript">
                $("#<%=divParcelSearchPanel.ClientID %>").keydown(function (event) {
                    pressEnter2TriggerClick(event, $("#<%=btnSearch.ClientID %>"));
                });
            </script>
        </ACA:AccelaInlineScript>
    </ContentTemplate>
</asp:UpdatePanel>
<ACA:AccelaHideLink ID="hlEnd" runat="server" CssClass="ACA_Hide" AltKey="aca_common_msg_formend" />
<ACA:AccelaHeightSeparate ID="sepLineForAddress" runat="server" Height="25" />
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_pageLoaded(<%=ClientID %>_PageLoaded);
    
    function <%=ClientID %>_PageLoaded(sender, args)
    {
        if($get('<%=hfDisableFormFlag.ClientID %>').value == 'Y') 
        {
            disableParcelForm(true);
        }
    }

    function ddlAutoFillParcelChanged()
    {
        var ddl = document.getElementById("<%=ddlAutoFillParcelInfo.ClientID%>"); 
        FillWithMyParcelInfo(ddl.value,FillWithParcelInfo);
        
        // show condition notice
        if(typeof(GetParcelConditionInfo) != "undefined"){
            GetParcelConditionInfo(ddl.value, ddl.id);
            GetParcelCondition(ddl.value); 
        }
    }

    function IsShowParcelLockCondition(value) {
        var isShowLockCondition = false;
        
        if (typeof(value) == "string" && value != "") {
            isShowLockCondition = true;
        }
        
        return isShowLockCondition;
    }
    
    var IsShowParcelCondition = false;
        
    function showDetailParcelCondition(a)
    { 
        $get("divParcelConditionInfo").className = IsShowParcelCondition? "ACA_Hide" : "ACA_Show";
        a.innerHTML = IsShowParcelCondition ? '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>':'<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
        IsShowParcelCondition = !IsShowParcelCondition;
    }
    
    function GetParcelConditionInfo(value, ddlId) {
        if (IsShowParcelLockCondition(value)) {
            PageMethods.GetParcelCondtionMessage(value,moduleName,CallbackParcelConditionInfo);
            
            $get("divParcelConditon").className = "ACA_Hide";
        }
    }
    
    function CallbackParcelConditionInfo(result) {        
        var divParcelConditionMessage = document.getElementById("divParcelConditionMessage");
        var spanConditionPaddingTop = document.getElementById("spanConditionPaddingTop");
        if (result =='')
        {
            if (spanConditionPaddingTop)
                spanConditionPaddingTop.className = "ACA_Hide";
            if(divParcelConditionMessage)
                divParcelConditionMessage.className = "ACA_Hide";
            document.getElementById("divParcelConditionInfo").className = "ACA_Hide";
        }
        else
        {
            if (spanConditionPaddingTop)
                spanConditionPaddingTop.className = "ACA_Show";
            if(divParcelConditionMessage)
                divParcelConditionMessage.className = "ACA_Show";
        } 
        if(divParcelConditionMessage)
            divParcelConditionMessage.innerHTML = result;
    } 
    
    function chkAutoFillParcelChanged()
    {
        var chk=document.getElementById("<%=chkAutoFillParcelInfo.ClientID%>"); 
        var ddl = document.getElementById("<%=ddlAutoFillParcelInfo.ClientID%>");
        
        if(chk != null && ddl != null)
        {
            if(chk.checked)
            {
                FillWithMyParcelInfo(ddl.value,FillWithParcelInfo);
                GetParcelConditionInfo(ddl.value, ddl.id);
                GetParcelCondition(ddl.value); 
            }
            
            ddl.disabled = !chk.checked;  
        } 
        
        
    }
    
    function disableParcelForm(isDisable)
    {  
        var chkAutoFill = document.getElementById('<%=chkAutoFillParcelInfo.ClientID %>');        
        var ddlAutoFill = document.getElementById("<%=ddlAutoFillParcelInfo.ClientID%>"); 
        var form = document.getElementById("<%=panParcel.ClientID %>"); 
        disableForm(form,chkAutoFill,ddlAutoFill,isDisable);
     }

    function FillWithParcelInfo(info)
    {
        if (myValidationErrorPanel) {
            myValidationErrorPanel.clearErrors();
        }
        
        var splitChar = "";
        if (info == '')
        {
            return;
        } 
        
        var index = info.indexOf(splitChar);
        var normalField;
        var template;
        var templateJson;
        
        if(index > -1)
        {
            normalField = info.substring(0,index);
            template = info.substring(index+1);
            templateJson = eval('(' + template + ')'); 
        }
        else
        {
            normalField = info;
        }
        
        var json = eval('(' + normalField + ')');
        
        if (!IsTrue(json.IsLocked)){
                SetValueById('<%=txtParcelNo.ClientID%>',json.parcelNumber);
                SetValueById('<%=txtLot.ClientID%>',json.lot);
                SetValueById('<%=txtBlock.ClientID%>',json.block);
                SetValueById('<%=ddlSubdivision.ClientID%>',json.subdivision);
                SetValueById('<%=txtBook.ClientID%>',json.book);
                SetValueById('<%=txtPage.ClientID%>',json.page);
                SetValueById('<%=txtTract.ClientID%>',JsonDecode(json.tract));
                SetValueById('<%=txtLegalDescription.ClientID%>', JsonDecode(json.legalDesc));
                SetValueById('<%=txtParcelArea.ClientID%>',json.parcelArea);
                SetValueById('<%=txtLandValue.ClientID%>',json.landValue);
                SetValueById('<%=txtImprovedValue.ClientID%>',json.improvedValue);
                SetValueById('<%=txtExceptionValue.ClientID%>',json.exemptValue);
                SetValueById('<%=txtRefParcelNo.ClientID%>',json.l1ParcelNo);
                SetValueById('<%=txtParcelUID.ClientID%>',json.UID);
                SetValueById('<%=hfDuplicateParcelKeys.ClientID%>',"");
                SetValueById('<%=txtSourceSeq.ClientID%>',json.sourceSeq);
                
            if(typeof(templateJson)!="undefined" &&  typeof(<%=templateEdit.ClientID %>_SetTemplateValue) == "function"){
                <%=templateEdit.ClientID %>_SetTemplateValue(templateJson);
            } 
        }
         
        var isSectionValidate = "<%=IsValidate%>"=="True"; 
        var isDisable = isSectionValidate &&!IsTrue(json.IsLocked);
        if(isDisable)
        {
            disableParcelForm(isDisable);
            $get('<%=hfDisableFormFlag.ClientID%>').value = "Y";
        }
    }
    
if (typeof (myValidationErrorPanel) != "undefined") {
    myValidationErrorPanel.registerIDs4Recheck("<%=btnSearch.ClientID %>");
    myValidationErrorPanel.registerIDs4Recheck("<%=btnClear.ClientID %>");
}

function IsTempleteValueEmpty()
{
    var result;
    eval('result = typeof(<%=TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION %>) != "undefined";');
    if(result)
    {
        eval('result = <%=TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION %>();');
        return result;
        //if(!result)return false;
    }
    return true;
}

function FillParcelFromGIS(info)
{
    var clientIdPrefix = '<%=txtParcelNo.ClientID.Replace("txtParcelNo","") %>';
    var reg = /_/igm;
    var id= clientIdPrefix.replace(reg,'$');//.replace('_','$');
    SetNotAskForSPEAR();
    __doPostBack(id + 'btnPostback',info);
}

function <%=CONTROL_VALUE_VALIDATE_FUNCTION %>()
{
    if (!IsTempleteValueEmpty())
    {
        return false;
    }
    
    var preFix = '<%=txtParcelNo.ClientID.Replace("txtParcelNo",string.Empty) %>';
    var control =  $get(preFix + 'txtLot');
    var lot = GetValue(control).trim();
    control =  $get(preFix + 'txtBlock'); 
    var block = GetValue(control).trim();
    control =  $get(preFix + 'ddlSubdivision'); 
    var subdivision = GetValue(control).trim();
    control =  $get(preFix + 'txtBook'); 
    var book = GetValue(control).trim();
    control =  $get(preFix + 'txtPage'); 
    var page = GetValue(control).trim();
    control =  $get(preFix + 'txtTract'); 
    var tract = GetValue(control).trim();
    control =  $get(preFix + 'txtLegalDescription'); 
    var legalDescription = GetValue(control).trim();
    control =  $get(preFix + 'txtParcelArea'); 
    var parcelArea = GetValue(control).trim();
    control =  $get(preFix + 'txtLandValue'); 
    var landValue = GetValue(control).trim();
    control =  $get(preFix + 'txtImprovedValue'); 
    var improvedValue = GetValue(control).trim();
    control =  $get(preFix + 'txtExceptionValue'); 
    var exceptionValue = GetValue(control).trim();
    control =  $get(preFix + 'txtParcelNo'); 
    var txtParcelNo = GetValue(control).trim();

    
    return lot == '' && block == '' && subdivision == '' && book == '' && page == '' && tract == '' && legalDescription == '' 
        && parcelArea == '' && landValue == '' && improvedValue == '' && exceptionValue == '' && txtParcelNo == '';
}

    function AfterParcelButtonClick() 
    {
        SetNotAskForSPEAR();
        ResetIsShowParcelConditionFlg();
        var submitPermitted = myValidationErrorPanel.isValidationPassExceptRequiredField('<%=ClientID %>');
        
        if (!submitPermitted) {            
            return false;
        }
        
        if(typeof (myValidationErrorPanel) != "undefined") {
            myValidationErrorPanel.clearCurrentSectionErrors('<%=ClientID %>');
        }

        return true;
    }
    
    var IsShowparcelCondition = false;
    var initialParcelConditionStatus="0";
    function ResetIsShowParcelConditionFlg() 
    {
        IsShowparcelCondition = false;
    }
    
    function showMoreparcelCondition(div,a)
    {
        if(div=='undefined') return;
        if(initialParcelConditionStatus=="0"){
            IsShowparcelCondition=false;
            initialParcelConditionStatus="1";
        }
        
        $get(div).style.display= IsShowparcelCondition?'none': '';
        $get(a).innerHTML = IsShowparcelCondition?'<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>':'<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
        IsShowparcelCondition = !IsShowparcelCondition;
    } 
    
    //Call function is defined in Conditions.cs at line 346.
    function EndparcelRequest(linkId,divConditions)
    {
        var lnk = document.getElementById(linkId);
        if (lnk != null) 
        {
            if (IsShowparcelCondition) 
            {
                lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>'
                $get(divConditions).style.display=  '';
            }
            else 
            {
                lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>'                
                $get(divConditions).style.display=  'none';
            }
        }
    }
    
    function IsValidateParcel()
    {
        var preFix = '<%=txtParcelNo.ClientID.Replace("txtParcelNo",string.Empty) %>';
        var control =  $get(preFix + 'txtLot');
        var lot = GetValue(control).trim();
        control =  $get(preFix + 'txtBlock'); 
        var block = GetValue(control).trim();
        control =  $get(preFix + 'ddlSubdivision'); 
        var subdivision = GetValue(control).trim();
        control =  $get(preFix + 'txtBook'); 
        var book = GetValue(control).trim();
        control =  $get(preFix + 'txtPage'); 
        var page = GetValue(control).trim();
        control =  $get(preFix + 'txtTract'); 
        var tract = GetValue(control).trim();
        control =  $get(preFix + 'txtLegalDescription'); 
        var legalDescription = GetValue(control).trim();
        control =  $get(preFix + 'txtParcelArea'); 
        var parcelArea = GetValue(control).trim();
        control =  $get(preFix + 'txtLandValue'); 
        var landValue = GetValue(control).trim();
        control =  $get(preFix + 'txtImprovedValue'); 
        var improvedValue = GetValue(control).trim();
        control =  $get(preFix + 'txtExceptionValue'); 
        var exceptionValue = GetValue(control).trim();
        control =  $get(preFix + 'txtParcelNo'); 
        var txtParcelNo = GetValue(control).trim();
        
        if((lot!="" || block!="" || subdivision!= "" ||book!=""||page!=""||tract!="" || legalDescription != "" ||parcelArea!=""||landValue!=""
            ||improvedValue!=""||exceptionValue!="" || !IsTempleteValueEmpty()) && (txtParcelNo=="") )
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Open the page of associated parcel and owner infomation for address.
    function <%=ClientID %>_OpenParcelSearchResult() {
        var url = '<%= Page.ResolveUrl("~/APO/ParcelSearchResult.aspx") %>';
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
    function <%=UpdateParcelAndAssociatesFunctionName %>() {
        window.setTimeout(function() {
            delayShowLoading();
        }, 100);

        __doPostBack('<%=btnUpdateParcelAndAssociates.UniqueID %>', '');
    }
</script>