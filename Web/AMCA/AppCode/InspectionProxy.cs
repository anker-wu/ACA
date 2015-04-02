#region Header

/**
 * <pre>
 *
 *  Accela Mobile Citizen Access
 *  File: InspectionProxy.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 *
 *  Description:
 *  Methods from ACA 7.0 code modules that have adapted for use by AMCA.
 *
 *  Notes:
 * $Id: InspectionProxy.cs 131474 2009-06-25 02:34:33Z dave.brewster $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  10-05-2009           Dave Brewster           New page added for version 7.0
 * </pre>
 */

#endregion Header
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Html.Inspection;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.Web.Inspection;

/// <summary>
/// Summary description for InspectionProxy
/// </summary>
public class InspectionProxy
{
    public bool OnErrorReturn = false;
    public bool TestReturnError = false;
    public string ExceptionMessage = string.Empty;

    #region DWB - 7.0 - Schedule Inspection - Code Copied From Cap/InspectionSchedule.aspx.cs

    /// <summary>
    /// Do schedule / reschedule / request operation.
    /// </summary>
    /// <param name="inspectionParameter">this inspection parameter</param>
    /// <param name="agency">The agency code.</param>
    /// <param name="moduleName">The module name.</param>
    /// <param name="additionNotes">The addition notes.</param>
    /// <returns>Return ture or false. if message not empty, show that there is error.</returns>
    public Boolean ScheduleInspection(InspectionParameter inspectionParameter, string agency, string moduleName, string additionNotes)
    {
        string message = String.Empty;
        var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
        
        //prepare inspectionModel and array list.
        InspectionModel inspectionModel = AssembleInspectionModel(inspectionParameter, agency, moduleName, additionNotes);
        if (inspectionModel == null)
        {
            return false;
        }

        InspectionModel[] inspectionModelList = new InspectionModel[1];
        inspectionModelList[0] = inspectionModel;

        //prepare actMode
        string actMode = inspectionBll.GetActionMode(inspectionParameter.Action);

        //prepare inspector
        SysUserModel inspector = new SysUserModel();
        inspector.userID = AppSession.User.PublicUserId;
        inspector.agencyCode = agency;

        try
        {
            if (inspectionModel.activity.inAdvanceFlag == ACAConstant.COMMON_Y)
            {
                //validate schedule date according to the inspection flow
                string messageKey = inspectionBll.ValidateScheduleDateByFlow(inspectionModel.activity.capID, inspectionModel);

                if (!String.IsNullOrEmpty(messageKey))
                {
                    message = LabelUtil.GetTextByKey("ins_inspectionList_inadvance_invalid_scheduledate", moduleName);
                    message = String.Format(message, inspectionParameter.Type);
                    OnErrorReturn = true;
                    ExceptionMessage = message;
                    return false;
                }
            }

            inspectionBll.BatchScheduleInspections(agency, inspectionModelList, actMode, inspector);
        }
        catch (ACAException ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
            return false;
        }

        return true;
    }

