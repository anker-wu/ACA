<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Cap.CapEdit"
    MasterPageFile="~/Default.master" ValidateRequest="false" CodeBehind="CapEdit.aspx.cs" %>

<%@ Reference Control="~/Component/ContactEdit.ascx" %>
<%@ Reference Control="~/Component/AddressEdit.ascx" %>
<%@ Reference Control="~/Component/ParcelEdit.ascx" %>
<%@ Reference Control="~/Component/OwnerEdit.ascx" %>
<%@ Reference Control="~/Component/LicenseEdit.ascx" %>
<%@ Reference Control="~/Component/CapDescriptionEdit.ascx" %>
<%@ Reference Control="~/Component/AppSpecInfoEdit.ascx" %>
<%@ Reference Control="~/Component/AppSpecInfoTableEdit.ascx" %>
<%@ Reference Control="~/Component/DetailInfoEdit.ascx" %>
<%@ Reference Control="~/Component/AttachmentEdit.ascx" %>
<%@ Reference Control="~/Component/EducationEdit.ascx" %>
<%@ Reference Control="~/Component/MultiContactsEdit.ascx" %>
<%@ Reference Control="~/Component/ContinuingEducationEdit.ascx" %>
<%@ Reference Control="~/Component/ExaminationEdit.ascx" %>
<%@ Reference Control="~/Component/MultiLicensesEdit.ascx" %>
<%@ Reference Control="~/Component/ValuationCalculatorEdit.ascx" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Web.Util" %>
<%@ Register Src="~/Component/PageFlowActionBar.ascx" TagPrefix="ACA" TagName="PageFlowActionBar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <script src="../Scripts/Education.js" type="text/javascript"></script>
    <script src="../scripts/GeneralNameList.js" type="text/javascript"></script>
    <script src="../Scripts/DisableForm.js" type="text/javascript"></script>
    <script src="../Scripts/MessageDialog.js" type="text/javascript"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/FileUpload.js")%>"></script>
    <script type="text/javascript">
        var NeedAsk = true;
        var DisabelSave = false;
        var EnabledAutoFillDDLIDs = '';
        var moduleName ='<%=ModuleName %>';
        var CapEditPageNotValidateEmpetyValueControlFlag = false;

        function EnabledAutoFillDDL()
        {
            var ids = EnabledAutoFillDDLIDs.split('|');
            for(var i=0;i<ids.length;i++)
            {
                if(ids[i] != '')
                {
                    $get(ids[i]).disabled = false;
                }
            }
        }

        function SaveAndResume()
        {
            if($.global.isAdmin)
            {
                return;
            }
        
            if(!CheckParcelInput() && '<%=StandardChoiceUtil.IsSuperAgency() %>' != 'True')
            {
                alert('<%=GetTextByKey("per_cap_edit_needParcelInfo").Replace("'","\\'") %>');
                var parcelNo = $get("ctl00_PlaceHolderMain_ParcelEdit_txtParcelNo");            

                if (parcelNo != null) 
                {
                    if(GetValue(parcelNo) == "" && parcelNo.disabled == false)
                    {
                        parcelNo.focus();  
                    }
        
                }
                return;            
            }
        
            var isContactTypeNull = $get("ctl00_PlaceHolderMain_MultiContactsEdit_contactList_hfIsContactTypeNull");
            var ContactList = $get("ctl00_PlaceHolderMain_MultiContactsEdit_contactList_gdvContactList");

            if (isContactTypeNull && isContactTypeNull.value == "Y")
            {
                alert('<%=GetTextByKey("per_contactlist_needcontacttype").Replace("'","\\'") %>');

                if (ContactList != null) 
                {
                    ContactList.focus();
                }
            
                return;
            }
    
            CapEditPageNotValidateEmpetyValueControlFlag = true;
            var options = new WebForm_PostBackOptions("SaveAndResume", "", true, "", "", false, true);
        
            if(typeof(Page_ClientValidate)=="undefined" || Page_ClientValidate(options.validationGroup)) {
                var p = new ProcessLoading();
                p.showLoading(true);

                NeedAsk = false;
                __doPostBack("SaveAndResume", "");    
            }
            else
            {
                CapEditPageNotValidateEmpetyValueControlFlag = false;
            }
        
        }
    
        function RemoveDisabledField() {
                if(typeof(SetFieldToDisabled) !="undefined")
                {
                    for(id in ReadOnlyFieldCollection){
                        SetFieldToDisabled(id, false);
                    }
                }
        }
    
        function CheckParcelInput()
        {
            if('<%=IsParcelSectionExists %>' == 'True')
            {
                return IsValidateParcel();
            }
        
            return true;
        }

        function SetNotAsk(startTimer)
        {
            NeedAsk = false;
            if(startTimer)
            {
                window.setTimeout('NeedAsk=true',1500);
            }
        }
    
        var prm = Sys.WebForms.PageRequestManager.getInstance();
    
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
    
        var onHanding = false;
 
        function InitializeRequest(sender, args)
        {
            onHanding = true;
            hideMessage();
            document.body.style.cursor = 'wait';
        }

        function EndRequest(sender, args)
        {
            NeedAsk = true;
            EnabledAutoFillDDL();
            document.body.style.cursor = '';
            onHanding = false;
            
            if (typeof (myValidationErrorPanel) != "undefined") {
                myValidationErrorPanel.reservedAllErrors();
            }
        }
        
         function RefreshAddress() {
            SetNotAskForSPEAR();
            __doPostBack('<%=btnRefreshAddress.UniqueID %>');
        }
    
        function FillAddress(info)
        {
            FillGISAddress(info);
        }
    
        function FillWithMyOwnerInfo(value,callbackFunc) {
            PageMethods.GetPublicUserModel(value, moduleName, '<%=ConfigManager.AgencyCode %>', '<%=IsFromAuthAgentPage %>', callbackFunc);
        }
    
        function FillWithMyAddressInfo(value, callbackFunc) {
            PageMethods.GetPublicUserModel(value, moduleName, '<%=ConfigManager.AgencyCode %>', '<%=IsFromAuthAgentPage %>', callbackFunc);
        }

        function FillWithMyParcelInfo(value, callbackFunc) {
            PageMethods.GetPublicUserModel(value, moduleName, '<%=ConfigManager.AgencyCode %>', '<%=IsFromAuthAgentPage %>', callbackFunc);
        }
    
        //Get remind message.
        function GetOwnerCondtionMessage(value)
        {
            PageMethods.GetOwnerCondtionMessage(value,moduleName,callbackMessage);
            $get("divAllKindMessage").style.display = 'none'; 
            $get("divConditon").style.display = 'none';  
            $get("divRefOwnerList").style.display = 'none'; 
        }
    
        function callbackMessage(result)
        {
            var divMessage = document.getElementById("divMessage");
            var spanConditionPaddingTop = document.getElementById("spanConditionPaddingTop");
            if (result =='')
            {
                if (spanConditionPaddingTop)
                    spanConditionPaddingTop.style.display = 'none';
                if(divMessage)
                    divMessage.style.display = 'none';
                document.getElementById("divOwnerConditionInfo").style.display='none';
            }
            else
            {
                if (spanConditionPaddingTop)
                    spanConditionPaddingTop.style.display = 'block';
                if(divMessage)
                    divMessage.style.display = 'block';
            } 
            if(divMessage)
                divMessage.innerHTML = result;
        }
    
        //Get owner condition.

        function GetOwnerCondition(value)
        {
            PageMethods.GetOwnerCondition(value,moduleName,callbackOwnerCondition);
        }

        function GetAddressCondition(value) {
            PageMethods.GetAddressCondition(value, moduleName, callbackAddressCondition);
        }

        function callbackAddressCondition(result) {
            document.getElementById("divAddressConditionList").innerHTML = result;
        }

        function GetParcelCondition(value) {
            PageMethods.GetParcelCondition(value, moduleName, callbackParcelCondition);
        }

        function callbackParcelCondition(result) {
            document.getElementById("divParcelConditionList").innerHTML = result;
        }

        function callbackOwnerCondition(result)
        {
            document.getElementById("divConditionList").innerHTML = result;
        }
    
        function IEBack()
        {
            var url = window.location.href;
            var index = url.indexOf('FormIndex');
            if(index > -1)
            {
                var re = new RegExp("(.*)stepNumber=(\\d)(.*)FormIndex=(\\d)(.*)","ig");
                re.exec(url);
                if(RegExp.$4 == 1)
                {
                    window.location.href = '../Cap/CapType.aspx?Module=<%=Accela.ACA.Common.Common.ScriptFilter.EncodeUrlEx(ModuleName)%>&stepNumber=0';
                }
                else
                {
                    window.location.href = RegExp.$1 + 'stepNumber=' + (parseInt(RegExp.$2) - 1) + RegExp.$3 + 'FormIndex=' + (parseInt(RegExp.$4) - 1) + RegExp.$5;
                }
            }
            else
            {
                window.location.href = '../Cap/CapType.aspx?Module=<%=Accela.ACA.Common.Common.ScriptFilter.EncodeUrlEx(ModuleName)%>&stepNumber=0';
            }
            return false;
        }
    
        if ($.browser.opera) {
            //After click back button of Opera, the previous page would be redirect to current url for keeping form data.
            $(window).bind('popstate', function (e) {
                window.location.href = window.location.href;
            });
        }
        
        function popUpDetailDialog(pageUrl, objectTargetID) {
            var popupDialogWidth = 680;
            var popupDialogHeight = 600;
            ACADialog.popup({ url: pageUrl, width: popupDialogWidth, height: popupDialogHeight, objectTarget: objectTargetID });
        }

        function RefreshEduExamList(componentName, componentType, refContactSeqNbr) {
            window.setTimeout(function () {
                delayShowLoading();
            }, 100);

            __doPostBack('<%=btnRefreshEduExamList.UniqueID %>', componentName + '<%=ACAConstant.SPLIT_CHAR %>' + componentType + '<%=ACAConstant.SPLIT_CHAR %>' + refContactSeqNbr);
        }

        function openEduExamLookUpDialog(sender, componentName, componentId, contactSeqNbr) {
            var url = '<%=FileUtil.AppendApplicationRoot("LicenseCertification/RefContactEducationExamLookUp.aspx") %>?&<%=UrlConstant.CONTACT_COMPONENT_NAME %>=' + componentName
            + '&<%=UrlConstant.COMPONENT_TYPE%>=' + componentId
            + '&Module=<%=ModuleName %>';
            
            if(contactSeqNbr) {
                url += '&contactSeqNbr=' + contactSeqNbr;
            }
            
            ACADialog.popup({ url: url, width: 800, height: 550, objectTarget: sender.id });
            return false;
        }
    </script>
    <div class="ACA_Area_CapEdit">
        <%--below div is used to add a calendar control to initial javascript, otherwise an error will raise in Safari broswer--%>
        <div id="divDate" style="width:0px; height:0px; display:none">
            <ACA:AccelaCalendarText ID="AccelaCalendarText1" CssClass="ACA_NShot"
                            runat="server" title="This is a hidden field."></ACA:AccelaCalendarText>
        </div>  
        <%--end --%>   
        <div class="ACA_RightItem">
            <div>
                <ACA:BreadCrumpToolBar ID="BreadCrumpToolBar" runat="server"></ACA:BreadCrumpToolBar>
            </div>
            <span id="SecondAnchorInACAMainContent"></span>
            <div class="ACA_TabRow">
                <span class="ACA_FRight required_indicate">
                    <span class="ACA_Required_Indicator">*</span><ACA:AccelaLabel ID="per_permitReg_label_indicate"
                        LabelKey="per_permitReg_label_indicate" runat="server"></ACA:AccelaLabel>
                </span>
            </div>
            <asp:PlaceHolder ID="placeHolder" runat="server"></asp:PlaceHolder>
            <div class="actionbar_bottom">
                <ACA:PageFlowActionBar runat="server" ID="actionBarBottom" />
            </div>
        </div>
    
        <!-- message box begin -->
        <div id="divBox" class="ACA_Hide PopUpDlg contact_duplicate_dialog">
            <div>
                <p>
                    <strong>
                        <ACA:AccelaLabel ID="lblSSNFeinMessageTitle" LabelKey="ssn_fein_message_title" runat="server" /></strong></p>
            </div>
            <div class="ACA_TabRow">
                <div class="ACA_Section_Instruction ACA_Section_Instruction_FontSize">
                    <ACA:AccelaLabel ID="lblEmseMessage" runat="server" />
                </div>
            </div>
            <div class="ACA_Template_Unit">
                <table role='presentation' cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                           <ACA:AccelaHideLink ID="hlBegin" AltKey="img_alt_form_begin" Width="0" runat="server" NextControlID="btnOK" />
                        </td>
                        <td>
                            <div class="ACA_LgButton ACA_LgButton_FontSize">
                                <ACA:AccelaButton runat="server" ID="btnOK" LabelKey="per_capedit_duplicate_continue"
                                    OnClick="BtnOk_Click" OnClientClick='SetNotAsk();' />
                            </div>  
                        </td>
                        <td style="width:10px"></td> 
                        <td>             
                            <div class="ACA_LgButton ACA_LgButton_FontSize">
                                <ACA:AccelaButton runat="server" LabelKey="per_capedit_duplicate_cancel" ID="btnCancel"
                                    OnClientClick="SetNotAsk();focusContinueButton();return cancel();" />
                            </div> 
                        </td> 
                    </tr>
                </table>
            </div>
        </div>    
        <div id="deck" class="ACA_Hide"></div>
        <!-- message box end -->
        <div class="ACA_TabRow">            
            <asp:UpdatePanel ID="refreshButtonPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="btnRefreshEduExamList" runat="Server" CssClass="ACA_Hide" OnClick="RefreshEduExamListButton_Click" TabIndex="-1"></asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
       <asp:UpdatePanel ID="updatePanelAddress" runat="server" UpdateMode="Conditional">
           <ContentTemplate>
              <asp:LinkButton ID="btnRefreshAddress" runat="Server" CssClass="ACA_Hide" OnClick="BtnRefreshAddress_Click" TabIndex="-1"/>
           </ContentTemplate>
       </asp:UpdatePanel>
   </div>
</asp:Content>
