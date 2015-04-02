<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.AddressList" Codebehind="AddressList.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common.Common"  %>
<%@ Register src="ServiceControl.ascx" tagname="ServiceControl" tagprefix="ACA" %>
<div id="divAddressResult" runat="server" visible="false">
    <div id="divList">
        <div class="ACA_TabRow">
            &nbsp;</div>
        <div id="divAddressPan" runat="server">
            <div class="ACA_SmLabel ACA_SmLabel_FontSize">
                <div id="divForAdminShowAddress" runat="server" visible="false">
                    <ACA:AccelaLabel ID="lblSelectAddressAdmin" runat="server" LabelKey="superAgency_workLocation_label_selectAddress"></ACA:AccelaLabel>
                    <ACA:AccelaLabel ID="lblAddressAppendPrefix" runat="server" LabelKey="superAgency_workLocation_label_addressFound"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaLabel ID="lblSelectAddress" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
            </div>
            <div id="divAddressList" runat="server">
                <ACA:AccelaGridView ID="gvAddress" runat="server" IsInSPEARForm="true" 
                SummaryKey="gdv_addressedit_addresslist_summary" CaptionKey="aca_caption_addressedit_addresslist"
                    AllowPaging="true" AllowSorting="true" OnRowDataBound="Address_RowDataBound"
                    PagerStyle-HorizontalAlign="center" OnRowCommand="Address_RowCommand"
                    ShowCaption="true" Visible="false" GridViewNumber="60108" OnPageIndexChanging="Address_PageIndexChanging" OnGridViewSort="Address_GridViewSort">
                    <Columns>
                        <ACA:AccelaTemplateField AttributeName="lnkAddressHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" CommandName="Header"
                                        SortExpression="FullAddress" MaxLength="1024" LabelKey="per_WorkLocation_AddressList_Address"
                                        CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblAddress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FullAddress") %>'></ACA:AccelaLabel>                                
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="160px" />
                            <HeaderStyle Width="160px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkDescriptionHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkDescriptionHeader" runat="server" CommandName="Header" SortExpression="addressDescription"
                                        LabelKey="aca_worklocation_adresslist_description" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "addressDescription") %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <HeaderStyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkCityHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkCityHeader" runat="server" CommandName="Header" SortExpression="City"
                                        LabelKey="per_WorkLocation_AdressList_City" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "City") %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <HeaderStyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkStateHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkStateHeader" runat="server" CommandName="Header"
                                        SortExpression="State" LabelKey="per_WorkLocation_AdressList_State" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_XShoter">
                                    <ACA:AccelaLabel ID="lblState" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "State") %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                            <HeaderStyle Width="15px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkZipHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkZipHeader" runat="server" CommandName="Header" SortExpression="Zip"
                                        LabelKey="per_WorkLocation_AdressList_Zip" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblZip" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Zip") %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <HeaderStyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkParcelHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkParcelHeader" runat="server" CommandName="Header"
                                        SortExpression="ParcelNumber" LabelKey="per_WorkLocation_AdressList_Parcel" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblParcel" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ParcelNumber") %>'></ACA:AccelaLabel>
                                    <ACA:AccelaLabel ID="lblRefParcelSequenceNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ParcelSequenceNumber") %>'
                                        Visible="false"></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="130px" />
                            <HeaderStyle Width="130px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField ColumnId="Owner" AttributeName="lnkOwnerHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkOwnerHeader" runat="server" CommandName="Header"
                                        SortExpression="OwnerFullName" LabelKey="per_parcel_table_header_owner" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblOwner" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OwnerFullName") %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="160px" />
                            <HeaderStyle Width="160px" />
                            </ACA:AccelaTemplateField>
                            <ACA:AccelaTemplateField AttributeName="lnkActionsHeader">
                                <headertemplate>
                                    <div>
                                        <ACA:AccelaLabel IsGridViewHeadLabel="true" ID="lnkActionsHeader" runat="server" LabelKey="per_permitList_Label_action" IsNeedEncode="false"></ACA:AccelaLabel>
                                    </div>
                                </headertemplate>
                                <itemtemplate>
                                    <div>
                                        <ACA:AccelaLinkButton ID="lnkGetService" runat="server" CommandName="GetService" LabelKey="aca_worklocation_adresslist_select"  CausesValidation="false"></ACA:AccelaLinkButton>
                                    </div>
                                </itemtemplate>
                                <itemstyle Width="80px"/>
                                <headerstyle Width="80px"/>
                            </ACA:AccelaTemplateField>
                    </Columns>
                </ACA:AccelaGridView>
            </div>
        </div>
        <div class="ACA_TabRow">
            &nbsp;</div>
            <ACA:ServiceControl ID="serviceControl" runat="server" />
    </div>
    <div class="ACA_TabRow">
        &nbsp;</div>
</div>