    /// <summary>
    /// Get Assemble Inspection Model.
    /// </summary>
    /// <param name="inspectionParameter">The inspection parameter.</param>
    /// <param name="agency">The agency code.</param>
    /// <param name="moduleName">The module name.</param>
    /// <param name="additionNotes">The addition notes.</param>
    /// <returns>Return a InspectionModel.</returns>
    private static InspectionModel AssembleInspectionModel(InspectionParameter inspectionParameter, string agency, string moduleName, string additionNotes)
    {
        string time1String = String.Empty;
        string time2String = String.Empty;
        string endtime1String = String.Empty;
        string endtime2String = String.Empty;
        string activityType = String.Empty;
        
        InspectionAction inspectionAction = inspectionParameter.Action;
        var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
        //prepare actMode
        string statusAfterAction = inspectionBll.GetStatusAfterAction(inspectionAction);

        DateTime? activityDate = inspectionParameter.ScheduledDateTime;
        DateTime? endActivityDate = inspectionParameter.EndScheduledDateTime;

        if (activityDate != null && activityDate != DateTime.MinValue)
        {
            InspectionScheduleType scheduleType = inspectionParameter.ScheduleType;
            var readyTimeEnabled = inspectionParameter.ReadyTimeEnabled != null ? inspectionParameter.ReadyTimeEnabled.Value : false;

            if (scheduleType == InspectionScheduleType.ScheduleUsingCalendar
                || scheduleType == InspectionScheduleType.RequestSameDayNextDay
                || (scheduleType == InspectionScheduleType.RequestOnlyPending && readyTimeEnabled)
                || scheduleType == InspectionScheduleType.Unknown)
            {
                InspectionViewUtil.BuildTimeValue(activityDate, inspectionParameter.TimeOption, out time1String, out time2String);
                InspectionViewUtil.BuildTimeValue(endActivityDate, inspectionParameter.TimeOption, out endtime1String, out endtime2String);
            }
        }

        //prepare activityType
        activityType = inspectionParameter.Type;

        //prepare CapIDModel
        CapIDModel4WS capIDModel = new CapIDModel4WS();
        capIDModel.id1 = inspectionParameter.RecordID1;
        capIDModel.id2 = inspectionParameter.RecordID2;
        capIDModel.id3 = inspectionParameter.RecordID3;
        capIDModel.serviceProviderCode = inspectionParameter.AgencyCode;

        InspectionModel inspectionmodel = new InspectionModel();

        CommentModel commentModel = new CommentModel();
        commentModel.text = additionNotes;
        inspectionmodel.comment = commentModel;

        ActivityModel activityModel = new ActivityModel();
        activityModel.capID = TempModelConvert.Trim4WSOfCapIDModel(capIDModel);
        activityModel.activityDate = activityDate;
        activityModel.time1 = time1String;
        activityModel.time2 = time2String;
        activityModel.auditID = AppSession.User.PublicUserId;
        activityModel.auditDate = DateTime.Now;
        activityModel.auditStatus = ACAConstant.VALID_STATUS;
        activityModel.activityType = activityType;
        activityModel.serviceProviderCode = agency;
        activityModel.inAdvanceFlag = inspectionParameter.InAdvance == true ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

        activityModel.estimatedStartTime = FormatEstimatedTime(time1String, time2String);
        activityModel.estimatedEndTime = FormatEstimatedTime(endtime1String, endtime2String);

        if (AppSession.User != null && AppSession.User.ApprovedContacts != null)
        {
            var userContact = AppSession.User.ApprovedContacts.Length == 1
                                  ? AppSession.User.ApprovedContacts[0]
                                  : null;

            activityModel.requestorFname = userContact == null ? string.Empty : userContact.firstName;
            activityModel.requestorMname = userContact == null ? string.Empty : userContact.middleName;
            activityModel.requestorLname = userContact == null ? string.Empty : userContact.lastName;
        }

        activityModel.requestorUserID = AppSession.User.UserID;
        activityModel.contactFname = inspectionParameter.ContactFirstName;
        activityModel.contactMname = inspectionParameter.ContactMiddleName;
        activityModel.contactLname = inspectionParameter.ContactLastName;

        //prepare inspectionId
        long inspectionId = 0;
        long.TryParse(inspectionParameter.ID, out inspectionId);

        activityModel.idNumber = inspectionId;

        long sequenceNumber = 0;
        long.TryParse(inspectionParameter.TypeID, out sequenceNumber);

        activityModel.inspSequenceNumber = sequenceNumber;
        activityModel.status = statusAfterAction;
        activityModel.activityDescription = activityType;
        activityModel.requiredInspection = inspectionParameter.Required == true ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

        activityModel.contactPhoneNumIDD = inspectionParameter.ContactPhoneIDD;
        activityModel.contactPhoneNum = inspectionParameter.ContactPhoneNumber;

        // set the requestor phone number
        if (!string.IsNullOrEmpty(AppSession.User.CellPhone))
        {
            activityModel.reqPhoneNum = AppSession.User.CellPhone;
            activityModel.reqPhoneNumIDD = AppSession.User.CellPhoneCountryCode;
        }
        else if (!string.IsNullOrEmpty(AppSession.User.WorkPhone))
        {
            activityModel.reqPhoneNum = AppSession.User.WorkPhone;
            activityModel.reqPhoneNumIDD = AppSession.User.WorkPhoneCountryCode;
        }
        else if (!string.IsNullOrEmpty(AppSession.User.HomePhone))
        {
            activityModel.reqPhoneNum = AppSession.User.HomePhone;
            activityModel.reqPhoneNumIDD = AppSession.User.HomePhoneCountryCode;
        }

        inspectionmodel.activity = activityModel;

        return inspectionmodel;
    }

