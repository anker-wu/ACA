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

using System.Web;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// routing url path
    /// </summary>
    public class UrlRoutingForResumeHandler : UrlRoutingHandler
    {
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

                    // check whether the cap can be used for resume
                    bool canResume = false;

                    // check whether it's anonymous user 
                    if (simpleCapModel.hasPrivilegeToHandleCap)
                    {
                        if (CapUtil.IsPartialCap(simpleCapModel.capClass))
                        {
                            canResume = true;
                        }
                    }

                    if (canResume == false)
                    {
                        RedirectToErrorPage(LabelUtil.GetGUITextByKey("aca_urlrouting_label_nopermission_record"));
                    }

                    RecordInitializationHelper.InitilizeRelatedDataToSession(capModel);

                    string definedRedirectUrlTemplate = GetUrlTemplate();

                    // ~/Cap/CapEdit.aspx?agencyCode={0}&Module={1}&capID1={2}&capID2={3}&capID3={4}&permitType=resume&stepNumber=2&pageNumber=1&isFeeEstimator={5}&FilterName={6}
                    string redirectUrl = string.Format(
                        definedRedirectUrlTemplate,
                        AgencyCode,
                        capModel.moduleName,
                        CapId1,
                        CapId2,
                        CapId3,
                        capModel.capClass == ACAConstant.INCOMPLETE ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                        GetFilterNameForResume(capModel));

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
        /// get filter name for resume application
        /// </summary>
        /// <param name="capModel">Cap model.</param>
        /// <returns>filter name.</returns>
        private string GetFilterNameForResume(CapModel4WS capModel)
        {
            string filterName = string.Empty;
            ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
            bool isTradeLicenseCap = capTypeBll.IsMatchTheFilter(capModel.capType, capModel.moduleName, ACAConstant.REQUEST_PARMETER_TRADE_LICENSE);

            if (isTradeLicenseCap)
            {
                filterName = ACAConstant.REQUEST_PARMETER_TRADE_LICENSE;
            }
            else
            {
                bool isTradeNameCap = capTypeBll.IsMatchTheFilter(capModel.capType, capModel.moduleName, ACAConstant.REQUEST_PARMETER_TRADE_NAME);

                if (isTradeNameCap)
                {
                   filterName = ACAConstant.REQUEST_PARMETER_TRADE_NAME;
                }
            }

            return filterName;
        }
    }
}