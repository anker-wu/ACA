<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" EnableViewStateMac="false"
    Inherits="Accela.ACA.Web.Cap.CapDetail" ValidateRequest="false" CodeBehind="CapDetail.aspx.cs" %>
<%@ Register Src="~/Component/DocumentStatusList.ascx" TagName="DocumentStatusList" TagPrefix="ACA" %>
<%@ Register Src="../Component/PermitDetailList.ascx" TagName="PermitDetailList"
    TagPrefix="uc2" %>
<%@ Register Src="~/Component/AttachmentEdit.ascx" TagName="AttachmentEdit" TagPrefix="ACA" %>
<%@ Register Src="~/Component/CAPs2MyCollection.ascx" TagName="CAPs2MyCollection"
    TagPrefix="uc4" %>
<%@ Register Src="~/Component/EducationList.ascx" TagName="EducationList" TagPrefix="uc3" %>
<%@ Register Src="~/Component/ContinuingEducationList.ascx" TagName="ContEducationList"
    TagPrefix="uc6" %>
<%@ Register Src="~/Component/ContinuingEducationSummaryList.ascx" TagName="ContEducationSummaryList"
    TagPrefix="uc7" %>
<%@ Register Src="~/Component/ExamList.ascx" TagName="ExamList" TagPrefix="uc5" %>
<%@ Register Src="~/Component/AssetListEdit.ascx" TagName="AssetList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/ValuationCalculatorView.ascx" TagName="ValuationCalculatorView"
    TagPrefix="uc8" %>
