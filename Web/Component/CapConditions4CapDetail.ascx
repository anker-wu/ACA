<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CapConditions4CapDetail.ascx.cs" Inherits="Accela.ACA.Web.Component.CapConditions4CapDetail" %>
<%@ Register Src="~/Component/SectionHeader.ascx" TagName="SectionHeader" TagPrefix="uc1" %>

<div>
    <!--condition bar start-->
    <div>
        <asp:Panel ID="pnlLocked" runat="server" Visible="false">
            <div class="ACA_Message_Locked ACA_Message_Locked_FontSize">
                <div runat="server" id="divTitleLocked" class="ACA_Locked_Icon">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_locked") %>" src="<%=ImageUtil.GetImageURL("locked_24.gif") %>"/>
                </div>
                <ACA:AccelaLabel ID="lblLocked" IsNeedEncode="false" runat="server" />
            </div>
            <ACA:AccelaHeightSeparate ID="sepForLockCondition" runat="server" Height="10" />
        </asp:Panel>
        <asp:Panel ID="pnlHold" runat="server" Visible="false">
            <div class="ACA_Message_Hold ACA_Message_Hold_FontSize">
                <div runat="server" id="divTitleOnHold" class="ACA_Hold_Icon">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_hold") %>" src="<%=ImageUtil.GetImageURL("hold_24.gif") %>"/>
                </div>
                <ACA:AccelaLabel ID="lblHold" IsNeedEncode="false" runat="server" />
            </div>
            <ACA:AccelaHeightSeparate ID="sepForHoldCondition" runat="server" Height="10" />
        </asp:Panel>
        <asp:Panel ID="pnlNotice" runat="server" Visible="false">
            <div class="ACA_Message_Note ACA_Message_Note_FontSize">
                <div runat="server" id="divTitleNotice" class="ACA_Note_Icon">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_note") %>" src="<%=ImageUtil.GetImageURL("note_24.gif") %>"/>
                </div>
                <ACA:AccelaLabel ID="lblNotice" IsNeedEncode="false" runat="server" />
            </div>        
            <ACA:AccelaHeightSeparate ID="sepForNoteCondition" runat="server" Height="10" />
        </asp:Panel>
        <asp:Panel ID="pnlRequired" runat="server" Visible="false">
            <div class="ACA_Condition_Required ACA_Condition_Required_FontSize">
                <div runat="server" id="divTitleRequired" class="ACA_Condition_Required_Icon">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_required") %>" src="<%=ImageUtil.GetImageURL("required_24.gif") %>"/>
                </div>
                <ACA:AccelaLabel ID="lblRequired" IsNeedEncode="false" runat="server" />
            </div>
        </asp:Panel>
    </div>
    <!--condition bar end-->

    <!--general condition section start-->         
    <div id="divGeneralConditionsInfo" runat="server" visible="false">
        <uc1:SectionHeader runat="server" ID="shGeneralConditionsInfo" EnableExpand="false"
                Collapsible="true" SectionBodyClientID="divGeneralConditions" Collapsed="false">
        </uc1:SectionHeader>
        <asp:UpdatePanel ID="upGeneralConditions" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divGeneralConditions" class="ACA_Page">
                    <ACA:AccelaGridView ID="gdvGeneralConditionsList" runat="server" ShowExportLink="false" ShowHeader="false" ShowHeaderWhenEmpty="false" PageSize="5" role="presentation"
                        ShowCaption="true" AutoGenerateColumns="False" IsAutoWidth="true" CssClass="ACA_FullWidthTable" AllowPaging="true" RowStyle-CssClass="ACA_TabRow_Odd" AlternatingRowStyle-CssClass="ACA_TabRow_Odd" OnRowDataBound="GeneralConditionsList_RowDataBound">
                        <Columns>
                            <ACA:AccelaTemplateField>
                                <ItemTemplate>                                    
                                    <div id="divGeneralConditionsRecordType" runat="server" visible="false"  class="fontbold font13px ACA_Title_Color">
                                        <ACA:AccelaLabel ID="lblGeneralConditionsRecordType" IsNeedEncode="false" runat="server"/><br />
                                    </div>
                                    <div id="divGeneralConditionsGroupName" runat="server" visible="false">
                                        <ACA:AccelaLabel ID="lblGeneralConditionsGroupName" IsNeedEncode="false" CssClass="font13px fontbold ACA_NewDiv_Text_TabRow_Color_6" runat="server"/>
                                        <ACA:AccelaLabel ID="lblGeneralConditionsGroupCount" IsNeedEncode="false" CssClass="font12px" runat="server"/><br />
                                    </div>
                                    <div id="divGeneralConditionsType" runat="server" visible="false" class="fontbold font11px ACA_NewDiv_Text_TabRow_Color_6">
                                        <ACA:AccelaLabel ID="lblGeneralConditionsType" IsNeedEncode="false" runat="server"/><br />
                                    </div>                                    
                                    <ACA:AccelaLabel ID="lblGeneralConditionsInfo" IsNeedEncode="false" CssClass="conditiondetailpage" runat="server"/><br />
                                </ItemTemplate>
                                <ItemStyle CssClass="ACA_VerticalAlign" />
                            </ACA:AccelaTemplateField>
                        </Columns>     
                    </ACA:AccelaGridView>
                    <div id="divGeneralConditionsField" visible="false" runat="server">
                        <span>Click the area below to edit general condition available variables according to format:</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.ConditionName%>: The condition name</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.Status%>: The condition status</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.Severity%>: The condition severity</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.Priority%>: The condition priority</span><br />
                        <asp:PlaceHolder ID="phGeneralCondition" runat="server">
                            <span><%=ConditionsUtil.ListItemVariables.StatusDate%>: The status date</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.AppliedDate%>: The applied date</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.EffectiveDate%>: The effective date</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.ExpirationDate%>: The expiration date</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.ActionByDept%>: The action department</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.ActionByUser%>: The action user</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.AppliedByDept%>: The applied department</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.AppliedByUser%>: The applied user</span><br />
                        </asp:PlaceHolder>                                
                        <span><%=ConditionsUtil.ListItemVariables.ShortComments%>: The short comments</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.LongComments%>: The long comments</span><br/>
                        <span><%=ConditionsUtil.ListItemVariables.AdditionalInformation%>: The additional information</span>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="10" runat="server" />
                        <ACA:AccelaLabel ID="lblGeneralConditionsField" LabelType="BodyText" runat="server" />
                    </div>
                </div>   
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!--general condition section end-->

    <!--condition of approval section start-->    
    <div id="divConditionsOfApprovalInfo" runat="server" visible="false">
        <asp:UpdatePanel ID="upConditionsOfApproval" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:SectionHeader runat="server" ID="shConditionOfApproval" EnableExpand="false"
                    Collapsible="true" SectionBodyClientID="divConditionsOfApproval" Collapsed="false" SearchType="TextType" ShowInstruction="true">
                </uc1:SectionHeader>
                <div id="divConditionsOfApproval" class="ACA_Page" runat="server">
                    <div id="divHideOrShowMet" runat="server" visible="false">
                        <div id="divHideOrShowMetDaily" runat="server" visible="false">
                            <ACA:AccelaLinkButton ID="btnHideOrShowMet" runat="server" CssClass="ACA_LinkButton ACA_Title_Color font12px" CausesValidation="false" OnClick="HideOrShowMetButton_Click" ></ACA:AccelaLinkButton>
                        </div>
                        <div id="divHideOrShowMetAdmin" runat="server" visible="false">
                            <ACA:AccelaLinkButton ID="btnHideMet" runat="server" CssClass="ACA_LinkButton ACA_Title_Color font12px" LabelKey="aca_capcondition_label_hidemet"></ACA:AccelaLinkButton>
                            <ACA:AccelaLinkButton ID="btnShowMet" runat="server" CssClass="ACA_LinkButton ACA_Title_Color font12px" LabelKey="aca_capcondition_label_showmet"></ACA:AccelaLinkButton>
                        </div>
                    </div>
                    <ACA:AccelaLabel ID="NoRecordFound" CssClass="ACA_CapDetail_NoRecord font12px" LabelKey="aca_recorddetail_conditionofapprovalsearch_norecord" IsNeedEncode="false" runat="server" Visible="false"/>
                    <ACA:AccelaGridView ID="gdvConditionsOfApprovalList" runat="server" ShowExportLink="true" ShowHeader="false" ShowHeaderWhenEmpty="false" PageSize="5" role="presentation"
                        ShowCaption="true" AutoGenerateColumns="False" IsAutoWidth="true" CssClass="ACA_FullWidthTable" AllowPaging="true" RowStyle-CssClass="ACA_TabRow_Odd" AlternatingRowStyle-CssClass="ACA_TabRow_Odd" OnRowDataBound="ConditionsOfApprovalList_RowDataBound"
                        OnGridViewDownload="ConditionsOfApprovalList_GridViewDownload">
                        <Columns>
                            <ACA:AccelaTemplateField>
                                <ItemTemplate>
                                    <div id="divConditionsOfApprovalRecordType" runat="server" visible="false" class="fontbold font13px ACA_Title_Color">                                    
                                        <ACA:AccelaLabel ID="lblConditionsOfApprovalRecordType" IsNeedEncode="false" runat="server"/><br />
                                    </div>
                                    <div id="divConditionsOfApprovalGroupName" runat="server" visible="false">
                                        <ACA:AccelaLabel ID="lblConditionsOfApprovalGroupName" IsNeedEncode="false" CssClass="font13px fontbold ACA_NewDiv_Text_TabRow_Color_6" runat="server"/>
                                        <ACA:AccelaLabel ID="lblConditionsOfApprovalGroupCount" IsNeedEncode="false" CssClass="font12px" runat="server"/><br />
                                    </div>
                                    <div id="divConditionsOfApprovalType" runat="server" visible="false" class="fontbold font11px ACA_NewDiv_Text_TabRow_Color_6">
                                        <ACA:AccelaLabel ID="lblConditionsOfApprovalType" IsNeedEncode="false" runat="server"/><br />
                                    </div>
                                    <ACA:AccelaLabel ID="lblConditionsOfApprovalInfo" IsNeedEncode="false" CssClass="conditiondetailpage" runat="server"/><br />
                                </ItemTemplate>
                                <ItemStyle CssClass="ACA_VerticalAlign" />
                            </ACA:AccelaTemplateField>
                        </Columns>
                    </ACA:AccelaGridView>
                    <div id="divConditionsOfApprovalField" visible="false" runat="server">
                        <span><%=ConditionsUtil.ListItemVariables.ConditionName%>: The condition name</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.Status%>: The condition status</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.Severity%>: The condition severity</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.Priority%>: The condition priority</span><br />
                        <asp:PlaceHolder ID="phConditionsOfApproval" runat="server">
                            <span><%=ConditionsUtil.ListItemVariables.StatusDate%>: The status date</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.AppliedDate%>: The applied date</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.ActionByDept%>: The action department</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.ActionByUser%>: The action user</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.AppliedByDept%>: The applied department</span><br />
                            <span><%=ConditionsUtil.ListItemVariables.AppliedByUser%>: The applied user</span><br />
                        </asp:PlaceHolder>
                        <span><%=ConditionsUtil.ListItemVariables.ShortComments%>: The short comments</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.LongComments%>: The long comments</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.AdditionalInformation%>: The additional information</span><br />
                        <span><%=ConditionsUtil.ListItemVariables.ViewDetail%>: The URL link variable to show condition details in a pop-up dialog</span>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server" />
                        <span>Click the area below to format the list of <font class="ACA_FontSize12">pending</font> conditions of approval using the variables above.</span><br />
                        <ACA:AccelaLabel ID="lblPendingConditionsOfApprovalField" LabelType="BodyText" runat="server" />
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="10" runat="server" />
                        <span>Click the area below to format the list of <font class="ACA_FontSize12">met</font> conditions of approval using the variables above.</span><br />
                        <ACA:AccelaLabel ID="lblMetConditionsOfApprovalField" LabelType="BodyText" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!--condition of approval section end-->
</div>
<script type="text/javascript">
    function ViewConditionDetail(obj, agencyCode, capId1, capId2, capId3, conditionNbr, moduleName) {
        SetNotAskForSPEAR();
        var targetUrl = '<%=FileUtil.AppendApplicationRoot("Cap/ConditionOfApprovalDetail.aspx") %>';
        var params = "?agencyCode=" + agencyCode + "&capId1=" + capId1 + "&capId2=" + capId2 + "&capId3=" + capId3 + "&conditionNbr=" + conditionNbr + "&Module=" + moduleName;
        ACADialog.popup({ url: targetUrl + params, width: 750, height: 600, objectTarget: obj });
        return false;
    }

    function ViewConditionAdditionalInfoDetail(obj, agencyCode, conditionNbr) {
        SetNotAskForSPEAR();
        var targetUrl = '<%=FileUtil.AppendApplicationRoot("Cap/ConditionAdditionalInfoDetail.aspx") %>';
        var params = "?agencyCode=" + agencyCode + "&conditionNbr=" + conditionNbr;
        ACADialog.popup({ url: targetUrl + params, width: 500, height: 600, objectTarget: obj });
        return false;
    }
</script>