#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefExaminationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: RefExaminationBll.cs 140043 2009-07-21 06:09:54Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This interface provide the ability to operation reference side examination .
    /// </summary>
    public class RefExaminationBll : BaseBll, IRefExaminationBll
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of RefExaminationService.
        /// </summary>
        private RefExaminationWebServiceService RefExaminationService
        {
            get
            {
                return WSFactory.Instance.GetWebService<RefExaminationWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines whether [is beyond allowance date] [the specified examination].
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="providerNo">The provider no.</param>
        /// <param name="examStartDate">The exam start date.</param>
        /// <param name="refExamNbr">The ref exam NBR.</param>
        /// <returns><c>true</c> if [is beyond allowance date] [the specified examination]; otherwise, <c>false</c>.</returns>
        public bool IsBeyondAllowanceDate(string agencyCode, string providerNo, DateTime? examStartDate, long? refExamNbr)
        {
            DateTime dt = DateTime.Now;
            long refExamSeq = 0;

            if (examStartDate != null)
            {
                dt = examStartDate.Value;
            }

            if (refExamNbr != null)
            {
                refExamSeq = refExamNbr.Value;
            }

            return RefExaminationService.isBeyondAllowanceDate(
                agencyCode,
                providerNo,
                dt,
                examStartDate != null,
                refExamSeq,
                refExamNbr != null);
        }

        /// <summary>
        /// Get ProviderList by RefExamModel.
        /// </summary>
        /// <param name="refExaminationModel">a refExaminationModel</param>
        /// <returns>ProviderModel array</returns>
        ProviderModel4WS[] IRefExaminationBll.GetProviderListByRefExam(RefExaminationModel4WS refExaminationModel)
        {
            return RefExaminationService.getProviderListByRefExam(refExaminationModel);
        }

        /// <summary>
        /// Get refExaminationModel list by providerModel.
        /// </summary>
        /// <param name="providerModel">a providerModel</param>
        /// <param name="capTypeModel">a cap type model</param>
        /// <returns>refExaminationModel array</returns>
        RefExaminationModel4WS[] IRefExaminationBll.GetRefExaminationListByProvider(ProviderModel4WS providerModel, CapTypeModel capTypeModel)
        {
            return RefExaminationService.getRefExaminationListByProvider(providerModel, capTypeModel);
        }

        /// <summary>
        /// Get RefExamination by PK.
        /// </summary>
        /// <param name="refExaminationPKModel">a refExaminationPKModel</param>
        /// <returns>A RefExaminationModel</returns>
        RefExaminationModel4WS IRefExaminationBll.GetRefExaminationByPK(RefExaminationPKModel4WS refExaminationPKModel)
        {
            return RefExaminationService.getRefExaminationByPK(refExaminationPKModel);
        }

        /// <summary>
        /// Get Reference examination provider model
        /// </summary>
        /// <param name="examName">examination name</param>
        /// <param name="providerName">provider name</param>
        /// <returns>Examination and provider relationship information</returns>
        XRefExaminationProviderModel IRefExaminationBll.GetRefExamProviderModel(string examName, string providerName)
        {            
            return RefExaminationService.getXRefExaminationProvider(AgencyCode, examName, providerName);
        }

        /// <summary>
        /// Get RefExamination List.
        /// </summary>
        /// <param name="refExaminationModel">a refExaminationModel</param>
        /// <returns>RefExaminationModel array</returns>
        RefExaminationModel4WS[] IRefExaminationBll.GetRefExaminationList(RefExaminationModel4WS refExaminationModel)
        {
            return RefExaminationService.getRefExaminationList(refExaminationModel);
        }

        /// <summary>
        /// Get RefExamination List, and filter by work flow restriction
        /// </summary>
        /// <param name="refExaminationModel">a refExaminationModel</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="from">where are request from.</param>
        /// <returns>RefExaminationModel array</returns>
        RefExaminationModel4WS[] IRefExaminationBll.GetRefExaminationListFilterByWFRestriction(RefExaminationModel4WS refExaminationModel, CapIDModel capID, string from)
        {
            return RefExaminationService.getRefExaminationListFilterByWFRestriction(refExaminationModel, capID, from);
        }

        /// <summary>
        /// Get Ref Required Examination models of the cap type.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <returns>ref Examination model array</returns>
        RefExaminationModel4WS[] IRefExaminationBll.GetRefExaminationList(CapTypeModel capType)
        {
            RefExaminationModel4WS[] refExaminationList = null;

            if (capType != null)
            {
                //create a ref examination model
                RefExaminationModel4WS refExamination = BuildRefExaminationModel4WS(capType, ACAConstant.COMMON_Y);

                //get Examination model list by reference Examination model.
                IRefExaminationBll refExaminationBLL = ObjectFactory.GetObject<IRefExaminationBll>();
                refExaminationList = refExaminationBLL.GetRefExaminationList(refExamination);
            }

            return refExaminationList;
        }

        /// <summary>
        /// Get Ref Examination models by examinationName , providerName, service provider code
        /// </summary>
        /// <param name="examinationName">examination name</param>
        /// <param name="providerName">provider name</param>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="capType">cap type</param>
        /// <returns>ref examination model array</returns>
        RefExaminationModel4WS[] IRefExaminationBll.GetRefExaminationList(string examinationName, string providerName, string serviceProviderCode, CapTypeModel capType)
        {
            //Build a Ref Examination Model
            RefExaminationModel4WS refExamination = BuildRefExaminationModel4WS(examinationName, providerName, serviceProviderCode, capType);

            //Get Providers
            IRefExaminationBll refExaminationBLL = ObjectFactory.GetObject<IRefExaminationBll>();
            RefExaminationModel4WS[] refExaminations = refExaminationBLL.GetRefExaminationList(refExamination);

            return refExaminations;
        }

        /// <summary>
        /// Get Reference Examination name List By Name.
        /// </summary>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <param name="examName">examination Name, pass null or empty will get all examination names.</param>
        /// <returns>return key value pair of examination list. Key is sequence number, value is examination name.</returns>
        MapEntry4WS[] IRefExaminationBll.GetRefExaminationListByName(string serviceProviderCode, string examName)
        {
            return RefExaminationService.getRefExaminationListByName(serviceProviderCode, examName);
        }

        /// <summary>
        /// Get Ref provider models by examinationName , providerName.service provider code
        /// </summary>
        /// <param name="examinationName">examination name</param>
        /// <param name="providerName">provider name</param>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <returns>ref provider model array</returns>
        ProviderModel4WS[] IRefExaminationBll.GetRefProviderList(string examinationName, string providerName, string serviceProviderCode)
        {
            //Build a Ref Examination Model
            RefExaminationModel4WS refExamination = BuildRefExaminationModel4WS(examinationName, providerName, serviceProviderCode);

            //Get Providers
            IRefExaminationBll refExaminationBLL = (IRefExaminationBll)ObjectFactory.GetObject(typeof(IRefExaminationBll));
            ProviderModel4WS[] providers = refExaminationBLL.GetProviderListByRefExam(refExamination);

            return providers;
        }

        /// <summary>
        /// Build RefExaminationModel4WS Module
        /// </summary>
        /// <param name="examinationName">Examination Name</param>
        /// <param name="providerName">Provider Name</param>
        /// <param name="serviceProviderCode">Service Provider Code</param>
        /// <returns>RefExaminationModel4WS Module</returns>
        private RefExaminationModel4WS BuildRefExaminationModel4WS(string examinationName, string providerName, string serviceProviderCode)
        {
            //Build a Provider Model
            ProviderModel4WS provider = new ProviderModel4WS();
            provider.providerName = providerName;
            provider.serviceProviderCode = serviceProviderCode;

            //Build a Ref Examination Provider Model
            XRefExaminationProviderModel4WS xRefExaminationProvider = new XRefExaminationProviderModel4WS();
            xRefExaminationProvider.serviceProviderCode = serviceProviderCode;
            xRefExaminationProvider.providerModel = provider;

            //Build a Ref Examination Provider Model Array
            XRefExaminationProviderModel4WS[] refExaminationProviders = new XRefExaminationProviderModel4WS[1];
            refExaminationProviders[0] = xRefExaminationProvider;

            //Build a Ref Examination Model
            RefExaminationModel4WS refExamination = new RefExaminationModel4WS();
            refExamination.examName = examinationName;
            refExamination.refExamProviderModels = refExaminationProviders;
            refExamination.serviceProviderCode = serviceProviderCode;

            return refExamination;
        }

        /// <summary>
        /// Build RefExaminationModel4WS Module
        /// </summary>
        /// <param name="capType">CAP Type</param>
        /// <param name="isRequired">Required or Not or Null</param>
        /// <returns>RefExaminationModel4WS Module</returns>
        private RefExaminationModel4WS BuildRefExaminationModel4WS(CapTypeModel capType, string isRequired)
        {
            //create a xRef Examination App type model
            XRefExaminationAppTypeModel4WS xRefExaminationAppType = new XRefExaminationAppTypeModel4WS();
            xRefExaminationAppType.group = capType.group;
            xRefExaminationAppType.type = capType.type;
            xRefExaminationAppType.subType = capType.subType;
            xRefExaminationAppType.category = capType.category;
            if (!string.IsNullOrEmpty(isRequired))
            {
                xRefExaminationAppType.required = isRequired;
            }

            //create a xRef Examination App type model array
            XRefExaminationAppTypeModel4WS[] xRefExaminationAppTypeModels = new XRefExaminationAppTypeModel4WS[1];
            xRefExaminationAppTypeModels[0] = xRefExaminationAppType;

            //create a ref examination model
            RefExaminationModel4WS refExamination = new RefExaminationModel4WS();
            refExamination.serviceProviderCode = capType.serviceProviderCode;
            refExamination.refExamAppTypeModels = xRefExaminationAppTypeModels;

            return refExamination;
        }

        /// <summary>
        /// Build RefExaminationModel4WS Module
        /// </summary>
        /// <param name="examinationName">Examination Name</param>
        /// <param name="providerName">Provider Name</param>
        /// <param name="serviceProviderCode">Service Provider Code</param>
        /// <param name="capType">CAP Type</param>
        /// <returns>RefExaminationModel4WS Module</returns>
        private RefExaminationModel4WS BuildRefExaminationModel4WS(string examinationName, string providerName, string serviceProviderCode, CapTypeModel capType)
        {
            //Build a Ref Examination Model
            RefExaminationModel4WS refExamination = BuildRefExaminationModel4WS(examinationName, providerName, serviceProviderCode);

            if (capType != null)
            {
                refExamination.refExamAppTypeModels = BuildRefExaminationModel4WS(capType, string.Empty).refExamAppTypeModels;
            }

            return refExamination;
        }

        #endregion
    }
}
