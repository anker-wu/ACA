<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalSearchCapView.ascx.cs"
    Inherits="Accela.ACA.Web.Component.GlobalSearchCapView" %>
<br />
<div>
 <div id="divCapViewTitle">
    <div class="ACA_Title_Bar">
        <div class="ACA_FLeft">
            <h1>
                <ACA:AccelaLabel ID="lblCAPViewTitle" runat="server" LabelKey="per_globalsearch_label_cap" LabelType="SectionTitleWithBar" />
            </h1>
        </div>
        <div class="ACA_FRight">
            <p>
                <ACA:AccelaDropDownList ID="ddlModule" runat="server" OnSelectedIndexChanged="ModuleDropdown_IndexChanged"
                    AutoPostBack="true" SourceType="HardCode" ToolTipLabelKey="aca_common_msg_dropdown_changedatafilter_tip">
                </ACA:AccelaDropDownList>
            </p>
        </div>
    </div>
    <span ID="lblCAPViewTitle_sub_label" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize" runat="server" ></span>
  </div>
    <input id="lblNeedReBind" type="hidden" runat="server" title="" />
        <ACA:AccelaGridView ID="gdvPermitList" GridViewNumber="60088" runat="server" AllowPaging="True" 
            SummaryKey="gdv_globalsearchcapview_permitlist_summary" CaptionKey="aca_caption_globalsearchcapview_permitlist"
            AutoGenerateCheckBoxColumn="false" AllowSorting="True" AutoGenerateColumns="False"
            PageSize="10" ShowExportLink="false" ShowCartLink="false" ShowCaption="true"
            HeaderStyle-CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize" ShowAdd2CollectionLink="false" RowStyle-CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize"
            AlternatingRowStyle-CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize" PagerStyle-HorizontalAlign="center"
            PagerStyle-VerticalAlign="bottom" OnRowCommand="PermitList_RowCommand" OnPageIndexChanging="PermitList_PageIndexChanging"
            OnRowDataBound="PermitList_RowDataBound" GlobalSearchType="CAP">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkDateHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkDateHeader" runat="server" CommandName="Header" SortExpression="CreatedDate"
                                    CausesValidation="false" LabelKey="per_permitList_Label_date">
                                </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaDateLabel ID="lblUpdatedTime" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate"
                                Text2='<%# DataBinder.Eval(Container.DataItem, "CreatedDate")%>'></ACA:AccelaDateLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="60px" />
                    <headerstyle Width="60px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPermitNumberHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkPermitNumberHeader" runat="server" CausesValidation="false"
                                    CommandName="Header" SortExpression="PermitNumber" LabelKey="per_permitList_Label_permitNumber">
                                </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <strong>
                                <ACA:AccelaLabel ID="lblPermitNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PermitNumber") %>' />
                                <asp:LinkButton ID="lnkPermitNumber" runat="server" CommandName="Action" CausesValidation="false" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PermitNumber") %>'></asp:LinkButton>
                            </strong>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="110px" />
                    <headerstyle Width="110px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPermitTypeHeader">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkPermitTypeHeader" runat="server" CausesValidation="false"
                                    CommandName="Header" SortExpression="PermitType" LabelKey="per_permitList_Label_permitType">
                                </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PermitType") %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <headerstyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAgencyHeader">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkAgencyHeader" runat="server" CausesValidation="false"
                                    CommandName="Header" SortExpression="AgencyCode" LabelKey="mycollection_detailpage_column_agencycode">
                                </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblAgencyCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AgencyCode")%>'>
                            </ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <headerstyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkModuleHeader">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkModuleHeader" runat="server" CausesValidation="false"
                                    CommandName="Header" SortExpression="ModuleName" LabelKey="per_permitList_module_name">
                                </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblModuleName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ModuleName") %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                    <headerstyle Width="50px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkDescHeader">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkDescHeader" runat="server" CausesValidation="false"
                                    CommandName="Header" SortExpression="Description" LabelKey="per_mypermitList_description">
                                </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <headerstyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPermitSearchProjectNameHeader">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkPermitSearchProjectNameHeader" CausesValidation="false"
                                    runat="server" CommandName="Header" SortExpression="ProjectName" LabelKey="per_permitList_label_permitSearchProjectName">
                                </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblProjectName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectName") %>'>
                            </ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <headerstyle Width="80px" />
                </ACA:AccelaTemplateField>
                <%-- begin address column --%>
                <ACA:AccelaTemplateField AttributeName="lnkAddressHeader">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" CausesValidation="false"
                                    CommandName="Header" SortExpression="Address" LabelKey="per_permitList_label_permitaddress">
                                </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblAddress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Address") %>'>
                            </ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <headerstyle Width="130px" />
                </ACA:AccelaTemplateField>
                <%-- end address column --%>
                <ACA:AccelaTemplateField AttributeName="lnkStatusHeader">
                    <HeaderTemplate>
                        <div style="width: auto; white-space: nowrap;" class="ACA_CapListStyle ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkStatusHeader" runat="server" CausesValidation="false"
                                    CommandName="Header" SortExpression="Status" LabelKey="per_permitList_Label_status">
                                </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="width: auto; white-space: nowrap;" class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblCapClass" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <headerstyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkRelatedRecordsHeader">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkRelatedRecordsHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="RelatedRecords"
                                LabelKey="per_permitlist_label_relatedrecords"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <asp:Literal ID="litRelatedRecords" runat="server"></asp:Literal>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="110px"/>
                    <headerstyle Width="110px"/>
                </ACA:AccelaTemplateField>
            </Columns>
            <HeaderStyle Width="100%" />
            <RowStyle Width="100%" />
        </ACA:AccelaGridView>
</div>
