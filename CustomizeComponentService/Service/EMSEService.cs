#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EMSEServices.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: BasePage.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.ComponentService.Model;
using Accela.ACA.CustomizeAPI;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.ComponentService.Service
{
    /// <summary>
    /// This class provide the ability to operate cap
    /// </summary>
    public class EMSEService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EMSEService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public EMSEService(UserContext context) : base(context)
        {
        }

        /// <summary>
        /// handled with the onLogin EMSE event
        /// </summary>
        /// <param name="eventName">The EMSE event name.</param>
        /// <param name="userName">The login parameters Model user name.</param>
        /// <returns>an EMSEOnLoginResultModel4WS</returns>
        public SimpleEMSEOnLoginResultModel RunAfterLoginEMSEEvent(string eventName, string userName)
        {
            OnLoginParamsModel4WS infoModel = new OnLoginParamsModel4WS();
            infoModel.username = userName;
            EMSEOnLoginResultModel4WS emseOnLoginResult = EmseUtil.RunEMSEScriptOnLogin(eventName, AppSession.User.PublicUserId, infoModel);
            SimpleEMSEOnLoginResultModel simpleEmseOnLoginResult = new SimpleEMSEOnLoginResultModel(emseOnLoginResult);

            return simpleEmseOnLoginResult;
        }
    }
}
