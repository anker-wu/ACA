<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.ContinuingEducationDetailEdit" Codebehind="ContinuingEducationDetailEdit.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="GradingStyle.ascx" TagName="GradingStyle" TagPrefix="ACA" %>
<%@ Register Src="~/Component/GenericTemplateEdit.ascx" TagPrefix="ACA" TagName="GenericTemplateEdit" %>

<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
      <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server"> 
                        <ACA:AccelaTextBoxWithImageButton ID="txtContEducationName" 
                            runat="server" LabelKey="continuing_education_detail_education_name"
                            MaxLength="80" CausesValidation="false" IsDBRequired="true" ImageClientScript="showContEducationNameList(true);return false;">
                        </ACA:AccelaTextBoxWithImageButton>
                   
                        <ACA:AccelaTextBox ID="txtClass" runat="server" LabelKey="continuing_educatin_detail_class"
                            MaxLength="80">
                        </ACA:AccelaTextBox>
                   
                       <ACA:AccelaCalendarText ID="txtCompletedDate"  runat="server"
                          LabelKey="continuing_education_detail_dateofclass"></ACA:AccelaCalendarText>
                   
                       <ACA:AccelaNumberText ID="txtClassHours"  runat="server" IsNeedDot="true" MaxLength="13" DecimalDigitsLength="2"
                            LabelKey="continuing_education_detail_completedhours">
                        </ACA:AccelaNumberText>
                    
                        <ACA:GradingStyle ID="txtFinalScore" runat="server" DisplayType="none" LabelKey="continuing_education_detail_final_score"  />   
                   
                        <ACA:AccelaTextBoxWithImageButton ID="txtProviderName"  runat="server"
                            LabelKey="continuing_education_detail_provider_name" MaxLength="65"
                            CausesValidation="false" ImageClientScript="showContEducationProviderNameList();return false;">
                        </ACA:AccelaTextBoxWithImageButton>
                  
                        <ACA:AccelaTextBox ID="txtProviderNumber"  runat="server" LabelKey="continuing_education_detail_provider_number"
                            MaxLength="65"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtAddress1"  runat="server" LabelKey="continuing_education_detail_address1"
                            MaxLength="200">
                        </ACA:AccelaTextBox>                    
                        <ACA:AccelaTextBox ID="txtAddress2"  runat="server" LabelKey="continuing_education_detail_address2"
                            MaxLength="200">
                        </ACA:AccelaTextBox>                    
                        <ACA:AccelaTextBox ID="txtAddress3"  runat="server" LabelKey="continuing_education_detail_address3"
                            MaxLength="200">
                        </ACA:AccelaTextBox>                   
                        <ACA:AccelaTextBox ID="txtCity"  PositionID="SpearFormContEducationCity"
                            AutoFillType="City" runat="server" LabelKey="continuing_education_detail_city"
                            MaxLength="30">
                        </ACA:AccelaTextBox>                    
                        <ACA:AccelaStateControl ID="txtState" PositionID="SpearFormContEducationState" AutoFillType="State"
                            runat="server" LabelKey="continuing_education_detail_state" />
                   
                        <ACA:AccelaZipText ID="txtZip" runat="server" Validate="zip" MaxLength="10" LabelKey="continuing_education_detail_zip">
                        </ACA:AccelaZipText>
                    
                        <ACA:AccelaPhoneText ID="txtPhone1" runat="server" MaxLength="40" LabelKey="continuing_education_detail_phone1">
                        </ACA:AccelaPhoneText>
                  
                        <ACA:AccelaPhoneText ID="txtPhone2" runat="server" MaxLength="40" LabelKey="continuing_education_detail_phone2">
                        </ACA:AccelaPhoneText>
                   
                        <ACA:AccelaPhoneText ID="txtFax" runat="server" MaxLength="40" LabelKey="continuing_education_detail_fax">
                        </ACA:AccelaPhoneText>
                    
                        <ACA:AccelaEmailText ID="txtEmail" LabelKey="continuing_education_detail_email" AutoPostBack="false"
                            MaxLength="70" Validate="email"  runat="server" SetFocusOnError="true">
                        </ACA:AccelaEmailText>
                        <ACA:AccelaTextBox ID="txtRequired" runat="server" Enabled="false"  LabelKey="continuing_education_detail_required"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtApproved" runat="server" Enabled="false" LabelKey="aca_conteducationdetail_label_approved"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtComments"  runat="server" LabelKey="continuing_education_detail_comments"
                            TextMode="MultiLine" Height="60px" MaxLength="2000" Validate="maxlength">
                        </ACA:AccelaTextBox>
                        <ACA:AccelaCountryDropDownList ID="ddlCountryCode" runat="server" LabelKey="aca_continuing_education_detail_label_country">
                        </ACA:AccelaCountryDropDownList>
          <ACA:GenericTemplateEdit runat="server" ID="genericTemplate" />
        </ACA:AccelaFormDesignerPlaceHolder>
        <asp:HiddenField ID="hdnContEducationSeqNumber" runat="server" />
        <asp:HiddenField ID="hdnRefContEducationSeqNumber" runat="server" />
        <asp:HiddenField ID="hdnRequired" runat="server" />
        <asp:HiddenField ID="hdnPassingScore" runat="server" />
        <asp:HiddenField ID="hdnOrginalEduName" runat="server" />
        <asp:HiddenField ID="hdnPostBackValidate" Value="0" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<script language="javascript" type="text/javascript">
    AddValidationSectionID('<%=ClientID %>');

    function getContinueEducationName() {
        return GetValueById('<%=txtContEducationName.ClientID %>');
    }

    function getContEducationProviderName() {
        return GetValueById('<%=txtProviderName.ClientID %>');
    }
    
    //****************Continuing Education PopUp Selected *****************//
    function showContEducationNameList(searchBehind)
    {
        if ($('#<% = txtContEducationName.ClientID%>').is('[readonly]')) {
            return;
        }
        
        if(searchBehind) {
            ShowLoading();

            var continueEducationName = getContinueEducationName();
            var providerName = getContEducationProviderName();
            Accela.ACA.Web.WebService.EducationService.GetRefContEducationNames(continueEducationName, providerName, '<%=CapAgencyCode %>', '<%=ModuleName %>', CallBackGetContEducations);
        } else {
            var info = '<%=RefContinuingEducationNameValueString%>';
            showContEducations(info, true);
        }
        
        if (searchBehind) {
            stopBublle();
        }
    }
    
    function CallBackGetContEducations(info) {
        showContEducations(info, false);

        HideLoading();
    }
    
    function changeContEduName() {
        var refOrginalName = $('#<%=hdnOrginalEduName.ClientID%>').val();
        var examName = getContinueEducationName();
        
        if (refOrginalName != examName) {
            if (typeof (myValidationErrorPanel) != "undefined"){
                myValidationErrorPanel.clearErrors();
            }

            $('#<%=hdnOrginalEduName.ClientID%>').val(examName);
            
            if (!$.global.isAdmin) {
                var p = new ProcessLoading();
                p.showLoading();
            }
            
            __doPostBack('<%=updatePanel.UniqueID + POST_BACK_BY_CHANGE_CONTEDU%>', '');
        }
    }
    
    var prmContEdu = Sys.WebForms.PageRequestManager.getInstance();
    prmContEdu.add_pageLoaded(<%=ClientID %>_PageLoaded);
    function <%=ClientID %>_PageLoaded(sender, args) {
        $('#<% = txtContEducationName.ClientID%>').autosearch("showContEducationNameList");
    }
    
    prmContEdu.add_endRequest(<%=ClientID %>_EndRequest);
    function <%=ClientID %>_EndRequest(sender, args) {
        var needValidate = $('#<%=hdnPostBackValidate.ClientID%>');
        
        if (needValidate.val() == '1') {
            needValidate.val('0');
        }
        
        if(<%=IsFromRefContact.ToString().ToLower() %>) {
            $("#<%=txtRequired.ClientID %>_element_group").hide();
        }
    }
    
    // Get all Coninuing Education names.
    function showContEducations(info, searchWithOutCapType)
    {
        var contEducationNameControl = document.getElementById('<%=txtContEducationName.ClientID %>');
        var noRecordMsg = '<% =LabelUtil.GetTextByKey("acc_reg_message_noRecord", string.Empty).Replace("'","\\'")%>';
        // This param is define in GeneralNameList.js, it's use for focus current link after pop up window closed. 
        currentObj = document.getElementById("<%=txtContEducationName.ImageClientID %>");
        selectGeneralNameList(GetValue(contEducationNameControl), info, noRecordMsg,
            '<%=LabelUtil.GetTextByKey("continuing_education_searchform_conteducation_name", string.Empty).Replace("'","\\'") %>', "contEducationNameSelected",
            getElementsLeftWithControlWidth(contEducationNameControl), getElementTop(contEducationNameControl) + contEducationNameControl.offsetHeight, !searchWithOutCapType);
    }    
    // Continuing Education Name selected.
    function contEducationNameSelected(value)
    {
        var record = value.split('\b');
        var contEducationName = record[0];
        var refContEducationNumber = record[1];
        var moduleName = '<%=ModuleName %>';
        Accela.ACA.Web.WebService.EducationService.GetRefContEducationData(refContEducationNumber, contEducationName, moduleName, '<%=CapAgencyCode %>', callBackFillContEducationInfo);
        stopBublle();
    }
    
    // Fill Continuing Education Information.
    function callBackFillContEducationInfo(info)
    {    
        if(info == '')return;
        
        if (typeof (myValidationErrorPanel) != "undefined"){
            myValidationErrorPanel.clearErrors();
        }

        var json = eval('(' + info + ')');
        SetValueById('<%=txtContEducationName.ClientID%>', JsonDecode(json.ConEducationName));
        $('#<%=hdnOrginalEduName.ClientID%>').val(JsonDecode(json.ConEducationName));
        SetValueById('<%=txtRequired.ClientID%>',json.Required);
        setGradingStyle('<%= txtFinalScore.ClientID%>', json.ContEducationGradingStyle);
        $('#<%=hdnPassingScore.ClientID%>').val(json.PassingScore);
        $('#<%=hdnRefContEducationSeqNumber.ClientID%>').val(json.ConEducationNbr);
        
        if (!$.global.isAdmin) {
            var p = new ProcessLoading();
            p.showLoading();
        }
        
        __doPostBack('<%=updatePanel.UniqueID + POST_BACK_BY_CHANGE_CONTEDU%>', json.ConEducationNbr);
    }
    //*************** End Continuing Education PopUp Selected*************//
    
    
    //****************Provider PopUp Selected *****************//
    function showContEducationProviderNameList() {
        ShowLoading();

        Accela.ACA.Web.WebService.EducationService.GetContEducationProviders(getContinueEducationName(), getContEducationProviderName(), '<%=CapAgencyCode %>',callBackGetContEducationProviders); 
    }
    
    // Get Continuing Education Providers.
    function callBackGetContEducationProviders(info)
    {
        var providerNameControl = document.getElementById('<%=txtProviderName.ClientID %>');
        var noRecordMsg = '<% =LabelUtil.GetTextByKey("acc_reg_message_noRecord", string.Empty).Replace("'","\\'")%>';
        // This param is define in GeneralNameList.js, it's use for focus current link after pop up window closed.
        currentObj = document.getElementById("<%=txtProviderName.ImageClientID %>");
        selectGeneralNameList(GetValue(providerNameControl), info, noRecordMsg, '<%=LabelUtil.GetTextByKey("education_searchform_providername_label", string.Empty).Replace("'","\\'") %>', "contEducationProviderNameSelected", getElementsLeftWithControlWidth(providerNameControl), getElementTop(providerNameControl) + providerNameControl.offsetHeight,true);

        HideLoading();
    }
    
    // Get provider information by call web service.
    function contEducationProviderNameSelected(providerName)
    {
        Accela.ACA.Web.WebService.EducationService.GetContEducationProviderData(providerName,'<%=CapAgencyCode %>', CallBackContEducaionProviderInfo);
    }
    
    // Fill provider information.
    function CallBackContEducaionProviderInfo(info)
    {
        if(info == '')return;
        
        if (typeof (myValidationErrorPanel) != "undefined"){
            myValidationErrorPanel.clearErrors();
        }
        
        var json = eval('(' + info + ')');
        SetValueById('<%=txtProviderName.ClientID%>',JsonDecode(json.ProviderName));
        SetValueById('<%=txtProviderNumber.ClientID%>',json.ProviderNumber);
        SetValueById('<%=txtAddress1.ClientID%>',JsonDecode(json.Address1));
        SetValueById('<%=txtAddress2.ClientID%>',JsonDecode(json.Address2));
        SetValueById('<%=txtAddress3.ClientID%>',JsonDecode(json.Address3));
        SetValueById('<%=txtCity.ClientID%>',json.City);
        SetValueById('<%=txtState.ClientID%>',json.State);
        SetValueById('<%=txtZip.ClientID%>',json.Zip);
        
        SetValueById('<%=txtPhone1.CountryCodeClientID%>',json.Phone1CountryCode);
        SetValueById('<%=txtPhone1.ClientID%>',json.Phone1);
        SetValueById('<%=txtPhone2.CountryCodeClientID%>',json.Phone2CountryCode); 
        SetValueById('<%=txtPhone2.ClientID%>',json.Phone2);  
        SetValueById('<%=txtFax.CountryCodeClientID%>',json.FaxCountryCode); 
        SetValueById('<%=txtFax.ClientID%>',json.Fax);
        SetValueById('<%=txtEmail.ClientID%>', json.Email);
        
        var countryControl = $('#<%=ddlCountryCode.ClientID%>');

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
            
            if (!$.global.isAdmin) {
                var p = new ProcessLoading();
                p.showLoading();
            }
            
            __doPostBack('<%=updatePanel.UniqueID + POST_BACK_BY_COUNTRY%>', '<% = ACAConstant.ACA_COUNTRY_AUTOFILL_FLAG %>' + countrySettingJason);
        }
    }
    
    //****************End Provider PopUp Selected *****************//
    
    $(document).click(function () {
        if(typeof(clickInPanel) != "undefined" && !clickInPanel){
            changeContEduName();
        }
    });
    
    //Add page load function to update the tooltip for the Continuing Education Name field.
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_pageLoaded(PageLoaded);

    function PageLoaded(sender, args) {
        //Education Edit form will be refreshed when Continuing Education Name has been changed, so we will add a title to describe what will happen before the change.
        if (!$.global.isAdmin && IsTrue('<%=txtContEducationName.Visible %>')) {
            UpdateTextboxToolTip('<%=txtContEducationName.ClientID %>', '<%=GetTextByKey("continuing_education_detail_education_name") %>');
        }
    }

</script>
