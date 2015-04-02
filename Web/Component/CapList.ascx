<%@ Import namespace="System.ComponentModel"%>
<%@ Import namespace="Accela.ACA.Web.Payment"%>
<%@ Import namespace="Accela.ACA.Common.Util"%>

<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.CapList" Codebehind="CapList.ascx.cs" %>
<%@ Register Src="../Component/ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<asp:UpdatePanel ID="mapPanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<ACA:ACAMap ID="mapCapList" AGISContext="CAPList" OnShowOnMap="MapCapList_ShowACAMap" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>

<ACA:AccelaInlineScript runat="server" ID="cloneDialog">
<script type="text/javascript">
    function ShowCloneDialog(url) {
        window.ACADialog.popup({ url: url, width: 745, height: 380});
    }
</script>
</ACA:AccelaInlineScript>

<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <input id="lblNeedReBind" type="hidden" runat="server" title="" />
        <ACA:AccelaGridView ID="gdvPermitList" runat="server" AllowPaging="True" SummaryKey="gdv_caphome_caplist_summary"
        CaptionKey="aca_caption_caphome_caplist"
            AllowSorting="true" AutoGenerateColumns="False" PageSize="10" ShowExportLink="true" ShowCartLink="true" CheckBoxColumnIndex="0"
            ShowCaption="true" OnGridViewSort="PermitList_GridViewSort" ShowAdd2CollectionLink="true" BreakWord="true" ShowCloneRecordLink = "true"
            PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" OnRowCommand="PermitList_RowCommand"
            OnPageIndexChanging="PermitList_PageIndexChanging" OnPageIndexChanged="PermitList_PageIndexChanged" OnRowDataBound="PermitList_RowDataBound"
            OnGridViewDownload="PermitList_GridViewDownload">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkDateHeader" ExportDataField="Date" ExportFormat="ShortDate">
                    <headertemplate>
                        <div style="margin-right:5px;" class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkDateHeader" runat="server" CommandName="Header" SortExpression="Date" CausesValidation="false" 
                                LabelKey ="per_permitList_Label_date" >
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div style="margin-right:5px;">
                            <ACA:AccelaDateLabel id="lblUpdatedTime" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "Date")%>'></ACA:AccelaDateLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="60px"/>
                    <headerstyle Width="60px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPermitNumberHeader" ExportDataField="PermitNumber">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkPermitNumberHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="PermitNumber" 
                                LabelKey="per_permitList_Label_permitNumber" >
                             </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <strong><ACA:AccelaLabel ID="lblPermitNumber" runat ="server" Visible ="false" Text = '<%# DataBinder.Eval(Container.DataItem, "PermitNumber") %>' /></strong>
                            <asp:HyperLink ID="hlPermitNumber" runat ="server" ><strong><ACA:AccelaLabel ID="lblPermitNumber1" runat ="server" Text='<%# DataBinder.Eval(Container.DataItem, "PermitNumber") %>' /></strong></asp:HyperLink>
                            <ACA:AccelaLabel ID="lblCapID1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "capID1") %>' Visible="false" />
                            <ACA:AccelaLabel ID="lblCapID2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "capID2") %>' Visible="false" />
                            <ACA:AccelaLabel ID="lblCapID3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "capID3") %>' Visible="false" />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="110px"/>
                    <headerstyle Width="110px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPermitTypeHeader" ExportDataField="PermitType">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkPermitTypeHeader" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="PermitType" 
                                LabelKey="per_permitList_Label_permitType" >
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel  ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PermitType") %>' />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAgencyHeader" ExportDataField="agencyCode">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkAgencyHeader" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="agencyCode"
                         LabelKey="mycollection_detailpage_column_agencycode">
                          </ACA:GridViewHeaderLabel>
                            
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle" >
                            <%--use AccelaLabel control instead of asp.Label control, updated by Peter.Pan --%>
                            <ACA:AccelaLabel ID="lblAgencyCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "agencyCode")%>'>
                            </ACA:AccelaLabel> 
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkModuleHeader" ExportDataField="ModuleName">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkModuleHeader" runat="server" CausesValidation="false"
                                    CommandName="Header" SortExpression="ModuleName" LabelKey="per_permitList_module_name">
                                </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblModuleName" IsNeedEncode="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ModuleName") %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle  Width="130px"/>
                    <HeaderStyle  Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkDescHeader" ExportDataField="Description">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkDescHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="Description" 
                                LabelKey="per_mypermitList_description" >
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle">
                            <%--use AccelaLabel control instead of asp.Label control, updated by Peter.Pan --%>
                            <ACA:AccelaLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle  Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPermitSearchProjectNameHeader" ExportDataField="ProjectName">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkPermitSearchProjectNameHeader"  CausesValidation="false" runat="server" CommandName="Header" SortExpression="ProjectName" 
                                LabelKey="per_permitList_label_permitSearchProjectName">
                            </ACA:GridViewHeaderLabel>
                            
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle" >
                            <%--use AccelaLabel control instead of asp.Label control, updated by Peter.Pan --%>
                            <ACA:AccelaLabel ID="lblProjectName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectName") %>'>
                            </ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <%-- begin address column --%>
                <ACA:AccelaTemplateField AttributeName="lnkAddressHeader" ExportDataField="Address">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="Address" 
                              LabelKey="per_permitList_label_permitaddress">
                            </ACA:GridViewHeaderLabel>
                            
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle" >
                            <ACA:AccelaLabel ID="lblAddress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Address") %>'>
                            </ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <%-- end address column --%>
                <%-- begin trade name column --%>
                <ACA:AccelaTemplateField AttributeName="lnkEnglishNameHeader" ExportDataField="EnglishTradeName">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkEnglishNameHeader" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="EnglishTradeName" 
                                LabelKey="per_permitList_label_permitenglishtradename">
                            </ACA:GridViewHeaderLabel>
                            
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle" >
                            <ACA:AccelaLabel ID="lblEnglishTradeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EnglishTradeName") %>'>
                            </ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkArabicNameHeader" ExportDataField="ArabicTradeName">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row" style="text-align:right;">
                        <ACA:GridViewHeaderLabel ID="lnkArabicNameHeader" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="ArabicTradeName" 
                                LabelKey="per_permitList_label_permitarabictradename" >
                            </ACA:GridViewHeaderLabel>
                            
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle" style="text-align:right;" >
                            <ACA:AccelaLabel ID="lblArabicTradeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ArabicTradeName") %>'>
                            </ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <%-- end trade name column --%>
                <ACA:AccelaTemplateField AttributeName="lnkExpirationDateHeader" ExportDataField="ExpirationDate" ExportFormat="ShortDate">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkExpirationDateHeader" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="ExpirationDate" 
                                LabelKey="per_permitList_label_permitSearchExpirationDate">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle" >
                            <ACA:AccelaDateLabel id="lblExpirationDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "expirationDate") %>'></ACA:AccelaDateLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField> 
                <ACA:AccelaTemplateField AttributeName="lnkRelatedRecordsHeader" ExportDataField="RelatedRecords">
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
                <ACA:AccelaTemplateField AttributeName="lnkCreatedByHeader" ExportDataField="CreatedBy">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkCreatedByHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="CreatedBy" LabelKey="aca_cap_list_created_by"></ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel  ID="lblCreatedBy" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CreatedBy") %>' />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStatusHeader" ExportDataField="Status">
                    <headertemplate>
                        <div style="white-space:nowrap;" class="ACA_CapListStyle ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkStatusHeader" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="Status" 
                            LabelKey="per_permitList_Label_status">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div style="white-space:nowrap;" class="ACA_CapListStyle">
                            <asp:Panel ID="panelStatus" runat="server"><ACA:AccelaLabel ID="lblStatus" runat="server" IsNeedEncode="false" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>' /></asp:Panel>
                            <asp:Panel ID="panelbtnRenewalDetail" runat="server">
                                <a id="lnkRenewalDetail" runat="server" Visible="False">
                                    <ACA:AccelaLabel ID="lblRenewalDetail" runat="server" IsNeedEncode="false" Visible="False" />
                                </a>
                            </asp:Panel>
                        </div>
                    </itemtemplate>
                    <itemstyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCapClassHeader" ExportDataField="CapClass">
                    <headertemplate>
                        <div style="white-space:nowrap;" class="ACA_CapListStyle ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkCapClassHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="CapClass" 
                            LabelKey="per_permitlist_label_capclass">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div style="white-space:nowrap;" class="ACA_CapListStyle">
                            <asp:Panel ID="panelCapClass" runat="server"><ACA:AccelaLabel ID="lblCapClass" runat="server" IsNeedEncode="false" Text='<%# DataBinder.Eval(Container.DataItem, "CapClass") %>' /></asp:Panel>
                        </div>
                    </itemtemplate>
                    <itemstyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkActionsHeader">
                    <headertemplate>
                        <div style="white-space:nowrap;" class="ACA_CapListStyle ACA_Header_Row">
                            <ACA:AccelaLabel IsGridViewHeadLabel="true" ID="lnkActionsHeader" runat="server" LabelKey="per_permitList_Label_action" IsNeedEncode="false"></ACA:AccelaLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div style="white-space:nowrap;" class="ACA_CapListStyle">
                            <asp:Panel ID="Panel2" runat="server"><ACA:AccelaLinkButton ID="btnStatus" runat="server" CommandName="Action"  CausesValidation="false" Visible="false"></ACA:AccelaLinkButton></asp:Panel>
                            <asp:Panel ID="Panel3" runat="server"><ACA:AccelaLinkButton ID="btnFeeStatus" runat="server" CommandName="Action" CausesValidation="false" Visible="false"></ACA:AccelaLinkButton></asp:Panel>
                            <asp:Panel ID="Panel4" runat="server"><ACA:AccelaLinkButton ID="btnRenewalStatus" runat="server" CommandName="RenewalAction" CausesValidation="false" Visible="false"></ACA:AccelaLinkButton></asp:Panel>
                            <asp:Panel ID="Panel5" runat="server"><ACA:AccelaLinkButton ID="btnRequestTradeLic" runat="server" CommandName="Action"  CausesValidation="false" Visible="false"></ACA:AccelaLinkButton></asp:Panel>
                            <ACA:AccelaLinkButton ID="btnCreateAmendment" runat="server" CommandName="Action"  CausesValidation="false" Visible="false"></ACA:AccelaLinkButton>
                        </div>
                    </itemtemplate>
                    <itemstyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField ShowHeader="false" AttributeName="lnkPermitAddressHeader" ExportDataField="PermitAddress">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkPermitAddressHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="PermitAddress" 
                                LabelKey="per_permitList_Label_permitAddress" >
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel  ID="lblPermitAddress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PermitAddress") %>' />
                        </div>
                    </itemtemplate>
                    <headerstyle CssClass="ACA_Hide"/>
                    <ItemStyle CssClass="ACA_Hide"/>
                    <FooterStyle CssClass="ACA_Hide" />
                </ACA:AccelaTemplateField>
                </Columns>
            </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>

<%if (PaymentHelper.GetPaymentAdapterType() == PaymentAdapterType.Etisalat)
  { %>
<script type="text/javascript">
    function AdjustHeight()
    {
        if(window.parent!=window)
        {
            var ifrm = getParentDocument().getElementById("ACAFrame");
            var h="600";
            if(ifrm != null && ifrm.contentWindow==window)
            {
                if(isFireFox() == false)
                {
                    ifrm.style.height = ifrm.parentNode.style.height = h +"px";
                }
                else
                {
                    ifrm.height = h;
                }
            }
        }
        return false;
    }
    window.onunload = AdjustHeight;
</script>
<%} %>
