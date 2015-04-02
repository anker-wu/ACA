<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppSpecInfoTableList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.AppSpecInfoTableList" %>
<%@ Register Src="PopupActions.ascx" TagName="PopupActions" TagPrefix="uc1" %>
<div class="asit_listcontent">
    <asp:Repeater ID="rptASITableList" runat="server" OnItemDataBound="ASITableList_ItemDataBound"
        OnItemCommand="ASITableList_ItemCommand" EnableViewState="false" OnPreRender="ASITableList_PreRender">
        <ItemTemplate>
            <asp:UpdatePanel ID="tablePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Literal ID="litTableInfo" runat="server"></asp:Literal>
                    <ACA:AccelaHideLink  ID="lnkFocusAnchor" AltKey="aca_section508_msg_list" TabIndex="-1" runat="server"/>
                    <div id="divIncompleteMark" class="ACA_Message_Error ACA_Message_Error_FontSize" runat="server" visible="false">
                        <div class="ACA_Error_Icon">
                        <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_24.gif") %>"/>
                        </div>
                        <ACA:AccelaLabel ID="lblIncomplete" LabelKey="aca_asit_msg_validate_required_error" runat="server"> 
                        </ACA:AccelaLabel>
                    </div>
                    <ACA:AccelaHeightSeparate ID="sepForMark" runat="server" Height="15" />
                    <ACA:AccelaGridView ID="gdvASITable" IsInSPEARForm="true" AutoGenerateColumns="False" ShowCaption="true"
                        GridLines="None" AllowPaging="True" runat="server" EnableViewState="false" IsClearSelectedItems="true"
                        CheckBoxColumnIndex="0" Width="99%" AutoGenerateCheckBoxColumn="true" role="presentation"
                        OnPreRender="ASITable_PreRender" OnRowDataBound="ASITable_RowDataBound">
                        <Columns>
                            <ACA:AccelaTemplateField AttributeName="RequiredImageColumn" ShowHeader="false">
                                <ItemTemplate>
                                    <div ID="divImg" runat="server">
                                        <img id="imgMarkRequired" alt="<%=GetTextByKey("img_alt_mark_required") %>" src="<%= ImageUtil.GetImageURL("error_16.gif") %>" />
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="ACA_VerticalAlign" />
                            </ACA:AccelaTemplateField>
                            <ACA:AccelaTemplateField>
                                <ItemTemplate>
                                    <ACA:AccelaButton runat="server" ID="btnEditCurrentRow" CommandName="EditCurrentRow"
                                        CausesValidation="false" Text="editCurrent" CssClass="ACA_Hide" />
                                    <ACA:AccelaButton runat="server" ID="btnDeleteCurrentRow" CommandName="DeleteCurrentRow"
                                        CausesValidation="false" Text="delCurrent" CssClass="ACA_Hide" />
                                    <uc1:PopupActions ID="paActionMenu" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle Width="60px" />
                                <ItemStyle Width="60px" />
                            </ACA:AccelaTemplateField>
                        </Columns>
                    </ACA:AccelaGridView>
                    <div id="divActionRowHolder" runat="server" class="ACA_TabRow ACA_LiLeft action_buttons">
                        <ul>
                            <li>
                                <ACA:AccelaSplitButton ID="btnAdd" runat="server" Visible="False" CausesValidation="False" />
                            </li>
                            <li>
                                <ACA:AccelaButton ID="btnEdit" runat="server" Visible="False" CausesValidation="False" 
                                    DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" DivDisableCss="ACA_SmButtonDisabled ACA_SmButtonDisabled_FontSize"/>
                            </li>
                            <li>
                                <ACA:AccelaButton ID="btnDelete" runat="server" Visible="False" CausesValidation="False"
                                    DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" DivDisableCss="ACA_SmButtonDisabled ACA_SmButtonDisabled_FontSize"/>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ItemTemplate>
        <SeparatorTemplate>
            <div class="ACA_TabRow ACA_Line_Content">&nbsp;</div>
        </SeparatorTemplate>
    </asp:Repeater>
</div>