    /// <summary>
    /// Formats the estimated time.
    /// </summary>
    /// <param name="time1String">The time1 string.</param>
    /// <param name="time2String">AM/PM value.</param>
    /// <returns>Return the formated estimated time.</returns>
    private static string FormatEstimatedTime(string time1String, string time2String)
    {
        string result = String.Empty;

        if (!String.IsNullOrEmpty(time1String) && !String.IsNullOrEmpty(time2String))
        {
            result = String.Format("{0} {1}", time2String, time1String);
        }
        else if (!String.IsNullOrEmpty(time1String))
        {
            result = time1String;
        }

        return result;
    }
    
    #endregion

    #region DWB - 7.0 - Cancel Inspection - Code Copied From AMCA/AccelaProxy.cs and Cap/InspectionCancelConfirmation.ascx.cs
    /// <summary>
    /// Cancel Inspection Details       
    /// </summary>
    /// <param name="inspectionParameter">this inspeciton parameter</param>
    /// <param name="agency">this agency code</param>
    /// <returns></returns>
    public Boolean CancelInspectionUpdate(InspectionParameter inspectionParameter, string agency)
    {
        try
        {
            SysUserModel inspector = new SysUserModel();
            inspector.userID = AppSession.User.PublicUserId;
            inspector.agencyCode = agency;

            //prepare CapIDModel
            CapIDModel4WS capIDModel = new CapIDModel4WS();
            capIDModel.id1 = inspectionParameter.RecordID1;
            capIDModel.id2 = inspectionParameter.RecordID2;
            capIDModel.id3 = inspectionParameter.RecordID3;
            capIDModel.serviceProviderCode = inspectionParameter.AgencyCode;

            int result = 0;

            ActivityModel newActivity = new ActivityModel();
            //prepare inspectionId
            long inspectionId = 0;
            long.TryParse(inspectionParameter.ID, out inspectionId);
            newActivity.idNumber = inspectionId;
            newActivity.capID = TempModelConvert.Trim4WSOfCapIDModel(capIDModel);
            newActivity.activityGroup = inspectionParameter.Group;
            newActivity.activityType = inspectionParameter.Type;
            newActivity.activityDescription = inspectionParameter.Type;
            newActivity.serviceProviderCode = inspectionParameter.AgencyCode;
            
            //newActivity.scheduled = inspectionEntity.InspectionModel.activity.scheduled;
            //newActivity.signOffWorkflowTask = inspectionEntity.InspectionModel.activity.signOffWorkflowTask;

            InspectionModel[] inspectionlist = new InspectionModel[1];
            InspectionModel newInspection = new InspectionModel();
            newInspection.activity = newActivity;
            inspectionlist[0] = newInspection;

            IInspectionBll inspectionBll = (IInspectionBll)ObjectFactory.GetObject(typeof(IInspectionBll));
            //Cancel schedule inspection by given agency code, public user id, inspection list and inspector
            result = inspectionBll.CancelInspection(inspectionParameter.AgencyCode, AppSession.User.PublicUserId, inspectionlist, inspector);

            return true;
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
            return false;
        }
    }
    #endregion

    #region DWB - 7.1 - Inspection/inspectionType/InspectionCategory lists.

