<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequiredContactTypeIndicator.ascx.cs" Inherits="Accela.ACA.Web.Component.RequiredContactTypeIndicator" %>

<div id="divRequiredContactTypeIndicator" class="RequiredContactType_Indicator" runat="server"></div>
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    
    prm.add_pageLoaded(function () {
        PageMethods.DisplayRequiredContactTypeIndicator('<%=ModuleName %>', '<%=ComponentName %>', function (result) {
            var $divRequiredContactTypeIndicator = $('#<%=divRequiredContactTypeIndicator.ClientID %>');
        
            if (result != '' && <%=IsShowIndicator.ToString().ToLower()%>) {
                $divRequiredContactTypeIndicator.show();
                $divRequiredContactTypeIndicator.html(result);
            }
            else {
                $divRequiredContactTypeIndicator.hide();
            }
        });
    });
</script>
