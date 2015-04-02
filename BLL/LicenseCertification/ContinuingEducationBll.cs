#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContinuingEducationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ContinuingEducationBll.cs 142354 2009-08-07 02:19:45Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.Education;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This class provide the ability to operation daily side continuing education.
    /// </summary>
    public class ContinuingEducationBll : BaseBll, IContinuingEducationBll
    {
        #region Method

        /// <summary>
        /// Get summary continuing education list.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <param name="contEducations">continuing education models to be used for summary.</param>
        /// <returns>continuing education summary model list.</returns>
        IList<ContinuingEducationSummary> IContinuingEducationBll.GetContEducationSummaryList(CapTypeModel capType, ContinuingEducationModel4WS[] contEducations)
        {
            List<ContinuingEducationSummary> contEducationSummaries = new List<ContinuingEducationSummary>();

            if (contEducations == null || contEducations.Length == 0)
            {
                return contEducationSummaries;
            }

            // 1. Get the required hours from reference continue educations by cap type.
            RefContinuingEducationModel4WS[] refContinuingEducations = GetRefContEducations(capType, string.Empty);

            // 2. Build a continuing education summary list according to daily education list and ref education list.
            foreach (ContinuingEducationModel4WS contEducation in contEducations)
            {
                if (contEducation == null)
                {
                    continue;
                }

                bool isPassingCE = IsPassedContEdu(contEducation.gradingStyle, contEducation.finalScore, contEducation.passingScore);
                bool isRequire = ValidationUtil.IsYes(contEducation.requiredFlag);

                if (!isPassingCE && !isRequire)
                {
                    continue;
                }

                //to find the same item by contEducation name.
                ContinuingEducationSummary existedSummary = contEducationSummaries.Find(delegate(ContinuingEducationSummary summary) { return summary.EducaitonName == contEducation.contEduName; }); 

                if (existedSummary != null)
                {
                    existedSummary.CompletedHours += isPassingCE ? contEducation.hoursCompleted : 0;
                }
                else
                {
                    //if summary list doesn't contain education name, adding this item to summary list.
                    ContinuingEducationSummary contEducationSummary = new ContinuingEducationSummary();
                    contEducationSummary.EducaitonName = contEducation.contEduName;
                    contEducationSummary.CompletedHours = isPassingCE ? contEducation.hoursCompleted : 0;
                    contEducationSummary.IsRequireCE = isRequire;
                    contEducationSummary.RequiredHours = GetRefEduRequiredHours(contEducation.contEduName, refContinuingEducations, capType);

                    contEducationSummaries.Add(contEducationSummary);
                }
            }

            return contEducationSummaries;
        }

        /// <summary>
        /// Get continue education result passing or not
        /// </summary>
        /// <param name="gradingStyle">grading style</param>
        /// <param name="finalScore">final score</param>
        /// <param name="passingScore">passing score</param>
        /// <returns>true or false</returns>
        public bool IsPassedContEdu(string gradingStyle, string finalScore, string passingScore)
        {
            if (string.IsNullOrEmpty(gradingStyle) || GradingStyle.None.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.IsNullOrEmpty(finalScore))
            {
                return false;
            }
            else if (GradingStyle.Percentage.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase) || GradingStyle.Score.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                double finalScoreNum = I18nNumberUtil.ParseNumberFromWebService(finalScore);
                double passingScoreNum = I18nNumberUtil.ParseNumberFromWebService(passingScore);
                return finalScoreNum >= passingScoreNum;
            }
            else if (GradingStyle.Passfail.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(finalScore) && ACAConstant.COMMON_ONE.Equals(finalScore))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get ref continuing educations model array.
        /// </summary>
        /// <param name="capType">cap type model.</param>
        /// <param name="required">required flag.'Y'/'N' or Empty;</param>
        /// <returns>RefContinuingEducationModel array.</returns>
        private RefContinuingEducationModel4WS[] GetRefContEducations(CapTypeModel capType, string required)
        {
            XRefContinuingEducationAppTypeModel4WS xRefContinuingEducationAppType = new XRefContinuingEducationAppTypeModel4WS();

            xRefContinuingEducationAppType.group = capType.group;
            xRefContinuingEducationAppType.type = capType.type;
            xRefContinuingEducationAppType.category = capType.category;
            xRefContinuingEducationAppType.subType = capType.subType;
            xRefContinuingEducationAppType.serviceProviderCode = capType.serviceProviderCode;

            AuditModel4WS auditModel = new AuditModel4WS();
            auditModel.auditStatus = ACAConstant.VALID_STATUS;

            xRefContinuingEducationAppType.auditModel = auditModel;

            if (!string.IsNullOrEmpty(required))
            {
                xRefContinuingEducationAppType.required = required;
            }

            RefContinuingEducationModel4WS refContEducation = new RefContinuingEducationModel4WS();
            refContEducation.serviceProviderCode = capType.serviceProviderCode;
            refContEducation.refContEduAppTypeModels = new XRefContinuingEducationAppTypeModel4WS[] { xRefContinuingEducationAppType };
            IRefContinuingEducationBll refContEducationBll = ObjectFactory.GetObject<IRefContinuingEducationBll>();
            return refContEducationBll.GetRefContEducationList(refContEducation);
        }

        /// <summary>
        /// Get refEducation required hours by education name.
        /// </summary>
        /// <param name="eduName">education name.</param>
        /// <param name="refEducations">the refEducation model list with the same cap type.</param>
        /// <param name="capType">Cap type.</param>
        /// <returns>required hours.</returns>
        private double GetRefEduRequiredHours(string eduName, IList<RefContinuingEducationModel4WS> refEducations, CapTypeModel capType)
        {
            double requiredHours = 0;

            if (refEducations == null || refEducations.Count <= 0)
            {
                return requiredHours;
            }

            foreach (RefContinuingEducationModel4WS refEdu in refEducations)
            {
                if (refEdu == null || refEdu.refContEduAppTypeModels == null || refEdu.refContEduAppTypeModels.Length <= 0)
                {
                    continue;
                }

                if (refEdu.contEduName == eduName)
                {
                    foreach (XRefContinuingEducationAppTypeModel4WS xRefContEduAppType in refEdu.refContEduAppTypeModels)
                    {
                        if (xRefContEduAppType.group == capType.group && xRefContEduAppType.type == capType.type
                            && xRefContEduAppType.subType == capType.subType && xRefContEduAppType.category == capType.category)
                        {
                            double parsedRequiredHours = 0;

                            if (I18nNumberUtil.TryParseNumberFromWebService(xRefContEduAppType.requiredHours, out parsedRequiredHours))
                            {
                                requiredHours = parsedRequiredHours;
                            }
                        }
                    }
                }
            }

            return requiredHours;
        }

        #endregion Method
    }
}
