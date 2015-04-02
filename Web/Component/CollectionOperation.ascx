<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.CollectionOperation" Codebehind="CollectionOperation.ascx.cs" %>
<%@ Register src="CAPs2MyCollection.ascx" TagName="CAPs2MyCollection" TagPrefix="ACA" %>

<%@ import namespace="Accela.ACA.Common" %>

<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<div> 
    <div>
        <div  style="padding-bottom:6px;">
        <table role='presentation'>
            <tr>
                <td><b><ACA:AccelaLabel ID="lblModuleName" runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize"></ACA:AccelaLabel></b></td>
            </tr>
        </table></div>
        <div>
        <table role='presentation'>
            <tr>
                <td>
                    <ACA:AccelaLinkButton ID="lnkMove" runat="server" LabelKey="mycollection_detailpage_label_move"
                        CssClass="ACA_SmLabel ACA_SmLabel_FontSize"></ACA:AccelaLinkButton> 
                    </td>
                    <td class="ACA_SmLabel ACA_SmLabel_FontSize">&nbsp;|&nbsp;</td>
                    <td>
                    <ACA:AccelaLinkButton ID="lnkCopy" runat="server" LabelKey="mycollection_detailpage_label_copy"
                        CssClass="ACA_SmLabel ACA_SmLabel_FontSize"></ACA:AccelaLinkButton> 
                    </td>
                    <td  class="ACA_SmLabel ACA_SmLabel_FontSize">&nbsp;|&nbsp;</td>
                    <td>
                    <ACA:AccelaLinkButton ID="lnkRemove" runat="server" LabelKey="mycollection_detailpage_label_remove"
                     OnClick="RemoveLink_Click" CssClass="ACA_SmLabel ACA_SmLabel_FontSize"></ACA:AccelaLinkButton>  
                </td>
            </tr>
        </table></div> 
    </div>
    <div id="divCopy" runat="server"  class="ACA_Add2CollectionForm">
      <ACA:CAPs2MyCollection ID="OperateCAPs2Collection" runat="server" />  
    </div> 
    <asp:HiddenField ID="hfActionFlag" runat="server" /> 
 </div>
 </ContentTemplate>
 </asp:UpdatePanel>
 
 <script language="javascript" type="text/javascript">  
    
     var isRightOrLeft = '<%=IsRightToLeft %>';
     
    function ConfirmRemoveCap_<%=lnkRemove.ClientID %>()
    {
        //not-selected any CAP.
        var needRemove = false ;
        
        if(!IsExistCheckedBox("<%=lnkRemove.ClientID %>"))
        {
          showMessage('messageSpan', '<%=Server.HtmlEncode(GetTextByKey("mycollection_caphomepage_message_nocapselected").Replace("'", "\\'")) %>', 'Notice', true, 1);
        }
        else
        {
            if(confirm('<%=GetTextByKey("mycollection_detailpage_removecaps_message").Replace("'", "\\'") %>'))
            {
                needRemove = true;
            }
        }
        
        return needRemove; 
    } 
    
    //Check if exist checked CAP checkboxs.
    function IsExistCheckedBox(lnkRemoveId) 
    {
        var prefix = lnkRemoveId.substring(0, lnkRemoveId.indexOf("CollectionOperation"));
        var hdfItem = $get("<%=SelectedItemsFieldClientID %>");

        //hdfItem stores the checked record. 
        // value: "" means no checked records, "," means first checked, then unmarked it.
        if (hdfItem != null && hdfItem.value != "" && hdfItem.value != ",") 
        {
            return true;
        }
        
        return false;
    }
    
    function DoFocus(obj, focusElementId)
    {
        //Definded in mycollectionmethod.js
        addCollectionClickElement = obj;

        var focusElement = document.getElementById(focusElementId);
        
        if(focusElement != null)
        {
            $(focusElement).focus();
        }
    }
    
    function copy2Collection_<%=lnkCopy.ClientID %>(e, divCopyId, focusElementId)
    { 
        var obj = e.srcElement? e.srcElement : e.target;
        var divCopy = window.document.getElementById(divCopyId);
       
        if(divCopy==null)
        {
            return;
        }

        $(".ACA_Add2CollectionForm").hide();
        divCopy.style.display = "block";
        
        if(isRightOrLeft == "True")
        {
            divCopy.style.left =  (getElementLeft(obj)-(divCopy.offsetWidth-8) + obj.offsetWidth) + "px";
        }
        else
        {
            divCopy.style.left = getElementLeft(obj) + "px";
        } 
//        clearNameDefaultValue();
        divCopy.style.top = (getElementTop(obj)+obj.offsetHeight) + "px";
        
        window.document.getElementById("<%=hfActionFlag.ClientID %>").value = '<%=ACAConstant.COPY_TO %>';
        setAddedMessagePosition(obj,isRightOrLeft);
        DoFocus(obj, focusElementId);
    }  
    
    function move2Collection_<%=lnkMove.ClientID %>(e, divCopyId, focusElementId)
    {
        var obj = e.srcElement? e.srcElement : e.target;        
        var divCopy = window.document.getElementById(divCopyId);
        
        if(divCopy==null)
        {
            return;
        }

        $(".ACA_Add2CollectionForm").hide();     
        divCopy.style.display = "block";

        if(isRightOrLeft == "True")
        {
            divCopy.style.left =  (getElementLeft(obj)-(divCopy.offsetWidth-30) + obj.offsetWidth-30) + "px";
        }
        else
        {
            divCopy.style.left = getElementLeft(obj) + "px";
        } 
//        clearNameDefaultValue();
        divCopy.style.top = (getElementTop(obj)+obj.offsetHeight) + "px";

        window.document.getElementById("<%=hfActionFlag.ClientID %>").value = '<%=ACAConstant.MOVE_TO %>';
        setAddedMessagePosition(obj,isRightOrLeft);
        DoFocus(obj, focusElementId);
    }   

    function getElementTop(obj)
    {
        var top = obj.offsetTop;
        
        var parentobj = obj.offsetParent;
        while (parentobj)
        {
            if (parentobj.offsetTop == undefined) break;
            top += parentobj.offsetTop;
            parentobj = parentobj.offsetParent;
        }
        
        return top;
    }
    
    function getElementLeft(obj)
    {
        var left = obj.offsetLeft;
        
        var parentobj = obj.offsetParent;
        while (parentobj)
        {
            if (parentobj.offsetLeft == undefined) break;
            left += parentobj.offsetLeft;
            parentobj = parentobj.offsetParent;
        }
        return left;
    } 
    

    
 </script>
