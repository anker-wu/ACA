<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.EducationDetailEdit" Codebehind="EducationDetailEdit.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="~/Component/GenericTemplateEdit.ascx" TagPrefix="ACA" TagName="GenericTemplateEdit" %>

<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
         <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server"> 
                        <ACA:AccelaTextBoxWithImageButton ID="txtWithImgMajorName" Validate="maxlength" 
                         runat="server" LabelKey="education_detail_major_discipline" MaxLength="80" IsDBRequired="true" CausesValidation="false" ImageClientScript="displayEducationNamesForm(true);return false;">
                        </ACA:AccelaTextBoxWithImageButton >
                    
                        <ACA:AccelaDropDownList ID="ddlDegree" LabelKey="education_detail_degree" runat="server">
                        </ACA:AccelaDropDownList>
                   
                        <ACA:AccelaTextBox ID="txtAttended" runat="server" LabelKey="education_detail_attended"
                            MaxLength="60">
                        </ACA:AccelaTextBox>
                    
                        <ACA:AccelaTextBox ID="txtGraduated" runat="server" LabelKey="education_detail_graduateded"
                            MaxLength="60">
                        </ACA:AccelaTextBox>
                    
                        <ACA:AccelaTextBoxWithImageButton ID="txtWithImgProviderName" Validate="maxlength" 
                         runat="server" LabelKey="education_detail_provider_name" MaxLength="65" CausesValidation="false" ImageClientScript="displayProviderNamesForm();return false;">
                        </ACA:AccelaTextBoxWithImageButton>
                   
                        <ACA:AccelaTextBox ID="txtProviderNumber" Validate="maxlength" 
                            runat="server" LabelKey="education_detail_provider_number" MaxLength="65"></ACA:AccelaTextBox>
                    
                        <ACA:AccelaTextBox ID="txtAddress1"  runat="server" LabelKey="education_detail_address1"
                            MaxLength="200">
                        </ACA:AccelaTextBox>
                    
                        <ACA:AccelaTextBox ID="txtAddress2"  runat="server" LabelKey="education_detail_address2"
                            MaxLength="200">
                        </ACA:AccelaTextBox>
                   
                        <ACA:AccelaTextBox ID="txtAddress3"  runat="server" LabelKey="education_detail_address3"
                            MaxLength="200">
                        </ACA:AccelaTextBox>
                   
                        <ACA:AccelaTextBox ID="txtCity" PositionID="SpearFormEducationCity"  AutoFillType="City" runat="server" LabelKey="education_detail_city"
                            MaxLength="30">
                        </ACA:AccelaTextBox>
                   
                        <ACA:AccelaStateControl ID="txtState" PositionID="SpearFormEducationState" AutoFillType="State" runat="server" LabelKey="education_detail_state" />
                    
                        <ACA:AccelaZipText ID="txtZip" runat="server" Validate="zip" MaxLength="10"  LabelKey="education_detail_zip">
                        </ACA:AccelaZipText>
                   
                        <ACA:AccelaPhoneText ID="txtPhone1" runat="server" MaxLength="40" LabelKey="education_detail_phone1">
                        </ACA:AccelaPhoneText>
                  
                        <ACA:AccelaPhoneText ID="txtPhone2" runat="server" MaxLength="40" LabelKey="education_detail_phone2">
                        </ACA:AccelaPhoneText>
                  
                        <ACA:AccelaPhoneText ID="txtFax" runat="server" MaxLength="40" LabelKey="education_detail_fax">
                        </ACA:AccelaPhoneText>
                    
                        <ACA:AccelaEmailText ID="txtEmail" LabelKey="education_detail_email" AutoPostBack="false"
                            MaxLength="50" Validate="email"  runat="server" SetFocusOnError="true">                  
                        </ACA:AccelaEmailText>                    
                        <ACA:AccelaTextBox ID="txtRequired" runat="server" Enabled="false"  LabelKey="education_detail_required"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtApproved" runat="server" Enabled="false"  LabelKey="aca_educationdetail_label_approved"></ACA:AccelaTextBox>
                   
                        <ACA:AccelaTextBox ID="txtComments"  runat="server" LabelKey="education_detail_comments"
                            TextMode="MultiLine" Height="60px" MaxLength="2000" Validate="maxlength">
                        </ACA:AccelaTextBox>
                        <ACA:AccelaCountryDropDownList ID="ddlCountryCode" runat="server" LabelKey="aca_education_detail_label_country">
                        </ACA:AccelaCountryDropDownList>
             <ACA:GenericTemplateEdit runat="server" ID="genericTemplate" />
        </ACA:AccelaFormDesignerPlaceHolder>
        <asp:HiddenField ID="hdnEducationSeqNumber" runat="server" />
        <asp:HiddenField ID="hdnRefEducationSeq" runat="server" />
        <asp:HiddenField ID="hdnOrginalEduName" runat="server" />
        <asp:HiddenField ID="hdnPostBackValidate" Value="0" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<script language="javascript" type="text/javascript">
    AddValidationSectionID('<%=ClientID %>');

    function getMajorName() {
        return GetValueById('<%= txtWithImgMajorName.ClientID %>');
    }

    function getEducationProviderName() {
        return GetValueById('<%= txtWithImgProviderName.ClientID %>');
    }
    
    //****************Begin Provider PopUp Selected *****************//
    function displayProviderNamesForm()
    {
        ShowLoading();

        Accela.ACA.Web.WebService.EducationService.GetEducationProviders(getMajorName(), getEducationProviderName(), '<%=CapAgencyCode %>', callBackGetEducationProviders);
    } 
    
    function callBackGetEducationProviders(info)
    {
        var providerNameControl = document.getElementById('<%= txtWithImgProviderName.ClientID %>');
        var noRecordMsg = '<% =LabelUtil.GetTextByKey("acc_reg_message_noRecord", string.Empty).Replace("'","\\'")%>';
        // This param is define in GeneralNameList.js, it's use for focus current link after pop up window closed.
        currentObj = document.getElementById('<%= txtWithImgProviderName.ImageClientID %>');
        selectGeneralNameList(GetValue(providerNameControl), info, noRecordMsg, '<%=LabelUtil.GetTextByKey("education_detail_provider_name", string.Empty).Replace("'","\\'") %>', "educationProviderNameSelected", getElementsLeftWithControlWidth(providerNameControl), getElementTop(providerNameControl) + providerNameControl.offsetHeight,true);

        HideLoading();
    }
    
    function educationProviderNameSelected(value)
    {
        isDisabled = true;
        Accela.ACA.Web.WebService.EducationService.GetEducationProviderData(value, '<%=CapAgencyCode %>', callBackFillEducationProviderInfo);
    }
    
    // fill provider information.
    function callBackFillEducationProviderInfo(info)
    {
        if(info == '')return;
        
        if (typeof (myValidationErrorPanel) != "undefined"){
            myValidationErrorPanel.clearErrors();
        }

        var json = eval('(' + info + ')');
        SetValueById('<%=txtWithImgProviderName.ClientID%>',JsonDecode(json.ProviderName));
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
    
    //****************Begin Education PopUp Selected *****************//
    function displayEducationNamesForm(searchBehind)
    {
        if ($('#<% = txtWithImgMajorName.ClientID%>').is('[readonly]')) {
            return;
        }
        
         if(searchBehind) {
            ShowLoading();

            var continueEducationName = getMajorName();
            var providerName = getEducationProviderName();
            Accela.ACA.Web.WebService.EducationService.GetRefEducationNames(continueEducationName, providerName, '<%=CapAgencyCode %>', '<%=ModuleName %>', CallBackGetEducationNames);
            
        } else {
            var info = '<% = RefEducationNameValueString %>';
            showEducationNames(info, true);
        }
        
        if (searchBehind) {
            stopBublle();
        }
    } 
    
    function CallBackGetEducationNames(info) {
        showEducationNames(info, false);

        HideLoading();
    }
    
    function showEducationNames(info, searchWithOutCapType)
    {
        var majorNameControl = document.getElementById('<%= txtWithImgMajorName.ClientID %>');
        var noRecordMsg = '<% =LabelUtil.GetTextByKey("acc_reg_message_noRecord", string.Empty).Replace("'","\\'")%>';
        // This param is define in GeneralNameList.js, it's use for focus current link after pop up window closed.
        currentObj = document.getElementById('<%= txtWithImgMajorName.ImageClientID%>');
        selectGeneralNameList(GetValue(majorNameControl), info, noRecordMsg,
            '<%=LabelUtil.GetTextByKey("education_detail_major_discipline", string.Empty).Replace("'","\\'") %>', "educationNameSelected", 
            getElementsLeftWithControlWidth(majorNameControl), getElementTop(majorNameControl) + majorNameControl.offsetHeight, !searchWithOutCapType);
    }
    
    function educationNameSelected(value) {
        var record = value.split('\b');
        var refEducationNumber = record[1];
        var moduleName = '<%=ModuleName %>';
        Accela.ACA.Web.WebService.EducationService.GetRefEducationData(refEducationNumber, moduleName, '<%=CapAgencyCode %>', callBackFillEducationNameInfo);
        stopBublle();
    }

    // fill education information.
    function callBackFillEducationNameInfo(info) {
        if (info == '') return;
        
        if (typeof (myValidationErrorPanel) != "undefined"){
            myValidationErrorPanel.clearErrors();
        }
        
        var json = eval('(' + info + ')');
        SetValueById('<%=txtWithImgMajorName.ClientID%>', JsonDecode(json.RefEducationName));
        $('#<%=hdnOrginalEduName.ClientID%>').val(JsonDecode(json.RefEducationName));
        SetValueById('<%=ddlDegree.ClientID%>', json.Degree);
        SetValueById('<%=txtRequired.ClientID%>', json.Required);
        $('#<%=hdnRefEducationSeq.ClientID%>').val(json.RefEducationNbr);
        
        if (!$.global.isAdmin) {
            var p = new ProcessLoading();
            p.showLoading();
        }
        __doPostBack('<%=updatePanel.UniqueID + POST_BACK_BY_CHANGE_NAME%>', json.RefEducationNbr);
    }

    //****************End Education PopUp Selected *****************//

    $(document).click(function () {
        if(typeof(clickInPanel) != "undefined" && !clickInPanel){
            changeEduName();
        }
    });
    
    function changeEduName() {
        if (needPostBack()) {
            if (typeof (myValidationErrorPanel) != "undefined"){
                myValidationErrorPanel.clearErrors();
            }
            
            if (!$.global.isAdmin) {
                var p = new ProcessLoading();
                p.showLoading();
            }
            __doPostBack('<%=updatePanel.UniqueID + POST_BACK_BY_CHANGE_NAME%>', '');
        }
    }

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_pageLoaded(<%=ClientID %>_PageLoaded);

    function <%=ClientID %>_PageLoaded(sender, args) {
        $('#<% = txtWithImgMajorName.ClientID%>').autosearch("displayEducationNamesForm");
    }
    
    prm.add_endRequest(<%=ClientID %>_EndRequest);
    function <%=ClientID %>_EndRequest(sender, args) {
        var needValidate = $('#<%=hdnPostBackValidate.ClientID%>');
        if (needValidate.val() == '1') {
            needValidate.val('0');
        }
        
        if(<%=IsFromRefContact.ToString().ToLower() %>) {
            $("#<%=txtRequired.ClientID %>_element_group").hide();
        }
    }
    
    function needPostBack() {
        var eduName = getMajorName();
        var orginalEduName = $('#<%=hdnOrginalEduName.ClientID%>').val();
        
        if (eduName != orginalEduName) {
            $('#<%=hdnOrginalEduName.ClientID%>').val(eduName);
            return true;
        }

        return false;
    }
    
    //Add page load function to update the tooltip for the Major Discipline(Education Name) field.
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_pageLoaded(PageLoaded);

    function PageLoaded(sender, args) {
        //Education Edit form will be refreshed when Major Discipline(Education Name) has been changed, so we will add a title to describe what will happen before the change.
        if (!$.global.isAdmin && IsTrue('<%=txtWithImgMajorName.Visible %>')) {
            UpdateTextboxToolTip('<%=txtWithImgMajorName.ClientID %>', '<%=GetTextByKey("education_detail_major_discipline") %>');
        }
    }

</script>

