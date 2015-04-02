#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UrlRoutingForAmendmentHandler.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: Url routing http handler.
*
*  Notes:
* $Id: UrlRoutingForAmendmentHandler.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System.Web;

using Accela.ACA.BLL;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// routing url path
    /// </summary>
    public class UrlRoutingForAmendmentHandler : UrlRoutingHandler
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

                    bool isAmendable = simpleCapModel.capType != null && ValidationUtil.IsYes(simpleCapModel.capType.isAmendable);

                    // check whether the cap can be used for amendment
                    bool canAmend = !CapUtil.IsPartialCap(simpleCapModel.capClass)
                                    && simpleCapModel.hasPrivilegeToHandleCap
                                    && isAmendable
                                    && FunctionTable.IsEnableCreateAmendment();

                    if (canAmend == false)
                    {
                        RedirectToErrorPage(LabelUtil.GetGUITextByKey("aca_urlrouting_label_nopermission_record"));
                    }

                    string definedRedirectUrlTemplate = GetUrlTemplate();
                    string redirectUrl = CreateUrlForAmendment(capModel, definedRedirectUrlTemplate);

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

        /// <summary>
        /// create url for cap amendment
        /// </summary>
        /// <param name="capModel">CapModel for ACA.</param>
        /// <param name="amendmentUrlTemplate">amendment Url Template</param>
        /// <returns>url for amendment.</returns>
        private string CreateUrlForAmendment(CapModel4WS capModel, string amendmentUrlTemplate)
        {
            string parentCapModelId = string.Format("{0}-{1}-{2}", capModel.capID.id1, capModel.capID.id2, capModel.capID.id3);
            string trackingId = capModel.capID.trackingID.ToString();

            ICapTypePermissionBll capTypPermissionBll = ObjectFactory.GetObject<ICapTypePermissionBll>();

            CapTypePermissionModel capTypePermission = new CapTypePermissionModel();
            capTypePermission.controllerType = ControllerType.COMBINEBUTTIONSETTING.ToString();
            capTypePermission.moduleName = capModel.moduleName;
            capTypePermission.group = capModel.capType.group;
            capTypePermission.type = capModel.capType.type;
            capTypePermission.subType = capModel.capType.subType;
            capTypePermission.category = capModel.capType.category;
            capTypePermission.entityType = EntityType.CreateAmendment.ToString();
            capTypePermission.entityKey2 = capModel.capStatus;
            string capTypeFilterName = capTypPermissionBll.GetCapTypeFilterByAppStatus(AgencyCode, capTypePermission);

            string createdBy = string.Empty;
            IProxyUserRoleBll proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS user = proxyUserRoleBll.GetCurrentUser(capModel);

            createdBy = ACAConstant.PUBLIC_USER_NAME + user.userSeqNum;

            // ~/Cap/CapType.aspx?agencyCode={0}&Module={1}&parentCapModelID={2}&trackingID={3}&filterName={4}&createdBy={5}&stepNumber=0
            return string.Format(amendmentUrlTemplate, AgencyCode, capModel.moduleName, parentCapModelId, trackingId, capTypeFilterName, createdBy);
        }

        #endregion
    }
}