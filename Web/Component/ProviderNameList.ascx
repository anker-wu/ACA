<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.ProviderNameList" Codebehind="ProviderNameList.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>

<div style="padding-bottom: 6px; width: 240px;">
    <div style="padding-bottom: 4px">
        <table role='presentation' width="100%">
            <tr>
                <td>
                    <ACA:AccelaTextBox ID="txtProviderName" runat="server" CssClass="ACA_NLonger" LabelKey="education_searchform_providername_label">
                    </ACA:AccelaTextBox>
                </td>
                <td valign="top">
                    <img id="imgProviderList" onclick='CloseProviderListForm(this)' style="cursor: pointer;" alt="<%=GetTextByKey("img_alt_provider_list_button") %>"
                        src="<%=ImageUtil.GetImageURL("close.png") %>" />
                </td>
            </tr> 
        </table>
    </div> 
    <div id="divProviderName" class="ACA_SearchListForm4Education"> 
    </div>
</div>

<script language="javascript" type="text/javascript">
    var providerNames;
    
    // display provider names.
    function DisplayProviderNames(value)
    {
        providerNames = value.split('\f');

        // sort provider name list.
        providerNames.sort();
        
        CreateProviderHTML(providerNames);
    }
    
    // filter provider information by provider name.
    function FilterProviders()
    {   
        if(providerNames == null)
        {
            return;
        } 
        
        var providerName = GetValueById("<%=txtProviderName.ClientID %>");
        
        if (providerName == '')
        {
            CreateProviderHTML(providerNames);
        }
        else
        {
            var newProvdierNames = new Array(); 
            
            for(var i=0;i<providerNames.length;i++)
            {   
                if (providerNames[i].toLowerCase().indexOf(providerName.toLowerCase()) != -1)
                {
                    newProvdierNames[newProvdierNames.length] = providerNames[i];
                }
            }
             
            CreateProviderHTML(newProvdierNames);
        }
    }
    
    // create provider HTML by providers.
    function CreateProviderHTML(providers)
    {
        var tb = CreateTable(providers,'<%=FillMethodName %>'); 
        var divProviderName = document.getElementById("divProviderName");
        
        for (var j=0; j<divProviderName.childNodes.length; j++)
        {
            divProviderName.removeChild(divProviderName.childNodes[j]);
        }
        
        divProviderName.appendChild(tb);
    }
    
    // close provider list form.
    function CloseProviderListForm(obj)
    {   
        var provider = obj.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
        $get(provider.id).style.display = 'none';
    }
    
</script>