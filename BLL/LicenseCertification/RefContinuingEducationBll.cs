#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefContinuingEducationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: RefContinuingEducationBll.cs 140043 2009-07-21 06:09:54Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This interface provide the ability to operation reference side continuing education.
    /// </summary>
    public class RefContinuingEducationBll : BaseBll, IRefContinuingEducationBll
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of RefContinuingEducationService.
        /// </summary>
        private RefContinuingEducationWebServiceService RefContinuingEducationService
        {
            get
            {
                return WSFactory.Instance.GetWebService<RefContinuingEducationWebServiceService>();
            }
        }

        #endregion Properties

        /// <summary>
        /// Get refContinuingEducation by PK.
        /// </summary>
        /// <param name="refContinuingEducationPKModel">a refContinuingEducationPKModel</param>
        /// <returns>a RefContinuingEducationModel</returns>
        RefContinuingEducationModel4WS IRefContinuingEducationBll.GetRefContinuingEducationByPK(RefContinuingEducationPKModel4WS refContinuingEducationPKModel)
        {
            return RefContinuingEducationService.getRefContEducationByPK(refContinuingEducationPKModel);
        }

        /// <summary>
        /// Get Continuing Education model list by provider.
        /// </summary>
        /// <param name="providerModel">a provider information</param>
        /// <param name="capTypeModel">a cap type model</param>
        /// <returns>Continuing Education array</returns>
        RefContinuingEducationModel4WS[] IRefContinuingEducationBll.GetRefContEducationListByProvider(ProviderModel4WS providerModel, CapTypeModel capTypeModel)
        {
            return RefContinuingEducationService.getRefContEducationListByProvider(providerModel, capTypeModel);
        }

        /// <summary>
        /// Get ProviderList by Continuing Education.
        /// </summary>
        /// <param name="refContinuingEducationModel">a Continuing Education</param>
        /// <returns>Provider information array</returns>
        ProviderModel4WS[] IRefContinuingEducationBll.GetProviderListByRefContEducation(RefContinuingEducationModel4WS refContinuingEducationModel)
        {
            return RefContinuingEducationService.getProviderListByRefContEducation(refContinuingEducationModel);
        }

        /// <summary>
        /// Get Continuing Education List.
        /// </summary>
        /// <param name="refContinuingEducationModel">a Continuing Education</param>
        /// <returns>Continuing Education array</returns>
        RefContinuingEducationModel4WS[] IRefContinuingEducationBll.GetRefContEducationList(RefContinuingEducationModel4WS refContinuingEducationModel)
        {
            return RefContinuingEducationService.getRefContEducationList(refContinuingEducationModel);
        }

        /// <summary>
        /// Get Ref continuing education models by cap type.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <returns>ref continuing education models</returns>
        RefContinuingEducationModel4WS[] IRefContinuingEducationBll.GetRefContEducationsByCapType(CapTypeModel capType)
        {
            if (capType == null)
            {
                return null;
            }

            RefContinuingEducationModel4WS refContEducation = new RefContinuingEducationModel4WS();
            XRefContinuingEducationAppTypeModel4WS xRefContEducationAppType = new XRefContinuingEducationAppTypeModel4WS();

            xRefContEducationAppType.group = capType.group;
            xRefContEducationAppType.type = capType.type;
            xRefContEducationAppType.subType = capType.subType;
            xRefContEducationAppType.category = capType.category;
            xRefContEducationAppType.required = ACAConstant.COMMON_Y;

            XRefContinuingEducationAppTypeModel4WS[] contEducationAppTypeModels = new XRefContinuingEducationAppTypeModel4WS[1];
            contEducationAppTypeModels[0] = xRefContEducationAppType;

            refContEducation.serviceProviderCode = capType.serviceProviderCode;
            refContEducation.refContEduAppTypeModels = contEducationAppTypeModels;

            // get continuing education models by reference continuing education model.
            IRefContinuingEducationBll refContEducationBll = (IRefContinuingEducationBll)ObjectFactory.GetObject(typeof(IRefContinuingEducationBll));
            RefContinuingEducationModel4WS[] refContEducations = refContEducationBll.GetRefContEducationList(refContEducation);

            return refContEducations;
        }

        /// <summary>
        /// Get Reference continuing education name list By Name
        /// </summary>
        /// <param name="agencyCode">service Provider Code</param>
        /// <param name="contEduName">continuing education Name, pass null or empty will get all continuing education names.</param>
        /// <returns>return key value pair of continuing education list. Key is sequence number, value is continuing education name.</returns>
        MapEntry4WS[] IRefContinuingEducationBll.GetRefContEducationListByName(string agencyCode, string contEduName)
        {
            return RefContinuingEducationService.getRefContinuingEducationByName(agencyCode, contEduName);
        }
    }
}
