<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
 Inherits="Accela.ACA.Web.Cap.TradeNameDetail" ValidateRequest="false" Codebehind="TradeNameDetail.aspx.cs" %>
<%@ Import namespace="Accela.ACA.Common.Util"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="PlaceHolderMain">
<div class="ACA_RightItem">
    <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_tradename_detail" LabelType="PageInstruction"
                runat="server" />
    <h1>
        <ACA:AccelaLabel ID="lblTradeNameNumber" LabelKey="per_tradeNameDetail_label_tradeNameNumber"
            runat="server" ></ACA:AccelaLabel>
        <ACA:AccelaLabel ID="lblNumber" runat="server"></ACA:AccelaLabel>
        <br />
        <ACA:AccelaLabel ID="lblTradeName" runat="server"></ACA:AccelaLabel>
    </h1>
    <br />
    <ACA:AccelaLabel ID="lblTitle" LabelKey="per_tradeNameDetail_label_detail" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
    <div>
        <div class="ACA_TabRow">
            <div id="dvApplicant" runat="server" class="ACA_Div57P ACA_FLeft">
                <h1>
                    <i>
                        <ACA:AccelaLabel ID="lblApplicant" LabelKey="per_tradeNameDetail_label_applicant"
                            runat="server"></ACA:AccelaLabel>
                    </i>
                </h1>
                <span class="ACA_SmLabel ACA_SmLabel_FontSize">
                    <ACA:AccelaLabel ID="lblApplicantBasic" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                </span>
                <br />
            </div>
            <div id="dvLicenseProfessional" runat="server" class="ACA_Div37P ACA_FLeft">
                <h1>
                    <i>
                        <ACA:AccelaLabel ID="lblLicenseProfessional" LabelKey="per_permitDetail_label_licenseProfessioanl"
                            runat="server"></ACA:AccelaLabel>
                    </i>
                </h1>
                <span class="ACA_SmLabel ACA_SmLabel_FontSize">
                    <ACA:AccelaLabel ID="lblLicenseProfessionalBasic" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                </span>
            </div>
        </div>
    </div>
    <div id="tbMoreDetail" runat="server" class="ACA_TabRow">      
    <table role='presentation' border="0" width="100%" cellpadding="0" cellspacing="0">
      <tr>
          <td colspan="3" style="height: 24px">
            <h1 class="ACA_FLeftForStyle" >
              <a id="lnkTradeName" href="javascript:void(0);" onclick='ControlDisplay($get("TRMoreDetail"),$get("imgMoreDetail"),false,$get("lnkTradeName"),$get("<%=lblMoreDetail.ClientID %>"))'
               title="<%=GetTitleByKey("img_alt_expand_icon","per_tradeNameDetail_label_moreDetail") %>" class="NotShowLoading">
              <img style="cursor:pointer; border-width:0px;" alt="<%=GetTextByKey("img_alt_expand_icon") %>" 
              src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_collapsed.gif") %>" 
               id="imgMoreDetail"/></a>
               <ACA:AccelaLabel ID="lblMoreDetail" runat="server" IsDisplayLabel="True" IsNeedEncode="False"
                  LabelType="LabelText"  LabelKey="per_tradeNameDetail_label_moreDetail" ></ACA:AccelaLabel>
            </h1>
          </td>
      </tr >
      <tr id="TRMoreDetail" style="display:none;">
          <td class="moredetail_td">&nbsp;</td>
          <td colspan="2" style="height: 24px;">
              <div style="text-align: center;">
                  <div id="tbRCList" runat="server">
                        <table role='presentation' border="0" cellpadding="0" cellspacing="0" style="width: 98%">
                                                                              <tr>
                          <td class="MoreDetail_BlockTitle">
                              <h1>
                                <a id="lnkRelatContact" href="javascript:void(0);" onclick='ControlDisplay($get("trASITList"),$get("imgRc"),true,$get("lnkRelatContact"),$get("<%=lblRelatContact.ClientID %>"))'
                                 title="<%=GetTitleByKey("img_alt_expand_icon","per_tradeNameDetail_label_businessPartner") %>" class="NotShowLoading">
                                <img  style="cursor:pointer; border-width:0px" alt="<%=GetTextByKey("img_alt_expand_icon") %>" 
                                src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("plus_expand.gif") %>" 
                                id="imgRc" /></a>&nbsp;
                                <ACA:AccelaLabel ID="lblRelatContact" runat="server" IsDisplayLabel="True" IsNeedEncode="False"
                                  LabelType="LabelText" LabelKey="per_tradeNameDetail_label_businessPartner" >
                                  </ACA:AccelaLabel>
                              </h1>
                          </td>
                      </tr>
                          <tr id="trASITList" style="display:none">
                              <td class="MoreDetail_BlockContent4TradeName">
                                    <asp:PlaceHolder ID="pnlASITable" runat="server"></asp:PlaceHolder>
                              </td>
                          </tr>
                      </table>
                  </div>
              </div>
          </td>
      </tr>
    </table>
    <br />
    </div>
    <div runat ="server" id="divRelatedTradeLicense">
        <ACA:AccelaLabel ID="lblRelatedTradeLicense" LabelKey="per_tradeNameDetail_label_associatedLicense"
        runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
        <asp:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="always">
            <contenttemplate>
                <ACA:AccelaGridView ID="gdvPermitList" runat="server" AutoGenerateColumns="False" PageSize="10"
                PagerStyle-HorizontalAlign="center" AllowSorting="false"   GridViewNumber="60101"
                HeaderStyle-Font-Underline="false"  OnRowDataBound="PermitList_RowDataBound" 
                SummaryKey="gdv_tradedetail_permitlist_summary" CaptionKey="aca_caption_tradedetail_permitlist">
                <Columns>
                    <ACA:AccelaTemplateField AttributeName="lblTradeLicenseNumberHeader">
                        <headertemplate>
                            <div class="ACA_MLong ACA_Header_Row">
                                    <ACA:AccelaLabel ID="lblTradeLicenseNumberHeader" runat="server" CommandName="Header" 
                                        LabelKey="per_tradeNameDetail_Label_tradeLicenseNumber" ></ACA:AccelaLabel>
                            </div>
                        </headertemplate>
                        <itemtemplate>
                            <div class="ACA_MLong">
                                <ACA:AccelaLabel ID="lblTradeLicenseNumber" runat ="server" Text = '<%# DataBinder.Eval(Container.DataItem, "TradeLicenseNumber") %>' />
                            </div>
                        </itemtemplate>
                        <ItemStyle Width="100px" CssClass="ACA_AlignLeftOrRightTop"/>
                        <headerstyle Width="100px" CssClass="ACA_AlignLeftOrRight"/>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lblPermitTypeHeader">
                        <headertemplate>
                            <div class="ACA_MLong ACA_Header_Row">
                                    <ACA:AccelaLabel ID="lblPermitTypeHeader" runat="server" CommandName="Header" 
                                    LabelKey="per_tradeNameDetail_Label_permitType" ></ACA:AccelaLabel>
                            </div>
                        </headertemplate>
                        <itemtemplate>
                            <div class="ACA_MLong">
                                <ACA:AccelaLabel  ID="lblPermitType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PermitType") %>' />
                            </div>                                
                        </itemtemplate>
                        <ItemStyle Width="100px" CssClass="ACA_AlignLeftOrRightTop"/>
                        <headerstyle Width="100px" CssClass="ACA_AlignLeftOrRight"/>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lblEnteredDateHeader">
                        <headertemplate>
                            <div class="ACA_Column_Medium ACA_Header_Row">
                                     <ACA:AccelaLabel ID="lblEnteredDateHeader" runat="server" CommandName="Header" LabelKey ="per_tradeNameDetail_Label_dateEntered" >
                                        </ACA:AccelaLabel>
                            </div>
                        </headertemplate>
                        <itemtemplate>
                            <div class="ACA_Column_Medium">
                                <ACA:AccelaDateLabel ID="lblEnteredTime" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" Text2='<%# DataBinder.Eval(Container.DataItem, "DateEntered") %>' />
                            </div>                                
                        </itemtemplate>
                        <ItemStyle Width="100px" CssClass="ACA_AlignLeftOrRightTop"/>
                        <headerstyle Width="100px" CssClass="ACA_AlignLeftOrRight"/>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lblAction">
                        <headertemplate>
                            <div class="ACA_Column_Medium ACA_Header_Row">
                                     <ACA:AccelaLabel ID="lblAction" runat="server" LabelKey ="per_tradenamedetail_label_action" >
                                        </ACA:AccelaLabel>
                            </div>
                        </headertemplate>
                        <itemtemplate>
                            <div class="ACA_Column_Medium">
                                <asp:HyperLink ID="hlTradeLicenseDetail" runat ="server" ><strong><%# Accela.ACA.Web.BasePage.GetStaticTextByKey("per_tradeNameDetail_Label_view")%></strong></asp:HyperLink>
                                <ACA:AccelaLabel ID="lblCapID1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "capID1") %>' Visible="false" />
                                <ACA:AccelaLabel ID="lblCapID2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "capID2") %>' Visible="false" />
                                <ACA:AccelaLabel ID="lblCapID3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "capID3") %>' Visible="false" />
                            </div>                                
                        </itemtemplate>
                        <ItemStyle Width="200px" CssClass="ACA_AlignLeftOrRightTop"/>
                        <headerstyle Width="200px" CssClass="ACA_AlignLeftOrRight"/>
                    </ACA:AccelaTemplateField>
                </Columns>
                </ACA:AccelaGridView>
            </contenttemplate>
        </asp:UpdatePanel>
      
    </div>
</div>

    <script language="javascript" type="text/javascript">
        var CTreeTop = '<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_collapsed.gif") %>';
        var ETreeTop = '<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_expanded.gif") %>';
        var CTreeMiddle = '<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("minus_collapse.gif") %>';
        var ETreeMiddle = '<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("plus_expand.gif") %>';
        var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>';
        var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';

        function ControlDisplay(obj, imgObj, isLeefNode, lnkObj, lblValue) {
            if (obj.style.display == 'block' || obj.style.display == "" || obj.style.display == "table-row") {
                if (!isLeefNode) {
                    Collapsed(imgObj, CTreeTop, altExpanded);
                }
                else {
                    Collapsed(imgObj, ETreeMiddle, altExpanded);
                }

                AddTitle(lnkObj, altExpanded, lblValue);

                obj.style.display = 'none';
            }
            else {
                if (!isLeefNode) {
                    Expanded(imgObj, ETreeTop, altCollapsed);
                }
                else {
                    Expanded(imgObj, CTreeMiddle, altCollapsed);
                }

                if (document.all)
                    obj.style.display = 'block';
                else
                    obj.style.display = 'table-row';

                AddTitle(lnkObj, altCollapsed, lblValue);
            }
        }
    </script>

</asp:Content>
