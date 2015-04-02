<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.DelegateUsersView" Codebehind="DelegateUsersView.ascx.cs" %>

<%@ Register src="PopupActions.ascx" tagname="PopupActions" tagprefix="uc1" %>

<script type="text/javascript">
function showPopupDialog(pageUrl, objectTargetID) {
    ACADialog.popup({ url: pageUrl, width: 650, height: 640, scroll: false, objectTarget: objectTargetID });
}        
    
function ConfirmRemoveProxyUser(userSeqNum, IsProxyUser, objectTarget) {
    if (typeof (SetNotAsk) != 'undefined') {
        SetNotAsk();
    }

    if (confirm('<%=GetTextByKey("aca_message_confirm_removeproxy").Replace("'","\\'") %>')) {
        $get("<%=hdnUserSeqNum.ClientID%>").value = userSeqNum;
        $get("<%=hdnIsProxyUser.ClientID%>").value = IsProxyUser;
        $get("<%=lnkRemovePermission.ClientID%>").click();
    }
    else {
        $get(objectTarget).focus();
    }
}

function SetControlFocus(obj) {
    $get("<%=hdnFocusControlID.ClientID%>").value = obj.id;
}

function RefreshParent() {
    __doPostBack("<%=btnPostback.UniqueID%>", '');
}

function SetFocusBack() {
    var obj = $get("<%=hdnFocusControlID.ClientID%>");

    if (obj != null && obj.value != "") {
        document.getElementById(obj.value).focus();
    }
}

</script>

<asp:UpdatePanel ID="myDelegateUserPanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="btnPostback" runat="Server" OnClick="PostbackButton_Click" CssClass="ACA_Hide" TabIndex="-1"></asp:LinkButton>
<asp:Button ID="lnkRemovePermission" runat="server" TabIndex="-1" OnClick="RemoveAccountLink_OnClick" CssClass="ACA_Hide" />
<asp:HiddenField ID="hdnFocusControlID" runat="server" />
<div class="ACA_Page ACA_Page_FontSize">
</div>
<br />
<div class="ACA_TabRow ACA_Page_FontSize">
    <div class="Header_h3">
        <ACA:AccelaLabel ID="lblMyDelegateUserTitle" LabelKey="aca_who_can_access_account" runat="server"></ACA:AccelaLabel>
    </div>
</div>
<div class="ACA_Page_FontSize ACA_Page">
<asp:Repeater ID="rptMyDelegateUser" runat="server" OnItemDataBound="MyDelegateUser_ItemDataBound">
    <ItemTemplate>  
        <table role='presentation' class="ACA_FullWidthTable">
        <tr valign="bottom"> 
            <td class="ACA_Nowrap"> 
                <div id="divProxyUserInfromation" runat="server">
                    <ACA:AccelaLabel ID="lblUserName" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                    (<ACA:AccelaLabel ID="lblProxyUserEmailAddress" Text='<%# DataBinder.Eval(Container.DataItem, "email")%>' runat="server"></ACA:AccelaLabel>)
                </div>
            </td> 
            <td class="ACA_MLonger ACA_ARight">
                <div runat="server" class="ACA_LinkButton" id="divDelegateUserViewInvition" visible="false">
                    <ACA:AccelaLinkButton ID="btnViewInvitation" OnClientClick="SetControlFocus(this);" LabelKey="aca_delegate_view_invitation" CommandName="ViewInvitation" runat="server"></ACA:AccelaLinkButton>
                </div>
                <uc1:PopupActions ID="PopupActions" Visible="false" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="divProxyUserActionDate" class="ACA_Magin_Top_Negative3" runat="server"><ACA:AccelaLabel ID="lblProxyUserActionDate" IsNeedEncode="false" runat="server"></ACA:AccelaLabel></div>
            </td>
        </tr>
        </table>   
        <asp:HiddenField ID="hdnDelegateUserSeqNum" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "userSeqNum")%>'/>        
        <div id="divHeight" class="ProxyUser_Normal_Divide"></div>
    </ItemTemplate>
</asp:Repeater>
<div class="ACA_TabRow_Italic">
    <ACA:AccelaLabel runat="server" ID="lblDelegateUserNone" LabelKey="aca_delegate_no_user" Visible="false"></ACA:AccelaLabel>
