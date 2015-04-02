<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="Accela.ACA.Web.Account.LicenseList"
    Title="License List" ValidateRequest="false" CodeBehind="LicenseList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <script type="text/javascript">
    function ConfirmAddLicense()
    {       
        if(confirm('<%=GetTextByKey("acc_message_confirm_addLicense").Replace("'","\\'") %>'))
        {
            return true;
        }
        else
        {
            return false;
        }        
    }
    </script>
    <asp:UpdatePanel ID="LicenseListPanel" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <div class="ACA_Content" id="divLicenseList">
                <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_licenselist"
                    LabelType="PageInstruction" runat="server" />
                <h1>
                    <ACA:AccelaLabel ID="lblLicenseListExplain" LabelKey="acc_reg_label_licenseListExplain"
                        runat="server" />
                </h1>
                <div class="ACA_TabRow">
                    <p class="ACA_FRight">
                        <span class="ACA_Required_Indicator">*</span>
                        <ACA:AccelaLabel ID="lblLicenseIndicate" LabelKey="acc_reg_label_indicate" runat="server" /></p>
                </div>
                <ACA:AccelaLabel ID="lblLicenseInfo" LabelKey="acc_reg_label_licenseInfo" runat="server"
                    LabelType="SectionTitle" />
                <div class="ACA_TabRow">
                    <p>
                        <ACA:AccelaLabel ID="lblResultClewInfo" CssClass="ACA_Show" LabelKey="acc_reg_label_resultClewInfo" runat="server" />
                        <ACA:AccelaLabel ID="lblResultClewInfo4ActivateAccount" CssClass="ACA_Show" LabelKey="aca_licenselist_label_instruction_for_activateaccount" runat="server" />
                    </p>
                </div>
                <ACA:AccelaGridView ID="gdvLicenseList" runat="server" AllowPaging="True" AllowSorting="true" CaptionKey="aca_caption_capedit_licenselist"
                    OnRowCommand="LicenseList_RowCommand" OnRowDataBound="LicenseList_RowDataBound"
                    AutoGenerateColumns="False" ShowCaption="True" PageSize="10" PagerStyle-HorizontalAlign="center"
                    PagerStyle-VerticalAlign="bottom" GridViewNumber="60091" SummaryKey="gdv_capedit_licenselist_summary">
                    <Columns>
                        <ACA:AccelaTemplateField AttributeName="lnkLicenseNumberHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkLicenseNumberHeader" runat="server" CommandName="Header"
                                        SortExpression="LicenseNumber" LabelKey="acc_reg_label_Nbr" CausesValidation="false" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLinkButton ID="lnkLicenseNbr" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseNumber") %>'></ACA:AccelaLinkButton>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <HeaderStyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <%-- begin license expired status--%>
                        <ACA:AccelaTemplateField AttributeName="lnkLienseStatusHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkLienseStatusHeader" runat="server" CommandName="Header"
                                        SortExpression="IsLicExpired" CausesValidation="false" LabelKey="acc_reg_label_licensestatus" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblLicenseStatus" runat="server" Text='<%# LicenseUtil.GetLicenseStatus(Convert.ToBoolean(Eval("IsLicExpired")))%>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="130px" />
                            <HeaderStyle Width="130px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkInsuranceStatusHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkInsuranceStatusHeader" runat="server" CommandName="Header"
                                        SortExpression="IsInsExpired" CausesValidation="false" LabelKey="acc_reg_label_insurancestatus" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblInsuranceStatus" runat="server" Text='<%# LicenseUtil.GetLicenseStatus(Convert.ToBoolean(Eval("IsInsExpired"))) %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="130px" />
                            <HeaderStyle Width="130px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkBusinessLicStatusHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkBusinessLicStatusHeader" runat="server" CommandName="Header"
                                        SortExpression="IsBusLicExpired" CausesValidation="false" LabelKey="acc_reg_label_businesslicstatus" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblBusinessLicStatus" runat="server" Text='<%# LicenseUtil.GetLicenseStatus(Convert.ToBoolean(Eval("IsBusLicExpired"))) %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="160px" />
                            <HeaderStyle Width="160px" />
                        </ACA:AccelaTemplateField>
                        <%-- end license expired status--%>
                        <ACA:AccelaTemplateField AttributeName="lnkLicenseTypeHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkLicenseTypeHeader" runat="server" CommandName="Header"
                                        SortExpression="LicenseType" LabelKey="acc_reg_label_type" CausesValidation="false" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblLicenseType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseTypeText") %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <HeaderStyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkContractNameHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkContractNameHeader" runat="server" CommandName="Header"
                                        SortExpression="ContractName" LabelKey="acc_reg_label_name" CausesValidation="false" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblLicenseContractName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ContractName").ToString() %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <HeaderStyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkBusinessNameHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:GridViewHeaderLabel ID="lnkBusinessNameHeader" runat="server" CommandName="Header"
                                        SortExpression="BusinessName" LabelKey="aca_register_businessname" CausesValidation="false" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblLicenseBusinessName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BusinessName").ToString() %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <HeaderStyle Width="100px" />
                        </ACA:AccelaTemplateField>
                          <ACA:AccelaTemplateField AttributeName="lnkActionHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:AccelaLabel ID="lnkActionHeader" runat="server" LabelKey="aca_regis_licenselist_label_action"
                                        IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div runat="server" id="divAction">
                                    <ACA:AccelaLinkButton ID="lnkConnectLicense" LabelKey="aca_regis_contactlist_label_lnkconnect" OnClientClick="return ConfirmAddLicense();" 
                                        runat="server" CausesValidation="false" CommandName="ConnectLicense" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LicenseNumber") %>'></ACA:AccelaLinkButton></div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <HeaderStyle Width="100px" />
                        </ACA:AccelaTemplateField>
                    </Columns>
                </ACA:AccelaGridView>
                <ACA:AccelaHeightSeparate ID="sepForButton" runat="server" Height="25" />
                <ACA:AccelaButton ID="backToSearch" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                    LabelKey="acc_reg_label_searchAgain" runat="server" OnClick="BackToSearchButton_OnClick" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>