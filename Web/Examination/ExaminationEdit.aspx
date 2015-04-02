<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExaminationEdit.aspx.cs"  MasterPageFile="~/Dialog.master"
Inherits="Accela.ACA.Web.Examination.ExaminationEdit" %>
<%@ Register Src="~/Component/ExaminationDetailEdit.ascx" TagName="ExaminationDetailEdit" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <script src="../scripts/GeneralNameList.js" type="text/javascript"></script>
    <script src="../Scripts/Education.js" type="text/javascript"></script>
    <div  class="ACA_Dialog_Content_NoFontFormat">
        <uc1:ExaminationDetailEdit ForCAPDetail="true"  ID="ExaminationDetailEdit" ExaminationSectionPosition="CapDetail" runat="server" />
    </div>

    <script type="text/javascript">
        function examinationFinish(examNbr) {
            PageMethods.CAPSubmit('<%=ModuleName %>',examNbr,callbackExaminationSubmit);
        }

        function callbackExaminationSubmit(errormsg) {
            if (errormsg) {               
                showMessage4Popup(errormsg, "Error");
            }
            parent.LoadInitExaminationList();           
        } 
    </script>
</asp:Content>

