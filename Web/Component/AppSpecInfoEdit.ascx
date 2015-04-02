<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.AppSpecInfoEdit" Codebehind="AppSpecInfoEdit.ascx.cs" %>

<ACA:AccelaHideLink ID="hlBegin" runat="server" AltKey="img_alt_form_begin" CssClass="ACA_Hide"/>
<div class="ACA_TabRow ACA_Label ACA_Label_FontSize_Restore">
<script language="javascript" type="text/javascript">
var splitChar = "<%=Accela.ACA.Common.ACAConstant.SPLIT_CHAR%>";
var defaultSelectText = "<%=WebConstant.DropDownDefaultText%>";
</script>
<script type="text/javascript" src="../Scripts/DrillDown.js"></script>

<div class="ACA_ASI_Container"> 
    <asp:PlaceHolder ID="phPlumbingGroup" runat="server" EnableTheming="true"></asp:PlaceHolder> 
</div>
</div>
<ACA:AccelaHideLink ID="hlEnd" runat="server" AltKey="aca_common_msg_formend" CssClass="ACA_Hide"/>
<ACA:AccelaHeightSeparate ID="sepForASIT" runat="server" Height="20" />

