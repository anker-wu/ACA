<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalSearchAPOView.ascx.cs"
    Inherits="Accela.ACA.Web.Component.GlobalSearchAPOView" %>

<script language="javascript" type="text/javascript">
    function switchGridView(isInitial) {
        var objListByParcel = document.getElementById("divListByParcel");
        var objListByAddress = document.getElementById("divListByAddress");
        
        if (isInitial) {
            objListByParcel.style.display = "none";
            objListByAddress.style.display = "block";
        }
        else {
            objListByParcel.style.display = objListByParcel.style.display == "block" ? "none" : "block";
            objListByAddress.style.display = objListByAddress.style.display == "block" ? "none" : "block";
        }
    }
</script>

<br />
<div>
 <div id="divSectionTitle">
    <div class="ACA_Title_Bar">
        <div class="ACA_FLeft">
            <h1>
                <ACA:AccelaLabel ID="lblAPOViewTitle" runat="server" LabelKey="per_globalsearch_label_apo" LabelType="SectionTitleWithBar"/>
            </h1>
        </div>
        <div class="ACA_FRight">
            <p>
                <ACA:AccelaDropDownList ID="ddlAPOSearchType" runat="server" OnSelectedIndexChanged="APOSearchType_IndexChanged"
                    AutoPostBack="true" SourceType="Database" ToolTipLabelKey="aca_common_msg_dropdown_changedatafilter_tip">
                </ACA:AccelaDropDownList>
            </p>
        </div>
    </div>
    <span ID="lblAPOViewTitle_sub_label" runat="server" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize"></span>
 </div>
    <div id="divListByParcel" style="display: block">
        <ACA:AccelaGridView ID="gdvAPOListByParcel" runat="server" ShowExportLink="true" 
            SummaryKey="gdv_globalsearchapoview_apoparcellist_summary" CaptionKey="aca_caption_globalsearchapoview_apoparcellist"
            GlobalSearchType="PARCEL" AutoGenerateColumns="false" GridViewNumber="60090"
            ShowCaption="true" AllowSorting="True" AllowPaging="true" RowStyle-CssClass=""
            PagerStyle-HorizontalAlign="center" OnRowDataBound="APOList_RowDataBound" OnPageIndexChanging="APOList_PageIndexChanging"
            OnRowCommand="APOList_RowCommand">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkParcelNumberHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkParcelNumberHeader" runat="server" SortExpression="ParcelNumber"
                                LabelKey="APO_refAddressesList_ParcelNumber_Header"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemStyle Width="128px" />
                    <headerstyle Width="128px" />
                    <ItemTemplate>
                        <div>
                            <asp:HyperLink ID="hlParcelNumber" runat="server"><strong><ACA:AccelaLabel ID="IblParcelNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ParcelNumber")%>' /></strong></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField ColumnId="Owner" AttributeName="lnkOwnerHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:AccelaLabel ID="lnkOwnerHeader" IsGridViewHeadLabel="true" runat="server" LabelKey="APO_refAddressesList_Owner_Header"></ACA:AccelaLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemStyle Width="128px" />
                    <HeaderStyle Width="128px" />
                    <ItemTemplate>
                        <div>
                            <asp:HyperLink ID="hlOwnerName" runat="server"><strong><ACA:AccelaLabel ID="IblOwnerName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OwnerName")%>' /></strong></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddressHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:AccelaLabel ID="lnkAddressHeader" runat="server" IsGridViewHeadLabel="true" LabelKey="APO_refAddressesList_Address_Header"></ACA:AccelaLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemStyle Width="268px" />
                    <headerstyle Width="268px" />
                    <ItemTemplate>
                        <div>
                            <asp:HyperLink ID="hlAddressDescription" runat="server"><strong><ACA:AccelaLabel ID="IblAddressDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AddressDescription")%>' /></strong></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </div>
    <div id="divListByAddress" style="display: block">
        <ACA:AccelaGridView ID="gdvAPOListByAddress" runat="server" ShowExportLink="true"
         SummaryKey="gdv_globalsearchapoview_apoaddresslist_summary" CaptionKey="aca_caption_globalsearchapoview_apoaddresslist"
            GlobalSearchType="ADDRESS" AutoGenerateColumns="false" GridViewNumber="60093"
            ShowCaption="true" AllowSorting="True" AllowPaging="true" RowStyle-CssClass=""
            PagerStyle-HorizontalAlign="center"  OnRowDataBound="APOList_RowDataBound" OnPageIndexChanging="APOList_PageIndexChanging"
            OnRowCommand="APOList_RowCommand">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkAddressHeader">
                    <HeaderTemplate>
                    <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" SortExpression="AddressDescription"
                                LabelKey="APO_refAddressesList_Address_Header"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemStyle Width="268px" />
                    <headerstyle Width="268px" />
                    <ItemTemplate>
                        <div>
                            <asp:HyperLink ID="hlAddressDescription" runat="server"><strong><ACA:AccelaLabel ID="IblAddressDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AddressDescription")%>'/></strong></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkParcelNumberHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:AccelaLabel ID="lnkParcelNumberHeader" runat="server" IsGridViewHeadLabel="true" LabelKey="APO_refAddressesList_ParcelNumber_Header"></ACA:AccelaLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemStyle Width="128px" />
                    <headerstyle Width="128px" />
                    <ItemTemplate>
                        <div>
                            <asp:HyperLink ID="hlParcelNumber" runat="server"><strong><ACA:AccelaLabel ID="IblParcelNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ParcelNumber")%>' /></strong></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField ColumnId="Owner" AttributeName="lnkOwnerHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:AccelaLabel ID="lnkOwnerHeader" runat="server" IsGridViewHeadLabel="true" LabelKey="APO_refAddressesList_Owner_Header"></ACA:AccelaLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemStyle Width="128px" />
                    <HeaderStyle Width="128px" />
                    <ItemTemplate>
                        <div>
                            <asp:HyperLink ID="hlOwnerName" runat="server"><strong><ACA:AccelaLabel ID="IblOwnerName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OwnerName")%>' /></strong></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </div>
</div>
