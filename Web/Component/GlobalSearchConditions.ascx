<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalSearchConditions.ascx.cs"
    Inherits="Accela.ACA.Web.Component.GlobalSearchConditions" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>
<div id="gsContainer" class="gs_container">
    <table role='presentation' class="gs_top_table" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td id="tdSpace">
                &nbsp;
            </td>
            <td class="gs_border" style="width:16.5em">
                <p>
                    <input class="gs_search_box" type="text" autocomplete="off" id="txtSearchCondition" name="txtSearchCondition" title='<%= LabelUtil.RemoveHtmlFormat(GetTextByKey("per_globalsearch_label_search")) %>' />
                    <ACA:AccelaLabel ID="lblSearchCondition" Width="163" Height="18" CssClass="gs_search_box font11px"
                        Visible="false" LabelKey="per_globalsearch_label_search" runat="server" LabelType="SimpleLabelText"></ACA:AccelaLabel>
                </p>
            </td>
            <td style="width:2px">
            <span>&nbsp;</span>
            </td>
            <td class="gs_border " style="width:18px;text-align:center;vertical-align:middle;">
                <a id="btnSearch" class="gs_go NotShowLoading" title="<%= GetTextByKey("img_alt_globalsearch_searchbutton") %>" href="#">
                    <img class="gs_go_img" alt="<%= GetTextByKey("img_alt_globalsearch_searchbutton") %>" src="<%=ImageUtil.GetImageURL("gsearch_disabled.png") %>"/>
                </a>
            </td>
            <td class="gs_border_right" style='width: 12px;height: 16px; text-align:center;vertical-align:middle;' id="tdSearchHistory">
                <a id="btnSearchHistory" class="gs_history NotShowLoading" title="<%= GetTextByKey("img_alt_globalsearch_searchhistory") %>" href="#">
                    <img class="gs_history_img" alt="<%= GetTextByKey("img_alt_globalsearch_searchhistory") %>" src="<%=ImageUtil.GetImageURL("Caret_down_sml.gif") %>" onclick="return false;"/>
                </a>
            </td>
        </tr>
    </table>
</div>
<div id="divHistoryList" class="gs_border gs_history_list" style="display:none">
</div>
<script type="text/javascript">
    var searchText = '<%= GetWaterMark() %>';
    var minLimitMessage = '<%= GetTextByKey("per_globalsearch_message_searchhint").Replace("'","\\'")%>';
    var historyNoResult = '<%= GetTextByKey("per_globalsearch_no_recent_results").Replace("'","\\'")%>';
    var searchResult = '<%= GetTextByKey("per_globalsearch_recent_results").Replace("'","\\'")%>';
    var historyHint = '<%= GetTextByKey("per_globalsearch_message_history").Replace("'","\\'")%>';
    var loading = '<%= GetTextByKey("capdetail_message_loading").Replace("'","\\'")%>';
    rootURL = '<%= FileUtil.ApplicationRoot %>';
    searchImg = '<%=ImageUtil.GetImageURL("gsearch.png") %>';
    searchDisabledImg = '<%=ImageUtil.GetImageURL("gsearch_disabled.png") %>';
    
    // preload image
    var img = new Image();
    img.src = searchImg;
    
    var globalHistory = null;
    var globalSearch = null;
    if ($.global.isAdmin) {
        var txtSearchCondition = $get("txtSearchCondition");
        txtSearchCondition.style.display = "none";
        document.getElementById("btnSearch").disabled = true;
    }else{
        globalSearch = new GlobalSearch("txtSearchCondition","btnSearch",searchText,minLimitMessage);
        $("#btnSearchHistory").popUp({oTarget:'divHistoryList', onShowEvent:'mouseover', onHideEvent:'mouseout', onFocusEvent:'focus', align:'right', offsetTop:6});
        globalHistory = new GlobalSearchHistory("divHistoryList",historyNoResult,searchResult);
    }
    
    var timerForSearchHistory = null;
    
    function SetFocusInForSearchHistory()
    {        
        clearTimeout(timerForSearchHistory);
        timerForSearchHistory = null;
    }
    
    function SetBlurOutForSearchHistory()
    {
        clearTimeout(timerForSearchHistory);
        timerForSearchHistory = null;
        timerForSearchHistory = setTimeout(hideHistoryList, 1000);
    }
    
    function hideHistoryList()
    {
        $("#divHistoryList").hide();
    }

</script>