<%@ Register Src="~/Component/TrustAccountList.ascx" TagName="TrustAcctList" TagPrefix="uc9" %>
<%@ Register Src="~/Component/ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<%@ Register Src="~/Component/InspectionList.ascx" TagName="InspectionList" TagPrefix="insp" %>
<%@ Register Src="~/Component/WorkLocationView.ascx" TagName="WorkLocation" TagPrefix="uc2" %>
<%@ Register Src="~/Component/WorkStatus.ascx" TagName="WorkStatus" TagPrefix="ws" %>
<%@ Register Src="~/Component/CapConditions4CapDetail.ascx" TagName="CapConditions" TagPrefix="ACA" %>
<%@ Register Src="~/Component/SectionHeader.ascx" TagName="SectionHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="PlaceHolderMain">
    <%if (!AppSession.IsAdmin) { %>
    <!-- Facebook js reg, & position div-->
    <div id="fb-root"></div>
    <script type="text/javascript">
        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/all.js#xfbml=1&appId=<%= ConfigManager.FacebookAppId %>";
            fjs.parentNode.insertBefore(js, fjs);
        } (document, 'script', 'facebook-jssdk'));
    </script>
    <% } %>
    <div class="ACA_Area_CapDetail">
        <%
            Response.Buffer = true;
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "No-Cache");
        %>
        <script type="text/javascript" src="../Scripts/pagination.js"></script>
        <script type="text/javascript" src="../Scripts/textCollapse.js"></script>
        <script type="text/javascript" src="../Scripts/MyCollectionMethods.js"></script>
        <script type="text/javascript" src="../Scripts/RelatedRecordsTree.js"></script>
        <script type="text/javascript" src="../Scripts/ShoppingCartMethods.js"></script>
        <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/FileUpload.js")%>"></script> 
        <script type="text/javascript">
            
            //document.forms["aspnetForm"].method="post";
            //document.forms["aspnetForm"].enctype="multipart/form-data";
            var CTreeTop='<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
            var ETreeTop='<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
            var CTreeMiddle='<%=ImageUtil.GetImageURL("minus_collapse.gif") %>';
            var ETreeMiddle='<%=ImageUtil.GetImageURL("plus_expand.gif") %>';
            var imgCollapsed = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
            var imgExpanded = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
            var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>'; 
            var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';
            var norecords = '<%= AppSession.IsAdmin? string.Empty: GetTextByKey("ins_inspectionList_message_noRecord").Replace("'","\\'") %>';
            var IsFirstLoadProcessInfo = true;            
            var IsFirstLoadFeeListInfo = true;
            var IsFirstLoadNav=true;
            var IsFirstLoadRelatedCap = true;
            var IsShowAllRelatedRecord = false;

            function print_onclick(url)
            {
                hideMessage();
	            var a=window.open(url, "_blank","top=200,left=200,height=600,width=800,status=yes,toolbar=1,menubar=no,location=no,scrollbars=yes");
            }
            
           function expandAttachment()
           {
                var div=document.getElementById("divNewAttachment");
                var lnk = document.getElementById("lnkUploadNewAttachment");

                div.style.display="block";
                lnk.className = "ACA_Content_Expand";
           }                    

            var timeoutExpandAttachment;
           
            function ExpandAttachmentSection() {
                timeoutExpandAttachment = setTimeout(ExpandAttachmentSectionDelay, 1000);
            }

            function ExpandAttachmentSectionDelay() {
                var iframe = $("[id$=iframeAttachmentList]")[0];
                
                if (typeof (iframe) == "undefined" || iframe == null || iframe.contentWindow.document.body == null) {
                    return;
                }
                
                // set the attachmentlist iframe's width to its container.
                iframe.width = $('#<%= divAttachmentContainer.ClientID %>').width();
                var collapse = '<%=GetTextByKey("inspection_resultcommentlist_collapselink") %>'; 
                var readMore = '<%=GetTextByKey("inspection_resultcommentlist_readmorelink") %>';

                if (typeof(iframe.contentWindow.RebuildEllipsis)!="undefined") {
                    iframe.contentWindow.RebuildEllipsis({ readMore: readMore, collapse: collapse, containerId: iframe.id });
                }

                var height = iframe.contentWindow.document.body.scrollHeight;
                iframe.height = height;
                clearTimeout(timeoutExpandAttachment);
            }

            // expand the workflow section
            function ExpandWorkflowSection() {
                if (IsFirstLoadProcessInfo) {
                    $('#divProcessLoading').show();
                    var moduleName = "<%=ModuleName %>";
                    var agencyCode = "<%=ConfigManager.AgencyCode %>";
                    PageMethods.GetProcessingData(agencyCode, moduleName, callbackProcessingInfo);
                }
            }
            
            function callbackProcessingInfo(result) {
                IsFirstLoadProcessInfo = false;
                $('#divProcessLoading').hide();

                var divProcess=document.getElementById("divProcessingTable");
                if(result==""){
                    divProcess.innerHTML="<span class=\"ACA_CapDetail_NoRecord font12px\">"+norecords+"</span>";
                }else{
                    divProcess.innerHTML = result;
                } 
            }
            
	        function adjustMargin()
            {
                var container = document.getElementById("buttonsContainer");

               var lis= container.getElementsByTagName("Li");
 
               for(var i = 0;i<lis.length;i++)
               {
                    if(lis[i].childNodes[0].innerHTML == "")
                        lis[i].style.margin = "0px";
               }
            }

           function feesSection_report_onclick(url)
           {
                hideMessage();
	            var a=window.open(url, "_blank","top=200,left=200,height=600,width=800,status=yes,toolbar=1,menubar=no,location=no,scrollbars=yes");
	       } 

           function setObjShowOrHide(obj, isShow) {
               if (obj && typeof (obj) == "string") {
                   obj = document.getElementById(obj);
               }
               
               if (obj && obj.innerHTML) {
                   if (isShow) {
                       obj.className = obj.className.replace("ACA_Hide", "ACA_Section_Instruction ACA_Section_Instruction_FontSize");
                   }
                   else {
                       obj.className = obj.className.replace("ACA_Section_Instruction ACA_Section_Instruction_FontSize", "ACA_Hide");
                   }
               }
           }

            var hasFee = false;
            function ExpandFeeSection() {
                // show the loading message first
                $("#divFeeListPaid").show();

                if (IsFirstLoadFeeListInfo) {
                    PageMethods.DisplayFeeNoPaid("1", "<%=Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(ModuleName)%>", callbackFeeNoPaidDetailInfo);
                    PageMethods.DisplayFeePaid("1", "<%=Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(ModuleName)%>", "<%=ReportName%>", "<%=ReceiptNbr%>", "<%=ReportID%>", "<%=DisplayReceiptReport%>", callbackFeePaidDetailInfo);
                }

                if (!IsFirstLoadFeeListInfo && !hasFee) {
                    $("#divFeeList").hide();
                    $("#divFeeListPaid").hide();
                    $("#divNoRecord").show();
                }
            }

           function callbackFeeNoPaidDetailInfo(result)
           {
                IsFirstLoadFeeListInfo=false;
                var divFee=document.getElementById("divFeeList");
                var divNoRecord=document.getElementById("divNoRecord");
                divFee.innerHTML = result;
                
                if(result=="")
                { 
                     divFee.style.height="0px";
                     divFee.style.display="none";
                     hasFee = false;                     
                }
                else
                {
                    divFee.style.display="";
                    hasFee = true;
                }
                
                divNoRecord.style.display = "none";
           } 
           
           function callbackFeePaidDetailInfo(result)
           {
                IsFirstLoadFeeListInfo=false;
                var divFeePaid=document.getElementById("divFeeListPaid");
                var divFee=document.getElementById("divFeeList");
                var divNoRecord=document.getElementById("divNoRecord");
                divFeePaid.className=""; 
                divFeePaid.innerHTML = result;
                if(result=="")
                { 
                    if(hasFee)
                    {
                        divFeePaid.style.display="none";
                    }
                    else
                    {
                        divFeePaid.innerHTML="<span class=\"ACA_CapDetail_NoRecord font12px\">"+norecords+"</span>";
                    }
                }
                else
                {
                    divFeePaid.style.display="";
                    hasFee = true;
                    divNoRecord.style.display = "none";
                } 
                
                if(!hasFee)
                {                 
                    divNoRecord.innerHTML="<span class=\"ACA_CapDetail_NoRecord font12px\">"+norecords+"</span>";  
                    divNoRecord.style.display = "";
                    divFee.style.display="none";
                    divFeePaid.style.display="none";
                }
           } 
           
           function changePage(pageNum,isPaid)
           {
                if(isPaid=="false")
                {
                    PageMethods.DisplayFeeNoPaid(pageNum, "<%=Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(ModuleName)%>", callbackFeeNoPaidDetailInfo);
	            }
	            else
	            {
	                PageMethods.DisplayFeePaid(pageNum, "<%=Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(ModuleName)%>", "<%=ReportName%>", "<%=ReceiptNbr%>", "<%=ReportID%>", "<%=DisplayReceiptReport%>", callbackFeePaidDetailInfo); 
	            }
           }

           function ExpandRelatedPermitSection(isReload) {
               if (IsFirstLoadRelatedCap || isReload) {
                   var moduleName = "<%=Accela.ACA.Common.Common.ScriptFilter.AntiXssJavaScriptEncode(ModuleName) %>";

                   if (!isReload) {
                       $("#divRelatedCapLoading").show();
                       $("#divRelatePermitButton").show();
                   }

                   PageMethods.GetBuildCapTree(moduleName, encodeURIComponent(IsShowAllRelatedRecord), callbackCapInfo, onGererateCapTreeError);

                   if (isReload) {
                       if (encodeURIComponent(IsShowAllRelatedRecord)=="true") {
                           $("#" + "<%=btnSwitchTreeView.ClientID %>").html('<span>' + '<%=GetTextByKey("aca_relatedrecord_label_directlyrelatedrecord").Replace("'","\\'") %>' + '</span>');
                       }
                       else {
                           $("#" + "<%=btnSwitchTreeView.ClientID %>").html('<span>' + '<%=GetTextByKey("aca_relatedrecord_label_entiretree").Replace("'","\\'") %>' + '</span>');
                       }
                   }

                   IsShowAllRelatedRecord = !(encodeURIComponent(IsShowAllRelatedRecord)=="true");
               }
                else {
                    $("#divRelatedCapLoading").hide();
                }
            }

           //Return the HTML Sql for build Cap list tree 
           function callbackCapInfo(result)
           {
                IsFirstLoadRelatedCap = false;
                $('#divRelatedCapLoading').hide();
                
                if(result=="")
                { 
                    document.getElementById("divRelatedCapTree").innerHTML ="<span class=\"ACA_CapDetail_NoRecord font12px\">"+norecords+"</span>";
                }
                else
                {
                    document.getElementById("divRelatedCapTree").innerHTML = result;
                }

                var p = new ProcessLoading();p.hideLoading();
           }

            function onGererateCapTreeError(err) {
                $("#divRelatedCapTree").html('');
                showNormalMessage(HTMLEncode(err.get_message()), 'Error');
                $('#divRelatedCapLoading').hide();
                var p = new ProcessLoading();
                p.hideLoading();
            }

           function addOnlyOneCAP2Collection(obj) 
           {
            // This param is define in MyCollectionMethods.js, it's use for focus current link after pop up window closed.
            addCollectionClickElement = obj;
            var isRightOrLeft = '<%= IsRightToLeft %>';
            var divForAddCap2Collection  = window.document.getElementById("divForAddCap2Collection");
            
            if(divForAddCap2Collection==null)
            {
                return;
            }
            
            divForAddCap2Collection.style.display = "block";

            if(isRightOrLeft == "True")
            {
                divForAddCap2Collection.style.left = 15 + "px"; 
            }
            else
            {
                divForAddCap2Collection.style.left = ((getElementLeft(obj)-divForAddCap2Collection.offsetWidth) + obj.offsetWidth) + "px";
            } 
            
            divForAddCap2Collection.style.top = (getElementTop(obj)+obj.offsetHeight) + "px";
            divForAddCap2Collection.style.zIndex = "9";
            
            setAddedMessageForDetail(obj, isRightOrLeft);
            var rdoButton = document.getElementById('<%=addForDetailPage.RDOExistCollectionID %>');
            if (rdoButton == null) {
                rdoButton = document.getElementById('<%=addForDetailPage.RDONewCollectionID %>');
            }

            if (rdoButton != null) {
                rdoButton.focus();
            }
        }

        function ShowClonePage() {
            if ($.global.isAdmin == false)
            {
                var url = "<%=GetClonePageUrl()%>";
                var objectTargetID = "<%=lnkCloneRecord.ClientID %>";
                window.ACADialog.popup({ url: url, width: 745, height: 380, objectTarget: objectTargetID });
            }

            return false;
        }

        function hideDialogLoading() {
            var processLoading = new ProcessLoading();
            processLoading.hideLoading();
        }

        function popUpDetailDialog(pageUrl, objectTargetID) {
            var popupDialogWidth = 680;
            var popupDialogHeight = 600;
            ACADialog.popup({ url: pageUrl, width: popupDialogWidth, height: popupDialogHeight, objectTarget: objectTargetID });
        }

        function ControlDisplay(obj, imgObj, isLeefNode, lnkObj, lblValue) {
            if (obj.style.display == 'block' || obj.style.display == "" || obj.style.display == "table-row") {
                if (!isLeefNode) {
                    Collapsed(imgObj, CTreeTop, altExpanded);
                    AddTitle(lnkObj, altExpanded, lblValue);
                }
                else {
                    Collapsed(imgObj, ETreeMiddle, altExpanded);
                    AddTitle(lnkObj, altExpanded, lblValue);
                }

                obj.style.display = 'none';
            }
            else {
                if (!isLeefNode) {
                    Expanded(imgObj, ETreeTop, altCollapsed);
                    AddTitle(lnkObj, altCollapsed, lblValue);
                }
                else {
                    Expanded(imgObj, CTreeMiddle, altCollapsed);
                    AddTitle(lnkObj, altCollapsed, lblValue);
                }
                
                obj.style.display = 'table-row';
            }
        }
        
        function AddToShoppingCart(obj) 
        {
            var moduleName = "<%=ModuleName %>";
            var isRightOrLeft = '<%= IsRightToLeft %>';
            setAddedMessageForDetail(obj, isRightOrLeft);
            PageMethods.AddToShoppingCart(moduleName, CallBackAdd2ShoppingCart);
        }
            
        function CallBackAdd2ShoppingCart(result)
        {
            eval(result);
        }
        </script>
        <div id="dvContent" class="ACA_RightItem" runat="server">
            <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_permit_detail"
                LabelType="PageInstruction" runat="server" />
            <table role='presentation' width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table role='presentation' border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <h1>
                                        <ACA:AccelaLabel ID="per_permitDetail_label_permit" LabelKey="per_permitDetail_label_permit"
                                            runat="server"></ACA:AccelaLabel>&nbsp;</h1>
                                </td>
                                <td>
                                    <h1>
                                        <ACA:AccelaLabel dir="ltr" ID="lblPermitNumber" runat="server"></ACA:AccelaLabel></h1>
                                </td>
                                <td>
                                    <h1>
                                        :</h1>
                                </td>
                                <td>
                                    <a ID="lnkLink4MoreInfo" class="external_link" runat="server">
                                        <ACA:AccelaLabel ID="lblLink4MoreInfo" LabelKey="aca_recorddetail_label_linkformoreinformation" runat="server"></ACA:AccelaLabel>
                                    </a>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                    </td>
                    <td class="ACA_CapDetail_Add2CollectionLink">
                        <a href="#" onclick="AddToShoppingCart(this)" style="cursor: pointer;"
                            class="add2collectionlink NotShowLoading">
                            <ACA:AccelaLabel LabelKey="per_gridview_label_addtocart" ID="lblAddToShoppingCart" runat="server"></ACA:AccelaLabel></a>
                        <a href="#" onclick="addOnlyOneCAP2Collection(this)" style="cursor: pointer;"
                            class="add2collectionlink NotShowLoading">
                            <ACA:AccelaLabel LabelKey="mycollection_detailpage_label_addtocollection" ID="lblAddCap2Collection"
                                runat="server"></ACA:AccelaLabel></a>
                    </td>
                </tr>
            </table>
            <div id="divForAddCap2Collection" class="ACA_Add2CollectionForm">
                <uc4:CAPs2MyCollection ID="addForDetailPage" runat="server" />
            </div>
            <!-- share button div -->
            <div class="div_social_media_type">
                <h1>
                    <ACA:AccelaLabel ID="lblPermitType" runat="server"></ACA:AccelaLabel>
                </h1>
                <div id="divSocialMedia" runat="server" class="socialmedia_bar">
                    <table role='presentation' border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="socialmedia_item">
                                <div class="fb-like" data-href="<%=DataUrl %>" data-send="false" data-layout="button_count"
                                    data-show-faces="false" data-font="arial">
                                </div>
                                <script type="text/javascript">
                                    window.fbAsyncInit = function () {
                                        if (!$.global.isAdmin) {
                                            //Facebook like button event customize.
                                            FB.Event.subscribe('edge.create',
                                                function (response) {
                                                    PageMethods.Post2SocialMedia('<%=ModuleName %>', "Facebook", "SHARE", "CAP", function () { });
                                                }
                                            );
                                        }
                                    }
                                </script>
                            </td>
                            <td class="socialmedia_item">
                                <a href="//twitter.com/share" class="twitter-share-button NotShowLoading" data-url="<%=DataUrl %>"
                                    data-text="<%=SharedComments %>">Tweet</a>
                                <script type="text/javascript">
                                    //init the twett's javascript ref.
                                    window.twttr = (function (d, s, id) {
                                        var t, js, fjs = d.getElementsByTagName(s)[0];
                                        if (d.getElementById(id)) return; js = d.createElement(s); js.id = id;
                                        js.src = "//platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs);
                                        return window.twttr || (t = { _e: [], ready: function (f) { t._e.push(f) } });
                                    } (document, "script", "twitter-wjs"));
                                    twttr.ready(function (twttr) {
                                        //twitter tweet button call back.
                                        twttr.events.bind('tweet', function (event) {
                                            PageMethods.Post2SocialMedia('<%=ModuleName %>', "Twitter", "SHARE", "CAP", function () { });
                                        });
                                    });
                                </script>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <br/>
            <div id="divRecordStatus" runat="server" Visible="False">
                <span class="record_detail_status">
                    <ACA:AccelaLabel ID="lblRecordStatusTitle" LabelKey="aca_recorddetail_label_recordstatus" runat="server"></ACA:AccelaLabel>&nbsp;
                </span>
                <span class="ACA_SmLabel ACA_SmLabel_FontSize">
                    <ACA:AccelaLabel ID="lblRecordStatus" runat="server"></ACA:AccelaLabel>
                </span>
            </div>
            <div id="divRecordExpirationDate" runat="server" Visible="False">
                <span class="record_detail_expirationdate">
                    <ACA:AccelaLabel ID="lblExpirtionDateTitle" LabelKey="aca_recorddetail_label_expirationdate" runat="server"></ACA:AccelaLabel>&nbsp;
                </span>
                <span class="ACA_SmLabel ACA_SmLabel_FontSize">
                    <ACA:AccelaLabel ID="lblExpirtionDate" runat="server"></ACA:AccelaLabel>
                </span>
            </div> 
            <!--documentlist section start-->
            <div id="divDocumentlist" runat="server">
                <ACA:DocumentStatusList ID="documentStatusList" runat="server" />
            </div>
            <!--documentlist section end-->
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="10" />
            <ACA:CapConditions ID="capConditions" HideLink4ViewMet="false" IsGroupByRecordType="false"
                GeneralConditionsTitleLabelKey="aca_recorddetail_label_generalcondition_sectionheader"
                ConditionsOfApprovalTitleLabelKey="aca_recorddetail_label_conditionofapproval_sectionheader"
                GeneralConditionsPatternLabelKey="aca_recorddetail_generalcondition_pattern"
                PendingConditionsOfApprovalPatternLabelKey="aca_recorddetail_pendingconditionofapproval_pattern"
                MetConditionsOfApprovalPatternLabelKey="aca_recorddetail_metconditionofapproval_pattern"
                runat="server" />
            <!-- Begin Work Location section-->
            <div id="divWorkLocation" runat="server">
                <uc1:SectionHeader runat="server" ID="shWorkLocation" TitleLabelKey="per_permitDetail_label_workLocation" EnableExpand="true"
                    Collapsible="true" SectionBodyClientID="divWorkLocationInfo">
                </uc1:SectionHeader>
                <div id="divWorkLocationInfo" class="ACA_Hide">
                    <div id="divWorkLocationDetail">
                        <uc2:WorkLocation ID="workLocation" runat="server" />
                    </div>
                    <div id="divWorkLocationMapDetail">
                        <ACA:ACAMap ID="mapCapDetail" AGISContext="CAPDetail" IsMiniMode="true" OnShowOnMap="CapDetailMap_ShowACAMap"
                            runat="server" />
                    </div>
                    <ACA:AccelaHeightSeparate ID="sepLineForWorkLocation" runat="server" Height="10" />
                </div>
            </div>
            <!--End Work Location section-->
            <!--Asset List Begin-->
            <div id="divAsset" runat="server" visible="true">
                <uc1:SectionHeader runat="server" ID="shAsset" TitleLabelKey="aca_capdetail_label_assettitle" EnableExpand="true"
                    Collapsible="true" SectionBodyClientID="divAssetList">
                </uc1:SectionHeader>
                <div id="divAssetList" class="ACA_Hide">
                    <asp:UpdatePanel ID="updatePanelAssetList" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <ACA:AssetList ID="assetList" runat="server" Location="CapDetail" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Asset List End-->
            <!--Begin Permit Detail Section-->
            <div id="divPermitDetail" runat="server" visible="false">
                <uc1:SectionHeader runat="server" ID="shPermitDetail" TitleLabelKey="per_permitDetail_label_detail" EnableExpand="true"
                    Collapsible="true" SectionBodyClientID="divPermitDetailInfo" Collapsed="false">
                </uc1:SectionHeader>
                <div id="divPermitDetailInfo" class="ACA_Hide">
                <uc2:PermitDetailList ID="PermitDetailList1" runat="server"></uc2:PermitDetailList>
                <div class="ACA_TabRow ACA_LiLeft action_buttons" id="buttonsContainer">
                    <ACA:AccelaHeightSeparate ID="sepForPermitDetail" runat="server" Height="5" />
                    <ul>
                        <li>
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="lnkPrintPermit" CssClass="NotShowLoading" LabelKey="per_permitDetail_label_printPermit"
                                    runat="server" Visible="false" ReportID="0"></ACA:AccelaButton>
                            </div>
                        </li>
                        <li>
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="lnkPrintReceipt" CssClass="NotShowLoading" LabelKey="per_permitDetail_label_printReceipt"
                                    runat="server" Visible="false" ReportID="0"> </ACA:AccelaButton>
                            </div>
                        </li>
                        <li>
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="lnkPrintSummary" CssClass="NotShowLoading" LabelKey="per_permitDetail_label_printSummary"
                                    runat="server" Visible="false" ReportID="0"></ACA:AccelaButton>
                            </div>
                        </li>
                        <li>
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="lnkCloneRecord" CssClass="NotShowLoading" LabelKey="aca_capdetail_label_clone_record"
                                    OnClientClick="return ShowClonePage();" runat="server" Visible="false"></ACA:AccelaButton>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="ACA_TabRow ACA_LiLeft action_buttons">
                    <ul>
                        <li id="liAmendment" runat="server">
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="lnkCreateAmendment" EnableRecordTypeFilter="true"
                                    LabelKey="per_permitDetail_label_createAmendment" runat="server" Visible="true"
                                    CausesValidation="false" OnClick="CreateAmendmentButton_Click"></ACA:AccelaButton>
                            </div>
                        </li>
                        <li id="liRenewal" runat="server">
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="btnRenewal" ToolTip="" LabelKey="" runat="server" CausesValidation="false"
                                    OnClick="BtnRenewalAndRequestTradeLicenseClick"></ACA:AccelaButton>
                            </div>
                        </li>
                        <li id="liRequestTradeLicense" runat="server">
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="btnRequestLicense" ToolTip="" LabelKey="per_tradeName_msg_requestTradeLicense"
                                    runat="server" CausesValidation="false" OnClick="BtnRenewalAndRequestTradeLicenseClick"></ACA:AccelaButton>
                            </div>
                        </li>
                    </ul>
                </div>
                <div id="divRenewalAdminBtnList" runat="server" class="ACA_TabRow ACA_LiLeft action_buttons">
                    <ul>
                        <li>
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="btnRenewalApplication" LabelKey="per_permitList_label_renewalApplication" 
                                    runat="server" CausesValidation="false"></ACA:AccelaButton>
                            </div>
                        </li>
                        <li>
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="btnRenewalPayFeedue" LabelKey="per_permitList_label_renewal_payfeedue" 
                                    runat="server" CausesValidation="false"></ACA:AccelaButton>
                            </div>
                        </li>
                        <li>
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="btnRenewalTradeName" LabelKey="per_permitList_label_renewaltradename" 
                                    runat="server" CausesValidation="false"></ACA:AccelaButton>
                            </div>
                        </li>
                        <li>
                            <div class="ACA_SmButton ACA_SmButton_FontSize">
                                <ACA:AccelaButton ID="btnRenewalTradeLicense" LabelKey="per_permitList_label_renewaltradelicense" 
                                    runat="server" CausesValidation="false"></ACA:AccelaButton>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            </div>
            <!--   End Permit Detail Section-->
            <!--   Fee Detail Section-->
            <div id="divFee" runat="server">
                <uc1:SectionHeader runat="server" ID="shFee" TitleLabelKey="per_feeDetails_label_feeTitel" EnableExpand="true"
                    Collapsible="true" OnClientExpanded="ExpandFeeSection();" SectionBodyClientID="divFeeListContent">
                </uc1:SectionHeader>
                <div id="divFeeListContent" class="ACA_Hide">
                    <div id="divFeeList" style="display: none">
                    </div>
                    <div id="divFeeListPaid" style="display: none" class="ACA_SmLabel ACA_SmLabel_FontSize">
                        <%= GetTextByKey("capdetail_message_loading")%>
                    </div>
                    <div id="divNoRecord" style="display: none" class="ACA_SmLabel ACA_SmLabel_FontSize">
                    </div>
                </div>
            </div>
            <!--   end Fee Detail Section-->
            <!--   Begin Inspection Detail Section-->
            <div id="divInspection" runat="server" visible="false">
                <uc1:SectionHeader runat="server" ID="shInspection" TitleLabelKey="ins_inspectionList_label_inspection" EnableExpand="true"
                    Collapsible="true" SectionBodyClientID="inspectionTable" Collapsed="false">
                </uc1:SectionHeader>
                <div id="inspectionTable" class="ACA_Hide">
                    <insp:InspectionList ID="InspectionList" runat="server" />
                </div>
            </div>
            <!-- End Inspection Detail Section-->
            <!--workflow section start-->
            <div id="divWorkflow" runat="server">
                <div id="divProcessStatus" runat="server">
                    <uc1:SectionHeader runat="server" ID="shProcessStatus" TitleLabelKey="per_workStatus_label_proceeStatus" EnableExpand="true"
                        Collapsible="true" OnClientExpanded="ExpandWorkflowSection();" SectionBodyClientID="divProcessInfo">
                    </uc1:SectionHeader>
                    <div id="divProcessInfo" class="ACA_Hide">
                        <div id="divProcessLoading" style="display: none" class="ACA_SmLabel ACA_SmLabel_FontSize">
                            <%= GetTextByKey("capdetail_message_loading") %></div>
                        <div id="divProcessingTable" class="fontbold">
                        </div>
                        <ws:WorkStatus ID="processInstruction" runat="server" />
                    </div>
                </div>
            </div>
            <!--workflow section end-->
            <!--   Attachment Begin -->
            <div id="divAttachmentContainer" runat="server">
                <uc1:SectionHeader runat="server" ID="shAttachment" TitleLabelKey="per_attachment_Label_attachTitle" EnableExpand="true"
                    Collapsible="true" OnClientExpanded="ExpandAttachmentSection();" SectionBodyClientID="divAttachment">
                </uc1:SectionHeader>
                <div id="divAttachment" class="ACA_Hide">
                    <!--Attachment List Section end-->
                    <div class="ACA_TabRow">
                        <ACA:AttachmentEdit ID="attachmentEdit" IsDetailPage="true" runat="server" />
                    </div>
                </div>
            </div>
            <!--   Attachment End -->
            <!--Related Permits Begin-->
            <div runat="server" id="divRelatedPermits">
                <uc1:SectionHeader runat="server" ID="shRelatedPermit" TitleLabelKey="per_permitDetail_label_relatedPermit" EnableExpand="true"
                    Collapsible="true" OnClientExpanded="ExpandRelatedPermitSection(false)" SectionBodyClientID="divRelatedPermit">
                </uc1:SectionHeader>
                <div id="divRelatedPermit" class="ACA_Hide ACA_TabRow ACA_LiLeft">
                    <div id="divRelatePermitButton" class="ACA_Row">
                        <ul>
                            <li>
                                <div class="ACA_SmButton ACA_SmButton_FontSize">
                                    <ACA:AccelaButton ID="btnSwitchTreeView" OnClientClick="ExpandRelatedPermitSection(true);var p = new ProcessLoading();p.showLoading(true);return false;"
                                        LabelKey="aca_relatedrecord_label_entiretree" runat="server"></ACA:AccelaButton>
                                </div>
                            </li>
                            <li id="switchTreeViewBtnContainerForAdmin" runat="server">
                                <div class="ACA_SmButton ACA_SmButton_FontSize">
                                    <ACA:AccelaButton ID="btnSwitchTreeViewForAdmin" LabelKey="aca_relatedrecord_label_directlyrelatedrecord"
                                        runat="server"></ACA:AccelaButton>
                                </div>
                            </li>
                        </ul>
                        <ACA:AccelaHeightSeparate ID="heightSeparater" Height="5" runat="server" />
                    </div>
                    <div id="divRelatedCapLoading" style="display: none" class="ACA_SmLabel ACA_SmLabel_FontSize">
                        <%= GetTextByKey("capdetail_message_loading") %></div>
                    <div id="divRelatedCapTree">
                    </div>
                </div>
            </div>
            <!--Related Permits End-->
            <!--Education Begin-->
            <div id="divEducation" runat="server" visible="false">
                <uc1:SectionHeader runat="server" ID="shEducation" TitleLabelKey="per_detail_education_section_name" EnableExpand="true"
                    Collapsible="true" SectionBodyClientID="divEducationList">
                </uc1:SectionHeader>
                <div id="divEducationList" class="ACA_Hide">
                    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc3:EducationList ID="educationList" GViewID="60124" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Education End-->
            <!--Continuing Education Begin-->
            <div id="divContEducation" runat="server" visible="false">
                <uc1:SectionHeader runat="server" ID="shContEducation" TitleLabelKey="continuing_education_capdetail_section_name" EnableExpand="true"
                    Collapsible="true" SectionBodyClientID="divContEducationList">
                </uc1:SectionHeader>
                <div id="divContEducationList" class="ACA_Hide">
                    <uc7:ContEducationSummaryList ID="contEducationSummaryList" runat="server" />
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="5" />
                    <asp:UpdatePanel ID="updatePanelContEducation" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc6:ContEducationList ID="contEducationList" GViewID="60125" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Continuing Education End-->
            <!--Examination Begin-->
            <div id="divExamination" runat="server" visible="false">
                <uc1:SectionHeader runat="server" ID="shExamination" TitleLabelKey="examination_title" EnableExpand="true"
                    Collapsible="true" SectionBodyClientID="divExaminationList">
                </uc1:SectionHeader>
                <div id="divExaminationList" class="ACA_Hide">
                    <asp:UpdatePanel ID="updatePanelExamination" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc5:ExamList OnRefreshCapContact="ExaminationList_RefreshCapContact" ID="ExaminationList"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Examination End-->
            <!--Valuation Calculator Begin-->
            <div id="divValuationCalculator" runat="server" visible="true">
                <uc1:SectionHeader runat="server" ID="shValuationCalculator" TitleLabelKey="valuationcalculator_title" EnableExpand="true"
                    Collapsible="true" SectionBodyClientID="divValuationCalculatorList">
                </uc1:SectionHeader>
                <div id="divValuationCalculatorList" class="ACA_Hide">
                    <asp:UpdatePanel ID="updateValuationCalculator" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc8:ValuationCalculatorView ID="ValuationCalculatorView" runat="server" DisplayType="CapDetail" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Valuation Calculator End-->
            <!--Trust Account List Begin-->
            <div id="divTrustAcct" runat="server" visible="true">
                <uc1:SectionHeader runat="server" ID="shTrustAcct" TitleLabelKey="per_permitdetail_trustaccount_title" EnableExpand="true"
                    Collapsible="true" SectionBodyClientID="divTrustAcctList">
                </uc1:SectionHeader>
                <div id="divTrustAcctList" class="ACA_Hide">
                    <asp:UpdatePanel ID="updatePanelTrustAcct" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc9:TrustAcctList ID="trustAcctList" runat="server" GViewID="60105" Location="CapDetail" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Trust Account List End-->
            <div id="divAdded" class="ACA_Loading_Message ACA_SmLabel ACA_SmLabel_FontSize">
            </div>
        </div>
        <script type="text/javascript">
            if (typeof (ExportCSV) != 'undefined') {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(EndRequest);
            }

            function EndRequest(sender, args) {
                //export file.
                ExportCSV(sender, args);
            }
        </script>
        <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
    </div>
</asp:Content>
