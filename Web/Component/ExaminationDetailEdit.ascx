<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ExaminationDetailEdit"
    CodeBehind="ExaminationDetailEdit.ascx.cs" %>
<%@ Register Src="~/Component/GenericTemplateEdit.ascx" TagPrefix="ACA" TagName="GenericTemplateEdit" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="GradingStyle.ascx" TagName="GradingStyle" TagPrefix="ACA" %>

<asp:UpdatePanel ID="detailPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
            <ACA:AccelaTextBoxWithImageButton ID="txtWithImgExaminationName" runat="server" Validate="maxlength"
              IsDBRequired="true"  LabelKey="examination_detail_name"  MaxLength="80" CausesValidation="false"
                ImageClientScript="showExaminationNameList(true);return false;" />
            <ACA:AccelaCalendarText ID="actExaminationDate" runat="server" CssClass="ACA_Medium"
                LabelKey="examination_detail_examination_date" />
            <ACA:GradingStyle ID="txtFinalScore" runat="server" DisplayType="none" LabelKey="examination_detail_final_score" />

            <ACA:AccelaTimeSelection ID="startTime" onblur="verifyExaminationDate();" LabelKey="examination_detail_start_time" AmPmTooltipLabelKey="aca_examination_label_start_time_ampm|tip" runat="server" ></ACA:AccelaTimeSelection>
            <ACA:AccelaTimeSelection ID="endTime" onblur="verifyExaminationDate();" runat="server" LabelKey="examination_detail_end_time" AmPmTooltipLabelKey="aca_examination_label_end_time_ampm|tip"></ACA:AccelaTimeSelection>

            <ACA:AccelaTextBoxWithImageButton ID="txtWithImgProviderName" runat="server" Validate="maxlength"
                LabelKey="examination_detail_provider_name" MaxLength="65"
                CausesValidation="false" ImageClientScript="showExaminationProviderNameList();return false;" />
            <ACA:AccelaTextBox ID="txtProviderNumber" runat="server" LabelKey="examination_detail_provider_number"
                Validate="maxlength" />
            <ACA:AccelaTextBox ID="txtAddress1" runat="server" LabelKey="examination_detail_address1"/>

            <ACA:AccelaTextBox ID="txtRosterID" runat="server" LabelKey="examination_detail_rosterid" />

            <ACA:AccelaTextBox ID="txtAddress2" runat="server" LabelKey="examination_detail_address2"/>
            
            <ACA:AccelaDropDownList ID="ddlExaminationStatus" IsDBRequired="true" OnSelectedIndexChanged="ExaminationStatus_IndexChanged"  LabelKey="examination_detail_status" 
                 ToolTipLabelKey="aca_common_msg_dropdown_changeexamstatus_tip" runat="server"></ACA:AccelaDropDownList>

            <ACA:AccelaTextBox ID="txtAddress3" runat="server" LabelKey="examination_detail_address3"/>                 
            <ACA:AccelaTextBox ID="txtCity" PositionID="SpearFormExaminationCity" runat="server" AutoFillType="City" LabelKey="examination_detail_city" MaxLength="30" />
            <ACA:AccelaStateControl ID="txtState" PositionID="SpearFormExaminationState" runat="server"
                AutoFillType="State" LabelKey="examination_detail_state" />
            <ACA:AccelaZipText ID="txtZip" runat="server" Validate="zip" MaxLength="10" CssClass="ACA_Medium"
                LabelKey="examination_detail_zip" />
            <ACA:AccelaPhoneText ID="txtPhone1" runat="server" MaxLength="40" LabelKey="examination_detail_phone_number1"/>
            <ACA:AccelaPhoneText ID="txtPhone2" runat="server" MaxLength="40" LabelKey="examination_detail_phone_number2"/>
            <ACA:AccelaPhoneText ID="txtFax" runat="server" MaxLength="40" LabelKey="examination_detail_fax"/>
            <ACA:AccelaEmailText ID="txtEmail" LabelKey="examination_detail_email" AutoPostBack="false" MaxLength="50" Validate="email"  runat="server" SetFocusOnError="true" />
            <ACA:AccelaTextBox ID="txtRequired" runat="server" Enabled="false" CssClass="ACA_NShot"
                LabelKey="examination_detail_required" />
            <ACA:AccelaTextBox ID="txtApproved" runat="server" Enabled="false" CssClass="ACA_NShot" LabelKey="aca_examinationdetail_label_approved"></ACA:AccelaTextBox>
            <ACA:AccelaTextBox ID="txtComments"  runat="server" LabelKey="examination_detail_comments"
                TextMode="MultiLine" Rows="5" MaxLength="2000" Validate="maxlength" />
            <ACA:AccelaCountryDropDownList ID="ddlCountryCode" runat="server" LabelKey="aca_examination_detail_label_country">
            </ACA:AccelaCountryDropDownList>
            <ACA:GenericTemplateEdit runat="server" ID="genericTemplate" />
        </ACA:AccelaFormDesignerPlaceHolder>
        <div class="ACA_Row ACA_LiLeft Footer">
            <ul>
                <li>
                    <ACA:AccelaButton ID="btnSave" runat="server"
                            CausesValidation="true" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                            OnClientClick="ExaminationClientClick();return verifyExaminationDate();" OnClick="Examination_Saved" />
                </li>
                <li>
                    <ACA:AccelaLinkButton ID="btnCancel" CausesValidation="False" CssClass="ACA_LinkButton"
                        OnClientClick="SetNotAsk();" OnClick="Examination_Clear" runat="server" />
                </li>
            </ul>
        </div>
        <asp:HiddenField ID="hdnPassingScore" runat="server" />
        <asp:HiddenField ID="hdnExamNbr" runat="server" />
        <asp:HiddenField ID="hdnOrginalExamName" runat="server" />
        <asp:HiddenField ID="hdnPostBackValidate" Value="0" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<script language="javascript" type="text/javascript">
    function ClosePopup() {       
        parent.closeExaminationPopupDialog();
    }
    
    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%=btnCancel.ClientID %>");
        myValidationErrorPanel.registerIDs4Recheck("<%=btnSave.ClientID %>");
    }
  
    AddValidationSectionID('<%=ClientID %>');

    //Validation Section Fields
    function ExaminationClientClick() {
        if (typeof (SetNotAsk) != 'undefined') {
            SetNotAsk();
        }

        SetCurrentValidationSectionID('<%=ClientID %>');
    }

    var agencyCode = '<%=CapAgencyCode %>';
    var moduleName = '<%=ModuleName %>';

    ///
    /// Achieves the examination name that users enter
    ///
    function getExaminationName() {
        return GetValueById('<%= txtWithImgExaminationName.ClientID %>');
    }

    ///
    /// Achieves the provider name that users enter
    ///
    function getProvierName() {
        return GetValueById('<%= txtWithImgProviderName.ClientID %>');
    }

    //****************Examination PopUp Selected *****************//
    //show Examination Name List
    function showExaminationNameList(searchBehind) {
        if ($('#<% = txtWithImgExaminationName.ClientID%>').is('[readonly]')) {
            return;
        }

        if (searchBehind) {
            ShowLoading();

            var examinationName = getExaminationName();
            var providerName = getProvierName();
            Accela.ACA.Web.WebService.EducationService.GetRefExaminationNames(examinationName, providerName, agencyCode, moduleName, CallBackGetExaminations);
        } else {
            var info = '<%=RefExaminationNameValueString%>';
            showExaminations(info, true);
        }
        
        if (searchBehind) {
            stopBublle();
        }
    }

    function CallBackGetExaminations(info) {
        showExaminations(info, false);

        HideLoading();
    }

    //Call back of show Examination Name List
    function showExaminations(info, searchWithOutCapType) {
        var examinationNameControl = document.getElementById('<%= txtWithImgExaminationName.ClientID %>');
        // achieve examination name
        var examinationName = getExaminationName();                
        var noRecordMsg = '<% =LabelUtil.GetTextByKey("acc_reg_message_noRecord", string.Empty).Replace("'","\\'")%>';
        // This param is define in GeneralNameList.js, it's use for focus current link after pop up window closed.
        currentObj = document.getElementById("<%=txtWithImgExaminationName.ImageClientID %>");
        selectGeneralNameList(examinationName, info, noRecordMsg,
            '<%=LabelUtil.GetTextByKey("examination_detail_name", string.Empty).Replace("'","\\'") %>', "ExaminationNameSelected", 
            getElementsLeftWithControlWidth(examinationNameControl), getElementTop(examinationNameControl) + examinationNameControl.offsetHeight, !searchWithOutCapType, "changeExamName");
    }
    //Select a Examination
    function ExaminationNameSelected(value) {
        var record = value.split('\b');
        var examinationName = record[0];
        var refExamNumber = record[1];

        SetValueById('<%=txtWithImgExaminationName.ClientID%>', examinationName);
        $get('<%= hdnExamNbr.ClientID%>').value = refExamNumber;
        UpdateGradeStyleForSelectChange(CallBackUpdateGradeStyleForExamSelect);
    }

    //Update exam name and grade style
    function UpdateGradeStyleForSelectChange(callBackFunction) {
        var providerName =GetValueById('<%=txtWithImgProviderName.ClientID %>');
        var examName = GetValueById('<%=txtWithImgExaminationName.ClientID %>');
        var examNbr = $get('<%=hdnExamNbr.ClientID %>').value;

        if(providerName != "" && examName != "")
        {
            //get exam and grade style information by call web service.
            Accela.ACA.Web.WebService.EducationService.GetExamUpdateGradeStyleDate(examNbr, examName, providerName, moduleName, agencyCode, callBackFunction);
        }
        else if(examNbr != "" && examName != "")
        {
            GetRefExaminationDataByExamNbr();
        }
    }
    
    //Get ref examination data info according to examination number and name
    function GetRefExaminationDataByExamNbr()
    {
        var examNbr = $get('<%=hdnExamNbr.ClientID %>').value;
        var examName = GetValueById('<%=txtWithImgExaminationName.ClientID %>');

        if(examNbr != "" && examName != "")
        {
            Accela.ACA.Web.WebService.EducationService.GetRefExaminationData(examNbr, examName, moduleName, agencyCode, CallBackFillExaminationInfo);
        }
    }

    //Call back of select examination name
    function CallBackUpdateGradeStyleForExamSelect(info){
        CallBackFillExaminationInfo(info);
    }


    //Call back of select provider name
    function CallBackUpdateGradeStyleForProviderSelect(info){
        CallBackFillExaminationInfo(info);
    }

    //Fill Examination Information
    function CallBackFillExaminationInfo(info) {
        if (typeof (myValidationErrorPanel) != "undefined"){
            myValidationErrorPanel.clearErrors();
        }
        
        var examName = getExaminationName();
        var refOrginalName = $('#<%=hdnOrginalExamName.ClientID%>').val();
        
        
        if (info) {
            var json = eval('(' + info + ')');
            setGradingStyle('<%= txtFinalScore.ClientID%>', json.ExamGradingStyle);
            SetValueById('<%=txtRequired.ClientID%>', json.Required);
            $('#<%=hdnExamNbr.ClientID%>').val(json.RefExamNbr);
            var isPassfail = json.ExamGradingStyle.toLowerCase() == '<%=GradingStyle.Passfail.ToString().ToLower() %>';

            if(!isNullOrEmpty(json.PassingScore) && !isPassfail){
                $('#<%= hdnPassingScore.ClientID%>').val(json.PassingScore);
            }
        }

        if (examName != refOrginalName) {
            $('#<%=hdnOrginalExamName.ClientID%>').val(examName);
            if (!$.global.isAdmin) {
                var p = new ProcessLoading();
                p.showLoading();
            }
            __doPostBack('<%=detailPanel.UniqueID + EXAMINATION_POSTBACK_STRING %>', $('#<%=hdnExamNbr.ClientID%>').val());
        }
    }
    //*************** End Examination PopUp Selected*************//

    //******************Provider Name Selected ******************//
    function showExaminationProviderNameList() {
        ShowLoading();

        var examinationName = getExaminationName();
        var providerName = getProvierName();
        Accela.ACA.Web.WebService.EducationService.GetExaminationProviders(examinationName, providerName, agencyCode, CallBackGetExaminationProviders);
    }

    //Call back of show Examination Name List
    function CallBackGetExaminationProviders(info) {
        var providerNameControl = document.getElementById('<%= txtWithImgProviderName.ClientID %>');
        var noRecordMsg = '<% =LabelUtil.GetTextByKey("acc_reg_message_noRecord", string.Empty).Replace("'","\\'")%>';
        // This param is define in GeneralNameList.js, it's use for focus current link after pop up window closed.
        currentObj = document.getElementById("<%=txtWithImgProviderName.ImageClientID %>");
        selectGeneralNameList(getProvierName(), info, noRecordMsg, '<%=LabelUtil.GetTextByKey("examination_detail_provider_name", string.Empty).Replace("'","\\'") %>', "ExaminationProviderNameSelected", getElementsLeftWithControlWidth(providerNameControl), getElementTop(providerNameControl) + providerNameControl.offsetHeight,true);

        HideLoading();
    }
    //fill provider name.
    function ExaminationProviderNameSelected(value) {
        //get provider information by call web service.
        Accela.ACA.Web.WebService.EducationService.GetExaminationProviderData(value, agencyCode, CallBackFillExaminationProviderInfo);
    }

    // fill provider information.
    function CallBackFillExaminationProviderInfo(info) {
        if (info == '') return;
        
        if (typeof (myValidationErrorPanel) != "undefined"){
            myValidationErrorPanel.clearErrors();
        }

        var json = eval('(' + info + ')');

        SetValueById('<%=txtWithImgProviderName.ClientID%>', JsonDecode(json.ProviderName));
        SetValueById('<%=txtProviderNumber.ClientID%>', json.ProviderNumber);
        SetValueById('<%=txtAddress1.ClientID%>', JsonDecode(json.Address1));
        SetValueById('<%=txtAddress2.ClientID%>', JsonDecode(json.Address2));
        SetValueById('<%=txtAddress3.ClientID%>', JsonDecode(json.Address3));
        SetValueById('<%=txtCity.ClientID%>', json.City);
       
        SetValueById('<%=txtState.ClientID%>', json.State);
        // locate current item at the first item if selectedIndex is equal to -1
        var stateDropdownlist = $get('<%=txtState.ClientID%>');
        if (stateDropdownlist &&
            stateDropdownlist.selectedIndex &&
            stateDropdownlist.selectedIndex == -1) {
            stateDropdownlist.selectedIndex = 0;
        }
                
        SetValueById('<%=txtZip.ClientID%>', json.Zip);
        SetValueById('<%=txtPhone1.CountryCodeClientID%>', json.Phone1CountryCode);
        SetValueById('<%=txtPhone1.ClientID%>', json.Phone1);
        SetValueById('<%=txtPhone2.CountryCodeClientID%>', json.Phone2CountryCode);
        SetValueById('<%=txtPhone2.ClientID%>', json.Phone2);
        SetValueById('<%=txtFax.CountryCodeClientID%>', json.FaxCountryCode);
        SetValueById('<%=txtFax.ClientID%>', json.Fax);
        SetValueById('<%=txtEmail.ClientID%>', json.Email);

        var countryControl = $('#<%=ddlCountryCode.ClientID%>');
        var examName = getExaminationName();
        var providerName = getProvierName();
        var examNbr = $('#<%=hdnExamNbr.ClientID%>').val();
        
        if (json.Country != countryControl.val()) {
            countryControl.val(json.Country);

            var countrySettingJason = '[{"countryClientID":"' + '<% = ddlCountryCode.ClientID %>' + '",';
            countrySettingJason += '"countryCode":"' + json.Country + '", "state":"' + json.State + '", "zip" : "' + json.Zip + '",';
            countrySettingJason += '"phone":[';
            countrySettingJason += '{"name":"txtPhone1", "value":"' + json.Phone1 + '", "iddvalue":"' + json.Phone1CountryCode + '"},';
            countrySettingJason += '{"name":"txtPhone2", "value":"' + json.Phone2 + '", "iddvalue":"' + json.Phone2CountryCode + '"},';
            countrySettingJason += '{"name":"txtFax", "value":"' + json.Fax + '", "iddvalue":"' + json.FaxCountryCode + '"}';
            countrySettingJason += ']}]';

            $('#<%=hdnPostBackValidate.ClientID%>').val('1');
            //if country  change, update the examination grade style in behind code.
            if (!$.global.isAdmin) {
                var p = new ProcessLoading();
                p.showLoading();
            }
            __doPostBack('<%=detailPanel.UniqueID + POST_BACK_BY_COUNTRY%>', '<% = ACAConstant.ACA_COUNTRY_AUTOFILL_FLAG %>' + countrySettingJason);
        } else if(!isNullOrEmpty(examNbr) && !isNullOrEmpty(examName)) {
            Accela.ACA.Web.WebService.EducationService.GetExamUpdateGradeStyleDate(examNbr, examName, providerName, moduleName, agencyCode, CallBackUpdateGradeStyleForProviderSelect);
        }
    }
    /****************End Provider Name Selected*****************/


    $(document).click(function () {
        if(typeof(clickInPanel) != "undefined" && !clickInPanel){
            changeExamName();
        }
    });

    //Special logic for examination Start Time/End Time/Examination Date.
    function verifyExaminationDate()
    {
        if (!$.global.isAdmin) {
            var startTime = $get('<%=startTime.ClientID %>');
            var endTime = $get('<%=endTime.ClientID %>');
            var examDate = $get('<%=actExaminationDate.ClientID %>'); 

            var startTimeVal = "";
            var endTimeVal = "";
            var examDateVal = "";

            if(startTime != null && typeof(startTime) != 'undefined')
            {
                var startTimeMask = $find(startTime.id + '_ext');
                startTimeVal = startTimeMask && startTimeMask._getClearMask ? startTimeMask._getClearMask(GetValue(startTime)) : GetValue(startTime);
            }

            if(endTime != null && typeof(endTime) != 'undefined')
            {
                var endTimeMask = $find(endTime.id + '_ext');
                endTimeVal = endTimeMask && endTimeMask._getClearMask ? endTimeMask._getClearMask(GetValue(endTime)) : GetValue(endTime);
            }

            if(examDate != null && typeof(examDate) != 'undefined')
            {
                examDateVal = GetValue(examDate);
            }

            if(startTime && startTimeVal != '')
            {
                doErrorCallbackFun('', startTime.id, 0);
            }
        
            if(endTime && endTimeVal != '')
            {
                doErrorCallbackFun('', endTime.id, 0);
            }

            if(examDate && examDateVal != '')
            {
                doErrorCallbackFun('', examDate.id, 0);
            }

            if(startTime && startTimeVal != '')
            {
                if(examDate && examDateVal == '')
                {
                    doErrorCallbackFun('', examDate.id, 2);
                    return false;
                }

                if(endTime && endTimeVal == '')
                {
                    doErrorCallbackFun('', endTime.id, 2);
                    return false;
                }
            }

            if(endTime && endTimeVal != '')
            {
                if(examDate && examDateVal == '')
                {
                    doErrorCallbackFun('', examDate.id, 2);
                    return false;
                }

                if(startTime && startTimeVal == '')
                {
                    doErrorCallbackFun('', startTime.id, 2);
                    return false;
                }
            }

            if(startTime != null && typeof(startTime) != 'undefined')
            {
                doErrorCallbackFun('', startTime.id, 0);
            }

            if(endTime != null && typeof(endTime) != 'undefined')
            {
                doErrorCallbackFun('', endTime.id, 0);
            }

            if(examDate != null && typeof(examDate) != 'undefined')
            {
                doErrorCallbackFun('', examDate.id, 0);
            }

            return true;
        }
    }
    
    function changeExamName() {
        var refOrginalName = $('#<%=hdnOrginalExamName.ClientID%>').val();
        var examName = getExaminationName();
        
        if (examName != refOrginalName) {
            if (typeof (myValidationErrorPanel) != "undefined"){
                myValidationErrorPanel.clearErrors();
            }
            
            $('#<%=hdnOrginalExamName.ClientID%>').val(examName);
            if (!$.global.isAdmin) {
                var p = new ProcessLoading();
                p.showLoading();
            }
            __doPostBack('<%=detailPanel.UniqueID + EXAMINATION_POSTBACK_STRING%>', '');
        }
    }
    
    function disableStatus() {
        var ctrlId = "<%=ddlExaminationStatus.ClientID %>";
        var ctrl = $("#" + ctrlId);

        if ($.exists(ctrl)) {
            ctrl.parent().addClass("ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
            DisableButton(ctrlId, true);
        }
    }

    var prmExam = Sys.WebForms.PageRequestManager.getInstance();
    prmExam.add_pageLoaded(<%=ClientID %>_PageLoaded);
    function <%=ClientID %>_PageLoaded(sender, args) {
        $('#<% = txtWithImgExaminationName.ClientID%>').autosearch("showExaminationNameList");
    }
    
    prmExam.add_endRequest(<%=ClientID %>_EndRequest);
    function <%=ClientID %>_EndRequest(sender, args) {
        var needValidate = $('#<%=hdnPostBackValidate.ClientID%>');
        if (needValidate.val() == '1') {
            needValidate.val('0');
        }
        
        if(<%=IsFromAccountContactEdit.ToString().ToLower() %>) {
            disableStatus();
            $("#<%=txtRequired.ClientID %>_element_group").hide();
        }
    }
    
    //Add page load function to update the tooltip for the Examination Name field.
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_pageLoaded(PageLoaded);

    function PageLoaded(sender, args) {
        //Education Edit form will be refreshed when Examination Name has been changed, so we will add a title to describe what will happen before the change.
        if (!$.global.isAdmin && IsTrue('<%=txtWithImgExaminationName.Visible %>')) {
            UpdateTextboxToolTip('<%=txtWithImgExaminationName.ClientID %>', '<%=GetTextByKey("examination_detail_name") %>');
        }
    }

</script>