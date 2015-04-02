#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EtisalatHelper.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EtisalatHelper.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  1-2-2009           Xinter Peng               Initial
 * </pre>
 */

#endregion Header

using System;
using System.Web;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// ETISALAT helper
    /// </summary>
    public class EtisalatHelper
    {
        #region Fields

        /// <summary>
        /// Action source: amendment cap
        /// </summary>
        public const string ACTIONSOURCE_AMENDMENTCAP = "Amendment CAP";

        /// <summary>
        /// Action source: create permit
        /// </summary>
        public const string ACTIONSOURCE_CREATEPERMIT = "CreatePermit";

        /// <summary>
        /// Action source: pay fee due
        /// </summary>
        public const string ACTIONSOURCE_PAYFEEDUE = "PayFeeDue";

        /// <summary>
        /// Action source: renew license
        /// </summary>
        public const string ACTIONSOURCE_RENEWLICENSE = "RenewLicense";

        /// <summary>
        /// return code: completion failed
        /// </summary>
        private const string RETURNCODE_COMPLETION_FAILED = "-100";

        /// <summary>
        /// return code: completion succeed
        /// completion return code, original code is 0 or negative number, negative number stands for error.
        /// </summary>
        private const string RETURNCODE_COMPLETION_SUCCEEDED = "100";

        /// <summary>
        /// return code: registration failed
        /// </summary>
        private const string RETURNCODE_REGISTRATION_FAILED = "-1";

        /// <summary>
        /// return code: registration paid
        /// </summary>
        private const string RETURNCODE_REGISTRATION_PAID = "3";

        /// <summary>
        /// return code: registered
        /// </summary>
        private const string RETURNCODE_REGISTRATION_REGISTERED = "1";

        /// <summary>
        /// return code: registration succeed
        /// </summary>
        private const string RETURNCODE_REGISTRATION_SUCCEEDED = "0";

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(EtisalatAdapter));

        #endregion Fields

        #region Methods

        /// <summary>
        /// adjust return code for completion stage.
        /// </summary>
        /// <param name="returnModel">The return model.</param>
        public static void AdjustReturnCode4Completion(EMSEResultBaseModel4WS returnModel)
        {
            if (returnModel != null &&
                !string.IsNullOrEmpty(returnModel.returnCode))
            {
                if ("0".Equals(returnModel.returnCode.Trim()))
                {
                    returnModel.returnCode = RETURNCODE_COMPLETION_SUCCEEDED;
                }
                else
                {
                    returnModel.returnCode = RETURNCODE_COMPLETION_FAILED;
                }
            }
        }

        /// <summary>
        /// decode query string value
        /// </summary>
        /// <param name="val">encoded value</param>
        /// <returns>decoded value</returns>
        public static string Decode(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }
            else
            {
                return val.Replace("~~", "|");
            }
        }

        /// <summary>
        /// encode query string value
        /// </summary>
        /// <param name="val">original value</param>
        /// <returns>encoded value</returns>
        public static string Encode(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }
            else
            {
                return val.Replace("|", "~~");
            }
        }

        /// <summary>
        /// extract value from query string
        /// </summary>
        /// <param name="queryString">query string</param>
        /// <param name="key">The key</param>
        /// <returns>extracted value</returns>
        public static string ExtractValueFromQueryString(string queryString, string key)
        {
            try
            {
                HttpRequest myHttpRequest = new HttpRequest("a", "http://local/", queryString);
                string result = myHttpRequest[key];
                if (string.IsNullOrEmpty(result))
                {
                    return string.Empty;
                }
                else
                {
                    return result.Trim();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Extract value from URL
        /// </summary>
        /// <param name="tempURL">temp URL</param>
        /// <param name="key">The key</param>
        /// <returns>extracted value</returns>
        public static string ExtractValueFromURL(string tempURL, string key)
        {
            try
            {
                Uri myUri = new Uri(tempURL);
                string myUriQuerystring = string.Empty;
                if (myUri.Query.StartsWith("?"))
                {
                    myUriQuerystring = myUri.Query.Substring(1);
                }

                return ExtractValueFromQueryString(myUriQuerystring, key);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// get action source
        /// </summary>
        /// <param name="isRenewalFlag">is renewal</param>
        /// <param name="isPay4ExistingCapFlag">is pay for existing Cap</param>
        /// <param name="isAmendmentFlag">is amendment</param>
        /// <returns>action source</returns>
        public static string GetActionSource(string isRenewalFlag, string isPay4ExistingCapFlag, string isAmendmentFlag)
        {
            //PAY_FEE_DUE
            if (ACAConstant.COMMON_Y == isPay4ExistingCapFlag)
            {
                return ACTIONSOURCE_PAYFEEDUE;
            }
            else if (ACAConstant.COMMON_Y == isRenewalFlag)
            {
                return ACTIONSOURCE_RENEWLICENSE;
            }
            else if (ACAConstant.COMMON_Y == isAmendmentFlag)
            {
                //Amendment
                return ACTIONSOURCE_AMENDMENTCAP;
            }
            else
            {
                //CreatePermit
                return ACTIONSOURCE_CREATEPERMIT;
            }
        }

        /// <summary>
        /// Get ETISALAT Status from EMSEResultBaseModel4WS
        /// </summary>
        /// <param name="returnModel">EMSE result base model</param>
        /// <returns>ETISALAT Status</returns>
        public static EtisalatStatus GetStatus(EMSEResultBaseModel4WS returnModel)
        {
            string returnCode = returnModel == null ? string.Empty : returnModel.returnCode;
            return ConvertToStatus(returnCode);
        }

        /// <summary>
        /// check if current CAP is doing amendment
        /// </summary>
        /// <param name="capIdModel">cap Id Model</param>
        /// <param name="userSeqNum">user sequence number</param>
        /// <returns>checking result</returns>
        public static bool IsDoingAmendment(CapIDModel4WS capIdModel, string userSeqNum)
        {
            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            CapModel4WS capModel = capBll.GetCapViewDetailByPK(capIdModel, userSeqNum);
            if (capModel != null &&
                capModel.parentCapID != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// convert return code to status
        /// </summary>
        /// <param name="returnCode">return code</param>
        /// <returns>ETISALAT Status</returns>
        private static EtisalatStatus ConvertToStatus(string returnCode)
        {
            if (string.IsNullOrEmpty(returnCode))
            {
                return EtisalatStatus.Unknown;
            }

            returnCode = returnCode.Trim();
            if (RETURNCODE_REGISTRATION_SUCCEEDED.Equals(returnCode))
            {
                return EtisalatStatus.RegistrationSucceeded;
            }
            else if (RETURNCODE_REGISTRATION_REGISTERED.Equals(returnCode))
            {
                return EtisalatStatus.Registered;
            }
            else if (RETURNCODE_REGISTRATION_PAID.Equals(returnCode))
            {
                return EtisalatStatus.Paid;
            }
            else if (RETURNCODE_REGISTRATION_FAILED.Equals(returnCode))
            {
                return EtisalatStatus.RegistrationFailed;
            }
            else if (RETURNCODE_COMPLETION_SUCCEEDED.Equals(returnCode))
            {
                return EtisalatStatus.CompletionSucceeded;
            }
            else if (RETURNCODE_COMPLETION_FAILED.Equals(returnCode))
            {
                return EtisalatStatus.CompletionFailed;
            }
            else
            {
                return EtisalatStatus.Unknown;
            }
        }

        #endregion Methods
    }
}