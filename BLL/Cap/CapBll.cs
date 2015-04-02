#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CapBll.cs 278182 2014-08-28 10:13:16Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using log4net;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// This class provide the ability to operation CAP.
    /// </summary>
    public class CapBll : BaseBll, ICapBll
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CapBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of AppStatusGroupService.
        /// </summary>
        private AppStatusGroupWebServiceService AppStatusGroupService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AppStatusGroupWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of CapService.
        /// </summary>
        private CapWebServiceService CapService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CapWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a regular CAP based on the Partial Cap
        /// </summary>
        /// <param name="spCode">Agency Code</param>
        /// <param name="capModel4WS">Partial Cap</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order sequence</param>
        /// <returns>Cap information</returns>
        public CapModel4WS CreateCapModelFromFeeEstimate(string spCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq)
        {
            return CreateCapModelFromFeeEstimate(spCode, capModel4WS, publicUserId, wotModelSeq, false);
        }

        /// <summary>
        /// Creates a regular CAP based on the Partial Cap
        /// </summary>
        /// <param name="spCode">Agency Code</param>
        /// <param name="capModel4WS">Partial Cap</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order sequence</param>
        /// <param name="isForDeferPayment">For Defer Payment</param>
        /// <returns>Cap information</returns>
        public CapModel4WS CreateCapModelFromFeeEstimate(string spCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq, bool isForDeferPayment)
        {
            return CreateCapModelFromFeeEstimate(spCode, capModel4WS, publicUserId, wotModelSeq, isForDeferPayment, false);
        }

        /// <summary>
        /// Creates a regular CAP based on the Partial Cap
        /// </summary>
        /// <param name="spCode">Agency Code</param>
        /// <param name="capModel4WS">Partial Cap</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order template</param>
        /// <param name="isForDeferPayment">For Defer Payment</param>
        /// <param name="isAmendment">is amendment</param>
        /// <returns>Cap information</returns>
        /// <exception cref="DataValidateException">{ <c>capModel4WS, capModel4WS.capID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS CreateCapModelFromFeeEstimate(string spCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq, bool isForDeferPayment, bool isAmendment)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.createCapModelFromFeeEstimate()");
            }

            if (capModel4WS == null || capModel4WS.capID == null)
            {
                throw new DataValidateException(new string[] { "capModel4WS", "capModel4WS.capID" });
            }

            try
            {
                CapModel4WS newCap = null;

                newCap = CapService.createCapModelFromFeeEstimate(spCode, capModel4WS, publicUserId, wotModelSeq, false, isForDeferPayment, isAmendment);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.createCapModelFromFeeEstimate()");
                }

                return newCap;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Creates regular sub Caps based on the Partial Caps
        /// </summary>
        /// <param name="capModel4WS">The parent cap</param>
        /// <returns>Children Simple Cap  Models that has created successfully</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public OnlinePaymentResultModel CreateCapModelsFromFeeEstimate(CapModel4WS capModel4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.CreateCapModelsFromFeeEstimate()");
            }

            try
            {
                OnlinePaymentResultModel onlinePaymentResult = CapService.createCapModelsFromFeeEstimate(AgencyCode, capModel4WS, User.PublicUserId, string.Empty);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.CreateCapModelsFromFeeEstimate()");
                }

                return onlinePaymentResult;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method for renewable licenses and permit.
        /// First time access the method to Create renewal partial cap as child cap of parent licenses cap to renewal processing.
        /// Second time get the renewal partial cap to continue this renewal process.
        /// </summary>
        /// <param name="parentCapID">Cap ID Model</param>
        /// <returns>Cap information</returns>
        /// <exception cref="DataValidateException">{ <c>capModel4WS, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS CreateOrGetRenewalPartialCap(CapIDModel4WS parentCapID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.CreateOrGetRenewalPartialCap()");
            }

            if (parentCapID == null)
            {
                throw new DataValidateException(new string[] { "capModel4WS", "callerID" });
            }

            try
            {
                CapModel4WS capModel = CapService.createOrGetRenewalPartialCap(parentCapID, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.CreateOrGetRenewalPartialCap()");
                }

                return capModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Create partial caps for super agency
        /// </summary>
        /// <param name="capModel4WS">CapModel4WS entity</param>
        /// <param name="services">user selected services</param>
        /// <param name="isFeeEstimates">is fee estimates</param>
        /// <returns>The parent cap information.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS CreatePartialCaps(CapModel4WS capModel4WS, ServiceModel[] services, bool isFeeEstimates)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.CreatePartialCaps()");
            }

            try
            {
                CapModel4WS capModel = CapService.createPartialCaps(AgencyCode, capModel4WS, User.PublicUserId, services, string.Empty, isFeeEstimates);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.CreatePartialCaps()");
                }

                return capModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to crate a partial cap.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="capModel4WS">cap model with applicant,application specific info and work location</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <param name="wotModelSeq">work order ID</param>
        /// <param name="isFeeEstimates">whether fee estimate</param>
        /// <returns>Cap information</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, capModel4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS CreateWrapperForPartialCap(string serviceProviderCode, CapModel4WS capModel4WS, string publicUserId, string wotModelSeq, bool isFeeEstimates)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.createWrapperForPartialCap()");
            }

            if (string.IsNullOrEmpty(serviceProviderCode) || capModel4WS == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "capModel4WS" });
            }

            try
            {
                capModel4WS.createdByACA = ACAConstant.COMMON_Y;

                CapModel4WS response = CapService.createWrapperForPartialCap(serviceProviderCode, capModel4WS, publicUserId, wotModelSeq, isFeeEstimates);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.createWrapperForPartialCap()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get application status model.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <returns>application status array</returns>
        /// <exception cref="DataValidateException"> agencyCode is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public AppStatusGroupModel4WS[] GetAppStatusGroupBySPC(string agencyCode, string moduleName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AppStatusGroupService.GetAppStatusGroupBySPC()");
            }

            if (string.IsNullOrEmpty(agencyCode))
            {
                throw new DataValidateException(new string[] { agencyCode });
            }

            try
            {
                AppStatusGroupModel4WS[] treeNodeModel = AppStatusGroupService.getAppStatusGroupBySPC(agencyCode, moduleName);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AppStatusGroupService.GetSubTreeNode()");
                }

                return treeNodeModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the simple Cap associate to a trade name
        /// </summary>
        /// <param name="capIDModule">CapIDModel4WS entity</param>
        /// <returns>cap information</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS GetCapByAltID(CapIDModel4WS capIDModule)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetCapByAltID()");
            }

            try
            {
                CapModel4WS capModel = CapService.getCapByAltID(capIDModule);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.GetCapByAltID()");
                }

                return capModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get cap model(cap simple information - only b1permit data) by cap ID. if the cap doesn't exist, return null.
        /// </summary>
        /// <param name="capID4WS">capID Model identify a cap..</param>
        /// <returns>cap information</returns>
        public CapModel4WS GetCapByPK(CapIDModel4WS capID4WS)
        {
            CapModel4WS capModel4WS = null;
            string callerID = this.User == null ? string.Empty : this.User.PublicUserId;

            // Add try/catch to avoid ObjectNotFoundException, when the Partial cap convert to real cap in V360
            try
            {
                // if the cap cannot be found, will throw the excpetion
                capModel4WS = CapService.getCapByPK(capID4WS, callerID);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return capModel4WS;
        }

        /// <summary>
        /// Get Renew Cap's Child Cap..
        /// </summary>
        /// <param name="capID4ws">cap ID model</param>
        /// <param name="relationship">Renewal (ACAConstant.CAP_RENEWAL)</param>
        /// <param name="capStatus">Incomplete (ACAConstant.RENEWAL_INCOMPLETE)</param>
        /// <returns>Renew Child Cap.</returns>
        /// <exception cref="DataValidateException">{ <c>capID4ws</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS GetCapByRelationshipPartialCap(CapIDModel4WS capID4ws, string relationship, string capStatus)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetCapByRelationshipPartialCap()");
            }

            if (capID4ws == null)
            {
                throw new DataValidateException(new string[] { "capID4ws" });
            }

            try
            {
                CapModel4WS capModel = CapService.getCapByRelationshipPartialCap(capID4ws, relationship, capStatus, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.GetCapByRelationshipPartialCap()");
                }

                return capModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get CAPID Model by AltID.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="altID">Alt ID number.</param>
        /// <returns>cap information.</returns>
        /// <exception cref="DataValidateException">{ <c>altID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapIDModel4WS GetCapIDByAltID(string agencyCode, string altID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetCapIDByAltID()");
            }

            if (string.IsNullOrEmpty(altID))
            {
                throw new DataValidateException(new string[] { "altID" });
            }

            try
            {
                string callerID = User == null ? string.Empty : User.PublicUserId;

                CapIDModel4WS capID = CapService.getCapIDByAltID(agencyCode, altID, callerID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.GetCapIDByAltID()");
                }

                return capID;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets cap List for ACA, it contains Cap search condition.
        /// </summary>
        /// <param name="capModel4WS">cap model object.</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="userSeqNum">public user sequence number</param>
        /// <param name="capInfo">CAP Info: COMPLETE, INCOMPLETE</param>
        /// <param name="isOnlyMyCAP">is only my CAP</param>
        /// <param name="moduleNames">search cross modules name.</param>
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>array of cap information</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public SearchResultModel GetCapList4ACA(CapModel4WS capModel4WS, QueryFormat queryFormat, string userSeqNum, string capInfo, bool isOnlyMyCAP, List<string> moduleNames, bool isSearchAllStartRow, string[] viewElementNames)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.getCapList4ACA()");
            }

            if (string.IsNullOrEmpty(AgencyCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                string[] moduleNameArray = null;

                if (moduleNames != null)
                {
                    moduleNameArray = moduleNames.ToArray();
                }

                SearchResultModel response = CapService.getCapList4ACA(AgencyCode, capModel4WS, queryFormat, userSeqNum, capInfo, isOnlyMyCAP, moduleNameArray, isSearchAllStartRow, viewElementNames);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.getCapList4ACA()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get a Cap associate to a trade name
        /// </summary>
        /// <param name="capID">Cap ID Model</param>
        /// <returns>cap information</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS GetCapViewByAltID(CapIDModel4WS capID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetCapViewByAltID()");
            }

            try
            {
                CapModel4WS capModel = CapService.getCapViewByAltID(capID, User.UserSeqNum);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.GetCapViewByAltID()");
                }

                return capModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets cap view by single set.
        /// Gets cap contains CapDetailModel, CapWorkDesModel, AddressModel, CapContactModel, ContactsGroup,
        /// ParcelModel, OwnerModel, LicenseProfessionalModel, <c>BValuatnModel</c>.
        /// </summary>
        /// <param name="capID4ws">cap ID model</param>
        /// <param name="userSeqNbr">public user sequence number</param>
        /// <param name="isConditionOfApproval">The is condition of approval.</param>
        /// <param name="isSuperAgency">There are difference logic between super agency and normal agency during get the cap data.
        /// In super agency, we need to go through the normal agency logic in sometimes. So expose the <see cref="isSuperAgency" /> parameter.
        /// Such as: User select single service to create cap, will follow the normal agency's logic.</param>
        /// <returns>Cap With Condition Model4WS</returns>
        public CapWithConditionModel4WS GetCapViewBySingle(CapIDModel4WS capID4ws, string userSeqNbr, string isConditionOfApproval, bool isSuperAgency)
        {
            return GetCapViewBySingle(capID4ws, userSeqNbr, false, isConditionOfApproval, isSuperAgency);
        }

        /// <summary>
        /// Gets cap view by single set.
        /// Gets cap contains CapDetailModel, CapWorkDesModel, AddressModel, CapContactModel, ContactsGroup,
        /// ParcelModel, OwnerModel, LicenseProfessionalModel, <c>BValuatnModel</c>.
        /// </summary>
        /// <param name="capID4ws">cap ID model</param>
        /// <param name="userSeqNbr">public user sequence number</param>
        /// <param name="isForView">is For View</param>
        /// <param name="isConditionOfApproval">The is condition of approval.</param>
        /// <param name="isSuperAgency">There are difference logic between super agency and normal agency during get the cap data.
        /// In super agency, we need to go through the normal agency logic in sometimes. So expose the <see cref="isSuperAgency" /> parameter.
        /// Such as: User select single service to create cap, will follow the normal agency's logic.</param>
        /// <returns>Cap information With Condition</returns>
        public CapWithConditionModel4WS GetCapViewBySingle(CapIDModel4WS capID4ws, string userSeqNbr, bool isForView, string isConditionOfApproval, bool isSuperAgency)
        {
            return GetCapViewBySingle(capID4ws, userSeqNbr, isForView, isConditionOfApproval, isSuperAgency, false);
        }

        /// <summary>
        /// Gets cap view by single set.
        /// Gets cap contains CapDetailModel, CapWorkDesModel, AddressModel, CapContactModel, ContactsGroup,
        /// ParcelModel, OwnerModel, LicenseProfessionalModel, <c>BValuatnModel</c>.
        /// </summary>
        /// <param name="capID4ws">cap ID model</param>
        /// <param name="userSeqNbr">public user sequence number</param>
        /// <param name="isForView">is For View</param>
        /// <param name="isConditionOfApproval">The is condition of approval.</param>
        /// <param name="isSuperAgency">There are difference logic between super agency and normal agency during get the cap data.
        /// In super agency, we need to go through the normal agency logic in sometimes. So expose the <see cref="isSuperAgency" /> parameter.
        /// Such as: User select single service to create cap, will follow the normal agency's logic.</param>
        /// <param name="isForRenew">if it is true, get the child record for renewal.</param>
        /// <returns>Cap information With Condition</returns>
        /// <exception cref="DataValidateException">{ <c>capID4ws</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapWithConditionModel4WS GetCapViewBySingle(CapIDModel4WS capID4ws, string userSeqNbr, bool isForView, string isConditionOfApproval, bool isSuperAgency, bool isForRenew)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.getCapViewBySingle()");
            }

            if (capID4ws == null)
            {
                throw new DataValidateException(new string[] { "capID4ws" });
            }

            try
            {
                // if the user is authorized agent or clerk, it need not check the cap permission to view the cap detail.
                bool needCapPermission = !(User.IsAuthorizedAgent || User.IsAgentClerk);
                CapWithConditionModel4WS response = CapService.getCapViewBySingle(capID4ws, userSeqNbr, isSuperAgency, isForView, isConditionOfApproval, needCapPermission, isForRenew);

                if (response == null)
                {
                    if (Logger.IsInfoEnabled)
                    {
                        Logger.InfoFormat("Cann't find CAP [ID:{0}-{1}-{2}] or User[NO:{3}] does not have right to access this CAP.", capID4ws.id1, capID4ws.id2, capID4ws.id3, userSeqNbr);
                    }
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.getCapViewBySingle()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets Related Cap view Detail.
        /// </summary>
        /// <param name="capID4ws">cap ID number.</param>
        /// <param name="userSeqNbr">public user sequence number</param>
        /// <returns>Cap information</returns>
        /// <exception cref="DataValidateException">{ <c>capID4ws</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS GetCapViewDetailByPK(CapIDModel4WS capID4ws, string userSeqNbr)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.getCapViewDetailByPK()");
            }

            if (capID4ws == null)
            {
                throw new DataValidateException(new string[] { "capID4ws" });
            }

            try
            {
                CapModel4WS response = CapService.getCapViewDetailByPK(capID4ws, userSeqNbr);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.getCapViewDetailByPK()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the condition model by cap type.
        /// </summary>
        /// <param name="capType">The cap type model.</param>
        /// <param name="includeInactivity">Indicates whether include inactivity status or not.</param>
        /// <returns>return the NoticeConditionModel list.</returns>
        /// <exception cref="DataValidateException">{ <c>capType</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public NoticeConditionModel[] GetStdConditionByRecordType(CapTypeModel capType, bool includeInactivity)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.getStdConditionByRecordType()");
            }

            if (capType == null)
            {
                throw new DataValidateException(new string[] { "capType" });
            }

            try
            {
                NoticeConditionModel[] response = CapService.getStdConditionByRecordType(capType, includeInactivity);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.getStdConditionByRecordType()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the child agencies from cap id models.
        /// </summary>
        /// <param name="paymentResultModels">cap id models</param>
        /// <returns>agencies table</returns>
        public DataTable GetChildAgencies(PaymentResultModel4WS[] paymentResultModels)
        {
            // create an empty datatable for agencies.
            DataTable dtAgencies = ConstructAgencyTable();

            if (paymentResultModels == null || paymentResultModels.Length <= 1)
            {
                return dtAgencies;
            }

            foreach (PaymentResultModel4WS paymentResult in paymentResultModels)
            {
                if (paymentResult.parentCap == true)
                {
                    continue;
                }

                if (paymentResult.capID == null)
                {
                    continue;
                }

                CapIDModel4WS capID = paymentResult.capID;

                if (dtAgencies.Select("agency='" + capID.serviceProviderCode + "'").Length <= 0)
                {
                    DataRow dr = dtAgencies.NewRow();
                    dr[0] = capID.serviceProviderCode;
                    dtAgencies.Rows.Add(dr);
                }
            }

            dtAgencies.DefaultView.Sort = "agency";

            return dtAgencies;
        }

        /// <summary>
        /// Get child cap id models by parent cap model
        /// </summary>
        /// <param name="parentCapModel">CapIDModel4WS entity</param>
        /// <returns>Array of CapIDModel4WS</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapIDModel4WS[] GetChildCaps(CapIDModel4WS parentCapModel)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetChildCaps()");
            }

            try
            {
                CapIDModel4WS[] childCapIDs = CapService.getChildCaps(parentCapModel, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.GetChildCaps()");
                }

                return childCapIDs;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get ModuleName array by capId list.
        /// </summary>
        /// <param name="capIdModels">CapID Model array</param>
        /// <returns>return string array</returns>
        public StringArray[] GetModuleNamesByCapIDList(CapIDModel[] capIdModels)
        {
            return this.CapService.getModuleNamesByCapIDList(capIdModels);
        }

        /// <summary>
        /// get the child agency's cap id table.
        /// </summary>
        /// <param name="paymentResults">payment result models</param>
        /// <param name="childAgency">child agency code</param>
        /// <returns>child agency's cap id table</returns>
        public DataTable GetChildCapsByAgency(PaymentResultModel4WS[] paymentResults, string childAgency)
        {
            //create an empty datatable for caps.
            DataTable dtCaps = ConstructCapTable();

            if (paymentResults == null || paymentResults.Length <= 1 || string.IsNullOrEmpty(childAgency))
            {
                return dtCaps;
            }

            foreach (PaymentResultModel4WS paymentResult in paymentResults)
            {
                if (paymentResult.parentCap == true)
                {
                    continue;
                }

                CapIDModel4WS capID = paymentResult.capID;

                if (capID.serviceProviderCode.CompareTo(childAgency) != 0)
                {
                    continue;
                }

                DataRow dr = dtCaps.NewRow();
                dr[0] = capID.id1;
                dr[1] = capID.id2;
                dr[2] = capID.id3;
                dr[3] = capID.id1 + "-" + capID.id2 + "-" + capID.id3;

                string services = string.Empty;

                if (paymentResult.serviceNames != null)
                {
                    for (int i = 0; i < paymentResult.serviceNames.Length; i++)
                    {
                        services = services + paymentResult.serviceNames[i] + "<br/>";
                    }
                }

                if (services.Length >= 5)
                {
                    dr[4] = services.Substring(0, services.Length - 5);
                }
                else
                {
                    dr[4] = string.Empty;
                }

                dr[5] = capID.serviceProviderCode;
                dr[6] = paymentResult.moduleName;
                dr[7] = paymentResult.hasFee;
                dr[8] = capID.customID;
                dtCaps.Rows.Add(dr);
            }

            dtCaps.DefaultView.Sort = "CapID";

            return dtCaps;
        }

        /// <summary>
        /// Get the related CAP ids which are not included in the incoming <see cref="capIds" /> based on the CAP <see cref="relationship" />.
        /// </summary>
        /// <param name="capIds">Cap Id list</param>
        /// <param name="relationship">It may be "R", "EST", "Renewal", "Amendment" or "Ass Form".</param>
        /// <returns>missed Cap Id list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public IList<CapIDModel> GetMissingRecordIDs4ShoppingCart(CapIDModel[] capIds, string relationship)
        {
            try
            {
                return CapService.getMissingRecordIDs4ShoppingCart(capIds, relationship);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the my cap List for ACA.
        /// 1) Get the license professional list base on registered user.
        /// 2) Loop the license professional to get the caps
        /// 3) Combine the caps into the original my permit list.
        /// 4) add the CAPs that were created by him
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="capModel4WS">cap model object</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="userSeqNum">public user sequence number</param>
        /// <param name="capInfo">CAP Info: COMPLETE, INCOMPLETE, ALL</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>array of CapModel4WS</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public SearchResultModel GetMyCapList4ACA(string serviceProviderCode, CapModel4WS capModel4WS, QueryFormat queryFormat, string userSeqNum, string capInfo, string[] viewElementNames)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetMyCapList4ACA()");
            }

            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                SimpleCapModel[] response = CapService.getMyCapList4ACA(serviceProviderCode, capModel4WS, queryFormat, userSeqNum, capInfo, viewElementNames);
                SearchResultModel capResult = new SearchResultModel();
                capResult.resultList = response;

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.GetMyCapList4ACA()");
                }

                return capResult;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get permit list data by address model.
        /// </summary>
        /// <param name="address">address value.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="callerId">user id (public group).</param>
        /// <param name="isOnlyMyCAP">is only my permit</param>
        /// <param name="moduleNames">search cross modules name.</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>the permit list array.
        /// It contains 5 column:
        /// Date
        /// PermitNumber
        /// PermitType
        /// Description
        /// Status</returns>
        /// <exception cref="DataValidateException">{ <c>address</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public SearchResultModel GetPermitsByAddress(AddressModel address, string agencyCode, string moduleName, string callerId, bool isOnlyMyCAP, List<string> moduleNames, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames)
        {
            LogDebugInfo("Begin CapBll.GetPermitsByAddress()");

            if (address == null)
            {
                throw new DataValidateException(new string[] { "address" });
            }

            try
            {
                CapModel4WS capModel4WS = new CapModel4WS();

                capModel4WS.addressModel = address;
                capModel4WS.moduleName = moduleName;
                string[] moduleNameArray = null;

                if (moduleNames != null)
                {
                    moduleNameArray = moduleNames.ToArray();
                }

                SearchResultModel caps = CapService.getCapsByRefObjectID(agencyCode, capModel4WS, callerId, isOnlyMyCAP, moduleNameArray, queryFormat, isSearchAllStartRow, viewElementNames);

                LogDebugInfo("End CapBll.GetPermitsByAddress()");

                return caps;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get permit list data by contact.
        /// </summary>
        /// <param name="capContactModel">cap contact model.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="isOnlyMyCAP">is only my permit</param>
        /// <param name="moduleNames">search cross modules name.</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>the permit list array.
        /// It contains 5 column:
        /// Date
        /// PermitNumber
        /// PermitType
        /// Description
        /// Status</returns>
        /// <exception cref="DataValidateException">{ <c>capContactModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public SearchResultModel GetPermitsByContact(CapContactModel4WS capContactModel, string moduleName, bool isOnlyMyCAP, List<string> moduleNames, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames)
        {
            LogDebugInfo("Begin CapBll.GetPermitsByContact()");

            if (capContactModel == null)
            {
                throw new DataValidateException(new string[] { "capContactModel" });
            }

            try
            {
                CapModel4WS capModel4WS = new CapModel4WS();

                capModel4WS.capContactModel = capContactModel;
                capModel4WS.moduleName = moduleName;
                string[] moduleNameArray = null;

                if (moduleNames != null)
                {
                    moduleNameArray = moduleNames.ToArray();
                }

                SearchResultModel caps = CapService.getCapsByRefObjectID(AgencyCode, capModel4WS, User.PublicUserId, isOnlyMyCAP, moduleNameArray, queryFormat, isSearchAllStartRow, viewElementNames);
                LogDebugInfo("End CapBll.GetPermitsByContact()");

                return caps;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get permit list data by license id.
        /// </summary>
        /// <param name="licenseType">the license type.</param>
        /// <param name="licenseStateNumber">the license state number.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="callerId">user id (public group).</param>
        /// <param name="isOnlyMyCAP">is only my permit</param>
        /// <param name="moduleNames">search cross modules name.</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>the permit list array. 
        /// It contains 5 column:
        /// Date
        /// PermitNumber
        /// PermitType
        /// Description
        /// Status
        /// </returns>
        public SearchResultModel GetPermitsByLicenseId(string licenseType, string licenseStateNumber, string agencyCode, string moduleName, string callerId, bool isOnlyMyCAP, List<string> moduleNames, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames)
        {
            LogDebugInfo("Begin CapBll.GetPermitsByLicenseId()");

            if (string.IsNullOrEmpty(licenseType) || string.IsNullOrEmpty(licenseStateNumber))
            {
                throw new DataValidateException(new string[] { "licenseType", "licenseStateNumber" });
            }

            try
            {
                CapModel4WS capModel4WS = new CapModel4WS();

                LicenseProfessionalModel license = new LicenseProfessionalModel();
                license.licenseType = licenseType;
                license.licenseNbr = licenseStateNumber;

                capModel4WS.licenseProfessionalModel = TempModelConvert.ConvertToLicenseProfessionalModel4WS(license);
                capModel4WS.moduleName = moduleName;
                string[] moduleNameArray = null;

                if (moduleNames != null)
                {
                    moduleNameArray = moduleNames.ToArray();
                }

                SearchResultModel caps = CapService.getCapsByRefObjectID(agencyCode, capModel4WS, callerId, isOnlyMyCAP, moduleNameArray, queryFormat, isSearchAllStartRow, viewElementNames);

                LogDebugInfo("End CapBll.GetPermitsByLicenseId()");

                return caps;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets Related Cap List.
        /// </summary>
        /// <param name="rootCAPID4WS">root CAP ID</param>
        /// <param name="searchConstions4WS">cap model containing search conditions</param>
        /// <param name="queryFormat">query format</param>
        /// <returns>Array of CapModel4WS</returns>
        /// <exception cref="DataValidateException">{ <c>rootCAPID4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS[] GetRelatedCapList(CapIDModel4WS rootCAPID4WS, CapModel4WS searchConstions4WS, QueryFormat queryFormat)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.getRelatedCapList()");
            }

            if (rootCAPID4WS == null)
            {
                throw new DataValidateException(new string[] { "rootCAPID4WS" });
            }

            try
            {
                CapModel4WS[] response = CapService.getRelatedCapList(rootCAPID4WS, searchConstions4WS, queryFormat);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.getRelatedCapList()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get related Caps tree
        /// </summary>
        /// <param name="rootCAPID4WS">CapIDModel4WS entity</param>
        /// <param name="searchConstions4WS">CapModel4WS entity</param>
        /// <param name="queryFormat">queryFormat entity</param>
        /// <returns>ProjectTreeNodeModel4WS model</returns>
        /// <exception cref="DataValidateException">{ <c>rootCAPID4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ProjectTreeNodeModel4WS GetRelatedCapTree(CapIDModel4WS rootCAPID4WS, CapModel4WS searchConstions4WS, QueryFormat queryFormat)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetRelatedCapTree()");
            }

            if (rootCAPID4WS == null)
            {
                throw new DataValidateException(new string[] { "rootCAPID4WS" });
            }

            try
            {
                ProjectTreeNodeModel4WS response = CapService.getRelatedCapTree(rootCAPID4WS, searchConstions4WS, queryFormat, User.UserSeqNum);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.GetRelatedCapList()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get licensee's related caps. and add the user role into them.
        /// </summary>
        /// <param name="licensee">License Model 4WS</param>
        /// <param name="moduleNames">active module names</param>
        /// <param name="queryFormat">the queryFormat model</param>
        /// <returns>a data table for cap list</returns>
        public DataTable GetRelatedCapsByRefLP(LicenseModel4WS licensee, List<string> moduleNames, QueryFormat queryFormat)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetRelatedCapsByRefLP()");
            }

            string callerID = this.User == null ? string.Empty : this.User.PublicUserId;

            CapModel4WS[] capList = CapService.getRelatedCapsByRefLP(licensee, moduleNames.ToArray(), callerID, queryFormat);

            // convert capList to datatable, return empty table if no capModel to convert.
            DataTable dtPermits = BuildPermitDataTable(capList);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetRelatedCapsByRefLP()");
            }

            return dtPermits;
        }

        /// <summary>
        /// Get review child renewal cap ID by renewal parent Cap ID.
        /// </summary>
        /// <param name="capID4WS">Cap ID Model for web service</param>
        /// <returns>CapIDModel4WS object</returns>
        /// <exception cref="DataValidateException">{ <c>capModel4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapIDModel4WS GetReviewChildCapIDByMasterID(CapIDModel4WS capID4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetReviewChildCapIDByMasterID()");
            }

            if (capID4WS == null)
            {
                throw new DataValidateException(new string[] { "capModel4WS" });
            }

            try
            {
                CapIDModel4WS capID = CapService.getReviewChildCapIDByMasterID(capID4WS, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.GetReviewChildCapIDByMasterID()");
                }

                return capID;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get a related records tree for current record ID.
        /// This tree only include the parent layer and the child layer of the current record ID(<see cref="currentRecordId" />).
        /// </summary>
        /// <param name="currentRecordId">Current record ID.</param>
        /// <param name="queryFormat">query format</param>
        /// <returns>Tree node model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ProjectTreeNodeModel4WS GetSectionalRelatedCapTree(CapIDModel currentRecordId, QueryFormat queryFormat)
        {
            try
            {
                ProjectTreeNodeModel4WS capTree = CapService.getSectionalRelatedCapTree(currentRecordId, queryFormat, User.PublicUserId);

                return capTree;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Service list by Cap ID.
        /// </summary>
        /// <param name="capID4WS">CapID Model</param>
        /// <returns>return an array of Service Model</returns>
        public ServiceModel[] GetServicesByRecordID(CapIDModel4WS capID4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetServicesByRecordID()");
            }

            ServiceModel[] services = CapService.getServicesByRecordID(capID4WS);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetServicesByRecordID()");
            }

            return services;
        }

        /// <summary>
        /// Get Trade Name CapModel
        /// </summary>
        /// <param name="capIDModel">It like with capIDModel(customID,serviceProviderCode)</param>
        /// <returns>CapModel4WS of Trade Name</returns>
        /// <exception cref="DataValidateException">{ <c>capIDModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS GetTradeNameCapModel(CapIDModel4WS capIDModel)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.GetTradeNameCapModel()");
            }

            if (capIDModel == null)
            {
                throw new DataValidateException(new string[] { "capIDModel" });
            }

            try
            {
                CapModel4WS capModelCon = CapService.getCapByAltID(capIDModel);
                CapModel4WS capModel = new CapModel4WS();
                if (capModelCon != null && capModelCon.capID != null)
                {
                    // if the user is authorized agent or clerk, it need not check the cap permission to view the cap detail.
                    bool needCapPermission = !(User.IsAuthorizedAgent || User.IsAgentClerk);
                    capModel = CapService.getCapViewBySingle(capModelCon.capID, User.UserSeqNum, IsSuperAgency, false, ACAConstant.COMMON_N, needCapPermission, false).capModel;
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.GetTradeNameCapModel()");
                }

                return capModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Clone record
        /// </summary>
        /// <param name="srcCapID">source cap ID</param>
        /// <param name="sectionNamesList">clone sections' name</param>
        /// <param name="callerID">caller ID.</param>
        /// <returns>cap ID information list</returns>
        /// <exception cref="DataValidateException">{ <c>srcCapID, sectionNamesList</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        CapIDModel[] ICapBll.CloneRecords(CapIDModel srcCapID, string[] sectionNamesList, string callerID)
        {
            if (srcCapID == null || sectionNamesList == null || sectionNamesList.Length <= 0)
            {
                throw new DataValidateException(new string[] { "srcCapID", "sectionNamesList" });
            }

            try
            {
                CapIDModel[] capIDs = CapService.cloneRecords(srcCapID, sectionNamesList, User.PublicUserId);
                return capIDs;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get CompletePaymentResultWithAddressModel model with completePaymentResultModels.
        /// </summary>
        /// <param name="capPaymentResultModels">array of capPaymentResultModels</param>
        /// <returns>Complete payment result with address model</returns>
        CapPaymentResultWithAddressModel[] ICapBll.ConstructCapPaymentResultWithAddressModel(CapPaymentResultModel[] capPaymentResultModels)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.ConstructCapPaymentResultWithAddressModel()");
            }

            try
            {
                int length = capPaymentResultModels.Length;
                CapIDModel4WS[] capIDModels = new CapIDModel4WS[length];
                SimpleCapModel[] simpleCapModels = new SimpleCapModel[length];
                CapPaymentResultWithAddressModel[] capPaymentResultWithAddressModels = new CapPaymentResultWithAddressModel[length];

                for (int i = 0; i < length; i++)
                {
                    capIDModels[i] = TempModelConvert.Add4WSForCapIDModel(capPaymentResultModels[i].capID);
                }

                simpleCapModels = CapService.getSimpleCapsByCapID(capIDModels);
                if (simpleCapModels == null || simpleCapModels.Length < 1)
                {
                    return null;
                }

                for (int i = 0; i < length; i++)
                {
                    CapPaymentResultWithAddressModel capPaymentResultWithAddressModel = new CapPaymentResultWithAddressModel();
                    capPaymentResultWithAddressModel.capID = TempModelConvert.Add4WSForCapIDModel(capPaymentResultModels[i].capID);
                    capPaymentResultWithAddressModel.hasFee = capPaymentResultModels[i].hasFee;
                    capPaymentResultWithAddressModel.actionSource = capPaymentResultModels[i].actionSource;
                    capPaymentResultWithAddressModel.moduleName = capPaymentResultModels[i].moduleName;
                    capPaymentResultWithAddressModel.paramString = capPaymentResultModels[i].paramString;
                    capPaymentResultWithAddressModel.paymentStatus = capPaymentResultModels[i].paymentStatus;
                    capPaymentResultWithAddressModel.receiptNbr = capPaymentResultModels[i].receiptNbr;
                    capPaymentResultWithAddressModel.capType = CAPHelper.GetAliasOrCapTypeLabel(simpleCapModels[i]);
                    capPaymentResultWithAddressModel.address = simpleCapModels[i].addressModel;
                    capPaymentResultWithAddressModels[i] = capPaymentResultWithAddressModel;
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.ConstructCapPaymentResultWithAddressModel()");
                }

                return capPaymentResultWithAddressModels;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get child caps by mater cap id.
        /// </summary>
        /// <param name="parentCapID4WS">master cap id</param>
        /// <param name="relationship">cap relationship</param>
        /// <param name="status">the cap status</param>
        /// <returns>return the child Cap information.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        CapModel4WS[] ICapBll.GetChildCapDetailsByMasterID(CapIDModel4WS parentCapID4WS, string relationship, string status)
        {
            try
            {
                CapModel4WS[] response = CapService.getChildCapDetailsByMasterID(parentCapID4WS, relationship, status, User.PublicUserId);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get child cap id by master cap id.
        /// </summary>
        /// <param name="capID4WS">master cap id</param>
        /// <param name="relationship">cap relationship, e.g.: renew, pay fee due...</param>
        /// <param name="status">the cap status, e.g.: incomplete, pending...</param>
        /// <param name="isAcrossAgencies">true:In ACA super agency get child cap. otherwise is false.</param>
        /// <returns>Cap id model for ACA.</returns>
        CapIDModel4WS ICapBll.GetChildCapIDByMasterID(CapIDModel4WS capID4WS, string relationship, string status, bool isAcrossAgencies)
        {
            return CapService.getChildCapIDByMasterID(capID4WS, relationship, status, isAcrossAgencies, User.PublicUserId);
        }

        /// <summary>
        /// Get parent parent cap id by related child cap ID, relationship and status.
        /// </summary>
        /// <param name="childCapID">related child cap ID for web service</param>
        /// <param name="relationship">It may be "R", "EST", "Renewal", "Amendment" or "Ass Form".</param>
        /// <param name="status">1. It is for renewal CAP, it may be "Incomplete", "Review" or "Complete".
        /// 2. It is for amendment CAP, it may be  "Incomplete" or "Complete".
        /// 3. It is for Ass Form CAP, it may be null.</param>
        /// <returns>CapIDModel object</returns>
        /// <exception cref="DataValidateException">{ <c>childCapID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        CapIDModel ICapBll.GetParentCapIDByChildCapID(CapIDModel childCapID, string relationship, string status)
        {
            if (childCapID == null)
            {
                throw new DataValidateException(new string[] { "childCapID" });
            }

            try
            {
                CapIDModel capID = CapService.getParentCapIDByChildID(childCapID, relationship, status, User.PublicUserId);

                return capID;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get parent parent cap id list by related child cap ID, relationship and status.
        /// </summary>
        /// <param name="childCapID">related child cap ID for web service</param>
        /// <param name="relationship">It may be "R", "Renewal", "Amendment" or "Ass Form", except "EST".</param>
        /// <param name="status">1. It is for renewal CAP, it may be "Incomplete", "Review" or "Complete".
        /// 2. It is for amendment CAP, it may be  "Incomplete" or "Complete".
        /// 3. It is for Ass Form CAP, it may be null.</param>
        /// <returns>CapIDModel object array</returns>
        /// <exception cref="DataValidateException">{ <c>childCapID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        CapIDModel[] ICapBll.GetParentCapIDListByChildCapID(CapIDModel childCapID, string relationship, string status)
        {
            if (childCapID == null)
            {
                throw new DataValidateException(new string[] { "childCapID" });
            }

            try
            {
                CapIDModel[] capIDList = CapService.getParentCapIDListByChildID(childCapID, relationship, status, User.PublicUserId);

                return capIDList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Check that if configuration is valid for renewal license function.
        /// </summary>
        /// <param name="capID4WS">parent renewal cap ID.</param>
        /// <returns>configuration is wrong return false; configuration is right return true;</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        bool ICapBll.IsValidConfig4Renewal(CapIDModel4WS capID4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.IsValidConfig4Renewal()");
            }

            if (capID4WS == null)
            {
                return false;
            }

            try
            {
                bool result = CapService.IsValidConfig4Renewal(capID4WS, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.IsValidConfig4Renewal()");
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Generate a new alt id for fee estimate partial cap or partial cap, update cap alt ID and return it.
        /// </summary>
        /// <param name="capModel4WS">CapModel4WS entity</param>
        /// <param name="isFeeEstCap">Needs generate a new alt id for fee estimate partial cap, if this flag is true</param>
        /// <param name="isTemporaryCap">Needs generate a new alt id for fee estimate partial cap, if this flag is true.</param>
        /// <returns>alt ID number.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        string ICapBll.UpdateCapAltID(CapModel4WS capModel4WS, bool isFeeEstCap, bool isTemporaryCap)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.UpdateCapAltID()");
            }

            try
            {
                string result = CapService.updateCapAltID(capModel4WS, isFeeEstCap, isTemporaryCap, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.UpdateCapAltID()");
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get auto issue flag by renewal child cap ID.
        /// </summary>
        /// <param name="childCapID4WS">renewal child cap ID for web service</param>
        /// <returns>CapIDModel4WS object</returns>
        /// <exception cref="DataValidateException">{ <c>childCapID4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool IsAutoIssue4RenewalByChildCapID(CapIDModel4WS childCapID4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.isAutoIssue4RenewalByChildCapID()");
            }

            if (childCapID4WS == null)
            {
                throw new DataValidateException(new string[] { "childCapID4WS" });
            }

            try
            {
                bool isIssue = CapService.isAutoIssue4RenewalByChildCapID(childCapID4WS, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.isAutoIssue4RenewalByChildCapID()");
                }

                return isIssue;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Is the cap created by initial user
        /// </summary>
        /// <param name="capModel">the cap model</param>
        /// <returns>true or false.</returns>
        public bool IsCreateByDelegateUser(CapModel4WS capModel)
        {
            bool isCreatedByInitialUser = false;

            if (capModel == null)
            {
                return false;
            }

            if (capModel.createdBy != User.PublicUserId)
            {
                if (User.UserModel4WS.initialUsers != null && User.UserModel4WS.initialUsers.Length > 0)
                {
                    PublicUserModel4WS initUserModel = User.UserModel4WS.initialUsers.SingleOrDefault(p => p.userSeqNum == capModel.createdBy.Replace(ACAConstant.PUBLIC_USER_NAME, string.Empty));

                    if (initUserModel != null)
                    {
                        isCreatedByInitialUser = true;
                    }
                }
            }

            return isCreatedByInitialUser;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="capClass" /> is partial CAP class.
        /// </summary>
        /// <param name="capClass">the CAP class</param>
        /// <returns>true or false.</returns>
        public bool IsPartialCap(string capClass)
        {
            //for the previous logic in AA, the cap class of completed cap is null, in order to be compatible with old logic to add null check.
            return !string.IsNullOrEmpty(capClass) && !ACAConstant.COMPLETED.Equals(capClass, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// check whether the select Cap can be renewed by the current public user.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="capIDModel4WS">Cap ID Model</param>
        /// <returns>boolean:whether is renewed</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, capIDModel4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool IsRenewableForSingleCapID(string serviceProviderCode, CapIDModel4WS capIDModel4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.isRenewableForSingleCapID()");
            }

            if (string.IsNullOrEmpty(serviceProviderCode) || capIDModel4WS == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "capIDModel4WS" });
            }

            try
            {
                bool response = CapService.isRenewableForSingleCapID(serviceProviderCode, capIDModel4WS);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.isRenewableForSingleCapID()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Query permits Data is CapModel4WS what is apply in general search
        /// if only query my permits,user sequence should be the current user sequence number.
        /// if query all of permits, user sequence should be null.
        /// </summary>
        /// <param name="capModel">CapModel4WS as condition.</param>
        /// <param name="moduleNames">search cross modules name.</param>
        /// <param name="userSeqNum">user sequence number.</param>
        /// <param name="isOnlyMyCAP">is only my cap</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>SimpleCapModel4WS array.</returns>
        public SearchResultModel QueryPermitsGC(CapModel4WS capModel, List<string> moduleNames, string userSeqNum, bool isOnlyMyCAP, QueryFormat queryFormat, bool isSearchAllStartRow, string[] viewElementNames)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.QueryPermitsGC()");
            }

            SearchResultModel capList = GetCapsByConditionWithCapStyle(AgencyCode, capModel, queryFormat, userSeqNum, "ALL", isOnlyMyCAP, moduleNames, isSearchAllStartRow, viewElementNames);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.QueryPermitsGC()");
            }

            return capList;
        }

        /// <summary>
        /// save caps for sub agencies
        /// </summary>
        /// <param name="capModel4WS">The parent cap</param>
        /// <returns>The parent cap.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS SavePartialCaps(CapModel4WS capModel4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.SavePartialCaps()");
            }

            try
            {
                capModel4WS.createdByACA = ACAConstant.COMMON_Y;
                CapModel4WS capModel = CapService.savePartialCaps(AgencyCode, capModel4WS, User.PublicUserId, string.Empty, false);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.SavePartialCaps()");
                }

                return capModel;
            }
            catch (Exception e)
            {
                CapIDModel4WS[] childCapIDs = GetChildCaps(capModel4WS.capID);

                // All Child Cap should not be displayed in ACA when the seperated child cap process failures.
                CapModel4WS childCapModel = new CapModel4WS();
                foreach (CapIDModel4WS childCapID in childCapIDs)
                {
                    childCapModel.capID = childCapID;
                    childCapModel.capClass = ACAConstant.INCOMPLETE_TEMP_CAP;
                    UpdateCapClass(childCapModel);
                }

                throw new ACAException(e);
            }
        }

        /// <summary>
        /// create or update a partial cap.
        /// </summary>
        /// <param name="capModel4WS">cap model with applicant,application specific info and work location</param>
        /// <param name="wotModelSeq">work order ID</param>
        /// <param name="isFeeEstimates">whether fee estimate</param>
        /// <returns>CapModel4WS object</returns>
        /// <exception cref="DataValidateException">{ <c>capModel4WS, capModel4WS.capID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS SaveWrapperForPartialCap(CapModel4WS capModel4WS, string wotModelSeq, bool isFeeEstimates)
        {
            if (capModel4WS == null || capModel4WS.capID == null)
            {
                throw new DataValidateException(new string[] { "capModel4WS", "capModel4WS.capID" });
            }

            try
            {
                string agencyCode = capModel4WS.capID.serviceProviderCode;
                return CapService.saveWrapperForPartialCap(agencyCode, capModel4WS, User.PublicUserId, wotModelSeq, isFeeEstimates, false);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to update a cap's class.
        /// </summary>
        /// <param name="capModel4WS">cap model with applicant,application specific info and work location</param>
        /// <returns>CapModel4WS object</returns>
        /// <exception cref="DataValidateException">{ <c>capModel4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS UpdateCapClass(CapModel4WS capModel4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.UpdateCapClass()");
            }

            if (capModel4WS == null)
            {
                throw new DataValidateException(new string[] { "capModel4WS" });
            }

            try
            {
                CapModel4WS capModel = CapService.updateCapClass(capModel4WS);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.UpdateCapClass()");
                }

                return capModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update cap class
        /// </summary>
        /// <param name="caps">the cap information which need to be updated</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void UpdateCapStatus(CapModel4WS[] caps)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.UpdateCapStatus()");
            }

            if (caps == null)
            {
                return;
            }

            try
            {
                CapService.updateCapStatus(caps);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.UpdateCapStatus()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to update a partial cap.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="capModel4WS">cap model with applicant,application specific info and work location</param>
        /// <param name="publicUserId">"PUBLICUSER" + public user sequence number</param>
        /// <returns>CapModel4WS object</returns>
        /// <exception cref="DataValidateException">{ <c>capModel4WS, capModel4WS.capID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS UpdatePartialCapModelWrapper(string serviceProviderCode, CapModel4WS capModel4WS, string publicUserId)
        {
            if (capModel4WS == null || capModel4WS.capID == null)
            {
                throw new DataValidateException(new string[] { "capModel4WS", "capModel4WS.capID" });
            }

            try
            {
                capModel4WS.createdByACA = ACAConstant.COMMON_Y;

                return CapService.updatePartialCapModelWrapper(capModel4WS.capID.serviceProviderCode, capModel4WS, publicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update partial caps for super agency
        /// </summary>
        /// <param name="capModel4WS">The parent cap</param>
        /// <returns>The parent cap.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapModel4WS UpdatePartialCaps(CapModel4WS capModel4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapBll.UpdatePartialCaps()");
            }

            try
            {
                CapModel4WS capModel = CapService.updatePartialCaps(AgencyCode, capModel4WS, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapBll.UpdatePartialCaps()");
                }

                return capModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get licenses by customer sequence number
        /// </summary>
        /// <param name="contactSeqNum">the customer sequence number</param>
        /// <param name="modueName">the module name</param>
        /// <param name="filterName">the filter name</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="callerID">the caller id</param>
        /// <returns>SimpleCap model array</returns>
        /// <exception cref="DataValidateException">{ <c>contactSeqNum</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public SimpleCapModel[] GetLicensesByCustomer(string contactSeqNum, string modueName, string filterName, QueryFormat queryFormat, string callerID)
        {
            if (contactSeqNum == null)
            {
                throw new DataValidateException(new string[] { "contactSeqNum" });
            }

            try
            {
                return CapService.getCapsByContact4Clerk(AgencyCode, contactSeqNum, modueName, filterName, queryFormat, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the cap temporary data.
        /// </summary>
        /// <param name="capTemporaryData">The CapTemporaryDataModel</param>
        /// <returns>return the CapTemporaryDataModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTemporaryDataModel GetCapTemporaryData(CapTemporaryDataModel capTemporaryData)
        {
            try
            {
                return CapService.getCapTemporaryData(capTemporaryData);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// update the cap temporary data.
        /// </summary>
        /// <param name="capTemporaryData">The CapTemporaryDataModel</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void UpdateCapTemporaryData(CapTemporaryDataModel capTemporaryData)
        {
            try
            {
                CapService.updateCapTemporaryData(capTemporaryData);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Populate the CapModel4WS list to a DataTable.
        /// </summary>
        /// <param name="capModel4WSList">a CapModel4WS list</param>
        /// <returns>The permit list data table.</returns>
        public DataTable BuildPermitDataTable(CapModel4WS[] capModel4WSList)
        {
            // create an empty datatable for permits.
            DataTable dtPermit = ConstructPermitDataTable();

            if (capModel4WSList == null || capModel4WSList.Length == 0)
            {
                return dtPermit;
            }

            foreach (CapModel4WS cap in capModel4WSList)
            {
                string permitType = CAPHelper.GetAliasOrCapTypeLabel(cap);

                DataRow dr = dtPermit.NewRow();
                dr[1] = cap.altID;
                dr[2] = permitType;

                if (cap.capDetailModel != null)
                {
                    dr[3] = cap.capDetailModel.shortNotes ?? string.Empty;
                }
                else
                {
                    dr[3] = string.Empty;
                }

                string status = I18nStringUtil.GetString(cap.resCapStatus, cap.capStatus);

                dr[4] = status;
                dr[5] = cap.capID.id1;
                dr[6] = cap.capID.id2;
                dr[7] = cap.capID.id3;
                dr[8] = cap.capClass;

                if (cap.fileDate == null)
                {
                    dr[0] = DBNull.Value; // DateTime.Parse(cap.fileDate);
                }
                else
                {
                    dr[0] = I18nDateTimeUtil.ParseFromWebService4DataTable(cap.fileDate);
                }

                dr[9] = cap.specialText;
                dr["CapClass"] = cap.capClass ?? string.Empty;
                dr["HasNoPaidFees"] = cap.noPaidFeeFlag;
                dr["RenewalStatus"] = cap.renewalStatus;
                dr["PermitAddress"] = GetPermitAddreessForMap(cap);
                dr["hasPrivilegeToHandleCap"] = cap.hasPrivilegeToHandleCap;
                dr["EnglishTradeName"] = cap.licenseProfessionalModel == null ? string.Empty : cap.licenseProfessionalModel.businessName;
                dr["ArabicTradeName"] = cap.licenseProfessionalModel == null ? string.Empty : cap.licenseProfessionalModel.busName2;
                dr["relatedTradeLicense"] = cap.licenseProfessionalModel == null ? string.Empty : cap.licenseProfessionalModel.relatedTradeLic;
                dr["filterName"] = cap.capType.filterName;
                dr["isAmendable"] = cap.capType.isAmendable;
                dr["licenseType"] = cap.licenseProfessionalModel == null ? string.Empty : cap.licenseProfessionalModel.licenseType;

                if (cap.capID != null && !string.IsNullOrEmpty(cap.capID.serviceProviderCode))
                {
                    dr["AgencyCode"] = cap.capID.serviceProviderCode;
                }
                else
                {
                    dr["AgencyCode"] = ACAConstant.AgencyCode;

                    if (Logger.IsDebugEnabled)
                    {
                        Logger.Debug("agencyCode is null for General search");
                    }
                }

                dr["expirationDate"] = I18nDateTimeUtil.ParseFromWebService4DataTable(cap.expDate);

                dr["isTNExpired"] = cap.isTNExpired;
                dr["ModuleName"] = cap.moduleName;
                dr["Address"] = GetPermitAddreess(cap);
                dr["CreatedBy"] = string.Empty;
                dr["AppStatusGroup"] = cap.statusGroupCode;
                dr["AppStatus"] = cap.capStatus;
                dr["PaymentStatus"] = cap.paymentStatus;

                dtPermit.Rows.Add(dr);
            }

            return dtPermit;
        }

        /// <summary>
        /// Clear the component data in hidden page for the CapModel.
        /// </summary>
        /// <param name="capModel">The Current CapModel</param>
        /// <param name="pageFlowGroup">The PageFlow group</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void ClearCapData(CapModel4WS capModel, PageFlowGroupModel pageFlowGroup)
        {
            try
            {
                CapService.clearCapData(capModel.capID.serviceProviderCode, capModel, pageFlowGroup, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Remove child partial cap by cap id
        /// </summary>
        /// <param name="parentCapId">parent cap Id</param>
        /// <param name="childCapId">child cap Id</param>
        /// <param name="userId">user id</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void RemoveChildPartialCap(CapIDModel4WS parentCapId, CapIDModel4WS childCapId, string userId)
        {
            try
            {
                CapService.removeChildPartialCap(parentCapId, childCapId, userId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Construct a new DataTable for Permits.
        /// </summary>
        /// <returns>A DataTable that contains 5 columns.
        /// Date
        /// PermitNumber
        /// PermitType
        /// Description
        /// Status
        /// </returns>
        private static DataTable ConstructPermitDataTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("PermitNumber");
            table.Columns.Add("PermitType");
            table.Columns.Add("Description");
            table.Columns.Add("Status");
            table.Columns.Add("CapID1");
            table.Columns.Add("CapID2");
            table.Columns.Add("CapID3");
            table.Columns.Add("Class");
            table.Columns.Add("ProjectName");
            table.Columns.Add("CapClass");
            table.Columns.Add(new DataColumn("HasNoPaidFees", typeof(bool)));
            table.Columns.Add(new DataColumn("RenewalStatus", typeof(string)));
            table.Columns.Add(new DataColumn("expirationDate", typeof(DateTime)));
            table.Columns.Add(new DataColumn("PermitAddress", typeof(string)));
            table.Columns.Add(new DataColumn("hasPrivilegeToHandleCap", typeof(bool)));
            table.Columns.Add(new DataColumn("EnglishTradeName", typeof(string)));
            table.Columns.Add(new DataColumn("ArabicTradeName", typeof(string)));
            table.Columns.Add(new DataColumn("relatedTradeLicense", typeof(string)));
            table.Columns.Add(new DataColumn("filterName", typeof(string)));
            table.Columns.Add(new DataColumn("isAmendable", typeof(string)));
            table.Columns.Add(new DataColumn("licenseType", typeof(string)));
            table.Columns.Add(new DataColumn("isTNExpired", typeof(bool)));
            table.Columns.Add("AgencyCode");
            table.Columns.Add("ModuleName");
            table.Columns.Add("Address");
            table.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
            table.Columns.Add(new DataColumn("AppStatusGroup", typeof(string)));
            table.Columns.Add(new DataColumn("AppStatus", typeof(string)));
            table.Columns.Add(new DataColumn("PaymentStatus", typeof(string)));

            return table;
        }

        /// <summary>
        /// get address from CapModel4WS
        /// </summary>
        /// <param name="capMode">the cap model</param>
        /// <returns>Permit Address</returns>
        private static string GetPermitAddreess(CapModel4WS capMode)
        {
            string result = string.Empty;

            if (capMode != null)
            {
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                result = addressBuilderBll.BuildAddressByFormatType(capMode.addressModel, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
            }

            return result;
        }

        /// <summary>
        /// get address from CapModel4WS to display in Map
        /// </summary>
        /// <param name="capMode">the cap model</param>
        /// <returns>Permit Address</returns>
        private static string GetPermitAddreessForMap(CapModel4WS capMode)
        {
            string result = string.Empty;

            if (capMode != null)
            {
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                result = addressBuilderBll.BuildAddressByFormatType(capMode.addressModel, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
            }

            return result;
        }

        /// <summary>
        /// Construct a new DataTable for Agencies.
        /// </summary>
        /// <returns>A DataTable that contains 1 column.
        /// agency
        /// </returns>
        private DataTable ConstructAgencyTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("agency", typeof(string)));

            return table;
        }

        /// <summary>
        /// Construct a new DataTable for Caps.
        /// </summary>
        /// <returns>A DataTable that contains 5 columns.
        /// CapID1
        /// CapID2
        /// CapID3
        /// CapID
        /// Services
        /// </returns>
        private DataTable ConstructCapTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("CapID1", typeof(string)));
            table.Columns.Add(new DataColumn("CapID2", typeof(string)));
            table.Columns.Add(new DataColumn("CapID3", typeof(string)));
            table.Columns.Add(new DataColumn("CapID", typeof(string)));
            table.Columns.Add(new DataColumn("Services", typeof(string)));
            table.Columns.Add(new DataColumn("serviceProviderCode", typeof(string)));
            table.Columns.Add(new DataColumn("ModuleName", typeof(string)));
            table.Columns.Add(new DataColumn("HasFee", typeof(bool)));
            table.Columns.Add(new DataColumn("CustomID", typeof(string)));
            return table;
        }

        /// <summary>
        /// Get Cap List by Condition for Cap search.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="capModel4WS">the cap model</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="userSeqNum">public user sequence number</param>
        /// <param name="capInfo">CAP Info: COMPLETE, INCOMPLETE, ALL </param>
        /// <param name="isOnlyMyCAP">Is only my CAP</param>
        /// <param name="moduleNames">search cross modules name.</param>        
        /// <param name="isSearchAllStartRow">is search my permit list or not.</param>
        /// <param name="viewElementNames">Inactive grid view element names.</param>
        /// <returns>array of SimpleCapModel4WS</returns>
        private SearchResultModel GetCapsByConditionWithCapStyle(string serviceProviderCode, CapModel4WS capModel4WS, QueryFormat queryFormat, string userSeqNum, string capInfo, bool isOnlyMyCAP, List<string> moduleNames, bool isSearchAllStartRow, string[] viewElementNames)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Begin CapBll.getCapsByConditionWithCapStyle()");
            }

            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                string[] moduleNameArray = null;

                if (moduleNames != null)
                {
                    moduleNameArray = moduleNames.ToArray();
                }

                SearchResultModel response = CapService.getCapsByConditionWithCapStyle(serviceProviderCode, capModel4WS, queryFormat, userSeqNum, capInfo, isOnlyMyCAP, moduleNameArray, isSearchAllStartRow, viewElementNames);

                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("End CapBll.getCapsByConditionWithCapStyle()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Log info when debug is enabled.
        /// </summary>
        /// <param name="debugInfo">debug Info.</param>
        private void LogDebugInfo(string debugInfo)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(debugInfo);
            }
        }

        #endregion Methods
    }
}