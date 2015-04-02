<%@ Import namespace="System"%>
<%@ Import namespace="System.Web.UI"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.PlanList" Codebehind="PlanList.ascx.cs" %>
<%@ Register Assembly="Accela.Web.Controls" Namespace="Accela.Web.Controls" TagPrefix="ACA" %> 
<asp:HiddenField ID="txtCurrentPage" runat="server" Value="0" />
<!--<div class="ACA_TabRow  ACA_LiLeft">
            <ACA:AccelaLabel runat="server" ID="lblViewTitle"></ACA:AccelaLabel>
            <asp:DropDownList ID="ddlSelectPlan" runat="server" OnSelectedIndexChanged="SelectPlanDropdown_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
</div>-->
<h2><ACA:AccelaLabel ID="planreview_planlist_planlist" LabelKey="planreview_planlist_planlist" runat="server"></ACA:AccelaLabel></h2>
<div class="ACA_TabRow">
<ACA:AccelaGridView ID="gdvPermitList" runat="server" AllowPaging="True" AllowSorting="true" ShowCaption="true" 
    SummaryKey="gdv_planlist_permitlist_summary" CaptionKey="aca_caption_planlist_permitlist"
    AutoGenerateColumns="False" Width="738px" HeaderStyle-CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize" AlternatingRowStyle-CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize"
    RowStyle-CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize" PageSize="5" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" 
    OnPageIndexChanging="PermitList_PageIndexChanging" OnRowCommand="PermitList_RowCommand" OnRowDataBound="PermitList_RowDataBound">
    <Columns>
        <ACA:AccelaTemplateField>
            <headertemplate>
                <div class="Header_h4">
                    <ACA:GridViewHeaderLabel id="lnkPlanNameHeader" runat="server" LabelKey="planreview_planlist_planname" CausesValidation="false" CommandName="Header" SortExpression="planName" />
                </div>
            </headertemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblPlanName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "planName") %>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px"/>
            <headerstyle Width="80px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField>
            <headertemplate>
                <div class="Header_h4">
                    <ACA:GridViewHeaderLabel id="lnkFileNameHeader" runat="server" LabelKey="planreview_planlist_filename" CausesValidation="false" CommandName="Header" SortExpression="fileName" />
                </div>
            </headertemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblPlanType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "fileName") %>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="200px"/>
            <headerstyle Width="200px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField>
            <headertemplate>
                <div class="Header_h4">
                <ACA:GridViewHeaderLabel id="lnkRulesetHeader" runat="server" LabelKey="planreview_planlist_ruleset" CausesValidation="false" CommandName="Header" SortExpression="ruleset" />
                </div>
            </headertemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblRuleset" IsNeedEncode="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ruleset") %>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="150px"/>
            <headerstyle Width="150px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField>
            <headertemplate>
                <div class="Header_h4">              
                   <ACA:AccelaLabel ID="lnkReviewStatusHeader" IsGridViewHeadLabel="true" runat="server" LabelKey="planreview_planlist_reviewstatus"></ACA:AccelaLabel>                
                </div>
            </headertemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblReviewStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "reviewStatus") %>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px"/>
            <headerstyle Width="80px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField HeaderText="Submit Date" SortExpression="submitDate">
            <headertemplate>
                <div class="Header_h4">
                <ACA:GridViewHeaderLabel id="lnkSubmitDateHeader" runat="server" LabelKey="planreview_planlist_submitdate" CausesValidation="false" CommandName="Header" SortExpression="submitDate" />
                </div>
            </headertemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaDateLabel id="lblSubmitDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "submitDate")%>'></ACA:AccelaDateLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="120px"/>
            <headerstyle Width="120px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField ShowHeader="false">
            <ItemTemplate>
                <div class="ACA_SmLabel ACA_SmLabel_FontSize">
                     <ACA:AccelaLinkButton ID="btnDelete" LabelKey="ACA_AttachmentList_Label_Delete"   CausesValidation="False" CommandName="deleteRow" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "planSeq") %>'  runat="server" />
                   <ACA:AccelaLinkButton ID="btnView"  runat="server" CommandName="view" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "planSeq") %>' Visible="false"  Text="View" />
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
    </Columns>
    <EmptyDataRowStyle CssClass="table_text" />
    <PagerSettings Position="Bottom" Visible="true" Mode="numericfirstlast"  />
</ACA:AccelaGridView>
</div>