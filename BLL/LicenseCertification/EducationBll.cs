#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *  Notes:
 * $Id: EducationBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This class provide the ability to operation daily side Education.
    /// </summary>
    public class EducationBll : BaseBll, IEducationBll
    {
        #region Fields

        #endregion Fields

        #region Method

        /// <summary>
        /// Add child model to education model if child model is null.
        /// </summary>
        /// <param name="educations">education models</param>
        void IEducationBll.ContructChildModel2Education(EducationModel4WS[] educations)
        {
            if (educations == null || educations.Length == 0)
            {
                return;
            }

            foreach (EducationModel4WS education in educations)
            {
                if (education.auditModel == null)
                {
                    AuditModel4WS auditModel = new AuditModel4WS();
                    education.auditModel = auditModel;
                }

                if (education.providerDetailModel == null)
                {
                    ProviderDetailModel4WS providerDetail = new ProviderDetailModel4WS();
                    education.providerDetailModel = providerDetail;
                }

                if (education.educationPKModel == null)
                {
                    education.educationPKModel = new EducationPKModel4WS();
                }
            }
        }

        /// <summary>
        /// Get Ref education models by cap type.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <returns>ref education model array</returns>
        RefEducationModel4WS[] IEducationBll.GetRefEducationModelsByCapType(CapTypeModel capType)
        {
            if (capType == null)
            {
                return null;
            }

            RefEducationModel4WS refEducation = new RefEducationModel4WS();
            XRefEducationAppTypeModel4WS xRefEducationAppType = new XRefEducationAppTypeModel4WS();

            xRefEducationAppType.group = capType.group;
            xRefEducationAppType.type = capType.type;
            xRefEducationAppType.subType = capType.subType;
            xRefEducationAppType.category = capType.category;
            xRefEducationAppType.required = ACAConstant.COMMON_Y;

            XRefEducationAppTypeModel4WS[] eduAppTypeModels = new XRefEducationAppTypeModel4WS[1];
            eduAppTypeModels[0] = xRefEducationAppType;

            refEducation.serviceProviderCode = capType.serviceProviderCode;
            refEducation.refEduAppTypeModels = eduAppTypeModels;

            //get Education model list by reference Education model.
            IRefEducationBll refEducationBLL = ObjectFactory.GetObject<IRefEducationBll>();
            RefEducationModel4WS[] refEducationList = refEducationBLL.GetRefEducationList(refEducation);

            return refEducationList;
        }

        /// <summary>
        /// Convert RefEducation model array to Education model array.
        /// </summary>
        /// <param name="refEducations">ref education model array</param>
        /// <param name="callId">The call id.</param>
        /// <returns>
        /// education model array
        /// </returns>
        EducationModel4WS[] IEducationBll.ConvertRefEducations2Educations(RefEducationModel4WS[] refEducations, string callId)
        {
            IList<EducationModel4WS> educationList = new List<EducationModel4WS>();

            if (refEducations != null && refEducations.Length > 0)
            {
                foreach (RefEducationModel4WS refEducation in refEducations)
                {
                    educationList.Add(ConvertRefEducation2Education(refEducation, callId));
                }
            }

            EducationModel4WS[] educations = new EducationModel4WS[educationList.Count];
            educationList.CopyTo(educations, 0);

            return educations;
        }

        /// <summary>
        /// Convert RefEducation model to Education model.
        /// </summary>
        /// <param name="refEducation">ref education model</param>
        /// <param name="callId">The call id.</param>
        /// <returns>
        /// education model
        /// </returns>
        private EducationModel4WS ConvertRefEducation2Education(RefEducationModel4WS refEducation, string callId)
        {
            EducationModel4WS education = new EducationModel4WS();
            AuditModel4WS auditModel = new AuditModel4WS();

            auditModel.auditID = callId;
            auditModel.auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);
            auditModel.auditStatus = ACAConstant.VALID_STATUS;

            education.educationPKModel = new EducationPKModel4WS();
            education.educationPKModel.serviceProviderCode = refEducation.serviceProviderCode;
            education.auditModel = auditModel;
            education.educationName = refEducation.refEducationName;
            education.degree = refEducation.degree;
            education.requiredFlag = ACAConstant.COMMON_Y;
            education.approvedFlag = ACAConstant.COMMON_N;
            education.RefEduNbr = refEducation.refEducationNbr.ToString();
            education.entityType = ACAConstant.CAP_EDUCATION_ENTITY_TYPE;
            education.FromCapAssociate = true;

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

                education.providerDetailModel = new ProviderDetailModel4WS()
                {
                    countryCode = defaultCountryCode,
                    faxCountryCode = iddString,
                    phone1CountryCode = iddString,
                    phone2CountryCode = iddString
                };
            }

            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
            education.template =
                templateBll.GetGenericTemplateStructureByEntityPKModel(
                    new EntityPKModel()
                    {
                        serviceProviderCode = refEducation.serviceProviderCode,
                        entityType = (int)GenericTemplateEntityType.RefEducation,
                        seq1 = refEducation.refEducationNbr
                    },
                    false,
                    callId);

            return education;
        }

        #endregion Method
    }
}
