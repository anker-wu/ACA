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
using Accela.ACA.Web.Payment;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// routing url path
    /// </summary>
    public class UrlRoutingForRenewalHandler : UrlRoutingHandler
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

                    // check whether the cap can be accessed
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
                        
                        if (!CapUtil.IsPartialCap(simpleCapModel.capClass)
                            && FunctionTable.IsEnableRenewRecord()
                            && !string.IsNullOrEmpty(renewalStatus))
                        {
                            canAccess = true;
                        }
                    }

                    if (canAccess == false)
                    {
                        RedirectToErrorPage(LabelUtil.GetGUITextByKey("aca_urlrouting_label_nopermission_record"));
                    }

                    string definedRedirectUrlTemplate = GetUrlTemplate();

                    // handle for renewal
                    string redirectUrl = CreateUrlForRenewal(definedRedirectUrlTemplate, capModel);
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
        /// create url for cap Renewal
        /// </summary>
        /// <param name="definedRedirectUrlTemplate">defined Redirect Url Template</param>
        /// <param name="capModel">CapModel for ACA.</param>
        /// <returns>url for Renewal.</returns>
        private string CreateUrlForRenewal(string definedRedirectUrlTemplate, CapModel4WS capModel)
        {
            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();

            //for renewal licenses and permit process
            if (!EmseUtil.IsConfigEventScript(AgencyCode, ACAConstant.EMSE_APPLICATION_SUBMIT_AFTER))
            {
                RedirectToErrorPage(LabelUtil.GetTextByKey("per_permitList_renewal_noemseconfiguration", capModel.moduleName));
            }

            if (!capBll.IsValidConfig4Renewal(capModel.capID))
            {
                RedirectToErrorPage(LabelUtil.GetTextByKey("per_permitList_renewal_configuration_error", capModel.moduleName));
            }

            string filterName = capModel.capType.filterName;
            capModel = capBll.CreateOrGetRenewalPartialCap(capModel.capID);
            string redirectUrl = string.Empty;

            if (PaymentHelper.GetPaymentAdapterType() == PaymentAdapterType.Etisalat && ACAConstant.CAP_STATUS_PENDING.Equals(capModel.capClass, StringComparison.InvariantCultureIgnoreCase))
            {
                var actionFlag = EtisalatHelper.ACTIONSOURCE_RENEWLICENSE;
                string isPay4ExistingCapFlag = ACAConstant.COMMON_N;
                string isRenewalFlag = ACAConstant.COMMON_Y;

                redirectUrl = EtisalatAdapter.CheckPayment(capModel.moduleName, capModel.capID.id1, capModel.capID.id2, capModel.capID.id3, capModel.capID.customID, actionFlag, AgencyCode, isPay4ExistingCapFlag, isRenewalFlag);

                if (string.IsNullOrEmpty(redirectUrl))
                {
                    RedirectToErrorPage(LabelUtil.GetTextByKey("ACA_Pageflow_SaveError_Message", capModel.moduleName));
                }
            }
            else
            {
                //?permitType=renewal&amp;Module={0}&amp;FilterName={1}&amp;isFeeEstimator={2}&amp;stepNumber=2&amp;pageNumber=1&amp;isRenewal=Y
                redirectUrl = string.Format(definedRedirectUrlTemplate, capModel.moduleName, filterName, ACAConstant.COMMON_N);
            }

            // set template
            CapUtil.FillCapModelTemplateValue(capModel);

            // store cap to session
            CapUtil.SetCapInfoToAppSession(capModel, capModel.capType, capModel.moduleName);

            // Get pageflow group and store it to session
            IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
            var pageflowGroup = pageflowBll.GetPageflowGroupByCapType(capModel.capType);
            pageflowGroup = CapUtil.GetPageFlowWithoutBlankPage(capModel, pageflowGroup);
            AppSession.SetPageflowGroupToSession(pageflowGroup);

            return redirectUrl;
        }

        #endregion
    }
}