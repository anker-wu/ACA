/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AccelaProxy.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 *  All of global session objects should be definted in this class.
 *  Notes:
 *      $Id: AppSession.cs 77905 2007-10-15 12:49:28Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  07-18-2008           DWB                    2008 Mobile ACA interface redesign
 *                                              Added parameter to function LocationSearch.
 *                                              Modified location search to capture FullAddress, FullOwnerName, and ParcelNumber.
 *  10-04-2008           DWB                    Added procedure IsUserEmailValid
 * 04/01/2009            Dave Brewster          Added code to clear related CAPS list when only one CAP id is returned.
 *                                              And, added code to add the AltID to the data row.
 *  12/03/2009           Dave Brewster          Corrected Hide Inspection Times in ACA logic to implenent 
 *                                              block by "Day" logic.
 * 
* </pre>
 */
using System.Collections.Generic;
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Security.Authentication;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

using Accela.ACA.BLL;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Inspection;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.Common;
using Accela.ACA.WSProxy.WSModel;
using System.Text;
// using System.Web.UI.MobileControls;
using Accela.ACA.Html.Inspection;
using Accela.ACA.Common.Util;

/// <summary>
/// AccelaProxy Class contains all the methods to be exposed/integrated with Accela API with UI layer
/// </summary>
public class AccelaProxy
{
    #region Fields

    private const string DATATIME_FORMAT = "MM/dd/yyyy HH:mm:ss";
    private const string DATE_FORMAT = "MM/dd/yyyy";

    /// <summary>
    /// command cancel.
    /// </summary>
    private const string COMMAND_CANCEL = "cancel";

    /// <summary>
    /// command schedule.
    /// </summary>
    private const string COMMAND_SCHEDULE = "schedule";

    /// <summary>
    /// inspection list.
    /// </summary>
    private const string INSPECTION_LIST = "InspectionList";

    /// <summary>
    /// optional status.
    /// </summary>
    private const string OPTIONAL_STATUS = "Optional";

    /// <summary>
    /// required status.
    /// </summary>
    private const string REQUIRED_STATUS = "Required";

    /// <summary>
    /// result approved.
    /// </summary>
    private const string RESULT_APPROVED = "APPROVED";

    /// <summary>
    /// result pending.
    /// </summary>
    private const string RESULT_PENDING = "PENDING";

    /// <summary>
    /// result scheduled.
    /// </summary>
    private const string RESULT_SCHEDULED = "SCHEDULED";

    /// <summary>
    /// long view id
    /// </summary>
    private const long VIEW_ID = 5060;

    // DWB - 7.0 - copied from Components/ScheduleCalendar.ascx.cs

    /// <summary>
    /// block out.
    /// </summary>
    private const string BLOCK_OUT = "BLOCK OUT";

    /// <summary>
    /// block unit day.
    /// </summary>
    private const string BLOCK_UNIT_DAY = "DAY";

    /// <summary>
    /// block unit hour
    /// </summary>
    private const string BLOCK_UNIT_HOUR = "HOUR";

    /// <summary>
    /// block minute.
    /// </summary>
    private const string BLOCK_UNIT_MINUTE = "MINUTE";

    /// <summary>
    /// day after tomorrow.
    /// </summary>
    private const string DAY_AFTER_TOMORROW = "DAY AFTER TOMORROW";

    /// <summary>
    /// holiday status.
    /// </summary>
    private const string HOLIDAY_STATUS = "HOLIDAY";

    /// <summary>
    /// the next day.
    /// </summary>
    private const string NEXT_DAY = "NEXT DAY";

    /// <summary>
    /// the same day.
    /// </summary>
    private const string SAME_DAY = "SAME DAY";

    /// <summary>
    /// time am pm format.
    /// </summary>
    private const string TIME_AMPM_FORMAT = "hh:mm tt";

    /// <summary>
    /// unavai lable status.
    /// </summary>
    private const string UNAVAILABLE_STATUS = "UNAVAILABLE";

    /// <summary>
    /// week end status.
    /// </summary>
    private const string WEEKEND_STATUS = "WEEKEND";

    public bool OnErrorReturn = false;
    public bool TestReturnError = false;
    public string ExceptionMessage = string.Empty;

    #endregion Fields

    #region ModuleList
    /// <summary>
    /// Bind the tab list.
    /// </summary>
    /// <param name="tabName">string tab name</param>
    public TabItemCollection GetModuleList()
    {
        //DWB - 7.0 - this method is a combination of component/Navigation.ascx.cs methods
        //            BindTabs and GetTabsList
        ACAUserType userType = ACAUserType.Registered;
        IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
        IList<TabItem> tabList = bizBll.GetTabsList(ConfigManager.AgencyCode, userType, false);
        if (tabList == null)
        {
            return null;
        }
        TabItemCollection list = new TabItemCollection();
        foreach (TabItem tabItem in tabList)
        {
            if (tabItem.TabVisible == true && tabItem.Module != null && tabItem.Module != string.Empty)
            {
                TabItem tab = new TabItem();
                tab.Key = tabItem.Key;
                tab.Label = tabItem.Label;
                tab.Title = LabelUtil.GetTextByKey(tabItem.Label, tabItem.Module);
                tab.Url = tabItem.Url;
                tab.Order = tabItem.Order;
                tab.Module = tabItem.Module;
                list.Add(tab);
            }
        }
        return list;
    }
    #endregion

    #region user login validation
    
    /// <summary>
    /// User Login Validation 
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Psswrd"></param>
    /// <returns></returns>
    public bool IsUserLoginValid(string UserName, string Psswrd)
    {
        try
        {
            bool isUserLoginSuccessfull = Membership.ValidateUser(UserName, Psswrd);

            bool isLdapEnabled = StandardChoiceUtil.IsEnableLdapAuthentication();
            User user = AppSession.User;

            bool needChangePassword = user != null && user.UserModel4WS != null &&
                                      (ValidationUtil.IsYes(user.UserModel4WS.needChangePassword) ||
                                       AccountUtil.IsPasswordExpiration(user.UserModel4WS));

            /*
             * Use the internal authentication and needs to change password.
             */
            if (!isLdapEnabled && needChangePassword)
            {
                bool isPasswordExpires = ACAConstant.COMMON_Y.Equals(user.UserModel4WS.needChangePassword) ? false : true;

                if (isPasswordExpires)
                {
                    ExceptionMessage = GetTextByKey("aca_change_password_label_expiremessage", null);
                }
                else
                {
                    ExceptionMessage = GetTextByKey("password_update_alert_message", null);
                }

                return false;
            }

            return isUserLoginSuccessfull;
        }
        catch (ConfigurationErrorsException)
        {
            OnErrorReturn = true;
            ExceptionMessage = GetTextByKey("aca_system_configuration_error", null);
            return false;
        }
        catch (AuthenticationException)
        {
            OnErrorReturn = true;
            ExceptionMessage = GetTextByKey("aca.error.publicuser.login.fail", null);
            return false;
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message;
            return false;
        }
    }

