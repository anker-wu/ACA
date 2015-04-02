#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EducationUtil.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// It providers operations for Education logic.
    /// </summary>
    public static class EducationUtil
    {
        #region Fields

        /// <summary>
        /// Indicate the default index value when no record is selected in list.
        /// </summary>
        private const int ROWINDEX_DEFAULT_VALUE = -1;

        #endregion Fields

        #region Education Methods

        /// <summary>
        /// Converts to continuing education WS model array.
        /// </summary>
        /// <param name="refContinuingEducationModels">The reference continuing education models.</param>
        /// <returns>The continuing education4WS model array </returns>
        public static ContinuingEducationModel4WS[] ConvertToContEdu4WSModelArray(RefContinuingEducationModel[] refContinuingEducationModels)
        {
            List<ContinuingEducationModel4WS> contEduModels = new List<ContinuingEducationModel4WS>();

            foreach (var refContinuingEducationModel in refContinuingEducationModels)
            {
                contEduModels.Add(ConvertToContEdu4WSModel(refContinuingEducationModel));
            }

            return contEduModels.ToArray();
        }

        /// <summary>
        /// Converts to continuing education WS model.
        /// </summary>
        /// <param name="refContinuingEducationModel">The reference continuing education model.</param>
        /// <returns>The continuing education4WS model</returns>
        public static ContinuingEducationModel4WS ConvertToContEdu4WSModel(RefContinuingEducationModel refContinuingEducationModel)
        {
            ContinuingEducationModel4WS continuingEducationModel = new ContinuingEducationModel4WS();
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();

            AuditModel4WS auditModel = new AuditModel4WS();
            auditModel.auditID = AppSession.User.PublicUserId;
            auditModel.auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);
            auditModel.auditStatus = ACAConstant.VALID_STATUS;

            string defaultCountryCode = StandardChoiceUtil.GetDefaultCountry();

            if (!string.IsNullOrEmpty(defaultCountryCode))
            {
                IBizdomainProvider bizProvider = ObjectFactory.GetObject<IBizdomainProvider>();
                IList<ItemValue> countryIDDList = bizProvider.GetBizDomainList(BizDomainConstant.STD_CAT_COUNTRY_IDD);
                var iddItem = countryIDDList.FirstOrDefault(o => defaultCountryCode.Equals(o.Key));
                string iddString = iddItem == null ? string.Empty : iddItem.Value.ToString();

                continuingEducationModel.providerDetailModel = new ProviderDetailModel4WS()
                {
                    countryCode = defaultCountryCode,
                    faxCountryCode = iddString,
                    phone1CountryCode = iddString,
                    phone2CountryCode = iddString
                };
            }

            continuingEducationModel.auditModel = auditModel;
            continuingEducationModel.contEduName = refContinuingEducationModel.contEduName;
            continuingEducationModel.continuingEducationPKModel = new ContinuingEducationPKModel4WS();
            continuingEducationModel.continuingEducationPKModel.serviceProviderCode = refContinuingEducationModel.serviceProviderCode;
            continuingEducationModel.gradingStyle = refContinuingEducationModel.gradingStyle;
            continuingEducationModel.passingScore = refContinuingEducationModel.passingScore;
            continuingEducationModel.requiredFlag = ACAConstant.COMMON_Y;
            continuingEducationModel.approvedFlag = ACAConstant.COMMON_N;
            continuingEducationModel.RefConEduNbr = refContinuingEducationModel.refContEduNbr.ToString();
            continuingEducationModel.entityType = ACAConstant.CAP_CONT_EDUCATION_ENTITY_TYPE;
            continuingEducationModel.FromCapAssociate = true;

            continuingEducationModel.template = templateBll.GetGenericTemplateStructureByEntityPKModel(
                new EntityPKModel()
                {
                    serviceProviderCode = refContinuingEducationModel.serviceProviderCode,
                    entityType = (int)GenericTemplateEntityType.ContinuingEducationDefinition,
                    seq1 = refContinuingEducationModel.refContEduNbr,
                    key1 = refContinuingEducationModel.contEduName
                },
                false,
                AppSession.User.PublicUserId);

            continuingEducationModel.entityType = ACAConstant.CAP_CONT_EDUCATION_ENTITY_TYPE;

            return continuingEducationModel;
        }

        /// <summary>
        /// Converts to education model array.
        /// </summary>
        /// <param name="refEducationModels">The reference education models.</param>
        /// <returns>The education model array.</returns>
        public static EducationModel4WS[] ConvertToEducation4WSModelArray(RefEducationModel[] refEducationModels)
        {
            List<EducationModel4WS> eucationModels = new List<EducationModel4WS>();

            foreach (var refEducationModel in refEducationModels)
            {
                eucationModels.Add(ConvertToEducation4WSModel(refEducationModel));
            }

            return eucationModels.ToArray();
        }

        /// <summary>
        /// Converts to education model.
        /// </summary>
        /// <param name="refEducationModel">The reference education model.</param>
        /// <returns>The education model.</returns>
        public static EducationModel4WS ConvertToEducation4WSModel(RefEducationModel refEducationModel)
        {
            EducationModel4WS educationModel = new EducationModel4WS();
            AuditModel4WS auditModel = new AuditModel4WS();

            auditModel.auditID = AppSession.User.PublicUserId;
            auditModel.auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);
            auditModel.auditStatus = ACAConstant.VALID_STATUS;

            educationModel.educationPKModel = new EducationPKModel4WS();
            educationModel.educationPKModel.serviceProviderCode = refEducationModel.serviceProviderCode;
            educationModel.auditModel = auditModel;
            educationModel.educationName = refEducationModel.refEducationName;
            educationModel.degree = refEducationModel.degree;
            educationModel.requiredFlag = ACAConstant.COMMON_Y;
            educationModel.approvedFlag = ACAConstant.COMMON_N;
            educationModel.RefEduNbr = refEducationModel.refEducationNbr.ToString();
            educationModel.entityType = ACAConstant.CAP_EDUCATION_ENTITY_TYPE;
            educationModel.FromCapAssociate = true;

            IBizdomainProvider bizProvider = ObjectFactory.GetObject<IBizdomainProvider>();
            IList<ItemValue> countryDefalut = bizProvider.GetBizDomainList(BizDomainConstant.STD_COUNTRY_DEFAULT_VALUE);

            string defaultCountryCode = countryDefalut != null && countryDefalut.Count > 0
                                        ? countryDefalut[0].Key
                                        : string.Empty;

            if (!string.IsNullOrEmpty(defaultCountryCode))
            {
                IList<ItemValue> countryIDDList = bizProvider.GetBizDomainList(BizDomainConstant.STD_CAT_COUNTRY_IDD);
                var iddItem = countryIDDList.FirstOrDefault(o => defaultCountryCode.Equals(o.Key));
                string iddString = iddItem == null ? string.Empty : iddItem.Value.ToString();

                educationModel.providerDetailModel = new ProviderDetailModel4WS()
                {
                    countryCode = defaultCountryCode,
                    faxCountryCode = iddString,
                    phone1CountryCode = iddString,
                    phone2CountryCode = iddString
                };
            }

            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
            educationModel.template = templateBll.GetGenericTemplateStructureByEntityPKModel(
                        new EntityPKModel()
                        {
                            serviceProviderCode = refEducationModel.serviceProviderCode,
                            entityType = (int)GenericTemplateEntityType.RefEducation,
                            seq1 = refEducationModel.refEducationNbr
                        },
                        false,
                        AppSession.User.PublicUserId);

            return educationModel;
        }

        /// <summary>
        /// Gets the education require value.
        /// </summary>
        /// <param name="education">The education.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>The education required value.</returns>
        public static string GetEducationRequireValue(RefEducationModel4WS education, string moduleName)
        {
            string requiredValue = BasePage.GetStaticTextByKey("ACA_Common_No");

            if (education.refEduAppTypeModels != null && education.refEduAppTypeModels.Length > 0)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

                if (capModel == null)
                {
                    return requiredValue;
                }

                foreach (XRefEducationAppTypeModel4WS xRefEduAppType in education.refEduAppTypeModels)
                {
                    if (xRefEduAppType.group == capModel.capType.group && xRefEduAppType.type == capModel.capType.type
                        && xRefEduAppType.subType == capModel.capType.subType && xRefEduAppType.category == capModel.capType.category)
                    {
                        requiredValue = EducationUtil.ConvertRequiredField2Display(xRefEduAppType.required);
                        break;
                    }
                }
            }

            return requiredValue;
        }

        /// <summary>
        /// Gets the continuing education require value.
        /// </summary>
        /// <param name="refContEducation">The ref contact education.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>The continuing education require value.</returns>
        public static string GetContinuingEducationRequireValue(RefContinuingEducationModel4WS refContEducation, string moduleName)
        {
            string requiredValue = BasePage.GetStaticTextByKey("ACA_Common_No");

            if (refContEducation.refContEduAppTypeModels != null && refContEducation.refContEduAppTypeModels.Length > 0)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

                if (capModel == null)
                {
                    return requiredValue;
                }

                foreach (XRefContinuingEducationAppTypeModel4WS xRefContEduAppType in refContEducation.refContEduAppTypeModels)
                {
                    if (xRefContEduAppType.group == capModel.capType.group && xRefContEduAppType.type == capModel.capType.type
                        && xRefContEduAppType.subType == capModel.capType.subType && xRefContEduAppType.category == capModel.capType.category)
                    {
                        requiredValue = EducationUtil.ConvertRequiredField2Display(xRefContEduAppType.required);
                    }
                }
            }

            return requiredValue;
        }

        /// <summary>
        /// Construct reference Education model for search.
        /// </summary>
        /// <param name="providerName">provider name</param>
        /// <param name="refEducationName">reference education name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="capType">Specific cap type.</param>
        /// <param name="isRequired">Indicate if only to get the required Educations for the specific CapType.(Y/N or null)</param>
        /// <returns>
        /// reference Education model
        /// </returns>
        public static RefEducationModel4WS ConstructRefEducation(string providerName, string refEducationName, string agencyCode, CapTypeModel capType, string isRequired)
        {
            RefEducationModel4WS refEducation = new RefEducationModel4WS();
            XRefEducationProviderModel4WS[] refEduProviders = new XRefEducationProviderModel4WS[1];
            XRefEducationProviderModel4WS xRefEducationProvider = new XRefEducationProviderModel4WS();
            ProviderModel4WS provider = new ProviderModel4WS();

            if (capType != null)
            {
                refEducation.refEduAppTypeModels = new XRefEducationAppTypeModel4WS[1];
                refEducation.refEduAppTypeModels[0] = new XRefEducationAppTypeModel4WS()
                    {
                        category = capType.category,
                        group = capType.group,
                        subType = capType.subType,
                        type = capType.type,
                        serviceProviderCode = capType.serviceProviderCode,
                        required = isRequired
                    };
            }

            provider.providerName = providerName;
            provider.serviceProviderCode = agencyCode;

            xRefEducationProvider.serviceProviderCode = agencyCode;
            xRefEducationProvider.providerModel = provider;

            refEduProviders[0] = xRefEducationProvider;
            refEducation.refEducationName = refEducationName;
            refEducation.refEduProviderModels = refEduProviders;
            refEducation.serviceProviderCode = agencyCode;

            return refEducation;
        }

        /// <summary>
        /// Construct education model.
        /// </summary>
        /// <returns>education model</returns>
        public static EducationModel4WS ConstructEducationModel()
        {
            EducationModel4WS education = new EducationModel4WS();
            ProviderDetailModel4WS providerDetail = new ProviderDetailModel4WS();
            AuditModel4WS auditModel = new AuditModel4WS();
            EducationPKModel4WS educationPKModel4WS = new EducationPKModel4WS();
            education.auditModel = auditModel;
            education.providerDetailModel = providerDetail;
            education.educationPKModel = educationPKModel4WS;

            return education;
        }

        /// <summary>
        /// Validate whether exist duplicate education.
        /// </summary>
        /// <param name="newEducation">new education record</param>
        /// <param name="educations">existing educations</param>
        /// <returns>
        /// true - Existing duplicate record in education list.
        /// false - Don't Existing duplicate record in education list.
        /// </returns>
        public static bool IsExistDuplicateEducation(EducationModel4WS newEducation, IList<EducationModel4WS> educations)
        {
            // provided that one field is empty, don't need to validate the duplicate
            bool isExsitEmptyField = true;

            if (!string.IsNullOrEmpty(newEducation.educationName) && !string.IsNullOrEmpty(newEducation.degree) &&
                !string.IsNullOrEmpty(newEducation.providerName) && !string.IsNullOrEmpty(newEducation.providerNo))
            {
                isExsitEmptyField = false;
            }

            bool isExistDuplicate = false;

            if (newEducation != null && educations != null && !isExsitEmptyField)
            {
                isExistDuplicate = ValidateDuplicateEducation(newEducation, educations);
            }

            return isExistDuplicate;
        }

        #endregion Education Methods

        #region Continuing Education Methods

        /// <summary>
        /// Construct reference continuing education model for search.
        /// </summary>
        /// <param name="providerName">provider name</param>
        /// <param name="contEducationName">continuing education name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="capType">Specific cap type.</param>
        /// <param name="isRequired">Indicate if only to get the required Continuing Educations for the specific CapType.(Y/N or null)</param>
        /// <returns>
        /// reference continuing education model
        /// </returns>
        public static RefContinuingEducationModel4WS ConstructRefContEducation(string providerName, string contEducationName, string agencyCode, CapTypeModel capType, string isRequired)
        {
            RefContinuingEducationModel4WS refContEducation = new RefContinuingEducationModel4WS();
            XRefContinuingEducationProviderModel4WS[] refContEducationProviders = new XRefContinuingEducationProviderModel4WS[1];
            XRefContinuingEducationProviderModel4WS refContEducationProvider = new XRefContinuingEducationProviderModel4WS();
            ProviderModel4WS provider = new ProviderModel4WS();

            if (capType != null)
            {
                refContEducation.refContEduAppTypeModels = new XRefContinuingEducationAppTypeModel4WS[1];
                refContEducation.refContEduAppTypeModels[0] = new XRefContinuingEducationAppTypeModel4WS()
                    {
                        category = capType.category,
                        group = capType.group,
                        subType = capType.subType,
                        type = capType.type,
                        serviceProviderCode = capType.serviceProviderCode,
                        required = isRequired
                    };
            }

            provider.providerName = providerName;
            provider.serviceProviderCode = agencyCode;

            refContEducationProvider.serviceProviderCode = agencyCode;
            refContEducationProvider.providerModel = provider;

            refContEducationProviders[0] = refContEducationProvider;
            refContEducation.contEduName = contEducationName;
            refContEducation.refContEduProviderModels = refContEducationProviders;
            refContEducation.serviceProviderCode = agencyCode;

            return refContEducation;
        }

        /// <summary>
        /// Convert ref continuing models to continuing models.
        /// </summary>
        /// <param name="refContEducations">reference side continuing education</param>
        /// <param name="requiredFlag">required flag</param>
        /// <returns>continuing education models</returns>
        public static ContinuingEducationModel4WS[] ConvertRefContEducations2ContEducations(RefContinuingEducationModel4WS[] refContEducations, string requiredFlag)
        {
            IList<ContinuingEducationModel4WS> contEducationList = new List<ContinuingEducationModel4WS>();

            if (refContEducations != null && refContEducations.Length > 0)
            {
                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                foreach (RefContinuingEducationModel4WS refContEducation in refContEducations)
                {
                    ContinuingEducationModel4WS contEducation = new ContinuingEducationModel4WS();
                    AuditModel4WS auditModel = new AuditModel4WS();
                    auditModel.auditID = AppSession.User.PublicUserId;
                    auditModel.auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);
                    auditModel.auditStatus = ACAConstant.VALID_STATUS;

                    string defaultCountryCode = StandardChoiceUtil.GetDefaultCountry();

                    if (!string.IsNullOrEmpty(defaultCountryCode))
                    {
                        IBizdomainProvider bizProvider = ObjectFactory.GetObject<IBizdomainProvider>();
                        IList<ItemValue> countryIDDList = bizProvider.GetBizDomainList(BizDomainConstant.STD_CAT_COUNTRY_IDD);
                        var iddItem = countryIDDList.FirstOrDefault(o => defaultCountryCode.Equals(o.Key));
                        string iddString = iddItem == null ? string.Empty : iddItem.Value.ToString();

                        contEducation.providerDetailModel = new ProviderDetailModel4WS()
                        {
                            countryCode = defaultCountryCode,
                            faxCountryCode = iddString,
                            phone1CountryCode = iddString,
                            phone2CountryCode = iddString
                        };
                    }

                    contEducation.auditModel = auditModel;
                    contEducation.contEduName = refContEducation.contEduName;
                    contEducation.continuingEducationPKModel = new ContinuingEducationPKModel4WS();
                    contEducation.continuingEducationPKModel.serviceProviderCode = refContEducation.serviceProviderCode;
                    contEducation.gradingStyle = refContEducation.gradingStyle;
                    contEducation.passingScore = refContEducation.passingScore;
                    contEducation.requiredFlag = requiredFlag;
                    contEducation.approvedFlag = ACAConstant.COMMON_N;
                    contEducation.RefConEduNbr = refContEducation.refContEduNbr.ToString(); 
                    contEducation.entityType = ACAConstant.CAP_CONT_EDUCATION_ENTITY_TYPE;
                    contEducation.FromCapAssociate = true;

                    contEducation.template = templateBll.GetGenericTemplateStructureByEntityPKModel(
                        new EntityPKModel()
                        {
                            serviceProviderCode = refContEducation.serviceProviderCode,
                            entityType = (int)GenericTemplateEntityType.ContinuingEducationDefinition,
                            seq1 = refContEducation.refContEduNbr, 
                            key1 = refContEducation.contEduName
                        },
                        false,
                        AppSession.User.PublicUserId);

                    contEducation.entityType = ACAConstant.CAP_CONT_EDUCATION_ENTITY_TYPE;
                    contEducationList.Add(contEducation);
                }
            }

            ContinuingEducationModel4WS[] contEducations = new ContinuingEducationModel4WS[contEducationList.Count];
            contEducationList.CopyTo(contEducations, 0);

            return contEducations;
        }

        /// <summary>
        /// Add RowIndex Property
        /// </summary>
        /// <param name="contEducationList">Continuing Education List</param>
        /// <returns>Continuing Education List Collection</returns>
        public static IList<ContinuingEducationModel4WS> AddRowIndex2ContEducationModel(IList<ContinuingEducationModel4WS> contEducationList)
        {
            IList<ContinuingEducationModel4WS> addIndexcontEducationList = contEducationList;

            if (addIndexcontEducationList != null && addIndexcontEducationList.Count > 0)
            {
                for (int i = 0; i < addIndexcontEducationList.Count; i++)
                {
                    addIndexcontEducationList[i].RowIndex = i;
                }
            }

            return addIndexcontEducationList;
        }

        /// <summary>
        /// Adds the row index to education model.
        /// </summary>
        /// <param name="educationList">The education list.</param>
        /// <returns>The education model list that add the row index.</returns>
        public static IList<EducationModel4WS> AddRowIndex2EducationModel(IList<EducationModel4WS> educationList)
        {
            IList<EducationModel4WS> addIndexEducationList = educationList;

            if (addIndexEducationList != null && addIndexEducationList.Count > 0)
            {
                for (int i = 0; i < addIndexEducationList.Count; i++)
                {
                    addIndexEducationList[i].RowIndex = i;
                }
            }

            return addIndexEducationList;
        }

        /// <summary>
        /// validate duplicate record for continuing education.
        /// </summary>
        /// <param name="contEducation">continuing education to be used for comparing whether current education has existed in list.</param>
        /// <param name="contEducations">continuing education models.</param>
        /// <returns>
        /// true - Existing duplicate record in continuing education list.
        /// false - Don't Existing duplicate record in continuing education list.
        /// </returns>
        public static bool ExistDuplicateContEducation(ContinuingEducationModel4WS contEducation, IList<ContinuingEducationModel4WS> contEducations)
        {
            // provided that one field is empty, don't need to validate the duplicate
            bool isExsitEmptyField = true;

            if (!string.IsNullOrEmpty(contEducation.contEduName) && !string.IsNullOrEmpty(contEducation.className) &&
                !string.IsNullOrEmpty(contEducation.dateOfClass) && !string.IsNullOrEmpty(contEducation.providerName) &&
                !string.IsNullOrEmpty(contEducation.providerNo))
            {
                isExsitEmptyField = false;
            }

            bool isExistDuplicate = false;

            if (contEducation != null && contEducations != null && !isExsitEmptyField)
            {
                isExistDuplicate = ValidateDuplicate4ContEducation(contEducation, contEducations);
            }

            return isExistDuplicate;
        }

        #endregion Continuing Education Methods

        /// <summary>
        /// Convert required DB value to display value.
        /// </summary>
        /// <param name="requiredvalue">required DB value</param>
        /// <returns>required display value</returns>
        public static string ConvertRequiredField2Display(string requiredvalue)
        {
            string required = string.Empty;

            if (ACAConstant.COMMON_Y.Equals(requiredvalue, StringComparison.InvariantCultureIgnoreCase))
            {
                required = BasePage.GetStaticTextByKey("ACA_Common_Yes");
            }
            else if (ACAConstant.COMMON_N.Equals(requiredvalue, StringComparison.InvariantCultureIgnoreCase))
            {
                required = BasePage.GetStaticTextByKey("ACA_Common_No");
            }

            return required;
        }

        /// <summary>
        /// Convert required display value to save.
        /// </summary>
        /// <param name="requiredvalue">required value.</param>
        /// <returns>required DB value.</returns>
        public static string ConvertRequiredFeild2Save(string requiredvalue)
        {
            string required = string.Empty;

            if (requiredvalue.Equals(BasePage.GetStaticTextByKey("ACA_Common_Yes"), StringComparison.InvariantCultureIgnoreCase))
            {
                required = ACAConstant.COMMON_Y;
            }
            else
            {
                required = ACAConstant.COMMON_N;
            }

            return required;
        }

        /// <summary>
        /// Format display score string.
        /// </summary>
        /// <param name="gradingStyle">score grading style.</param>
        /// <param name="score">the score number.</param>
        /// <param name="isPassingScoreField">is passing score field.</param>
        /// <returns>
        /// format string for score.
        /// </returns>
        public static string FormatScore(string gradingStyle, string score, bool isPassingScoreField = false)
        {
            string formatScoreString = string.Empty;

            if (string.IsNullOrEmpty(gradingStyle) || string.IsNullOrEmpty(score))
            {
                if (isPassingScoreField && GradingStyle.Passfail.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(score))
                {
                    formatScoreString = LabelUtil.GetGlobalTextByKey("aca_common_pass");
                }
                else
                {
                    formatScoreString = score;
                }

                return formatScoreString;
            }

            if (GradingStyle.Percentage.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                formatScoreString = I18nNumberUtil.ConvertNumberStringFromWebServiceToUI(score) + ACAConstant.COMMA_PERCENT;
            }
            else if (GradingStyle.Score.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase)
                || GradingStyle.None.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                formatScoreString = I18nNumberUtil.ConvertNumberStringFromWebServiceToUI(score);
            }
            else if (GradingStyle.Passfail.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                if (ACAConstant.COMMON_ONE.Equals(score))
                {
                    formatScoreString = LabelUtil.GetGlobalTextByKey("aca_common_pass");
                }
                else if (ACAConstant.COMMON_ZERO.Equals(score))
                {
                    formatScoreString = LabelUtil.GetGlobalTextByKey("aca_common_fail");
                }
            }

            return formatScoreString;
        }

        /// <summary>
        /// Format display score string.
        /// </summary>
        /// <param name="gradingStyle">score grading style.</param>
        /// <returns>format string for grading style.</returns>
        public static string FormatGradingStyle(string gradingStyle)
        {
            string formatGrading = gradingStyle;

            string labelKey = string.Empty;

            if (GradingStyle.Passfail.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                labelKey = "per_refcoutinuingeducationlist_passfail";
            }
            else if (GradingStyle.Score.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                labelKey = "per_refcoutinuingeducationlist_passing_score";
            }
            else if (GradingStyle.Percentage.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                labelKey = "per_refcoutinuingeducationlist_passing_percent";
            }
            else if (GradingStyle.None.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                labelKey = "aca_common_none";
            }
            else if (GradingStyle.NA.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                labelKey = "aca_common_na";
            }

            if (!string.IsNullOrEmpty(labelKey))
            {
                formatGrading = LabelUtil.GetGlobalTextByKey(labelKey);
            }

            return formatGrading;
        }

        /// <summary>
        /// Determines whether current user is provider license user.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if current user has provider licenses; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasProviderLicense()
        {
            if (AppSession.User == null || AppSession.User.IsAnonymous)
            {
                return false;
            }

            bool hasLicense = false;

            // HasProviderLicense is Nullable bool value
            if (!AppSession.User.HasProviderLicense.HasValue)
            {
                IProviderBll providerBll = (IProviderBll)ObjectFactory.GetObject(typeof(IProviderBll));
                ProviderModel4WS[] providermodels = providerBll.GetProviderListByUserSeqNbr(ConfigManager.AgencyCode, AppSession.User.UserSeqNum);

                if (providermodels != null && providermodels.Length > 0)
                {
                    AppSession.User.HasProviderLicense = true;
                    hasLicense = true;
                }
                else
                {
                    AppSession.User.HasProviderLicense = false;
                }
            }
            else
            {
                hasLicense = AppSession.User.HasProviderLicense == true ? true : false;
            }

            return hasLicense;
        }

        /// <summary>
        /// Gets the ref education model.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="educationName">Name of the education.</param>
        /// <returns>The ref education model.</returns>
        public static RefEducationModel4WS GetRefEducationModel(string agencyCode, string educationName)
        {
            RefEducationModel4WS result = null;

            IRefEducationBll educationBll = ObjectFactory.GetObject<IRefEducationBll>();
            RefEducationModel4WS[] refEducations =
                educationBll.GetRefEducationList(new RefEducationModel4WS()
                {
                    serviceProviderCode = agencyCode,
                    refEducationName = educationName,
                });

            if (refEducations != null && refEducations.Length > 0)
            {
                result =
                    refEducations.FirstOrDefault(
                        o =>
                        educationName.Equals(o.refEducationName, StringComparison.InvariantCultureIgnoreCase));
            }

            return result;
        }

        /// <summary>
        /// Gets the ref continuing education model.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="contEduName">Name of the contact EDU.</param>
        /// <returns>The ref continuing education model.</returns>
        public static RefContinuingEducationModel4WS GetRefContinuingEducationModel(string agencyCode, string contEduName)
        {
            RefContinuingEducationModel4WS result = null;

            if (!string.IsNullOrWhiteSpace(contEduName))
            {
                IRefContinuingEducationBll contEducationBll = ObjectFactory.GetObject<IRefContinuingEducationBll>();
                RefContinuingEducationModel4WS[] refEducations =
                    contEducationBll.GetRefContEducationList(new RefContinuingEducationModel4WS()
                    {
                        serviceProviderCode = agencyCode,
                        contEduName = contEduName
                    });

                if (refEducations != null && refEducations.Length > 0)
                {
                    result =
                        refEducations.FirstOrDefault(
                            o => contEduName.Equals(o.contEduName, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return result;
        }

        /// <summary>
        /// Validate duplicate record for continuing education.
        /// </summary>
        /// <param name="contEducation">continuing education model</param>
        /// <param name="contEducations">continuing education models</param>
        /// <returns>
        /// true - Existing duplicate record in continuing education list.
        /// false - Don't Existing duplicate record in continuing education list.
        /// </returns>
        private static bool ValidateDuplicate4ContEducation(ContinuingEducationModel4WS contEducation, IList<ContinuingEducationModel4WS> contEducations)
        {
            bool isExistDuplicate = false;

            foreach (ContinuingEducationModel4WS contEducationModel in contEducations)
            {
                // Edit record need check whether is compare itself. 
                if (contEducation.RowIndex != ROWINDEX_DEFAULT_VALUE)
                {
                    if (contEducation.RowIndex == contEducationModel.RowIndex)
                    {
                        continue;
                    }
                }

                if (contEducation.contEduName.Equals(contEducationModel.contEduName) && contEducation.className.Equals(contEducationModel.className) &&
                    contEducation.dateOfClass.Equals(contEducationModel.dateOfClass) && contEducation.providerName.Equals(contEducationModel.providerName) &&
                    contEducation.providerNo.Equals(contEducationModel.providerNo))
                {
                    isExistDuplicate = true;

                    break;
                }
            }

            return isExistDuplicate;
        }

        /// <summary>
        /// Validate whether exist duplicate education.
        /// </summary>
        /// <param name="newEducation">new education record</param>
        /// <param name="educations">existing educations</param>
        /// <returns>
        /// true - Existing duplicate record in education list.
        /// false - Don't Existing duplicate record in education list.
        /// </returns>
        private static bool ValidateDuplicateEducation(EducationModel4WS newEducation, IList<EducationModel4WS> educations)
        {
            bool isExistDuplicate = false;

            foreach (EducationModel4WS existEducation in educations)
            {
                // Edit record need check whether is compare itself. 
                if (newEducation.RowIndex != ROWINDEX_DEFAULT_VALUE)
                {
                    if (newEducation.RowIndex == existEducation.RowIndex)
                    {
                        continue;
                    }
                }

                if (newEducation.educationName.Equals(existEducation.educationName) && newEducation.degree.Equals(existEducation.degree) &&
                    newEducation.providerName.Equals(existEducation.providerName) && newEducation.providerNo.Equals(existEducation.providerNo))
                {
                    isExistDuplicate = true;

                    break;
                }
            }

            return isExistDuplicate;
        }
    }
}
