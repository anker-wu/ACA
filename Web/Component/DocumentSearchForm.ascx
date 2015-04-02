<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentSearchForm.ascx.cs" Inherits="Accela.ACA.Web.Component.DocumentSearchForm" %>
<%@ Register Src="~/Component/ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<ACA:ACAMap ID="mapDocuments" AGISContext="DocumentSearch" IsAutoShow="true" runat="server" />