    /// <summary>
    /// Check whether the user is locked according to login failed attempts limit setting.
    /// </summary>
    /// <param name="isAnswerCorrect">is answer correct.</param>
    /// <returns>true: user was locked; otherwise not locked.</returns>
    public bool IsUserLockedByInvalidAnswer(bool isAnswerCorrect)
    {
        try
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            PublicUserModel4WS publicUserModel = AppSession.User.UserModel4WS;
            accountBll.IsLockedUserBySecurityQuestionFail(AgencyCode, publicUserModel.userID, publicUserModel.userSeqNum, isAnswerCorrect);
            return false;
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message;
            return true;
        }
    }

    /// <summary>
    /// Get the text(label message) by label key.
    /// </summary>
    /// <param name="key">label key which is unique.</param>
    /// <param name="moduleName">Module Name</param>
    /// <returns>The text(label message) accroding the key.if can't find the key, return String.Empty.</returns>
    public string GetTextByKey(string key, string moduleName)
    {
        return LabelUtil.GetTextByKey(key, moduleName);
    }
   
    /// <summary>
    /// Get public user by email.
    /// </summary>
    /// <param name="email">email</param>
    /// <returns>public user model.</returns>
    public PublicUserModel4WS GetPublicUserByEmail(string email)
    {
        try
        {
            IAccountBll accountBLL = (IAccountBll)ObjectFactory.GetObject(typeof(IAccountBll));
            PublicUserModel thisUser = accountBLL.GetPublicUserByEmailOrUserID(email);
            if (thisUser.userID != null)
            {
                PublicUserModel4WS thisUser4Ws = TempModelConvert.ConvertToPublicUserModel4WS(thisUser);
                User user = new User();
                user.UserModel4WS = thisUser4Ws;
                AppSession.User = user;
                return thisUser4Ws;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message;
            return null;
        }
    }

    #endregion

    #region related permit

    public DataTable GetRelatedPermits(CapIDModel4WS capIdModel)
    {
        DataTable result = new DataTable();
        ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
        // DWB - 04/31/2009 - VS2008 and ACA 7.0 conversion
        // Change: Added "AppSession.User.UserSeqNum" paramter to this method call.
        ProjectTreeNodeModel4WS capListTree = capBll.GetRelatedCapTree(capIdModel, null, null);

        if (capListTree.children != null && capListTree.children.Length > 0)
        {
            ProjectTreeNodeModel4WS capRelated = capListTree.children[0];
            if (capRelated.children == null || capRelated.children.Length == 0)
            {
                capListTree = null;
            }
        }

        if (capListTree != null && capListTree.children != null)
        {
            result = CreateDataSource(capListTree);
        }
        return result;
    }

    /// <summary>
    /// create data table by given caplist of treeview
    /// </summary>
    /// <param name="capList">ProjectTreeNodeModel4WS model treelist</param>
    /// <returns>datasource for UI</returns>
    private DataTable CreateDataSource(ProjectTreeNodeModel4WS capList)
    {
        DataTable dtCap = CreateTable();

        if (capList.children == null)
        {
            return dtCap;
        }

        int index = 0;

        foreach (ProjectTreeNodeModel4WS capTree in capList.children)
        {
            if (capTree == null)
            {
                continue;
            }

            CapModel4WS cap = capTree.CAP;
            string permitNumber = cap.altID != null ? cap.altID.ToString() : String.Empty;
            string permitType = GetAliasOrCapTypeLabel(cap);
            string status = cap.capStatus != null ? cap.capStatus.ToString() : String.Empty;
            string capStatus = cap.capClass != null ? cap.capClass.ToString() : String.Empty;
            string agencyCode = ACAConstant.AgencyCode;
            if (cap.capID != null && !String.IsNullOrEmpty(cap.capID.serviceProviderCode))
            {
                agencyCode = cap.capID.serviceProviderCode;
            }

            DataRow drCap = dtCap.NewRow();
            drCap["CapIndex"] = index;
            drCap["PermitNumber"] = cap.capID.id1.ToString() + "-" + cap.capID.id2.ToString() + "-" + cap.capID.id3.ToString();
            drCap["AltID"] = cap.altID != null ? cap.altID.ToString() : String.Empty;
            drCap["PermitType"] = permitType;
            drCap["Status"] = status;
            drCap["Class"] = capStatus;
            drCap["capID1"] = cap.capID.id1;
            drCap["capID2"] = cap.capID.id2;
            drCap["capID3"] = cap.capID.id3;
            drCap["ParentPermitNumber"] = DBNull.Value;
            drCap["AgencyCode"] = agencyCode;
            drCap["Module"] = cap.moduleName;
            //DWB- 11-16-2008 - Added address to datatable so it can be displayed
            //                  in the related caps list.
            if (cap.addressModel.displayAddress != null)
            {
                drCap["Address"] = cap.addressModel.displayAddress.ToString();
            }
            if (String.IsNullOrEmpty(cap.fileDate))
            {
                drCap["Date"] = DBNull.Value;
            }
            else
            {
                drCap["Date"] = I18nDateTimeUtil.FormatToDateStringForUI(I18nDateTimeUtil.ParseFromWebService(cap.fileDate));
            }

            dtCap.Rows.Add(drCap);

            if (capTree.children != null)
            {
                dtCap = GetChildCap(dtCap, capTree, permitNumber);
            }
           
            index++;
        }
        //DealData(dtCap, string.Empty, string.Empty);
        return dtCap;
    }
    /// <summary>
    /// create blank structure for cap list of treeview
    /// </summary>
    /// <returns>blank table for cap list</returns>
    private static DataTable CreateTable()
    {
        DataTable dtCap = new DataTable();
        dtCap.Columns.Add(new DataColumn("CapIndex", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Date", typeof(String)));
        dtCap.Columns.Add(new DataColumn("PermitNumber", typeof(String)));

        dtCap.Columns.Add(new DataColumn("ParentPermitNumber", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Indent", typeof(String)));
        dtCap.Columns.Add(new DataColumn("IndexCode", typeof(String)));
        dtCap.Columns.Add(new DataColumn("ImageURL", typeof(String)));

        dtCap.Columns.Add(new DataColumn("PermitType", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Description", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Status", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Class", typeof(String)));
        dtCap.Columns.Add(new DataColumn("capID1", typeof(String)));
        dtCap.Columns.Add(new DataColumn("capID2", typeof(String)));
        dtCap.Columns.Add(new DataColumn("capID3", typeof(String)));
        dtCap.Columns.Add(new DataColumn("AgencyCode", typeof(String)));
        //DWB- 11-16-2008 - Added address to be displayed in related caps list.
        dtCap.Columns.Add(new DataColumn("Address", typeof(String)));
        dtCap.Columns.Add(new DataColumn("AltID", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Module", typeof(String)));

        return dtCap;
    }
    /// <summary>
    /// Create data table by given related caplist of treeview
    /// </summary>
    /// <param name="capList">ProjectTreeNodeModel4WS model treelist</param>
    /// <returns>datasource for UI</returns>
    private DataTable GetChildCap(DataTable dtCap, ProjectTreeNodeModel4WS capTree, string parentPermitNumber)
    {
        if (capTree == null || capTree.children == null || capTree.children.Length == 0)
        {
            return dtCap;
        }

        int index = 0;

        foreach (ProjectTreeNodeModel4WS capChild in capTree.children)
        {
            if (capChild == null)
            {
                continue;
            }

            CapModel4WS capChildren = capChild.CAP;
            string permitNumber = capChildren.altID != null ? capChildren.altID.ToString() : String.Empty;
            string permitType = GetAliasOrCapTypeLabel(capChildren);

            string status = capChildren.capStatus != null ? capChildren.capStatus.ToString() : String.Empty;
            string capStatus = capChildren.capClass != null ? capChildren.capClass.ToString() : String.Empty;

            DataRow drCap = dtCap.NewRow();
            drCap["CapIndex"] = index;
            drCap["PermitNumber"] = capChildren.capID.id1 + "-" + capChildren.capID.id2 + "-" + capChildren.capID.id3;
            drCap["AltID"] = capChildren.altID != null ? capChildren.altID.ToString() : String.Empty;
            drCap["PermitType"] = permitType;
            drCap["Status"] = status;
            drCap["Class"] = capStatus;
            drCap["capID1"] = capChildren.capID.id1;
            drCap["capID2"] = capChildren.capID.id2;
            drCap["capID3"] = capChildren.capID.id3;
            drCap["ParentPermitNumber"] = parentPermitNumber;
            drCap["AgencyCode"] = capChildren.capID.serviceProviderCode;
            drCap["Module"] = capChildren.moduleName;

            if (String.IsNullOrEmpty(capChildren.fileDate))
            {
                drCap["Date"] = DBNull.Value;
            }
            else
            {

                drCap["Date"] = I18nDateTimeUtil.FormatToDateStringForUI(I18nDateTimeUtil.ParseFromWebService(capChildren.fileDate));
            }

            dtCap.Rows.Add(drCap);

            if (capChild.children != null)
            {
                dtCap = GetChildCap(dtCap, capChild, permitNumber);
            }


            index++;
        }

        return dtCap;

    }

    /// <summary>
    /// Get cap type label from CapModel4WS
    /// </summary>
    /// <param name="capModel"></param>
    /// <returns></returns>
    public static string GetAliasOrCapTypeLabel(CapModel4WS capModel)
    {
        string result = string.Empty;

        if (null != capModel && null != capModel.capType)
        {
            result = GetAliasOrCapTypeLabel(capModel.capType);
        }
        return result;
    }

    /// <summary>
    /// Get cap type label from the sequence:
    /// 1.capTypeModel.resAlias
    /// 2.GetCurrentLanguageCapType
    /// 3.capTypeModel.alias
    /// 4.GetMasterLanguageCapType
    /// </summary>
    /// <param name="capTypeModel"></param>
    /// <returns></returns>
    public static string GetAliasOrCapTypeLabel(CapTypeModel capTypeModel)
    {
        if (null == capTypeModel)
            return string.Empty;
        return I18nStringUtil.GetString(string.Format(@"{0}", capTypeModel.resAlias)
            , GetCapTypeLabel(capTypeModel, GetCapTypeOption.OnlyGetCurrentLanguageCapType)
            , string.Format(@"{0}", capTypeModel.alias)
            , GetCapTypeLabel(capTypeModel, GetCapTypeOption.OnlyGetMasterLanguageCapType)
            );
    }

    /// <summary>
    /// Get cap type label according to the GetCapTypeOption enumeration
    /// </summary>
    /// <param name="capTypeModel"></param>
    /// <param name="getCapTypeOption"></param>
    /// <returns></returns>
    public static string GetCapTypeLabel(CapTypeModel capTypeModel, GetCapTypeOption getCapTypeOption)
    {
        if (null == capTypeModel)
            return string.Empty;
        string result = string.Empty;
        //string pattern = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft ? @"{3}{4}{2}{4}{1}{4}{0}" : "{0}{4}{1}{4}{2}{4}{3}";
        string pattern = "{0}{4}{1}{4}{2}{4}{3}";
        bool existsInvalidResourceCapType = string.IsNullOrEmpty(capTypeModel.resGroup)
            || string.IsNullOrEmpty(capTypeModel.resType)
            || string.IsNullOrEmpty(capTypeModel.resSubType)
            || string.IsNullOrEmpty(capTypeModel.resCategory);

        if (!existsInvalidResourceCapType
            &&
                (GetCapTypeOption.OnlyGetCurrentLanguageCapType == getCapTypeOption
                || GetCapTypeOption.GetCurrentOrMasterLanguageCapType == getCapTypeOption
                )
            )
        {
            result = string.Format(pattern, capTypeModel.resGroup, capTypeModel.resType, capTypeModel.resSubType, capTypeModel.resCategory, ACAConstant.SLASH);
        }

        if (GetCapTypeOption.OnlyGetMasterLanguageCapType == getCapTypeOption
            ||
            (GetCapTypeOption.GetCurrentOrMasterLanguageCapType == getCapTypeOption && existsInvalidResourceCapType)
            )
        {
            result = string.Format(pattern, capTypeModel.group, capTypeModel.type, capTypeModel.subType, capTypeModel.category, ACAConstant.SLASH);
        }
        return result;
    }

    public enum GetCapTypeOption
    {
        OnlyGetMasterLanguageCapType
        ,
        OnlyGetCurrentLanguageCapType
       , GetCurrentOrMasterLanguageCapType
    }


    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="dtCapTree"></param>
    ///// <param name="permitNumber"></param>
    ///// <param name="indexCode"></param>
    ///// <returns>number of child related CAP</returns>
    //private int DealData(DataTable dtCapTree, string permitNumber, string indexCode)
    //{
    //    string filterExpression;

    //    if (permitNumber == string.Empty)
    //    {
    //        filterExpression = "ParentPermitNumber is null";
    //    }
    //    else
    //    {
    //        filterExpression = String.Format("ParentPermitNumber = '{0}'", permitNumber);
    //    }

    //    DataRow[] resultSet = dtCapTree.Select(filterExpression);
    //    int countOfChild;
    //    string subIndexCode;
    //    int countLevelOfTree;

    //    //Generate the relationship of related CAPs by indexcode of each CAP row
    //    for (int i = 0; i < resultSet.Length; i++)
    //    {
    //        subIndexCode = indexCode + Convert.ToString(100 + i).Trim();
    //        resultSet[i]["IndexCode"] = subIndexCode;

    //        //Get tree level by indexcode
    //        countLevelOfTree = subIndexCode.Length / 3;

    //        //Extend space for subCAP row to build tree view if sub CAP exists
    //        resultSet[i]["Indent"] = countLevelOfTree * TREE_LEVEL_SPACE + "px";

    //        countOfChild = DealData(dtCapTree, resultSet[i]["PermitNumber"].ToString(), resultSet[i]["IndexCode"].ToString());

    //        if (countOfChild == 0)
    //        {
    //            resultSet[i]["ImageURL"] = String.Empty;
    //        }
    //        else
    //        {
    //            resultSet[i]["ImageURL"] = CollectURL;
    //        }
    //    }

    //    return resultSet.Length;
    //}



    #endregion related permit

    #region PERMIT SEARCH Functionality

    /// <summary>
    /// Returns list of permits for current user.
    /// </summary>
    /// <param name="permitCondition"></param>
    /// <returns></returns>
    //DWB- 11-16-2008 - Added this procedure so ACA Mobile wouild build the My Permits
    //                  list using the same mehtods as ACA.
    //                  In IST 6.7 QueryPermits stopped working and would no longer return
    //                  the list of caps after the user logged in.
    public DataTable MyPermitSearch(Permit permitCondition, List<string> selectedModules)
    {
        // DWB - 7.0 - this is used by 7.0
        Permit tempSinglePermit = new Permit();
        CapModel4WS capModel = ConvertUtil.convertPermitToCapModel(permitCondition);
        DataTable capTable = null;

        // DWB - Test cap filter by wild card - This will be replaced
        //       Later with the CAP list filter entered by the user on the home page.
        if (permitCondition.Number != string.Empty || permitCondition.Module != null)
        {
            capModel.altID = permitCondition.Number;
            capModel.moduleName = permitCondition.Module;
        }

        ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

        //Test this later - ACA is using this.
        SimpleCapModel[] caps = null;
        if (selectedModules != null)
        {
            //selectedModules = new List<string>();
            //selectedModules.Add("Service Requests");
            SearchResultModel searchResult = capBll.QueryPermitsGC(capModel, selectedModules, AppSession.User.UserSeqNum, true, null, false, null);
            caps = ObjectConvertUtil.ConvertObjectArray2EntityArray<SimpleCapModel>(searchResult.resultList);
            if (caps != null && caps.Length != 0)
            {
                capTable = CreateCapListDataSource(caps, permitCondition.Module, true);
            }

        }
        else
        {
            SearchResultModel searchResult = capBll.GetMyCapList4ACA(AgencyCode, capModel, null, AppSession.User.UserSeqNum, null, null);
            caps = ObjectConvertUtil.ConvertObjectArray2EntityArray<SimpleCapModel>(searchResult.resultList);
            // SimpleCapModel4WS[] caps = capBll.QueryPermits(capModel, selectedModules, AppSession.User.UserSeqNum, true);
            //Permit[] tempPermit = null;
            if (caps != null)
            {
                capTable = CreateCapListDataSource(caps, permitCondition.Module, false);
            }
        }
        return capTable;
    }
    /// <summary>
    /// create data table by given caplist of treeview
    /// </summary>
    /// <param name="capList">ProjectTreeNodeModel4WS model treelist</param>
    /// <param name="isCrossModel">true if cross model search</param>
    /// <returns>datasource for UI</returns>
    public DataTable CreateCapListDataSource(SimpleCapModel[] capList, string moduleName, bool isCrossModel)
    {
        DataTable dtCap = CreateCapListTable();
        int index = 0;

        foreach (SimpleCapModel cap in capList)
        {
            if (cap == null)
            {
                continue;
            }
            if (!isCrossModel && cap.moduleName != moduleName)
            {
                continue;
            }

            string permitNumber = cap.altID != null ? cap.altID.ToString() : String.Empty;
            string permitType = GetAliasOrCapTypeLabel(cap.capType);
            string status = cap.capStatus != null ? cap.capStatus.ToString() : String.Empty;
            string capStatus = cap.capClass != null ? cap.capClass.ToString() : String.Empty;
            string agencyCode = ACAConstant.AgencyCode;
            if (cap.capID != null && !String.IsNullOrEmpty(cap.capID.serviceProviderCode))
            {
                agencyCode = cap.capID.serviceProviderCode;
            }

            DataRow drCap = dtCap.NewRow();
            drCap["Agency"] = cap.capID.serviceProviderCode;
            drCap["Module"] = cap.moduleName;
            drCap["Number"] = cap.capID.ID1.ToString() + "-" + cap.capID.ID2.ToString() + "-" + cap.capID.ID3.ToString();
            drCap["Alias"] = cap.altID != null ? cap.altID.ToString() : String.Empty;
            if (cap.fileDate == null)
            {
                drCap["Date"] = DBNull.Value;
            }
            else
            {
                drCap["Date"] = I18nDateTimeUtil.FormatToDateStringForWebService(cap.fileDate);
                drCap["sortDate"] = cap.fileDate.Value.ToString("yyyyMMddHHmmss");
            }
            drCap["Type"] = permitType;
            drCap["Status"] = status;
            drCap["capClass"] = cap.capClass;
            drCap["capId1"] = cap.capID.ID1;
            drCap["capId2"] = cap.capID.ID2;
            drCap["capId3"] = cap.capID.ID3;
            //DWB- 11-16-2008 - Added address to datatable so it can be displayed
            //                  in the related caps list.
            if (cap.addressModel != null && cap.addressModel.displayAddress != null)
            {
                drCap["Address"] = cap.addressModel.displayAddress.ToString();
            }

            dtCap.Rows.Add(drCap);

            index++;
        }
        //DealData(dtCap, string.Empty, string.Empty);
        return dtCap;
    }

    /// <summary>
    /// create blank structure for cap list of treeview
    /// </summary>
    /// <returns>blank table for cap list</returns>
    private static DataTable CreateCapListTable()
    {
        DataTable dtCap = new DataTable();
        dtCap.Columns.Add(new DataColumn("Agency", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Module", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Number", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Alias", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Date", typeof(String)));
        dtCap.Columns.Add(new DataColumn("sortDate", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Type", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Status", typeof(String)));
        dtCap.Columns.Add(new DataColumn("capClass", typeof(String)));
        dtCap.Columns.Add(new DataColumn("capID1", typeof(String)));
        dtCap.Columns.Add(new DataColumn("capID2", typeof(String)));
        dtCap.Columns.Add(new DataColumn("capID3", typeof(String)));
        dtCap.Columns.Add(new DataColumn("Address", typeof(String)));

        return dtCap;
    }
    /// <summary>
    /// Permit Search returns demo data for all type PermitSearch carried out in the UI   Kale.OK
    /// </summary>
    /// <param name="permitCondition"></param>
    /// <returns></returns>
    public Permit[] PermitSearch(Permit permitCondition, bool isSearchSpecialUser)
    {
        Permit tempSinglePermit = new Permit();
        CapModel4WS capModel = ConvertUtil.convertPermitToCapModel(permitCondition);

        //Kale Modi the moduleName can not be null,if null I set a default value.And I think that 'Building' is the best.
        //if (string.IsNullOrEmpty(capModel.moduleName))
        //{
        //    capModel.moduleName = "Building";
        //}

        ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
        string userSeq = null;

        if (!AppSession.User.IsAnonymous && isSearchSpecialUser)
        {
            userSeq = AppSession.User.UserSeqNum;
        }
        List<string> selectSearchModules = new List<string>();
        selectSearchModules.Add(capModel.moduleName);

        SearchResultModel searchResult = capBll.GetCapList4ACA(capModel, null, userSeq, null, true, selectSearchModules, false, null);
        SimpleCapModel[] caps = ObjectConvertUtil.ConvertObjectArray2EntityArray<SimpleCapModel>(searchResult.resultList);

        Permit[] tempPermit;
        tempPermit = ConvertUtil.convertSimpleCapsToPermits(caps);
        for (int i = 0; i < tempPermit.Length; i++)
        {
            if (caps[i].addressModel != null)
                tempPermit[i].Address = caps[i].addressModel.displayAddress;
            //tempSinglePermit.Number = tempPermit[i].Number;
            //tempSinglePermit.Type = tempPermit[i].Type;
            //tempPermit[i] = PermitRetrieve(tempSinglePermit);
        }
        return tempPermit;
        //Permit[] Permits = null;
        //try
        //{
        //    if (TestReturnError)
        //    {
        //        throw new Exception();
        //    }
        //    Permits = new Permit[255];
        //    string[] ownerArray = new string[5];
        //    ownerArray[0] = "Brandy Bumpkin";
        //    ownerArray[1] = "Mike Martin";
        //    ownerArray[2] = "Randy Rouster";
        //    ownerArray[3] = "David Drapler";
        //    ownerArray[4] = "Clive Cloister";

        //    string[] addrArray = new string[5];
        //    addrArray[0] = "1901 N MAIN ST, SUN RISE, AZ, 86004";
        //    addrArray[1] = "1905 N MAIN ST, SUN RISE, AZ, 86004";
        //    addrArray[2] = "1909 N MAIN ST, SUN RISE, AZ, 86004";
        //    addrArray[3] = "1915 N MAIN ST, SUN RISE, AZ, 86004";
        //    addrArray[4] = "1921 N MAIN ST, SUN RISE, AZ, 86004";

        //    for (int x = 0; x < Permits.Length; x++)
        //    {
        //        Permits[x] = new Permit();

        //        Random MyRandomNumber = new Random();

        //        Permits[x].Date = "12/12/07";
        //        Permits[x].Desc = "Single Family Dwelling with attached garage";
        //        Permits[x].Number = "07BLD-00000-" + x.ToString();
        //        Permits[x].Status = "Open";
        //        Permits[x].Type = "Building/Building Permit/Residential/New";
        //        Permits[x].Owner = ownerArray[MyRandomNumber.Next(0, 4)];
        //        Permits[x].Address = addrArray[MyRandomNumber.Next(0, 4)];
        //    }
        //}
        //catch (Exception ex)
        //{
        //    OnErrorReturn = true;
        //    ExceptionMessage = ex.Message.ToString();
        //}
        //// Returns Permits Demo Data for UI
        //return Permits;

        ////call CapBll.GetCapList4ACA()
        /// 
        /// 
    }
    /// <summary>
    ///  My permit Search Criteria for Alerts: My Permits, My Saved Application, My Pending Permits
    /// </summary>
    /// <returns></returns>
    public Permit[] PermitSearch(string SearchMode)
    {
        // DWB - This is used in 7.0
        Permit[] myPermits = null;
        Permit permintCondition = new Permit();
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            // accella integration code here...
            // based on search mode Accela API should returns Permits List
            // DWB - 07-29-2008 - Added support for "View Permits" mode.
            switch (SearchMode)
            {
                case "My Permits":
                    // myPermits = PermitSearch(permintCondition, true);
                    break;
                case "View Permits":
                    //DWB - 07-27-2009 - Obsolete for 7.0
                    //DWB- 11-16-2008 - modified to call new function to build my permits list.
                    //myPermits = MyPermitSearch(permintCondition);
                    break;
                case "My Saved Application":
                    // myPermits = PermitSearch(permintCondition, true);
                    break;
                case "My Pending Permits":
                    // myPermits = PermitSearch(permintCondition, true);
                    break;
            }
            // Calls and Returns Demo Data for UI
            //Kale Modi the second parameter is 'false' before,but I think it must be 'true'.because it will search a logined user's info.
            //myPermits = PermitSearch(permintCondition, true);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return myPermits;

        //call CapBll.GetMyCapList4ACA()
    }
    /// <summary>
    /// Method for Advanced searching (Permits) based on License info 
    /// </summary>
    /// <param name="license"></param>
    /// <returns></returns>

    public Permit[] PermitSearch(License license)
    {
        Permit[] myPermits = null;
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            // accella integration code here...
            // based on search mode Accela API should returns Permits List
            // Calls and Returns Demo Data for UI
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

            CapModel4WS capModel = new CapModel4WS();
            capModel.licenseProfessionalModel = ConvertUtil.convertLicenseToProfessional(license);
            // DWB - temporary remove - this procedure is private in capBLL
            // CapModel4WS[] caps = capBll.GetCapsByConditionWithCapStyle(AgencyCode, capModel, null, User.UserSeqNum, null);
            CapModel4WS[] caps = null;
            myPermits = ConvertUtil.convertCapsToPermits(caps);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }

        return myPermits;
    }


    /// <summary>
    /// Method for advanced searching(Permits) based on Location info
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public Permit[] PermitSearch(Location location)
    {
        Permit[] myPermits = null;
        Permit myPermitList = new Permit();
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            // accella integration code here...
            // based on search mode Accela API should returns Permits List
            // Calls and Returns Demo Data for UI
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

            CapModel4WS capModel = new CapModel4WS();
            capModel.addressModel = ConvertUtil.convertLocationToAddress(location);
            // DWB - temporary remove - this procedure is private in capBLL
            // CapModel4WS[] caps = capBll.GetCapsByConditionWithCapStyle(AgencyCode, capModel, null, User.UserSeqNum, null);
            CapModel4WS[] caps = null;

            myPermits = ConvertUtil.convertCapsToPermits(caps);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return myPermits;

        //call CapBll.GetPermitsByAddressId()
    }





    /// <summary>
    /// Method for advanced Search(Permits) based on PermitNumber,Type,Status,from date & to Date specified
    /// </summary>
    /// <param name="permitsNumber"></param>
    /// <param name="permitsType"></param>
    /// <param name="permitsStatus"></param>
    /// <param name="fromDate"></param>
    /// <param name="ToDate"></param>
    /// <returns></returns>
    public Permit[] PermitSearch(string permitsNumber, string permitsType, string permitsStatus, DateTime fromDate, DateTime ToDate)
    {
        Permit[] myPermits = null;
        Permit permitCondition = new Permit();
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            // accella integration code here...
            // based on search mode Accela API should returns Permits List
            // Calls and Returns Demo Data for UI
            permitCondition.Number = permitsNumber;
            permitCondition.Type = permitsType;
            //permitCondition.Status = permitsStatus;
            permitCondition.Date = fromDate.ToString(DATATIME_FORMAT);
            permitCondition.EndDate = ToDate.ToString(DATATIME_FORMAT);

            myPermits = PermitSearch(permitCondition, false);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return myPermits;

        //call CapBll.GetCapList4ACA()
    }
    ///// <summary>
    ///// Method for simple search(Permits) based on various criteria  Kale.OK
    ///// </summary>
    ///// <param name="searchBy"></param>
    ///// <param name="searchValue"></param>
    ///// <returns></returns>
    public Permit[] PermitSearch(string searchBy, string searchValue)
    {
        Permit[] myPermits = null;
        Permit myPermitList = new Permit();
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }

            // accella integration code here...
            switch (searchBy)
            {
                // Permit No wise Simple Search
                case "PermitNo":
                    myPermitList.Number = searchValue;
                    break;
                // Inspection Type wise Simple Search
                case "InspectionType":
                    break;
                // Name  wise Simple Search
                case "Name":
                    myPermitList.SpecialText = searchValue;
                    break;
                // Address wise Simple Search
                case "Address":
                    myPermitList.AddressModel.streetName = searchValue;
                    break;
                // Contractor wise Simple Search
                case "ContractorNo":
                    myPermitList.AddressModel.houseFractionStart = searchValue;
                    break;
                // Parcel No. wise Simple Search
                case "Parcel":
                    break;
            }
            // based on search mode Accela API should returns Permits List
            // Calls and Returns Demo Data for UI
            myPermits = PermitSearch(myPermitList, false);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return myPermits;

        //call CapBll.GetCapList4ACA()
    }

    /// <summary>
    /// Method for advanced Search(Related Permits) based on PermitNumber and Type.
    /// </summary>
    /// <param name="permitsNumber"></param>
    /// <param name="permitsType"></param>
    /// <returns></returns>
    //DWB- 11-16-2008 - Added for related permits modification.
    public Permit[] RelatedPermitSearch(string permitsNumber, string permitsType)
    {

        Permit[] myPermits = null;
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            // accella integration code here...
            // Calls and Returns Demo Data for UI

            CapIDModel4WS capId = new CapIDModel4WS();
            if (!string.IsNullOrEmpty(permitsNumber))
            {
                string[] ids = permitsNumber.Split('-');

                capId.id1 = ids[0];
                capId.id2 = ids.Length > 1 ? ids[1] : string.Empty;
                capId.id3 = ids.Length > 2 ? ids[2] : string.Empty;
            }
            capId.serviceProviderCode = ConfigManager.AgencyCode;
            DataTable dtPermits = GetRelatedPermits(capId);
            myPermits = ConvertUtil.convertDatatableToPermit(dtPermits);
            // myPermits = ConvertUtil.convertCapListDatatableToPermit(dtPermits);

            if (myPermits != null)
            {
                int iIndex = -1;
                for (int x = 0; x < myPermits.Length; x++)
                {
                    if (myPermits[x].Number.ToString() != permitsNumber)
                    {
                        iIndex++;
                    }
                }
                if (iIndex == -1)
                {
                    myPermits = null;
                }
                else
                {
                    Permit[] RelatedResults = new Permit[iIndex + 1];
                    iIndex = -1;
                    for (int x = 0; x < myPermits.Length; x++)
                    {
                        if (myPermits[x].Number.ToString() != permitsNumber)
                        {
                            iIndex++;
                            RelatedResults[iIndex] = myPermits[x];
                        }
                    }
                    return RelatedResults;
                }
            }
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return myPermits;
    }
    #endregion PERMIT SEARCH Functionality

    #region INSPECTIONS SEARCH Functionality
    /// <summary>
    /// Inspections Search returns demo data for all type Inspection Search carried out in the UI   Kale.OK
    /// </summary>
    /// <param name="SearchBy"></param>
    /// <returns></returns>
    public Inspection[] InspectionSearch(Inspection SearchBy)
    {
        Inspection[] Inspections = null;
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }

            IInspectionBll inspectionBll = (IInspectionBll)ObjectFactory.GetObject(typeof(IInspectionBll));

            InspectionModel[] inspections = inspectionBll.GetInspections(SearchBy.ModuleName, AgencyCode, null, ACAConstant.PUBLIC_USER_NAME + User.UserSeqNum);

            return ConvertUtil.convertInspectionWSModelsToInspections(inspections);

        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return Inspections;
    }

    /// <summary>
    /// Inspections Search returns demo data for all type Inspection Search carried out in the UI   Kale.OK
    /// Kale Modi Add a Function to get Inspections list for one Cap
    /// </summary>
    /// <param name="SearchBy"></param>
    /// <returns></returns>
    public Inspection[] InspectionSearch(Inspection SearchBy, bool isGetCapInspections)
    {
        Inspection[] Inspections = null;
        if (isGetCapInspections != true)
        {
            return Inspections;
        }
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }

            IInspectionBll inspectionBll = (IInspectionBll)ObjectFactory.GetObject(typeof(IInspectionBll));

            CapIDModel4WS capId = new CapIDModel4WS();

            if (!string.IsNullOrEmpty(SearchBy.PermitNo))
            {
                string[] ids = SearchBy.PermitNo.Split('-');

                capId.id1 = ids[0];
                capId.id2 = ids.Length > 1 ? ids[1] : string.Empty;
                capId.id3 = ids.Length > 2 ? ids[2] : string.Empty;
            }

            capId.serviceProviderCode = ConfigManager.AgencyCode;
            InspectionModel[] inspections = inspectionBll.GetInspectionListByCapID(SearchBy.ModuleName, TempModelConvert.Trim4WSOfCapIDModel(capId), null, User.UserSeqNum);
            return ConvertUtil.convertInspectionWSModelsToInspections(inspections);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        // Returns Permits Demo Data for UI
        return Inspections;
    }

    #region Old 6.5 code not used in 6.7 or 7.0
    ///// <summary>
    ///// Method for simple search(Inspections) based on various criteria   Kale.OK
    ///// </summary>
    ///// <returns></returns>
    //public Inspection[] InspectionSearch(string searchBy, string searchValue, string filter, bool sortByDate)
    //{
    //    Inspection[] myInspection = null;
    //    Inspection[] returnInspection = null;
    //    Inspection myInspectionList = new Inspection();
    //    //Kale Modi add a CapId parameter.
    //    if (!string.IsNullOrEmpty(searchValue))
    //    {
    //        myInspectionList.PermitNo = searchValue;
    //    }
    //    try
    //    {
    //        if (TestReturnError)
    //        {
    //            throw new Exception();
    //        }
    //        // based on search mode Accela API should returns Inspections List
    //        // Calls and Returns Demo Data for UI
    //        myInspection = InspectionSearch(myInspectionList, true);
    //        returnInspection = FilterInspection(searchBy, searchValue, filter, sortByDate, myInspection);

    //    }
    //    catch (Exception ex)
    //    {
    //        OnErrorReturn = true;
    //        ExceptionMessage = ex.Message.ToString();
    //    }
    //    return returnInspection;

    //    //cannot find the match BLL method.
    //}
    ///// <summary>
    ///// Method for simple search(Inspections) based on various criteria   Kale.OK
    ///// Kale Modi Add a CapID Parameter
    ///// </summary>
    ///// <returns></returns>
    //public Inspection[] InspectionSearch(string searchBy, string searchValue, string filter, bool sortByDate, string capId)
    //{
    //    Inspection[] myInspection = null;
    //    Inspection[] returnInspection = null;
    //    Inspection myInspectionList = new Inspection();
    //    //Kale Modi add a CapId parameter.
    //    if (!string.IsNullOrEmpty(capId))
    //    {
    //        myInspectionList.PermitNo = capId;
    //    }
    //    try
    //    {
    //        if (TestReturnError)
    //        {
    //            throw new Exception();
    //        }
    //        // based on search mode Accela API should returns Inspections List
    //        // Calls and Returns Demo Data for UI
    //        myInspection = InspectionSearch(myInspectionList, true);
    //        returnInspection = FilterInspection(searchBy, searchValue, filter, sortByDate, myInspection);

    //    }
    //    catch (Exception ex)
    //    {
    //        OnErrorReturn = true;
    //        ExceptionMessage = ex.Message.ToString();
    //    }
    //    return returnInspection;

    //    //cannot find the match BLL method.
    //}
    #endregion Old 6.5 code not used in 6.7 or 7.0

    /// <summary>
    /// Method for My Inspections Inspections for Alert     Kale.OK
    /// </summary>
    /// <returns></returns>
    public Inspection[] InspectionSearch()
    {
        Inspection[] myInspection = null;
        Inspection myInspectionList = new Inspection();
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            // accella integration code here...
            // based on search mode Accela API should returns Inspections List
            // Calls and Returns Demo Data for UI
            myInspection = InspectionSearch(myInspectionList);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return myInspection;

        //cannot find the match BLL method.
    }
    /// <summary>
    /// Method for My Inspections Inspections for Alert     Kale.OK
    /// Kale Modi add a function with PermitNO
    /// </summary>
    /// <returns></returns>
    public Inspection[] GetInspectionList(Inspection ThisInspection)
    {
        Inspection[] myInspection = null;
        //Inspection myInspectionList = new Inspection();

        //myInspectionList.PermitNo = PermitNO;
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            // accella integration code here...
            // based on search mode Accela API should returns Inspections List
            // Calls and Returns Demo Data for UI
            myInspection = InspectionSearch(ThisInspection, true);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return myInspection;

        //cannot find the match BLL method.
    }

    #endregion

    #region LOCATIONS LIST API
    ///// <summary>
    ///// Location Search Method with Demo Data for UI        Kale.OK
    ///// </summary>
    ///// <returns></returns>
    //    public Location[] LocationSearch(PermitsWorkLocations WorkLocation)
    //{
    //    Location[] Locations = null;
    //    try
    //    {
    //        if (TestReturnError)
    //        {
    //            throw new Exception();
    //        }
    //        // accella integration code here...
    //        // Location List should be returned from Accela API call

    //        //TODO the address condition model.
    //        RefAddressModel4WS ramw = new RefAddressModel4WS();
    //        /// DWB - 07-18-2008 - Added WorkLocation parameters to use for search criterion.
    //        ramw.houseNumberStart = WorkLocation.LocationNumber.ToString();
    //        ramw.houseNumberEnd = WorkLocation.LocationNumber.ToString();
    //        ramw.streetDirection = WorkLocation.DirList.ToString();
    //        ramw.streetName = WorkLocation.StreetName.ToString();
    //        ramw.streetSuffix = WorkLocation.StreetType.ToString();
    //        ramw.city = WorkLocation.City.ToString();
    //        ramw.county = WorkLocation.County.ToString();
    //        ramw.zip = WorkLocation.Zip.ToString();

    //        APOBll apoBll = new APOBll();
    //        DataTable dt = apoBll.GetAPOListByAddress(AgencyCode, ramw);
    //        Locations = new Location[dt.Rows.Count];
    //        //TODO convert dt to Location array.

    //        /*Locations = new Location[227];
    //        Street[] streetArray = new Street[3];
    //        streetArray[0] = new Street();
    //        streetArray[0].name = "MAIN";
    //        streetArray[0].type = "ST";
    //        streetArray[0].dir = "N";
    //        streetArray[1] = new Street();
    //        streetArray[1].name = "REDCASTLE";
    //        streetArray[1].type = "RD";
    //        streetArray[1].dir = "W";
    //        streetArray[2] = new Street();
    //        streetArray[2].name = "CENTER";
    //        streetArray[2].type = "ST";
    //        streetArray[2].dir = "E";
    //        Accela.ACA.Web.CA.Unit[] unitArray = new Accela.ACA.Web.CA.Unit[5];
    //        unitArray[0] = new Accela.ACA.Web.CA.Unit();
    //        unitArray[0].type = "APT";
    //        unitArray[0].number = "45";
    //        unitArray[1] = new Accela.ACA.Web.CA.Unit();
    //        unitArray[1].type = "SUITE";
    //        unitArray[1].number = "100";
    //        unitArray[2] = new Accela.ACA.Web.CA.Unit();
    //        unitArray[2].type = "APT";
    //        unitArray[2].number = "200";
    //        unitArray[3] = new Accela.ACA.Web.CA.Unit();
    //        unitArray[3].type = string.Empty;
    //        unitArray[3].number = string.Empty;
    //        unitArray[4] = new Accela.ACA.Web.CA.Unit();
    //        unitArray[4].type = "";
    //        unitArray[4].number = "";
    //        */
    //        for (int x = 0; x < dt.Rows.Count; x++)
    //        {
    //            Random MyRandomNumber = new Random();
    //            int streetIdx = MyRandomNumber.Next(0, 2);
    //            int unitIdx = MyRandomNumber.Next(0, 4);
    //            Location lt = new Location();
    //            lt.City = dt.Rows[x]["City"].ToString();
    //            lt.Country = dt.Rows[x]["Country"].ToString();
    //            lt.Dir = dt.Rows[x]["StreetDirection"].ToString();
    //            lt.Fraction = dt.Rows[x]["Country"].ToString();
    //            lt.Number = MyRandomNumber.Next(100, 3500).ToString();
    //            lt.State = dt.Rows[x]["State"].ToString();
    //            lt.StreetName = dt.Rows[x]["StreetName"].ToString();
    //            lt.StreetType = dt.Rows[x]["StreetSuffix"].ToString();
    //            lt.UnitNo = dt.Rows[x]["UnitStart"].ToString();
    //            lt.UnitType = dt.Rows[x]["UnitType"].ToString();
    //            lt.Zip = dt.Rows[x]["Zip"].ToString();

    //            //DWB - 07-17-2008 for 2008 Mobile ACA interface redesign
    //            lt.OwnerFullName = dt.Rows[x]["OwnerFullName"].ToString();
    //            lt.ParcelNumber = dt.Rows[x]["ParcelNumber"].ToString();
    //            lt.FullAddress = dt.Rows[x]["FullAddress"].ToString();
    //            lt.RefId = dt.Rows[x]["AddressId"].ToString();

    //            Locations[x] = lt;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        OnErrorReturn = true;
    //        ExceptionMessage = ex.Message.ToString();
    //    }
    //    //  Returns Demo Data for UI
    //    return Locations;

    //    //call APOBll.GetAPOListByAddress()
    //}
    #endregion

    #region LICENSE List API
    ///// <summary>
    ///// Licenses Search Method with Demo Data for UI    Kale.OK
    ///// </summary>
    ///// <returns></returns>
    //public License[] LicenseSearch()
    //{
    //    License[] Licenses = null;
    //    try
    //    {
    //        if (TestReturnError)
    //        {
    //            throw new Exception();
    //        }
    //        // accella integration code here...
    //        // License(s) List should be returned from Accela API call

    //        //TODO the address condition model.

    //        LicenseBLL licenseBll = new LicenseBLL();

    //        DataTable dt = licenseBll.GetLicenseValidList(AgencyCode, long.Parse(User.UserSeqNum), User.UserID, null);

    //        //TODO convert dt to License array.


    //        /*Licenses = new License[100];

    //        string[] fnameArray = new string[10];
    //        fnameArray[0] = "DAVID";
    //        fnameArray[1] = "MICHAEL";
    //        fnameArray[2] = "AARON";
    //        fnameArray[3] = "SHARON";
    //        fnameArray[4] = "JILL";
    //        fnameArray[5] = "OWEN";
    //        fnameArray[6] = "EVERETT";
    //        fnameArray[7] = "MICHELLE";
    //        fnameArray[8] = "BRIAN";
    //        fnameArray[9] = "BETSY";

    //        string[] lnameArray = new string[10];
    //        lnameArray[0] = "JONES";
    //        lnameArray[1] = "SMITH";
    //        lnameArray[2] = "DOE";
    //        lnameArray[3] = "STONE";
    //        lnameArray[4] = "YEAGER";
    //        lnameArray[5] = "LLYWELLYN";
    //        lnameArray[6] = "MORTENSON";
    //        lnameArray[7] = "MARSH";
    //        lnameArray[8] = "LEMON";
    //        lnameArray[9] = "KOETCHA";

    //        string[] typeArray = new string[5];
    //        typeArray[0] = "ARCHITECT";
    //        typeArray[1] = "ENGINEER";
    //        typeArray[2] = "CONTRACTOR";
    //        typeArray[3] = "ELECTRICIAN";
    //        typeArray[4] = "PLUMBER";

    //        string[] addrArray = new string[5];
    //        addrArray[0] = "1901 N MAIN ST";
    //        addrArray[1] = "5990 W CENTER ST";
    //        addrArray[2] = "198 N CHESTERFIELD ST";
    //        addrArray[3] = "250 S RED CASTLE ST";
    //        addrArray[4] = "446 E BOHEME ST";
    //        */

    //        // DWB - 07-22-2008 - Uncommented this code and corrected it to return license information.
    //        Licenses = new License[dt.Rows.Count];
    //        for (int x = 0; x < Licenses.Length; x++)
    //        {
    //            /* DWB Ole test code was returning hard coded information.
    //            Random MyRandomNumber = new Random();
    //            Licenses[x].FirstName = fnameArray[MyRandomNumber.Next(0, 9)];
    //            Licenses[x].LastName = lnameArray[MyRandomNumber.Next(0, 9)];
    //            Licenses[x].LicenseNumber = "128" + x.ToString();
    //            Licenses[x].LicenseType = typeArray[MyRandomNumber.Next(0, 4)];
    //            Licenses[x].BusinessName = Licenses[x].LastName + " " + Licenses[x].LicenseType + "S";
    //            Licenses[x].MiddleName = "";
    //            Licenses[x].StreetAddress1 = addrArray[MyRandomNumber.Next(0, 4)];
    //            Licenses[x].StreetAddress2 = "";
    //            Licenses[x].Zip = "86001";
    //            */
    //            Licenses[x] = new License();
    //            Licenses[x].FirstName =      dt.Rows[x]["FirstName"].ToString();
    //            Licenses[x].LastName =       dt.Rows[x]["LastName"].ToString();
    //            Licenses[x].LicenseNumber =  dt.Rows[x]["LicenseNumber"].ToString();
    //            Licenses[x].LicenseType =    dt.Rows[x]["LicenseType"].ToString();
    //            Licenses[x].BusinessName =   dt.Rows[x]["BusinessName"].ToString();
    //            Licenses[x].MiddleName =     dt.Rows[x]["MiddleName"].ToString(); ;
    //            Licenses[x].StreetAddress1 = dt.Rows[x]["Address1"].ToString();
    //            Licenses[x].StreetAddress2 = dt.Rows[x]["Address2"].ToString(); ;
    //            Licenses[x].City =           dt.Rows[x]["City"].ToString();
    //            Licenses[x].State =          dt.Rows[x]["State"].ToString();
    //            Licenses[x].Country =        "USA";
    //            Licenses[x].Zip =            dt.Rows[x]["Zip"].ToString(); ;

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        OnErrorReturn = true;
    //        ExceptionMessage = ex.Message.ToString();
    //    }
    //    //  Returns Demo Data for UI
    //    return Licenses;

    //    //call LicenseBLL.GetLicenseValidList()
    //}
    #endregion

    #region PERMIT Retrieve
    /// <summary>
    ///  Permit Retrieval               KALE.OK
    /// </summary>
    /// <param name="ThisPermit"></param>
    /// <returns></returns>
    public Permit PermitRetrieve(Permit ThisPermit)
    {
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            // accella integration code here...
            if (ThisPermit == null)
            {
                return null;
            }

            CapModel4WS capCondition = ConvertUtil.convertPermitToCapModel(ThisPermit);

            if(string.IsNullOrEmpty(capCondition.capID.serviceProviderCode))
            {
                capCondition.capID.serviceProviderCode = ConfigManager.AgencyCode;
            }
            
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            CapWithConditionModel4WS capWithCondition = capBll.GetCapViewBySingle(capCondition.capID, User.UserSeqNum, ACAConstant.COMMON_N, StandardChoiceUtil.IsSuperAgency());

            if (capWithCondition == null || capWithCondition.capModel == null)
            {
                return null;
            }
            //DWB- 11-16-2008 - AppSession variable need by methods in the UserRoleUtil class.
            AppSession.SetCapModelToSession(capWithCondition.capModel.moduleName.ToString(), capWithCondition.capModel);
            return ConvertUtil.convertCapModelToPermit(capWithCondition.capModel);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        //  Returns Single Permit Demo Data for UI
        return null;
    }
    #endregion

    #region INSPECTIONS retrieve
    /// <summary>
    /// Inspection retrieval        Kale.OK
    /// </summary>
    /// <param name="ThisInspection"></param>
    /// <returns></returns>
    public Inspection InspectionRetrieve(Inspection ThisInspection)
    {
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            //Kale Modi Add PermitNO Parameter.
            Inspection[] inspectionList = null;
            if (string.IsNullOrEmpty(ThisInspection.PermitNo))
            {
                inspectionList = InspectionSearch();
            }
            else
            {
                inspectionList = GetInspectionList(ThisInspection);
            }

            if (inspectionList != null)
            {
                foreach (Inspection tempInspection in inspectionList)
                {
                    if (tempInspection.Id == ThisInspection.Id)
                    {
                        ThisInspection = tempInspection;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        //  Returns Demo Data for UI
        return ThisInspection;

        //cannot find the match BLL method.
    }
    #endregion

    #region LOCATION retrieve
    ///// <summary>
    ///// Location Retrieval   Kale.OK
    ///// </summary>
    ///// <param name="ThisLocation"></param>
    ///// <returns></returns>
    //public Location LicenseRetrieve(Location ThisLocation)
    //{
    //    try
    //    {
    //        if (TestReturnError)
    //        {
    //            throw new Exception();
    //        }
    //        // accella integration code here...
    //        if (ThisLocation == null || string.IsNullOrEmpty(ThisLocation.RefId))
    //        {
    //            return null;
    //        }

    //        ServiceProviderBll spBLL = new ServiceProviderBll();
    //        RefAddressBll refAddressBLL = new RefAddressBll();

    //        ServiceProviderModel4WS spModel = spBLL.GetServiceProviderByPK(AgencyCode, User.UserID);
    //        long sourceNumber = spModel.sourceNumber;

    //        RefAddressModel4WS address = refAddressBLL.GetAddressByPK(sourceNumber, long.Parse(ThisLocation.RefId));

    //        return ConvertUtil.convertRefAddressToLocation(address);

    //        /*ThisLocation.Number = "1901";
    //        ThisLocation.City = "SUN RISE";
    //        ThisLocation.Dir = "N";
    //        ThisLocation.Fraction = "";
    //        ThisLocation.State = "AZ";
    //        ThisLocation.StreetName = "MAIN";
    //        ThisLocation.StreetType = "ST";
    //        ThisLocation.UnitNo = "";
    //        ThisLocation.UnitType = "";
    //        ThisLocation.Zip = "86004";*/
    //    }
    //    catch (Exception ex)
    //    {
    //        OnErrorReturn = true;
    //        ExceptionMessage = ex.Message.ToString();
    //    }
    //    //  Returns Demo Data for UI
    //    return null;

    //    //call RefAddressBll.GetAddressByPK()
    //}
    #endregion


    #region Months List retrieval,Days of one Month retrieval.
    /// <summary>
    /// Months Name Display    
    /// </summary>
    /// <param name="currentDate">current angence date</param>
    /// <returns>first day of months list(DateTime)</returns>
    public List<DateTime> MonthsRetrieve(DateTime currentDate)
    {
        List<DateTime> months = new List<DateTime>();
        try
        {
            DateTime startDate = new DateTime(currentDate.Year, currentDate.Month, 1);

            for (int i = 0; i < 6; i++)
            {
                months.Add(startDate.AddMonths(i));
            }
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return months;
    }
   
    #endregion
    
    #region Inspection Date Available List
    /// <summary>
    /// Inspection Schedule/Re-schedule Available Date(s) to be return from Accela API Kale.OK
    /// </summary>
    /// <param name="monthVal"></param>
    /// <param name="yearVal"></param>
    /// <returns></returns>
    public ArrayList ScheduleDateAvailableRetrieve(int monthVal, int yearVal)
    {
        ArrayList DaysList = new ArrayList();
        try
        {
            if (TestReturnError)
            {
                throw new Exception();
            }
            // accella integration code here...
            DateTime currentdate = DateTime.Now;
            DateTime tmpDate = DateTime.Now;
            if ((currentdate.Month == monthVal) && (currentdate.Year == yearVal))
            {
                DaysList.Add("'" + tmpDate.ToShortDateString() + "-1:00-3:00 PM'>" + tmpDate.ToShortDateString() + "-" + "1:00-3:00 PM");
            }
            else
            {
                String dtstring;
                dtstring = "1/" + monthVal.ToString() + "/" + yearVal.ToString();
                currentdate = Convert.ToDateTime(dtstring);
                tmpDate = currentdate;
                for (int i = 1; i <= 31; i++)
                {
                    DaysList.Add("'" + tmpDate.ToShortDateString() + "-9:00-9:50 AM'>" + tmpDate.ToShortDateString() + "-" + "9:00-9:50 AM");
                    tmpDate = currentdate.AddDays(i);

                    if (tmpDate.Month != monthVal)
                    {
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        //  Returns Demo Data for UI
        return DaysList;

        //call InspectionBll.GetDatePermissionRangeByInType()
    }

    #endregion
    
    #region Common Property

    /// <summary>
    /// Gets the current agency code.
    /// </summary>
    private string AgencyCode
    {
        get
        {
            return ConfigManager.AgencyCode;
        }
    }

    /// <summary>
    /// Gets current user object.
    /// </summary>
    private User User
    {
        get
        {
            return AppSession.User;
        }
    }

    private Hashtable CalendarEvents
    {
        get
        {
            if (HttpContext.Current.Session["CalendarEvents"] == null)
            {
                return new Hashtable();
            }
            return (Hashtable)HttpContext.Current.Session["CalendarEvents"];

        }
        set
        {
            HttpContext.Current.Session["CalendarEvents"] = value;
        }
    }
    #endregion Common Property

    // DWB - Copied from Cap/CapHome.aspx.cs
    /// <summary>
    /// Init UI for "cross module search".
    /// </summary>
    public bool isCrossModuleSearch()
    {
        IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
        string xPolicyValue = String.Empty;
        xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_ENABLE_CROSS_MODULE_SEARCH);
        bool isEnabled = true;

        if (String.IsNullOrEmpty(xPolicyValue))
        {
            xPolicyValue = ACAConstant.COMMON_N;
        }

        isEnabled = xPolicyValue.Equals(ACAConstant.COMMON_Y);

        return isEnabled;
    }

    /// <summary>
    /// Create a list cell 
    /// </summary>
    public string CreateListCell(
        string checkBox,
        string cellHTML,
        int rowNum,
        int recCount,
        int resultCount,
        int resultPageStart,
        int resultRecordsPerPage,
        bool isiPhone,
        bool isAlwaysOdd)
    {
        return CreateListCell(checkBox, cellHTML, rowNum, recCount, resultCount, resultPageStart, resultRecordsPerPage, isiPhone, isAlwaysOdd, true);
    }

    /// <summary>
    /// Get Field value from Form, Request, or QueryString.
    /// </summary>
    /// <param name="fieldName"> Field name </param>
    /// <param name="isRequired"> Indcates if the user must enterer data into the field or not</param>
    /// <returns>String</returns>
    // DWB - 07-18-2008 - Added new funciton.
    public string GetFieldValue(string fieldName, bool isRequired)
    {
        string theValue = string.Empty;
        try
        {
            theValue = (HttpContext.Current.Request.QueryString[fieldName] != null)
                   ? HttpContext.Current.Request.QueryString[fieldName] : ((HttpContext.Current.Request[fieldName] != null)
                   ? HttpContext.Current.Request.Form[fieldName].ToString() : string.Empty);
        }
        catch
        {
            theValue = string.Empty;
        }
        if (isRequired == true && theValue.ToString() == "")
        {
            // IsRequiredDataValid = false;
        }
        return theValue;
    }

    /// <summary>
    /// Validate inspection date time and get the available time result
    /// </summary>
    /// <param name="selectedDate">the select date</param>
    /// <param name="inspectionParameter">the InspectionParameter object</param>
    /// <param name="recordIDModel">the CapIDModel object</param>
    /// <returns>get the available result.</returns>
    public AvailableTimeResultModel GetAvailableTimeResult(DateTime? selectedDate, InspectionParameter inspectionParameter, CapIDModel recordIDModel)
    {
        AvailableTimeResultModel availableTimeResultModel = null;

        if (selectedDate != null)
        {
            switch (inspectionParameter.ScheduleType)
            {
                case InspectionScheduleType.None:
                case InspectionScheduleType.RequestOnlyPending:
                    break;
                case InspectionScheduleType.RequestSameDayNextDay:
                case InspectionScheduleType.ScheduleUsingCalendar:
                case InspectionScheduleType.Unknown:
                default:
                    bool isBlockedWhenNoInspectorFound = StandardChoiceUtil.IsBlockedWhenNoInspectorFound(ConfigManager.AgencyCode);
                    availableTimeResultModel = InspectionViewUtil.GetAvailableTimeResult(selectedDate.Value, recordIDModel, inspectionParameter, isBlockedWhenNoInspectorFound);
                    break;
            }
        }

        return availableTimeResultModel;
    }

    /// <summary>
    /// Create a list cell 
    /// </summary>
    public string CreateListCell(
        string checkBox,
        string cellHTML,
        int rowNum,
        int recCount,
        int resultCount,
        int resultPageStart,
        int resultRecordsPerPage,
        bool isiPhone,
        bool isOptionList,
        bool isLinkText)
    {
        StringBuilder strWork = new StringBuilder();
        string oddNumberRow = string.Empty;
        if (isOptionList != true)
        {
            oddNumberRow = "Even";
            if (recCount != 0 && recCount - ((recCount / 2) * 2) != 0)
            {
                oddNumberRow = "Odd";
            }
        }
        if (recCount == resultPageStart)
        {
            if (recCount == (resultCount - 1))
            {
                strWork.Append("<div id=\"pageListSingleRow");
            }
            else
            {
                strWork.Append("<div id=\"pageListTop" + oddNumberRow);
            }
        }
        else if ((rowNum + 1) == resultRecordsPerPage || recCount == (resultCount - 1))
        {
            strWork.Append("<div id=\"pageListBottom" + oddNumberRow);
        }
        else
        {
            strWork.Append("<div id=\"pageListMiddle" + oddNumberRow);
        }
        strWork.Append("\">");

        if (isiPhone == true)
        {
            strWork.Append("<table id=\"pageListCellTable\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" ><tr>");
        }
        if (checkBox != string.Empty)
        {
            strWork.Append("<td id=\"pageListCellTableLeft\">");
            strWork.Append(checkBox);
            strWork.Append("</td>");
        }
        strWork.Append("<td id=\"pageListCellTableMiddle\">");
        //strWork.Append(cellHTML
        //    + " " + rowNum.ToString()
        //    + " " + recCount.ToString()
        //    + " " + resultCount
        //    + " " + resultPageStart
        //    + " " + resultRecordsPerPage.ToString());
        strWork.Append(cellHTML);
        if (isiPhone == true && isLinkText)
        {
            strWork.Append("</td>");
            strWork.Append("<td id=\"pageListCellTableRight\">");
            strWork.Append("<img style=\"float:right\" src=\"img/chevron.png\" /> ");
            strWork.Append("</a>");
        }
        if (isiPhone == true)
        {
            strWork.Append("</td></tr></table>");
        }
        strWork.Append("</div>");
        return strWork.ToString();
    }

    public string CreateSelectListCell(
        string cellHTML,
        int rowNum,
        int recCount,
        int resultCount,
        int resultPageStart,
        int resultRecordsPerPage,
        bool isiPhone,
        bool isOptionList)
    {
        StringBuilder strWork = new StringBuilder();
        string rowPosition = string.Empty;

        string oddNumberRow = string.Empty;
        if (isOptionList != true)
        {
            oddNumberRow = "Even";
            if ((recCount != 0 && recCount - ((recCount / 2) * 2) != 0))
            {
                oddNumberRow = "Odd";
            }
        }
        if (recCount == resultPageStart)
        {
            if (recCount == (resultCount - 1))
            {
                strWork.Append("<div id=\"pageListSingleRow");
                rowPosition = " Single";
            }
            else
            {
                strWork.Append("<div id=\"pageListTop" + oddNumberRow);
                rowPosition = " Top";
            }
        }
        else if ((rowNum + 1) == resultRecordsPerPage || recCount == (resultCount - 1))
        {
            strWork.Append("<div id=\"pageListBottom" + oddNumberRow);
            rowPosition = " Bottom";
        }
        else
        {
            strWork.Append("<div id=\"pageListMiddle" + oddNumberRow);
            rowPosition = " Middle";
        }
        strWork.Append("\">");
        strWork.Append(cellHTML);
        /*
        strWork.Append(rowNum.ToString()
            + " " + recCount.ToString()
            + " " + resultCount
            + " " + resultPageStart
            + " " + resultRecordsPerPage.ToString()
            + rowPosition);
        */
        strWork.Append("</div>");
        return strWork.ToString();
    }

    /// <summary>
    /// InspectionListCell 
    /// </summary>
    /// <param name="cellHTML"> The HTML cell content to be added to the list</param>
    /// <param name="rowNum"> The row number of the target view's list that the cell is being inserted into </param>
    /// <param name="recCount"> The row count of records parsed from the source table where the cell data exists </param>
    /// <param name="resultPageStart"> The the row count of the first record in the page displayed in the list </param>
    /// <param name="resultRecordsPerPage"> The number of records displayed in a page of records </param>
    /// <param name="isiPhone"> indicates if iPhome mode is enabled </param>
    /// <param name="isOptionsList"> Indicates a simple pick list is being built rather than a data list </param>
    public string InspectionListCell(
        string cellHTML,
        int rowNum,
        int recCount,
        int resultCount,
        int resultPageStart,
        int resultRecordsPerPage,
        bool isiPhone,
        bool isOptionList)
    {
        StringBuilder strWork = new StringBuilder();
        string rowPosition = string.Empty;
        string oddNumberRow = string.Empty;
        if (isOptionList != true)
        {
            oddNumberRow = "Even";
            if ((recCount != 0 && recCount - ((recCount / 2) * 2) != 0))
            {
                oddNumberRow = "Odd";
            }
        }
        if (recCount == resultPageStart)
        {
            if (recCount == (resultCount - 1))
            {
                strWork.Append("<div id=\"inspectionListSingleRow");
                rowPosition = " Single";
            }
            else
            {
                strWork.Append("<div id=\"inspectionListTop" + oddNumberRow);
                rowPosition = "Top";
            }
        }
        else if ((rowNum + 1) == resultRecordsPerPage || recCount == (resultCount - 1))
        {
            strWork.Append("<div id=\"inspectionListBottom" + oddNumberRow);
            rowPosition = " Bottom";
        }
        else
        {
            strWork.Append("<div id=\"inspectionListMiddle" + oddNumberRow);
            rowPosition = " Middle";
        }
        strWork.Append("\">");
        strWork.Append(cellHTML);
        /*
        strWork.Append(rowNum.ToString()
            + " " + recCount.ToString()
            + " " + resultCount
            + " " + resultPageStart
            + " " + resultRecordsPerPage.ToString()
            + rowPosition);
        */
        strWork.Append("</div>");
        return strWork.ToString();
    }
    /// <summary>
    /// get field visible
    /// </summary>
    /// <param name="models">simple view element modles</param>
    /// <param name="fieldName">field name</param>
    /// <returns>true if field is visible,otherwise,it's false.</returns>
    public bool IsFieldVisible(SimpleViewElementModel4WS[] models, string fieldName)
    {
        if (models == null ||
            models.Length == 0)
        {
            return true;
        }

        foreach (SimpleViewElementModel4WS model in models)
        {
            if (model.viewElementName == fieldName)
            {
                return model.recStatus == ACAConstant.VALID_STATUS;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets the section permissions for the specified module.
    /// </summary>
    /// <param name="moduleName">The name of the module to get section permissions </param>
    /// <returns>returns all section permissions of this module. returns empty Dictionary instance if no section permissions</returns>
    public static Dictionary<string, UserRolePrivilegeModel> GetSectionPermissions(string moduleName)
    {
        Dictionary<string, UserRolePrivilegeModel> sectionPermissions = new Dictionary<string, UserRolePrivilegeModel>();

        IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
        XPolicyModel[] xpolicys = xPolicyBll.GetPolicyListByCategory(
            BizDomainConstant.STD_ITEM_CAPDETAIL_SECTIONROLES,
            ACAConstant.LEVEL_TYPE_MODULE,
            moduleName);

        if (xpolicys == null)
        {
            return sectionPermissions;
        }

        // find general section permissions
        foreach (XPolicyModel xpolicy in xpolicys)
        {
            if (EntityType.GENERAL.ToString().Equals(xpolicy.data3, StringComparison.InvariantCultureIgnoreCase))
            {
                if ((!string.IsNullOrEmpty(xpolicy.data2) && xpolicy.data2.Length < 5) ||
                    string.IsNullOrEmpty(xpolicy.data4))
                {
                    throw new ACAException("Invalid section permissions");
                }

                if (sectionPermissions.ContainsKey(xpolicy.data4))
                {
                    continue;
                }

                UserRolePrivilegeModel userRole = null;
                if (!string.IsNullOrEmpty(xpolicy.data2))
                {
                    //userRole = UserRoleUtil.ConvertToUserRolePrivilegeModel(xpolicy.data2);
                    var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
                    userRole = userRoleBll.ConvertToUserRolePrivilegeModel(xpolicy.data2);
                }

                FillInLicenseTypesInfo(xpolicys, xpolicy.data4, userRole);
                sectionPermissions.Add(xpolicy.data4, userRole);
            }
        }

        return sectionPermissions;
    }

    /// <summary>
    /// Gets a boolean value represpenting whether the given section needs to display for the current user according
    /// to the given section permissions.
    /// Note:
    /// For compatibility with previous version, in this feature, the following sections:
    ///   1.¡°Education¡±, 
    ///   2.¡°Continuing Education¡±, 
    ///   3.¡°Examination¡±
    /// by default will be NOT visible for any users (except Admin), and the other sections will be visible for any users
    /// </summary>
    /// <param name="section">The section to determine whether to display </param>
    /// <param name="sectionPermissions">The section permissions used to determine the specified whether to display</param>
    /// <returns>true if the specified section need to display; otherwise, false</returns>
    /// <remarks>by default, all sections is visible for any user unless administor explicitly configures permissions for sections</remarks>
    public static bool GetSectionVisibility(string section, Dictionary<string, UserRolePrivilegeModel> sectionPermissions, string moduleName)
    {
        bool isSectionVisible = true;

        // The default situation that no section permission is set
        if (sectionPermissions == null || sectionPermissions.Count == 0)
        {
            isSectionVisible = GetDefaultVisibility(section);
        }
        else
        {
            UserRolePrivilegeModel findedPermission = null;
            if (sectionPermissions.ContainsKey(section))
            {
                findedPermission = sectionPermissions[section];
            }

            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();

            if (findedPermission != null)
            {
                isSectionVisible = proxyUserRoleBll.HasReadOnlyPermission(AppSession.GetCapModelFromSession(moduleName), findedPermission);
            }
            else
            {
                // The default situation that no section permission is set for the specified section
                isSectionVisible = GetDefaultVisibility(section);
            }
        }

        return isSectionVisible;
    }

    /// <summary>
    /// Finds license type setting information for the specified section in the given XPolicyModel4WS array and fills 
    /// found license types into the specified 
    /// </summary>
    /// <param name="xpolicys">xpolicys in which to find matching licence types for the section</param>
    /// <param name="sectionName">the name of the section for which to find license types</param>
    /// <param name="sectionPermission">UserRolePrivilegeModel4WS instance to be filled in</param>
    private static void FillInLicenseTypesInfo(XPolicyModel[] xpolicys, string sectionName, UserRolePrivilegeModel sectionPermission)
    {
        // find related license types
        foreach (XPolicyModel xpolicy in xpolicys)
        {
            if (EntityType.LICENSETYPE.ToString().Equals(xpolicy.data3, StringComparison.InvariantCultureIgnoreCase) &&
                sectionName.Equals(xpolicy.data4, StringComparison.InvariantCultureIgnoreCase))
            {
                if (!string.IsNullOrEmpty(xpolicy.data2))
                {
                    sectionPermission.licenseTypeRuleArray
                        = xpolicy.data2.Split(new string[] { ACAConstant.SPLIT_DOUBLE_VERTICAL }, StringSplitOptions.RemoveEmptyEntries);
                }

                break;
            }
        }
    }

    /// <summary>
    /// Get the default section permission for the specified section.
    /// </summary>
    /// <param name="section">The section to get the default section permission</param>
    /// <returns>true if the default section permission is visible; otherwise, false</returns>
    /// <remarks>For compatibility with previous version, in this feature, the following sections:
    ///   1.¡°Education¡±, 
    ///   2.¡°Continuing Education¡±, 
    ///   3.¡°Examination¡±
    /// by default will be NOT visible for any users (except Admin), and the other sections will be visible for any users
    /// </remarks>
    private static bool GetDefaultVisibility(string section)
    {
        bool visible = true;

        if (CapDetailSectionType.EDUCATION.ToString().Equals(section, StringComparison.InvariantCultureIgnoreCase) ||
            CapDetailSectionType.CONTINUING_EDUCATION.ToString().Equals(section, StringComparison.InvariantCultureIgnoreCase) ||
            CapDetailSectionType.EXAMINATION.ToString().Equals(section, StringComparison.InvariantCultureIgnoreCase))
        {
            visible = false;
        }

        return visible;
    }
}

public class AvariableTimePeriod
{
    private DateTime _startTime;
    private DateTime _endTime;
    private string _AMOrPM;
    public DateTime StartTime
    {
        get
        {
            return _startTime;
        }
        set
        {
            _startTime = value;
        }
    }
    public DateTime EndTime
    {
        get
        {
            return _endTime;
        }
        set
        {
            _endTime = value;
        }
    }
    public string AMOrPM
    {
        get
        {
            return _AMOrPM;
        }
        set
        {
            _AMOrPM = value;
        }
    }


}

