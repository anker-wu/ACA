<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.CertifiedBusinessSearchForm" Codebehind="CertifiedBusinessSearchForm.ascx.cs" %>
<ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
    <ACA:AccelaDropDownList ID="ddlNIGPType" SourceType="Database" onchange="removeAllCommodityClass();" LabelKey="aca_certbusiness_label_nigptype" runat="server"></ACA:AccelaDropDownList>
    <ACA:AccelaTextBox ID="txtNIGPKeyword" LabelKey="aca_certbusiness_label_nigpkeyword" MaxLength="3000" runat="server"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtDescription" LabelKey="aca_certbusiness_label_nigpdescription" MaxLength="2000" runat="server"></ACA:AccelaTextBox>
	<ACA:AccelaListBox ID="listboxCommodityClass" LabelKey="aca_certbusiness_label_commodityclass" SelectionMode="Multiple" runat="server"></ACA:AccelaListBox>
    <ACA:AccelaDropDownList ID="ddlLargestContractExperience" SourceType="Database" LabelKey="aca_certbusiness_label_largestcontractexperience" runat="server" onchange="EnableContractValue()"></ACA:AccelaDropDownList>
    <ACA:AccelaDropDownList ID="ddlLargestContractValue" SourceType="Database" LabelKey="aca_certbusiness_label_largestcontractvalue" runat="server" onchange="ResetContractExperience()" Enabled="false"></ACA:AccelaDropDownList>
    <ACA:AccelaCheckBoxList ID="cbListCertifiedAs" LabelKey="aca_certbusiness_label_certifiedas" RepeatDirection="Horizontal" runat="server"/>
	<ACA:AccelaCheckBoxList ID="cbListOwnerEthnicity" LabelKey="aca_certbusiness_label_ownerethnicity" RepeatDirection="Horizontal" runat="server"/>
	<ACA:AccelaListBox ID="listboxLocation" LabelKey="aca_certbusiness_label_location" SelectionMode="Multiple" runat="server"></ACA:AccelaListBox>
    <ACA:AccelaTextBox ID="txtZipCode" onchange="CopyZipCodeToHiddenField(this);" LabelKey="aca_certbusiness_label_zipcode" MaxLength="1000" runat="server"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtCompanyName" LabelKey="aca_certbusiness_label_companyname" MaxLength="255" runat="server"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtBusinessNamedba" LabelKey="aca_certbusiness_label_businessnamedba" MaxLength="255" runat="server"></ACA:AccelaTextBox>
    <ACA:AccelaCalendarText ID="txtCertificationDateFrom" runat="server" LabelKey="aca_certbusiness_label_certifieddatefrom"></ACA:AccelaCalendarText>
    <ACA:AccelaCalendarText ID="txtCertificationDateTo" runat="server" LabelKey="aca_certbusiness_label_certifieddateto"></ACA:AccelaCalendarText>
</ACA:AccelaFormDesignerPlaceHolder>
<asp:HiddenField ID="hdCommodityClassCode" runat="server" />
<asp:HiddenField ID="hdLocation" runat="server" />
<asp:HiddenField ID="hdZipCode" runat="server" />

