<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.CAPs2MyCollection" Codebehind="CAPs2MyCollection.ascx.cs" %>
<script type="text/javascript" language="javascript">
    function AdjustCollectionNameStyle_<%=ClientID %>()
    {
        if(isFireFox())
        {
            return;
        }
        
        var tdEmpty = $get("<%=tdEmpty.ClientID %>");
        var txtName = $get("<%=txtName.ClientID %>");
        var txtNameError = document.getElementById("<%=txtName.ClientID %>_err_indicator");
        
        if(tdEmpty == null)
        {
            return;
        }
        
        if(txtName == null || txtNameError == null)
        {
            tdEmpty.style.display = 'block';
        }
        else if(txtNameError.style.display == '' && GetValue(txtName) == "" ) {
            tdEmpty.style.display = 'none';
        }
        else
        {
            tdEmpty.style.display = 'block';
        }
    }
    
   
    function EscapePopupWindowBykey_<%=btnCancel.ClientID%>(e)
    {
       if (e.keyCode == 27 && addCollectionClickElement != null) 
       {
         addCollectionClickElement.focus();
         addCollectionClickElement = null;
         ExitAddForm_<%=btnCancel.ClientID %>(document.getElementById('<%= btnCancel.ClientID%>'));
       }
    }

</script> 
<asp:UpdatePanel ID="updatePanelAdd" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<table role='presentation' onkeydown="javascript:EscapePopupWindowBykey_<%=btnCancel.ClientID%>(event);">
    <tr>
        <td><ACA:AccelaRadioButton ID="rdoExistCollection" GroupName="collection" CssClass="ACA_Collection_RdoButton ACA_Collection_RdoButton_FontSize" runat="server" /></td>
        <td><ACA:AccelaLabel ID="lblExistingCollection" AssociatedControlID="rdoExistCollection" LabelKey="mycollection_addpage_lablel_exsisting"  CssClass="ACA_Collection_RdoButton font13px" runat="server">
        </ACA:AccelaLabel></td>
    </tr>
    <tr>
        <td style="padding-bottom:6px;"></td><td style="padding-bottom:6px;"><ACA:AccelaDropDownList ID="ddlMyCollection" Width="16.3em" runat="server" AutoPostBack="false"></ACA:AccelaDropDownList></td></tr>
    <tr>
        <td><ACA:AccelaRadioButton ID="rdoNewCollection"  GroupName="collection" CssClass="ACA_Collection_RdoButton ACA_Collection_RdoButton_FontSize"  runat="server" /></td>
        <td><ACA:AccelaLabel ID="lblNewCollection" AssociatedControlID="rdoNewCollection" LabelKey="mycollection_addpage_lablel_new"  CssClass="ACA_Collection_RdoButton font13px" runat="server">
        </ACA:AccelaLabel></td> 
    </tr>
    <tr>
        <td id="tdEmpty" runat="server">&nbsp;</td>
        <td colspan="2"><ACA:AccelaTextBox ID="txtName"  Validate="required" runat="server"  Width="16em" MaxLength="50"  LabelKey="mycollection_renamepage_label_name"></ACA:AccelaTextBox></td>
    </tr>
    <tr>
        <td>&nbsp;&nbsp;&nbsp;</td><td style="padding-bottom:6px;"><ACA:AccelaTextBox ID="txtDesc" runat="server"  Height="7em" Width="16em"  LabelKey="mycollection_addpage_lablel_desription"
            TextMode="MultiLine" MaxLength="255"  Validate="MaxLength"></ACA:AccelaTextBox></td>
    </tr>
    <tr>
        <td></td>
        <td>
            <table role='presentation'>
                <tr>
                    <td>
                        <div class="ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaLinkButton ID="btnAdd" runat="server"  LabelKey="mycollection_addpage_lablel_add" CausesValidation="false" OnClick="AddButton_Click">
                            </ACA:AccelaLinkButton>
                        </div>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <div class="ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaLinkButton ID="btnCancel" runat="server"  LabelKey="mycollection_addpage_lablel_cancel">
                            </ACA:AccelaLinkButton>
                        </div>
                   </td>
                </tr>
            </table>
         </td>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>


