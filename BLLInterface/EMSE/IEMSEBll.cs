#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IEMSEBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IEMSEBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.BLL.EMSE
{
    /// <summary>
    /// Defines methods for EMSE function.
    /// </summary>
    public interface IEMSEBll
    {
        #region Methods

        /// <summary>
        /// copy trade name cap information to trade license cap.
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="sourceCapID">source Cap ID</param>
        /// <param name="targetCapID">target Cap ID</param>
        /// <returns>error message</returns>
        string CopyCapInfo(string eventName, CapIDModel4WS sourceCapID, CapIDModel4WS targetCapID);

        /// <summary>
        /// update EMSE event and script association.
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="scriptCode">script name</param>
        /// <param name="callerID">public user ID</param>        
        void CreateOrUpdateEventScriptCode(string eventName, string scriptCode, string callerID);

        /// <summary>
        /// get EMSE Script name from DB
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="callerID">public user ID</param>
        /// <returns>event script.</returns>
        string GetEventScriptByPK(string eventName, string callerID);

        /// <summary>
        /// get list of EMSE event name by agencyCode 
        /// </summary>
        /// <returns>two array list,one is EMSE code ,the other is EMSE title</returns>
        object[] GetScriptNameList();

        /// <summary>
        /// Judge whether config the Event Script, searched from Cache.
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <returns>Is config event script</returns>
        bool IsConfigEventScript(string eventName);

        /// <summary>
        /// Judge whether config the Event Script, searched from Cache.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="eventName">event name.</param>
        /// <returns>Is config event script</returns>
        bool IsConfigEventScript(string agencyCode, string eventName);

        /// <summary>
        /// when page flow property has EMSE event name,call this method to trigger EMSE
        /// </summary>
        /// <param name="agencyCode">Page flow's agency code.</param>
        /// <param name="scriptCode">SCRIPT_CODE value.</param>
        /// <param name="capModel4WS">capModel4WS object.</param>
        /// <param name="isFromConfirmPage">is from confirm Page.</param>
        /// <returns>EMSE result object.</returns>
        EMSEResultModel4WS RunEMSEScript4PageFlow(string agencyCode, string scriptCode, CapModel4WS capModel4WS, bool isFromConfirmPage);

        /// <summary>
        /// this method to trigger login on EMSE
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="paramsModel">parameter object.</param>
        /// <returns>EMSE result object on login process.</returns>
        EMSEOnLoginResultModel4WS RunEMSEScriptOnLogin(string eventName, OnLoginParamsModel4WS paramsModel);

        /// <summary>
        /// this method to trigger an emse after saving and resuming
        /// </summary>
        /// <param name="capModel">cap entity</param>
        /// <returns>EMSE result object.</returns>
        EMSEResultBaseModel4WS RunEMSEScriptSaveAndResume(CapModel4WS capModel);

        /// <summary>
        /// this method to trigger EMSE to validate License
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="licenseModel">LicenseModel4WS entity</param>
        /// <returns>BaseModel4WS object.</returns>
        EMSEResultBaseModel4WS RunEMSEValidationLicense(string eventName, LicenseModel4WS licenseModel);

        /// <summary>
        /// Trigger EMSE for shopping cart.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="eventName">The EMSE event name.</param>
        /// <param name="capTypes">The cap type list.</param>
        /// <param name="totalAmount">The total amount to pay.</param>
        /// <returns>Return the EMSE result.</returns>
        EMSEResultModel4WS RunEMSEScript4ShoppingCart(string agencyCode, string eventName, CapTypeModel[] capTypes, double totalAmount);

        /// <summary>
        /// trigger fee estimate event for display condition
        /// </summary>
        /// <param name="capModel"> capModel4WS entity</param>
        void TriggerFeeEstimateAfter4ACAEvent(CapModel4WS capModel);

        #endregion Methods
    }
}