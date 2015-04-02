<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceControl.ascx.cs" Inherits="Accela.ACA.Web.Component.ServiceControl" %>
<%@ Import Namespace="Accela.ACA.Common.Common"  %>

<div class="servicelist">
    <asp:UpdatePanel ID="panService" runat="server">
        <ContentTemplate>
            <div id="divSearchBar" runat="server" visible="false">            
                <table role="presentation" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <div class="ACA_FLeft">
                                <ACA:AccelaTextBox runat="server" ID="txtSearch" IsDisplayLabel="false" LayoutType="Horizontal" ToolTipLabelKey="aca_record_label_service_filter"
                                    LabelKey="aca_servicecontrol_label_search" InstructionAlign="Right" Width="13.5em" MaxLength="100"/>
                            </div>
                            <div id="divFilter" runat="server" class="search_filter_button"><input type="image" id="imgShowServiceGroup" src="<%=ImageUtil.GetImageURL("icon_filter.png") %>" alt="<%= LabelUtil.GetTextByKey("aca_servicecontrol_label_availableservicegroup", ModuleName)%>" /></div>
                        </td>
                        <td>
                            <ACA:AccelaButton ID="btnSearch" runat="server" LabelKey="aca_servicecontrol_label_buttonsearch"
                                CausesValidation="false" OnClientClick="RsetSelectedServicesGroup();" OnClick="SearchButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divServicePan" visible="false" runat="server">
                <div class="ACA_SmLabel ACA_SmLabel_FontSize">
                    <div id="divForAdminShowService" runat="server" visible="false">
                        <ACA:AccelaLabel ID="lblSelectServiceAdmin" runat="server" LabelKey="superAgency_workLocation_label_selectService"></ACA:AccelaLabel>
                        <ACA:AccelaLabel ID="lblServiceAppendPrefix" runat="server" LabelKey="superAgency_workLocation_label_serviceFound"></ACA:AccelaLabel>
                    </div>
                    <ACA:AccelaLabel ID="lblSelectService" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                </div>
                <div id="divServiceList" runat="server">
                    <div class="ACA_List ACA_List_OverFlow">
                        <asp:Repeater runat="server" ID="rptAgency" OnItemDataBound="AgencyRepeater_ItemDataBound">
                            <ItemTemplate>
                                <div class="ServiceListContainer" class="ACA_Link_Text ACA_HyperLink">
                                    <div class="ServiceListGroupName">
                                        <a href="javascript:void(0)" onclick="ToggleServiceList(this); return false;" class="NotShowLoading">
                                            <img class="ACA_NoBorder" src="<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>" alt="<%=GetTextByKey("img_alt_expand_icon") %>" />
                                            <ACA:AccelaLabel ID="lblGroupCode" CssClass="ACA_Title_Color" runat="server"/>
                                        </a>
                                    </div>
                                    <div style="display: none;" class="ServiceItemList">
                                        <asp:CheckBoxList ID="cbListServices" runat="server" RepeatLayout="Flow" CssClass="aca_checkbox aca_checkbox_fontsize" />
                                        <asp:RadioButtonList id="rbListServices" runat="server"  RepeatLayout="Flow" CssClass="aca_checkbox aca_checkbox_fontsize" />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="ACA_TabRow">&nbsp;</div>
                <span id="spanLicExpiredNotice" runat="Server"></span>
            </div>
            <div class="ACA_Message_Notice ACA_Message_Notice_FontSize" id="divNoResult" EnableViewState="false" runat="server" visible="false">
                <div id="divForAdminShowResults" runat="server" visible="false">
                    <ACA:AccelaLabel ID="lblNoResultInAdmin" runat="server" LabelKey="superAgency_workLocation_label_noAddressResults"></ACA:AccelaLabel><br />
                    <ACA:AccelaLabel ID="lblWebSiteInAdmin" runat="server" LabelKey="superAgency_workLocation_label_visitOfficialSite"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaLabel ID="lblNoResultMsg" IsNeedEncode="false" runat="server" Style=" display:block;"></ACA:AccelaLabel>
                <a id="webSite" runat="server" visible="false" class="ACA_Message_Notice_Link ACA_Message_Notice_Link_FontSize" target="_blank">
                    <%=ScriptFilter.FilterScript(GetOfficialSiteUrl())%>
                </a>
            </div>

            <input type="hidden" runat="server" id="hasService" />
            <input type="hidden" runat="server" id="hdnSelectedServices" />
            <input type="hidden" runat="server" id="hdnSelectedServicesGroup" />
            <ACA:AccelaInlineScript runat="server">
            <script type="text/javascript">
                $(document).ready(function () {
                    var $imgShowServiceGroup = $('#imgShowServiceGroup');
                    $('#<%=divFilter.ClientID %>').attr('title', '<%= LabelUtil.GetTextByKey("aca_servicecontrol_label_availableservicegroup", ModuleName).Replace("'", "\\'")%>');

                    $imgShowServiceGroup.click(function (e) {
                        if ($.global.isAdmin) {
                            return false;
                        }

                        // display the service group checkbox list in correct position.
                        var obj = e.target;
                        $('#divServiceGroup').show();

                        var pointX = getElementsLeftWithControlWidth(obj);
                        var pointY = getElementTop(obj) + obj.offsetHeight;

                        if ($.global.isRTL) {
                            $('#divServiceGroup').css('right', pointX).css('top', pointY);
                        }
                        else {
                            $('#divServiceGroup').css('left', pointX).css('top', pointY);
                        }

                        FocusObject(e, 'lnkBeginFocus');

                        // reset the selected serivce groups if user Cancel the selected item.
                        RsetSelectedServicesGroup();
                    });

                    $imgShowServiceGroup.bind('mouseover mouseout', function () {
                        if ($.global.isAdmin) {
                            return false;
                        }

                        var filterIcon = '<%=ImageUtil.GetImageURL("icon_filter.png") %>';
                        var filterIconHover = '<%=ImageUtil.GetImageURL("icon_filter_hover.png") %>';

                        if ($(this).attr('src') == filterIconHover) {
                            $(this).attr('src', filterIcon);
                        } else {
                            $(this).attr('src', filterIconHover);
                        }
                    });

                    $('#<%=txtSearch.ClientID%>').keydown(function (e) {
                        if (e.keyCode == 13) {
                            invokeClick($('#<%=btnSearch.ClientID %>')[0]);
                            return false;
                        }
                    });
                });
            </script>
            </ACA:AccelaInlineScript>
        </ContentTemplate>
    </asp:UpdatePanel>
    <ACA:AccelaHeightSeparate ID="sepForButton" runat="server" Height="5" />
    <div id="divContinueButton">
        <ACA:AccelaButton ID="btnContinue" runat="server" LabelKey="superAgency_workLocation_label_buttonContinue"
            CausesValidation="false" OnClick="ContinueButton_Click" DivEnableCss="ACA_LgButton ACA_LgButtonForRight ACA_LgButtonForRight_FontSize"></ACA:AccelaButton>
    </div>
    
    <div id="divServiceGroup" class="searchbox_service_group">
        <a id='lnkBeginFocus' href='javascript:void(0);' class='ACA_FLeft NotShowLoading' title='<%=LabelUtil.GetGUITextByKey("img_alt_form_begin")%>'>
            <img alt="" src="<%=ImageUtil.GetImageURL("spacer.gif")%>" class="ACA_NoBorder" />
        </a>
        <div class="service_group_header">
            <div class="service_group_title">
                <%= LabelUtil.GetTextByKey("aca_servicecontrol_label_availableservicegroup", ModuleName)%>
            </div>
        </div>
        <div class="service_group_body">
            <asp:CheckBoxList ID="cbListServiceGroup" Width="265" runat="server" RepeatLayout="Table" RepeatColumns="2" CssClass="aca_checkbox aca_checkbox_fontsize" />
        </div>
        <div class="service_group_foot">
            <div class="ACA_FLeft">
                <ACA:AccelaButton ID="btnOK" runat="server" OnClientClick="SetCurrentSelectedGroup();" OnClick="ServiceGroupLink_Selected" LabelKey="aca_servicecontrol_label_availableservicegroup_btnok" CausesValidation="false" DivEnableCss="ACA_LgButton ACA_LgButtonForRight ACA_LgButtonForRight_FontSize"/>
            </div>
            <div class="ACA_FLeft">
                <ACA:AccelaButton ID="btnCancel" runat="server" OnClientClick="HideServiceGroup(); return false;" onkeydown="ClosePopup(event);" LabelKey="aca_servicecontrol_label_availableservicegroup_btncancel" CausesValidation="false" DivEnableCss="ACA_LgButton ACA_LgButtonForRight ACA_LgButtonForRight_FontSize"/>
            </div>
        </div>
    </div>
