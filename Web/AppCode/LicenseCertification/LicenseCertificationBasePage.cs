#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseCertificationBasePage.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  This class deal with time zone conversion
 *
 *  Notes:
 * $Id: LicenseCertificationBasePage.cs 241086 2012-12-31 07:23:04Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Web.UI;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.AppCode.LicenseCertification
{
    /// <summary>
    /// License certification base page, display generic template and change the form designer layout.
    /// </summary>
    public class LicenseCertificationBasePage : FormDesignerBaseControl
    {
        #region fields

        /// <summary>
        /// The postBack ID to indicate user behavior that input a education name directly.
        /// </summary>
        protected const string POST_BACK_BY_CHANGE_NAME = "$POSTBACKBYCHANGENAME";

        /// <summary>
        /// The EXAMINATION POSTBACK STRING
        /// </summary>
        protected const string EXAMINATION_POSTBACK_STRING = "$EXAMIANTIONPOSTBACK";

        /// <summary>
        /// The postBack ID to indicate the user behavior that user change the continue education name directly.
        /// </summary>
        protected const string POST_BACK_BY_CHANGE_CONTEDU = "$POSTBACKBYCHANGECONTEDU";

        /// <summary>
        /// The postBack ID to indicate the user behavior that user change the country when select provider.
        /// </summary>
        protected const string POST_BACK_BY_COUNTRY = "$POSTBACKBYCOUNTRY";

        /// <summary>
        ///  Indicates if need to initialize regional settings
        /// </summary>
        private bool _isNeedInitRegional = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseCertificationBasePage" /> class.
        /// </summary>
        /// <param name="viewId">The view id.</param>
        public LicenseCertificationBasePage(string viewId)
            : base(viewId)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Accela form designer place holder.
        /// </summary>
        /// <value>
        /// The accela form designer place holder.
        /// </value>
        protected AccelaFormDesignerPlaceHolder AccelaFormDesignerPlaceHolder { get; set; }

        /// <summary>
        /// Gets or sets the generic template edit control.
        /// </summary>
        /// <value>
        /// The generic template edit control.
        /// </value>
        protected GenericTemplateEdit GenericTemplateEditControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to initialize regional settings.
        /// </summary>
        protected bool IsNeedInitRegional
        {
            get
            {
                return _isNeedInitRegional;
            }

            set
            {
                _isNeedInitRegional = value;
            }
        }

        /// <summary>
        /// Gets the agency code of current cap.
        /// </summary>
        protected string CapAgencyCode
        {
            get
            {
                if (AppSession.IsAdmin)
                {
                    return ConfigManager.AgencyCode;
                }
                else
                {
                    CapModel4WS cap = AppSession.GetCapModelFromSession(ModuleName);
                    return cap != null && cap.capType != null ? cap.capType.serviceProviderCode : ConfigManager.AgencyCode;
                }
            }
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// set the form designer permission's PermissionValue field based the specific the reference object ID. 
        /// </summary>
        /// <param name="refObjectId">Exam/Education/Continue Education use the reference sequence ID to indicate the different form layout.</param>
        public void SetPermissionValue(string refObjectId)
        {
            Permission = ControlBuildHelper.GetPermissionWithGenericTemplate(ViewId, Permission.permissionLevel, refObjectId);
        }

        /// <summary>
        /// Display generic template based on the specific reference object ID.
        /// </summary>
        /// <param name="refobjectId">Reference object ID used to get the generic template.</param>
        /// <param name="entityKey1">The entity key1.</param>
        public void DisplayGenericTemplate(string refobjectId, string entityKey1)
        {
            GenericTemplateEntityType entityType = GenericTemplateEntityType.DefaultEntity;

            switch (ViewId)
            {
                case GviewID.EducationEdit:
                case GviewID.RefContactEducationEdit:
                    entityType = GenericTemplateEntityType.RefEducation;
                    break;
                case GviewID.ContinuingEducationEdit:
                case GviewID.RefContactContinuingEducationEdit:
                    entityType = GenericTemplateEntityType.ContinuingEducationDefinition;
                    break;
                case GviewID.ExaminationEdit:
                case GviewID.RefContactExaminationEdit:
                    entityType = GenericTemplateEntityType.RefExamination;
                    break;
            }

            long? refNbr = null;
            long tempRefNbr;

            if (long.TryParse(refobjectId, out tempRefNbr))
            {
                refNbr = tempRefNbr;
            }

            /*
             * Get the template by Ref Entity.
             * Will get the default template if the refNbr is null.
             */
            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            TemplateModel model =
                templateBll.GetGenericTemplateStructureByEntityPKModel(
                    new EntityPKModel()
                        {
                            serviceProviderCode = CapAgencyCode,
                            entityType = (int)entityType,
                            seq1 = refNbr,
                            key1 = entityKey1
                        },
                    false,
                    AppSession.User.PublicUserId);

            GenericTemplateEditControl.ResetControl();
            GenericTemplateEditControl.Display(model);
        }

        /// <summary>
        /// Display generic template based on the specific reference object ID.
        /// </summary>
        /// <param name="refobjectId">Reference object ID used to get the generic template.</param>
        public void DisplayGenericTemplate(string refobjectId)
        {
            DisplayGenericTemplate(refobjectId, null);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //init the web service for education.
            if (!AppSession.IsAdmin)
            {
                InitEducationWebService();
            }
        }

        /// <summary>
        /// Initial the country.
        /// </summary>
        /// <param name="isPostBack">if set to <c>true</c> [is post back].</param>
        /// <param name="ddlCountry">The DDL country.</param>
        /// <param name="zipText">The zip text.</param>
        /// <param name="accelaState">State of the accela.</param>
        /// <param name="phoneTexts">The phone texts.</param>
        protected void InitCountry(bool isPostBack, AccelaCountryDropDownList ddlCountry, AccelaZipText zipText, AccelaStateControl accelaState, params AccelaPhoneText[] phoneTexts)
        {
            StringBuilder sbControls = new StringBuilder();

            foreach (var phoneText in phoneTexts)
            {
                sbControls.Append(phoneText.ClientID + ",");
            }

            ddlCountry.RelevantControlIDs = sbControls.ToString();
            ddlCountry.RegisterScripts();

            // If "_isNeedInitRegional" is false, means the Regional setting has already applied in Edit form. So do not to apply again.
            if (IsNeedInitRegional)
            {
                ControlUtil.ApplyRegionalSetting(isPostBack, false, true, !isPostBack, ddlCountry);
            }
        }

        /// <summary>
        /// Register Education web service.
        /// </summary>
        private void InitEducationWebService()
        {
            ScriptManager smg = ScriptManager.GetCurrent(Page);
            smg.EnablePageMethods = true;
            ServiceReference srEducation = new ServiceReference("~/WebService/EducationService.asmx");
            smg.Services.Add(srEducation);
        }

        #endregion
    }
}