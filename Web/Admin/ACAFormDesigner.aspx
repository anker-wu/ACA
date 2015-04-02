<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ACAFormDesigner.aspx.cs"
    Inherits="ACAFormDesigner" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/Default/custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        html, body
        {
            margin: 0;
            padding: 0;
           height: 100%;
            width: 100%;
        }
        body
        {
            
            margin: 0;
        }
        
        .l2rdiv{direction:rtl}
        .l2rdiv span{direction:rtl}
        .l2rdiv table{ direction:rtl}
        .l2rdiv table tr{ direction:rtl}
        .l2rdiv table tr td{ direction:rtl}
        
    </style>
    <script src="../Silverlight.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ChangeRefreshTag(tag) {
            if (this.parent.ChangeRefreshTag) {
                this.parent.ChangeRefreshTag(tag);
            }
        }

        function ChangeDataChangedTag(tag) {
            if (this.parent.DataChangedTag) {
                this.parent.DataChangedTag(tag);
            }
        }

        function ShowErrorMessage(msg) {
            if (this.parent.ShowErrorMessage) {
                this.parent.ShowErrorMessage(msg);
            }
        }

        function CheckDataTag() {
            if (this.parent.CheckDataChangeTarget) {
                this.parent.CheckDataChangeTarget();
            }
        }

        document.oncontextmenu = new Function("return false");

        function ChangeTypeHandle() {
            if (!this.parent.CheckDataChangeTarget()) {
                var lastSelect = document.getElementById("LastSelectIndex").value;
                document.getElementById("ddlEntityType").options[lastSelect].selected = true;
                return false;
            }
            else {
                __doPostBack('ddlEntityType', '');
                ChangeDataChangedTag(false);
            }
        }
    </script>
    <link href="../App_Themes/Default/form.css" rel="stylesheet" type="text/css" />
    <link href="styles/basic.css" rel="stylesheet" type="text/css" />
    <link href="styles/main.css" rel="stylesheet" type="text/css" />
</head>
<body style="overflow-y:hidden">
    <form id="form1" runat="server" style="height: 100%; width: 100%;">
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
    </span>
    <div id="div4Focus"  style=" position:absolute; width:1px; height:1px; z-index:-1;"></div>
    <div id="divType" style="width: 100%; height: 40px; background-color: #c6d4e4;
        border-bottom: 1px; border-bottom-color: Black;" runat="server">
        <span style="font-weight: bolder; margin-left:3px">
            <%= FormName%>: </span>
        <span alt="Please select a <%= TypeLabel%>  for the template fields display.">Please
            select a
            <%= TypeLabel%> for the template fields display.</span><br />
        <table role='presentation'>
            <tr>
                <td>
                    <%= TypeLabel%>:
                </td>
                <td>
                    <ACA:AccelaDropDownList ID="ddlEntityType" CssClass="ACA_NoBorder" runat="server"
                        OnSelectedIndexChanged="Types_SelectedIndexChanged" Style="height: auto;">
                    </ACA:AccelaDropDownList>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <div id="divFDViewer" style="width: 100%; height: 100%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <object id="FDViewer" data="data:application/x-silverlight-2," type="application/x-silverlight-2"
            width="100%" height="100%">
            <param name="source" value="<%=FormDesignerSource %>" />
            <param name="initParams" value="<%=InitParams %>" />
            <param name="background" value="white" />
            <param name="minRuntimeVersion" value="4.0.50401.0" />
            <param name="autoUpgrade" value="true" />
            <a class="NotShowLoading" href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration: none">
                <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight"
                    style="border-style: none" />
            </a>
        </object>
        <iframe id="_sl_historyFrame" class="aca_form_designer_frame"  title="<%=LabelUtil.GetGlobalTextByKey("aca_formdesigner_iframe") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
    </div>
    <script type="text/javascript">
    //for shield the undefined method js error
    try{
        document.getElementById("FDViewer").content.OnFullScreenChange = function() { }
        document.getElementById("FDViewer").content.OnResize = function() { }
        document.getElementById("FDViewer").onfocus = function() {
            document.getElementById("div4Focus").focus(); 
        }
    }
    catch(e){

    }

    function AutoFixHeight() {
        try{
            if ($get('divType') && $get('divType').offsetHeight > 0) {
                 $get('divFDViewer').style.height = document.documentElement.clientHeight - $get('divType').offsetHeight + 'px';
            }
            else {
                $get('divFDViewer').style.height = '100%';
            }
        }
        catch(e)
        {
        }
    }

    setInterval("AutoFixHeight()", 300);

    </script>
    <asp:HiddenField ID="LastSelectIndex" runat="server" />
    </form>
</body>
</html>
