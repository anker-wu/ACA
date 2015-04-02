#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseCertificationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 * 
 *  Notes:
 *      $Id: EducationDetail.ascx.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// License Certification
    /// </summary>
    public class LicenseCertificationBll : BaseBll, ILicenseCertificationBll
    {
        /// <summary>
        /// Gets an instance of ExaminationService.
        /// </summary>
        private LicenseCertificationWebServiceService LicenseCertificationWebService
        {
            get
            {
                return WSFactory.Instance.GetWebService<LicenseCertificationWebServiceService>();
            }
        }

        /// <summary>
        /// Gets the education by PK.
        /// </summary>
        /// <param name="educationPK">The education PK.</param>
        /// <returns>Education information</returns>
        public EducationModel GetEducationModelByPK(EducationPKModel educationPK)
        {
            return LicenseCertificationWebService.getEducationModelByPK(educationPK);
        }

        /// <summary>
        /// Gets the continuing education by PK.
        /// </summary>
        /// <param name="contEduPK">The continuing education PK.</param>
        /// <returns>Continuing education information</returns>
        public ContinuingEducationModel GetContEducationModelByPK(ContinuingEducationPKModel contEduPK)
        {
            return LicenseCertificationWebService.getContEducationModelByPK(contEduPK);
        }

        /// <summary>
        /// Get the reference contact's education list.
        /// </summary>
        /// <param name="refContactSeqNbr">The reference contact sequence number.</param>
        /// <returns>Education model array.</returns>
        public EducationModel[] GetRefPeopleEduList(string refContactSeqNbr)
        {
            if (string.IsNullOrEmpty(refContactSeqNbr))
            {
                throw new DataValidateException(new string[] { "refContactSeqNbr" });
            }

            try
            {
                return LicenseCertificationWebService.getRefPeopleEduList(AgencyCode, refContactSeqNbr);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the reference contact's continuing education list.
        /// </summary>
        /// <param name="refContactSeqNbr">The reference contact sequence number.</param>
        /// <returns>Continuing Education model array.</returns>
        public ContinuingEducationModel[] GetRefPeopleContEduList(string refContactSeqNbr)
        {
            if (string.IsNullOrEmpty(refContactSeqNbr))
            {
                throw new DataValidateException(new string[] { "refContactSeqNbr" });
            }

            try
            {
                return LicenseCertificationWebService.getRefPeopleContEduList(AgencyCode, refContactSeqNbr);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Add or update the reference contact's education.
        /// </summary>
        /// <param name="educationModel">The Education model.</param>
        /// <returns>True: success, otherwise fail.</returns>
        public bool AddOrUpdateRefPeopleEdu(EducationModel educationModel)
        {
            try
            {
                return LicenseCertificationWebService.updateRefPeopleEdu(educationModel, User.PublicUserId);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Add the reference contact's continuing education.
        /// </summary>
        /// <param name="continuingEducationModel">The Continuing Education model.</param>
        /// <returns>True: success, otherwise fail.</returns>
        public bool AddOrUpdateRefPeopleContEdu(ContinuingEducationModel continuingEducationModel)
        {
            try
            {
                return LicenseCertificationWebService.updateRefPeopleContEdu(continuingEducationModel, User.PublicUserId);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Delete the education.
        /// </summary>
        /// <param name="educationModel">The Education model.</param>
        /// <returns>True: success, otherwise fail.</returns>
        public bool DeleteEducation(EducationModel educationModel)
        {
            try
            {
                return LicenseCertificationWebService.deleteEducation(educationModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Delete the continuing education.
        /// </summary>
        /// <param name="continuingEducationModel">The Continuing Education model.</param>
        /// <returns>True: success, otherwise fail.</returns>
        public bool DeleteContinuingEducation(ContinuingEducationModel continuingEducationModel)
        {
            try
            {
                return LicenseCertificationWebService.deleteContinuingEducation(continuingEducationModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets the cap associate license certification.
        /// </summary>
        /// <param name="capType">Type of the cap.</param>
        /// <returns>CapAssociateLicenseCertification model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapAssociateLicenseCertification4WS GetCapAssociateLicenseCertification(CapTypeModel capType)
        {
            try
            {
                return LicenseCertificationWebService.getCapAssociateLicenseCertification(capType);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }
    }
}
