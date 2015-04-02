#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ILicenseCertificationBll.cs
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

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// License Certification
    /// </summary>
    public interface ILicenseCertificationBll
    {
        /// <summary>
        /// Gets the education by PK.
        /// </summary>
        /// <param name="educationPK">The education PK.</param>
        /// <returns>Education information</returns>
        EducationModel GetEducationModelByPK(EducationPKModel educationPK);

        /// <summary>
        /// Gets the continuing education by PK.
        /// </summary>
        /// <param name="contEduPK">The continuing education PK.</param>
        /// <returns>Continuing education information</returns>
        ContinuingEducationModel GetContEducationModelByPK(ContinuingEducationPKModel contEduPK);

        /// <summary>
        /// Get the reference contact's education list.
        /// </summary>
        /// <param name="refContactSeqNbr">The reference contact sequence number.</param>
        /// <returns>Education model array.</returns>
        EducationModel[] GetRefPeopleEduList(string refContactSeqNbr);

        /// <summary>
        /// Get the reference contact's continuing education list.
        /// </summary>
        /// <param name="refContactSeqNbr">The reference contact sequence number.</param>
        /// <returns>Continuing Education model array.</returns>
        ContinuingEducationModel[] GetRefPeopleContEduList(string refContactSeqNbr);

        /// <summary>
        /// Add or update the reference contact's education.
        /// </summary>
        /// <param name="educationModel">The Education model.</param>
        /// <returns>True: success, otherwise fail.</returns>
        bool AddOrUpdateRefPeopleEdu(EducationModel educationModel);

        /// <summary>
        /// Add the reference contact's continuing education.
        /// </summary>
        /// <param name="continuingEducationModel">The Continuing Education model.</param>
        /// <returns>True: success, otherwise fail.</returns>
        bool AddOrUpdateRefPeopleContEdu(ContinuingEducationModel continuingEducationModel);

        /// <summary>
        /// Delete the education.
        /// </summary>
        /// <param name="educationModel">The Education model.</param>
        /// <returns>True: success, otherwise fail.</returns>
        bool DeleteEducation(EducationModel educationModel);

        /// <summary>
        /// Delete the continuing education.
        /// </summary>
        /// <param name="continuingEducationModel">The Continuing Education model.</param>
        /// <returns>True: success, otherwise fail.</returns>
        bool DeleteContinuingEducation(ContinuingEducationModel continuingEducationModel);

        /// <summary>
        /// Gets the cap associate license certification.
        /// </summary>
        /// <param name="capType">Type of the cap.</param>
        /// <returns>CapAssociateLicenseCertification model</returns>
        CapAssociateLicenseCertification4WS GetCapAssociateLicenseCertification(CapTypeModel capType);
    }
}
