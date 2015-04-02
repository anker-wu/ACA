/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GFilterViewService.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 *  
 * 
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Linq;
using System.Web.Services;
using Accela.ACA.BLL.Admin;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.WebService
{
    /// <summary>
    /// The GFilter View Web Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class GFilterViewService : System.Web.Services.WebService
    {
        /// <summary>
        /// The admin BLL instance.
        /// </summary>
        private IAdminBll _adminBll = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GFilterViewService"/> class.
        /// </summary>
        public GFilterViewService()
        {
            if (!AppSession.IsAdmin)
            {
                throw new ACAException("unauthenticated web service invoking");
            }

             _adminBll = (IAdminBll)ObjectFactory.GetObject(typeof(IAdminBll));
        }

        /// <summary>
        /// Saves the filter screen view.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="levelType">Type of the level.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="simpleViewModel">The simple view model.</param>
        /// <param name="callerid">The caller id.</param>
        [WebMethod(EnableSession = true)]
        public void SaveFilterScreenView(string agencyCode, string levelType, string moduleName, SimpleViewModel4WS simpleViewModel, string callerid)
        {
            _adminBll.SaveSimpleViewModel(agencyCode, levelType, moduleName, simpleViewModel, callerid);
        }

        /// <summary>
        /// Gets the filter screen view. For form designer only.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="levelType">Type of the level.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="viewID">The view unique identifier.</param>
        /// <param name="screenPermission">The screen permission.</param>
        /// <param name="callerid">The caller id.</param>
        /// <returns>The SimpleViewModel4WS.</returns>
        [WebMethod(EnableSession = true)]
        public SimpleViewModel4WS GetFilterScreenView(string agencyCode, string levelType, string moduleName, string viewID, GFilterScreenPermissionModel4WS screenPermission, string callerid)
        {
            GFilterScreenPermissionModel4WS clonePermissionModel = ControlBuildHelper.GetPermissionWithGenericTemplate(viewID, screenPermission.permissionLevel, screenPermission.permissionValue);

            SimpleViewModel4WS simpleViewModel = _adminBll.GetSimpleViewMode(agencyCode, levelType, moduleName, viewID, clonePermissionModel, callerid);
            simpleViewModel.permissionModel = screenPermission;

            if (viewID == GviewID.LicenseeDetail)
            {
                simpleViewModel.labelLayoutType = string.IsNullOrEmpty(simpleViewModel.labelLayoutType)
                                                      ? "Left"
                                                      : simpleViewModel.labelLayoutType;

                foreach (SimpleViewElementModel4WS item in simpleViewModel.simpleViewElements)
                {
                    if (!string.Equals(item.elementType, ControlType.Line.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        item.elementType = ACAConstant.ELEMENT_TYPE_LABEL;

                        //Does not support Unit for NameValue control in license detail page.
                        item.unitType = string.Empty;
                    }
                }
            }

            if (GviewID.RegistrationContactForm.Equals(viewID) || GviewID.AuthAgentNewClerkContactForm.Equals(viewID))
            {
                ACAConstant.ContactSectionPosition contactSection = GviewID.RegistrationContactForm.Equals(viewID)
                                ? ACAConstant.ContactSectionPosition.RegisterAccount
                                : ACAConstant.ContactSectionPosition.RegisterClerk;

                bool genericTemplateVisible = PeopleUtil.IsVisibleGenericTemplate(contactSection);

                if (genericTemplateVisible)
                {
                    return simpleViewModel;
                }

                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                var peopleTemplateModels = templateBll.GetPeopleTemplateAttributes(screenPermission.permissionValue, agencyCode, callerid);
                var genericTemplateItems = simpleViewModel.simpleViewElements.Where(o => ValidationUtil.IsNo(o.standard));

                if (peopleTemplateModels != null && peopleTemplateModels.Length > 0)
                {
                    genericTemplateItems = genericTemplateItems.Where(o => !peopleTemplateModels.Select(t => t.attributeName).Contains(o.viewElementName));
                }

                foreach (SimpleViewElementModel4WS item in genericTemplateItems)
                {
                    item.recStatus = ACAConstant.INVALID_STATUS;
                }
            }

            return simpleViewModel;
        }
    }
}
