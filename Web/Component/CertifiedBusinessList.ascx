<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CertifiedBusinessList.ascx.cs" Inherits="Accela.ACA.Web.Component.CertifiedBusinessList" %>
<%@ Import Namespace="Accela.ACA.Web.Common" %>
<br />
<asp:UpdatePanel ID="CertifiedBusinessPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvCertBusinessList" GridViewNumber="60120" runat="server" AllowPaging="true"
         SummaryKey="gdv_certbusiness_list_summary" CaptionKey="aca_caption_certbusiness_list"
            AllowSorting="true" AutoGenerateCheckBoxColumn="true" CheckBoxColumnIndex="0" ShowExportLink="true"
            ShowCaption="true" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" ShowShortListLink="true"
            OnGridViewSort="CertBusinessList_GridViewSort" OnRowCommand="CertBusinessList_RowCommand" OnPageIndexChanging="CertBusinessList_PageIndexChanging" 
            OnDataBound="CertBusinessList_DataBound" OnGridViewDownload="CertBusinessList_GridViewDownload">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkVendorHeader" ExportDataField="businessName">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkVendorHeader" runat="server" 
                            CommandName="Header" SortExpression="businessName" 
                            LabelKey="aca_certbusinesslist_label_vendor"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                         </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLinkButton id="lnkVendorName" runat ="server" CommandName="Action"
                            CommandArgument='<%# string.Format("{0},{1},{2},{3}", DataBinder.Eval(Container.DataItem, "seqNumber"), DataBinder.Eval(Container.DataItem, "stateLicense"), DataBinder.Eval(Container.DataItem, "licenseType"), DataBinder.Eval(Container.DataItem, "agencyCode")) %>' Text='<%# DataBinder.Eval(Container.DataItem, "businessName") %>'></ACA:AccelaLinkButton>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCertificationHeader" ExportDataField="certification">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkCertificationHeader" runat="server" 
                            CommandName="Header" SortExpression="certification" 
                            LabelKey="aca_certbusinesslist_label_certification"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                         </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCertification" Text='<%# DataBinder.Eval(Container.DataItem, "certification") %>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkOwnerEthnicityHeader" ExportDataField="ownerEthnicity">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkOwnerEthnicityHeader" runat="server" CommandName="Header"
                                SortExpression="ownerEthnicity" LabelKey="aca_certbusinesslist_label_ownerethnicity"
                                CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel id="lblOwnerEthnicity" Text='<%# DataBinder.Eval(Container.DataItem, "ownerEthnicity") %>' runat ="server"></ACA:AccelaLabel>                        
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <headerstyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFirstNameHeader" ExportDataField="contactFirstName">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkFirstNameHeader" runat="server" 
                            CommandName="Header" SortExpression="contactFirstName" 
                            LabelKey="aca_certbusinesslist_label_firstname"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "contactFirstName") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkMiddleNameHeader" ExportDataField="contactMiddleName">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkMiddleNameHeader" runat="server" 
                            CommandName="Header" SortExpression="contactMiddleName" 
                            LabelKey="aca_certbusinesslist_label_middlename"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblMiddleName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "contactMiddleName") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLastNameHeader" ExportDataField="contactLastName">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkLastNameHeader" runat="server" 
                            CommandName="Header" SortExpression="contactLastName" 
                            LabelKey="aca_certbusinesslist_label_lastname"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "contactLastName") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkTelphoneHeader" ExportDataField="phone1">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkTelphoneHeader" runat="server" 
                            CommandName="Header" SortExpression="phone1" 
                            LabelKey="aca_certbusinesslist_label_phone1"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblTelphoneName" IsNeedEncode="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "phone1") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFaxHeader" ExportDataField="fax">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkFaxHeader" runat="server" 
                            CommandName="Header" SortExpression="fax" 
                            LabelKey="aca_certbusinesslist_label_fax"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblFaxName" IsNeedEncode="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "fax") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkEmailHeader" ExportDataField="email">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkEmailHeader" runat="server" SortExpression="email" LabelKey="aca_certbusinesslist_label_email" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "email")%>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCityHeader" ExportDataField="city">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkCityHeader" runat="server" SortExpression="city" LabelKey="aca_certbusinesslist_label_city" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "city")%>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStateHeader" ExportDataField="state">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkStateHeader" runat="server" SortExpression="state" LabelKey="aca_certbusinesslist_label_state" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblState" runat="server" Text='<%# I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, "state") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string)%>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkZipHeader" ExportDataField="zip">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkZipHeader" runat="server" SortExpression="zip" LabelKey="aca_certbusinesslist_label_zip" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblZip" runat="server" Text='<%# ModelUIFormat.FormatZipShow(DataBinder.Eval(Container.DataItem, "zip").ToString(), DataBinder.Eval(Container.DataItem, "countryCode").ToString())%>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLargestContractHeader" ExportDataField="strLargestContract">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkLargestContractHeader" runat="server" SortExpression="largestContract" LabelKey="aca_certbusinesslist_label_largestcontract" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLargestContract" Text='<%# DataBinder.Eval(Container.DataItem, "strLargestContract")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkVendorDbaHeader" ExportDataField="vendorDba">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkVendorDbaHeader" runat="server" SortExpression="vendorDba" LabelKey="aca_certbusinesslist_label_vendordba" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblVendorDba" Text='<%# DataBinder.Eval(Container.DataItem, "vendorDba")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>                
                <ACA:AccelaTemplateField AttributeName="lnkContactNameHeader" ExportDataField="contactName">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkContactNameHeader" runat="server" SortExpression="contactName" LabelKey="aca_certbusinesslist_label_contactname" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblContactName" Text='<%# DataBinder.Eval(Container.DataItem, "contactName")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress2Header" ExportDataField="address2">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAddress2Header" runat="server" SortExpression="address2" LabelKey="aca_certbusinesslist_label_address2" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress2" Text='<%# DataBinder.Eval(Container.DataItem, "address2")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress3Header" ExportDataField="address3">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAddress3Header" runat="server" SortExpression="address3" LabelKey="aca_certbusinesslist_label_address3" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress3" Text='<%# DataBinder.Eval(Container.DataItem, "address3")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCountryHeader" ExportDataField="countryCode">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkCountryHeader" runat="server" SortExpression="countryCode" LabelKey="aca_certbusinesslist_label_country" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCountry" Text='<%# StandardChoiceUtil.GetCountryByKey(DataBinder.Eval(Container.DataItem, "countryCode") as string)%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkClientName1Header" ExportDataField="clientName1">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkClientName1Header" runat="server" LabelKey="aca_certbusinesslist_label_jobexp1"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblClientName1" Text='<%# DataBinder.Eval(Container.DataItem, "clientName1")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkJobValue1Header" ExportDataField="jobValue1">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkJobValue1Header" runat="server" LabelKey="aca_certbusinesslist_label_valueofjob"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblJobValue1" Text='<%# DataBinder.Eval(Container.DataItem, "jobValue1")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkWorkDate1Header" ExportDataField="workDate1">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkWorkDate1Header" runat="server" LabelKey="aca_certbusinesslist_label_dateofwork"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblWorkDate1" Text='<%# DataBinder.Eval(Container.DataItem, "workDate1")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkDescription1Header" ExportDataField="description1">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkDescription1Header" runat="server" LabelKey="aca_certbusinesslist_label_descriptionofwork"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblDescription1" Text='<%# DataBinder.Eval(Container.DataItem, "description1")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>                                
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkClientName2Header" ExportDataField="clientName2">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkClientName2Header" runat="server" LabelKey="aca_certbusinesslist_label_jobexp2"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblClientName2" Text='<%# DataBinder.Eval(Container.DataItem, "clientName2")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkJobValue2Header" ExportDataField="jobValue2">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkJobValue2Header" runat="server" LabelKey="aca_certbusinesslist_label_valueofjob"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblJobValue2" Text='<%# DataBinder.Eval(Container.DataItem, "jobValue2")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkWorkDate2Header" ExportDataField="workDate2">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkWorkDate2Header" runat="server" LabelKey="aca_certbusinesslist_label_dateofwork"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblWorkDate2" Text='<%# DataBinder.Eval(Container.DataItem, "workDate2")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkDescription2Header" ExportDataField="description2">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkDescription2Header" runat="server" LabelKey="aca_certbusinesslist_label_descriptionofwork"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblDescription2" Text='<%# DataBinder.Eval(Container.DataItem, "description2")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>                
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkClientName3Header" ExportDataField="clientName3">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkClientName3Header" runat="server" LabelKey="aca_certbusinesslist_label_jobexp3"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblClientName3" Text='<%# DataBinder.Eval(Container.DataItem, "clientName3")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkJobValue3Header" ExportDataField="jobValue3">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkJobValue3Header" runat="server" LabelKey="aca_certbusinesslist_label_valueofjob"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblJobValue3" Text='<%# DataBinder.Eval(Container.DataItem, "jobValue3")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkWorkDate3Header" ExportDataField="workDate3">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkWorkDate3Header" runat="server" LabelKey="aca_certbusinesslist_label_dateofwork"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblWorkDate3" Text='<%# DataBinder.Eval(Container.DataItem, "workDate3")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField HideField4Export="true" AttributeName="lnkDescription3Header" ExportDataField="description3">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkDescription3Header" runat="server" LabelKey="aca_certbusinesslist_label_descriptionofwork"></ACA:GridViewHeaderLabel>  
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblDescription3" Text='<%# DataBinder.Eval(Container.DataItem, "description3")%>' runat="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
<br />