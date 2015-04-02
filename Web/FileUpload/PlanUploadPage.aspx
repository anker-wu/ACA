<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.FileUpload.PlanUploadPage" ValidateRequest="false" Codebehind="PlanUploadPage.aspx.cs" %>
<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>
<%@ Register Assembly="Accela.Web.Controls" Namespace="Accela.Web.Controls" TagPrefix="ACA" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head id="Head1" runat="server">
    <title><%=LabelUtil.GetGlobalTextByKey("aca_uploadpage_label_title|tip")%></title>
    <script type="text/javascript" src="../Scripts/jquery.js"></script>
    <script type="text/javascript" src="../Scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../Scripts/global.js"></script>
    <script type="text/javascript">
   function Callback(result,isExceptionOccur,callbackFunc)
    { 
        var index = callbackFunc.indexOf('(');
        if(index > -1)
        {
            callbackFunc = callbackFunc.substring(0,index);
        }
        if(callbackFunc != '' && eval('typeof(window.parent.' + callbackFunc + ')') != 'undefined')
        {
            var executeString = 'window.parent.' + callbackFunc + "('"+ isExceptionOccur + "','" + result + "');";
            eval(executeString);
        }
    }
     
        
    function CheckFileType(doUpload)
    { 
        //var tip = 'File:';
        var inputFile = $get('inputFile');
        if(inputFile != null)
        {
            var value = inputFile.value;
            $get('fakeInput').value = value;
            var lbl = $get('lblFileUpload');
            var lblTip = $get('lblFileUploadtip');
            if(value != '')
            {
                var reg = /(\.html|\.htm|\.mht|\.mhtml)$/i;
                if(value.match(reg))
                {
                    //lblTip.innerText = tip + ' This file type cannot be uploaded';
                    lblTip.innerText = global_CheckFileType_Tip;
                    lblTip.className = "ACA_Error_Label";
                    lbl.className = "ACA_Error_Label";
                }
                else
                {
                    //lblTip.innerText = tip;
                    //lblTip.innerText = global_CheckFileType_FileLabel;
                    lblTip.innerText = "";
                    lblTip.className = "";
                    lbl.className = "";
                    $get('err_indicator').style.display='none';
                    document.getElementById('divFakeFile').className = "fakefile";
                    inputFile.className="file";//
                    if(doUpload)
                    {
                        DoUpload();
                    }
                }
            }
            else
            {
                lblTip.className = "ACA_Error_Label";
                lbl.className =  "ACA_Error_Label";
                $get('err_indicator').style.display='block';
                document.getElementById('divFakeFile').className = "fakefile2";
                inputFile.className="file2";//
                if(doUpload)
                {
                    DoUpload();
                }
            }
        }        
    }
    
    function InputKeyDown(e) 
    {
        if (Sys.Browser.name != "Firefox" && e.keyCode == 13) {
            $("#inputFile").trigger("click");
            return;
        }

        if (e.keyCode != 9 && e.keyCode != 13) {
            if (Sys.Browser.name == "Firefox") {
                return false;
            }
            else {
                e.returnValue = false;
            }
        }
    }
    
    function DoUpload()
    {
        var options = new WebForm_PostBackOptions("lnkSubmit", "", true, "", "", false, true);
        if(Page_ClientValidate(options.validationGroup))
        {
            ShowBlock(true);
            WebForm_DoPostBackWithOptions(options);    
        }
    }
    
    function ShowBlock(showProgressBar)
    {
      document.getElementById('divInfo').style.display = showProgressBar? 'none' : 'block';
      document.getElementById('divPb').style.display = showProgressBar? 'block' : 'none';
    }
    
    
  
   function AntoFitSize()
    {
        var inputFile =  document.getElementById('inputFile');
        
        if(typeof(inputFile)!="undefined" && inputFile != null)
        {
           if(parent.isFireFox() && Sys.Browser.version == "2")//browse's type is FireFox2.0
           {
                 inputFile.size = 75;
           }
           else
           {
                inputFile.size = 92;
           }
        } 
    }   
  
      // true: browser's type is fireFox
    // false: browser's type isn't fireFox
    function isFireFox()
    {
        if(navigator.userAgent.indexOf("Firefox")>0)
        {
            return true;
        } 
        else
        {
            return false; 
        }  
    }
  
    </script>

</head>

<body class="ACA_FileUploadPageStyle">

