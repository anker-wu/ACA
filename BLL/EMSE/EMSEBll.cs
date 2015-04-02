#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EMSEBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: EMSEBll.cs 277355 2014-08-14 07:28:03Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using log4net;

namespace Accela.ACA.BLL.EMSE
{
    /// <summary>
    /// This class provide the ability to operation emse script.
    /// </summary>
    public class EMSEBll : BaseBll, IEMSEBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of EMSEService.
        /// </summary>
        private EMSEWebServiceService EMSEService
        {
            get
            {
                return WSFactory.Instance.GetWebService<EMSEWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// copy trade name cap information to trade license cap.
        /// </summary>     
        /// <param name="eventName">event name.</param>
        /// <param name="sourceCapID">source Cap ID</param>
        /// <param name="targetCapID">target Cap ID</param>        
        /// <returns>error message</returns>
        public string CopyCapInfo(string eventName, CapIDModel4WS sourceCapID, CapIDModel4WS targetCapID)
        {
            try
            {
                return EMSEService.copyCapInfoFromTNtoTL(eventName, sourceCapID, targetCapID, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// update EMSE event and script association.
        /// </summary>
        /// <param name="eventName">event Name</param>
        /// <param name="scriptCode">script name</param>
        /// <param name="callerID">public user ID</param>
        public void CreateOrUpdateEventScriptCode(string eventName, string scriptCode, string callerID)
        {
            try
            {
                EMSEService.createOrUpdateEventScriptCode(AgencyCode, eventName, scriptCode, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get EMSE Script name from DB
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="callerID">public user ID</param>
        /// <returns>event script.</returns>
        public string GetEventScriptByPK(string eventName, string callerID)
        {
            try
            {
                return EMSEService.getEventScriptByPK(AgencyCode, eventName, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get list of EMSE event name by agencyCode 
        /// </summary>
        /// <returns>two array list,one is SCRIPT_CODE ,the other is EVENT_NAME</returns>
        public object[] GetScriptNameList()
        {
            try
            {
                return EMSEService.getScriptNameListByServProvCode(AgencyCode);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Judge whether config the Event Script, searched from Cache.
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <returns>Is config event script</returns>
        public bool IsConfigEventScript(string eventName)
        {
            return IsConfigEventScript(AgencyCode, eventName);
        }

        /// <summary>
        /// Judge whether config the Event Script, searched from Cache.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="eventName">event name.</param>
        /// <returns>Is config event script</returns>
        public bool IsConfigEventScript(string agencyCode, string eventName)
        {
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            Hashtable htEMSEEventConfig = cacheManager.GetCachedItem(agencyCode, agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_EMSEEVENT));

            if (htEMSEEventConfig == null || !htEMSEEventConfig.ContainsKey(eventName))
            {
                return false;
            }

            string emseScript = htEMSEEventConfig[eventName] as string;

            return !string.IsNullOrEmpty(emseScript);
        }

        /// <summary>
        /// when page flow property has EMSE event name,call this method to trigger EMSE
        /// </summary>
        /// <param name="agencyCode">Page flow's agency code.</param>
        /// <param name="scriptCode">SCRIPT_CODE value.</param>
        /// <param name="capModel4WS">capModel4WS object.</param>
        /// <param name="isFromConfirmPage">is from confirm Page.</param>
        /// <returns>EMSE result object.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public EMSEResultModel4WS RunEMSEScript4PageFlow(string agencyCode, string scriptCode, CapModel4WS capModel4WS, bool isFromConfirmPage = false)
        {
            try
            {
                return EMSEService.runEMSEScript4PageFlow(scriptCode, agencyCode, capModel4WS, isFromConfirmPage, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// this method to trigger login on EMSE
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="paramsModel">parameter object.</param>
        /// <returns>EMSE result object on login process.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public EMSEOnLoginResultModel4WS RunEMSEScriptOnLogin(string eventName, OnLoginParamsModel4WS paramsModel)
        {
            try
            {
                paramsModel.agencyCode = AgencyCode;
                paramsModel.language = Language;

                return EMSEService.runEMSEScriptOnLogin(eventName, AgencyCode, paramsModel, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// this method to trigger an emse after saving and resuming
        /// </summary>
        /// <param name="capModel">cap entity</param>
        /// <returns>EMSE result object.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public EMSEResultBaseModel4WS RunEMSEScriptSaveAndResume(CapModel4WS capModel)
        {
            try
            {
                return EMSEService.runEMSESaveAndResumeAfter(capModel, AgencyCode, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// this method to trigger EMSE to validate License
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="licenseModel">LicenseModel4WS entity</param>
        /// <returns>BaseModel4WS object.</returns>
        public EMSEResultBaseModel4WS RunEMSEValidationLicense(string eventName, LicenseModel4WS licenseModel)
        {
            try
            {
                return EMSEService.runEMSEValidationLicense(eventName, licenseModel, AgencyCode, User.PublicUserId); //EMSEService.runEMSEValidationLicense(eventName, servProvCode, licenseModel, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Trigger EMSE for shopping cart.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="eventName">The EMSE event name.</param>
        /// <param name="capTypes">The cap type list.</param>
        /// <param name="totalAmount">The total amount to pay.</param>
        /// <returns>Return the EMSE result.</returns>
        public EMSEResultModel4WS RunEMSEScript4ShoppingCart(string agencyCode, string eventName, CapTypeModel[] capTypes, double totalAmount)
        {
            try
            {
                EMSEResultModel4WS emseResultModel = EMSEService.runEMSEScript4ShoppingCart(agencyCode, eventName, capTypes, totalAmount, User.PublicUserId);

                return emseResultModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// trigger fee estimate event for display condition
        /// </summary>
        /// <param name="capModel4WS">cap Model.</param>
        public void TriggerFeeEstimateAfter4ACAEvent(CapModel4WS capModel4WS)
        {
            try
            {
                EMSEService.triggerFeeEstimateAfter4ACAEvent(capModel4WS, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}