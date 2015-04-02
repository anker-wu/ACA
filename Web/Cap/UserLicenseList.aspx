<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Accela.ACA.Web.Cap.UserLicenseList" MasterPageFile="~/Default.master" Codebehind="UserLicenseList.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <div class="ACA_Content"> 
        <div id="divDelegateSection" visible="false" runat="server">
                <h1>
                    <ACA:AccelaLabel ID="lblDelegateUserTitle" runat="server" LabelKey="aca_create_application_as"></ACA:AccelaLabel>
                </h1>
                <div id="divDelegateUser" class="ACA_Page ACA_Page_FontSize">
                    <table role='presentation'>
                        <tr> 
                            <td colspan="2">
                                <ACA:AccelaRadioButton ID="rdMySelf" GroupName="UserSelected" CssClass="ACA_Label ACA_Label_FontSize_Smaller" runat="server" AutoPostBack="true" OnCheckedChanged="UserType_OnSelectedIndexChanged" LabelKey="aca_delegate_myself" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ACA:AccelaRadioButton ID="rdInitUsers" GroupName="UserSelected" CssClass="ACA_Label ACA_Label_FontSize_Smaller" Checked="true" runat="server" AutoPostBack="true" OnCheckedChanged="UserType_OnSelectedIndexChanged" LabelKey="aca_delegate_another_person" />
                             </td>
                             <td>
                                <ACA:AccelaDropDownList ID="ddlInitUserList" IsHiddenLabel="true" Required="true" AutoPostBack="true" OnSelectedIndexChanged="InitUserListDropDown_OnSelectedIndexChanged" 
                                     ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip" runat="server"></ACA:AccelaDropDownList>
                             </td>
                        </tr>
                    </table>
                </div>
          </div>
        <br />
        <div id="divInstruction" runat="server" class="ACA_TabRow">
             <h1>
                <ACA:AccelaLabel ID="per_applyPermit_label_selectLicProfessional" LabelKey="per_applyPermit_label_selectLicProfessional"
                    runat="server" />
            </h1>
            <ACA:AccelaHeightSeparate ID="sepForTiltle" runat="server" Height="13" />
            <div class="ACA_Page ACA_Page_FontSize">
                <ACA:AccelaLabel ID="per_permitType_label_permitType" LabelKey="per_selectLicProfessional_label_instructionInfo"
                    runat="server" LabelType="BodyText" />
            </div>
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="5" />
        </div>
        <div id="divLicenseSelector" runat="server" style="padding-bottom:25px;">
            <div>
                <asp:UpdatePanel ID="LicensePanel" runat="server">
                    <ContentTemplate>
                        <div>
                            <ACA:AccelaDropDownList ID="ddlLicenseID" LabelKey="per_permitLicenseEdit_label_licensedPro"
                                AutoPostBack="true" OnSelectedIndexChanged="LicenseIDDropDown_SelectedIndexChanged" onchange="licenseIdChange();"
                                Required="true" runat="server" ToolTipLabelKey="aca_common_msg_dropdown_selectlicense_tip" /></div>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="0" />
                        <asp:Panel ID="licenseInfo" runat="server" Visible="false">
                            <div class="ACA_TabRow ACA_LiLeft" style="	color: #666666;font-family: Arial, sans-serif;font-size: 1.2em;">
                                <table role='presentation'>
                                    <tr valign="top">
                                        <td width="400px">
                                            <ACA:AccelaLabel ID="lblLicenseBasic" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                                        </td>
                                        <td>
                                            <ACA:AccelaLabel ID="lblLicenseExt" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" runat="server" Height="12" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <br />
        </div>
    
    <div id="divValidationLicense" class="ACA_LgButtonHeight" runat="server">
        <ACA:AccelaButton ID="btnValidationLicense" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize ACA_Button_Text"
            LabelKey="per_applyPermit_label_continueAppPro" runat="server" OnClick="ValidationLicense" />
    </div>
   <div visible="false">  
        <asp:Button ID="btnContinueToConfirm" runat="server" style="display:none" />
    </div>
            
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate4" runat="server" Height="5" />
        <div id="divTradeNameList" runat="server">
         <asp:UpdatePanel ID="TradeNamePanel" runat="server">
           <ContentTemplate>
           <ACA:AccelaGridView ID="gdvTradeNameList" runat="server" AllowPaging="True" GridViewNumber="60098" 
            SummaryKey="gdv_userlicense_tradenamelist_summary" CaptionKey="aca_caption_userlicense_tradenamelist"
            AllowSorting="true" AutoGenerateColumns="False" PageSize="10"
            ShowCaption="true" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" 
            OnRowDataBound="TradeNameList_RowDataBound">
            <Columns>
                 <ACA:AccelaTemplateField AttributeName="lnkNumberHeader">
                    <headertemplate>
                        <div class="ACA_CapListStyle">
                        <ACA:GridViewHeaderLabel ID="lnkNumberHeader" runat="server" CommandName="Header" SortExpression="TradeNumber" 
                                LabelKey ="per_tradeNameList_Label_number" >
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                         <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblNumber" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" Text='<%# Eval("TradeNumber") %>' />
                         </div>
                    </itemtemplate>
                    <ItemStyle Width="80px"/>
                    <headerstyle Width="80px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkEnglishNameHeader">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkEnglishNameHeader" runat="server" CommandName="Header" SortExpression="EnglishName" 
                                LabelKey ="per_tradeNameList_Label_englishName" >
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblEnglishName" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" Text='<%# Eval("EnglishName") %>' />
                        </div>
                    </itemtemplate>
                        <ItemStyle Width="100px"/>
                        <headerstyle Width="100px"/>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkName2Header">
                        <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkName2Header" runat="server" CommandName="Header" SortExpression="Name2" 
                                LabelKey="per_tradeNameList_Label_arabicName" >
                             </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle"  style="text-align:right;">
                            <ACA:AccelaLabel ID="lblName2" runat ="server"  Text = '<%# Eval("Name2") %>' />
                        </div>
                    </itemtemplate>
                        <ItemStyle Width="100px"/>
                        <headerstyle Width="100px"/>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkStatusHeader">
                        <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkStatusHeader" runat="server" CommandName="Header" SortExpression="Name2" 
                                LabelKey="per_permitList_licenseList_Status" >
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel  ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' />
                        </div>
                    </itemtemplate>
                        <ItemStyle Width="100px"/>
                        <headerstyle Width="100px"/>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkActionsHeader">
                        <headertemplate>
                        <div style="white-space:nowrap;" class="ACA_CapListStyle ACA_Header_Row">                       
                            <ACA:AccelaLabel IsGridViewHeadLabel="true" ID="lnkActionsHeader" runat="server" LabelKey="per_tradenamelist_label_action" IsNeedEncode="false"></ACA:AccelaLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLinkButton ID="btnRequestLicense" runat="server" CommandName="CreateLicense" Visible="false"></ACA:AccelaLinkButton>
                            <ACA:AccelaLabel ID="lblType" runat="server"   Text='<%# Eval("Type") %>' Visible="false"></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                        <ItemStyle Width="130px"/>
                        <headerstyle Width="130px"/>
                    </ACA:AccelaTemplateField>
                </Columns>
            </ACA:AccelaGridView>
           </ContentTemplate>
         </asp:UpdatePanel>
        </div>
    </div>

    <script type="text/javascript">
        function licenseIdChange() {
            if ($('#<%=ddlLicenseID.ClientID %>').val() != '') {
                doErrorCallbackFun('', '<%=ddlLicenseID.ClientID %>', 0);
            }
        }
    </script>
</asp:Content>