<ACA:AccelaInlineScript runat="server" ID="scriptRegister">
<script type="text/javascript">

    // change the ethnicity's status
    $(document).ready(function () {
        var $cbListCertifiedAs = $('#<%= cbListCertifiedAs.ClientID %>');
        
        SetOwnerEthnicityEnableAttribute($cbListCertifiedAs);

        $cbListCertifiedAs.bind("click", function () { SetOwnerEthnicityEnableAttribute($cbListCertifiedAs); });
    });

    // Set the OwnerEthnicity's enable attribute
    function SetOwnerEthnicityEnableAttribute($cbListCertifiedAsId) {
        var isOwnerEthnicityEnable = false;
        var str = '<%=CertifiedItems2EnableEthnicity %>';

        if (str != "") {
            var certifiedValueArray = str.split('<%=Accela.ACA.Common.ACAConstant.SPLIT_CHAR4URL1 %>');

            $cbListCertifiedAsId.find(":checked").each(function () {
                var certifiedValue = $(this).next('label').text();

                for (i = 0; i < certifiedValueArray.length; i++) {
                    if (certifiedValue == certifiedValueArray[i]) {
                        isOwnerEthnicityEnable = true;
                        break;
                    }
                }
            });
        }

        if ($cbListCertifiedAsId.find(":checked").length == 0) {
            isOwnerEthnicityEnable = true;
        }

        var $cbListOwnerEthnicity = $('#<%= cbListOwnerEthnicity.ClientID %>').find(":checkbox");
        if (isOwnerEthnicityEnable) {
            $cbListOwnerEthnicity.attr("disabled", false);
        }
        else {
            $cbListOwnerEthnicity.attr("checked", false);
            $cbListOwnerEthnicity.attr("disabled", true);
        }
    }

    // popup and add the commodity class
    function addCommodityClass(targetID) {
        var nigpType = $('#<%= ddlNIGPType.ClientID %>').val();

        ACADialog.popup({ url: 'GeneralDrillDown.aspx?nigpType=' + nigpType, width: 800, height: 350, objectTarget: targetID });
    }

    // the commodity class popup callback
    function addCommodityClassCallBack(values) {
        var listboxCommodityClass = $('#<%= listboxCommodityClass.ClientID %>');
        listboxCommodityClass.empty();

        // set the commodity class value
        var resultValue = '|';

        $.each(values, function (i, n) {
            if (n != '') {
                listboxCommodityClass.append('<option value="' + n + '" title="' + n + '">' + n + '</option>');

                resultValue = resultValue + n + '|';
            }
        });

        if (resultValue.length >= 1) {
            var hdCommodityClassCode = $('#<%= hdCommodityClassCode.ClientID %>');
            hdCommodityClassCode.val(resultValue);
        }
    }

    function removeCommodityClass() {
        var hdCommodityClassCode = $('#<%= hdCommodityClassCode.ClientID %>');    
        var selected = $("#<%= listboxCommodityClass.ClientID %> option:selected");

        $.each(selected, function (i, n) {
            var result = hdCommodityClassCode.val();

            result = result.replace('|' + n.value + '|', '|');
            hdCommodityClassCode.val(result);
        });

        selected.remove();
    }

    function removeAllCommodityClass() {
        var listboxCommodityClass = $('#<%= listboxCommodityClass.ClientID %>');
        listboxCommodityClass.empty();
    }

    // get the commodity class that list in the listbox. Used by popup layer this form showed.
    // digit: show the digit need to return. if null, return the all value.
    function getCommodityClass(digit) {
        var result = '|';

        $('#<%= listboxCommodityClass.ClientID %> option').each(function (i, n) {
            if (digit == null || digit == '' || n.value.length >= digit) {
                var item = n.value;

                if (digit != null && digit != '') {
                    item = n.value.substr(0, digit);
                }

                if (result.indexOf('|' + item + '|') == -1) {
                    result = result + item + '|';
                }
            }
        });

        if (result.length == 1) {
            result = '';
        }
        return result;
    }

    // add and popup location page
    function addLocation(targetID) {
        ACADialog.popup({ url: 'GeneralPopupSelect.aspx?select=location', width: 400, height: 340, objectTarget: targetID });       
    }   

    function addLocationCallBack(values) {
        var listboxLocation = $('#<%= listboxLocation.ClientID %>');
        listboxLocation.empty();       

        var resultValue = ','; 

        // set the location value
        $.each(values, function (i, n) {
            if (n != '') {
                listboxLocation.append('<option value="' + n + '" title="'+ n + '">' + n + '</option>');

                resultValue = resultValue + n + ',';
            }
        });

        if (resultValue.length >= 1) {
            var hdLocation = $('#<%= hdLocation.ClientID %>');
            hdLocation.val(resultValue);
        }
    }

    function removeLocation() {
        var hdLocation = $('#<%= hdLocation.ClientID %>');
        var selected = $("#<%= listboxLocation.ClientID %> option:selected");        

        $.each(selected, function (i, n) {
            var result = hdLocation.val();
            result = result.replace(',' + n.value + ',', ',');
            hdLocation.val(result);
        });

        selected.remove();
    }

    function getLocation() {
        var result = ',';

        $('#<%= listboxLocation.ClientID %> option').each(function (i, n) {
            result = result + n.value + ',';
        });

        if (result.length == 1) {
            result = '';
        }
        return result;
    }

    function addZipCode(targetID) {
        ACADialog.popup({ url: 'GeneralPopupSelect.aspx?select=zipcode', width: 400, height: 340, objectTarget: targetID }); 
    }

    function addZipCodeCallBack(values) {
        var result = '';

        for (var i = 0; i < values.length; i++) {
            var value = $.trim(values[i])

            if (value != '') {
                result = result + value + ',';
            }
        }

        if (result.length > 0) {
            result = result.substr(0, result.length - 1);
        }

        SetValueById('<%= txtZipCode.ClientID %>', result);
        $('#<%= hdZipCode.ClientID %>').val(result);
    }

    function CopyZipCodeToHiddenField(obj) {
        var value = $(obj).val();
        value = $.trim(value).replace(' ', '');

        $('#<%= hdZipCode.ClientID %>').val(value);
    }

    function getZipCode() {
        var result = GetValueById('<%= txtZipCode.ClientID %>');

        if (result != '') {
            result = ',' + result + ',';
        }

        return result;
    }

    function EnableContractValue() {
        var selectedComparison = $('#<% = ddlLargestContractExperience.ClientID %>').children('option:selected').val();
        var contractValue = $('#<%= ddlLargestContractValue.ClientID %>');

        if (selectedComparison == '') {
            var firstOption = contractValue.children().get(0);

            if (firstOption != null) {
                firstOption.selected = true;
            }

            contractValue.attr('disabled', 'disabled');
        }
        else {
            contractValue.removeAttr('disabled');
        }
    }

    function ResetContractExperience() {
        var contractValue = $('#<% = ddlLargestContractValue.ClientID %>');
        var selectedValue = contractValue.children('option:selected').val();

        if (selectedValue == '') {
            var ComparisonFirstOption = $('#<%= ddlLargestContractExperience.ClientID %>').children().get(0);
            ComparisonFirstOption.selected = true;
            contractValue.attr('disabled', 'disabled');
        }
    }
</script>
</ACA:AccelaInlineScript>