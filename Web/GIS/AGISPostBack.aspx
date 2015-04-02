<%@ Page Language="C#"  EnableViewStateMac="false" EnableViewState="false" AutoEventWireup="true" CodeBehind="AGISPostBack.aspx.cs" Inherits="Accela.ACA.Web.GIS.AGISPostBack" %>
<%@ import namespace="Accela.ACA.Web" %>
<%@ Import namespace="Accela.ACA.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" >
<head runat="server">
   <title><%=LabelUtil.GetGlobalTextByKey("aca_gispostback_label_title|tip")%></title>
   <link rel="stylesheet" type="text/css" title="default" href="../App_Themes/default/form.css" />
    <script type="text/javascript">
      
        function ShowParcel(){
            var frm = document.getElementById('<%=form1.ClientID %>');
            var ParcelNum=document.getElementById("ParcelNum");
            var ParcelSeq=document.getElementById("ParcelSeq");
            var AddressID=document.getElementById("AddressID");
            var AddressSeq=document.getElementById("AddressSeq");
            var ParcelUID=document.getElementById("ParcelUID");
            var url=frm.action+"?ParcelNum="+encodeURIComponent(ParcelNum.value)+"&ParcelSeq="+encodeURIComponent(ParcelSeq.value)+"&AddressID="+encodeURIComponent(AddressID.value)+"&AddressSeq="+encodeURIComponent(AddressSeq.value)+"&ParcelUID="+encodeURIComponent(ParcelUID.value);
            var win = window.parent.parent;
            win.location.href = url;
        }
        
        function ShowRecordList(){
            var frm = document.getElementById('<%=form1.ClientID %>');
            var gisId = document.getElementById("GisId");
            var layerId =document.getElementById("LayerId");
            var serviceId =document.getElementById("ServiceId");
            var agency =document.getElementById("AgencyCode");
            var moduleName = document.getElementById("ModuleName");
            var url=frm.action+"?GisId="+encodeURIComponent(gisId.value)+"&LayerId="+encodeURIComponent(layerId.value)+"&ServiceId="+encodeURIComponent(serviceId.value)+"&HideHeader=True&<%=UrlConstant.AgencyCode %>="+encodeURIComponent(agency.value);
            url = url+"&ModuleName="+encodeURIComponent(moduleName.value);
            var win = window.parent.parent;
            win.ACADialog.popup({url:url,width:800,height:550,objectTarget:win.id});
        }
        
        function ShowAddress(){
            var frm = document.getElementById('<%=form1.ClientID %>');
            var AddressID=document.getElementById("AddressID");
            var AddressSeq=document.getElementById("AddressSeq");
            var ParcelNum=document.getElementById("ParcelNum");
            var AddressUID=document.getElementById("AddressUID");
            var agency =document.getElementById("AgencyCode");
            var url=frm.action+"?AddressID="+encodeURIComponent(AddressID.value)+"&AddressSeq="+encodeURIComponent(AddressSeq.value)+"&ParcelNum="+encodeURIComponent(ParcelNum.value)+"&AddressUID="+encodeURIComponent(AddressUID.value)+"&<%=UrlConstant.AgencyCode %>="+encodeURIComponent(agency.value);
            var win = window.parent.parent;
            win.location.href=url;
        }

        function removeViewState(){
            var vs = document.getElementById("__VIEWSTATE");
            if (vs != null) {
                vs.parentNode.removeChild(vs);
            }
        }

        function CreateRecord() {
            removeViewState();
            var win = window.parent.parent;
            var url = document.getElementById("Url");
            win.location.href = url.value;
        }

        function SendAddress() {
            removeViewState();
            var model = document.getElementById("xml");
            var win = window.parent.parent;
            win.FillGISAddress(model.value);
        }
        
        function SendAsset() {
            removeViewState();
            var model = document.getElementById("xml");
            var win = window.parent.parent;
            win.GetAsset(model.value);
        }

        function SendFeature() {
            removeViewState();
            var model = document.getElementById("xml");
            var win = window.parent.parent;
            win.FillParcelFromGIS(model.value);
        }

        function ScheduleInspection() {
            removeViewState();
            var win = window.parent.parent;
            var url = document.getElementById("Url");
            win.location.href = url.value;
        }

        function ShowErrorMessage() {
            var no_record_message = '<%=LabelUtil.GetGlobalTextByKey("aca_gis_no_record_message").Replace("'","\\'") %>';
            alert(no_record_message);
        }

        function ShowParcelLockMessage(){
            var record_lock_message='<%=LabelUtil.GetGlobalTextByKey("aca_gis_message_parcellocked").Replace("'","\\'") %>';
            alert(record_lock_message);
        }

        /// show parcel locked with create new record.
        function ShowParcelLockMessageWithCreateRecord(){
            var record_lock_message='<%=LabelUtil.GetGlobalTextByKey("aca_gis_message_parcellockedwithcreaterecord").Replace("'","\\'") %>';
            alert(record_lock_message);
        }

        function ShowRecordLockMessage(){
            var record_lock_message='<%=LabelUtil.GetGlobalTextByKey("aca_gis_message_recordlocked").Replace("'","\\'") %>';
            alert(record_lock_message);
        }

        function ShowAddressLockMessage(){
            var record_lock_message='<%=LabelUtil.GetGlobalTextByKey("aca_gis_message_addresslocked").Replace("'","\\'") %>';
            alert(record_lock_message);
        }

        function ShowAddressLockMessageWithCreateRecord(){
            var record_lock_message='<%=LabelUtil.GetGlobalTextByKey("aca_gis_message_addresslockedwithcreaterecord").Replace("'","\\'") %>';
            alert(record_lock_message);
        }

        function ShowDisableParcelMessage(){
            var disableparcelmessage = '<%=LabelUtil.GetGlobalTextByKey("aca_gis_msg_disable_parcel").Replace("'","\\'") %>';
            alert(disableparcelmessage);
        }

        function ShowNoRecordLinkGISObjectMessage(){
            var norecord_link_gis_object= '<%=LabelUtil.GetGlobalTextByKey("aca_gis_msg_norecord_link_gis_object").Replace("'","\\'") %>';
            alert(norecord_link_gis_object);
        }
        
        function ResumeApplication(){
            removeViewState();
            var win = window.parent.parent;
            var url = document.getElementById("Url");
            win.location.href = url.value;
        }
        
        function ShowGeoDocuments(){
            var frm = document.getElementById('<%=form1.ClientID %>');
            var url=frm.action;
            var win = window.parent.parent;
            win.ACADialog.popup({url:url,width:800,height:320,objectTarget:win.id}); 
        }
        
    </script>
</head>
<body id="body" runat="server">
    <form id="form1" runat="server">
        <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
</body>
</html>