</div>

<ACA:AccelaInlineScript runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        if ($.global.isAdmin == false) {
            CheckSelection('<%=btnContinue.ClientID %>', '<%=hdnSelectedServices.ClientID %>');
        }
        
        // Click the outside area to close the popup.
        $(document).click(function (e) {
            if ($(e.target).attr('id') == 'imgShowServiceGroup') {
                return false;
            }

            if ($('#divServiceGroup').is(":visible")) {
                var clickInPanel = $(e.target).attr('id') == 'divServiceGroup' || $(e.target).parents('#divServiceGroup').length > 0;
                
                if (!clickInPanel) {
                    HideServiceGroup();
                }
            }
        });
    });
    
    function HideServiceGroup() {
        $('#divServiceGroup').hide();
    }
    
    function ClosePopup(e) {
        if ((e.shiftKey == false && e.keyCode == 9) || e.keyCode == 13) {
            HideServiceGroup();
            FocusObject(e, 'imgShowServiceGroup');
        }
    }

    function SetCurrentSelectedGroup() {
        var selectedGroups = '';
        
        $('#<%=cbListServiceGroup.ClientID %> :checkbox').each(function() {
            if ($(this).attr('checked')) {
                var serviceGroup = $(this).next().text();
                selectedGroups += serviceGroup + '\f';
            }
        });
        
        $('#<%=hdnSelectedServicesGroup.ClientID %>').val(selectedGroups);
    }
    
    function RsetSelectedServicesGroup() {
        if ($.global.isAdmin) {
            return false;
        }
                        
        var selectedGroups = $('#<%=hdnSelectedServicesGroup.ClientID %>').val();

        if (selectedGroups.length == 0) {
            $('#<%=cbListServiceGroup.ClientID %> :checkbox').each(function () {
                $(this).attr('checked', false);
            });
        } else {
            selectedGroups = '\f' + selectedGroups;

            $('#<%=cbListServiceGroup.ClientID %> :checkbox').each(function () {
                var serviceGroup = '\f' + $(this).next().text() + '\f';
                var checked = selectedGroups.indexOf(serviceGroup) > -1;

                $(this).attr('checked', checked);
            });
        }
    }

    function initServiceListStatus(displayParam) {
        var links = $("#<%=divServiceList.ClientID%>").find("a.ACA_HyperLink");
     
        if(links.length > 0) {
            var displayOfServiceitemlist = IsTrue(displayParam);
            
            for (var index = 0; index < links.length; index++) {
                ToggleServiceList(links[index],displayOfServiceitemlist);
            }
        }
    }
    
    /*
     * restore = true: force expand the service list
     * restore = false: force collapse the service list
     * resotre = null/undefined, toggle the service list according the current expand/collapse status.
     */
    function ToggleServiceList(link,restore) {
        var $divServiceItems = $(link).closest("div.ServiceListContainer").find("div.ServiceItemList");
        var imgDoc = $(link).find("img")[0];
        var visible = $divServiceItems.is(':visible');

        if(restore!= null && restore!= undefined) {
            visible = !restore;
        }
        
        if (visible) {
            $divServiceItems.hide();
            var imgCollapsed = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
            var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';
            Collapsed(imgDoc, imgCollapsed, altExpanded);
            AddTitle(link, altExpanded, null);
        }
        else {
            $divServiceItems.show();
            var imgExpanded = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
            var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>';
            Expanded(imgDoc, imgExpanded, altCollapsed);
            AddTitle(link, altCollapsed, null);
        }
    }
</script>
</ACA:AccelaInlineScript>