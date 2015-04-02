#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UrlRoutingHandler.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: Url routing http handler.
*
*  Notes:
* $Id: FileUploadHandler.ashx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Web;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// routing url path
    /// </summary>
    public class UrlRoutingForRequestTradeLicenceHandler : UrlRoutingHandler
    {
        #region Method

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the System.Web.IHttpHandler interface.
        /// </summary>
        /// <param name="context">An System.Web.HttpContext object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                if (ConfigObject != null)
                {
                    SetContextCulture();

                    if (ACAConstant.ANONYMOUS_FLAG.Equals(UserSeqNum))
                    {
                        UserUtil.ForceLoginForDeepLink();
                        return;
                    }

                    CapModel4WS capModel;
                    CapIDModel4WS capIdModel = new CapIDModel4WS
                                               {
                                                   id1 = CapId1,
                                                   id2 = CapId2,
                                                   id3 = CapId3,
                                                   serviceProviderCode = AgencyCode
                                               };

                    SimpleCapModel simpleCapModel = RecordInitializationHelper.GetRecordById(capIdModel, out capModel);

                    bool isTNExpired = simpleCapModel.isTNExpired;
                    string appStatusGroup = simpleCapModel.statusGroupCode;
                    string appStatus = simpleCapModel.capStatus;
                    string filterName = simpleCapModel.capType.filterName;
                    bool canRequestTN = false;

                    // check whether the cap can be used for Request TradeLicense
                    if (!CapUtil.IsPartialCap(simpleCapModel.capClass)
                        && simpleCapModel.hasPrivilegeToHandleCap
                        && ACAConstant.REQUEST_PARMETER_TRADE_NAME.Equals(filterName, StringComparison.InvariantCulture)
                        && !isTNExpired
                        && LicenseUtil.IsDisplayRequestTradeLicenseLink(AgencyCode, capModel.moduleName, appStatusGroup, appStatus))
                    {
                        canRequestTN = true;
                    }

                    if (canRequestTN == false)
                    {
                        RedirectToErrorPage(LabelUtil.GetGUITextByKey("aca_urlrouting_label_nopermission_record"));
                    }

                    string definedRedirectUrlTemplate = GetUrlTemplate();
                    string redirectUrl = string.Format(definedRedirectUrlTemplate, capModel.moduleName, simpleCapModel.altID, simpleCapModel.licenseType);

                    HttpContext.Current.Session[SessionConstant.FROM_DEEP_LINK] = true;
                    RedirectToURL(redirectUrl);
                }
            }
            catch (ACAException ex)
            {
                RedirectToErrorPage(ex.Message);
            }
        }

        /// <summary>
        ///  Need validate(CapId1, CapId2, CapId3)
        /// </summary>
        /// <returns>Pass return true. Else redirect error page</returns>
        protected override bool IsParamsValidated()
        {
            return ValidateParams(CapId1, CapId2, CapId3);
        }

        #endregion
    }
}