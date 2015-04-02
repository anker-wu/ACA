<%@ Import namespace="System.ComponentModel"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.MyCollectionSummary" Codebehind="MyCollectionSummary.ascx.cs" %>
<%@ Register src="CollectionEdit.ascx" tagname="CollectionEdit" tagprefix="uc1" %> 
<script language="javascript" type="text/javascript">

    var isRightOrLeft = '<%=IsRightToLeft %>';
    
    function DisplayCollectionEditForm(e)
    {
       var obj = e.srcElement? e.srcElement : e.target;
        
       var divCollectionEdit = window.document.getElementById("divCollectionEdit");
       
       if(divCollectionEdit==null)
       {
            return;
       }
       
       divCollectionEdit.style.display = "block";
       if(isRightOrLeft == "True")
       {
          divCollectionEdit.style.left =  (getElementLeft(obj)-8)+ "px";
       }
       else
       {
          divCollectionEdit.style.left =  (getElementLeft(obj)-divCollectionEdit.offsetWidth + obj.offsetWidth) + "px";
       }
        
        
       divCollectionEdit.style.top = (getElementTop(obj)+obj.offsetHeight) + "px";
       FocusNameTextBox_<%=EditForCollection.ClientID %>();
    }  
    
    function UpdateMyCollection(renameSuccessMessage)
    {
        alert(renameSuccessMessage);
        window.location.reload();
    }
    
    function ShowCollectionForm()
    {
        var divCollectionEdit = window.document.getElementById("divCollectionEdit");
        divCollectionEdit.style.display = "block";
        var divAction = window.document.getElementById("divAction");
        divAction.style.display  = 'block';
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
<asp:UpdatePanel ID="updatePanelSummary" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<div class="ACA_TabRow">
    <div class="ACA_TabRow">
        <H1>
            <ACA:AccelaLabel ID="lblMyCollectionName"  runat="server" /> 
        </H1>
        <p>
            <ACA:AccelaLabel ID="lblMyCollectionDesc" runat="server" /> 
        </p>
    </div>
    <div>
         <table role='presentation' class="ACA_SmLabel ACA_SmLabel_FontSize">
            <tr>
                <td>
                    <b>
                        <ACA:AccelaLabel ID="lblTotalRecords" LabelKey="mycollection_detailpage_label_totalrecords"
                            runat="server"></ACA:AccelaLabel>&nbsp;</b></td>
                <td>
                    <b>
                        <ACA:AccelaLabel ID="lblTotalRocordsNum" runat="server"></ACA:AccelaLabel>&nbsp;</b>
                </td> 
                <td><ACA:AccelaLabel ID="lblTotalRecordsDetail" runat="server" IsNeedEncode="false"></ACA:AccelaLabel></td>
            </tr> 
        </table>
        <table role='presentation' class="ACA_SmLabel ACA_SmLabel_FontSize">
           <tr>
                <td>
                    <b>
                        <ACA:AccelaLabel ID="lblInspectionSummary" LabelKey="mycollection_detailpage_label_inspectionsummary"
                            runat="server"></ACA:AccelaLabel>&nbsp;</b></td>
                <td>
                    <b>
                        <ACA:AccelaLabel ID="lblInspectionSummaryNum" runat="server" ></ACA:AccelaLabel>&nbsp;</b></td>
                <td><ACA:AccelaLabel ID="lblInspectionSummaryDetail"  IsNeedEncode="false"  runat="server"></ACA:AccelaLabel></td>
            </tr> 
        </table>
        <table role='presentation' width="100%" class="ACA_TabRow_Line">
            <tr>
                <td class="ACA_SmLabel ACA_SmLabel_FontSize">
                    <table role='presentation'>
                        <tr><td>
                            <b><ACA:AccelaLabel ID="lblFeeSummary" LabelKey="mycollection_detailpage_label_feesummary" runat="server" />&nbsp;</b>
                            </td>
                            <td class="ACA_SmLabel"><ACA:AccelaLabel ID="lblFeeSummaryDetail"  IsNeedEncode="false"  runat="server"/></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table role='presentation' class="ACA_AlignRightOrLeft ACA_FRight">
                        <tr>
                            <td>
                                <ACA:AccelaButton ID="btnRename" DivEnableCss="ACA_SmButton ACA_LgButton ACA_LgButton_FontSize" runat="server" LabelKey="mycollection_detailpage_label_renamecollection" OnClientClick="DisplayCollectionEditForm(event);return false;">
                                </ACA:AccelaButton>     
                             </td><td>&nbsp;&nbsp;</td>
                             <td> 
                                <ACA:AccelaButton ID="btnDelete" DivEnableCss="ACA_SmButton ACA_LgButton ACA_LgButton_FontSize"  runat="server" LabelKey="mycollection_detailpage_label_delcollection" OnClick="DeleteButton_Click">
                                </ACA:AccelaButton>     
                            </td>
                        </tr>
                    </table> 
                </td>
            </tr>
        </table> 
    </div>
<div id="divAction" style="display:none;">
    <hr />
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
                 CssClass="ACA_SmLabel ACA_SmLabel_FontSize"></ACA:AccelaLinkButton>
                
            </td>
        </tr>
    </table><br /> 
</div>
<div id="divCollectionEdit"  class="ACA_RenameCollectionForm">
    <uc1:CollectionEdit ID="EditForCollection" runat="server" />
</div></div>
</ContentTemplate>
</asp:UpdatePanel>
