<%@ Control Language="C#" AutoEventWireup="True" Inherits="Accela.ACA.Web.Component.OwnerEdit" Codebehind="OwnerEdit.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="uc1" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="uc1" %>
<%@ Register Src="~/Component/RefAPOOwnerList.ascx" TagName="OwnerList" TagPrefix="ACA" %>
<ACA:AccelaHideLink ID="hlBegin" runat="server" AltKey="img_alt_form_begin" CssClass="ACA_Hide"/>
<asp:UpdatePanel ID="panOwner" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:HiddenField ID="hfRefOwnerId" runat="server" />
    <asp:HiddenField ID="hfSourceSeq" runat="server" />
    <asp:HiddenField ID="hfRefOwnerUID" runat="server" />
    <asp:HiddenField ID="hfDisableFormFlag" runat="server" />
    <asp:HiddenField ID="hfAPOKeys"  runat="server"/>
    <!--
    0: Default status.
    1: Postback triggered by auto-fill function, so need to re-validate edit form in the EndRequest event.
    -->
    <asp:HiddenField ID="hdfPostbackFlag" Value="0" runat="server" />
    <div id="divAutoFill" class="ACA_TabRow_AutoFill" runat="server">
        <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
            <tr>
                <td style="float:left">
                    <ACA:AccelaCheckBox ID="chkAutoFillOwnerInfo" runat="server" AutoPostBack="false" LabelKey="ACA_ContactEdit_AutoFill_Label" />
                </td>
                <td>
                    <ACA:AccelaDropDownList ID="ddlAutoFillOwnerInfo" runat="server" AutoPostBack="false"
                        Enabled="false" IsHiddenLabel="true" LabelKey="aca_owner_label_autofilldropdown" 
                        ToolTipLabelKey="aca_common_msg_dropdown_autofillrecord_tip"/>
                </td>
            </tr>
        </table>
    </div>

    <div id="divOwnerSearchPanel" runat="server">
    <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">        
            <ACA:AccelaTextBox ID="txtTitle"  runat="server" MaxLength="255"
                LabelKey="per_owner_label_title" />
            <ACA:AccelaTextBox ID="txtName"  runat="server" MaxLength="320"
                LabelKey="per_owner_label_name" />
            <ACA:AccelaTextBox ID="txtAddress1"  runat="server" MaxLength="40"
                LabelKey="per_owner_label_address1">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAddress2"  runat="server" MaxLength="40"
                LabelKey="per_owner_label_address2">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtAddress3"  runat="server" MaxLength="40"
                LabelKey="per_owner_label_address3">
            </ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtCity" runat="server" MaxLength="30" 
                AutoFillType="City" PositionID="SpearFormOwnerCity" LabelKey="per_owner_label_city"></ACA:AccelaTextBox>
            <ACA:AccelaStateControl ID="ddlAppState" AutoFillType="State" PositionID="SpearFormOwnerState"
                LabelKey="per_owner_label_state" runat="server" />
            <ACA:AccelaZipText ID="txtZip"  runat="server" MaxLength="10"
                LabelKey="per_owner_label_zip">
            </ACA:AccelaZipText>
            <ACA:AccelaCountryDropDownList ID="ddlCountry" runat="server" LabelKey="per_owner_label_country">
            </ACA:AccelaCountryDropDownList>
            <ACA:AccelaPhoneText ID="txtPhone" runat="server" LabelKey="per_owneredit_phone"></ACA:AccelaPhoneText>
            <ACA:AccelaPhoneText ID="txtFax" runat="server" LabelKey="per_owneredit_fax"></ACA:AccelaPhoneText>
            <ACA:AccelaEmailText ID="txtEmail" MaxLength="70" runat="server" LabelKey="per_owneredit_email"></ACA:AccelaEmailText>
            <uc1:TemplateEdit ID="templateEdit" runat="server" />
        </ACA:AccelaFormDesignerPlaceHolder>
    </div>

        <div class="ACA_TabRow ACA_LiLeft">
            <div>
                <ul>
                    <span class="ACA_Error_Indicator" runat="server" style="position: relative;margin-top: 4px;" id="imgErrorIcon" visible="false">
                        <img class="ACA_Error_Indicator_In_Same_Row" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>"/>
                    </span>
                    <li runat="server" id="liSearchButton"> 
                        <ACA:AccelaButton ID="btnSearch" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" runat="server" LabelKey="per_ownerList_label_buttonSearchEdit"
                            OnClick="SearchButton_Click" CausesValidation="false" OnClientClick="AfterOwnerButtonClick()">
                        </ACA:AccelaButton>
                    </li>
                    <li> 
                        <ACA:AccelaButton ID="btnClearAddress" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" runat="server" LabelKey="per_ownerList_label_buttonClear"
                            OnClick="ClearAddressButton_Click" CausesValidation="false" OnClientClick="AfterOwnerButtonClick()">
                        </ACA:AccelaButton> 
                    </li>
                </ul>
            </div>
        </div>
        <ACA:AccelaHeightSeparate ID="sepLine" runat="server" Height="0"/>
        <a id="lnkOwnerResult"></a>
        <div id="divAllKindMessage" style="display:block">
        <div class="ACA_Message_Locked ACA_Message_Locked_FontSize" id="divLockedMessage" runat="server" visible="false">
            <div runat="Server" id="divLockIcon" class="ACA_Locked_Icon">
                <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_locked") %>" src="<%=ImageUtil.GetImageURL("locked_24.gif") %>"/>
            </div>
            <ACA:AccelaLabel ID="lblLockedMessage" runat="server"></ACA:AccelaLabel>
            <br />
        </div>
        <div class="ACA_Message_Hold ACA_Message_Hold_FontSize" id="divHoldMessage" runat="server" visible="false">
            <div runat="Server" id="divHoldIcon" class="ACA_Hold_Icon">
                <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_hold") %>" src="<%=ImageUtil.GetImageURL("hold_24.gif") %>"/>
            </div>
            <ACA:AccelaLabel ID="lblHoldMessage" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
            <br />
        </div>
        <div class="ACA_Message_Note ACA_Message_Note_FontSize" id="divNoteMessage" runat="server" visible="false">
            <div runat="Server" id="divNoteIcon" class="ACA_Note_Icon" >
                <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_note") %>" src="<%=ImageUtil.GetImageURL("note_24.gif") %>"/>
            </div>
            <ACA:AccelaLabel ID="lblNoteMessage" runat="server"></ACA:AccelaLabel>
            <br />
        </div></div>
        <span id="spanConditionPaddingTop" style="display:none">
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="10" />
        </span>
        <div  id="divMessage" style="display:none">
             
        </div>
        <div id="divOwnerConditionInfo" style="display:none">
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="10" />
            <ACA:AccelaLabel ID="lblOwnerConditionTitle" LabelKey="aca_permit_owner_conditions_title" runat="server" LabelType="SectionTitle"/>
            <div id="divConditionList"></div>
            <br />
        </div>
        <div id="divConditon">
            <uc1:Conditions ID="ucConditon" runat="server" />
        </div>
        <div id="divRefOwnerList">
            <div id="divResultNotice" runat ="server" visible ="false">
                <div class="ACA_TabRow ACA_Line_Content" style="margin-top:9px;">&nbsp;</div>
                <p>
                    <ACA:AccelaLabel ID="lblResultNotice" IsNeedEncode="false" runat="server" ></ACA:AccelaLabel>
                </p>
            </div>
            <ACA:OwnerList ID="ucOwnerList" runat="server" Visible="False" GridViewNumber="60192" IsInSPEARForm="True"
                OnPageIndexChanging="Owner_GridViewIndexChanging" OnGridViewSort="Owner_GridViewSort"
                AllowSelectingByRadioButton="False" RowCommandType="SelectOwner" OnOwnerSelected="Owner_Selected" />
        </div>
        
        <ACA:AccelaInlineScript ID="AccelaInlineScript1" runat="server">
            <script type="text/javascript">
                $("#<%=divOwnerSearchPanel.ClientID %>").keydown(function (event) {
                    pressEnter2TriggerClick(event, $("#<%=btnSearch.ClientID %>"));
                });
            </script>
        </ACA:AccelaInlineScript>

    </ContentTemplate>
