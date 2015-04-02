<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefLicenseList.ascx.cs" Inherits="Accela.ACA.Web.Component.RefLicenseList" %>
<div id="divLicense" class="ACA_Row">
    <ACA:AccelaGridView ID="gdvLicenseList" runat="server" IsInSPEARForm="true"  
    SummaryKey="gdv_licenseedit_licenselist_summary" CaptionKey="gdv_licenseedit_licenselist_summary" Caption="aca_caption_licenseedit_licenselist"
        PagerStyle-HorizontalAlign="center" AllowPaging="true" AllowSorting="true" OnRowDataBound="LicenseList_RowDataBound"
        OnRowCommand="LicenseList_RowCommand" ShowCaption="true" GridViewNumber="60042" CheckBoxColumnIndex="0"
        OnClientSelectAll="MultipleLicense_Selected();" OnClientSelectSingle="MultipleLicense_Selected();"
        OnPageIndexChanging="LicenseList_PageIndexChanging" OnGridViewSort="LicenseList_GridViewSort">
        <Columns>
            <ACA:AccelaTemplateField AttributeName="lnkLicenseProHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkLicenseProHeader" runat="server" 
                                    CommandName="Header" SortExpression="LicenseNumber" 
                                    LabelKey="LicenseEdit_LicensePro_LicenseList_License"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblLicenseNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseNumber") %>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="160px" />
                <HeaderStyle Width="160px" />
            </ACA:AccelaTemplateField>
            <%-- begin license expired status--%>
            <ACA:AccelaTemplateField AttributeName="lnkLienseStatusHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkLienseStatusHeader" runat="server" CommandName="Header"
                             SortExpression="IsLicExpired" CausesValidation="false"
                             LabelKey="licenseedit_licensepro_licenselist_licensestatus" />                         
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                              <ACA:AccelaLabel ID="lblLicenseStatus" runat="server" 
                              Text='<%# LicenseUtil.GetLicenseStatus(Convert.ToBoolean(Eval("IsLicExpired"))) %>'/>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="130px" />
                <HeaderStyle Width="130px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkInsuranceStatusHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkInsuranceStatusHeader" runat="server" CommandName="Header"
                             SortExpression="IsInsExpired" CausesValidation="false" 
                             LabelKey="licenseedit_licensepro_licenselist_insurancestatus"/>                         
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                              <ACA:AccelaLabel ID="lblInsuranceStatus" runat="server" 
                              Text='<%# LicenseUtil.GetLicenseStatus(Convert.ToBoolean(Eval("IsInsExpired"))) %>'/>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="130px" />
                <HeaderStyle Width="130px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkBusinessLicStatusHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkBusinessLicStatusHeader" runat="server" CommandName="Header"
                             SortExpression="IsBusLicExpired" CausesValidation="false"
                             LabelKey="licenseedit_licensepro_licenselist_businesslicstatus"/>                         
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                              <ACA:AccelaLabel ID="lblBusinessLicStatus" runat="server" 
                              Text='<%# LicenseUtil.GetLicenseStatus(Convert.ToBoolean(Eval("IsBusLicExpired"))) %>'/>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="160px" />
                <HeaderStyle Width="160px" />
            </ACA:AccelaTemplateField>
            <%-- end license expired status--%>
            <ACA:AccelaTemplateField AttributeName="lnkLicenseTypeHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkLicenseTypeHeader" runat="server" 
                                    CommandName="Header" SortExpression="LicenseType" 
                                    LabelKey="LicenseEdit_LicensePro_LicenseList_LicenseType"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblLicenseType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseTypeText") %>'></ACA:AccelaLabel>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="130px" />
                <HeaderStyle Width="130px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkContactTypeHeader">
                <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkContactTypeHeader" runat="server" 
                                    CommandName="Header" SortExpression="ContactType" 
                                    LabelKey="licenseedit_licenselist_contacttype"  
                                    CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                    </headertemplate>
                    <itemtemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblContactType" runat="server" Text='<%# DropDownListBindUtil.GetTypeFlagTextByValue(Convert.ToString(DataBinder.Eval(Container.DataItem, "ContactType"))) %>'></ACA:AccelaLabel>
                                </div>
                   </itemtemplate>
                   <ItemStyle Width="100px"/>
                   <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkSSNHeader">
                    <headertemplate>
                                <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkSSNHeader" runat="server" 
                                    CommandName="Header" SortExpression="MaskedSSN" 
                                    LabelKey="licenseedit_licenselist_ssn"  
                                    CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                    </headertemplate>
                    <itemtemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblSSN" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaskedSSN") %>'></ACA:AccelaLabel>
                                </div>
                   </itemtemplate>
                   <ItemStyle Width="100px"/>
                   <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFEINHeader">
                    <headertemplate>
                                <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkFEINHeader" runat="server" 
                                    CommandName="Header" SortExpression="FEIN" 
                                    LabelKey="licenseedit_licenselist_fein"  
                                    CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                    </headertemplate>
                    <itemtemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblFEIN" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MaskedFEIN") %>'></ACA:AccelaLabel>
                                </div>
                   </itemtemplate>
                   <ItemStyle Width="100px"/>
                   <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseNameHeader">
                    <headertemplate>
                                <div class="ACA_Header_Row">
                                <ACA:GridViewHeaderLabel ID="lnkLicenseNameHeader" runat="server" 
                                    CommandName="Header" SortExpression="ContractName" 
                                    LabelKey="LicenseEdit_LicensePro_LicenseList_LicenseName"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </headertemplate>
                    <itemtemplate>
                                    <div>
                                    <ACA:AccelaLabel ID="lblLicenseName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ContractName").ToString() %>'></ACA:AccelaLabel>
                                     </div>
                             </itemtemplate>
                    <ItemStyle Width="200px"/>
                    <headerstyle Width="200px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBusinessNameHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBusinessNameHeader" runat="server" CommandName="Header"
                                SortExpression="BusinessName" LabelKey="licenseedit_licensepro_licenselist_businessname"
                                CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblBusinessName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BusinessName") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="200px" />
                    <headerstyle Width="200px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBusinessLicenseHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBusinessLicenseHeader" runat="server" CommandName="Header"
                                SortExpression="BusinessLicense" LabelKey="licenseedit_licensepro_licenselist_businesslicense"
                                CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblBusinessLicense" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BusinessLicense") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <headerstyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkEmailHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkEmailHeader" runat="server" CommandName="Header" 
                                SortExpression="Email" LabelKey="licenseedit_licensepro_licenselist_email"
                                CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblEmail" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Email")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="160px" />
                    <HeaderStyle Width="160px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <div id="divConditionMessage" style="display: none">
        </div>
        <div id="divConditionInfo" style="display: none">
        <ACA:AccelaLabel ID="lblLicenseConditionTitle" LabelKey="aca_permit_licenseprofessional_conditions_title" runat="server" LabelType="SectionTitle"/>
        <div id="divConditionList">
        </div>
    </div>
</div>
<script type="text/javascript">
    function SingleSelectLicense4RefLicenseList(obj) {
        if (typeof(GetCurrentContinueButtonClientID) != 'undefined'){
            var ctrId = GetCurrentContinueButtonClientID();
            SetWizardButtonDisable(ctrId, false);
        }
    }

    function MultipleLicense_Selected() {
        if (typeof(GetCurrentContinueButtonClientID) != 'undefined') {
            var ctrId = GetCurrentContinueButtonClientID();
            var selectedValues = $("#<%=gdvLicenseList.GetSelectedItemsFieldClientID() %>").val();
            var disabled = selectedValues == null || selectedValues == "," ? true : false;
            SetWizardButtonDisable(ctrId, disabled);
        }
    }
</script>