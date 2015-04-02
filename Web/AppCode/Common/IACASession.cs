#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IACASession.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: IACASession.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;
using Accela.ACA.BLL.Account;
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
    /// It provides the ACA session interface.
    /// </summary>
    public interface IACASession
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Breadcrumb information, which is used on apply process.
        /// </summary>
        BreadCrumbParmsInfo BreadCrumbParmsInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the inspection data.
        /// </summary>
        List<InspectionListItemViewModel> InspectionData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the examination data.
        /// </summary>
        List<ExaminationListItemViewModel> ExaminationData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the inspection parameters.
        /// </summary>
        /// <value>
        /// The inspection parameters.
        /// </value>
        Hashtable InspectionParameters
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the examination parameters.
        /// </summary>
        /// <value>The examination parameters.</value>
        Hashtable ExaminationParameters
        {
            get;
            set;
        }

        /// <summary> 
        /// Gets or sets the parcel Info model, which is used on apply process.
        /// </summary>
        ParcelInfoModel SelectedParcelInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Breadcrumb information, which is used on shopping cart process.
        /// </summary>
        BreadCrumbParmsInfo ShoppingCartBreadCrumbParmsInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current user session.
        /// </summary>
        User User
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HistoryAction session.
        /// </summary>
        HistoryAction HistoryAction
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets announcements from session.
        /// </summary>
        /// <returns>announcements object</returns>
        List<AnnouncementModel> GetAnnouncementsFromSession();

        /// <summary>
        /// Gets announcement flag from session.
        /// </summary>
        /// <returns>The announcement flag.</returns>
        string GetAnnouncementFlagFromSession();

        /// <summary>
        /// Sets announcements to session
        /// </summary>
        /// <param name="announcements">the announcement list</param>
        void SetAnnouncementsToSession(List<AnnouncementModel> announcements);

        /// <summary>
        /// Sets announcement flag to session
        /// </summary>
        /// <param name="flag">the announcement flag.</param>
        void SetAnnouncementFlagToSession(string flag);

        /// <summary>
        /// Gets the caps id model from session.
        /// </summary>
        /// <returns>CapIDModels object.</returns>
        CapIDModel4WS[] GetCapIDModelsFromSession();

        /// <summary>
        /// Get the parent Cap ID model from Session.
        /// </summary>
        /// <param name="relationship">It may be <c>"R", "EST", "Renewal", "Amendment" or "AssoForm"</c>.</param>
        /// <returns>parent Cap ID model</returns>
        CapIDModel4WS GetParentCapIDModelFromSession(string relationship);

        /// <summary>
        /// Gets the cap model from session.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>CapModel object.</returns>
        CapModel4WS GetCapModelFromSession(string moduleName);

        /// <summary>
        /// get the Cart Item Number From Session
        /// </summary> 
        /// <returns>The shopping cart item number.</returns>
        string GetCartItemNumberFromSession();

        /// <summary>
        /// Get my collection models from session.
        /// </summary>
        /// <returns>The my collections.</returns>
        MyCollectionModel[] GetMyCollectionsFromSession();

        /// <summary>
        /// get OnlinePaymentResultModel from session. Them were returned from creating real caps .
        /// </summary> 
        /// <returns>The OnlinePaymentResultModel</returns>
        OnlinePaymentResultModel GetOnlinePaymentResultModelFromSession();

        /// <summary>
        /// Gets the page flow group from session.
        /// </summary>
        /// <returns>PageFlowGroupModel object.</returns>
        PageFlowGroupModel GetPageflowGroupFromSession();

        /// <summary>
        /// Get the parent cap type from session.
        /// </summary>
        /// <returns>CapTypeModel object.</returns>
        CapTypeModel GetParentCapTypeFromSession();

        /// <summary>
        /// Get the parent page flow group from session.
        /// </summary>
        /// <returns>PageFlowGroupModel object.</returns>
        PageFlowGroupModel GetParentPageflowGroupFromSession();

        /// <summary>
        /// gets PaymentResultModel4WSs from session. Them were returned from creating real caps .
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>The PaymentResultModel list.</returns>
        PaymentResultModel4WS[] GetPaymentResultModelsFromSession(string moduleName);

        /// <summary>
        /// Get report parameters from session.
        /// </summary>
        /// <returns>The PaymentResultModel list.</returns>
        ParameterModel4WS[] GetReportParameterFromSession();

        /// <summary>
        /// Gets the selected services from session.
        /// </summary>
        /// <returns>ServiceManagementModel4WS list.</returns>
        ServiceModel[] GetSelectedServicesFromSession();

        /// <summary>
        /// Get the flag which current user whether has Provider License from session.
        /// </summary>
        /// <returns>Indicate the current user whether has Provider License.</returns>
        int GetHasProviderLicenseFlagFromSession();

        /// <summary>
        /// Get the UI data from session.
        /// </summary>
        /// <returns>A hashtable contains the all UI data.</returns>
        Hashtable GetUIDataFromSession();

        /// <summary>
        /// Gets the asset search model from session.
        /// </summary>
        /// <returns>The Asset search model</returns>
        AssetMasterModel GetAssetSearchModelFromSession();

        /// <summary>
        /// Gets the attachment upload file info
        /// </summary>
        /// <param name="moduleName">The module name</param>
        /// <returns>The upload file information.</returns>
        Dictionary<string, List<FileUploadInfo>> GetUploadFileInfoFromSession(string moduleName);

        /// <summary>
        /// Sets the cap id models to session. 
        /// </summary> 
        /// <param name="capIDs">the CapIDModel to be set to session.</param>
        void SetCapIDModelsToSession(CapIDModel4WS[] capIDs);

        /// <summary>
        /// Set the parent Cap ID model to Session.
        /// </summary>
        /// <param name="relationship">It may be <c>"R", "EST", "Renewal", "Amendment" or "AssoForm"</c>.</param>
        /// <param name="parentCapID">parent Cap ID model</param>
        void SetParentCapIDModelToSession(string relationship, CapIDModel4WS parentCapID);

        /// <summary>
        /// Sets the cap model to session. 
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="capModel">the CapModel to be set to session.</param>
        void SetCapModelToSession(string moduleName, CapModel4WS capModel);

        /// <summary>
        /// Sets the Cart Item Number To Session
        /// </summary>
        /// <param name="cartNumber">The shopping cart number.</param>
        void SetCartItemNumberToSession(string cartNumber);

        /// <summary>
        /// Set my collection models to session.
        /// </summary>
        /// <param name="myCollectionModelList">my collection model list</param>
        void SetMyCollectionsToSession(MyCollectionModel[] myCollectionModelList);

        /// <summary>
        /// Set OnlinePaymentResultModel to session,them were returned from creating real caps.
        /// </summary>
        /// <param name="onlinePaymentResultModel">The online payment result model</param>
        void SetOnlinePaymentResultModelToSession(OnlinePaymentResultModel onlinePaymentResultModel);

        /// <summary>
        /// Sets the cap model to session. 
        /// </summary>
        /// <param name="pageflow">the PageFlowGroupModel to be set to session.</param>
        void SetPageflowGroupToSession(PageFlowGroupModel pageflow);

        /// <summary>
        /// Set the parent cap type model to session. 
        /// </summary>
        /// <param name="capType">the CapTypeModel to be set to session.</param>
        void SetParentCapTypeToSession(CapTypeModel capType);

        /// <summary>
        /// Set the parent page flow group model to session. 
        /// </summary>
        /// <param name="parentPageflow">the PageFlowGroupModel to be set to session.</param>
        void SetParentPageflowGroupToSession(PageFlowGroupModel parentPageflow);

        /// <summary>
        /// sets PaymentResultModel4WSs to session,them were returned from creating real caps.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="paymentResultModels">The PaymentResultModel4WSs</param>
        void SetPaymentResultModelsToSession(string moduleName, PaymentResultModel4WS[] paymentResultModels);

        /// <summary>
        /// Set report parameters to session.
        /// </summary>
        /// <param name="parameters">The ParameterModel4WS list.</param>
        void SetReportParameterToSession(ParameterModel4WS[] parameters);

        /// <summary>
        /// Sets the selected services to session. 
        /// </summary>
        /// <param name="services">the selected services to be set to session.</param>
        void SetSelectedServicesToSession(ServiceModel[] services);

        /// <summary>
        /// Sets the flag which current user whether has Provider License to session.
        /// </summary>
        /// <param name="hasProviderLicenseFlag">indicate current user whether has Provider License.</param>
        void SetHasProviderLicenseFlagToSession(int hasProviderLicenseFlag);

        /// <summary>
        /// Set UI data to session.
        /// </summary>
        /// <param name="uiData">A hashtable contains the all UI data.</param>
        void SetUIDataToSession(Hashtable uiData);

        /// <summary>
        /// Gets the people model from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the people object.</returns>
        PeopleModel GetPeopleModelFromSession(string contactSeqNbr);

        /// <summary>
        /// Sets the people model to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="peopleModel">The people model.</param>
        void SetPeopleModelToSession(string contactSeqNbr, PeopleModel peopleModel);

        /// <summary>
        /// Sets the asset search model to session.
        /// </summary>
        /// <param name="assetMasterModel">The asset master model.</param>
        void SetAssetSearchModelToSession(AssetMasterModel assetMasterModel);

        /// <summary>
        /// Get the expression info from session.
        /// </summary>
        /// <returns>A hashtable contains the all expression data.</returns>
        Hashtable GetExpressionDataFromSession();

        /// <summary>
        /// Set the expression info to session.
        /// </summary>
        /// <param name="expressionData">A hashtable contains the all expression data.</param>
        void SetExpressionDataToSession(Hashtable expressionData);

        /// <summary>
        /// Gets the reference contact education list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference education list object.</returns>
        EducationModel4WS[] GetContactEducationListFromSession(string contactSeqNbr);

        /// <summary>
        /// Gets the reference contact continuing education list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The continuing contact sequence number.</param>
        /// <returns>Return the reference continuing education list object.</returns>
        ContinuingEducationModel4WS[] GetContactContEducationListFromSession(string contactSeqNbr);

        /// <summary>
        /// Sets the reference contact education list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="educationList">The education list.</param>
        void SetContactEducationListToSession(string contactSeqNbr, EducationModel4WS[] educationList);

        /// <summary>
        /// Gets the reference contact examination list from session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>Return the reference examination list object.</returns>
        ExaminationModel[] GetContactExaminationListFromSession(string contactSeqNbr);

        /// <summary>
        /// Sets the reference contact examination list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="examinationList">The examination list.</param>
        void SetContactExaminationListToSession(string contactSeqNbr, ExaminationModel[] examinationList);

        /// <summary>
        /// Sets the reference contact continuing education list to session.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <param name="contEducationList">The continuing education list.</param>
        void SetContactContEducationListToSession(string contactSeqNbr, ContinuingEducationModel4WS[] contEducationList);

        /// <summary>
        /// Sets the contact session parameter.
        /// </summary>
        /// <param name="contactParametersModel">The contact parameters model.</param>
        void SetContactSessionParameter(ContactSessionParameter contactParametersModel);

        /// <summary>
        /// Gets the contact session parameter.
        /// </summary>
        /// <returns>The contact session parameter.</returns>
        ContactSessionParameter GetContactSessionParameter();

        /// <summary>
        /// Sets the register contact session parameter.
        /// </summary>
        /// <param name="contactParametersModel">The contact parameters model.</param>
        void SetRegisterContactSessionParameter(ContactSessionParameter contactParametersModel);

        /// <summary>
        /// Gets the register contact session parameter.
        /// </summary>
        /// <returns>The register contact session parameter.</returns>
        ContactSessionParameter GetRegisterContactSessionParameter();

        /// <summary>
        /// Sets the cap type associate license certification.
        /// </summary>
        /// <param name="capAssociateLicenseCertification">The cap associate license certification.</param>
        void SetCapTypeAssociateLicenseCertification(CapAssociateLicenseCertification4WS capAssociateLicenseCertification);

        /// <summary>
        /// Gets the cap type associate license certification.
        /// </summary>
        /// <returns>The cap type associate license certification.</returns>
        CapAssociateLicenseCertification4WS GetCapTypeAssociateLicenseCertification();

        /// <summary>
        /// Sets selected contact associated examinations/educations/continuing educations to session.
        /// </summary>
        /// <param name="selectedLicenseCertification">The selected license certification.</param>
        void SetSelectedContactLicenseCertification(SelectedContactLicenseCertificationModel selectedLicenseCertification);

        /// <summary>
        /// Gets selected contact associated examinations/educations/continuing educations from session.
        /// </summary>
        /// <returns>The selected contact license certification.</returns>
        SelectedContactLicenseCertificationModel GetSelectedContactLicenseCertification();

        /// <summary>
        /// Get the associated form parent CapModel from Session
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The associated parent cap.</returns>
        CapModel4WS GetAssociatedParentCapFromSession(string moduleName);

        /// <summary>
        /// Sets the associated form cap CapModel to session. 
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="capModel">the associated form parent CapModel to be set to session.</param>
        void SetAssociatedParentCapToSession(string moduleName, CapModel4WS capModel);

        /// <summary>
        /// Sets the Condition Additional Information to Session.
        /// </summary>
        /// <param name="additionalInfo">The Condition Additional Information List</param>
        void SetConditionAdditionalInfoToSession(Dictionary<string, string> additionalInfo);

        /// <summary>
        /// Sets the upload file info to session.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="htFileUploadInfos">file Upload Information list.</param>
        void SetUploadFileInfoToSession(string moduleName, Dictionary<string, List<FileUploadInfo>> htFileUploadInfos);

        /// <summary>
        /// Gets the Condition Additional Information from Session.
        /// </summary>
        /// <returns>The condition additional information from session.</returns>
        Dictionary<string, string> GetConditionAdditionalInfoFromSession();

        /// <summary>
        /// Sets the APO session.
        /// </summary>
        /// <param name="apoSessionParameter">The APO session parameters model.</param>
        void SetAPOSessionParameter(APOSessionParameterModel apoSessionParameter);

        /// <summary>
        /// Gets the APO session.
        /// </summary>
        /// <returns>APO session parameters model</returns>
        APOSessionParameterModel GetAPOSessionParameter();

        #endregion Methods
    }
}