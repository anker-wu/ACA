#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EmseUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EmseUtil.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Text;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.EMSE;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Trigger EMSE Script
    /// </summary>
    public static class EmseUtil
    {
        #region Fields

        /// <summary>
        /// The returnCode is "-1" stands for display message
        /// </summary>
        public static readonly string RETURN_CODE_FOR_DISPLAY_MESSAGE = "-1";

        /// <summary>
        /// The flag of return to Login Page
        /// </summary>
        public static readonly string RETURN_TO_LOGINPAGE_YES = "Y";

        /// <summary>
        /// Create an instance of ILog
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(EmseUtil));

        #endregion Fields

        #region Methods

        /// <summary>
        /// add EMSE error message to log
        /// </summary>
        /// <param name="info">EmseErrorLogInfo object</param>
        public static void EMSEInfoLog(EmseErrorLogInfo info)
        {
            StringBuilder buf = new StringBuilder();

            buf.Append(Environment.NewLine);
            buf.Append("--------------------------------------" + Environment.NewLine);
            buf.Append("DateTime:" + DateTime.Now.ToString() + Environment.NewLine);
            buf.Append("ScriptCode:" + info.ScriptCode + Environment.NewLine);
            buf.Append("EventType:" + info.EventType + Environment.NewLine);
            buf.Append("CapID1:" + info.CapID1 + Environment.NewLine);
            buf.Append("CapID2:" + info.CapID2 + Environment.NewLine);
            buf.Append("CapID3:" + info.CapID3 + Environment.NewLine);
            buf.Append("CapModelIsNull:" + info.CapModelIsNull + Environment.NewLine);
            buf.Append("ErrorMessage:" + info.UserErrorMessage + Environment.NewLine);
            buf.Append("SystemErrorMessage:" + info.SystemErrorMessage + Environment.NewLine);
            buf.Append("--------------------------------------" + Environment.NewLine);

            Logger.Info(buf.ToString());
        }

        /// <summary>
        /// Execute EMSE event
        /// </summary>
        /// <param name="capModel4WS">CapModel4WS object</param>
        /// <param name="pageModel">PageModel object</param>
        /// <param name="eventType">the event type: <c>onloadEvent,beforeClick afterClick</c></param>
        /// <param name="isFromConfirmPage">whether from confirm page or not.</param>
        /// <returns>ErrorMessage,if null means successful</returns>
        public static EMSEResultModel4WS ExecuteEMSE(ref CapModel4WS capModel4WS, PageModel pageModel, EmseEventType eventType, bool isFromConfirmPage = false)
        {
            if (capModel4WS == null || pageModel == null)
            {
                return null;
            }

            EMSEResultModel4WS emseModel4WS = null;
            string emseErrorMsg = string.Empty;
            string eventName = string.Empty;

            switch (eventType)
            {
                case EmseEventType.OnloadEvent:
                    eventName = pageModel.onloadEventName;
                    break;
                case EmseEventType.BeforeButtonEvent:
                    eventName = pageModel.beforeClickEventName;
                    break;
                case EmseEventType.AfterButtonEvent:
                    eventName = pageModel.afterClickEventName;
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(eventName))
            {
                EmseErrorLogInfo errorInfo = new EmseErrorLogInfo();
                errorInfo.CapID1 = capModel4WS.capID.id1;
                errorInfo.CapID2 = capModel4WS.capID.id2;
                errorInfo.CapID3 = capModel4WS.capID.id3;
                errorInfo.EventType = eventType.ToString();
                errorInfo.ScriptCode = eventName;

                try
                {
                    //save page state track data to DB
                    PageFlowUtil.UpdatePageTrace(capModel4WS);
                    
                    AppSpecificInfoGroupModel4WS[] oldASISubGroups = capModel4WS.appSpecificInfoGroups;
                    AppSpecificTableGroupModel4WS[] oldASITGroups = capModel4WS.appSpecTableGroups;

                    IEMSEBll emseBll = ObjectFactory.GetObject<IEMSEBll>();
                    emseModel4WS = emseBll.RunEMSEScript4PageFlow(pageModel.serviceProviderCode, eventName, capModel4WS, isFromConfirmPage);

                    //refresh page state tracking data for session because EMSE may be changed the tracking data. 
                    PageFlowUtil.RefreshPageStateTracking(capModel4WS);

                    //errorCode=0 means success,-1 means error
                    if (string.IsNullOrEmpty(emseModel4WS.errorMessage) && emseModel4WS.errorCode != "-1" &&
                        emseModel4WS.cap != null)
                    {
                        emseModel4WS.cap.IsContactsChecked4Record = capModel4WS.IsContactsChecked4Record;
                        emseModel4WS.cap.IsLicensesChecked4Record = capModel4WS.IsLicensesChecked4Record;

                        capModel4WS = emseModel4WS.cap;
                        ResetASISubgroup(oldASISubGroups, capModel4WS.appSpecificInfoGroups);

                        if (oldASITGroups != null)
                        {
                            if (capModel4WS.appSpecTableGroups == null)
                            {
                                capModel4WS.appSpecTableGroups = oldASITGroups;
                            }
                            else
                            {
                                ResetASITGroups(oldASITGroups, capModel4WS.appSpecTableGroups);
                            }
                        }

                        //temp key field will lost after run EMSE, need reset.
                        if (capModel4WS.licenseProfessionalList != null && capModel4WS.licenseProfessionalList.Length != 0)
                        {
                            foreach (var licenseProfessionalModel in capModel4WS.licenseProfessionalList.Where(o => o != null))
                            {
                                licenseProfessionalModel.TemporaryID = CommonUtil.GetRandomUniqueID();
                            }
                        }

                        //temp key field will lost after run EMSE, need reset.
                        if (capModel4WS.contactsGroup != null && capModel4WS.contactsGroup.Length != 0)
                        {
                            int i = 1;

                            foreach (var contact in capModel4WS.contactsGroup.Where(o => o != null && o.people != null))
                            {
                                contact.people.RowIndex = i++;
                            }
                        }

                        ContactUtil.InitializeContactsGroup4CapModel(capModel4WS);
                        LicenseUtil.InitializeLicenseProfessional4CapModel(capModel4WS);
                    }
                    else
                    {
                        errorInfo.UserErrorMessage = string.IsNullOrEmpty(emseModel4WS.errorMessage) == false ? emseModel4WS.errorMessage : string.Empty;
                        errorInfo.CapModelIsNull = emseModel4WS.cap == null ? "Is Null" : "Not Null";
                        errorInfo.ErrorCode = emseModel4WS.errorCode ?? string.Empty;
                        emseErrorMsg = string.IsNullOrEmpty(emseModel4WS.errorMessage) == false ? emseModel4WS.errorMessage : LabelUtil.GetTextByKey("per_EMSE4Pageflow_error", string.Empty);

                        if (errorInfo.ErrorCode.Equals(ACAConstant.SSNORFEIN_ERRORCODE))
                        {
                            emseErrorMsg = ACAConstant.SSNORFEIN_ERRORCODE + emseErrorMsg;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorInfo.SystemErrorMessage = ex.Message + ex.Source + ex.StackTrace;
                    emseErrorMsg = LabelUtil.GetTextByKey("per_EMSE4Pageflow_error", string.Empty);
                }
                finally
                {
                    EMSEInfoLog(errorInfo);
                }
            }

            if (!string.IsNullOrEmpty(emseErrorMsg))
            {
                if (emseModel4WS == null)
                {
                    emseModel4WS = new EMSEResultModel4WS();
                }
                
                emseModel4WS.errorMessage = emseErrorMsg;
            }
            
            return emseModel4WS;
        }

        /// <summary>
        /// Get page flow group from session,if null get from BLL
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>Page Flow Group Model object</returns>
        public static PageFlowGroupModel GetPageflowGroup(string moduleName)
        {
            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();

            if (pageflowGroup == null)
            {
                IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

                pageflowGroup = pageflowBll.GetPageflowGroupByCapType(capModel.capType);
                pageflowGroup = CapUtil.GetPageFlowWithoutBlankPage(capModel, pageflowGroup);

                AppSession.SetPageflowGroupToSession(pageflowGroup);
            }

            return pageflowGroup;
        }

        /// <summary>
        /// Judge whether config the Event Script, searched from Cache.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="eventName">the emse event name</param>
        /// <returns>true if config the Event Script. Otherwise, return false.</returns>
        public static bool IsConfigEventScript(string agencyCode, string eventName)
        {
            IEMSEBll emseBll = ObjectFactory.GetObject<IEMSEBll>();

            bool isConfigEventScript = false;

            if (!string.IsNullOrEmpty(eventName))
            {
                isConfigEventScript = emseBll.IsConfigEventScript(agencyCode, eventName);
            }

            return isConfigEventScript;
        }

        /// <summary>
        /// this method to trigger login on EMSE
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="callerID">string caller id.</param>
        /// <param name="paramsModel"><c>OnLoginParamsModel4WS</c> object</param>
        /// <returns>EMSEOnLoginResultModel4WS object</returns>
        public static EMSEOnLoginResultModel4WS RunEMSEScriptOnLogin(string eventName, string callerID, OnLoginParamsModel4WS paramsModel)
        {
            IEMSEBll emseBll = ObjectFactory.GetObject<IEMSEBll>();
            bool isConfigEventScript = emseBll.IsConfigEventScript(eventName);

            if (isConfigEventScript)
            {
                return emseBll.RunEMSEScriptOnLogin(eventName, paramsModel);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// this method to trigger EMSE to validate License
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="callerID">string caller id</param>
        /// <param name="licenseModel">LicenseModel4WS object</param>
        /// <returns>EMSEResultBaseModel4WS object</returns>
        public static EMSEResultBaseModel4WS RunEMSEValidationLicense(string eventName, string callerID, LicenseModel4WS licenseModel)
        {
            IEMSEBll emseBll = ObjectFactory.GetObject<IEMSEBll>();

            // Search whether the event bind to script from cache.
            bool isConfigEventScript = emseBll.IsConfigEventScript(eventName);

            if (isConfigEventScript)
            {
                return emseBll.RunEMSEValidationLicense(eventName, licenseModel);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Trigger the shopping cart EMSE.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="eventName">The EMSE event name.</param>
        /// <param name="capTypes">The cap type list.</param>
        /// <param name="totalAmount">The total amount to pay.</param>
        /// <returns>Return the EMSE result.</returns>
        public static EMSEResultModel4WS RunEMSEScript4ShoppingCart(string agencyCode, string eventName, CapTypeModel[] capTypes, double totalAmount)
        {
            IEMSEBll emseBll = ObjectFactory.GetObject<IEMSEBll>();
            
            return emseBll.RunEMSEScript4ShoppingCart(agencyCode, eventName, capTypes, totalAmount);
        }

        /// <summary>
        /// Trigger save and resume event
        /// </summary>
        /// <param name="capModel4WS">The CapModel4WS instance</param>
        /// <returns>EMSEResultBaseModel4WS object</returns>
        public static EMSEResultBaseModel4WS TriggerEMSESaveAndResume(CapModel4WS capModel4WS)
        {
            if (capModel4WS == null)
            {
                return null;
            }

            IEMSEBll emseBll = ObjectFactory.GetObject<IEMSEBll>();

            return emseBll.RunEMSEScriptSaveAndResume(capModel4WS);
        }

        /// <summary>
        /// get return data by json group name and key, example json:
        ///  {'PageFlow': {'StepNumber': '3', 'PageNumber':'1'}}
        ///  group: PageFlow
        ///  key: StepNumber or PageNumber
        /// </summary>
        /// <param name="emseResult">EMSE return model</param>
        /// <param name="group">group code which from EMSE return json value</param>
        /// <param name="key">key of return value</param>
        /// <returns>the value which gets by key</returns>
        public static string GetEMSEReturnData(EMSEResultModel4WS emseResult, string group, string key)
        {
            if (emseResult == null || string.IsNullOrEmpty(emseResult.returnData))
            {
                return null;
            }

            string valueByKey = null;
            JObject returnData = JsonConvert.DeserializeObject(emseResult.returnData) as JObject;
            
            if (returnData != null && returnData.Count > 0)
            {
                JObject subValues = returnData[group] as JObject;
                
                if (subValues != null || subValues.Count > 0)
                {
                    valueByKey = subValues[key].ToString();
                }
            }

            return valueByKey;
        }

        /// <summary>
        /// Set ASI Information back
        /// </summary>
        /// <param name="oldASISubGroups">the old ASI model.</param>
        /// <param name="newASISubGroups">The new ASI sub groups.</param>
        private static void ResetASISubgroup(AppSpecificInfoGroupModel4WS[] oldASISubGroups, AppSpecificInfoGroupModel4WS[] newASISubGroups)
        {
            if (oldASISubGroups == null || oldASISubGroups.Length < 1 || newASISubGroups == null || newASISubGroups.Length < 1)
            {
                return;
            }

            foreach (AppSpecificInfoGroupModel4WS oldASISubGroup in oldASISubGroups)
            {
                foreach (AppSpecificInfoGroupModel4WS newASISubGroup in newASISubGroups)
                {
                    string agencyCode = oldASISubGroup.fields[0].serviceProviderCode;

                    if (newASISubGroup.fields[0].serviceProviderCode == agencyCode
                        && newASISubGroup.groupCode == oldASISubGroup.groupCode
                        && newASISubGroup.groupName == oldASISubGroup.groupName)
                    {
                        newASISubGroup.instruction = oldASISubGroup.instruction;
                        newASISubGroup.resInstruction = oldASISubGroup.resInstruction;
                        newASISubGroup.columnLayout = oldASISubGroup.columnLayout;
                        newASISubGroup.columnArrangement = oldASISubGroup.columnArrangement;
                        newASISubGroup.labelDisplay = oldASISubGroup.labelDisplay;

                        ResetASIFields(newASISubGroup, oldASISubGroup);
                    }
                }
            }
        }

        /// <summary>
        /// Set ASI Fields back to model.
        /// </summary>
        /// <param name="newSubgroup">the new sub group</param>
        /// <param name="oldSubgroup">the old sub group</param>
        private static void ResetASIFields(AppSpecificInfoGroupModel4WS newSubgroup, AppSpecificInfoGroupModel4WS oldSubgroup)
        {
            if (newSubgroup == null || newSubgroup.fields == null || newSubgroup.fields.Length < 1
                || oldSubgroup == null || oldSubgroup.fields == null || oldSubgroup.fields.Length < 1)
            {
                return;
            }

            foreach (AppSpecificInfoModel4WS newASIField in newSubgroup.fields)
            {
                foreach (AppSpecificInfoModel4WS oldASIField in oldSubgroup.fields)
                {
                    if (oldASIField.fieldLabel == newASIField.fieldLabel
                        && oldASIField.fieldType == newASIField.fieldType)
                    {
                        newASIField.waterMark = oldASIField.waterMark;
                        newASIField.resWaterMark = oldASIField.resWaterMark;
                        newASIField.instruction = oldASIField.instruction;
                        newASIField.resInstruction = oldASIField.resInstruction;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Traversal the <see cref="sourGroups"/> and use the attributes in the <see cref="sourGroups"/> to fill out the  <see cref="destGroups"/>.
        /// </summary>
        /// <param name="sourGroups">Source ASIT Groups</param>
        /// <param name="destGroups">Destination ASIT Groups</param>
        private static void ResetASITGroups(AppSpecificTableGroupModel4WS[] sourGroups, AppSpecificTableGroupModel4WS[] destGroups)
        {
            foreach (AppSpecificTableGroupModel4WS sGroup in sourGroups)
            {
                if (sGroup == null)
                {
                    continue;
                }

                AppSpecificTableGroupModel4WS dGroup = destGroups.SingleOrDefault(
                    group => group != null &&
                        group.capIDModel != null &&
                        group.capIDModel.Equals(sGroup.capIDModel) &&
                        group.groupName != null &&
                        group.groupName.Equals(sGroup.groupName, StringComparison.InvariantCultureIgnoreCase));

                if (dGroup != null)
                {
                    ResetASITSubgroup(sGroup, dGroup);
                }
            }
        }

        /// <summary>
        /// Set ASI Table Information back
        /// </summary>
        /// <param name="oldASITGroupModel">The old ASIT group model.</param>
        /// <param name="newASITGroupModel">The new ASIT group model.</param>
        private static void ResetASITSubgroup(AppSpecificTableGroupModel4WS oldASITGroupModel, AppSpecificTableGroupModel4WS newASITGroupModel)
        {
            if (oldASITGroupModel == null || oldASITGroupModel.tablesMapValues == null || oldASITGroupModel.tablesMapValues.Length < 1
                || newASITGroupModel == null || newASITGroupModel.tablesMapValues == null || newASITGroupModel.tablesMapValues.Length < 1)
            {
                return;
            }

            AppSpecificTableModel4WS[] oldASITSubGroups = oldASITGroupModel.tablesMapValues;
            AppSpecificTableModel4WS[] newASITSubGroups = newASITGroupModel.tablesMapValues;

            foreach (AppSpecificTableModel4WS oldASITSubGroup in oldASITSubGroups)
            {
                foreach (AppSpecificTableModel4WS newASITSubGroup in newASITSubGroups)
                {
                    if (!string.Equals(newASITSubGroup.groupName, oldASITSubGroup.groupName, StringComparison.OrdinalIgnoreCase)
                        || !string.Equals(newASITSubGroup.tableName, oldASITSubGroup.tableName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (oldASITSubGroup.templateLayoutConfig != null)
                    {
                        if (newASITSubGroup.templateLayoutConfig == null)
                        {
                            newASITSubGroup.templateLayoutConfig = new TemplateLayoutConfigModel();
                        }

                        newASITSubGroup.templateLayoutConfig.instruction = oldASITSubGroup.templateLayoutConfig.instruction;
                        newASITSubGroup.templateLayoutConfig.buttonAddDisplay = oldASITSubGroup.templateLayoutConfig.buttonAddDisplay;
                        newASITSubGroup.templateLayoutConfig.buttonAddMoreLabel = oldASITSubGroup.templateLayoutConfig.buttonAddMoreLabel;
                        newASITSubGroup.templateLayoutConfig.buttonAddRowLabel = oldASITSubGroup.templateLayoutConfig.buttonAddRowLabel;
                        newASITSubGroup.templateLayoutConfig.buttonDeleteDisplay = oldASITSubGroup.templateLayoutConfig.buttonDeleteDisplay;
                        newASITSubGroup.templateLayoutConfig.buttonDeleteRowLabel = oldASITSubGroup.templateLayoutConfig.buttonDeleteRowLabel;
                        newASITSubGroup.templateLayoutConfig.buttonEditDisplay = oldASITSubGroup.templateLayoutConfig.buttonEditDisplay;
                        newASITSubGroup.templateLayoutConfig.buttonEditRowLabel = oldASITSubGroup.templateLayoutConfig.buttonEditRowLabel;

                        if (oldASITSubGroup.templateLayoutConfig.i18NModel != null)
                        {
                            if (newASITSubGroup.templateLayoutConfig.i18NModel == null)
                            {
                                newASITSubGroup.templateLayoutConfig.i18NModel = new TemplateLayoutConfigI18NModel();
                            }

                            newASITSubGroup.templateLayoutConfig.i18NModel.instruction = oldASITSubGroup.templateLayoutConfig.i18NModel.instruction;
                            newASITSubGroup.templateLayoutConfig.i18NModel.buttonAddMoreLabel = oldASITSubGroup.templateLayoutConfig.i18NModel.buttonAddMoreLabel;
                            newASITSubGroup.templateLayoutConfig.i18NModel.buttonAddRowLabel = oldASITSubGroup.templateLayoutConfig.i18NModel.buttonAddRowLabel;
                            newASITSubGroup.templateLayoutConfig.i18NModel.buttonDeleteRowLabel = oldASITSubGroup.templateLayoutConfig.i18NModel.buttonDeleteRowLabel;
                            newASITSubGroup.templateLayoutConfig.i18NModel.buttonEditRowLabel = oldASITSubGroup.templateLayoutConfig.i18NModel.buttonEditRowLabel;
                        }
                    }

                    ResetASITFields(newASITSubGroup, oldASITSubGroup);
                }
            }
        }

        /// <summary>
        /// Set ASI table Fields back to model.
        /// </summary>
        /// <param name="newSubgroup">the new sub group</param>
        /// <param name="oldSubgroup">the old sub group</param>
        private static void ResetASITFields(AppSpecificTableModel4WS newSubgroup, AppSpecificTableModel4WS oldSubgroup)
        {
            if (newSubgroup == null || newSubgroup.columns == null || newSubgroup.columns.Length < 1
                || oldSubgroup == null || oldSubgroup.columns == null || oldSubgroup.columns.Length < 1)
            {
                return;
            }

            foreach (AppSpecificTableColumnModel4WS newASITColumn in newSubgroup.columns)
            {
                foreach (AppSpecificTableColumnModel4WS oldASITColumn in oldSubgroup.columns)
                {
                    if (!string.Equals(newASITColumn.columnName, oldASITColumn.columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (oldASITColumn.templateLayoutConfig != null)
                    {
                        if (newASITColumn.templateLayoutConfig == null)
                        {
                            newASITColumn.templateLayoutConfig = new TemplateLayoutConfigModel();
                        }

                        newASITColumn.templateLayoutConfig.waterMark = oldASITColumn.templateLayoutConfig.waterMark;
                        newASITColumn.templateLayoutConfig.instruction = oldASITColumn.templateLayoutConfig.instruction;
                        newASITColumn.templateLayoutConfig.alternativeLabel = oldASITColumn.templateLayoutConfig.alternativeLabel;

                        if (oldASITColumn.templateLayoutConfig.i18NModel != null)
                        {
                            if (newASITColumn.templateLayoutConfig.i18NModel == null)
                            {
                                newASITColumn.templateLayoutConfig.i18NModel = new TemplateLayoutConfigI18NModel();
                            }

                            newASITColumn.templateLayoutConfig.i18NModel.waterMark = oldASITColumn.templateLayoutConfig.i18NModel.waterMark;
                            newASITColumn.templateLayoutConfig.i18NModel.instruction = oldASITColumn.templateLayoutConfig.i18NModel.instruction;
                            newASITColumn.templateLayoutConfig.i18NModel.alternativeLabel = oldASITColumn.templateLayoutConfig.i18NModel.alternativeLabel;
                        }
                    }

                    break;
                }
            }
        }
        
        #endregion Methods
    }
}