</asp:UpdatePanel>
<ACA:AccelaHideLink ID="hlEnd" runat="server" AltKey="aca_common_msg_formend" CssClass="ACA_Hide"/>
<ACA:AccelaHeightSeparate ID="sepForOwnerEditResult" runat="server" Height="25"/>

<script type="text/javascript">
    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%=btnSearch.ClientID %>");
        myValidationErrorPanel.registerIDs4Recheck("<%=btnClearAddress.ClientID %>");
    }

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(<%=ClientID %>_EndRequest);
    prm.add_pageLoaded(<%=ClientID %>_PageLoaded);
    
    function <%=ClientID %>_EndRequest(sender, args) 
    {
        if($get('<%=hdfPostbackFlag.ClientID %>').value == '1')
        {
            //Postback triggered by auto-fill function, so need to re-validate edit form in the EndRequest event.
            $get('<%=hdfPostbackFlag.ClientID%>').value = '0';
            ShowOwnerCondition();
        }
    }

    function <%=ClientID %>_PageLoaded(sender, args)
    {
        if($get('<%=hfDisableFormFlag.ClientID %>').value == 'Y') 
        {
            disableOwnerForm(true);
        }
    }

    function <%=CONTROL_VALUE_VALIDATE_FUNCTION %>()
    {
        var result;
        eval('result = typeof(<%=TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION %>) != "undefined";');
        if (result)
        {
            eval('result = <%=TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION %>();');
            if (!result)
            {
                return false;
            }
        }
    
        var preFix = '<%=btnSearch.ClientID.Replace("btnSearch",string.Empty) %>';
        var control = $get(preFix + 'txtName');
        var txtName = GetValue(control).trim();
        control = $get(preFix + 'txtAddress1');
        var txtAddress1 = GetValue(control).trim();
        control = $get(preFix + 'txtAddress2');
        var txtAddress2 = GetValue(control).trim();
        control = $get(preFix + 'txtAddress3');
        var txtAddress3 = GetValue(control).trim();
        control = $get(preFix + 'txtZip');
        var txtZip = GetValue(control).trim();
        control = $get(preFix + 'ddlAppState_State1');
        var ddlAppState = GetValue(control).trim();
        control = $get(preFix + 'ddlCountry');
        var ddlCountry = GetValue(control).trim();
        control = $get(preFix + 'txtTitle');
        var txtTitle = GetValue(control).trim();
        control =  $get(preFix + 'txtCity'); 
        var txtCity = GetValue(control).trim();
        control =  $get(preFix + 'txtFax'); 
        var txtFax = GetValue(control).trim();
        control =  $get(preFix + 'txtPhone'); 
        var txtPhone = GetValue(control).trim();
        control =  $get(preFix + 'txtEmail'); 
        var txtEmail = GetValue(control).trim(); 

        return txtTitle == '' && txtName == '' && txtAddress1 == '' && txtAddress2 == '' && txtAddress3 == '' 
                && txtZip == '' && ddlAppState == '' && ddlCountry == '' && txtCity == '' && txtFax == '' && txtPhone == '' && txtEmail == '';
    }

    function AfterOwnerButtonClick() 
    {
        SetNotAskForSPEAR();
        ResetIsShowOwnerConditionFlg();
        if(document.getElementById("spanConditionPaddingTop"))
            document.getElementById("spanConditionPaddingTop").style.display='none';
        if(document.getElementById("divMessage"))
            document.getElementById("divMessage").style.display='none';
        if(document.getElementById("divownerConditionList"))
            document.getElementById("divownerConditionList").style.display='none'; 
        document.getElementById("divOwnerConditionInfo").style.display='none'; 
        var submitPermitted = myValidationErrorPanel.isValidationPassExceptRequiredField('<%=ClientID %>');
        
        if (!submitPermitted) {            
            return false;
        }
        
        if(typeof (myValidationErrorPanel) != "undefined") {
            myValidationErrorPanel.clearCurrentSectionErrors('<%=ClientID %>');
        }

        return true;
    }
    
    var IsShowownerCondition = false;
    var initialOwnerConditionStatus="0";
    function ResetIsShowOwnerConditionFlg() 
    {
        IsShowownerCondition = false;
    }
    
    function SetOwnerListVisible()
    { 
        $get("divRefOwnerList").style.display ='block'; 
    }
    
    function showMoreownerCondition(div,a)
    {
        if(div=='undefined') return;
        if(initialOwnerConditionStatus=="0"){
            IsShowownerCondition=false;
            initialOwnerConditionStatus="1";
        }
        $get(div).style.display= IsShowownerCondition?'none': '';
        $get(a).innerHTML = IsShowownerCondition?'<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>':'<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
        IsShowownerCondition = !IsShowownerCondition;
    }
    
    function showDetailOwnerCondition(a)
    { 
        $get("divOwnerConditionInfo").style.display= IsShowownerCondition? 'none' : '';
        a.innerHTML = IsShowownerCondition ? '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>':'<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
        IsShowownerCondition = !IsShowownerCondition;
    }
   
   //Call function is defined in Conditions.cs at line 346.
    function EndownerRequest(linkId,divConditions)
    {
        var lnk = document.getElementById(linkId);
        if (lnk != null) 
        {
            if (IsShowownerCondition) 
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

    function ddlAutoFillOwnerChanged()
    {
        myValidationErrorPanel.clearErrors();
        IsShowownerCondition=false;
        var ddl = document.getElementById("<%=ddlAutoFillOwnerInfo.ClientID%>"); 
        FillWithMyOwnerInfo(ddl.value,FillWithOwnerInfo); 
    }
    
    function chkAutoFillOwnerChanged()
    {
        var chk=document.getElementById("<%=chkAutoFillOwnerInfo.ClientID%>"); 
        var ddl = document.getElementById("<%=ddlAutoFillOwnerInfo.ClientID%>");  
        
        if(chk != null && ddl != null)
        {
            if(chk.checked)
            {
                myValidationErrorPanel.clearErrors();
                IsShowownerCondition=false;
                FillWithMyOwnerInfo(ddl.value,FillWithOwnerInfo);
            }

            ddl.disabled = !chk.checked;  
        }
    }

    function disableOwnerForm(isDisable)
    {  
        var chkAutoFill = document.getElementById('<%=chkAutoFillOwnerInfo.ClientID %>');
        var ddlAutoFill = document.getElementById("<%=ddlAutoFillOwnerInfo.ClientID%>"); 
        var form = document.getElementById("<%=panOwner.ClientID %>");
        disableForm(form,chkAutoFill,ddlAutoFill,isDisable);
      }
      
     
    function FillWithOwnerInfo(info)
    {
        if (myValidationErrorPanel) {
            myValidationErrorPanel.clearErrors();
        }
        
        if (info == '')
        {
            return;
        }

        var fieldsJson =eval('(' + info + ')');
        var json =fieldsJson.normalField;
        var templateJson=fieldsJson.templateField;
        var countryCtrl = $('#<% = ddlCountry.ClientID %>');
        var isCountryChanged = false;
        var countrySettingJason = '';

        if (!IsTrue(json.IsLocked))
        {
            SetValueById('<%=txtTitle.ClientID%>',JsonDecode(json.Title));
            SetValueById('<%=txtName.ClientID%>',JsonDecode(json.OwnerName));
            SetValueById('<%=hfRefOwnerId.ClientID%>',json.OwnerNumber);
            SetValueById('<%=hfRefOwnerUID.ClientID%>',json.OwnerUID);
            SetValueById('<%=hfSourceSeq.ClientID%>',json.SourceSeq);
            SetValueById('<%=txtAddress1.ClientID%>',JsonDecode(json.Address1));
            SetValueById('<%=txtAddress2.ClientID%>',JsonDecode(json.Address2));
            SetValueById('<%=txtAddress3.ClientID%>',JsonDecode(json.Address3));
            SetValueById('<%=txtCity.ClientID%>',json.City); 
            SetValueById('<%=txtZip.ClientID%>',json.Zip);
            SetValueById('<%=ddlAppState.ClientID%>',json.State);
            SetValueById('<%=txtPhone.CountryCodeClientID%>',json.PhoneIDD);
            SetValueById('<%=txtPhone.ClientID%>',json.Phone);
            SetValueById('<%=txtFax.CountryCodeClientID%>',json.FaxIDD);
            SetValueById('<%=txtFax.ClientID%>',json.Fax);
            SetValueById('<%=txtEmail.ClientID%>',json.Email);
            
            if (countryCtrl.val() != json.Country) {
                if (countryCtrl.is(':visible')) {
                    countryCtrl.val(json.Country);
                }

                countrySettingJason = '[{"countryClientID":"' + '<% = ddlCountry.ClientID %>' + '" ,"countryCode":"' + json.Country + '", "state":"' + json.State + '", "zip" : "' + json.Zip + '",';
                countrySettingJason += '"phone":[';
                countrySettingJason += '{"name":"txtPhone", "value":"' + json.Phone + '", "iddvalue":"' + json.PhoneIDD + '"},';
                countrySettingJason += '{"name":"txtFax", "value":"' + json.Fax + '", "iddvalue":"' + json.FaxIDD + '"}';
                countrySettingJason += ']}]';
                isCountryChanged = true;
            }

            if(json.APOKeys == ""){
                SetValueById('<%=hfAPOKeys.ClientID %>',""); 
            }
            else{
                SetValueById('<%=hfAPOKeys.ClientID %>',Sys.Serialization.JavaScriptSerializer.serialize(json.APOKeys)); 
            }

            if(typeof(templateJson)!="undefined" &&  typeof(<%=templateEdit.ClientID %>_SetTemplateValue) == "function")
            {
                <%=templateEdit.ClientID %>_SetTemplateValue(templateJson);
            }
        }

        var isSectionValidate = "<%=IsValidate%>"=="True";
        var isDisable = isSectionValidate &&!IsTrue(json.IsLocked);
        if(isDisable)
        {
            disableOwnerForm(isDisable);
            $get('<%=hfDisableFormFlag.ClientID%>').value = "Y";
        }
        
        if (isCountryChanged) {
            $get('<%=hdfPostbackFlag.ClientID%>').value = '1';

            //If auto-fill changed Country field, need send a postback to apply the regional settings.
            __doPostBack('<%= panOwner.UniqueID %>', '<%=ACAConstant.ACA_COUNTRY_AUTOFILL_FLAG %>' + countrySettingJason);
        } else {
            ShowOwnerCondition();
        }
    }
    
    <%=SetTemplateJsFunction %>;

    function ShowOwnerCondition()
    {
        // show condition notice
        var ddl = document.getElementById("<%=ddlAutoFillOwnerInfo.ClientID%>"); 

        if(typeof(GetOwnerCondtionMessage) != "undefined" && ddl)
        {
            GetOwnerCondtionMessage(ddl.value);
        }

        if(typeof(GetOwnerCondition) != "undefined" && ddl)
        {   
            GetOwnerCondition(ddl.value); 
        }
    }
</script>
