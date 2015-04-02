#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseProfessionalService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseProfessionalService.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Linq;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.ComponentService.Model;
using Accela.ACA.CustomizeAPI;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Service
{
    /// <summary>
    /// This class provides License Professional Service for custom component
    /// </summary>
    public class LicenseProfessionalService : BaseService
    {
        /// <summary>
        /// Cap Business Class
        /// </summary>
        private ILicenseProfessionalBll licenseProfessionalBll = ObjectFactory.GetObject<ILicenseProfessionalBll>();

        /// <summary>
        /// Initializes a new instance of the LicenseProfessionalService class.
        /// </summary>
        /// <param name="context">User Context</param>
        public LicenseProfessionalService(UserContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Get License Professional List
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="capId1">id1 in CapIDModel4WS</param>
        /// <param name="capId2">id2 in CapIDModel4WS</param>
        /// <param name="capId3">id3 in CapIDModel4WS</param>
        /// <returns>LP list</returns>
        public List<LicenseProfessionalWrapperModel> GetLicenseProfessionalList(string agencyCode, string capId1, string capId2, string capId3)
        {
            List<LicenseProfessionalWrapperModel> licenseProfessionalList = new List<LicenseProfessionalWrapperModel>();

            CapIDModel4WS capID = new CapIDModel4WS { serviceProviderCode = agencyCode, id1 = capId1, id2 = capId2, id3 = capId3 };

            LicenseProfessionalModel[] licenseProfessionals = licenseProfessionalBll.GetLPListByCapID(agencyCode, capID, UserSeqNum);

            if (licenseProfessionals != null)
            {
                licenseProfessionalList.AddRange(licenseProfessionals.Select(item => new LicenseProfessionalWrapperModel(item)));
            }

            return licenseProfessionalList;
        }
    }
}
