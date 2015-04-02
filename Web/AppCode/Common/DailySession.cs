#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DailySession.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DailySession.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;
using System.Web;
using Accela.ACA.BLL.Account;
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
    /// This class is for DailySession
    /// </summary>
    public class DailySession : IACASession
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Breadcrumb information, which is used on apply process.
        /// </summary>
        public BreadCrumbParmsInfo BreadCrumbParmsInfo
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstant.SESSION_BREADCRUMB] == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Current.Session[SessionConstant.SESSION_BREADCRUMB] as BreadCrumbParmsInfo;
                }
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_BREADCRUMB] = value;
            }
        }

        /// <summary>
        /// Gets or sets the inspection information.
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
        /// Gets or sets the inspection information.
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
        /// Gets or sets the selected parcel info, which is used on apply process to fill the address/parcel/owner section.
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
        /// Gets or sets the Breadcrumb information, which is used on shopping cart process.
        /// </summary>
        public BreadCrumbParmsInfo ShoppingCartBreadCrumbParmsInfo
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstant.SESSION_SHOPPING_CART_BREADCRUMB] == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Current.Session[SessionConstant.SESSION_SHOPPING_CART_BREADCRUMB] as BreadCrumbParmsInfo;
                }
            }

            set
            {
                HttpContext.Current.Session[SessionConstant.SESSION_SHOPPING_CART_BREADCRUMB] = value;
            }
        }

        /// <summary>
        /// Gets or sets the current user session.
        /// </summary>
        public User User
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

        #endregion Properties

        #region Methods

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
        /// Gets announcements from session.
        /// </summary>
        /// <returns>announcements object</returns>
        List<AnnouncementModel> IACASession.GetAnnouncementsFromSession()
        {
            return (List<AnnouncementModel>)HttpContext.Current.Session[SessionConstant.SESSION_ANNOUNCEMENT_LIST];
        }

        /// <summary>
        /// Gets announcement flag from session.
        /// </summary>
        /// <returns>The announcement flag.</returns>
        string IACASession.GetAnnouncementFlagFromSession()
        {
            return (string)HttpContext.Current.Session[SessionConstant.SESSION_ANNOUNCEMENT_LIST_FLAG];
        }         

        /// <summary>
        /// Gets the caps id model from session.
        /// </summary>
        /// <returns>CapIDModels object.</returns>
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
            return (CapIDModel4WS)HttpContext.Current.Session[SessionConstant.SESSION_PARENT_CAPID_MODEL + "|" + relationship];
        }

        /// <summary>
        /// Gets the cap model from session.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>CapModel object.</returns>
        CapModel4WS IACASession.GetCapModelFromSession(string moduleName)
        {
            return (CapModel4WS)HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL + "|" + moduleName];
        }

        /// <summary>
        /// get the Cart Item Number From Session
        /// </summary> 
        /// <returns>The shopping cart item number.</returns>
        string IACASession.GetCartItemNumberFromSession()
        {
            if (HttpContext.Current.Session[SessionConstant.SESSION_SHOPPINGCART_ITEMNUMBER] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[SessionConstant.SESSION_SHOPPINGCART_ITEMNUMBER].ToString();
            }
        }

        /// <summary>
        /// Get my collection models from session.
        /// </summary>
        /// <returns>The my collections.</returns>
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
        /// <returns>The OnlinePaymentResultModel.</returns>
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
            return (PageFlowGroupModel)HttpContext.Current.Session[SessionConstant.SESSION_PAGEFLOW_GROUP];
        }

        /// <summary>
        /// Get the parent cap type from session.
        /// </summary>
        /// <returns>CapTypeModel object.</returns>
        CapTypeModel IACASession.GetParentCapTypeFromSession()
        {
            return (CapTypeModel)HttpContext.Current.Session[SessionConstant.SESSION_PARENT_CAPTYPE];
        }

        /// <summary>
        /// Get the parent page flow group from session.
        /// </summary>
        /// <returns>PageFlowGroupModel object.</returns>
        PageFlowGroupModel IACASession.GetParentPageflowGroupFromSession()
        {
            return (PageFlowGroupModel)HttpContext.Current.Session[SessionConstant.SESSION_PARENT_PAGEFLOW_GROUP];
        }

        /// <summary>
        /// gets the PaymentResultModel4WS from session. Them were returned from creating real caps .
        /// </summary>
        /// <param name="moduleName">The module name</param>
        /// <returns>The PaymentResultModel4WS list</returns>
        PaymentResultModel4WS[] IACASession.GetPaymentResultModelsFromSession(string moduleName)
        {
            return (PaymentResultModel4WS[])HttpContext.Current.Session[SessionConstant.SESSION_PAYMENTRESULT_MODELS + "|" + moduleName];
        }

        /// <summary>
        /// Get report parameters from session.
        /// </summary>
        /// <returns>The PaymentResultModel list.</returns>
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
        /// Gets the attachment upload file info
        /// </summary>
        /// <param name="moduleName">The module name</param>
        /// <returns>The upload file information.</returns>
        Dictionary<string, List<FileUploadInfo>> IACASession.GetUploadFileInfoFromSession(string moduleName)
        {
            return (Dictionary<string, List<FileUploadInfo>>)HttpContext.Current.Session[SessionConstant.SESSION_ATTACHMENT_UPLOADFILEINFO + "|" + moduleName];
        }

        /// <summary>
        /// Get the UI data from session.
        /// </summary>
        /// <returns>A hashtable contains the all UI data.</returns>
        Hashtable IACASession.GetUIDataFromSession()
        {
            return HttpContext.Current.Session[SessionConstant.SESSION_UI_DATA] as Hashtable;
        }

        /// <summary>
        /// Gets the asset search model from session.
        /// </summary>
        /// <returns>
        /// The Asset search model
        /// </returns>
        AssetMasterModel IACASession.GetAssetSearchModelFromSession()
        {
            return HttpContext.Current.Session[SessionConstant.SESSION_ASSET_SEARCH_MODEL] as AssetMasterModel;
        }

        /// <summary>
        /// Sets the cap id models to session. 
        /// </summary> 
        /// <param name="capIDs">the capIDs to be set to session.</param>
        void IACASession.SetCapIDModelsToSession(CapIDModel4WS[] capIDs)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_CAPID_MODELS] = capIDs;
        }

        /// <summary>
        /// Set the parent Cap ID model to Session.
        /// </summary>
        /// <param name="relationship">It may be <c>"R", "EST", "Renewal", "Amendment" or "AssoForm"</c>.</param>
        /// <param name="parentCapID">parent Cap ID model</param>
        void IACASession.SetParentCapIDModelToSession(string relationship, CapIDModel4WS parentCapID)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_PARENT_CAPID_MODEL + "|" + relationship] = parentCapID;
        }

        /// <summary>
        /// Sets the cap model to session. 
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="capModel">the CapModel to be set to session.</param>
        void IACASession.SetCapModelToSession(string moduleName, CapModel4WS capModel)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_CAP_MODEL + "|" + moduleName] = capModel;
        }

        /// <summary>
        /// Sets the Cart Item Number To Session
        /// </summary> 
        /// <param name="cartNumber">The cart number.</param>
        void IACASession.SetCartItemNumberToSession(string cartNumber)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_SHOPPINGCART_ITEMNUMBER] = cartNumber;
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
        /// <param name="onlinePaymentResultModel">The OnlinePaymentResultModel.</param>
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
        /// <param name="moduleName">The module name</param>
        /// <param name="paymentResultModels">The PaymentResultModel4WS array.</param>
        void IACASession.SetPaymentResultModelsToSession(string moduleName, PaymentResultModel4WS[] paymentResultModels)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_PAYMENTRESULT_MODELS + "|" + moduleName] = paymentResultModels;
        }

        /// <summary>
        /// Set report parameters to session.
        /// </summary>
        /// <param name="parameters">The parameter models.</param>
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
        /// Gets the people model from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the people object.</returns>
        PeopleModel IACASession.GetPeopleModelFromSession(string contactSeqNbr)
        {
            return (PeopleModel)HttpContext.Current.Session[SessionConstant.SESSION_PEOPLE + "|" + contactSeqNbr];
        }

        /// <summary>
        /// Sets the people model to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="peopleModel">The people model.</param>
        void IACASession.SetPeopleModelToSession(string contactSeqNbr, PeopleModel peopleModel)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_PEOPLE + "|" + contactSeqNbr] = peopleModel;
        }

        /// <summary>
        /// Sets the asset search model to session.
        /// </summary>
        /// <param name="assetMasterModel">The asset master model.</param>
        void IACASession.SetAssetSearchModelToSession(AssetMasterModel assetMasterModel)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_ASSET_SEARCH_MODEL] = assetMasterModel;
        }

        /// <summary>
        /// Get the expression info from session.
        /// </summary>
        /// <returns>A hashtable contains the all expression data.</returns>
        Hashtable IACASession.GetExpressionDataFromSession()
        {
            return HttpContext.Current.Session[SessionConstant.SESSION_EXPRESSION_DATA] as Hashtable;
        }

        /// <summary>
        /// Set the expression info to session.
        /// </summary>
        /// <param name="uiData">A hashtable contains the all expression data.</param>
        void IACASession.SetExpressionDataToSession(Hashtable uiData)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_EXPRESSION_DATA] = uiData;
        }

        /// <summary>
        /// Gets the reference contact education list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference education list object.</returns>
        EducationModel4WS[] IACASession.GetContactEducationListFromSession(string contactSeqNbr)
        {
            return (EducationModel4WS[])HttpContext.Current.Session[SessionConstant.SESSION_REF_CONTACT_EDUCATIONS + "|" + contactSeqNbr];
        }

        /// <summary>
        /// Sets the reference contact education list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="educationList">The education list.</param>
        void IACASession.SetContactEducationListToSession(string contactSeqNbr, EducationModel4WS[] educationList)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_REF_CONTACT_EDUCATIONS + "|" + contactSeqNbr] = educationList;
        }

        /// <summary>
        /// Gets the reference contact examination list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference examination list object.</returns>
        ExaminationModel[] IACASession.GetContactExaminationListFromSession(string contactSeqNbr)
        {
            return (ExaminationModel[])HttpContext.Current.Session[SessionConstant.SESSION_REF_CONTACT_EXAMINATIONS + "|" + contactSeqNbr];
        }

        /// <summary>
        /// Sets the reference contact examination list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="examinationList">The examination list.</param>
        void IACASession.SetContactExaminationListToSession(string contactSeqNbr, ExaminationModel[] examinationList)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_REF_CONTACT_EXAMINATIONS + "|" + contactSeqNbr] = examinationList;
        }

        /// <summary>
        /// Gets the reference contact education list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference education list object.</returns>
        ContinuingEducationModel4WS[] IACASession.GetContactContEducationListFromSession(string contactSeqNbr)
        {
            return (ContinuingEducationModel4WS[])HttpContext.Current.Session[SessionConstant.SESSION_REF_CONTACT_CONT_EDUCATIONS + "|" + contactSeqNbr];
        }

        /// <summary>
        /// Sets the reference contact continuing education list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="conteducationList">The continuing education list.</param>
        void IACASession.SetContactContEducationListToSession(string contactSeqNbr, ContinuingEducationModel4WS[] conteducationList)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_REF_CONTACT_CONT_EDUCATIONS + "|" + contactSeqNbr] = conteducationList;
        }

        /// <summary>
        /// Sets the contact session parameter.
        /// </summary>
        /// <param name="contactParametersModel">The contact parameters model.</param>
        void IACASession.SetContactSessionParameter(ContactSessionParameter contactParametersModel)
        {
            HttpContext.Current.Session[SessionConstant.CONTACT_SESSION_PARAMETER] = contactParametersModel;
        }

        /// <summary>
        /// Gets the contact session parameter.
        /// </summary>
        /// <returns>The contact session parameter.</returns>
        ContactSessionParameter IACASession.GetContactSessionParameter()
        {
            return HttpContext.Current.Session[SessionConstant.CONTACT_SESSION_PARAMETER] as ContactSessionParameter;
        }

        /// <summary>
        /// Sets the contact session parameter.
        /// </summary>
        /// <param name="contactParametersModel">The contact parameters model.</param>
        void IACASession.SetRegisterContactSessionParameter(ContactSessionParameter contactParametersModel)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_REGISTER_PEPOLE_MODEL] = contactParametersModel;
        }

        /// <summary>
        /// Gets the register contact session parameter.
        /// </summary>
        /// <returns>The register contact session parameter.</returns>
        ContactSessionParameter IACASession.GetRegisterContactSessionParameter()
        {
            return HttpContext.Current.Session[SessionConstant.SESSION_REGISTER_PEPOLE_MODEL] as ContactSessionParameter;
        }

        /// <summary>
        /// Sets the cap type associate license certification.
        /// </summary>
        /// <param name="capAssociateLicenseCertification">The cap associate license certification.</param>
        void IACASession.SetCapTypeAssociateLicenseCertification(CapAssociateLicenseCertification4WS capAssociateLicenseCertification)
        {
            HttpContext.Current.Session[SessionConstant.CAP_ASSOCIATE_LICENSE_CERTIFICATION] = capAssociateLicenseCertification;
        }

        /// <summary>
        /// Gets the cap type associate license certification.
        /// </summary>
        /// <returns>The cap type associate license certification.</returns>
        CapAssociateLicenseCertification4WS IACASession.GetCapTypeAssociateLicenseCertification()
        {
            return HttpContext.Current.Session[SessionConstant.CAP_ASSOCIATE_LICENSE_CERTIFICATION] as CapAssociateLicenseCertification4WS;
        }

        /// <summary>
        /// Sets selected contact associated examinations/educations/continuing educations to session.
        /// </summary>
        /// <param name="selectedLicenseCertification">The selected license certification.</param>
        void IACASession.SetSelectedContactLicenseCertification(SelectedContactLicenseCertificationModel selectedLicenseCertification)
        {
            HttpContext.Current.Session[SessionConstant.SELECTED_CONTACT_LICENSE_CERTIFICATION] = selectedLicenseCertification;
        }

        /// <summary>
        /// Gets selected contact associated examinations/educations/continuing educations from session.
        /// </summary>
        /// <returns>The selected contact license certification.</returns>
        SelectedContactLicenseCertificationModel IACASession.GetSelectedContactLicenseCertification()
        {
            return HttpContext.Current.Session[SessionConstant.SELECTED_CONTACT_LICENSE_CERTIFICATION] as SelectedContactLicenseCertificationModel;
        }

        /// <summary>
        /// Gets the CapModel from session.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>CapModel object.</returns>
        CapModel4WS IACASession.GetAssociatedParentCapFromSession(string moduleName)
        {
            return (CapModel4WS)HttpContext.Current.Session[SessionConstant.SESSION_ASSOCIATED_PARENT_CAP_MODEL + "|" + moduleName];
        }

        /// <summary>
        /// Sets the CapModel to session. 
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="capModel">the Associated form parent CapModel to be set to session.</param>
        void IACASession.SetAssociatedParentCapToSession(string moduleName, CapModel4WS capModel)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_ASSOCIATED_PARENT_CAP_MODEL + "|" + moduleName] = capModel;
        }

        /// <summary>
        /// Sets the upload file info to session.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="htFileUploadInfos">file Upload Information list.</param>
        void IACASession.SetUploadFileInfoToSession(string moduleName, Dictionary<string, List<FileUploadInfo>> htFileUploadInfos)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_ATTACHMENT_UPLOADFILEINFO + "|" + moduleName] = htFileUploadInfos;
        }

        /// <summary>
        /// Sets the Condition Additional Information to Session.
        /// </summary>
        /// <param name="additionalInfo">The Condition Additional Info List</param>
        void IACASession.SetConditionAdditionalInfoToSession(Dictionary<string, string> additionalInfo)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_CONDITION_ADDITIONAL_INFORMATION] = additionalInfo;        
        }

        /// <summary>
        /// Gets the Condition Additional Information from Session.
        /// </summary>
        /// <returns>The condition additional information from session.</returns>
        Dictionary<string, string> IACASession.GetConditionAdditionalInfoFromSession()
        {
            return HttpContext.Current.Session[SessionConstant.SESSION_CONDITION_ADDITIONAL_INFORMATION] as Dictionary<string, string>;
        }

        /// <summary>
        /// Sets the APO session.
        /// </summary>
        /// <param name="apoSessionParameter">The APO session parameters model.</param>
        void IACASession.SetAPOSessionParameter(APOSessionParameterModel apoSessionParameter)
        {
            HttpContext.Current.Session[SessionConstant.APO_SESSION_PARAMETER] = apoSessionParameter;
        }

        /// <summary>
        /// Gets the APO session.
        /// </summary>
        /// <returns>APO session parameters model</returns>
        APOSessionParameterModel IACASession.GetAPOSessionParameter()
        {
            return HttpContext.Current.Session[SessionConstant.APO_SESSION_PARAMETER] as APOSessionParameterModel;
        }

        #endregion Methods
    }
}
