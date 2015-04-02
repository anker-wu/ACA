<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.Conditions" Codebehind="Conditions.ascx.cs" %>
<div id="sepForConditionTop" runat="server" class="ConditionsTop" >&nbsp;</div>
<div id="divContainer" runat="server">
    <div id="divIcon" runat="server">
        <img id="imgIcon" runat="server" class="ACA_Hide" />
    </div>
    <div>
        <div runat="Server" id="divMsg"></div>
        <table role='presentation'>
            <tr valign="top" class='condition_notice_tr'>
                <td runat="Server" id="tdCondition"></td>
                <td runat="Server" id="tdConditionName" class='condition_notice_name'></td>
                <td runat="Server" id="tdSeverity"></td>
                <td runat="Server" id="tdStatus"></td>
            </tr>
        </table>
        <table role='presentation' border="0" cellspacing="0" cellpadding="0">
        	<tr class='condition_notice_tr'>
        		<td runat="Server" id="tdSummary"></td>
        	</tr>
        </table>
    </div>
    <div style=" clear:both">
    <a runat="Server" id="lnkShowHideCondition" href="javascript:void(0);" class="NotShowLoading" style="cursor: pointer;"></a>
    </div>
</div>

<asp:UpdatePanel ID="panConditionList" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div runat="Server" id="divConditionList"  style="display: none">
        <ACA:AccelaHeightSeparate ID="sepLineForCondition" runat="server" Height="10" />
            <ACA:AccelaLabel ID="lblConditionTitle" runat="server" LabelType="SectionTitle"/>
            <div attr='condition_notice'>
             <ACA:AccelaGridView ID="gdvConditionList" runat="server" AllowPaging="false" AutoGenerateColumns="False"
              SummaryKey="gdv_condition_conditionlist_summary" CaptionKey="aca_caption_condition_conditionlist"
                IsAutoWidth="True" ShowCaption="true" EmptyDataRowStyle-CssClass="ACA_SmLabel ACA_SmLabel_FontSize"
                AllowSorting="true" IsInSPEARForm="true"  OnRowDataBound="ConditionList_RowDataBound" 
                OnRowCommand="ConditionList_RowCommand" PagerStyle-HorizontalAlign="center" 
                GridViewNumber="60036">
                <Columns>
                    <ACA:AccelaTemplateField ShowHeader="false">
                        <itemtemplate>
                                            <div>
                                            <a id="<%# GetLinkExpandCommentID(Eval("RowIndex")) %>" href="<%# ExpandComment(Eval("RowIndex")) %>" class="ACA_Content_Collapse NotShowLoading" title="<%=GetTitleByKey("img_alt_expand_icon","ACA_Common_Label_Comment") %>">
                                                <img id="<%# ComposeID(Eval("RowIndex")) %>" class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_expand_icon") %>" src="<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>"/>
                                            </a>
                                            </div>
                                        </itemtemplate>
                        <itemstyle Width="16px" CssClass="ACA_VerticalAlign"/>
                        <HeaderStyle Width="16px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField ShowHeader="false">
                        <itemtemplate>
                                            <div>
                                            <asp:Image runat="server" id="imgSevrity" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "src") %>' AlternateText='<%# DataBinder.Eval(Container.DataItem, "alt") %>'></asp:Image>
                                            </div>
                                        </itemtemplate>
                        <itemstyle Width="20px" CssClass="ACA_VerticalAlign"/>
                        <HeaderStyle Width="20px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkNameHeader">
                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkNameHeader"  runat="server" SortExpression="objectConditionName" LabelKey="per_conditionList_Label_name"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                        <itemtemplate>
                                            <div>
                                                <strong><ACA:AccelaLabel  ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "objectConditionName") %>' /></strong>
                                            </div>
                                        </itemtemplate>
                        <ItemStyle Width="128px" />
                        <headerstyle Width="128px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkStatusHeader">
                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkStatusHeader" runat="server" CommandName="Header" SortExpression="conditionStatus" LabelKey="per_conditionList_Label_status"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                        <itemtemplate>
                                            <div>
                                                <ACA:AccelaLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "conditionStatus") %>' />
                                            </div>
                                        </itemtemplate>
                        <ItemStyle Width="80px" />
                        <headerstyle Width="80px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkSeverityHeader">
                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkSeverityHeader" runat="server" CommandName="Header" SortExpression="impactCode" LabelKey="per_conditionList_Label_severity"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                        <itemtemplate>
                                            <div>
                                                <ACA:AccelaLabel  ID="lblSeverity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "impactCode") %>'></ACA:AccelaLabel>
                                            </div>
                                        </itemtemplate>
                        <ItemStyle Width="80px" />
                        <headerstyle Width="80px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkAppliedDateHeader">
                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkAppliedDateHeader" runat="server" CommandName="Header" SortExpression="issuedDate" LabelKey="per_conditionList_Label_appliedDate"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                        <itemtemplate>
                                            <div>
                                                <ACA:AccelaDateLabel id="lblAppliedDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "issuedDate")%>'></ACA:AccelaDateLabel>
                                            </div>
                                        </itemtemplate>
                        <ItemStyle Width="128px" />
                        <headerstyle Width="128px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkEffectiveDateHeader">
                        <headertemplate>
                                            <div>
                                                <ACA:GridViewHeaderLabel ID="lnkEffectiveDateHeader" runat="server" CommandName="Header" SortExpression="effectDate" LabelKey="per_conditionList_Label_effectiveDate"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                                            </div>
                                        </headertemplate>
                        <itemtemplate>
                                            <div>
                                                <ACA:AccelaDateLabel id="lblEffectiveDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "effectDate")%>'></ACA:AccelaDateLabel>
                                            </div>
                                        </itemtemplate>
                        <ItemStyle Width="128px" />
                        <headerstyle Width="128px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkExpirationDateHeader">
                        <headertemplate>
                                        <div>
                                            <ACA:GridViewHeaderLabel ID="lnkExpirationDateHeader" runat="server" CommandName="Header" SortExpression="expireDate" LabelKey="per_conditionList_Label_expiredDate"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                                        </div>
                                        </headertemplate>
                        <itemtemplate>
                                            <div>
                                                <ACA:AccelaDateLabel id="lblExpirationDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "expireDate")%>'></ACA:AccelaDateLabel>
                                            </div>
                                        </itemtemplate>
                        <ItemStyle Width="128px" />
                        <headerstyle Width="128px" />
                    </ACA:AccelaTemplateField>
           
                               <ACA:AccelaTemplateField>
                                <itemtemplate>
                                    <tr>
                                        <td colspan="100%">
                                            <div id="<%# GetCommentDiv(Eval("RowIndex")) %>" style="display: none;">
                                                <div id="commentPanel" runat="server" style="width: 770px">
                                                    <table role='presentation' cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td><div style="width:41px;">&nbsp;</div></td>
                                                            <td valign="top" class='ACA_Comments'>
                                                                <%=GetTextByKey("ACA_Common_Label_Comment")%>&nbsp;&nbsp;
                                                            </td>
                                                            <td>
                                                                <ACA:AccelaLabel ID="lblComment" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "conditionDescription") %>'></ACA:AccelaLabel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                               </itemtemplate>
                    </ACA:AccelaTemplateField>
                </Columns>
                <EmptyDataRowStyle CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
                <PagerSettings Position="Bottom" Visible="true" Mode="numericfirstlast" NextPageText="Next"
                    PreviousPageText="Previous" />
            </ACA:AccelaGridView>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<ACA:AccelaInlineScript runat="server">
<script type="text/javascript">
    var imgCollapsed = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
    var imgExpanded = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
    var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>'; 
    var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';

    var hideContactLink = '<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
    var showContactLink = '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>';
    
    function showMorecontactCondition(containerId,linkId) {
        showMoreCondition(containerId, linkId);
    }

    function showMoreCondition(containerId,linkId) {
        if(containerId=='undefined') return;

        $("#"+containerId).toggle();
        
        var status = $("#"+containerId).css("display");

        if(status == "block"){
            $get(linkId).innerHTML = hideContactLink;
        }else{
            $get(linkId).innerHTML = showContactLink;
        }
    }
    
    function ShowConditionList(isShow)
    {
        if(isShow)
        {
            $("#<%=divConditionList.ClientID %>").css('display', 'block');
        } 
        else 
        {
            $("#<%=divConditionList.ClientID %>").css('display', 'none');
        }
    }
</script>
</ACA:AccelaInlineScript>