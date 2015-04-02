/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: cs 203363 2011-09-08 09:58:37Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,       &lt;Who&gt;,        &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.BLL.AddressBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Text.RegularExpressions;

    using Accela.ACA.BLL.Common;
    using Accela.ACA.Common;
    using Accela.ACA.Common.Common;
    using Accela.ACA.Common.Util;
    using Accela.ACA.WSProxy;

    /// <summary>
    /// Address Builder Helper
    /// </summary>
    public class AddressBuilderBll : BaseBll, IAddressBuilderBll
    {
        /// <summary>
        /// long address with format.
        /// </summary>
        private const string DEFAULT_LONG_ADDRESS_WITH_FORMAT = "<span class='fontbold'>$$HouseNumberStart$$$$StartFraction$$$$Street#(end)$$$$EndFraction$$$$Direction$$$$Prefix$$$$StreetName$$$$StreetType$$$$StreetSuffix$$</span><br/>$$UnitType$$$$UnitStart$$$$Unit#(end)$$<br/>$$SecondaryRoad$$$$SecondaryRoadNumber$$<br/>$$NeighborhoodPrefix$$$$Neighborhood$$<br/>$$Description$$$$Distance$$<br/>$$InspectionDistrictPrefix$$$$InspectionDistrict$$<br/>$$City$$$$County$$$$State$$$$Zip$$<br/>$$Country$$<br/>$$StreetAddress$$<br/>$$AddressLine1$$<br/>$$AddressLine2$$<br/>$$LevelPrefix$$$$LevelNumberStart$$$$LevelNumberEnd$$<br/>$$HouseNumberAlphaStart$$$$HouseNumberAlphaEnd$$";

        /// <summary>
        /// long address no format.
        /// </summary>
        private const string DEFAULT_LONG_ADDRESS_NO_FORMAT = "$$HouseNumberStart$$$$StartFraction$$$$Street#(end)$$$$EndFraction$$$$Direction$$$$Prefix$$$$StreetName$$$$StreetType$$$$StreetSuffix$$,$$UnitType$$$$UnitStart$$$$Unit#(end)$$,$$SecondaryRoad$$$$SecondaryRoadNumber$$,$$NeighborhoodPrefix$$$$Neighborhood$$,$$Description$$$$Distance$$,$$InspectionDistrictPrefix$$$$InspectionDistrict$$,$$City$$$$County$$$$State$$$$Zip$$,$$StreetAddress$$,$$AddressLine1$$$$AddressLine2$$,$$LevelPrefix$$$$LevelNumberStart$$$$LevelNumberEnd$$,$$HouseNumberAlphaStart$$$$HouseNumberAlphaEnd$$";

        /// <summary>
        /// short address no format.
        /// </summary>
        private const string DEFAULT_SHORT_ADDRESS_NO_FORMAT = "$$HouseNumberStart$$$$Direction$$$$StreetName$$$$StreetType$$$$StreetSuffix$$,$$UnitType$$$$UnitStart$$,$$City$$$$State$$$$Zip$$$$Country$$,$$LevelPrefix$$$$LevelNumberStart$$$$LevelNumberEnd$$,$$HouseNumberAlphaStart$$$$HouseNumberAlphaEnd$$";

        /// <summary>
        /// short address with format.
        /// </summary>
        private const string DEFAULT_SHORT_ADDRESS_WITH_FORMAT = "$$HouseNumberStart$$$$Direction$$$$StreetName$$$$StreetType$$$$StreetSuffix$$,$$UnitType$$$$UnitStart$$,$$City$$$$State$$$$Zip$$$$Country$$,$$AddressType$$,$$LevelPrefix$$$$LevelNumberStart$$$$LevelNumberEnd$$,$$HouseNumberAlphaStart$$$$HouseNumberAlphaEnd$$";

        /// <summary>
        /// Gets the address item separator.
        /// </summary>
        /// <value>The address item separator.</value>
        private string AddressItemSeparator
        {
            get
            {
                return " ";
            }
        }

        /// <summary>
        /// Gets the address group separator.
        /// </summary>
        /// <value>The address group separator.</value>
        private string AddressGroupSeparator
        {
            get
            {
                return ",";
            }
        }

        /// <summary>
        /// Get the formatted value for "Work Location"
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <param name="models">The models.</param>
        /// <param name="additionalLocationTitle">The additional location title.</param>
        /// <returns>an assembled address string</returns>
        public string Build4WorkLocation(AddressModel[] addresses, SimpleViewElementModel4WS[] models, string additionalLocationTitle)
        {
            string addressPattern = GetAddressPattern(AddressFormatType.LONG_ADDRESS_WITH_FORMAT);
           
            StringBuilder buf = new StringBuilder();

            if (addresses == null || addresses.Length == 0)
            {
                return string.Empty;
            }

            buf.Append("<table role='presentation' id='tbl_worklocation' style='TEMPLATE_STYLE' class='table_child'>");
            int index = -1;

            for (int i = 0; i < addresses.Length; i++)
            {
                string addressStr = string.Empty;
                bool showIndex = true;
                string tableRowStyle = string.Empty;
                string strIndex = string.Empty;
                StringBuilder contant = new StringBuilder();
                AddressModel address = addresses[i];

                if (address.addressId == null)
                {
                    continue;
                }
                else
                {
                    index++;
                }

                if (index == 1)
                {
                    buf.Append("<tr><td class='td_child_left font12px'></td><td>&nbsp;</td>");
                    buf.Append("<tr><td class='td_child_left font12px'></td><td><strong>");
                    buf.Append("<a id='link_additional_locations' href='javascript:void(0)' onclick=\"DisplayAdditionInfo('tr_additional_locations',this,'tbl_worklocation');\" class=\"NotShowLoading\">");
                    buf.Append(additionalLocationTitle);
                    buf.Append("</a>");
                    buf.Append("</strong></td></tr>");
                }

                if (index > 0)
                {
                    tableRowStyle = " tips='tr_additional_locations' style='display:none;'";
                }

                buf.Append("<tr " + tableRowStyle + "><td class='td_child_left font12px'>");

                addressStr = ReplaceDefinedPattern(addressPattern, address, models, false);

                if (addressStr != string.Empty)
                {
                    if (index > 0 && showIndex == true)
                    {
                        strIndex = string.Format("{0}{1}", index.ToString(), ACAConstant.BRACKET);
                        showIndex = false;
                    }

                    contant.Append(addressStr + ACAConstant.HTML_BR);
                }

                buf.Append(strIndex);
                buf.Append("</td><td class='NotBreakWord'>");
                buf.Append(contant);
                buf.Append("</td></tr>");
            }

            buf.Append("</table>");

            return buf.ToString();
        }

        /// <summary>
        /// Build address from AddressModel
        /// </summary>
        /// <param name="addressModel">The address model.</param>
        /// <returns>an assembled address string</returns>
        public string Build4Map(AddressModel addressModel)
        {
            string result = string.Empty;

            if (addressModel != null)
            {
                string addressPattern = GetAddressPattern(AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
                result = ReplaceDefinedPattern(addressPattern, addressModel, null, true);
            }

            return result;
        }

        /// <summary>
        /// Build address for license display
        /// </summary>
        /// <param name="licenseModel">the license model</param>
        /// <param name="country">the country</param>
        /// <returns>Return the license address string</returns>
        public string Build4License(LicenseModel4WS licenseModel, string country)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (licenseModel != null)
            {
                AppendItemSeparator(stringBuilder, licenseModel.address1, ACAConstant.HTML_BR);
                AppendItemSeparator(stringBuilder, licenseModel.address2, ACAConstant.HTML_BR);
                AppendItemSeparator(stringBuilder, licenseModel.address3, ACAConstant.HTML_BR);

                string state = GetStateI18n(licenseModel.state, licenseModel.countryCode);

                AppendItemSeparator(stringBuilder, licenseModel.city, ACAConstant.HTML_BR);
                AppendItemSeparator(stringBuilder, state, ACAConstant.HTML_NBSP);
                AppendItemSeparator(stringBuilder, this.FormatZip(licenseModel.zip, licenseModel.countryCode), ACAConstant.HTML_NBSP);
                AppendItemSeparator(stringBuilder, country, ACAConstant.HTML_BR);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the state i18n.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns>State I18N value.</returns>
        public string GetStateI18n(string state, string countryCode)
        {
            string resState = string.Empty;

            if (!string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(countryCode))
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();

                //if country code is empty, send none to don't get default country
                Dictionary<string, string> statesI18n;
                cacheManager.GetRegionalModelByCountry(countryCode, out statesI18n);

                if (statesI18n != null && statesI18n.Count > 0)
                {
                    statesI18n.TryGetValue(state, out resState);
                }
            }

            return string.IsNullOrEmpty(resState) ? state : resState;
        }

        /// <summary>
        /// Build no format short address.
        /// </summary>
        /// <typeparam name="T">The generic type of the address data.</typeparam>
        /// <param name="addressModel">AddressModel model</param>
        /// <param name="models">SimpleViewElementModel4WS model list</param>
        /// <param name="addressFormatType">address Format Type</param>
        /// <returns>an assembled address string</returns>
        string IAddressBuilderBll.BuildAddressByFormatType<T>(T addressModel, SimpleViewElementModel4WS[] models, AddressFormatType addressFormatType)
        {
            string result = string.Empty;

            if (addressModel != null)
            {
                string addressPattern = GetAddressPattern(addressFormatType);
                result = ReplaceDefinedPattern(addressPattern, addressModel, models, false);
            }

            return result;
        }

        /// <summary>
        /// Generate Address string from a specified provider detail model.
        /// </summary>
        /// <param name="providerDetailModel">a provider detail model</param>
        /// <returns>an assembled address string</returns>
        string IAddressBuilderBll.BuildAddress4Provider(ProviderDetailModel providerDetailModel)
        {
            string result = string.Empty;

            if (providerDetailModel != null)
            {
                StringBuilder addressBuilder = new StringBuilder();

                AppendItemSeparator(addressBuilder, providerDetailModel.address1);
                AppendItemSeparator(addressBuilder, providerDetailModel.address2);
                AppendItemSeparator(addressBuilder, providerDetailModel.address3);
                AppendItemSeparator(addressBuilder, providerDetailModel.city);

                //if model add the country need using country field.
                AppendItemSeparator(addressBuilder, this.GetStateI18n(providerDetailModel.state, string.Empty));
                AppendItemSeparator(addressBuilder, this.FormatZip(providerDetailModel.zip, providerDetailModel.countryCode));
                AppendItemSeparator(addressBuilder, this.GetCountryByKey(providerDetailModel.countryCode, false));

                result = addressBuilder.ToString();
                result = TrimSeparator(result, true, this.AddressGroupSeparator);
            }

            return result;
        }

        /// <summary>
        /// Generate Address string from a specified provider location model.
        /// </summary>
        /// <param name="lc">a provider location model</param>
        /// <returns>an assembled address string</returns>
        string IAddressBuilderBll.BuildAddress4ProviderLocation(RProviderLocationModel lc)
        {
            string result = string.Empty;

            if (lc != null)
            {
                StringBuilder addressBuilder = new StringBuilder();

                AppendItemSeparator(addressBuilder, lc.address1);
                AppendItemSeparator(addressBuilder, lc.address2);
                AppendItemSeparator(addressBuilder, lc.address3);
                AppendItemSeparator(addressBuilder, lc.city);
                AppendItemSeparator(addressBuilder, this.GetStateI18n(lc.state, string.Empty));
                AppendItemSeparator(addressBuilder, this.FormatZip(lc.zip, lc.countryCode));
                AppendItemSeparator(addressBuilder, this.GetCountryByKey(lc.countryCode, false));

                result = addressBuilder.ToString();
                result = TrimSeparator(result, true, this.AddressGroupSeparator);
            }

            return result;
        }

        /// <summary>
        /// Replace pattern with real value.
        /// </summary>
        /// <typeparam name="T">The generic type of the address data.</typeparam>
        /// <param name="addressPattern">address pattern</param>
        /// <param name="addressT">address/ref address model/data row</param>
        /// <param name="models">simple view element model.</param>
        /// <param name="isForVEMap">is for VE map.</param>
        /// <returns>address format.</returns>
        private string ReplaceDefinedPattern<T>(string addressPattern, T addressT, SimpleViewElementModel4WS[] models, bool isForVEMap)
        {
            StringBuilder addressLineBuilder = new StringBuilder(addressPattern);

            if (addressT is AddressModel)
            {
                AddressModel address = addressT as AddressModel;

                addressLineBuilder.Replace(AddressFormatVariables.HouseNumberRangeStart, AppendSpace(Convert.ToString(address.houseNumberRangeStart)))
                      .Replace(AddressFormatVariables.HouseNumberStart, IsFieldVisible(models, "txtStreetNo") ? AppendSpace(Convert.ToString(address.houseNumberStart)) : string.Empty)
                      .Replace(AddressFormatVariables.HouseNumberRangeEnd, AppendSpace(Convert.ToString(address.houseNumberRangeEnd)))
                      .Replace(AddressFormatVariables.HouseNumberEnd, IsFieldVisible(models, "txtStreetEnd") ? AppendSpace(Convert.ToString(address.houseNumberEnd)) : string.Empty)
                      .Replace(AddressFormatVariables.Direction, IsFieldVisible(models, "ddlStreetDirection") ? AppendSpace(I18nStringUtil.GetString(address.resStreetDirection, address.streetDirection)) : string.Empty)
                      .Replace(AddressFormatVariables.StreetName, IsFieldVisible(models, "txtStreetName") ? AppendSpace(address.streetName) : string.Empty)
                      .Replace(AddressFormatVariables.StreetType, IsFieldVisible(models, "ddlStreetSuffix") ? AppendSpace(I18nStringUtil.GetString(address.resStreetSuffix, address.streetSuffix)) : string.Empty)
                      .Replace(AddressFormatVariables.UnitType, IsFieldVisible(models, "ddlUnitType") ? AppendSpace(I18nStringUtil.GetString(address.resUnitType, address.unitType)) : string.Empty)
                      .Replace(AddressFormatVariables.UnitRangeStart, AppendSpace(address.unitRangeStart))
                      .Replace(AddressFormatVariables.UnitStart, IsFieldVisible(models, "txtUnitNo") ? AppendSpace(ConvertToString(address.unitStart)) : string.Empty)
                      .Replace(AddressFormatVariables.UnitRangeEnd, AppendSpace(address.unitRangeEnd))
                      .Replace(AddressFormatVariables.City, IsFieldVisible(models, "txtCity") ? AppendSpace(ScriptFilter.FilterScript(address.city, false)) : string.Empty)
                      .Replace(AddressFormatVariables.State, IsFieldVisible(models, "txtState") ? AppendSpace(GetStateI18n(address.state, address.countryCode)) : string.Empty)
                      .Replace(AddressFormatVariables.Zip, IsFieldVisible(models, "txtZip") ? AppendSpace(ConvertToString(FormatZip(address.zip, address.countryCode))) : string.Empty)
                      .Replace(AddressFormatVariables.InspectionDistrict, IsFieldVisible(models, "txtInspectionD") ? AppendSpace(ConvertToString(address.inspectionDistrict)) : string.Empty)
                      .Replace(AddressFormatVariables.CountryCode, IsFieldVisible(models, "ddlCountry") ? ScriptFilter.FilterScript(AppendSpace(GetCountryByKey(address.countryCode, isForVEMap)), false) : string.Empty)
                      .Replace(AddressFormatVariables.StartFraction, IsFieldVisible(models, "txtStartFraction") ? AppendSpace(address.houseFractionStart) : string.Empty)
                      .Replace(AddressFormatVariables.HouseFractionEnd, IsFieldVisible(models, "txtEndFraction") ? AppendSpace(address.houseFractionEnd) : string.Empty)
                      .Replace(AddressFormatVariables.Prefix, IsFieldVisible(models, "txtPrefix") ? AppendSpace(address.streetPrefix) : string.Empty)
                      .Replace(AddressFormatVariables.StreetSuffix, IsFieldVisible(models, "ddlStreetSuffixDirection") ? AppendSpace(I18nStringUtil.GetString(address.resStreetSuffixdirection, address.streetSuffixdirection)) : string.Empty)
                      .Replace(AddressFormatVariables.UnitEnd, IsFieldVisible(models, "txtUnitEnd") ? AppendSpace(ConvertToString(address.unitEnd)) : string.Empty)
                      .Replace(AddressFormatVariables.SecondaryRoad, IsFieldVisible(models, "txtSecondaryRoad") ? AppendSpace(ConvertToString(address.secondaryRoad)) : string.Empty)
                      .Replace(AddressFormatVariables.SecondaryRoadNumber, IsFieldVisible(models, "txtSecondaryRoadNo") ? AppendSpace(ConvertToString(address.secondaryRoadNumber)) : string.Empty)
                      .Replace(AddressFormatVariables.NeighborhoodPrefix, IsFieldVisible(models, "txtNeighborhoodP") ? AppendSpace(ConvertToString(address.neighborhoodPrefix)) : string.Empty)
                      .Replace(AddressFormatVariables.Neighborhood, IsFieldVisible(models, "txtNeighborhood") ? AppendSpace(ConvertToString(address.neighborhood)) : string.Empty)
                      .Replace(AddressFormatVariables.Description, IsFieldVisible(models, "txtDescription") ? AppendSpace(ConvertToString(address.addressDescription)) : string.Empty)
                      .Replace(AddressFormatVariables.Distance, IsFieldVisible(models, "txtDistance") ? AppendSpace(ConvertToString(address.distance)) : string.Empty)
                      .Replace(AddressFormatVariables.XCoordinator, AppendSpace(Convert.ToString(address.XCoordinator)))
                      .Replace(AddressFormatVariables.YCoordinator, AppendSpace(Convert.ToString(address.YCoordinator)))
                      .Replace(AddressFormatVariables.InspectionDistrictPrefix, IsFieldVisible(models, "txtInspectionDP") ? AppendSpace(ConvertToString(address.inspectionDistrictPrefix)) : string.Empty)
                      .Replace(AddressFormatVariables.County, IsFieldVisible(models, "txtCounty") ? AppendSpace(ConvertToString(address.county)) : string.Empty)
                      .Replace(AddressFormatVariables.StreetAddress, IsFieldVisible(models, "txtStreetAddress") ? AppendSpace(ScriptFilter.FilterScript(ConvertToString(address.fullAddress), false)) : string.Empty)
                      .Replace(AddressFormatVariables.AddressLine1, IsFieldVisible(models, "txtAddressLine1") ? AppendSpace(ScriptFilter.FilterScript(address.addressLine1, false)) : string.Empty)
                      .Replace(AddressFormatVariables.AddressLine2, IsFieldVisible(models, "txtAddressLine2") ? AppendSpace(ScriptFilter.FilterScript(address.addressLine2, false)) : string.Empty)
                      .Replace(AddressFormatVariables.AddressType, AppendSpace(ScriptFilter.FilterScript(address.addressType, false)))
                      .Replace(AddressFormatVariables.LevelPrefix, IsFieldVisible(models, "txtLevelPrefix") ? AppendSpace(ConvertToString(address.levelPrefix)) : string.Empty)
                      .Replace(AddressFormatVariables.LevelNumberStart, IsFieldVisible(models, "txtLevelNbrStart") ? AppendSpace(ConvertToString(address.levelNumberStart)) : string.Empty)
                      .Replace(AddressFormatVariables.LevelNumberEnd, IsFieldVisible(models, "txtLevelNbrEnd") ? AppendSpace(ConvertToString(address.levelNumberEnd)) : string.Empty)
                      .Replace(AddressFormatVariables.HouseAlphaStart, IsFieldVisible(models, "txtHouseAlphaStart") ? AppendSpace(ConvertToString(address.houseNumberAlphaStart)) : string.Empty)
                      .Replace(AddressFormatVariables.HouseAlphaEnd, IsFieldVisible(models, "txtHouseAlphaEnd") ? AppendSpace(ConvertToString(address.houseNumberAlphaEnd)) : string.Empty);
            }
            else if (addressT is RefAddressModel)
            {
                RefAddressModel address = addressT as RefAddressModel;

                addressLineBuilder.Replace(AddressFormatVariables.HouseNumberRangeStart, AppendSpace(Convert.ToString(address.houseNumberRangeStart)))
                      .Replace(AddressFormatVariables.HouseNumberStart, IsFieldVisible(models, "txtStreetNo") ? AppendSpace(Convert.ToString(address.houseNumberStart)) : string.Empty)
                      .Replace(AddressFormatVariables.HouseNumberRangeEnd, AppendSpace(Convert.ToString(address.houseNumberRangeEnd)))
                      .Replace(AddressFormatVariables.HouseNumberEnd, IsFieldVisible(models, "txtStreetEnd") ? AppendSpace(Convert.ToString(address.houseNumberEnd)) : string.Empty)
                      .Replace(AddressFormatVariables.Direction, IsFieldVisible(models, "ddlStreetDirection") ? AppendSpace(I18nStringUtil.GetString(address.resStreetDirection, address.streetDirection)) : string.Empty)
                      .Replace(AddressFormatVariables.StreetName, IsFieldVisible(models, "txtStreetName") ? AppendSpace(address.streetName) : string.Empty)
                      .Replace(AddressFormatVariables.StreetType, IsFieldVisible(models, "ddlStreetSuffix") ? AppendSpace(I18nStringUtil.GetString(address.resStreetSuffix, address.streetSuffix)) : string.Empty)
                      .Replace(AddressFormatVariables.UnitType, IsFieldVisible(models, "ddlUnitType") ? AppendSpace(I18nStringUtil.GetString(address.resUnitType, address.unitType)) : string.Empty)
                      .Replace(AddressFormatVariables.UnitRangeStart, AppendSpace(address.unitRangeStart))
                      .Replace(AddressFormatVariables.UnitStart, IsFieldVisible(models, "txtUnitNo") ? AppendSpace(ConvertToString(address.unitStart)) : string.Empty)
                      .Replace(AddressFormatVariables.UnitRangeEnd, AppendSpace(address.unitRangeEnd))
                      .Replace(AddressFormatVariables.City, IsFieldVisible(models, "txtCity") ? AppendSpace(ScriptFilter.FilterScript(address.city, false)) : string.Empty)
                      .Replace(AddressFormatVariables.State, IsFieldVisible(models, "txtState") ? AppendSpace(GetStateI18n(address.state, address.countryCode)) : string.Empty)
                      .Replace(AddressFormatVariables.Zip, IsFieldVisible(models, "txtZip") ? AppendSpace(ConvertToString(FormatZip(address.zip, address.countryCode))) : string.Empty)
                      .Replace(AddressFormatVariables.InspectionDistrict, IsFieldVisible(models, "txtInspectionD") ? AppendSpace(ConvertToString(address.inspectionDistrict)) : string.Empty)
                      .Replace(AddressFormatVariables.CountryCode, IsFieldVisible(models, "ddlCountry") ? ScriptFilter.FilterScript(AppendSpace(GetCountryByKey(address.countryCode, isForVEMap)), false) : string.Empty)
                      .Replace(AddressFormatVariables.StartFraction, IsFieldVisible(models, "txtStartFraction") ? AppendSpace(address.houseFractionStart) : string.Empty)
                      .Replace(AddressFormatVariables.HouseFractionEnd, IsFieldVisible(models, "txtEndFraction") ? AppendSpace(address.houseFractionEnd) : string.Empty)
                      .Replace(AddressFormatVariables.Prefix, IsFieldVisible(models, "txtPrefix") ? AppendSpace(address.streetPrefix) : string.Empty)
                      .Replace(AddressFormatVariables.StreetSuffix, IsFieldVisible(models, "ddlStreetSuffixDirection") ? AppendSpace(I18nStringUtil.GetString(address.resStreetSuffixdirection, address.streetSuffixdirection)) : string.Empty)
                      .Replace(AddressFormatVariables.UnitEnd, IsFieldVisible(models, "txtUnitEnd") ? AppendSpace(ConvertToString(address.unitEnd)) : string.Empty)
                      .Replace(AddressFormatVariables.SecondaryRoad, IsFieldVisible(models, "txtSecondaryRoad") ? AppendSpace(ConvertToString(address.secondaryRoad)) : string.Empty)
                      .Replace(AddressFormatVariables.SecondaryRoadNumber, IsFieldVisible(models, "txtSecondaryRoadNo") ? AppendSpace(ConvertToString(address.secondaryRoadNumber)) : string.Empty)
                      .Replace(AddressFormatVariables.NeighborhoodPrefix, IsFieldVisible(models, "txtNeighborhoodP") ? AppendSpace(ConvertToString(address.neighborhoodPrefix)) : string.Empty)
                      .Replace(AddressFormatVariables.Neighborhood, IsFieldVisible(models, "txtNeighborhood") ? AppendSpace(ConvertToString(address.neighborhood)) : string.Empty)
                      .Replace(AddressFormatVariables.Description, IsFieldVisible(models, "txtDescription") ? AppendSpace(ConvertToString(address.addressDescription)) : string.Empty)
                      .Replace(AddressFormatVariables.Distance, IsFieldVisible(models, "txtDistance") ? AppendSpace(ConvertToString(address.distance)) : string.Empty)
                      .Replace(AddressFormatVariables.XCoordinator, AppendSpace(Convert.ToString(address.XCoordinator)))
                      .Replace(AddressFormatVariables.YCoordinator, AppendSpace(Convert.ToString(address.YCoordinator)))
                      .Replace(AddressFormatVariables.InspectionDistrictPrefix, IsFieldVisible(models, "txtInspectionDP") ? AppendSpace(ConvertToString(address.inspectionDistrictPrefix)) : string.Empty)
                      .Replace(AddressFormatVariables.County, IsFieldVisible(models, "txtCounty") ? AppendSpace(ConvertToString(address.county)) : string.Empty)
                      .Replace(AddressFormatVariables.StreetAddress, IsFieldVisible(models, "txtStreetAddress") ? AppendSpace(ScriptFilter.FilterScript(ConvertToString(address.fullAddress), false)) : string.Empty)
                      .Replace(AddressFormatVariables.AddressLine1, IsFieldVisible(models, "txtAddressLine1") ? AppendSpace(ScriptFilter.FilterScript(address.addressLine1, false)) : string.Empty)
                      .Replace(AddressFormatVariables.AddressLine2, IsFieldVisible(models, "txtAddressLine2") ? AppendSpace(ScriptFilter.FilterScript(address.addressLine2, false)) : string.Empty)
                      .Replace(AddressFormatVariables.AddressType, AppendSpace(ScriptFilter.FilterScript(address.addressType, false)))
                      .Replace(AddressFormatVariables.LevelPrefix, IsFieldVisible(models, "txtLevelPrefix") ? AppendSpace(ConvertToString(address.levelPrefix)) : string.Empty)
                      .Replace(AddressFormatVariables.LevelNumberStart, IsFieldVisible(models, "txtLevelNbrStart") ? AppendSpace(ConvertToString(address.levelNumberStart)) : string.Empty)
                      .Replace(AddressFormatVariables.LevelNumberEnd, IsFieldVisible(models, "txtLevelNbrEnd") ? AppendSpace(ConvertToString(address.levelNumberEnd)) : string.Empty)
                      .Replace(AddressFormatVariables.HouseAlphaStart, IsFieldVisible(models, "txtHouseAlphaStart") ? AppendSpace(ConvertToString(address.houseNumberAlphaStart)) : string.Empty)
                      .Replace(AddressFormatVariables.HouseAlphaEnd, IsFieldVisible(models, "txtHouseAlphaEnd") ? AppendSpace(ConvertToString(address.houseNumberAlphaEnd)) : string.Empty);
            }
            else if (addressT is ContactAddressModel)
            {
                ContactAddressModel address = addressT as ContactAddressModel;

                addressLineBuilder.Replace(AddressFormatVariables.AddressType, IsFieldVisible(models, "ddlAddressType") ? AppendSpace(ScriptFilter.FilterScript(address.addressType, false)) : string.Empty)
                    .Replace(AddressFormatVariables.StreetAddress, IsFieldVisible(models, "txtFullAddress") ? AppendSpace(ScriptFilter.FilterScript(ConvertToString(address.fullAddress), false)) : string.Empty)
                    .Replace(AddressFormatVariables.AddressLine1, IsFieldVisible(models, "txtAddressLine1") ? AppendSpace(ScriptFilter.FilterScript(address.addressLine1, false)) : string.Empty)
                    .Replace(AddressFormatVariables.AddressLine2, IsFieldVisible(models, "txtAddressLine2") ? AppendSpace(ScriptFilter.FilterScript(address.addressLine2, false)) : string.Empty)
                    .Replace(AddressFormatVariables.AddressLine2, IsFieldVisible(models, "txtAddressLine3") ? AppendSpace(ScriptFilter.FilterScript(address.addressLine2, false)) : string.Empty)
                    .Replace(AddressFormatVariables.HouseNumberStart, IsFieldVisible(models, "txtStreetStart") ? AppendSpace(Convert.ToString(address.houseNumberStart)) : string.Empty)
                    .Replace(AddressFormatVariables.HouseNumberEnd, IsFieldVisible(models, "txtStreetEnd") ? AppendSpace(Convert.ToString(address.houseNumberEnd)) : string.Empty)
                    .Replace(AddressFormatVariables.Direction, IsFieldVisible(models, "ddlStreetDirection") ? AppendSpace(address.streetDirection) : string.Empty)
                    .Replace(AddressFormatVariables.Prefix, IsFieldVisible(models, "txtPrefix") ? AppendSpace(address.streetPrefix) : string.Empty)
                    .Replace(AddressFormatVariables.StreetName, IsFieldVisible(models, "txtStreetName") ? AppendSpace(address.streetName) : string.Empty)
                    .Replace(AddressFormatVariables.StreetType, IsFieldVisible(models, "ddlStreetType") ? AppendSpace(address.streetSuffix) : string.Empty)
                    .Replace(AddressFormatVariables.UnitType, IsFieldVisible(models, "ddlUnitType") ? AppendSpace(address.unitType) : string.Empty)
                    .Replace(AddressFormatVariables.UnitStart, IsFieldVisible(models, "txtUnitStart") ? AppendSpace(ConvertToString(address.unitStart)) : string.Empty)
                    .Replace(AddressFormatVariables.UnitEnd, IsFieldVisible(models, "txtUnitEnd") ? AppendSpace(ConvertToString(address.unitEnd)) : string.Empty)
                    .Replace(AddressFormatVariables.StreetSuffix, IsFieldVisible(models, "ddlStreetSuffixDirection") ? AppendSpace(address.streetSuffixDirection) : string.Empty)
                    .Replace(AddressFormatVariables.CountryCode, IsFieldVisible(models, "ddlCountry") ? ScriptFilter.FilterScript(AppendSpace(GetCountryByKey(address.countryCode, isForVEMap)), false) : string.Empty)
                    .Replace(AddressFormatVariables.City, IsFieldVisible(models, "txtCity") ? AppendSpace(ScriptFilter.FilterScript(address.city, false)) : string.Empty)
                    .Replace(AddressFormatVariables.State, IsFieldVisible(models, "txtState") ? AppendSpace(GetStateI18n(address.state, address.countryCode)) : string.Empty)
                    .Replace(AddressFormatVariables.Zip, IsFieldVisible(models, "txtZip") ? AppendSpace(ConvertToString(FormatZip(address.zip, address.countryCode))) : string.Empty)
                    .Replace(AddressFormatVariables.LevelPrefix, IsFieldVisible(models, "txtLevelPrefix") ? AppendSpace(ConvertToString(address.levelPrefix)) : string.Empty)
                    .Replace(AddressFormatVariables.LevelNumberStart, IsFieldVisible(models, "txtLevelNbrStart") ? AppendSpace(ConvertToString(address.levelNumberStart)) : string.Empty)
                    .Replace(AddressFormatVariables.LevelNumberEnd, IsFieldVisible(models, "txtLevelNbrEnd") ? AppendSpace(ConvertToString(address.levelNumberEnd)) : string.Empty)
                    .Replace(AddressFormatVariables.HouseAlphaStart, IsFieldVisible(models, "txtHouseAlphaStart") ? AppendSpace(ConvertToString(address.houseNumberAlphaStart)) : string.Empty)
                    .Replace(AddressFormatVariables.HouseAlphaEnd, IsFieldVisible(models, "txtHouseAlphaEnd") ? AppendSpace(ConvertToString(address.houseNumberAlphaEnd)) : string.Empty);
            }
            else if (addressT is DataRow)
            {
                DataRow dr = addressT as DataRow;
                addressLineBuilder.Replace(AddressFormatVariables.HouseNumberRangeStart, string.Empty)
                  .Replace(AddressFormatVariables.HouseNumberStart, IsFieldVisible(models, "txtStreetNo") ? AppendSpace(dr["HouseNumberStart"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.HouseNumberRangeEnd, string.Empty)
                  .Replace(AddressFormatVariables.HouseNumberEnd, IsFieldVisible(models, "txtStreetEnd") ? AppendSpace(dr["houseNumberEnd"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.Direction, IsFieldVisible(models, "ddlStreetDirection") ? AppendSpace(dr["StreetDirection"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.StreetName, IsFieldVisible(models, "txtStreetName") ? AppendSpace(dr["StreetName"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.StreetType, IsFieldVisible(models, "ddlStreetSuffix") ? AppendSpace(dr["StreetSuffix"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.UnitType, IsFieldVisible(models, "ddlUnitType") ? AppendSpace(dr["UnitType"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.UnitRangeStart, string.Empty)
                  .Replace(AddressFormatVariables.UnitStart, IsFieldVisible(models, "txtUnitNo") ? AppendSpace(dr["UnitStart"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.UnitRangeEnd, string.Empty)
                  .Replace(AddressFormatVariables.City, IsFieldVisible(models, "txtCity") ? AppendSpace(dr["City"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.State, IsFieldVisible(models, "txtState") ? AppendSpace(GetStateI18n(dr["State"].ToString(), dr["CountryCode"].ToString())) : string.Empty)
                  .Replace(AddressFormatVariables.Zip, IsFieldVisible(models, "txtZip") ? AppendSpace(FormatZip(dr["Zip"].ToString(), dr["CountryCode"].ToString())) : string.Empty)
                  .Replace(AddressFormatVariables.InspectionDistrict, IsFieldVisible(models, "txtInspectionD") ? AppendSpace(dr["inspectionDistrict"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.CountryCode, string.Empty)
                  .Replace(AddressFormatVariables.StartFraction, IsFieldVisible(models, "txtStartFraction") ? AppendSpace(dr["houseFractionStart"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.HouseFractionEnd, IsFieldVisible(models, "txtEndFraction") ? AppendSpace(dr["houseFractionEnd"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.Prefix, IsFieldVisible(models, "txtPrefix") ? AppendSpace(dr["streetPrefix"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.StreetSuffix, IsFieldVisible(models, "ddlStreetSuffixDirection") ? AppendSpace(dr["streetSuffixdirection"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.UnitEnd, IsFieldVisible(models, "txtUnitEnd") ? AppendSpace(dr["unitEnd"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.SecondaryRoad, IsFieldVisible(models, "txtSecondaryRoad") ? AppendSpace(dr["secondaryRoad"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.SecondaryRoadNumber, IsFieldVisible(models, "txtSecondaryRoadNo") ? AppendSpace(dr["secondaryRoadNumber"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.NeighborhoodPrefix, IsFieldVisible(models, "txtNeighborhoodP") ? AppendSpace(dr["neighberhoodPrefix"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.Neighborhood, IsFieldVisible(models, "txtNeighborhood") ? AppendSpace(dr["neighborhood"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.Description, IsFieldVisible(models, "txtDescription") ? AppendSpace(dr["addressDescription"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.Distance, IsFieldVisible(models, "txtDistance") ? AppendSpace(dr["distance"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.XCoordinator, string.Empty)
                  .Replace(AddressFormatVariables.YCoordinator, string.Empty)
                  .Replace(AddressFormatVariables.InspectionDistrictPrefix, IsFieldVisible(models, "txtInspectionDP") ? AppendSpace(dr["inspectionDistrictPrefix"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.County, IsFieldVisible(models, "txtCounty") ? AppendSpace(dr["County"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.StreetAddress, IsFieldVisible(models, "txtStreetAddress") ? AppendSpace(dr["fullAddress0"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.AddressLine1, IsFieldVisible(models, "txtAddressLine1") ? AppendSpace(dr["AddressLine1"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.AddressLine2, IsFieldVisible(models, "txtAddressLine2") ? AppendSpace(dr["AddressLine2"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.AddressType, AppendSpace(ScriptFilter.FilterScript(dr["addressType"].ToString(), false)))
                  .Replace(AddressFormatVariables.LevelPrefix, IsFieldVisible(models, "txtLevelPrefix") ? AppendSpace(dr["LevelPrefix"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.LevelNumberStart, IsFieldVisible(models, "txtLevelNbrStart") ? AppendSpace(dr["LevelNumberStart"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.LevelNumberEnd, IsFieldVisible(models, "txtLevelNbrEnd") ? AppendSpace(dr["LevelNumberEnd"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.HouseAlphaStart, IsFieldVisible(models, "txtHouseAlphaStart") ? AppendSpace(dr["HouseAlphaStart"].ToString()) : string.Empty)
                  .Replace(AddressFormatVariables.HouseAlphaEnd, IsFieldVisible(models, "txtHouseAlphaEnd") ? AppendSpace(dr["HouseAlphaEnd"].ToString()) : string.Empty);
            }
            
            //replace multiple ',' to one.
            string filterResult = Regex.Replace(addressLineBuilder.ToString().Trim(), @"[\,]{2,}", ",");

            //replace start or end ',' to empty.
            filterResult = Regex.Replace(filterResult.Trim(), @"^[\,]|[\,]$", string.Empty);

            //replace multiple '<br/>' to one.
            filterResult = Regex.Replace(filterResult.Trim(), @"(<br/>){2,}", "<br/>", RegexOptions.IgnoreCase);

            //replace start or end '<br/>' to empty.
            filterResult = Regex.Replace(filterResult.Trim(), @"^(<br/>)|(<br/>)$", string.Empty, RegexOptions.IgnoreCase);

            return filterResult.Trim();
        }

        /// <summary>
        /// Append space to string.
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>string append space</returns>
        private string AppendSpace(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim();

                if (!string.IsNullOrEmpty(str))
                {
                    str = AddressItemSeparator + str;
                }
            }

            return str;
        }

        /// <summary>
        /// Appends item separator.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="addressItem">an address item.</param>
        private void AppendItemSeparator(StringBuilder stringBuilder, string addressItem)
        {
            AppendItemSeparator(stringBuilder, addressItem, this.AddressItemSeparator);
        }

        /// <summary>
        /// Format the zip
        /// </summary>
        /// <param name="zip">original zip</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns>Formatted zip</returns>
        private string FormatZip(string zip, string countryCode)
        {
            ICacheManager cacheBll = ObjectFactory.GetObject<ICacheManager>();
            Dictionary<string, string> state;
            RegionalModel regionalModel = string.IsNullOrEmpty(countryCode)
                                              ? null
                                              : cacheBll.GetRegionalModelByCountry(countryCode, out state);
            string zipMask = string.Empty;

            if (regionalModel != null && !ValidationUtil.IsNo(regionalModel.useZipCode))
            {
                zipMask = regionalModel.zipCodeMask;
            }

            return I18nZipUtil.FormatZipByMask(zipMask, zip);
        }

        /// <summary>
        /// Get address pattern from standard choice.
        /// </summary>
        /// <param name="addressFormatType">address format key.</param>
        /// <returns>address format setting in standard choice.</returns>
        private string GetAddressPattern(AddressFormatType addressFormatType)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string addressPattern = bizBll.GetValueForStandardChoice(AgencyCode, BizDomainConstant.STD_CAT_I18N_ADDRESS_FORMAT, addressFormatType.ToString());

            if (string.IsNullOrEmpty(addressPattern))
            {
                switch (addressFormatType)
                {
                    case AddressFormatType.LONG_ADDRESS_WITH_FORMAT:
                        addressPattern = DEFAULT_LONG_ADDRESS_WITH_FORMAT;
                        break;
                    case AddressFormatType.LONG_ADDRESS_NO_FORMAT:
                        addressPattern = DEFAULT_LONG_ADDRESS_NO_FORMAT;
                        break;
                    case AddressFormatType.SHORT_ADDRESS_WITH_FORMAT:
                        addressPattern = DEFAULT_SHORT_ADDRESS_WITH_FORMAT;
                        break;
                    case AddressFormatType.SHORT_ADDRESS_NO_FORMAT:
                        addressPattern = DEFAULT_SHORT_ADDRESS_NO_FORMAT;
                        break;
                    default:
                        break;
                }
            }

            return addressPattern;
        }

        /// <summary>
        /// get field visible
        /// </summary>
        /// <param name="models">simple view element models</param>
        /// <param name="fieldName">field name</param>
        /// <returns>
        /// <c>true</c> if field is visible; otherwise, <c>false</c>.
        /// </returns>
        private bool IsFieldVisible(SimpleViewElementModel4WS[] models, string fieldName)
        {
            bool result = true;

            if (models != null && models.Length > 0)
            {
                foreach (SimpleViewElementModel4WS model in models)
                {
                    if (string.Equals(model.viewElementName, fieldName))
                    {
                        result = ACAConstant.VALID_STATUS.Equals(model.recStatus, StringComparison.OrdinalIgnoreCase);
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Convert object to string
        /// </summary>
        /// <param name="o">object value</param>
        /// <returns>string converted</returns>
        private string ConvertToString(object o)
        {
            return o == null ? string.Empty : o.ToString();
        }

        /// <summary>
        /// Trims the separator.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="needTrim">need Trim.</param>
        /// <param name="separators">The separators.</param>
        /// <returns>output string with ending with separators trimmed</returns>
        private string TrimSeparator(string input, bool needTrim, params string[] separators)
        {
            string result = input;

            if (needTrim)
            {
                result = string.IsNullOrEmpty(result) ? string.Empty : result.Trim();
            }

            if (!string.IsNullOrEmpty(result) && separators != null && separators.Length > 0)
            {
                foreach (string separator in separators)
                {
                    if (result.EndsWith(separator, StringComparison.OrdinalIgnoreCase))
                    {
                        result = result.Substring(0, result.Length - separator.Length);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Appends item separator.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="addressItem">an address item.</param>
        /// <param name="customizedItemSeparator">The customized item separator.</param>
        private void AppendItemSeparator(StringBuilder stringBuilder, string addressItem, string customizedItemSeparator)
        {
            if (!string.IsNullOrEmpty(addressItem) && addressItem.Trim().Length > 0)
            {
                addressItem = addressItem.Trim();
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(customizedItemSeparator);
                }

                stringBuilder.Append(addressItem);
            }
        }

        /// <summary>
        /// Get country by key for support multi-language.
        /// </summary>
        /// <param name="countryKey">country Key</param>
        /// <param name="isForVEMap">is For VEMap</param>
        /// <returns>country text for display.</returns>
        private string GetCountryByKey(string countryKey, bool isForVEMap)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(countryKey))
            {
                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                result = bizBll.GetValueForStandardChoice(AgencyCode, BizDomainConstant.STD_CAT_COUNTRY, countryKey);
            }

            if (string.IsNullOrEmpty(result) && isForVEMap)
            {
                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                result = bizBll.GetValueForACAConfig(AgencyCode, BizDomainConstant.STD_ITEM_DEFAULT_COUNTRY);
            }

            return result;
        }

        /// <summary>
        /// List item of Address format.
        /// </summary>
        private struct AddressFormatVariables
        {
            /// <summary>
            /// House number range start
            /// </summary>
            public const string HouseNumberRangeStart = "$$HouseNumberRangeStart$$";

            /// <summary>
            /// House number start
            /// </summary>
            public const string HouseNumberStart = "$$HouseNumberStart$$";

            /// <summary>
            /// House number range end
            /// </summary>
            public const string HouseNumberRangeEnd = "$$HouseNumberRangeEnd$$";

            /// <summary>
            /// House number end
            /// </summary>
            public const string HouseNumberEnd = "$$Street#(end)$$";

            /// <summary>
            /// the direction
            /// </summary>
            public const string Direction = "$$Direction$$";

            /// <summary>
            /// Street name
            /// </summary>
            public const string StreetName = "$$StreetName$$";

            /// <summary>
            /// Street type
            /// </summary>
            public const string StreetType = "$$StreetType$$";

            /// <summary>
            /// The unit type
            /// </summary>
            public const string UnitType = "$$UnitType$$";

            /// <summary>
            /// Unit range start
            /// </summary>
            public const string UnitRangeStart = "$$UnitRangeStart$$";

            /// <summary>
            /// Unit start
            /// </summary>
            public const string UnitStart = "$$UnitStart$$";

            /// <summary>
            /// Unit range end
            /// </summary>
            public const string UnitRangeEnd = "$$UnitRangeEnd$$";

            /// <summary>
            /// The city
            /// </summary>
            public const string City = "$$City$$";

            /// <summary>
            /// The state
            /// </summary>
            public const string State = "$$State$$";

            /// <summary>
            /// The zip
            /// </summary>
            public const string Zip = "$$Zip$$";

            /// <summary>
            /// The inspection District
            /// </summary>
            public const string InspectionDistrict = "$$InspectionDistrict$$";

            /// <summary>
            /// The CountryCode
            /// </summary>
            public const string CountryCode = "$$Country$$";

            /// <summary>
            /// The Start Fraction
            /// </summary>
            public const string StartFraction = "$$StartFraction$$";

            /// <summary>
            /// The House Fraction End
            /// </summary>
            public const string HouseFractionEnd = "$$EndFraction$$";

            /// <summary>
            /// The Prefix
            /// </summary>
            public const string Prefix = "$$Prefix$$";

            /// <summary>
            /// The Street Suffix
            /// </summary>
            public const string StreetSuffix = "$$StreetSuffix$$";

            /// <summary>
            /// The Unit End
            /// </summary>
            public const string UnitEnd = "$$Unit#(end)$$";

            /// <summary>
            /// The Secondary Road
            /// </summary>
            public const string SecondaryRoad = "$$SecondaryRoad$$";

            /// <summary>
            /// Secondary Road Number
            /// </summary>
            public const string SecondaryRoadNumber = "$$SecondaryRoadNumber$$";

            /// <summary>
            /// Neighborhood Prefix
            /// </summary>
            public const string NeighborhoodPrefix = "$$NeighborhoodPrefix$$";

            /// <summary>
            /// The Neighborhood
            /// </summary>
            public const string Neighborhood = "$$Neighborhood$$";

            /// <summary>
            /// The Description
            /// </summary>
            public const string Description = "$$Description$$";

            /// <summary>
            /// The Distance
            /// </summary>
            public const string Distance = "$$Distance$$";

            /// <summary>
            /// X Coordinator
            /// </summary>
            public const string XCoordinator = "$$XCoordinator$$";

            /// <summary>
            /// Y Coordinator
            /// </summary>
            public const string YCoordinator = "$$YCoordinator$$";

            /// <summary>
            /// Inspection District Prefix
            /// </summary>
            public const string InspectionDistrictPrefix = "$$InspectionDistrictPrefix$$";

            /// <summary>
            /// The County
            /// </summary>
            public const string County = "$$County$$";

            /// <summary>
            /// Street Address
            /// </summary>
            public const string StreetAddress = "$$StreetAddress$$";

            /// <summary>
            /// Address Line 1
            /// </summary>
            public const string AddressLine1 = "$$AddressLine1$$";

            /// <summary>
            /// Address Line 2
            /// </summary>
            public const string AddressLine2 = "$$AddressLine2$$";

            /// <summary>
            /// Address Type
            /// </summary>
            public const string AddressType = "$$AddressType$$";

            /// <summary>
            /// Level Prefix
            /// </summary>
            public const string LevelPrefix = "$$LevelPrefix$$";

            /// <summary>
            /// Level number start
            /// </summary>
            public const string LevelNumberStart = "$$LevelNumberStart$$";

            /// <summary>
            /// Level number end
            /// </summary>
            public const string LevelNumberEnd = "$$LevelNumberEnd$$";

            /// <summary>
            /// House alpha start
            /// </summary>
            public const string HouseAlphaStart = "$$HouseNumberAlphaStart$$";

            /// <summary>
            /// House alpha end
            /// </summary>
            public const string HouseAlphaEnd = "$$HouseNumberAlphaEnd$$";
        }
    }
}