<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" 
    Inherits="Accela.ACA.Web.APO.ParcelDetail" ValidateRequest="false" Codebehind="ParcelDetail.aspx.cs" %>
<%@ Import namespace="Accela.ACA.Common.Util" %>
<%@ Import namespace="Accela.ACA.WSProxy" %>
<%@ Register Src="~/Component/RefAPOAddressList.ascx" TagName="AddressList" TagPrefix="uc1" %>
<%@ Register Src="~/Component/RefAPOOwnerList.ascx" TagName="OwnerList" TagPrefix="uc1" %>
<%@ Register Src="~/Component/ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<%@ Register Src="~/Component/RefGenealogyList.ascx" TagName="RefGenealogyList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    
    <div id="MainContent" class="ACA_Content" runat="server">
        <div id="infoParcel" runat="server">
            <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_parcel_detail" LabelType="PageInstruction"
                runat="server" />
            <ACA:ACAMap ID="mapParcel" AGISContext="ParcelDetail" OnShowOnMap="MapParcel_ShowOnMap" runat="server" />
            <span runat="server" id="spanDisabledHolder" visible="false">
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="15" />
                <span runat="server" id="spanDisabledInfo"></span>
            </span>
            
            <div runat="server" id="spanRelatedInfo">
                <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Session_Detail" ID="APO_ParcelDetail_lbl_Session_Detail"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>

                <div id="dvParcelInfo" runat="server"> 
                    <table role='presentation' class="NoBorder ACA_FullWidthTable">
                    <tr class="ACA_VerticalAlign"><td class='<%=ParcelInfoCss %>'>               
                    <table role='presentation' class="ACA_FullWidthTable">
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%; " nowrap="nowrap">
                                <h1>
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Header" ID="APO_ParcelDetail_lbl_Parcel_Header"
                                        runat="server"></ACA:AccelaLabel>
                                </h1>
                            </td>
                            <td class="ACA_AlignLeftOrRight" >&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%; vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Number" ID="APO_ParcelDetail_lbl_Parcel_Number"
                                        runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight" style="vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelNumber" runat="server"></ACA:AccelaLabel>
                                </div>
                           </td>
                        </tr>
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%; vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Lot" ID="APO_ParcelDetail_lbl_Parcel_Lot"
                                        runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight" style="vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelLot" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%;vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Block" ID="APO_ParcelDetail_lbl_Parcel_Block" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight" style="vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelBlock" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Subdivision" ID="APO_ParcelDetail_lbl_Parcel_Subdivision"
                                        runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelSubdivision" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="aca_parceldetail_label_status" ID="lblStatusTitle"
                                        runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblStatus" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%; vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Book" ID="APO_ParcelDetail_lbl_Parcel_Book"
                                        runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight" style="vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelBook" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Page" ID="APO_ParcelDetail_lbl_Parcel_Page"
                                        runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelPage" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%; vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Area" ID="APO_ParcelDetail_lbl_Parcel_Area"
                                        runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight" style="vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelArea" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%; vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Land" ID="APO_ParcelDetail_lbl_Parcel_Land"
                                        runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight" style="vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelLand" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Improved" ID="APO_ParcelDetail_lbl_Parcel_Improved"
                                        runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelImproved" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="ACA_AlignRightOrLeft" style="width:20%;vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Exemption" ID="APO_ParcelDetail_lbl_Parcel_Exemption"
                                        runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                            <td class="ACA_AlignLeftOrRight" style="vertical-align:text-bottom;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelExemption" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                        <asp:Repeater ID="rptAttribute" runat="server">
                            <ItemTemplate>
                            <tr>
                                <td class="ACA_AlignRightOrLeft" style="width:20%;" nowrap="nowrap"><div class="Header_h3"><%# Accela.ACA.Common.Common.ScriptFilter.FilterScript(Convert.ToString( DataBinder.Eval(Container.DataItem, "attributeLabel")))%>: </div></td>
                                <td class="ACA_AlignLeftOrRight">
                                    <div class="Header_h3">
                                        <table role='presentation'>
                                            <tr>
                                                <td>
                                                    <%# Accela.ACA.Common.Common.ScriptFilter.FilterScript(ModelUIFormat.GetTemplateValue4Display(Container.DataItem as TemplateAttributeModel))%>
                                                </td>
                                                <td>
                                                    <%# Accela.ACA.Common.Common.ScriptFilter.FilterScript(I18nStringUtil.GetString(Convert.ToString(DataBinder.Eval(Container.DataItem, "resAttributeUnitType")), Convert.ToString(DataBinder.Eval(Container.DataItem, "attributeUnitType")) ))%>
                                                </td>
                                            </tr>
                                        </table>
                                     </div>
                                 </td> 
                            </tr>
                            </ItemTemplate>
                         </asp:Repeater>                
                    </table>
                    </td><td id="tblParcelLegal" runat="server">
                    <table role='presentation' class="ACA_FullWidthTable">
                        <tr>
                            <td>
                                <h1>
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Legal" ID="APO_ParcelDetail_lbl_Parcel_Legal"
                                        runat="server"></ACA:AccelaLabel>
                                </h1>
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space: normal;word-wrap: break-word;word-break: normal;padding-top:10px;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblParcelLegal" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top:45px;">
                                <h1>
                                    <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Parcel_Tract" ID="APO_ParcelDetail_lbl_Parcel_Tract"
                                        runat="server"></ACA:AccelaLabel>
                                </h1>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100px;padding-top:10px;white-space: normal;word-wrap: break-word;word-break: normal;">
                                <div class="Header_h3">
                                    <ACA:AccelaLabel ID="lblparcelTract" runat="server"></ACA:AccelaLabel>
                                </div>
                            </td>
                        </tr>
                    </table>
                    </td></tr>
                    </table>
                </div>
                <div class="td_mini_map_right" id="dvMiniMap" runat="server">
                    <ACA:ACAMap ID="miniMapParcel" runat="server" IsMiniMode="true" AGISContext="ParcelDetail" OnShowOnMap="MapParcel_ShowOnMap" />
                </div>
              
                <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Session_Addresses" ID="APO_ParcelDetail_lbl_Session_Addresses"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                <div id="ucAddressList" runat="server" class="ACA_FLeft" style=" clear: both;">
                    <uc1:AddressList ID="addressList" GViewID="60047" IsShowMap="false" runat="server" OnPageIndexChanging="Address_GridViewIndexChanging" OnGridViewSort="Address_GridViewSort"/>
                </div>
                 
                <ACA:AccelaLabel LabelKey="APO_ParcelDetail_lbl_Session_Contacts" ID="APO_ParcelDetail_lbl_Session_Contacts"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                 
                <div id="ucOwnerList" runat="server" class="ACA_FLeft">
                    <uc1:OwnerList ID="ownerList" runat="server" OnPageIndexChanging="RefOwner_GridViewIndexChanging" OnGridViewSort="RefOwner_GridViewSort"/>
                </div>
                <div id="divRefGenealogyList" runat="server" visible="false" class="ACA_FLeft ACA_TabRow">
                    <uc1:RefGenealogyList ID="refGenealogyList" runat="server" />
                </div>
                
            </div>
        </div>
    </div>
</asp:Content>