<script type="text/javascript">
var global_CheckFileType_Tip=' <%= LabelUtil.GetGlobalTextByKey("aca_fileuploadpage_checkfiletype_tip").Replace("'","\\'") %>';
var global_CheckFileType_FileLabel=' <%=String.Format(LabelUtil.GetGlobalTextByKey("aca_fileuploadpage_checkfiletype_filelabel").Replace("'","\\'"), "") %>';
</script>

    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div id="divInfo">
            <ACA:AccelaLabel ID="planreview_start_subhdr_label" LabelKey="planreview_start_subhdr" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" runat="server"></ACA:AccelaLabel>
            <br /><br />
            
        <asp:HiddenField ID="hfDocType" runat="server" Value="0" />
        <asp:HiddenField ID="hfCallbackFunc" runat="server" />
         <div class="ACA_TabRow ACA_LiLeft">
            <ACA:AccelaTextBox runat="server" ID="txbPlanName" Validate="required" CssClass="ACA_Label ACA_Label_FontSize" LabelKey="planreview_planlist_planname" style="width: 300px" MaxLength="100" />               
        </div>  
         <div>
            <div class="ACA_TabRow ACA_Label ACA_Label_FontSize">
                <table role='presentation' cellpadding="0" cellspacing="0" class='ACA_TDAlignLeftOrRightTop'>
                    <tr>
                        <td>
                            <div class="ACA_Error_Indicator" id="err_indicator" style="display: none">
                                <img class="ACA_Error_Indicator_In_Same_Row" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>"/>
                            </div>
                        </td>
                        <td>
                            <div style="width: 43em;">
                                 <div id="divFakeFile">
                                    <table role='presentation'>
                                        <tr>
                                            <td>
                                                <input id="fakeInput" class="ACA_Label ACA_Label_FontSize" title="This is a hidden field." style="width: 30em;" readonly="readonly" /></td>
                                            <td>
                                                <div class="ACA_Tab_Row ACA_LgButton ACA_LgButton_FontSize">
                                                    <ACA:AccelaButton ID="AccelaLabel2" runat="Server" LabelKey="ACA_FileUploadPage_Label_Browse"></ACA:AccelaButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="ACA_LRight">
                                    <Upload:InputFile ID="inputFile"   runat="server" Height="5px" onchange="CheckFileType(false)"
                                        CssClass="file transparent" onkeydown="return InputKeyDown(event);" oncontextmenu="return false;"
                                        Size="75" />
                                </div>
                                <div class="ACA_Required_Indicator">*</div>
                                <ACA:AccelaLabel ID="lblFileUpload" LabelKey="planreview_planlist_filename" runat="server"></ACA:AccelaLabel>
                                <ACA:AccelaLabel ID="lblFileUploadtip" runat="server"></ACA:AccelaLabel>
                            </div>
                            <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                ControlToValidate="inputFile"></asp:RequiredFieldValidator></div>
                            <div class="ACA_TabRow">
                                <ACA:AccelaLabel ID="lblFileSize" runat="server" CssClass="ACA_Sub_Label ACA_Sub_Label_FontSize" Width="494px"></ACA:AccelaLabel>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            
            <div class="ACA_TabRow ACA_LiLeft">
                <ACA:AccelaCheckBoxList ID="cblRuleSet" runat="server" LabelKey="planreview_start_rulesets" CssClass="ACA_SmLabel ACA_SmLabel_FontSize"/>
            </div>
            
            <ACA:AccelaLinkButton ID="lnkSubmit" runat="server" OnClick="SubmitButton_Click" TabIndex="-1"/>
            <div class="ACA_TabRow ACA_LgButton ACA_LgButton_FontSize ACA_Button_Text">
                <a href="javascript:CheckFileType(true);">
                    <ACA:AccelaLabel ID="lblSub" runat="server" LabelKey="per_attachment_Label_attachFile"></ACA:AccelaLabel>
                </a>
            </div>
            
             <script type="text/javascript">
        if (isFireFox())
        {
            document.write('<br /><br />');
        }
        </script>
            
        </div>
        
        </div>
        <div id="divPb"  class="ACA_Content" style="display: none">
            <br />
            <h1>
                <i>
                    <ACA:AccelaLabel ID="AccelaLabel1" runat="server" LabelKey="per_attachment_Label_uploads"></ACA:AccelaLabel>
                </i>
            </h1>
            <br />
            <Upload:ProgressBar ID="progressBar" runat="server" Triggers="submitButton_Click" Inline="true"
                Width="503px" Height="40px" OnPreRender="ProgressBar_Render">
                <ACA:AccelaLabel ID="lblCheckProgress" runat="server"  />
            </Upload:ProgressBar> 
        </div>       
        <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
 
 <script type="text/javascript">
        var callbackFunc = document.getElementById('hfCallbackFunc').value;
        if(callbackFunc != '')
        {           
            eval(callbackFunc);
        } 
    </script> 
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>
