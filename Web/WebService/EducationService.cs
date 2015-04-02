#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: EducationService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 * Service for JS function called in client end
 *  Notes:
 * $Id: EducationService.cs 144963 2009-08-27 07:02:33Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Text;
using System.Web.Services;

using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.WebService
{
    /// <summary>
    /// This class is provider related operation for Education.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class EducationService : System.Web.Services.WebService
    {
        #region Education

        /// <summary>
        /// Initializes a new instance of the <see cref="EducationService"/> class.
        /// </summary>
        public EducationService()
        {
            if (AppSession.User == null)
            {
                throw new ACAException("unauthenticated web service invoking");
            }
        }

        /// <summary>
        /// Get ref Education names.
        /// </summary>
        /// <param name="refEducationName">ref Education name</param>
        /// <param name="providerName">provider name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>education names</returns>
        [WebMethod(Description = "Get reference education names string from web service", EnableSession = true)]
        public string GetRefEducationNames(string refEducationName, string providerName, string agencyCode, string moduleName)
        {
            CapModel4WS cap = AppSession.GetCapModelFromSession(moduleName);
            CapTypeModel capType = cap == null ? null : cap.capType;
            RefEducationModel4WS refEducation = EducationUtil.ConstructRefEducation(providerName, refEducationName, agencyCode, capType, null);

            IRefEducationBll refEducationBLL = (IRefEducationBll)ObjectFactory.GetObject(typeof(IRefEducationBll));
            RefEducationModel4WS[] educations = refEducationBLL.GetRefEducationList(refEducation);

            // if there is no education returned search by provider name and refEducation name
            // then search education again only match with education name.
            if ((educations == null || educations.Length == 0) && !string.IsNullOrEmpty(providerName))
            {
                refEducation.refEduProviderModels[0].providerModel.providerName = string.Empty;
                educations = refEducationBLL.GetRefEducationList(refEducation);
            }

            return BuildEducationNames(educations);
        }

        /// <summary>
        /// Get reference education data by invoke web service.
        /// </summary>
        /// <param name="refEducationNumber">reference education number</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>reference education data</returns>
        [WebMethod(Description = "Get reference education data from web service", EnableSession = true)]
        public string GetRefEducationData(string refEducationNumber, string moduleName, string agencyCode)
        {
            RefEducationPKModel4WS educationPKModel = new RefEducationPKModel4WS();
            educationPKModel.refEducationNbr = Convert.ToInt64(refEducationNumber);
            educationPKModel.serviceProviderCode = agencyCode;

            IRefEducationBll refEducationBLL = (IRefEducationBll)ObjectFactory.GetObject(typeof(IRefEducationBll));
            RefEducationModel4WS education = refEducationBLL.GetRefEducationByPK(educationPKModel);

            string educationInfo = string.Empty;

            if (education != null && education.refEducationName != null)
            {
                educationInfo = BuildEducationInfo(education, moduleName);
            }

            return educationInfo;
        }

        #endregion Education

        #region Continuing Education

        /// <summary>
        /// Get all Ref Continuing Education name.
        /// </summary>
        /// <param name="refContEducationName">Ref Continuing Education name</param>
        /// <param name="providerName">Provider name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>Ref Continuing Education names</returns>
        [WebMethod(Description = "Get reference continung education names string from web service", EnableSession = true)]
        public string GetRefContEducationNames(string refContEducationName, string providerName, string agencyCode, string moduleName)
        {
            CapModel4WS cap = AppSession.GetCapModelFromSession(moduleName);
            CapTypeModel capType = cap == null ? null : cap.capType;
            RefContinuingEducationModel4WS refContEducation = EducationUtil.ConstructRefContEducation(providerName, refContEducationName, agencyCode, capType, null);

            IRefContinuingEducationBll refContEducationBLL = (IRefContinuingEducationBll)ObjectFactory.GetObject(typeof(IRefContinuingEducationBll));
            RefContinuingEducationModel4WS[] refContEducations = refContEducationBLL.GetRefContEducationList(refContEducation);

            // if there is no education returned search by provider name and contEducationName name
            // then search education again only match with continuing education name.
            if ((refContEducations == null || refContEducations.Length == 0) && !string.IsNullOrEmpty(providerName))
            {
                refContEducation.refContEduProviderModels[0].providerModel.providerName = string.Empty;
                refContEducations = refContEducationBLL.GetRefContEducationList(refContEducation);
            }

            return BuildContEducationNames(refContEducations);
        }

        /// <summary>
        /// Get Ref Continuing Education information.
        /// </summary>
        /// <param name="refContEducationNumber">Ref Continuing Education number</param>
        /// <param name="refContEducationName">Ref Continuing Education name</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>Ref Continuing Education Info</returns>
        [WebMethod(Description = "Get reference continuing education info from web service", EnableSession = true)]
        public string GetRefContEducationData(string refContEducationNumber, string refContEducationName, string moduleName, string agencyCode)
        {
            RefContinuingEducationPKModel4WS refContEducationPK = new RefContinuingEducationPKModel4WS();
            refContEducationPK.refContEduNbr = Convert.ToInt64(refContEducationNumber);
            refContEducationPK.serviceProviderCode = agencyCode;

            IRefContinuingEducationBll refContEducationBll = (IRefContinuingEducationBll)ObjectFactory.GetObject(typeof(IRefContinuingEducationBll));
            RefContinuingEducationModel4WS refContEducation = refContEducationBll.GetRefContinuingEducationByPK(refContEducationPK);

            string contEducationInfo = string.Empty;

            if (refContEducation != null && refContEducation.contEduName != null && refContEducation.contEduName.Equals(refContEducationName))
            {
                contEducationInfo = BuildContEducationInfo(refContEducation, moduleName);
            }

            return contEducationInfo;
        }

        #endregion Continuing Education

        #region Examination

        /// <summary>
        /// Get Examination Names,split with '\f'
        /// </summary>
        /// <param name="examinationName">search examination name</param>
        /// <param name="providerName">search provider name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>
        /// examination names
        /// </returns>
        [WebMethod(Description = "Get reference examination names string from web service", EnableSession = true)]
        public string GetRefExaminationNames(string examinationName, string providerName, string agencyCode, string moduleName)
        {
            //Get examinations
            CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(moduleName);
            CapTypeModel capType = capModel4WS == null ? null : capModel4WS.capType;

            IRefExaminationBll refExaminationBLL = (IRefExaminationBll)ObjectFactory.GetObject(typeof(IRefExaminationBll));
            RefExaminationModel4WS[] refExaminations = refExaminationBLL.GetRefExaminationList(examinationName, providerName, agencyCode, capType);

            return BuildExaminationNames(refExaminations);
        }

        /// <summary>
        /// Get reference examination data by invoke web service.
        /// </summary>
        /// <param name="examinationNumber">reference examination number</param>
        /// <param name="examinationName">reference examination name</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>reference examination data</returns>
        [WebMethod(Description = "Get reference examination data from web service", EnableSession = true)]
        public string GetRefExaminationData(string examinationNumber, string examinationName, string moduleName, string agencyCode)
        {
            RefExaminationModel4WS examination = GetRefExamination(examinationNumber, agencyCode);

            string examinationInfo = string.Empty;

            if (examination != null && examination.examName != null && examination.examName.Equals(examinationName, StringComparison.OrdinalIgnoreCase))
            {
                string requiredValue = GetExamationRequiredStatus(examination, moduleName);
                examinationInfo = BuildExamination(examination.gradingStyle, examination.passingScore, requiredValue, examination.refExamNbr.ToString());
            }

            return examinationInfo;
        }

        /// <summary>
        /// Get examination update grade style date
        /// </summary>
        /// <param name="examNum">the examination number</param>
        /// <param name="examName">examination name</param>
        /// <param name="providerName">provider name</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>get examination update grade style</returns>
        [WebMethod(Description = "Get reference examination grade style data from web service", EnableSession = true)]
        public string GetExamUpdateGradeStyleDate(string examNum, string examName, string providerName, string moduleName, string agencyCode)
        {
            IRefExaminationBll refExaminationBLL = (IRefExaminationBll)ObjectFactory.GetObject(typeof(IRefExaminationBll));
            XRefExaminationProviderModel examinationProvider = refExaminationBLL.GetRefExamProviderModel(examName, providerName);
            string examinationInfo = string.Empty;

            if (examinationProvider != null)
            {
                RefExaminationModel4WS refExamination = null;

                if (!string.IsNullOrEmpty(examNum))
                {
                    refExamination = GetRefExamination(examNum, agencyCode);
                }

                string requiredValue = GetExamationRequiredStatus(refExamination, moduleName);

                examinationInfo = BuildExamination(
                    examinationProvider.gradingStyle,
                    examinationProvider.passingScore == null ? string.Empty : examinationProvider.passingScore.ToString(),
                    requiredValue,
                    examinationProvider.refExamNbr.ToString());
            }

            return examinationInfo;
        }

        #endregion

        #region Provider

        /// <summary>
        /// get education provider names
        /// </summary>
        /// <param name="refEducationName">education name</param>
        /// <param name="providerName">provider Name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>education provider names</returns>
        [WebMethod(Description = "Get provider names from web service", EnableSession = true)]
        public string GetEducationProviders(string refEducationName, string providerName, string agencyCode)
        {
            RefEducationModel4WS refEducation = EducationUtil.ConstructRefEducation(providerName, refEducationName, agencyCode, null, null);

            IRefEducationBll refEducationBLL = (IRefEducationBll)ObjectFactory.GetObject(typeof(IRefEducationBll));
            ProviderModel4WS[] providers = refEducationBLL.GetProviderListByRefEducation(refEducation);

            // if there is no provider returned search by provider name and refEducation name
            // then search provider again only match with provider name.
            if ((providers == null || providers.Length == 0) && !string.IsNullOrEmpty(refEducationName))
            {
                refEducation.refEducationName = string.Empty;
                providers = refEducationBLL.GetProviderListByRefEducation(refEducation);
            }

            //Bind Provider Control and Display
            return BuildProviderNames(providers);
        }

        /// <summary>
        /// Get education provider.
        /// </summary>
        /// <param name="providerName">provider name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>provider model</returns>
        [WebMethod(Description = "Get provider data from web service", EnableSession = true)]
        public string GetEducationProviderData(string providerName, string agencyCode)
        {
            RefEducationModel4WS refEducation = EducationUtil.ConstructRefEducation(providerName, string.Empty, agencyCode, null, null);

            IRefEducationBll refEducationBLL = (IRefEducationBll)ObjectFactory.GetObject(typeof(IRefEducationBll));
            ProviderModel4WS[] providers = refEducationBLL.GetProviderListByRefEducation(refEducation);

            string providerInfo = string.Empty;

            if (providers == null || providers.Length == 0)
            {
                return providerInfo;
            }

            foreach (ProviderModel4WS provider in providers)
            {
                if (provider.providerName.Equals(providerName))
                {
                    providerInfo = BuildProviderInfo(provider);
                    break;
                }
            }

            return providerInfo;
        }

        /// <summary>
        /// Get Continuing Education provider names.
        /// </summary>
        /// <param name="refContEducationName">education name</param>
        /// <param name="providerName">provider Name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>education provider names</returns>
        [WebMethod(Description = "Get provider names from web service", EnableSession = true)]
        public string GetContEducationProviders(string refContEducationName, string providerName, string agencyCode)
        {
            RefContinuingEducationModel4WS refContEducation = EducationUtil.ConstructRefContEducation(providerName, refContEducationName, agencyCode, null, null);

            IRefContinuingEducationBll refContEducationBLL = (IRefContinuingEducationBll)ObjectFactory.GetObject(typeof(IRefContinuingEducationBll));
            ProviderModel4WS[] providers = refContEducationBLL.GetProviderListByRefContEducation(refContEducation);

            // if there is no provider returned search by provider name and contEducationName name
            // then search provider again only match with provider name.
            if ((providers == null || providers.Length == 0) && !string.IsNullOrEmpty(refContEducationName))
            {
                refContEducation.contEduName = string.Empty;
                providers = refContEducationBLL.GetProviderListByRefContEducation(refContEducation);
            }

            //Bind Provider Control and Display
            return BuildProviderNames(providers);
        }

        /// <summary>
        /// Get Continuing Education Info.
        /// </summary>
        /// <param name="providerName">provider name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>provider model</returns>
        [WebMethod(Description = "Get provider data from web service", EnableSession = true)]
        public string GetContEducationProviderData(string providerName, string agencyCode)
        {
            RefContinuingEducationModel4WS refContEduationModel = EducationUtil.ConstructRefContEducation(providerName, string.Empty, agencyCode, null, null);

            IRefContinuingEducationBll refContEducationBll = (IRefContinuingEducationBll)ObjectFactory.GetObject(typeof(IRefContinuingEducationBll));
            ProviderModel4WS[] providers = refContEducationBll.GetProviderListByRefContEducation(refContEduationModel);

            string providerInfo = string.Empty;

            if (providers == null || providers.Length == 0)
            {
                return providerInfo;
            }

            foreach (ProviderModel4WS provider in providers)
            {
                if (provider.providerName.Equals(providerName))
                {
                    providerInfo = BuildProviderInfo(provider);
                    break;
                }
            }

            return providerInfo;
        }

        /// <summary>
        /// get examination provider names
        /// </summary>
        /// <param name="examinationName">examination name</param>
        /// <param name="providerName">provider Name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>examination provider names</returns>
        [WebMethod(Description = "Get provider names from web service", EnableSession = true)]
        public string GetExaminationProviders(string examinationName, string providerName, string agencyCode)
        {
            //Get providers
            IRefExaminationBll refExaminationBLL = (IRefExaminationBll)ObjectFactory.GetObject(typeof(IRefExaminationBll));
            ProviderModel4WS[] refProviders = refExaminationBLL.GetRefProviderList(examinationName, providerName, agencyCode);

            // if there is no examination returned search by provider name and refExam name
            // then search examination again only match with examination name.
            if ((refProviders == null || refProviders.Length == 0) && !string.IsNullOrEmpty(examinationName))
            {
                refProviders = refExaminationBLL.GetRefProviderList(string.Empty, providerName, agencyCode);
            }

            //Bind Provider Control and Display
            return BuildProviderNames(refProviders);
        }

        /// <summary>
        /// Get Examination Info
        /// </summary>
        /// <param name="providerName">Provider Name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>Examination Info</returns>
        [WebMethod(Description = "Get provider module from web service", EnableSession = true)]
        public string GetExaminationProviderData(string providerName, string agencyCode)
        {
            IRefExaminationBll refExaminationBLL = (IRefExaminationBll)ObjectFactory.GetObject(typeof(IRefExaminationBll));
            ProviderModel4WS[] refProviders = refExaminationBLL.GetRefProviderList(string.Empty, providerName, agencyCode);

            string providerInfo = string.Empty;

            if (refProviders == null || refProviders.Length == 0)
            {
                return providerInfo;
            }

            foreach (ProviderModel4WS provider in refProviders)
            {
                if (provider.providerName.Equals(providerName))
                {
                    providerInfo = BuildProviderInfo(provider);
                    break;
                }
            }

            return providerInfo;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Build a education Names String ,Split with '\f'
        /// </summary>
        /// <param name="educations">RefEducationModel4WS ArrayList</param>
        /// <returns>Education Names</returns>
        private string BuildEducationNames(RefEducationModel4WS[] educations)
        {
            StringBuilder sbEducations = new StringBuilder();

            if (educations != null)
            {
                // Sort by Education name.
                educations = educations.OrderBy(o => o.refEducationName).ToArray();

                foreach (RefEducationModel4WS education in educations)
                {
                    if (!string.IsNullOrEmpty(sbEducations.ToString()))
                    {
                        sbEducations.Append(ACAConstant.SPLIT_CHAR);
                    }

                    sbEducations.Append(ScriptFilter.EncodeJson(education.refEducationName));
                    sbEducations.Append(ACAConstant.SPLIT_CHAR2);
                    sbEducations.Append(ScriptFilter.EncodeJson(Convert.ToString(education.refEducationNbr)));
                }
            }

            return sbEducations.ToString();
        }

        /// <summary>
        /// Build json string for reference education information.
        /// </summary>
        /// <param name="education">reference education model</param>
        /// <param name="moduleName">the module name</param>
        /// <returns>reference education information</returns>
        private string BuildEducationInfo(RefEducationModel4WS education, string moduleName)
        {
            if (education == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder("{");
            CapUtil.AddKeyValue(sb, "RefEducationNbr", education.refEducationNbr.ToString());
            CapUtil.AddKeyValue(sb, "RefEducationName", ScriptFilter.EncodeJson(education.refEducationName));
            CapUtil.AddKeyValue(sb, "Degree", education.degree);

            string requiredValue = BasePage.GetStaticTextByKey("ACA_Common_No");

            if (education.refEduAppTypeModels != null && education.refEduAppTypeModels.Length > 0)
            {
                CapTypeModel capType = GetCapTypeModel(moduleName);

                foreach (XRefEducationAppTypeModel4WS xRefEduAppType in education.refEduAppTypeModels)
                {
                    if (xRefEduAppType.group == capType.group && xRefEduAppType.type == capType.type
                        && xRefEduAppType.subType == capType.subType && xRefEduAppType.category == capType.category)
                    {
                        requiredValue = EducationUtil.ConvertRequiredField2Display(xRefEduAppType.required);
                        break;
                    }
                }
            }

            CapUtil.AddKeyValue(sb, "Required", requiredValue, true);

            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Build a continuing education Names String ,Split with '\f'
        /// </summary>
        /// <param name="refContEducations">RefContinuingEducationModel4WS ArrayList</param>
        /// <returns>Continuing Education Names</returns>
        private string BuildContEducationNames(RefContinuingEducationModel4WS[] refContEducations)
        {
            StringBuilder sb = new StringBuilder();

            if (refContEducations != null && refContEducations.Length > 0)
            {
                // Sort by Continuing Education name.
                refContEducations = refContEducations.OrderBy(o => o.contEduName).ToArray();

                foreach (RefContinuingEducationModel4WS contEducation in refContEducations)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(ACAConstant.SPLIT_CHAR);
                    }

                    sb.Append(ScriptFilter.EncodeJson(contEducation.contEduName));
                    sb.Append(ACAConstant.SPLIT_CHAR2);
                    sb.Append(ScriptFilter.EncodeJson(Convert.ToString(contEducation.refContEduNbr)));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Build json string for ref continuing education information.
        /// </summary>
        /// <param name="refContEducation">reference education model</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>reference continuing education information</returns>
        private string BuildContEducationInfo(RefContinuingEducationModel4WS refContEducation, string moduleName)
        {
            if (refContEducation == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder("{");
            CapUtil.AddKeyValue(sb, "ConEducationNbr", refContEducation.refContEduNbr.ToString());
            CapUtil.AddKeyValue(sb, "ConEducationName", ScriptFilter.EncodeJson(refContEducation.contEduName));
            CapUtil.AddKeyValue(sb, "ContEducationGradingStyle", refContEducation.gradingStyle);
            CapUtil.AddKeyValue(sb, "PassingScore", refContEducation.passingScore);
            string requiredValue = BasePage.GetStaticTextByKey("ACA_Common_No");

            if (refContEducation.refContEduAppTypeModels != null && refContEducation.refContEduAppTypeModels.Length > 0)
            {
                CapTypeModel capType = GetCapTypeModel(moduleName);

                foreach (XRefContinuingEducationAppTypeModel4WS xRefContEduAppType in refContEducation.refContEduAppTypeModels)
                {
                    if (xRefContEduAppType.group == capType.group && xRefContEduAppType.type == capType.type
                        && xRefContEduAppType.subType == capType.subType && xRefContEduAppType.category == capType.category)
                    {
                        requiredValue = EducationUtil.ConvertRequiredField2Display(xRefContEduAppType.required);
                    }
                }
            }

            CapUtil.AddKeyValue(sb, "Required", requiredValue, true);

            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// get cap type model by model name.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <returns>Cap type model from session</returns>
        private CapTypeModel GetCapTypeModel(string moduleName)
        {
            CapTypeModel capType = new CapTypeModel();

            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            if (capModel != null && capModel.capType != null)
            {
                capType = capModel.capType;
            }

            return capType;
        }

        /// <summary>
        /// Get examination required status for current cap type.
        /// </summary>
        /// <param name="examinationModel">the examination model.</param>
        /// <param name="moduleName">Module name.</param>
        /// <returns>The examination required status - Yes/No.</returns>
        private string GetExamationRequiredStatus(RefExaminationModel4WS examinationModel, string moduleName)
        {
            string requiredValue = BasePage.GetStaticTextByKey("ACA_Common_No");

            if (examinationModel != null && examinationModel.refExamAppTypeModels != null && examinationModel.refExamAppTypeModels.Length > 0)
            {
                CapTypeModel capType = GetCapTypeModel(moduleName);

                foreach (XRefExaminationAppTypeModel4WS xRefEaxamAppType in examinationModel.refExamAppTypeModels)
                {
                    if (xRefEaxamAppType.group == capType.group && xRefEaxamAppType.type == capType.type
                        && xRefEaxamAppType.subType == capType.subType && xRefEaxamAppType.category == capType.category)
                    {
                        requiredValue = EducationUtil.ConvertRequiredField2Display(xRefEaxamAppType.required);
                        break;
                    }
                }
            }

            return requiredValue;
        }

        /// <summary>
        /// Get Ref Examination model.
        /// </summary>
        /// <param name="examinationNumber">the examination number.</param>
        /// <param name="agencyCode">the agency code</param>
        /// <returns>the ref examination model.</returns>
        private RefExaminationModel4WS GetRefExamination(string examinationNumber, string agencyCode)
        {
            RefExaminationPKModel4WS examinationPK = new RefExaminationPKModel4WS();
            examinationPK.refExamNbr = Convert.ToInt64(examinationNumber);
            examinationPK.serviceProviderCode = agencyCode;

            IRefExaminationBll refExaminationBLL = (IRefExaminationBll)ObjectFactory.GetObject(typeof(IRefExaminationBll));
            RefExaminationModel4WS examination = refExaminationBLL.GetRefExaminationByPK(examinationPK);

            return examination;
        }

        /// <summary>
        /// Build a Examination Names String ,Split with '\f'
        /// </summary>
        /// <param name="examinations">RefExaminationModel4WS ArrayList</param>
        /// <returns>Examination Names</returns>
        private string BuildExaminationNames(RefExaminationModel4WS[] examinations)
        {
            StringBuilder sbExams = new StringBuilder();

            if (examinations != null)
            {
                // Sort by provider name.
                examinations = examinations.OrderBy(o => o.examName).ToArray();

                foreach (RefExaminationModel4WS examination in examinations)
                {
                    if (!string.IsNullOrEmpty(sbExams.ToString()))
                    {
                        sbExams.Append(ACAConstant.SPLIT_CHAR);
                    }

                    sbExams.Append(ScriptFilter.EncodeJson(examination.examName));
                    sbExams.Append(ACAConstant.SPLIT_CHAR2);
                    sbExams.Append(ScriptFilter.EncodeJson(Convert.ToString(examination.refExamNbr)));
                }
            }

            return sbExams.ToString();
        }

        /// <summary>
        /// Build json string for reference examination information.
        /// </summary>
        /// <param name="gradingStyle">Grading style</param>
        /// <param name="passingScore">Passing score</param>
        /// <param name="required">Required status</param>
        /// <param name="refNbr">The ref NBR.</param>
        /// <returns>
        /// reference examination information
        /// </returns>
        private string BuildExamination(string gradingStyle, string passingScore, string required, string refNbr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            CapUtil.AddKeyValue(sb, "RefExamNbr", refNbr);
            CapUtil.AddKeyValue(sb, "ExamGradingStyle", gradingStyle);
            CapUtil.AddKeyValue(sb, "PassingScore", passingScore);
            CapUtil.AddKeyValue(sb, "Required", required, true);
            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Build json string for provider information.
        /// </summary>
        /// <param name="provider">provider model</param>
        /// <returns>provider information</returns>
        private string BuildProviderInfo(ProviderModel4WS provider)
        {
            if (provider == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder("{");
            CapUtil.AddKeyValue(sb, "ProviderName", ScriptFilter.EncodeJson(provider.providerName));
            CapUtil.AddKeyValue(sb, "ProviderNumber", provider.providerNo);

            RefLicenseProfessionalModel4WS refLP = provider.refLicenseProfessionalModel;

            if (refLP != null)
            {
                CapUtil.AddKeyValue(sb, "Address1", refLP.address1 == null ? string.Empty : ScriptFilter.EncodeJson(refLP.address1));
                CapUtil.AddKeyValue(sb, "Address2", refLP.address2 == null ? string.Empty : ScriptFilter.EncodeJson(refLP.address2));
                CapUtil.AddKeyValue(sb, "Address3", refLP.address3 == null ? string.Empty : ScriptFilter.EncodeJson(refLP.address3));
                CapUtil.AddKeyValue(sb, "City", refLP.city ?? string.Empty);
                CapUtil.AddKeyValue(sb, "State", refLP.state ?? string.Empty);
                CapUtil.AddKeyValue(sb, "Country", refLP.countyCode ?? string.Empty);
                CapUtil.AddKeyValue(sb, "Zip", ModelUIFormat.FormatZipShow(refLP.zip ?? string.Empty, refLP.countyCode));
                CapUtil.AddKeyValue(sb, "Phone1", ModelUIFormat.FormatPhone4EditPage(refLP.phone1 ?? string.Empty, refLP.countyCode));
                CapUtil.AddKeyValue(sb, "Phone1CountryCode", refLP.phone1CountryCode ?? string.Empty);
                CapUtil.AddKeyValue(sb, "Phone2", ModelUIFormat.FormatPhone4EditPage(refLP.phone1 ?? string.Empty, refLP.countyCode));
                CapUtil.AddKeyValue(sb, "Phone2CountryCode", refLP.phone2CountryCode ?? string.Empty);
                CapUtil.AddKeyValue(sb, "Fax", ModelUIFormat.FormatPhone4EditPage(refLP.fax ?? string.Empty, refLP.countyCode));
                CapUtil.AddKeyValue(sb, "FaxCountryCode", refLP.faxCountryCode ?? string.Empty);
                string email = refLP.email ?? string.Empty;
                CapUtil.AddKeyValue(sb, "Email", email, true);
            }

            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Build provider Name String 
        /// </summary>
        /// <param name="providers">ProviderModel4WS ArrayList</param>
        /// <returns>provider name String</returns>
        private string BuildProviderNames(ProviderModel4WS[] providers)
        {
            StringBuilder sbProvider = new StringBuilder();

            if (providers != null)
            {
                // Sort by provider name.
                providers = providers.OrderBy(o => o.providerName).ToArray();

                foreach (ProviderModel4WS provider in providers)
                {
                    if (sbProvider.ToString().Length > 0)
                    {
                        sbProvider.Append(ACAConstant.SPLIT_CHAR);
                    }

                    sbProvider.Append(ScriptFilter.EncodeJson(provider.providerName));
                }
            }

            return sbProvider.ToString();
        }

        #endregion Private Methods
    }
}