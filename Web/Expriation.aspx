<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="Accela.ACA.Web.Expriation" Codebehind="Expriation.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <table role='presentation' width="730" cellpadding="0" cellspacing="0" border="0">
        <!-- Error title -->
        <tr>
            <td id="infoIconAlign" width="60" class="ACA_AlignLeftOrRight" valign="top" rowspan="2">
            </td>
            <td id="mainTitleAlign" valign="middle" class="ACA_AlignLeftOrRight" width="*">
                <h1 id="mainTitle"><%=LabelUtil.GetGlobalTextByKey("aca_expriation_title") %></h1>
            </td>
        </tr>
        <tr>
            <!-- This row is for HTTP status code, as well as the divider-->
            <td id="errorCodeAlign" class="errorCodeAndDivider ACA_AlignRightOrLeft">
                &nbsp;
                <div class="divider">
                </div>
            </td>
        </tr>
        <!-- Error Body -->
        <!-- What you can do -->
        <tr>
            <td>
                &nbsp;
            </td>
            <td id="likelyCausesAlign" valign="top" class="ACA_AlignLeftOrRight">
                <div class="Header_h3" id="likelyCauses"><%=LabelUtil.GetGlobalTextByKey("aca_expriation_reasons_title") %></div>
                <ul>
                    <li id="causeErrorInAddress"><%=LabelUtil.GetGlobalTextByKey("aca_expriation_reasons_content") %></li>
                </ul>
            </td>
        </tr>
        
    </table>
</asp:Content>
