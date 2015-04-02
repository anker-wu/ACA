<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Attachment.AttachmentsList" Codebehind="AttachmentsList.aspx.cs" %>

<%@ Register Src="~/Component/AttachmentList.ascx" TagName="attachmentList" TagPrefix="ucl" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head runat="server">
    <title><%=LabelUtil.GetGlobalTextByKey("aca_attachment_label_title|tip")%></title>
    <script type="text/javascript" src="../Scripts/jquery.js"></script>
    <script type="text/javascript" src="../Scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../Scripts/global.js"></script>
    <script type="text/javascript" src="../Scripts/textCollapse.js"></script>
    <script type="text/javascript" src="../Scripts/ProcessLoading.js"></script>
    <script type="text/javascript" src="../Scripts/FileUpload.js"></script>
    <script type="text/javascript">
        var Args;
        var TimerID = 0;

        window.onload = function() {
            var isAdmin = IsTrue(getparmByUrl("isAdmin"));

            if (isAdmin) {
                setInterval("setAttachmentListHeight()", 300);
            } else {
                setAttachmentListHeight();
            }

            Args = $get('hfValue').value.split('|');
            if (Args[0] > 0) {
                StartTimer();
            } else if (typeof (parent.HideWaitMessage) != 'undefined') {
                parent.HideWaitMessage();
            }

            parent.HideLoading();
        };

        function StartTimer()
        {
            if(TimerID > 0)
            {
                window.clearTimeout(TimerID);
            }
            
            TimerID = window.setTimeout('CheckPendingAttachments()',2000);
        }
        
        function CheckPendingAttachments()
        {
            PageMethods.GetPendingAttachments(Args[1], HandleResult, OnError);
        }

        function HasPendingDocs() {
            return Args[0] > 0;
        }

        function OnError() { }
        
        function HandleResult(result)
        {
            if(Args[0] != result)
            {
                //parent.hideMessage();
                RefreshPage();
            }
            else
            {
                StartTimer();
            }
        }
        
        function RefreshPage()
        {
            window.location.href = window.location.href;
        }        

        function setAttachmentListHeight() {
            // set the attachmentlist iframe's height to its container.
            var iframe = getOwnIframe(window);

            if (iframe) {
                iframe.height = document.body.offsetHeight;
            }
        }

        $(document).ready(function () {
            if ($.browser.opera) {
                //Make links 'tab-able' in Opera
                SetTabIndexForOpera();

                //For support opera to Tab between iframe content and outside of iframe
                $("#hlAttachmentListBegin", window.parent.document).keydown(function (e) {
                    OverrideTabKey2(e, $("#attachmentList_lnkFocusAnchor"));
                });

                $("#attachmentList_lnkAttachmentEnd").keydown(function (e) {
                    OverrideTabKey2(e, $("#hlAttachmentListEnd", window.parent.document));
                });
            }
        });
    </script>
</head>
<body class="ACA_Page_NoScrollBar">
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
    </span>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release" />
        <div>
            <ucl:attachmentList ID="attachmentList" runat="server" />
        </div>
        <asp:HiddenField ID="hfValue" runat="server" />
        <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
    <script type="text/javascript">
        // use for the process loading to get the loading's title and src.
        var getText = parent.getText;

        if ('<%=AppSession.IsAdmin%>'.toLowerCase() == 'false') {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(function () {
                var p = new ProcessLoading();
                p.initControlLoading(true);
            });
        }
    </script>
</body>
</html>