    /// <summary>
    /// Get inspection datatable by cap ID.
    /// </summary>
    /// <returns>a DataTable</returns>
    //DWB - 7.0 = copied from AMCA 6.7 InspectionBLL
    //public List<InspectionViewModel> GetInspectionDataModelsByCapID(CapIDModel4WS capID4WS, QueryFormat4WS queryFormat4WS, string UserSeqNum)
    public List<InspectionViewModel> GetInspectionDataModelsByCapID(string moduleName)
    {
        CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

        var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
        // get inspection locked status of cap
        bool isCapLockedOrHold = ConditionsUtil.IsConditionLockedOrHold(capModel.capID, AppSession.User.UserSeqNum);

        // get inspections details
        var dataModelList = inspectionBll.GetDataModels(capModel.moduleName, capModel, isCapLockedOrHold, AppSession.User.UserModel4WS);
        dataModelList = dataModelList.OrderBy(p => p.LastUpdated).ToList();

        var results = new List<InspectionViewModel>();
        if (dataModelList != null)
        {
            foreach (InspectionDataModel dataModel in dataModelList)
            {
                var viewModel = InspectionViewUtil.BuildViewModel(capModel.moduleName, dataModel);
                results.Add(viewModel);
            }
        }

        return results;
    }

    /// <summary>
    /// Get inspection datatable by cap ID.
    /// </summary>
    /// <returns>a DataTable</returns>
    //DWB - 7.0 = copied from AMCA 6.7 InspectionBLL
    public InspectionTreeNodeModel GetInspectionRelatedTree(long InspectionID, string moduleName)
    {

        CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

        InspectionTreeNodeModel fullTree = null;

        IInspectionBll inspectionBll = (IInspectionBll)ObjectFactory.GetObject(typeof(IInspectionBll));
        fullTree = inspectionBll.GetRelatedInspections(TempModelConvert.Trim4WSOfCapIDModel(capModel.capID), InspectionID.ToString());

        return fullTree;
    }

    /// <summary>
    /// Get inspection coment by cap ID.
    /// </summary>
    /// <returns>a DataTable</returns>
    //DWB - 7.0 = copied from AMCA 6.7 InspectionBLL
    public IList<InspectionViewModel> GetInspectionStatusHistory(long InspectionID, string moduleName)
    {

        CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

        IList<InspectionViewModel> inspectionHistoryViewModels = null;

        IInspectionBll inspectionBll = (IInspectionBll)ObjectFactory.GetObject(typeof(IInspectionBll));
        var inspectionHistoryDataModels = inspectionBll.GetInspectionHistoryList(InspectionID, moduleName, capModel, AppSession.User.UserModel4WS);

        if (inspectionHistoryDataModels != null)
        {
            inspectionHistoryViewModels = new List<InspectionViewModel>();
            foreach (var dataModel in inspectionHistoryDataModels)
            {
                var tempViewModel = InspectionViewUtil.BuildViewModel(moduleName, dataModel);
                inspectionHistoryViewModels.Add(tempViewModel);
            }
        }
        return inspectionHistoryViewModels;
    }

    /// <summary>
    /// Get inspection coment by cap ID.
    /// </summary>
    /// <returns>a DataTable</returns>
    //DWB - 7.0 = copied from AMCA 6.7 InspectionBLL
    public IList<InspectionViewModel> GetInspectionCommentHistory(long InspectionID, string moduleName)
    {

        CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

        IList<InspectionViewModel> inspectionHistoryViewModels = null;

        IInspectionBll inspectionBll = (IInspectionBll)ObjectFactory.GetObject(typeof(IInspectionBll));
        var inspectionHistoryDataModels = inspectionBll.GetInspectionHistoryList(InspectionID, moduleName, capModel, AppSession.User.UserModel4WS);

        if (inspectionHistoryDataModels != null)
        {
            inspectionHistoryViewModels = new List<InspectionViewModel>();
            string previousComment = string.Empty;
            int dummyCounter = 0;
            foreach (var dataModel in inspectionHistoryDataModels)
            {
                if (dataModel.ResultComments != null && dataModel.ResultComments != string.Empty && dataModel.ResultComments.ToString() != previousComment.ToString())
                {
                    var tempViewModel = InspectionViewUtil.BuildViewModel(moduleName, dataModel);
                    inspectionHistoryViewModels.Add(tempViewModel);
                }
            }
        }
        return inspectionHistoryViewModels;
    }


