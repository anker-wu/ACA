<%@ Page Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" Inherits="Accela.ACA.Web.GeneralProperty.GeneralDrillDown" ValidateRequest="false" Codebehind="GeneralDrillDown.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <asp:HiddenField ID="hdThreeDigitSelectedValue" runat="server" />
    <asp:HiddenField ID="hdFiveDigitSelectedValue" runat="server" />
    <!-- search -->
    <div id="divSearch" class="ACA_DrillDown_Search">
        <div class="ACA_FLeft">
            <ACA:AccelaTextBox ID="txtSearch" onkeypress="SearchNIGPCode(event)" LabelKey="aca_certbusiness_label_drilldown_searchtitle" LayoutType="Horizontal" runat="server"></ACA:AccelaTextBox>
        </div>
        <div class="ACA_LgButton ACA_LgButton_FontSize ACA_FLeft ACA_Magin_Top_Negative3">
            <ACA:AccelaButton ID="lnkSearch" OnClick="SearchButton_Click" LabelKey="aca_certbusiness_label_drilldown_action_search" runat="server" />
        </div>
    </div>
    <ACA:AccelaHeightSeparate Height="10" runat="server"></ACA:AccelaHeightSeparate>
    <!-- data list -->
    <div id="divDrillDownList" class="ACA_FullWidthTable ACA_DrillDown_Container">
        <ACA:AccelaGridView ID="gvDrillDownList" runat="server" SummaryKey="gdv_asit_drilldownlist_summary" CaptionKey="aca_caption_asit_drilldownlist"
         IsInSPEARForm="true" AutoGenerateColumns="false" Width="100%" HeaderStyle-BorderStyle="Ridge" GridLines="None" AllowPaging="True" HorizontalAlign="Center"
            OnRowDataBound="DrillDownList_RowDataBound" ShowCaption="true">
            <Columns>
                <ACA:AccelaTemplateField>
                    <HeaderTemplate>
                       <asp:CheckBox id="chkAll" onclick="SetSelectAll(this);" runat="server" Visible="False"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox id="chkSingle" onclick="SetSelectItems(this);" runat="server"/>
                    </ItemTemplate>
                    <HeaderStyle Width="25px" />
                    <ItemStyle Width="25px" />
                </ACA:AccelaTemplateField>
                 <ACA:AccelaTemplateField Visible="false">               
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ID") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField>
                  <HeaderTemplate>
                        <ACA:GridViewHeaderLabel ID="lblKeyHeader" runat="server" SortExpression="ClassCode"></ACA:GridViewHeaderLabel>
                   </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblClassCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClassCode") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="740px" />
                    <ItemStyle Width="740px" />
                </ACA:AccelaTemplateField>
            </Columns>
            <HeaderStyle BorderStyle="None" CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize ACA_AlignLeftOrRightTop" HorizontalAlign="Left" />
            <PagerStyle HorizontalAlign="Center" CssClass="ACA_Table_Pages ACA_Table_Pages_FontSize" VerticalAlign="Bottom" />
            <EmptyDataRowStyle CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
        </ACA:AccelaGridView>
    </div>
    <!-- button list -->
    <div id="divButtonList" class="ACA_TabRow">
    <table role='presentation'>
        <tr valign="bottom">
            <td id="tdNext" runat="server">
                <div class="ACA_LgButtonDisable ACA_LgButton_FontSize">
                    <ACA:AccelaButton ID="lnkNext" OnClick="NextButton_Click" LabelKey="aca_certbusiness_label_drilldown_action_next" runat="server" />
                </div>
            </td>
            <td id="tdFinish" runat="server">
                <div class="ACA_LgButtonDisable ACA_LgButton_FontSize">
                    <ACA:AccelaButton ID="lnkFinish" CssClass="NotShowLoading" LabelKey="aca_certbusiness_label_drilldown_action_finish" runat="server" />
                </div>
            </td>
            <td id="tdFinishSpace" runat="server" class="PopupButtonSpace">&nbsp;</td>
            <td id="tdBack" runat="server">
                <div class="ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkBack" OnClick="BackButton_Click" LabelKey="aca_certbusiness_label_drilldown_action_back" runat="server" />
                </div>
            </td>            
            <td class="PopupButtonSpace">&nbsp;</td>
            <td>
                <div class="ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkCancel" OnClientClick="parent.ACADialog.close(); return false;" LabelKey="aca_certbusiness_label_drilldown_action_cancel" runat="server" />
                </div>
            </td>
        </tr>
    </table>
    </div>

    <script type="text/javascript">
        var splitChar = '<%= SplitChar %>';
        var hdThreeDigitSelectedValue = $('#<%= hdThreeDigitSelectedValue.ClientID %>');
        var hdFiveDigitSelectedValue = $('#<%= hdFiveDigitSelectedValue.ClientID %>');        

        // focus on search text box.
        $('#<%=txtSearch.ClientID %>').focus();
        
        // when load, check the button list status, ex: page index changed.
        var result = '';
        if ('<%=StepType %>' == '<%=STEP_TYPE_FIRST %>') {
            result = hdThreeDigitSelectedValue.val();
        }
        else {
            result = hdFiveDigitSelectedValue.val();
        }

        // set the check status with the exist value
        if ('<%= IsPostBack%>' == 'False') {
            result = parent.getCommodityClass(3);

            if (result != '') {
                var fiveDigitValue = parent.getCommodityClass();

                hdThreeDigitSelectedValue.val(result);
                hdFiveDigitSelectedValue.val(fiveDigitValue);

                $(':checkbox').each(function () {
                    if (result.indexOf(splitChar + $(this).val() + splitChar) != -1) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }

        // set the button list status
        if (result == null || result == '') {
            SetButtonListStatus(true);
        }
        else {
            SetButtonListStatus(false);
        }

        function SetSelectAll(obj) {
            var checked = $(obj).attr('checked');
            var checkboxs = $(":checkbox").not(obj);

            checkboxs.attr('checked', checked);

            $.each(checkboxs, function () {
                SetSelectItems(this);
            });
        }

        function SetSelectItems(obj) {
            var result = '';
            if ('<%=StepType %>' == 'FirstStep') {
                result = hdThreeDigitSelectedValue.val();
            }
            else {
                result = hdFiveDigitSelectedValue.val();
            }

            var value = $(obj).attr('value');
            result = GetCurrentCheckedValues(obj, value, result, splitChar);

            if ('<%=StepType %>' == 'FirstStep') {
                hdThreeDigitSelectedValue.val(result);
            }
            else {
                hdFiveDigitSelectedValue.val(result);
            }

            // set the button list status
            if (result == null || result == '') {
                SetButtonListStatus(true);
            }
            else {
                SetButtonListStatus(false);
            }
        }

        function SetResult() {
            var result = hdFiveDigitSelectedValue.val();
            
            if (result != null && $.trim(result) != "") {
                var resultArray = result.split(splitChar);
                var length = 0;

                for (var i = 0; i < resultArray.length; i++) {
                    if (resultArray[i] != null && resultArray[i] != '') {
                        length++;
                    }
                }

                // Restrict selected records of NIGP sub class within 200.
                if (length > 200) {
                    var notice = '<%= LabelUtil.GetGUITextByKey("aca_certbusiness_msg_largestrecordsnotice") %>';
                    notice = notice.replace('{0}', length);
                    alert(notice);

                    return false;
                }

                resultArray.sort();

                parent.addCommodityClassCallBack(resultArray);
                parent.ACADialog.close();
            }
        }

        function SetButtonListStatus(disabled) {
            // set the button 'Next' and 'Finish' enable and disable
            var lnkNext = $('#<%= lnkNext.ClientID %>');
            var lnkFinish = $('#<%= lnkFinish.ClientID %>');

            if (disabled) {
                if ($.exists(lnkNext)) {
                    SetControlDisabled(lnkNext, true);
                }

                if ($.exists(lnkFinish)) {
                    SetControlDisabled(lnkFinish, true);
                }
            }
            else {
                if ($.exists(lnkNext)) {
                    SetControlDisabled(lnkNext, false);
                }

                if ($.exists(lnkFinish)) {
                    SetControlDisabled(lnkFinish, false);

                    // bind the button 'Finish's click event
                    $(lnkFinish).click(function (e) {
                        if (SetResult() == false) {
                            return false;
                        };
                    })
                }
            }
        }

        function SetControlDisabled(obj, disabled) {
            if (disabled) {
                obj.attr('disabled', 'disabled');
                obj.click(function () { return false; });
                obj.parent().removeClass();
                obj.parent().addClass('ACA_LgButtonDisable ACA_LgButton_FontSize');
            }
            else {
                obj.removeAttr('disabled');
                obj.unbind("click");
                obj.parent().removeClass();
                obj.parent().addClass('ACA_LgButton ACA_LgButton_FontSize');
            }
        }

        function SearchNIGPCode(evt) {
            var event = evt ? evt : (window.event ? window.event : null);
            var keyCode = event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode);

            if (event.keyCode == 13) {
                __doPostBack('<%=lnkSearch.ClientID %>'.replace(/_/g, '$'));
            }
        }
    </script>
</asp:Content>
