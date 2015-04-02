<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.CollectionEdit" Codebehind="CollectionEdit.ascx.cs" %>
<asp:UpdatePanel ID="updatePanelCollectionEdit" runat="server" UpdateMode="Conditional">
<ContentTemplate>
 <table role='presentation'>
    <tr>
        <td><div class="Header_h2"><ACA:AccelaLabel ID="lblRenameCollection"  runat="server" LabelKey="mycollection_renamepage_label_rename"></ACA:AccelaLabel></div></td>
    </tr> 
    <tr>
        <td class="ACA_TabRow"><ACA:AccelaTextBox ID="txtName" runat="server" Validate="required" Width="16em" MaxLength="50" LabelKey="mycollection_renamepage_label_name" CssClass="ACA_SmLabel ACA_SmLabel_FontSize"></ACA:AccelaTextBox></td></tr>
    <tr>
        <td><ACA:AccelaTextBox ID="txtDesc" runat="server" Rows="4" Width="16em"  CssClass="ACA_SmLabel ACA_SmLabel_FontSize" LabelKey="mycollection_addpage_lablel_desription"
            TextMode="MultiLine" MaxLength="255"  Validate="MaxLength"></ACA:AccelaTextBox></td>
    </tr>
    <tr>
        <td>
            <table role='presentation'>
                <tr>
                    <td>
                        <ACA:AccelaButton ID="btnChange" runat="server" OnClientClick="return CloseChangeForm(this,'add')" LabelKey="mycollection_renamepage_label_change" OnClick="ChangeButton_Click"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize">
                        </ACA:AccelaButton>
                    </td>
                    <td>&nbsp;</td>
                    <td> 
                        <ACA:AccelaButton ID="btnCancel" runat="server" OnClientClick="CloseChangeForm(this,'cancel');return false;" LabelKey="mycollection_addpage_lablel_cancel"
                         DivEnableCss="ACA_LgButton ACA_LgButton_FontSize">
                        </ACA:AccelaButton>
                    </td>
                </tr>
            </table>
         </td>
    </tr> 
</table>
</ContentTemplate>
</asp:UpdatePanel>
<script language="javascript" type="text/javascript">
    function FocusNameTextBox_<%=ClientID %>()
    {
        var txtCollectionName = document.getElementById("<%=txtName.ClientID %>");
        
        if(txtCollectionName != null && txtCollectionName != "undefined")
        {
            txtCollectionName.focus();
        }
    }
    
    var popErrIconWidth;// if err indicator is pop up. definded collection edit control width.
    function AdjustCollectionEditStyle_<%=ClientID %>()
    {
        if(isFireFox())
        {
            return;
        }
        
        var txtName = $get("<%=txtName.ClientID %>");
        var txtNameError = document.getElementById("<%=txtName.ClientID %>_err_indicator");
        var collectionEdit = document.getElementById("divCollectionEdit");
        
        if(collectionEdit == null || txtName == null || txtNameError == null)
        {
            return;
        }
        
		//err indicator is pop up.
        if(txtNameError.style.display == '' && GetValue(txtName) == "" )
        {
            //at first, when pop up error indicator icon. setting pop_err_width.
            if(popErrIconWidth==null)
            {
                popErrIconWidth = collectionEdit.offsetWidth + 20;
            }
            
            collectionEdit.style.width = popErrIconWidth;
        }
        else
        {
            if(popErrIconWidth!=null)
            {
                collectionEdit.style.width = popErrIconWidth-20;
            }
        }
    }
    
      //Close form which add caps to my collection.
    function CloseChangeForm(obj,actionFlag)
    {  
        if(actionFlag == 'add')
        {
            var nameValue = GetValueById("<%=txtName.ClientID %>").replace(/(^\s*)|(\s*$)/g, "");
            var desValue = GetValueById("<%=txtDesc.ClientID %>").replace(/(^\s*)|(\s*$)/g, "");
            if(nameValue =="" || nameValue.length > 50 || desValue.length > 255)
            {
                return false;
            }
        }
        var divCollectionEdit = document.getElementById("divCollectionEdit");
        divCollectionEdit.style.display = 'none';
        return true;
    } 
    
     //Collection name changed event.
    function collectionNameChanged()
    {
        var value = GetValueById("<%=txtName.ClientID %>").replace(/(^\s*)|(\s*$)/g, "");

        var btnChange = document.getElementById("<%=btnChange.ClientID %>");

        if (value == '')
        {
            DisableButton(btnChange.id, true);
            
            btnChange.parentNode.className = "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize";
        }
        else
        {
            DisableButton(btnChange.id, false);
            
            btnChange.parentNode.className = "ACA_LgButton ACA_LgButton_FontSize";
        }
    }
</script>