</div>
<div class="ACA_TabRow">
    <div id="divAddDelegateUser" runat="server" class="ACA_FLeft ACA_DivMargin6 ACA_LinkButton">
        	<ACA:AccelaLinkButton ID="btnAddDelegateUser" runat="server" LabelKey="aca_add_delegate" CssClass="NotShowLoading"></ACA:AccelaLinkButton>
    </div>
    <div id="divShowRejectExpiredProxyUsers" runat="server" class="ACA_FLeft ACA_LinkButton">
        	<ACA:AccelaLinkButton ID="btnShowRejectExpiredProxyUsers" OnClientClick="SetControlFocus(this);" runat="server" OnClick="ShowRejectExpiredProxyUsersButton_OnClick" LabelKey="aca_show_expired_items"></ACA:AccelaLinkButton>
    </div> 
    <div id="divHiddenRejectProxyItems" runat="server" Visible="false" class="ACA_TabRow ACA_LinkButton">
        	<ACA:AccelaLinkButton ID="btnHiddenRejectProxyItems" runat="server" LabelKey="aca_hide_expired_items"></ACA:AccelaLinkButton>
    </div>
</div>
</div>

<div id="divHeight" class="ProxyUser_PersonNote_Divide"></div>
<div class="ACA_TabRow ACA_Page_FontSize">
    <div class="Header_h3">
        <ACA:AccelaLabel ID="lblInitUserTitle" LabelKey="aca_whose_account_can_access" runat="server"></ACA:AccelaLabel>
    </div>
</div>

<div class="ACA_Page_FontSize ACA_Page">
<asp:Repeater ID="rptMyInitUser" runat="server" OnItemDataBound="MyInitUser_ItemDataBound" OnItemCommand="MyInitUser_ItemCommand">
    <ItemTemplate>  
        <table role='presentation' class="ACA_FullWidthTable">
        <tr> 
            <td class="ACA_Nowrap">
                <div id="divInitUserInfromation" runat="server">
                    <ACA:AccelaLabel ID="lblInitUserName" runat="server" IsNeedEncode="False"></ACA:AccelaLabel>
                    (<ACA:AccelaLabel ID="lblInitUserEmailAddress" Text='<%# DataBinder.Eval(Container.DataItem, "email")%>' runat="server"></ACA:AccelaLabel>)
                </div>
            </td> 
            <td class="ACA_MLonger ACA_ARight">
                <span class="ACA_LinkButton ACA_DivMargin6" id="divAccept" runat="server" visible="false">
                    <ACA:AccelaLinkButton ID="btnAccept" OnClientClick="SetControlFocus(this);" CommandName="Accept" LabelKey="aca_delegate_accept" runat="server"></ACA:AccelaLinkButton>
                </span>
                <span class="ACA_LinkButton" id="divReject" runat="server" visible="false">                    
                    <ACA:AccelaLinkButton ID="btnReject" OnClientClick="SetControlFocus(this);" LabelKey="aca_delegate_reject" CommandName="Reject"  runat="server"></ACA:AccelaLinkButton>
                </span>
                <uc1:PopupActions ID="InitUserActions" Visible="false" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2"><div id="divInitUserActionDate" class="ACA_Magin_Top_Negative5" runat="server"><ACA:AccelaLabel ID="lblInitUserActionDate" IsNeedEncode="false" runat="server"></ACA:AccelaLabel></div></td>
        </tr>
        </table>  
        <asp:HiddenField ID="hdnInitUserSeqNum" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "userSeqNum")%>'/>         
    </ItemTemplate>
</asp:Repeater>
<div class="ACA_TabRow_Italic">
    <ACA:AccelaLabel runat="server" ID="lblInitUserNone" LabelKey="aca_delegate_no_user" Visible="false"></ACA:AccelaLabel>
</div>
<div id="divInitHeight" class="ProxyUser_Normal_Divide"></div>
<div id="divShowRejectExpiredInitUsers" runat="server" class="ACA_TabRow ACA_LinkButton">
        <ACA:AccelaLinkButton ID="btnShowRejectExpiredInitUsers" runat="server" OnClick="ShowRejectExpiredInitUsersButton_OnClick" LabelKey="aca_show_expired_items"></ACA:AccelaLinkButton> 
</div>
<div id="divHiddenInitExpiredItems" runat="server"  Visible="false" class="ACA_TabRow ACA_LinkButton">
        <ACA:AccelaLinkButton ID="btnShowRejctInitItems" runat="server" LabelKey="aca_hide_expired_items"></ACA:AccelaLinkButton>
</div>
</div>
<asp:HiddenField ID="hdnShowRejectExpiredProxyUsers" runat="server" />
<asp:HiddenField ID="hdnShowRejectExpiredInitUsers" runat="server" />

    <asp:HiddenField runat="server" ID="hdnUserSeqNum"/>
    <asp:HiddenField runat="server" ID="hdnIsProxyUser"/>
</ContentTemplate>
</asp:UpdatePanel>