    /// <summary>
    /// Get inspection datatable by cap ID.
    /// </summary>
    /// <returns>a DataTable</returns>
    //DWB - 7.0 = copied from AMCA 6.7 InspectionBLL
    public List<InspectionTypeDataModel> GetInspectionTypeModelsByCapID(CapIDModel4WS capID4WS, string UserSeqNum)
    {
        if (capID4WS == null)
        {
            throw new DataValidateException(new string[] { "capID4WS" });
        }

        ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
        CapModel4WS capModel = capBll.GetCapViewDetailByPK(capID4WS, UserSeqNum);
        CapTypeModel capTypeModel = capModel.capType;

        string callerID = ACAConstant.PUBLIC_USER_NAME + UserSeqNum;

        var results = new List<InspectionViewModel>();
        var inspectionTypeBll = ObjectFactory.GetObject<IInspectionTypeBll>();
        // get inspection list base on Cap ID
        bool isCapLockedOrHold = ConditionsUtil.IsConditionLockedOrHold(capModel.capID, AppSession.User.UserSeqNum);

        var list = inspectionTypeBll.GetAvailableInspectionTypes(capModel.moduleName, capModel, isCapLockedOrHold, AppSession.User.UserModel4WS, true, null, AppSession.User.PublicUserId);

        return list;
    }

    private const string CATEGORY_OTHERS_KEY = "\fOthers\f";

    /// <summary>
    /// Gets the inspection categories.
    /// </summary>
    /// <param name="inspectionTypeViewModels">The inspection type view models.</param>
    /// <param name="recordIDModel">The record ID model.</param>
    /// <returns>the inspection categories.</returns>
    public List<InspectionCategoryDataModel> GetInspectionCategories(List<InspectionTypeDataModel> inspectionTypeViewModels, CapIDModel4WS capID4WS, string UserSeqNum)
    {
        var result = new List<InspectionCategoryDataModel>();
        var inspectionTypeBll = ObjectFactory.GetObject<IInspectionTypeBll>();

        var recordIDModel = TempModelConvert.Trim4WSOfCapIDModel(capID4WS);

        var categoryDataModels = inspectionTypeBll.GetInspectionCategoriesByCapID(recordIDModel);

        //get interset by available categories and categories of inspection types
        if (categoryDataModels != null && inspectionTypeViewModels != null)
        {
            categoryDataModels = (from c in categoryDataModels
                                  from i in inspectionTypeViewModels
                                  where i != null
                                  from c1 in i.Categories
                                  where c.ID == c1.ID
                                  select c).Distinct().ToList();
        }

        //sort categories by display text
        if (categoryDataModels != null)
        {
            categoryDataModels = categoryDataModels.OrderBy(p => p.Category).ToList();
        }

        //check if exist inspections with no category
        bool existInspectionsWithNoCategory =
            inspectionTypeViewModels == null ? false
            :
            (from i in inspectionTypeViewModels
             where i != null
              && (i.Categories == null || i.Categories.Count() == 0)
             select i).Count() > 0;

        //add others category if categories count >0 and exist inspections with no category
        if (categoryDataModels != null && categoryDataModels.Count > 0 && existInspectionsWithNoCategory)
        {
            var othersCategory = new InspectionCategoryDataModel();
            othersCategory.ID = CATEGORY_OTHERS_KEY;
            othersCategory.Category = LabelUtil.GetTextByKey("inspection_category_others", String.Empty);
            othersCategory.DisplayOrder = 99999;
            categoryDataModels.Add(othersCategory);
        }

        if (categoryDataModels != null)
        {
            result = categoryDataModels;
        }

        return result;
    }

    #endregion
}
