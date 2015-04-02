<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="AnnouncementList.aspx.cs" 
Inherits="Accela.ACA.Web.Announcement.AnnouncementList" %> 
<%@ Register src="~/Component/AnnouncementList.ascx" tagname="AnnouncementListComponent" tagprefix="uc1" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" Runat="Server"> 
    <div class="ACA_Content">
         <div class="ACA_TabRow">
            <ACA:AccelaLabel ID="lblAnnouncements" LabelKey="announcement_listmanagement_announcementsname" runat="server" LabelType="SubSectionText" />
        </div>
        <div>
            <uc1:AnnouncementListComponent ID="AnnouncementListComponent" runat="server" /> 
        </div> 
    </div>
</asp:Content>

