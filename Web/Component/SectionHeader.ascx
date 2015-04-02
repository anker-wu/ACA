<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SectionHeader.ascx.cs"
    Inherits="Accela.ACA.Web.Component.SectionHeader" %>
<ACA:AccelaHeightSeparate ID="hsHeight" runat="server" Height="0" />
<span id="<%=this.ClientID %>"></span>
<div runat="server" id="divSectionHeader" class="ACA_SectionHeaderTemp">
    <asp:PlaceHolder runat="server" ID="phDefaultLayout">
        <div class="ACA_Title_Bar">
            <h1>
                <span class="ACA_FLeft"><a runat="server" id="lnkCollapseOrExpand" class="SectionTextDecoration NotShowLoading" href="javascript:void(0);">
                    <img runat="server" id="imgCollapseOrExpand" class="SectionTitleArrow" />
                </a>
                    <ACA:AccelaLabel ID="lblSectionTitle" runat="server" />
                </span>
            </h1>
            <div id="divSearchArea" runat="server" class="ACA_FRight" visible="false">
                <table role="presentation" border="0" cellspacing="0" cellpadding="0" class="section_search_border">
                        <tr>
                            <td width="165px">
                                <p>
                                <asp:TextBox ID="txtSearchCondition" AutoCompleteType="Disabled" runat="server" MaxLength="200" Width="15.8em"></asp:TextBox>
                                <ACA:AccelaLabel ID="lblSearchCondition" Width="163" CssClass="gs_search_box watermark INPUT"
                                    Visible="false" LabelKey="aca_sectionsearch_label_search" runat="server" LabelType="SimpleLabelText"></ACA:AccelaLabel>
                                    </p>
                            </td>
                            <td width="18px;">
                                <asp:ImageButton ID="btnSubmitSearch4Admin" runat="server" Visible="False" CssClass="gs_go_img"/>
                                <ACA:AccelaImageButton ID="btnSubmitSearch" runat="server" OnClick="SearchButton_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            <span class="ACA_FRight">
                <asp:PlaceHolder runat="server" ID="phSectionHeaderTemplate" />
            </span>
            </div>
        <asp:Label ID="lblSectionTitle_sub_label" runat="server" 
            HiddenCssClass="ACA_Hide" ShownCssClass="ACA_Section_Instruction ACA_Section_Instruction_FontSize"></asp:Label>       
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phCustomizedLayout">
        <div>
            <ACA:AccelaLabel ID="lblCustomizedLayoutSectionTitle" runat="server" />
        </div>
    </asp:PlaceHolder>
    <asp:Button ID="btnSearch" CssClass="ACA_Hide" OnClick="SearchButton_Click" runat="server" />
</div>
<script type="text/javascript">
    var searchWaterMark = '<%= GetWaterMark() %>';

    function <%=ClientID %>_Keydown(event){
        var e = event || window.event || arguments.callee.caller.arguments[0];
        if(e && e.keyCode == 13){
            __doPostBack('<%=btnSearch.UniqueID %>', '');
        }
    }   

    function <%=ClientID %>_CleanText(obj){
        if(obj.value == searchWaterMark){
            obj.value = "";
        }
         obj.className = "gs_search_box INPUT";
    }
    
    function <%=ClientID %>_SearchBlur(obj){
        if(obj.value.trim() == "" && event.keyCode != 13)
        {
            obj.value = searchWaterMark;
            obj.className = "gs_search_box watermark INPUT";
        }
    }
</script>
