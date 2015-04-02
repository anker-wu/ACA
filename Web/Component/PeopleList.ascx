<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PeopleList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.PeopleList" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<asp:UpdatePanel ID="peopleListUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="dgvPeopleList" runat="server" ShowExportLink="true" Width="77em"
            GridViewNumber="60106" AllowPaging="true" AllowSorting="true" ShowCaption="true"
            AutoGenerateColumns="false" OnRowCommand="PeopleList_RowCommand" PagerStyle-HorizontalAlign="center"
            SummaryKey="gdv_trustaccountdetail_peoplelist_summary" CaptionKey="aca_caption_trustaccountdetail_peoplelist" 
            OnRowDataBound="PeopleList_RowDataBound">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="InkPeopleType">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="InkPeopleType" runat="server" CommandName="Header" SortExpression="Type"
                                LabelKey="per_peoplelist_Type" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblPeopleType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.Type.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFirstName">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkFirstName" runat="server" LabelKey="per_peoplelist_firstname"
                                SortExpression="FirstName" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblFirstName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.FirstName.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkMiddleName">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkMiddleName" runat="server" LabelKey="per_peoplelist_middlename"
                                SortExpression="MiddleName" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblMiddleName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.MiddleName.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLastName">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkLastName" runat="server" LabelKey="per_peoplelist_lastname"
                                SortExpression="LastName" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLastName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.LastName.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress1">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAddress1" runat="server" LabelKey="per_peoplelist_address1"
                                SortExpression="Address1" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.Address1.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress2">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAddress2" runat="server" LabelKey="per_peoplelist_address2"
                                SortExpression="Address2" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.Address2.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress3">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAddress3" runat="server" LabelKey="per_peoplelist_address3"
                                SortExpression="Address3" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress3" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.Address3.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPhone1">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkPhone1" runat="server" LabelKey="per_peoplelist_phone1"
                                SortExpression="Phone1" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblPhone1" runat="server" IsNeedEncode="false" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.Phone1.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPhone2">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkPhone2" runat="server" LabelKey="per_peoplelist_phone2"
                                SortExpression="Phone2" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblPhone2" runat="server" IsNeedEncode="false" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.Phone2.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFax">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkFax" runat="server" LabelKey="per_peoplelist_fax"
                                SortExpression="Fax" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblFax" runat="server" IsNeedEncode="false" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.Fax.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkEmail">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkEmail" runat="server" LabelKey="per_peoplelist_email"
                                SortExpression="Email" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblEmail" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.Email.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="160px" />
                    <HeaderStyle Width="160px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseNumber">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkLicenseNumber" runat="server" LabelKey="per_peoplelist_licensenumber"
                                SortExpression="LicenseNumber" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLicenseNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.LicenseNumber.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="120px" />
                    <HeaderStyle Width="120px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseExpirationDate">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkLicenseExpirationDate" runat="server" LabelKey="per_peoplelist_licenseexpirationdate"
                                SortExpression="LicenseExpirationDate" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaDateLabel ID="lblLicenseExpirationDate" runat="server" DateType="ShortDate"
                                Text2='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.LicenseExpirationDate.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="170px" />
                    <HeaderStyle Width="170px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCountryOrRegion">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkCountryOrRegion" runat="server" LabelKey="per_peoplelist_country"
                                SortExpression="CountryOrRegion" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCountryOrRegion" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.CountryOrRegion.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="120px" />
                    <HeaderStyle Width="120px" />
                </ACA:AccelaTemplateField>
				<ACA:AccelaTemplateField AttributeName="lnkFullName">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkFullName" runat="server" LabelKey="aca_peoplelist_label_fullname"
                                SortExpression="FullName" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblFullName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.PeopleList.FullName.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
