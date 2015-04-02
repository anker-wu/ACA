#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminSession.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AdminSession.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;
using System.Web;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common.GlobalSearch;
using Accela.ACA.Web.Examination;
using Accela.ACA.Web.Inspection;
using Accela.ACA.Web.People;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.UI;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Session class for Admin
    /// </summary>
    internal class AdminSession : IACASession
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Breadcrumb information, which is used on apply process.
        /// </summary>
        public BreadCrumbParmsInfo BreadCrumbParmsInfo
        {
            get
            {
                return HttpContext.Current.Session[SessionConstant.SESSION_BREADCRUMB] as BreadCrumbParmsInfo;
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_BREADCRUMB] = value;
            }
        }

        /// <summary>
        /// Gets or sets the inspection data.
        /// </summary>
        public List<InspectionListItemViewModel> InspectionData
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstant.SESSION_INSPECTION_DATA] == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Current.Session[SessionConstant.SESSION_INSPECTION_DATA] as List<InspectionListItemViewModel>;
                }
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_INSPECTION_DATA] = value;
            }
        }

        /// <summary> 
        /// Gets or sets the inspection data.
        /// </summary>
        public List<ExaminationListItemViewModel> ExaminationData
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstant.SESSION_EXAMINATION_DATA] == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Current.Session[SessionConstant.SESSION_EXAMINATION_DATA] as List<ExaminationListItemViewModel>;
                }
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_EXAMINATION_DATA] = value;
            }
        }

        /// <summary>
        /// Gets or sets the inspection parameters.
        /// </summary>
        /// <value>
        /// The inspection parameters.
        /// </value>
        public Hashtable InspectionParameters
        {
            get
            {
                return HttpContext.Current.Session[SessionConstant.SESSION_INSPECTION_PARAMETERS] as Hashtable;
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_INSPECTION_PARAMETERS] = value;
            }
        }

        /// <summary>
        /// Gets or sets the examination parameters.
        /// </summary>
        /// <value>The examination parameters.</value>
        public Hashtable ExaminationParameters
        {
            get
            {
                return HttpContext.Current.Session[SessionConstant.SESSION_EXAMINATION_PARAMETERS] as Hashtable;
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_EXAMINATION_PARAMETERS] = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected parcel info, which is used on apply process to fill the address section.
        /// </summary>
        public ParcelInfoModel SelectedParcelInfo
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstant.SESSION_SELECTED_PARCEL_INFO] == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Current.Session[SessionConstant.SESSION_SELECTED_PARCEL_INFO] as ParcelInfoModel;
                }
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_SELECTED_PARCEL_INFO] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Breadcrumb information, which is used on shopping cart.
        /// </summary>
        public BreadCrumbParmsInfo ShoppingCartBreadCrumbParmsInfo
        {
            get
            {
                return HttpContext.Current.Session[SessionConstant.SESSION_SHOPPING_CART_BREADCRUMB] as BreadCrumbParmsInfo;
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_SHOPPING_CART_BREADCRUMB] = value;
            }
        }

        /// <summary>
        /// Gets or sets the current user session.
        /// </summary>
        User IACASession.User
        {
            get
            {
                return HttpContext.Current.Session[SessionConstant.SESSION_USER] as User;
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_USER] = value;
            }
        }

        /// <summary>
        /// Gets or sets the HistoryAction information.
        /// </summary>
        public HistoryAction HistoryAction
        {
            get
            {
                return HttpContext.Current.Session[SessionConstant.SESSION_HISTORYACTION] as HistoryAction;
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_HISTORYACTION] = value;
            }
        }

        /// <summary>
        /// Sets the Condition Additional Information to Session.
        /// </summary>
        /// <param name="additionalInfo">The Condition Additional Information List</param>
        void IACASession.SetConditionAdditionalInfoToSession(Dictionary<string, string> additionalInfo)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Gets the Condition Additional Information from Session.
        /// </summary>
        /// <returns>The condition additional information from session.</returns>
        Dictionary<string, string> IACASession.GetConditionAdditionalInfoFromSession()
        {
            return null;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the cap models from session.
        /// </summary>
        /// <returns>CapModel object.</returns>
        CapIDModel4WS[] IACASession.GetCapIDModelsFromSession()
        {
            return (CapIDModel4WS[])HttpContext.Current.Session[SessionConstant.SESSION_CAPID_MODELS];
        }

        /// <summary>
        /// Get the parent Cap ID model from Session.
        /// </summary>
        /// <param name="relationship">It may be <c>"R", "EST", "Renewal", "Amendment" or "AssoForm"</c>.</param>
        /// <returns>parent Cap ID model</returns>
        CapIDModel4WS IACASession.GetParentCapIDModelFromSession(string relationship)
        {
            return null;
        }

        /// <summary>
        /// Get announcements from session
        /// </summary>
        /// <returns>announcement model</returns>
        List<AnnouncementModel> IACASession.GetAnnouncementsFromSession()
        {
            return null;
        }

        /// <summary>
        /// Gets announcement flag from session.
        /// </summary>
        /// <returns>The announcement flag.</returns>
        string IACASession.GetAnnouncementFlagFromSession()
        {
            return null;
        }

        /// <summary>
        /// Gets the cap model from session.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>CapModel object.</returns>
        CapModel4WS IACASession.GetCapModelFromSession(string moduleName)
        {
            return (CapModel4WS)HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN];
        }

        /// <summary>
        ///  Get Shopping Cart Item Number Form Session
        /// </summary>
        /// <returns>cart item number</returns>
        string IACASession.GetCartItemNumberFromSession()
        {
            return HttpContext.Current.Session[SessionConstant.SESSION_SHOPPINGCART_ITEMNUMBER].ToString();
        }

        /// <summary>
        /// Get my collection models from session.
        /// </summary>
        /// <returns>MyCollectionModel4WS array</returns>
        MyCollectionModel[] IACASession.GetMyCollectionsFromSession()
        {
            return (MyCollectionModel[])HttpContext.Current.Session[SessionConstant.SESSION_MYCOLLECTIONS];
        }

        /// <summary>
        /// Get the flag which current user whether has Provider License from session.
        /// </summary>
        /// <returns>Indicate the current user whether has Provider License.</returns>
        int IACASession.GetHasProviderLicenseFlagFromSession()
        {
            if (HttpContext.Current.Session[SessionConstant.SESSION_HAS_PROVIDER_LICENSE] == null)
            {
                return TabUtil.NONE_PROVIDER_LICENSE;
            }

            return (int)HttpContext.Current.Session[SessionConstant.SESSION_HAS_PROVIDER_LICENSE];
        }

        /// <summary>
        /// gets the OnlinePaymentResultModel from session. Them were returned from creating real caps .
        /// </summary> 
        /// <returns>OnlinePaymentResultModel from session</returns>
        OnlinePaymentResultModel IACASession.GetOnlinePaymentResultModelFromSession()
        {
            return (OnlinePaymentResultModel)HttpContext.Current.Session[SessionConstant.SESSION_ONLINE_PAYMENT_RESULT];
        }

        /// <summary>
        /// Gets the page flow group from session.
        /// </summary>
        /// <returns>PageFlowGroupModel object.</returns>
        PageFlowGroupModel IACASession.GetPageflowGroupFromSession()
        {
            IPageflowBll pageflowBll = (IPageflowBll)ObjectFactory.GetObject(typeof(IPageflowBll));
            PageFlowGroupModel model = pageflowBll.GetPageflowGroupByCapType(null);
            HttpContext.Current.Session[SessionConstant.SESSION_PAGEFLOW_GROUP] = model;

            return model;
        }

        /// <summary>
        /// Get the parent cap type from session.
        /// </summary>
        /// <returns>CapTypeModel object.</returns>
        CapTypeModel IACASession.GetParentCapTypeFromSession()
        {
            return null;
        }

        /// <summary>
        /// Get the parent page flow group from session.
        /// </summary>
        /// <returns>PageFlowGroupModel object.</returns>
        PageFlowGroupModel IACASession.GetParentPageflowGroupFromSession()
        {
            return null;
        }

        /// <summary>
        /// gets the PaymentResultModel4WS from session. Them were returned from creating real caps .
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>PaymentResultModel4WS array</returns>
        PaymentResultModel4WS[] IACASession.GetPaymentResultModelsFromSession(string moduleName)
        {
            return (PaymentResultModel4WS[])HttpContext.Current.Session[SessionConstant.SESSION_PAYMENTRESULT_MODELS + "|" + moduleName];
        }

        /// <summary>
        /// Get report parameters from session.
        /// </summary>
        /// <returns>ParameterModel4WS array</returns>
        ParameterModel4WS[] IACASession.GetReportParameterFromSession()
        {
            if (HttpContext.Current.Session[SessionConstant.SESSION_REPORT_PARAMETER] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[SessionConstant.SESSION_REPORT_PARAMETER] as ParameterModel4WS[];
            }
        }

        /// <summary>
        /// Gets the selected services from session.
        /// </summary>
        /// <returns>ServiceManagementModel4WS list.</returns>
        ServiceModel[] IACASession.GetSelectedServicesFromSession()
        {
            return (ServiceModel[])HttpContext.Current.Session[SessionConstant.SESSION_SELECTED_SERVICES];
        }

        /// <summary>
        /// Get the UI data from session.
        /// </summary>
        /// <returns>A hashtable contains the all UI data.</returns>
        Hashtable IACASession.GetUIDataFromSession()
        {
            return null;
        }

        /// <summary>
        /// Gets the asset search model from session.
        /// </summary>
        /// <returns>
        /// The Asset search model
        /// </returns>
        AssetMasterModel IACASession.GetAssetSearchModelFromSession()
        {
            return null;
        }

        /// <summary>
        /// Sets the cap model to session. 
        /// </summary>
        /// <param name="capIDModels">the CapModel to be set to session.</param>
        void IACASession.SetCapIDModelsToSession(CapIDModel4WS[] capIDModels)
        {
            //in admin mode, we never need to set this session to null
            if (capIDModels != null)
            {
                HttpContext.Current.Session[SessionConstant.SESSION_CAPID_MODELS] = capIDModels;
            }
        }

        /// <summary>
        /// Set the parent Cap ID model to Session.
        /// </summary>
        /// <param name="relationship">It may be <c>"R", "EST", "Renewal", "Amendment" or "AssoForm"</c>.</param>
        /// <param name="parentCapID">parent Cap ID model</param>
        void IACASession.SetParentCapIDModelToSession(string relationship, CapIDModel4WS parentCapID)
        {
            if (parentCapID != null)
            {
                HttpContext.Current.Session[SessionConstant.SESSION_PARENT_CAPID_MODEL + "|" + relationship] = parentCapID;
            }
        }

        /// <summary>
        /// Sets the cap model to session. 
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="capModel">the CapModel to be set to session.</param>
        void IACASession.SetCapModelToSession(string moduleName, CapModel4WS capModel)
        {
            //in admin mode, we never need to set this session to null
            if (capModel != null)
            {
                HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL_ADMIN] = capModel;
            }
        }

        /// <summary>
        ///  set Shopping Cart Item Number to Session
        /// </summary>
        /// <param name="cartItemsNumber">shopping cart item number</param>
        void IACASession.SetCartItemNumberToSession(string cartItemsNumber)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_SHOPPINGCART_ITEMNUMBER] = cartItemsNumber;
        }

        /// <summary>
        /// Set my collection models to session.
        /// </summary>
        /// <param name="myCollectionModelList">my collection model list</param>
        void IACASession.SetMyCollectionsToSession(MyCollectionModel[] myCollectionModelList)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_MYCOLLECTIONS] = myCollectionModelList;
        }

        /// <summary>
        /// Sets the flag which current user whether has Provider License to session.
        /// </summary>
        /// <param name="hasProviderLicenseFlag">indicate current user whether has Provider License.</param>
        void IACASession.SetHasProviderLicenseFlagToSession(int hasProviderLicenseFlag)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_HAS_PROVIDER_LICENSE] = hasProviderLicenseFlag;
        }

        /// <summary>
        /// sets the OnlinePaymentResultModel to session,them were returned from creating real caps.
        /// </summary> 
        /// <param name="onlinePaymentResultModel">OnlinePaymentResultModel object</param>
        void IACASession.SetOnlinePaymentResultModelToSession(OnlinePaymentResultModel onlinePaymentResultModel)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_ONLINE_PAYMENT_RESULT] = onlinePaymentResultModel;
        }

        /// <summary>
        /// Sets the cap model to session. 
        /// </summary>
        /// <param name="pageflow">the PageFlowGroupModel to be set to session.</param>
        void IACASession.SetPageflowGroupToSession(PageFlowGroupModel pageflow)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_PAGEFLOW_GROUP] = pageflow;
        }

        /// <summary>
        /// Set the parent cap type model to session. 
        /// </summary>
        /// <param name="capType">the CapTypeModel to be set to session.</param>
        void IACASession.SetParentCapTypeToSession(CapTypeModel capType)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_PARENT_CAPTYPE] = capType;
        }

        /// <summary>
        /// Set the parent page flow group model to session.
        /// </summary>
        /// <param name="pageflow">the PageFlowGroupModel to be set to session.</param>
        void IACASession.SetParentPageflowGroupToSession(PageFlowGroupModel pageflow)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_PARENT_PAGEFLOW_GROUP] = pageflow;
        }

        /// <summary>
        /// sets the Payment Result models to session,them were returned from creating real caps.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="paymentResultModels">PaymentResultModel4WS array</param>
        void IACASession.SetPaymentResultModelsToSession(string moduleName, PaymentResultModel4WS[] paymentResultModels)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_PAYMENTRESULT_MODELS + "|" + moduleName] = paymentResultModels;
        }

        /// <summary>
        /// Set report parameters to session.
        /// </summary>
        /// <param name="parameters">ParameterModel4WS array</param>
        void IACASession.SetReportParameterToSession(ParameterModel4WS[] parameters)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_REPORT_PARAMETER] = parameters;
        }

        /// <summary>
        /// Set selected services to session
        /// </summary>
        /// <param name="services">ServiceModel array</param>
        void IACASession.SetSelectedServicesToSession(ServiceModel[] services)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_SELECTED_SERVICES] = services;
        }

        /// <summary>
        /// Set UI data to session.
        /// </summary>
        /// <param name="uiData">A hashtable contains the all UI data.</param>
        void IACASession.SetUIDataToSession(Hashtable uiData)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_UI_DATA] = uiData;
        }

        /// <summary>
        /// Sets announcements to session
        /// </summary>
        /// <param name="announcements">the announcement list</param>
        void IACASession.SetAnnouncementsToSession(List<AnnouncementModel> announcements)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_ANNOUNCEMENT_LIST] = announcements;
        }

        /// <summary>
        /// Sets announcement flag to session
        /// </summary>
        /// <param name="flag">the announcement flag.</param>
        void IACASession.SetAnnouncementFlagToSession(string flag)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_ANNOUNCEMENT_LIST_FLAG] = flag;
        }

        /// <summary>
        /// Gets the people model from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the people object.</returns>
        PeopleModel IACASession.GetPeopleModelFromSession(string contactSeqNbr)
        {
            return null;
        }

        /// <summary>
        /// Sets the people model to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="peopleModel">The people model.</param>
        void IACASession.SetPeopleModelToSession(string contactSeqNbr, PeopleModel peopleModel)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Sets the asset search model to session.
        /// </summary>
        /// <param name="assetMasterModel">The asset master model.</param>
        void IACASession.SetAssetSearchModelToSession(AssetMasterModel assetMasterModel)
        {
            //not need implement, keep empty.
        }

        /// <summary>
        /// Get the expression info from session.
        /// </summary>
        /// <returns>A hashtable contains the all expression data.</returns>
        Hashtable IACASession.GetExpressionDataFromSession()
        {
            return null;
        }

        /// <summary>
        /// Set the expression info to session.
        /// </summary>
        /// <param name="expressionData">A hashtable contains the all expression data.</param>
        void IACASession.SetExpressionDataToSession(Hashtable expressionData)
        {
        }

        /// <summary>
        /// Gets the reference contact education list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference education list object.</returns>
        EducationModel4WS[] IACASession.GetContactEducationListFromSession(string contactSeqNbr)
        {
            return null;
        }

        /// <summary>
        /// Gets the reference contact education list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference education list object.</returns>
        ContinuingEducationModel4WS[] IACASession.GetContactContEducationListFromSession(string contactSeqNbr)
        {
            return null;
        }

        /// <summary>
        /// Gets the attachment upload file info
        /// </summary>
        /// <param name="moduleName">The module name</param>
        /// <returns>The upload file information.</returns>
        Dictionary<string, List<FileUploadInfo>> IACASession.GetUploadFileInfoFromSession(string moduleName)
        {
            return null;
        }

        /// <summary>
        /// Sets the reference contact education list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="educationList">The education list.</param>
        void IACASession.SetContactEducationListToSession(string contactSeqNbr, EducationModel4WS[] educationList)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Gets the contact examination list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference examination list object.</returns>
        ExaminationModel[] IACASession.GetContactExaminationListFromSession(string contactSeqNbr)
        {
            return null;
        }

        /// <summary>
        /// Sets the contact examination list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="examinationList">The examination list.</param>
        void IACASession.SetContactExaminationListToSession(string contactSeqNbr, ExaminationModel[] examinationList)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Sets the reference contact education list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="contEducationList">The continuing education list.</param>
        void IACASession.SetContactContEducationListToSession(string contactSeqNbr, ContinuingEducationModel4WS[] contEducationList)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Sets the contact session parameter.
        /// </summary>
        /// <param name="contactParametersModel">The contact parameters model.</param>
        void IACASession.SetContactSessionParameter(ContactSessionParameter contactParametersModel)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Sets the upload file info to session.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="htFileUploadInfos">file Upload Information list.</param>
        void IACASession.SetUploadFileInfoToSession(string moduleName, Dictionary<string, List<FileUploadInfo>> htFileUploadInfos)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Gets the contact session parameter.
        /// </summary>
        /// <returns>The contact session parameter.</returns>
        ContactSessionParameter IACASession.GetContactSessionParameter()
        {
            // not need implement, keep empty.
            return null;
        }

        /// <summary>
        /// Sets the Register contact session parameter.
        /// </summary>
        /// <param name="contactParametersModel">The contact parameters model.</param>
        void IACASession.SetRegisterContactSessionParameter(ContactSessionParameter contactParametersModel)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Gets the register contact session parameter.
        /// </summary>
        /// <returns>The register contact session parameter.</returns>
        ContactSessionParameter IACASession.GetRegisterContactSessionParameter()
        {
            // not need implement, keep empty.
            return null;
        }

        /// <summary>
        /// Sets the cap type associate license certification.
        /// </summary>
        /// <param name="capAssociateLicenseCertification">The cap associate license certification.</param>
        void IACASession.SetCapTypeAssociateLicenseCertification(CapAssociateLicenseCertification4WS capAssociateLicenseCertification)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Gets the cap type associate license certification.
        /// </summary>
        /// <returns>The cap type associate license certification.</returns>
        CapAssociateLicenseCertification4WS IACASession.GetCapTypeAssociateLicenseCertification()
        {
            // not need implement, keep empty.
            return null;
        }

        /// <summary>
        /// Sets selected contact associated examinations/educations/continuing educations to session.Sets selected contact associated examinations/educations/continuing educations to session.
        /// </summary>
        /// <param name="selectedLicenseCertification">The selected license certification.</param>
        void IACASession.SetSelectedContactLicenseCertification(SelectedContactLicenseCertificationModel selectedLicenseCertification)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Gets selected contact associated examinations/educations/continuing educations from session.
        /// </summary>
        /// <returns>The selected contact license certification.</returns>
        SelectedContactLicenseCertificationModel IACASession.GetSelectedContactLicenseCertification()
        {
            // not need implement, keep empty.
            return null;
        }

        /// <summary>
        /// Gets the CapModel from session.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>CapModel object.</returns>
        CapModel4WS IACASession.GetAssociatedParentCapFromSession(string moduleName)
        {
            return null;
        }

        /// <summary>
        /// Sets the CapModel to session. 
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="capModel">the Associated form parent CapModel to be set to session.</param>
        void IACASession.SetAssociatedParentCapToSession(string moduleName, CapModel4WS capModel)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Sets the APO session.
        /// </summary>
        /// <param name="apoSessionParameter">The APO session parameters model.</param>
        void IACASession.SetAPOSessionParameter(APOSessionParameterModel apoSessionParameter)
        {
            // not need implement, keep empty.
        }

        /// <summary>
        /// Gets the APO session.
        /// </summary>
        /// <returns>APO session parameters model</returns>
        APOSessionParameterModel IACASession.GetAPOSessionParameter()
        {
            // not need implement, keep empty.
            return null;
        }

        #endregion Methods
    }
}