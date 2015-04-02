/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: IAddressBuilderBll.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: IAddressBuilderBll.cs 276976 2014-08-08 10:10:01Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,       &lt;Who&gt;,        &lt;What&gt;
 * </pre>
 */
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.AddressBuilder
{
    /// <summary>
    /// Interface Address Builder business
    /// </summary>
    public interface IAddressBuilderBll
    {
        /// <summary>
        /// Generate Address string from a specified provider detail model.
        /// </summary>
        /// <param name="providerDetailModel">a provider detail model</param>
        /// <returns>an assembled address string</returns>
        string BuildAddress4Provider(ProviderDetailModel providerDetailModel);

        /// <summary>
        /// Build format long address.
        /// </summary>
        /// <typeparam name="T">address information object</typeparam>
        /// <param name="addressModel">AddressModel model</param>
        /// <param name="models">The models.</param>
        /// <param name="formatType">Type of the format.</param>
        /// <returns>an assembled address string</returns>
        string BuildAddressByFormatType<T>(T addressModel, SimpleViewElementModel4WS[] models, AddressFormatType formatType);

        /// <summary>
        /// Get the formatted value for "Work Location"
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <param name="models">The models.</param>
        /// <param name="additionalLocationTitle">The additional location title.</param>
        /// <returns>an assembled address string</returns>
        string Build4WorkLocation(AddressModel[] addresses, SimpleViewElementModel4WS[] models, string additionalLocationTitle);

        /// <summary>
        /// Build address from AddressModel
        /// </summary>
        /// <param name="addressModel">The address model.</param>
        /// <returns>an assembled address string</returns>
        string Build4Map(AddressModel addressModel);

        /// <summary>
        /// Build address for license display
        /// </summary>
        /// <param name="licenseModel">the license model</param>
        /// <param name="country">the country</param>
        /// <returns>Return the license address string</returns>
        string Build4License(LicenseModel4WS licenseModel, string country);

        /// <summary>
        /// Generate Address string from a specified provider location model.
        /// </summary>
        /// <param name="lc">a provider location model</param>
        /// <returns>an assembled address string</returns>
        string BuildAddress4ProviderLocation(RProviderLocationModel lc);

        /// <summary>
        /// Gets the state i18n.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns>State I18N value.</returns>
        string GetStateI18n(string state, string countryCode);
    }
}
