<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.CapConditions" Codebehind="CapConditions.ascx.cs" %>

<div>
    <!--condition bar start-->
    <div>
        <asp:Panel ID="pnlLocked" runat="server" Visible="false">
            <div class="ACA_Message_Locked ACA_Message_Locked_FontSize">
                <div runat="server" id="divTitleLocked" class="ACA_Locked_Icon">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_locked") %>" src="<%=ImageUtil.GetImageURL("locked_24.gif") %>"/>
                </div>
                <ACA:AccelaLabel ID="lblLocked" IsNeedEncode="false" runat="server" />
                <br />
                <br />
                <a href="javascript:void(0);" onclick="<%=ClientID %>_showMoreCondition(this);" style="cursor:pointer" class="NotShowLoading"><%=GetTextByKey("per_condition_Label_show")%></a>
            </div>
            <ACA:AccelaHeightSeparate ID="sepForLockCondition" runat="server" Height="10" />
        </asp:Panel>
        <asp:Panel ID="pnlHold" runat="server" Visible="false">
            <div class="ACA_Message_Hold ACA_Message_Hold_FontSize">
                <div runat="server" id="divTitleOnHold" class="ACA_Hold_Icon">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_hold") %>" src="<%=ImageUtil.GetImageURL("hold_24.gif") %>"/>
                </div>
                <ACA:AccelaLabel ID="lblHold" IsNeedEncode="false" runat="server" />
                <br />
                <br />
                <a href="javascript:void(0);" onclick="<%=ClientID %>_showMoreCondition(this);" style="cursor:pointer" class="NotShowLoading"><%=GetTextByKey("per_condition_Label_show")%></a>
            </div>
            <ACA:AccelaHeightSeparate ID="sepForHoldCondition" runat="server" Height="10" />
        </asp:Panel>
        <asp:Panel ID="pnlNotice" runat="server" Visible="false">
            <div class="ACA_Message_Note ACA_Message_Note_FontSize">
                <div runat="server" id="divTitleNotice" class="ACA_Note_Icon">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_note") %>" src="<%=ImageUtil.GetImageURL("note_24.gif") %>"/>
                </div>
                <ACA:AccelaLabel ID="lblNotice" IsNeedEncode="false" runat="server" />
                <br />
                <br />
                <a href="javascript:void(0);" onclick="<%=ClientID %>_showMoreCondition(this);" style="cursor:pointer" class="NotShowLoading"><%=GetTextByKey("per_condition_Label_show")%></a>
            </div>        
            <ACA:AccelaHeightSeparate ID="sepForNoteCondition" runat="server" Height="10" />
        </asp:Panel>
        <asp:Panel ID="pnlRequired" runat="server" Visible="false">
            <div class="ACA_Condition_Required ACA_Condition_Required_FontSize">
                <div runat="server" id="divTitleRequired" class="ACA_Condition_Required_Icon">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_condition_notice_required") %>" src="<%=ImageUtil.GetImageURL("required_24.gif") %>"/>
                </div>
                <ACA:AccelaLabel ID="lblRequired" IsNeedEncode="false" runat="server" />
                <br />
                <br />
                <a href="javascript:void(0);" onclick="<%=ClientID %>_showMoreCondition(this);" style="cursor:pointer" class="NotShowLoading"><%=GetTextByKey("per_condition_Label_show")%></a>
            </div>        
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate5" runat="server" Height="10" />
        </asp:Panel>
    </div>
    <!--condition bar end-->

    <div id="divConditionPanel" class="ACA_Hide" runat="server">
        <!--general condition section start-->
        <div id="divGeneralConditionsInfo" runat="server" visible="false">
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate4" runat="server" Height="10" />
            <ACA:AccelaLabel ID="lblGeneralConditionsTitle" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
            <asp:UpdatePanel ID="upGeneralConditions" runat="server">
                <ContentTemplate>
                    <div class="ACA_Page">                   
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
                                        <ACA:AccelaLabel ID="lblGeneralConditionsInfo" IsNeedEncode="false" runat="server" CssClass="conditiondetailpage"/><br />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="ACA_VerticalAlign" />
                                </ACA:AccelaTemplateField>
                            </Columns>     
                        </ACA:AccelaGridView>
                        <div id="divGeneralConditionsField" visible="false" runat="server">
                            <span>Click the area below to edit general condition available variables according to format:</span><br />
                            <span>$$ConditionName$$: The condition name</span><br />
                            <span>$$Status$$: The condition status</span><br />
                            <span>$$Severity$$: The condition severity</span><br />
                            <span>$$Priority$$: The condition priority</span><br />
                            <asp:PlaceHolder ID="phGeneralCondition" runat="server">
                                <span>$$StatusDate$$: The status date</span><br />
                                <span>$$AppliedDate$$: The applied date</span><br />
                                <span>$$EffectiveDate$$: The effective date</span><br />
                                <span>$$ExpirationDate$$: The expiration date</span><br />
                                 <span>$$ActionByDept$$: The action department</span><br />
                                <span>$$ActionByUser$$: The action user</span><br />
                                <span>$$AppliedByDept$$: The applied department</span><br />
                                <span>$$AppliedByUser$$: The applied user</span><br />    
                            </asp:PlaceHolder>                                
                            <span>$$ShortComments$$: The short comments</span><br />
                            <span>$$LongComments$$: The long comments</span><br/>
                            <span>$$AdditionalInformation$$: The additional information</span>
                            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="15" runat="server" />
                            <ACA:AccelaLabel ID="lblGeneralConditionsField" LabelType="BodyText" runat="server" />
                        </div>
                    </div>   
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!--general condition section end-->

        <!--condition of approval section start-->    
        <div id="divConditionsOfApprovalInfo" runat="server" visible="false">
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="10" />
            <ACA:AccelaLabel ID="lblConditionsOfApprovalTitle" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
            <asp:UpdatePanel ID="upConditionsOfApproval" runat="server">
                <ContentTemplate>
                    <div class="ACA_Page">
                        <div id="divHideOrShowMet" runat="server" visible="false">
                            <div id="divHideOrShowMetDaily" runat="server" visible="false">
                                <ACA:AccelaLinkButton ID="btnHideOrShowMet" runat="server" CssClass="ACA_LinkButton ACA_Title_Color font12px" CausesValidation="false" OnClick="HideOrShowMetButton_Click" ></ACA:AccelaLinkButton>
                            </div>
                            <div id="divHideOrShowMetAdmin" runat="server" visible="false">                        
                                <ACA:AccelaLinkButton ID="btnHideMet" runat="server" CssClass="ACA_LinkButton ACA_Title_Color font12px" LabelKey="aca_capcondition_label_hidemet"></ACA:AccelaLinkButton>
                                <ACA:AccelaLinkButton ID="btnShowMet" runat="server" CssClass="ACA_LinkButton ACA_Title_Color font12px" LabelKey="aca_capcondition_label_showmet"></ACA:AccelaLinkButton>
                            </div>
                        </div>
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
                                        <ACA:AccelaLabel ID="lblConditionsOfApprovalInfo" IsNeedEncode="false" runat="server" CssClass="conditiondetailpage"/><br />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="ACA_VerticalAlign" />
                                </ACA:AccelaTemplateField>                
                            </Columns>
                        </ACA:AccelaGridView>
                        <div id="divConditionsOfApprovalField" visible="false" runat="server">
                            <span>Click the area below to edit condition of approval available variables according to format:</span><br />
                            <span>$$ConditionName$$: The condition name</span><br />
                            <span>$$Status$$: The condition status</span><br />
                            <span>$$Severity$$: The condition severity</span><br />
                            <span>$$Priority$$: The condition priority</span><br />
                            <asp:PlaceHolder ID="phConditionsOfApproval" runat="server">
                                <span>$$StatusDate$$: The status date</span><br />
                                <span>$$AppliedDate$$: The applied date</span><br />
                                <span>$$ActionByDept$$: The action department</span><br />
                                <span>$$ActionByUser$$: The action user</span><br />
                                <span>$$AppliedByDept$$: The applied department</span><br />
                                <span>$$AppliedByUser$$: The applied user</span><br />
                            </asp:PlaceHolder>
                            <span>$$ShortComments$$: The short comments</span><br />
                            <span>$$LongComments$$: The long comments</span><br/>
                            <span>$$AdditionalInformation$$: The additional information</span>
                            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="15" runat="server" />
                            <ACA:AccelaLabel ID="lblConditionsOfApprovalField" LabelType="BodyText" runat="server" />                        
                        </div>
                    </div> 
                    <ACA:AccelaInlineScript  runat="server" ID="inlinescriptOfBtnExport">
                        <script type="text/javascript">
                            if ($.exists("a[id$='4btnExport'")) {
                                $("a[id$='4btnExport']").each(function () {
                                    $(this).bind("click", function () {
                                        SetNotAskForSPEAR();
                                    });
                                });
                            }
                        </script>
                    </ACA:AccelaInlineScript>  
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!--condition of approval section end-->
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function(){
        var $divConditionPanel = $('#<%=divConditionPanel.ClientID %>');      

        if ($.global.isAdmin || '<%=ShowConditionBar%>' == 'False' || $divConditionPanel.is(":visible")) {
            $divConditionPanel.show();
        }
        else {
            $divConditionPanel.hide();
        }
    });

    function <%=ClientID %>_showMoreCondition(lnkViewDetail){
        if(typeof(SetNotAsk) != 'undefined'){
            SetNotAsk();
        }

        var $divConditionPanel = $('#<%=divConditionPanel.ClientID %>');

        if($.exists($divConditionPanel)){
            $divConditionPanel.toggle();

            if(lnkViewDetail != null){
                lnkViewDetail.innerHTML = $divConditionPanel.is(":visible") ? '<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>':'<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>';                
            }
        }
    }
    
    function ViewConditionAdditionalInfoDetail(obj, agencyCode, conditionNbr) {
        SetNotAskForSPEAR();
        var targetUrl = '<%=FileUtil.AppendApplicationRoot("Cap/ConditionAdditionalInfoDetail.aspx") %>';
        var params = "?agencyCode=" + agencyCode + "&conditionNbr=" + conditionNbr;
        ACADialog.popup({ url: targetUrl + params, width: 500, height: 600, objectTarget: obj });
        return false;
    }
</script>