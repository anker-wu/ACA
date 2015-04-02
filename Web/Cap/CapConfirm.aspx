<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Cap.CapConfirm"
    ValidateRequest="false" MasterPageFile="~/Default.master" Codebehind="CapConfirm.aspx.cs" %>

<%@ Reference Control="~/Component/ContactView.ascx" %>
<%@ Reference Control="~/Component/AddressView.ascx" %>
<%@ Reference Control="~/Component/ParcelView.ascx" %>
<%@ Reference Control="~/Component/OwnerView.ascx" %>
<%@ Reference Control="~/Component/LicenseView.ascx" %>
<%@ Reference Control="~/Component/CapDescriptionView.ascx" %>
<%@ Reference Control="~/Component/LicenseView.ascx" %>
<%@ Reference Control="~/Component/AppSpecInfoView.ascx" %>
<%@ Reference Control="~/Component/AppSpecInfoTableView.ascx" %>
<%@ Reference Control="~/Component/DetailInfoView.ascx" %>
<%@ Reference Control="~/Component/AttachmentEdit.ascx" %>
<%@ Reference Control="~/Component/EducationEdit.ascx" %>
<%@ Reference Control="~/Component/ContinuingEducationEdit.ascx" %>
<%@ Reference Control="~/Component/MultiContactsEdit.ascx" %>
<%@ Reference Control="~/Component/ExaminationEdit.ascx" %>
<%@ Reference Control="~/Component/MultiLicensesEdit.ascx" %>
<%@ Reference Control="~/Component/ValuationCalculatorView.ascx" %>
<%@ Register Src="~/Component/PageFlowActionBar.ascx" TagPrefix="ACA" TagName="PageFlowActionBar" %>
<%@ Register src="../Component/CapReviewCertification.ascx" tagname="CapReviewCertification" tagprefix="ACA" %>
<%@ Register Src="~/Component/CapFeeList.ascx" TagName="CapFeeList" TagPrefix="ACA"%>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <script src="../Scripts/MyCollectionMethods.js" type="text/javascript"></script>
    <script src="../scripts/GeneralNameList.js" type="text/javascript"></script>
    <script src="../Scripts/Education.js" type="text/javascript"></script>
    <script src="../Scripts/Validation.js" type="text/javascript"></script>
    <script src="../Scripts/Dialog.js" type="text/javascript"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/FileUpload.js")%>"></script>
    <script type="text/javascript">
    window.history.forward(1);     
    </script>
    <script type="text/javascript">
    var btnCollection = new KeyValuePair();
    btnCollection.Add('btnApplicantInfo','<%= BtnApplicantInfo%>');
    btnCollection.Add('btnContact1Info','<%= BtnContact1Info%>');
    btnCollection.Add('btnContact2Info','<%= BtnContact2Info%>');
    btnCollection.Add('btnContact3Info','<%= BtnContact3Info%>');
    btnCollection.Add('btnParcelInfo','<%= BtnParcelInfo%>');
    btnCollection.Add('btnMultiContactsInfo', '<%= BtnMultiContactsInfo%>');
    btnCollection.Add('btnMultiLicensesInfo', '<%= BtnMultiLicensesInfo%>');
    btnCollection.Add('btnEducationInfo', '<%= BtnEducationInfo%>');
    btnCollection.Add('btnExaminationInfo', '<%= BtnExaminationInfo%>');
    btnCollection.Add('btnContinuingEducationInfo', '<%= BtnContinuingEducationInfo%>');
    btnCollection.Add('btnAttachmentInfo', '<%= BtnAttachmentInfo%>');
    btnCollection.Add('btnOwnerInfo','<%= BtnOwnerInfo%>');
    btnCollection.Add('btnLicenseInfo','<%= BtnLicenseInfo%>');
    btnCollection.Add('btnWorkLocationInfo','<%= BtnWorkLocationInfo%>');
    btnCollection.Add('btnDescriptionInfo','<%= BtnDescriptionInfo%>');
    btnCollection.Add('btnAppSpecInfo','<%= BtnAppSpecInfo%>');
    btnCollection.Add('btnAppSpecTableInfo','<%= BtnAppSpecTableInfo%>');
    btnCollection.Add('btnDetailInfoInfo','<%= BtnDetailInfo%>');
    btnCollection.Add('btnValuationCalculatorInfo','<%= BtnValuationCalculatorInfo%>');
    var btnArray = btnCollection.GetArray();
    var moduleName ='<%=ModuleName %>';
    var StopSwitch = false;
    var EditText = '<%=GetTextByKey("per_permitConfirm_label_editButton").Replace("'","\\'") %>';
    var CancelText = '<%=GetTextByKey("per_permitConfirm_label_cancelButton").Replace("'","\\'") %>';
    
    var NeedAsk = true;
    
   var EnabledAutoFillDDLIDs = '';

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
        
    function SetNotAsk(startTimer)
    {
        NeedAsk = false;
        if(startTimer)
        {
            window.setTimeout('NeedAsk=true',1500);
        }
        
        if(arguments.length==2){
            var obj=arguments[1];
            if(typeof(obj)=="undefined"){
                return true;
            }
            if(obj.disabled==true || obj.disabled=="disabled")
                return false;    
        }
        
        return true;
    }
    
    function ClientClick() {
        NeedAsk = false;
        return true;
    }

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    prm.add_endRequest(EndRequest);

    var onHanding = false;

    function InitializeRequest(sender, args)
    {
        onHanding = true;
        document.body.style.cursor = 'wait';
    }
    
    function EndRequest(sender, args)
    {
        onHanding = false;
        document.body.style.cursor = '';
        
        if (typeof (ExportCSV) != 'undefined') {
            //export file.
            ExportCSV(sender, args);
        }
    }
    
    var  keepMessage = false;
    
    function FillAddress(info)
    {
        FillGISAddress(info);
    }

    function HideWaitMessage() {
        var divTipMsg = $get("divTipMsg");
        if (divTipMsg.style.display == "block") {
            SetNotAsk();
            WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions("<%=BtnContinueClientID %>", "", true, "", "", false, true));
        }
    }
    
    function ControlSectionDisplay(objId, imgObjId, lnkObjId, lblValue, componentName) {
        if ('<%=AppSession.IsAdmin %>'.toLowerCase() == 'true') {
            return;
        }

        ExpandOrCollapseSection(objId, imgObjId, lnkObjId, lblValue);
        
        if (componentName == 'attachment' || componentName == 'condition document') {
            var iframe = $("#" + objId).find("iframe")[0];
           
            if (typeof(iframe.contentWindow.setAttachmentListHeight) != "undefined") {
                iframe.contentWindow.setAttachmentListHeight();
            }
        }
    }

    </script>
    
    <div class="ACA_Area_CapConfirm">
        <div class="ACA_TabRow">
            <div id="divTipMsg" class="ACA_Tip_Message ACA_SmLabel ACA_SmLabel_FontSize">
               <div align="center">
                <table role='presentation'>
                    <tr>
                        <td>
                            <div class="ACA_Image_Loader"></div>
                        </td>
                        <td>
                            <%=GetTextByKey("per_cap_document_uploading_tip", ModuleName) %> 
                        </td>
                </tr>
            </table>
            </div>
            </div>
        </div>
        <div id="divContent" class="ACA_RightItem">
            <div>
                <ACA:BreadCrumpToolBar ID="BreadCrumpToolBar" runat="server"></ACA:BreadCrumpToolBar>
            </div>
            <span id="SecondAnchorInACAMainContent"></span>
            <ACA:AccelaHeightSeparate ID="sepForTital" runat="server" Height="10" />
            <div class="actionbar_top">
                <ACA:PageFlowActionBar runat="server" ID="actionBarTop" IsConfirmPage="True" />
            </div>
            <p>
                <ACA:AccelaLabel ID="AccelaLabel3" LabelKey="per_permitConfirm_text_confirmInfo"
                    runat="server"></ACA:AccelaLabel></p>
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="5" />
            <ACA:AccelaLabel ID="AccelaLabel4" LabelKey="per_permitConfirm_label_permitType"
                runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
            <p>
                <strong><asp:Label ID="lblPermitType" runat="server"></asp:Label></strong>
            </p>
            <ACA:AccelaHeightSeparate ID="sepForPermitType" runat="server" Height="15" />
            <div style="margin-top: 0px">
                <asp:PlaceHolder ID="placeHolder" runat="server"></asp:PlaceHolder>

            </div>
            <asp:PlaceHolder ID="placeDisclaimer" runat="server">
                <ACA:CapReviewCertification ID="capReviewCertification1" runat="server" />
            </asp:PlaceHolder>
        </div>
        <div class="ACA_RightItem">
            <div runat="server" id="divFeeList" Visible="False">
                <ACA:AccelaLabel ID="lblFeeList" LabelKey="aca_permitconfirm_label_feetitle" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                <ACA:CapFeeList ID="capFeeList" IsReviewPage="true" runat="server" />
            </div>
            <div class="actionbar_bottom">
                <ACA:PageFlowActionBar runat="server" ID="actionBarBottom" IsConfirmPage="True" />
            </div>
        </div>
    </div>
    <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
</asp:Content>