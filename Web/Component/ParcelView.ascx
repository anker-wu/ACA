<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ParcelView" Codebehind="ParcelView.ascx.cs" %>
<%@ Register Src="~/Component/TemplateView.ascx" TagName="TemplateView" TagPrefix="ucl" %>
<div id="divParcelNo" runat="server" visible="false" class="fontbold">
    <table role='presentation' class="font11px color666">
        <tr>
            <td>
                <ACA:AccelaLabel ID="txtParcelNoLabel" runat="server" LabelKey="per_parcel_label_parcelNo">
                </ACA:AccelaLabel>
            </td>
            <td>
                <ACA:AccelaLabel ID="txtParcelNo" IsNeedEncode="false" runat="server">
                </ACA:AccelaLabel>
            </td>
        </tr>
    </table>
</div>
<div id="divLot" visible ="false" runat="server">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtLotLabel" runat="server" LabelKey="per_parcel_label_lot">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtLot" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divBlock" visible ="false" runat="server">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtBlockLabel" runat="server" LabelKey="per_parcel_label_block">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtBlock" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divSubdivision" visible ="false" runat="server">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtSubdivisionLabel" runat="server" LabelKey="per_parcel_label_subdivision">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtSubdivision" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divBook" visible ="false" runat="server">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtBookLabel" runat="server" LabelKey="per_parcel_label_book">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtBook" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divPage" runat="server" visible="false">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtPageLabel" runat="server" LabelKey="per_parcel_label_page">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtPage" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divTract" runat="server" visible="false">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtTractLabel" runat="server" LabelKey="per_parcel_label_tract">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtTract" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divLegalDescription" runat="server" visible="false">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtLegalDescriptionLabel" runat="server" LabelKey="per_parcel_label_legalDescription">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtLegalDescription" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divParcelArea" runat="server" visible="false">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtParcelAreaLabel" runat="server" LabelKey="per_parcel_label_parcelArea">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtParcelArea" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divLandValue" runat="server" visible="false">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtLandValueLabel" runat="server" LabelKey="per_parcel_label_landValue">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtLandValue" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divImprovedValue" runat="server" visible="false">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtImprovedValueLabel" runat="server" LabelKey="per_parcel_label_improvedValue">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtImprovedValue" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divExceptionValue" runat="server" visible="false">
    
        <table role='presentation' class="font11px color666">
            <tr>
                <td>
                    <ACA:AccelaLabel ID="txtExceptionValueLabel" runat="server" LabelKey="per_parcel_label_exceptionValue">
                    </ACA:AccelaLabel>
                </td>
                <td>
                    <ACA:AccelaLabel ID="txtExceptionValue" IsNeedEncode="false" runat="server">
                    </ACA:AccelaLabel>
                </td>
            </tr>
        </table>
    
</div>
<div id="divTemplateView" runat="server" visible="false">
    <ucl:TemplateView ID="templateView" runat="server"  />
</div>
