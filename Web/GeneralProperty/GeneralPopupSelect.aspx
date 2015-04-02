<%@ Page Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" Inherits="Accela.ACA.Web.GeneralProperty.GeneralPopupSelect" ValidateRequest="false" Codebehind="GeneralPopupSelect.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <asp:HiddenField ID="hdSelectedValue" runat="server" />
    <ACA:AccelaLabel ID="lblPageInstruction" runat="server"></ACA:AccelaLabel>
    <div class="ACA_PopupSelect_Container">
        <ACA:AccelaGridView ID="gvPopupList" runat="server" IsInSPEARForm="true" 
            SummaryKey="gdv_addressedit_addresslist_summary" CaptionKey="aca_caption_addressedit_addresslist"
            AllowPaging="true" AllowSorting="true" PagerStyle-HorizontalAlign="center" ShowCaption="true" OnRowDataBound="PopupList_RowDataBound">
            <Columns>
                <ACA:AccelaTemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkAll" onclick="SetSelectAll(this);" Visible="False" runat="server"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk" onclick="SetSelectItems(this);" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle Width="25px" />
                    <ItemStyle Width="25px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField>
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lblKeyHeader" runat="server" SortExpression="Key"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblKey" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Key") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                 <ACA:AccelaTemplateField>
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lblValueHeader" runat="server" SortExpression="Value"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Value") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </div>

    <!-- button list -->
    <div id="divButtonList" class="ACA_TabRow">
    <table role='presentation'>
        <tr valign="bottom">
            <td id="tdZipCodeSubmit">
                <div class="ACA_LgButton ACA_LgButton_FontSize">
                    <ACA:AccelaButton ID="lnkZipCodeSubmit" CssClass="NotShowLoading" LabelKey="aca_certbusiness_label_popup_action_submit" runat="server" />
                </div>
            </td>
            <td class="PopupButtonSpace">&nbsp;</td>
            <td>
                <div class="ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkZipCodeCancel" CssClass="NotShowLoading" OnClientClick="parent.ACADialog.close(); return false;" LabelKey="aca_certbusiness_label_popup_action_cancel" runat="server" />
                </div>
            </td>
        </tr>
    </table>
    </div>

    <script type="text/javascript">
        var splitChar = '<%= Accela.ACA.Common.ACAConstant.COMMA %>';
        var hdSelectedValue = $('#<%= hdSelectedValue.ClientID %>');
        var result = $.trim(hdSelectedValue.val());        

        // set the check status with the exist value
        if ('<%= IsPostBack%>' == 'False') {
            if ('<%= Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(SelectPageValue) %>' == '<%= URL_SELECT_PAGE_LOCATION %>') {
                result = parent.getLocation();
            }
            else if ('<%= Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(SelectPageValue) %>' == '<%= URL_SELECT_PAGE_ZIPCODE %>') {
                result = parent.getZipCode();
            }

            if (result != '') {
                hdSelectedValue.val(result);

                $(':checkbox').each(function () {
                    if (result.indexOf(splitChar + $(this).val() + splitChar) != -1) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }

        var isEnableButton = result == null || result == '';
        SetButtonListStatus(isEnableButton);

        function SetSelectAll(obj) {
            var checked = $(obj).attr('checked');
            var checkboxs = $(":checkbox").not(obj);

            checkboxs.attr('checked', checked);

            $.each(checkboxs, function () {
                SetSelectItems(this);
            });
        }

        function SetSelectItems(obj) {
            var result = $.trim(hdSelectedValue.val());
            var value = $(obj).attr('value');

            result = GetCurrentCheckedValues(obj, value, result, splitChar);

            hdSelectedValue.val(result);

            var isEnableButton = result == null || result == '';

            SetButtonListStatus(isEnableButton);
        }

        function SetResult() {
            var result = $.trim(hdSelectedValue.val());

            if (result != null && $.trim(result) != "") {
                var resultArray = result.split(splitChar);
                resultArray.sort();

                if ('<%= Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(SelectPageValue) %>' == '<%= URL_SELECT_PAGE_LOCATION %>') {
                    parent.addLocationCallBack(resultArray);
                }
                else if ('<%= Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(SelectPageValue) %>' == '<%= URL_SELECT_PAGE_ZIPCODE %>') {
                    parent.addZipCodeCallBack(resultArray);
                }                
            }

            parent.ACADialog.close();
        }

        function SetButtonListStatus(disabled) {
            var lnkSubmit = $('#<%= lnkZipCodeSubmit.ClientID %>');

            if (disabled) {
                if ($.exists(lnkSubmit)) {
                    SetControlDisabled(lnkSubmit, true);
                }
            }
            else {
                if ($.exists(lnkSubmit)) {
                    SetControlDisabled(lnkSubmit, false)
                }
            }
        }

        function SetControlDisabled(obj, disabled) {
            if (disabled) {
                obj.attr('disabled', 'disabled');
                obj.unbind('click').click(function () { return false; });
                obj.parent().removeClass();
                obj.parent().addClass('ACA_LgButtonDisable ACA_LgButton_FontSize');
            }
            else {
                obj.removeAttr('disabled');
                obj.unbind('click');
                obj.bind('click', function () {
                    if (SetResult() == false) {
                        return false;
                    }
                });
                obj.parent().removeClass();
                obj.parent().addClass('ACA_LgButton ACA_LgButton_FontSize');
            }
        }

    </script>
</asp:Content>
