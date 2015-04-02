<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../Dialog.Master" CodeBehind="DrillDown.aspx.cs" Inherits="Accela.ACA.Web.ASIT.DrillDown" %>
<asp:content id="Content1" contentplaceholderid="phPopup" runat="server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <script language="javascript" type="text/javascript">
        var selectedItems = "";
        var btnNextScript = "";
        var btnFinishScript = "";

        function SetCurrentSelectedItems() {
            selectedItems = $get("<%=hdnSelectedItems.ClientID %>").value;
        }

        function EnableOperationButton(obj) {
            var btnNext = $get("<%=btnNext.ClientID %>");
            var btnFinish = $get("<%=btnFinish.ClientID %>");
            var isChecked = false;
            if (obj.type == "checkbox") {
                var tb = $get("<%=drillDownList.ClientID %>");
                for (j = 1; j < tb.rows.length; j++) {
                    var chk = null;
                    chk = tb.rows[j].cells[0].getElementsByTagName('input')[0];

                    if (chk != null) {
                        if (chk.checked) {
                            isChecked = true;
                            SetSelectedItems(chk.parentNode, true);
                            continue;
                        }
                        else {
                            SetSelectedItems(chk.parentNode, false);
                        }
                    }
                }
            }
            else if (obj.type == "radio") {
                var objs = document.getElementsByName(obj.name);
                for (i = 0; i < objs.length; i++) {
                    if (objs[i].checked) {
                        isChecked = true;
                        SetSelectedItems(objs[i], true);
                        continue;
                    }
                    else {
                        SetSelectedItems(objs[i], false);
                    }
                }
            }
            if (selectedItems != "") {
                isChecked = true;
            }
            if (isChecked) {
                if (btnNext != null) {
                    btnNext.disabled = false;
                    DELinkButton(btnNext, false, true);
                }
                if (btnFinish != null) {
                    btnFinish.disabled = false;
                    DELinkButton(btnFinish, false, false);
                }
            }
            else {
                if (btnNext != null) {
                    DELinkButton(btnNext, true, true);
                    btnNext.disabled = true;
                }
                if (btnFinish != null) {
                    DELinkButton(btnFinish, true, false);
                    btnFinish.disabled = true;
                }
            }
        }

        //Update all selected items into variable selectedItems . 
        //It inculde row index of all page.
        function SetSelectedItems(rowObj, isAdd) {
            var rowIndex;
            if (typeof (rowObj) != "undefined") {
                if (typeof (rowObj.attributes["rowindex"]) != "undefined") {
                    rowIndex = rowObj.attributes["rowindex"].value;
                }
                else {
                    rowIndex = rowObj.rowIndex;
                }
            }

            var strIndex = selectedItems.indexOf(rowIndex);
            if (isAdd && strIndex == -1) {
                selectedItems += rowIndex;
            }
            else if (!isAdd) {
                if (strIndex > -1) {
                    selectedItems = selectedItems.replace(rowIndex, "");
                }
            }
        }

        function DELinkButton(ctl, isDisabled, isNextBtn) {
            if (isFireFox()) {
                if (String(btnNextScript) == "" && isNextBtn)
                    btnNextScript = ctl.href;
                if (String(btnFinishScript) == "" && !isNextBtn)
                    btnFinishScript = ctl.href;
                var linkScript = isNextBtn ? btnNextScript : btnFinishScript;
                if (isDisabled) {
                    ctl.href = "JavaScript:;";
                    ctl.setAttribute('disabled', 'disabled');
                    ctl.parentNode.className = "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize ACA_FLeft";
                }
                else {
                    ctl.href = linkScript;
                    ctl.removeAttribute('disabled', 0);
                    ctl.parentNode.className = "ACA_LgButton ACA_LgButton_FontSize ACA_FLeft";
                }
            }
            else {
                if (isDisabled) {
                    ctl.setAttribute('disabled', 'disabled');
                    ctl.parentNode.className = "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize ACA_FLeft";
                }
                else {
                    ctl.removeAttribute('disabled', 0);
                    ctl.parentNode.className = "ACA_LgButton ACA_LgButton_FontSize ACA_FLeft";
                }
            }
        }

        function ENbtnServerClick() {
            var isChecked = selectedItems && selectedItems != "" ? true : false;
            var tb = $get("<%=drillDownList.ClientID %>");
            
            for (j = 1; j < tb.rows.length; j++) {
                var chk = $(tb.rows[j].cells[0]).find("input[type='radio']")[0];

                //if chk is not undefined so chk is radio button else  the chk is check box button 
                if (typeof (chk) == "undefined") {
                    chk = $(tb.rows[j].cells[0]).find("input[type='checkbox']")[0];
                }

                if (typeof (chk) != "undefined" && chk.checked) {
                    isChecked = true;
                    break;
                }
            }
            
            selectedItems = "";
            return isChecked;
        }

        function Cancel() {
            //Render expression result to new added fields.
            if (parent.ASITInsertFieldCollection) {
                parent.ASITInsertFieldCollection = [];
            }

            parent.ASIT_ClearUIData();
            parent.ACADialog.close();
            return false;
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_pageLoaded(function (sender, args) {
            var dialogCloseBtn = parent.$get(parent.ACADialog.close_id);

            if (dialogCloseBtn) {
                dialogCloseBtn.onclick = function() {
                    var parentIframe = parent.$get(parent.ACADialog.iframe_id);

                    if (parentIframe) {
                        parent.SetNotAskForSPEAR();
                        parentIframe.contentWindow.Cancel();
                    }

                    return false;
                };
            }
        });
    </script>
    <div class="asit_drilldown">
        <table role='presentation' border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr class="ACA_FLeft" style="width:100%">
                <td style="width:50%" class="ACA_FLeft">
                    <div class="columnName">
                        <ACA:AccelaLabel runat="server" IsNeedEncode="false" ID="lblColumnName"></ACA:AccelaLabel>
                    </div>
                    <div>
                        <ACA:AccelaLabel ID="lblSingleOrMulti" IsNeedEncode="false" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                    </div>
                </td>
                <td style="width: 50%;" class="ACA_FRight">
                    <table role='presentation' width="100%">
                        <tr>
                            <td valign="middle" class="ACA_AlignRightOrLeft">
                                <asp:Label ID="lblSearchName" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></asp:Label>
                                <asp:TextBox ID="txtSearchContent" runat="Server"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="reqSearch" ControlToValidate="txtSearchContent" runat="server" ErrorMessage="*"
                                    ValidationGroup="searchDrillDown" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                            <td class="ACA_AlignRightOrLeft">
                                <div class="ACA_LgButton ACA_LgButton_FontSize ACA_FLeft">
                                    <ACA:AccelaLinkButton ID="btnSearch" runat="server" LabelKey="aca_sys_drill_down_search_btn"
                                        OnClick="BtnSearch_Click" ValidationGroup="searchDrillDown" IsDisplayLabel="False" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <ACA:AccelaLabel ID="lblDDTip" runat="server" IsNeedEncode="false" CssClass="ACA_New_Label font11px"></ACA:AccelaLabel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <ACA:AccelaLabel ID="lblSelectPath" runat="server" IsNeedEncode="false" CssClass="breadcrumb"></ACA:AccelaLabel>
                </td>
            </tr>
        </table>
        <div id="divDrillDownList">
            <ACA:AccelaGridView ID="drillDownList" runat="server" OnRowDataBound="DrillDownList_RowDataBound"
                DataKeyNames="ID" IsInSPEARForm="true" SummaryKey="gdv_asit_drilldownlist_summary" CaptionKey="aca_caption_asit_drilldownlist"
                AutoGenerateColumns="true" Width="100%" HeaderStyle-BorderStyle="Ridge" GridLines="None" OnPreRender="DrillDownList_PreRender"
                AllowPaging="True" OnPageIndexChanging="DrillDownList_PageIndexChanging" HorizontalAlign="Center">
                <Columns>
                    <ACA:AccelaTemplateField>
                        <ItemTemplate>
                            <%if (IsSingleSection)
                              {%>
                            <asp:Literal ID="RadioButtonMarkup" runat="server"></asp:Literal><%}%>
                            <%else
                                {%>
                            <asp:CheckBox ID="chkSelectDrill" runat="server" /><%} %>
                        </ItemTemplate>
                        <HeaderStyle Width="20px" />
                        <ItemStyle Width="20px" />
                    </ACA:AccelaTemplateField>
                </Columns>
                <HeaderStyle BorderStyle="None" CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize ACA_AlignLeftOrRightTop"
                    HorizontalAlign="Left" />
                <PagerStyle HorizontalAlign="Center" CssClass="ACA_Table_Pages ACA_Table_Pages_FontSize"
                    VerticalAlign="Bottom" />
                <EmptyDataRowStyle CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
            </ACA:AccelaGridView>
        </div>
        <div class="ACA_TabRow">
            <table role='presentation'>
                <tr valign="bottom">
                    <td>
                        <div class="ACA_LgButton ACA_LgButton_FontSize ACA_FLeft">
                            <ACA:AccelaLinkButton ID="btnBack" runat="server" LabelKey="aca_asi_table_drill_down_back_action"
                                OnClick="BtnBack_Click" CausesValidation="false" />
                        </div>
                    </td>
                    <td>&nbsp;</td>
                    <td class="ACA_LgButton ACA_LgButton_FontSize ACA_FLeft">
                        <ACA:AccelaButton ID="btnNext" runat="server" LabelKey="aca_asi_table_drill_down_next_action"
                            OnClick="BtnNext_Click" OnClientClick="return ENbtnServerClick()"
                            CausesValidation="false" />
                    </td>
                    <td class="ACA_LgButton ACA_LgButton_FontSize ACA_FLeft">
                        <ACA:AccelaButton ID="btnFinish" runat="server" LabelKey="aca_asi_table_drill_down_Finsh_action"
                            OnClick="BtnFinish_Click" OnClientClick="return ENbtnServerClick()"
                            CausesValidation="false" />
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <div class="ACA_LinkButton">
                            <ACA:AccelaLinkButton ID="lnkCancel" OnClientClick="return Cancel();" LabelKey="aca_asi_table_drill_down_cancel_action" CausesValidation="false" runat="server" />
                        </div>
                    </td>
                </tr>
           </table>
        </div>
        <input type="hidden" runat="server" name="hdnSelectedItems" id="hdnSelectedItems" title="" />
    </div>
</asp:content>