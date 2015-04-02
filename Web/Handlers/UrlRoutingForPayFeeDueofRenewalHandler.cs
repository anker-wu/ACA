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

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// routing url path
    /// </summary>
    public class UrlRoutingForPayFeeDueofRenewalHandler : UrlRoutingHandler
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

                    CapModel4WS capModel;
                    CapIDModel4WS capIdModel = new CapIDModel4WS
                                               {
                                                   id1 = CapId1,
                                                   id2 = CapId2,
                                                   id3 = CapId3,
                                                   serviceProviderCode = AgencyCode
                                               };

                    SimpleCapModel simpleCapModel = RecordInitializationHelper.GetRecordById(capIdModel, out capModel);

                    // check whether the cap can be used for pay fee
                    bool canAccess = false;

                    if (!simpleCapModel.hasPrivilegeToHandleCap)
                    {
                        if (ACAConstant.ANONYMOUS_FLAG.Equals(UserSeqNum))
                        {
                            UserUtil.ForceLoginForDeepLink();
                            return;
                        }
                    }
                    else
                    {
                        string renewalStatus = simpleCapModel.renewalStatus ?? string.Empty;

                        // exist fee no paid yet
                        if (!CapUtil.IsPartialCap(simpleCapModel.capClass)
                            && ACAConstant.PAYFEEDUE_RENEWAL.Equals(renewalStatus, StringComparison.InvariantCulture)
                            && !StandardChoiceUtil.IsRemovePayFee(AgencyCode)
                            && FunctionTable.IsEnableRenewRecord())
                        {
                            canAccess = true;
                        }
                    }

                    if (canAccess == false)
                    {
                        RedirectToErrorPage(LabelUtil.GetGUITextByKey("aca_urlrouting_label_nopermission_record"));
                    }

                    PrepareDataForPayFeeDueofRenwal(capModel);

                    string definedRedirectUrlTemplate = GetUrlTemplate();
                    string redirectUrl = string.Format(definedRedirectUrlTemplate, AgencyCode, capModel.moduleName, CapId1, CapId2, CapId3);
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

        /// <summary>
        /// Prepare data for pay fee due of renewal
        /// </summary>
        /// <param name="capModel">The Cap Model.</param>
        private void PrepareDataForPayFeeDueofRenwal(CapModel4WS capModel)
        {
            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            CapIDModel4WS childCapID = capBll.GetChildCapIDByMasterID(capModel.capID, ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE, false);

            if (childCapID == null)
            {
                RedirectToErrorPage("Can't get child CAP for renewal.");
            }

            //2. Get CapModel by child cap ID. Currently, renewnal logic does not supports super agency.
            var capWithConditionModel = capBll.GetCapViewBySingle(childCapID, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, false);

            if (capWithConditionModel == null || capWithConditionModel.capModel == null)
            {
                RedirectToErrorPage("Can't get CAP with condition.");
            }

            var childCapModel = capWithConditionModel == null ? null : capWithConditionModel.capModel;

            if (childCapModel == null)
            {
                RedirectToErrorPage("Invalid Record ID for renewal.");
            }

            RecordInitializationHelper.InitilizeRelatedDataToSession(childCapModel);
        }

        #endregion
    }
}