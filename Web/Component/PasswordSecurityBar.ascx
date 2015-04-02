<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.PasswordSecurityBar"  
    CodeBehind="PasswordSecurityBar.ascx.cs" %>

<table role='presentation' cellspacing="0" cellpadding="0" border="0"  id="indicatorTable" title="">
	<tr>
		<td id="validateText" align="center" nowrap="nowrap" class="ACA_Label ACA_TabRow_Italic">
		</td>
	</tr>
	<tr>
		<td valign="middle">
		<table role='presentation' width="104" cellspacing="0" cellpadding="0" id="progressBar" class="ACA_Password_Default">
			<tr class="Tr_Height1">
				<td width="1"></td>
				<td width="1"></td>
				<td width="1" class="ACA_Bar_Border"></td>
				<td width="1" class="ACA_Bar_Border"></td>
				<td width="24" class="ACA_Bar_Border"></td>
				<td width="24" class="ACA_Bar_Border"></td>
				<td width="24" class="ACA_Bar_Border"></td>
				<td width="24" class="ACA_Bar_Border"></td>
				<td width="1" class="ACA_Bar_Border"></td>
				<td width="1" class="ACA_Bar_Border"></td>
				<td width="1"></td>
				<td width="1"></td>
			</tr>
			<tr class="Tr_Height1">
				<td></td>
				<td class="ACA_Bar_Border"></td>
				<td class="ACA_Bar_Left1"></td>
				<td class="ACA_Bar_Left1"></td>
				<td class="ACA_Bar_Left1"></td>
				<td class="ACA_Bar_Left2"></td>
				<td class="ACA_Bar_Right1"></td>
				<td class="ACA_Bar_Right2"></td>
				<td class="ACA_Bar_Right2"></td>
				<td class="ACA_Bar_Right2"></td>
				<td class="ACA_Bar_Border"></td>
				<td></td>
			</tr>
			<tr class="Tr_Height2">
				<td class="ACA_Bar_Border"></td>
				<td class="ACA_Bar_Left1"></td>
				<td class="ACA_Bar_Left1"></td>
				<td class="ACA_Bar_Left1"></td>
				<td class="ACA_Bar_Left1"></td>
				<td class="ACA_Bar_Left2"></td>
				<td class="ACA_Bar_Right1"></td>
				<td class="ACA_Bar_Right2"></td>
				<td class="ACA_Bar_Right2"></td>
				<td class="ACA_Bar_Right2"></td>
				<td class="ACA_Bar_Right2"></td>
				<td class="ACA_Bar_Border"></td>
			</tr>
			<tr class="Tr_Height1">
				<td></td>
				<td class="ACA_Bar_Border"></td>
				<td class="ACA_Bar_Left1"></td>
				<td class="ACA_Bar_Left1"></td>
				<td class="ACA_Bar_Left1"></td>
				<td class="ACA_Bar_Left2"></td>
				<td class="ACA_Bar_Right1"></td>
				<td class="ACA_Bar_Right2"></td>
				<td class="ACA_Bar_Right2"></td>
				<td class="ACA_Bar_Right2"></td>
				<td class="ACA_Bar_Border"></td>
				<td></td>
			</tr>
			<tr class="Tr_Height1">
				<td></td>
				<td></td>
				<td class="ACA_Bar_Border"></td>
				<td class="ACA_Bar_Border"></td>
				<td class="ACA_Bar_Border"></td>
				<td class="ACA_Bar_Border"></td>
				<td class="ACA_Bar_Border"></td>
				<td class="ACA_Bar_Border"></td>
				<td class="ACA_Bar_Border"></td>
				<td class="ACA_Bar_Border"></td>
				<td></td>
				<td></td>
			</tr>
		</table>
		</td>
		<td>&nbsp;</td>
		<td valign="middle" title="">
			<a href = "javascript:void(0);" onclick="if(typeof(SetNotAsk)!='undefined') SetNotAsk();" title="" id="btnRequirements" class="NotShowLoading"><ACA:AccelaLabel id="lnkRequirements" runat="server" LabelKey="aca_accountedit_password_requirements" /></a>
			<div id="RequirementsLayout" class="ACA_Password_Requirements">
			    <div class="ACA_Sub_Label ACA_Message_Label ACA_FRight">
			        <a href = "javascript:void(0);" onclick="if(typeof(SetNotAsk)!='undefined') SetNotAsk();hideRequirementLayout();" title="" class="NotShowLoading">
			        <ACA:AccelaLabel id="lblPasswordClose" runat="server" LabelKey="aca_accountedit_password_close" /></a>
			    </div>
			    <ACA:AccelaHeightSeparate ID="sepForDesclaimer" runat="server" Height="5" />
                <span class="font14px">
                    <ACA:AccelaLabel ID="passwordRequirements" runat="server" LabelType="bodyText" IsNeedEncode="false"></ACA:AccelaLabel>
                </span>
			</div>
		</td>
	</tr>
</table>

<script language="javascript" type="text/javascript">
var lblPasswordStrength = "<%= GetTextByKey("aca_accountedit_password_strength").Replace("'","\\'")%>";
var lblPasswordWrong = "<%= GetTextByKey("aca_accountedit_password_wrong").Replace("'","\\'")%>";
var lblPasswordTooShoot = "<%= GetTextByKey("aca_accountedit_password_tooshort").Replace("'","\\'")%>";

// Change the style according to the different result;
function doChangePassworStrengthdBar(errorCode, errorMessage, pwdScore)
{
	var text = document.getElementById("validateText");
	var progressBar = document.getElementById("progressBar");
	var promptText = document.getElementById("prompt");
	var indicatorTable = document.getElementById("indicatorTable");

	text.innerHTML = errorMessage;
	var promptMessage="";
	
	switch(pwdScore)
	{
		case "3":
			progressBar.className = "ACA_Password_Weak";
			break;
		case "4":
			progressBar.className = "ACA_Password_Medium";
			break;
		case "5":
			progressBar.className = "ACA_Password_Strong";
			break;
		default:
			if(errorCode != 100)
			{
			    progressBar.className = "ACA_Password_Wrong";
			    //indicator display message
			    errorMessage = lblPasswordWrong;
			}
			else
			{
				progressBar.className = "ACA_Password_Short";
				errorMessage = lblPasswordTooShoot;
			}
	}
	//indicator display message
	text.innerHTML = errorMessage;
	//support section 508
	indicatorTable.title = errorMessage;
}

function setToDefault()
{
	var text = document.getElementById("validateText");
	var progressBar = document.getElementById("progressBar");
	var promptText = document.getElementById("prompt");
	var indicatorTable = document.getElementById("indicatorTable");

	text.innerHTML = lblPasswordStrength;
	
	progressBar.className = "ACA_Password_Default";
	text.innerHTML = lblPasswordStrength;
	indicatorTable.title = lblPasswordStrength;
}

function showRequirementLayout()
{
    $("#RequirementsLayout").show();
}

function hideRequirementLayout()
{
    $("#RequirementsLayout").hide();
}

with (Sys.WebForms.PageRequestManager.getInstance()) {
    add_pageLoaded(function () {
        setToDefault();
        
        $("#btnRequirements").popUp({
            oTarget:'RequirementsLayout',
            onShowEvent:'click',
            onHideEvent:'mouseout',
            onFocusEvent:'click',
            align:'left',
            offsetTop:3});
    });
}

</script>
