<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="Accela.ACA.Web.Plan.PlanStart"  Codebehind="PlanStart.aspx.cs" %>

<%@ Register Src="../Component/PlanList.ascx" TagName="PlanList" TagPrefix="uc1" %>

<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload"
    TagPrefix="Upload" %>
<%@ Register Assembly="Accela.Web.Controls" Namespace="Accela.Web.Controls" TagPrefix="ACA" %> 

 
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">

 <script type="text/javascript"  >
 
 function AfterAttachmentUpload(isExceptionOccur,result)
           {
                if(isExceptionOccur == 'True')
                { 
                    showNormalMessage(result, 'Error');
                }
                else
                {
                    if(result != '')
                    {
                        showNormalMessage(result, 'Error');
                    }
                    else
                    {
                        window.location.href = window.location.href;
                    } 
                 }
           }
   function SetCwinHeight(){
      var bobo=document.getElementById("divNewAttachment"); //iframe id
      if (document.getElementById){
       if (bobo && !window.opera){
        if (bobo.contentDocument && bobo.contentDocument.body.offsetHeight){
         bobo.height = bobo.contentDocument.body.offsetHeight;
        }else if(bobo.Document && bobo.Document.body.scrollHeight){
         bobo.height = bobo.Document.body.scrollHeight;
        }
       }
      }
    }        
           
 </script>

    <script type="text/javascript">
    var global_CheckFileType_Tip=' <%= LabelUtil.GetGlobalTextByKey("aca_fileuploadpage_checkfiletype_tip").Replace("'","\\'") %>';
    var global_CheckFileType_FileLabel=' <%=String.Format(LabelUtil.GetGlobalTextByKey("aca_fileuploadpage_checkfiletype_filelabel").Replace("'","\\'"), "") %>';
    </script>    
  
      <div class="ACA_Content ACA_TabRow">
      <h1><ACA:AccelaLabel ID="planreview_start_hdr_label" LabelKey="planreview_start_hdr" runat="server"></ACA:AccelaLabel>     
            </h1>
            <br />
        <iframe id="divNewAttachment"  onload="Javascript:SetCwinHeight()" src="../FileUpload/PlanUploadPage.aspx?module=<%=Accela.ACA.Common.Common.ScriptFilter.EncodeUrlEx(ModuleName)%>&CallbackFunc=AfterAttachmentUpload"
             frameborder="0" width="100%"
            scrolling="no"><%=String.Format(LabelUtil.GetGlobalTextByKey("iframe_planstart_uploadfile_title"), "../FileUpload/PlanUploadPage.aspx?module=" + Accela.ACA.Common.Common.ScriptFilter.AntiXssUrlEncode(ModuleName) + "&CallbackFunc=AfterAttachmentUpload")%></iframe>
      </div>         
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
            <contenttemplate>
                <div class="ACA_Content" style="overflow-x: auto;">
                      <uc1:PlanList ID="PlanList" runat="server" DataSourceType="Uploaded" />
                </div>   
            </contenttemplate>
        </asp:UpdatePanel>
        
         <div class="ACA_Content ACA_Tab_Row ACA_LgButton ACA_LgButton_FontSize">                   
            <ACA:AccelaLinkButton ID="btnContinue" runat="server" LabelKey="planreview_start_continue" OnClick="ContinueButton_Click" OnClientClick="showMessage('', 'Error')" CausesValidation="false" />
          </div>      
 </asp:Content>