<script language="javascript" type="text/javascript"> 

    function rdoCollectionClick_<%=rdoExistCollection.ClientID %>(obj)
    { 
        document.getElementById("<%=txtName.ClientID %>").disabled = true;
        document.getElementById("<%=txtDesc.ClientID %>").disabled = true;
        document.getElementById("<%=ddlMyCollection.ClientID %>").disabled = false; 
        ddlMyCollectionChanged_<%=ddlMyCollection.ClientID %>(); 
     }
     
     function rdoCollectionClick_<%=rdoNewCollection.ClientID %>(obj)
     {
        document.getElementById("<%=txtName.ClientID %>").disabled = false;
        document.getElementById("<%=txtDesc.ClientID %>").disabled = false;
        
        if (document.getElementById("<%=ddlMyCollection.ClientID %>") != null)
        {
            document.getElementById("<%=ddlMyCollection.ClientID %>").disabled = true; 
        }
        
        collectionNameChanged_<%=txtName.ClientID %>();
     }
     
     //Existing collection changed.
    function ddlMyCollectionChanged_<%=ddlMyCollection.ClientID %>()
    {
        var value = document.getElementById("<%=ddlMyCollection.ClientID %>").value;

        var btnAdd = document.getElementById("<%=btnAdd.ClientID %>");
        
        if(value == '')
        {
            DisableButton(btnAdd.id, true);
            
            btnAdd.parentNode.className = "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize";
        }
        else
        {
            DisableButton(btnAdd.id, false);
            
            btnAdd.parentNode.className = "ACA_LgButton ACA_LgButton_FontSize";
        }
    }

    //Collection name changed event.
    function collectionNameChanged_<%=txtName.ClientID %>()
    {
        var value = GetValueById("<%=txtName.ClientID %>").replace(/(^\s*)|(\s*$)/g, "");

        var btnAdd = document.getElementById("<%=btnAdd.ClientID %>");

        if(value == '')
        {
            DisableButton(btnAdd.id, true);
            
            btnAdd.parentNode.className = "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize";
        }
        else
        {
            DisableButton(btnAdd.id, false);
            
            btnAdd.parentNode.className = "ACA_LgButton ACA_LgButton_FontSize";
        }
    }
    
    ///When current record is last record after enter tab key then focus current obj
    function focusCurrentObj(e) 
    {
      if (!e.shiftKey && e.keyCode == 9) 
      {
        if (window.event) 
        {
            window.event.returnValue = false;
        }
        else 
        {
            e.preventDefault();  //for firefox
        }
        
        ExitAddForm_<%=btnCancel.ClientID %>(document.getElementById('<%= btnCancel.ClientID%>'))
      }
    }
    
    function CloseAddForm_<%=btnAdd.ClientID %>(obj)
    { 
        var nameValue = GetValueById("<%=txtName.ClientID %>").replace(/(^\s*)|(\s*$)/g, ""); 
        var desValue = GetValueById("<%=txtDesc.ClientID %>").replace(/(^\s*)|(\s*$)/g, "");
        
        if(document.getElementById("<%=rdoExistCollection.ClientID %>")!=null)
        {
            var collectionValue = document.getElementById("<%=ddlMyCollection.ClientID %>").value;
            var idRdoExistCollectionChecked = document.getElementById("<%=rdoExistCollection.ClientID %>").checked;
            if(idRdoExistCollectionChecked)
            {
                if(collectionValue == "")
                {
                    return false;
                }
            }
            else
            {
                if(nameValue =="" || nameValue.length > 50 || desValue.length > 255)
                {
                    return false;
                }
            }
        }
        else
        {
            if(nameValue =="" || nameValue.length > 50 || desValue.length > 255)
            {
                return false;
            }
        } 

        var divCollection = obj.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
        document.getElementById(divCollection.id).style.display='none';
        return true;
    }
    
    function ExitAddForm_<%=btnCancel.ClientID %>(obj)
    {
        var divCollection = obj.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
        document.getElementById(divCollection.id).style.display='none';
        // This param is define in MyCollectionMethods.js, it's use for focus current link after pop up window closed.
        if (addCollectionClickElement != null)
        {
            try{
                addCollectionClickElement.focus();
            }
            catch(e){
            }

            addCollectionClickElement = null;
        }
    }
   
  
    function clearNameDefaultValue()
    {
        var nameValue = GetValueById("<%=txtName.ClientID %>").replace(/(^\s*)|(\s*$)/g, "");
        if(nameValue=="name")
        {
            SetValueById("<%=txtName.ClientID %>","");
        } 
    }
    
    
    //Set add button disabled when first open this form.
    function SetAddButtonDiabled_<%=btnAdd.ClientID %>()
    {
        var btnAdd = document.getElementById("<%=btnAdd.ClientID %>");

        DisableButton(btnAdd.id, true);

        btnAdd.parentNode.className = "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize";
    }
    
     <% if (!AppSession.IsAdmin)
      {%>
           SetAddButtonDiabled_<%=btnAdd.ClientID %>();
      <% 
     }%> 
</script>
