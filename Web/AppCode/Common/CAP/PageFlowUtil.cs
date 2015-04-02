#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PageFlowUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PageFlowUtil.cs 260067 2013-11-07 15:10:50Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// The utility class for page flow.
    /// </summary>
    public static class PageFlowUtil
    {
        #region Fields

        /// <summary>
        /// The entity type for page flow trace in CAP temporary data.
        /// </summary>
        private const string PAGEFLOW_TRACE_ENTITY_TYPE = "PageflowTrackingRecorder";

        /// <summary>
        /// The composite format string for the key of the backend storage which indicates whether a page is loaded or not.
        /// </summary>
        private const string PAGEFLOW_IS_SETUP_PAGE_TRACE_FORMAT = "{0}_IsSetupPageTrace";

        /// <summary>
        /// The composite format string for the key of the backend storage which keeps track of page.
        /// </summary>
        private const string PAGEFLOW_TRACKING_RECORDER_FORMAT = "{0}_PageflowTrackingRecorder";

        #endregion Fields

        /// <summary>
        /// Constants to indicate in what state a page is.
        /// </summary>
        private enum PageState
        {
            /// <summary>
            /// Page is in hidden state.
            /// </summary>
            Hidden = 0,

            /// <summary>
            /// Page is in display state.
            /// </summary>
            Display = 1
        }

        /// <summary>
        /// Gets or sets a value indicating whether page flow trace of the status of the current Partial Record is update or not.
        /// </summary>
        public static bool IsPageFlowTraceUpdated
        { 
            get
            {
                object flagOfPageTraceUpdate = HttpContext.Current.Session["IsPageFlowTraceUpdated"];

                if (flagOfPageTraceUpdate != null)
                {
                    return (bool)flagOfPageTraceUpdate;
                }

                return false;
            } 

            set
            {
                HttpContext.Current.Session["IsPageFlowTraceUpdated"] = value;
            }
        }

        /// <summary>
        /// Indicates whether the page trace has been initialized.
        /// </summary>
        /// <param name="capModel">the CAP model.</param>
        /// <returns>indicates whether the page trace has been initialized.</returns>
        public static bool IsSetupPageTrace(CapModel4WS capModel)
        {
            if (capModel == null || capModel.capID == null)
            {
                return false;
            }

            string keyOfPageTrace = string.Format(PAGEFLOW_IS_SETUP_PAGE_TRACE_FORMAT, capModel.capID.toKey());
            object result = HttpContext.Current.Session[keyOfPageTrace];

            if (result == null)
            {
                return false;
            }

            return (bool)result;
        }

        /// <summary>
        /// Update the Page Trace
        /// </summary>
        /// <param name="capModel">The Cap model.</param>
        public static void UpdatePageTrace(CapModel4WS capModel)
        {
            if (capModel == null || capModel.capID == null)
            {
                return;
            }

            int[] pageTracks = GetPageStateTracking(capModel);

            if (pageTracks == null)
            {
                return;
            }

            string pageTrack = string.Join(ACAConstant.COMMA, pageTracks.Select(x => x.ToString()).ToArray());
            
            CapTemporaryDataPKModel pkModel = new CapTemporaryDataPKModel();
            pkModel.servProvCode = capModel.capID.serviceProviderCode;
            pkModel.capID1 = capModel.capID.id1;
            pkModel.capID2 = capModel.capID.id2;
            pkModel.capID3 = capModel.capID.id3;

            SimpleAuditModel auditModel = new SimpleAuditModel();
            auditModel.auditID = AppSession.User.PublicUserId;
            auditModel.auditStatus = ACAConstant.VALID_STATUS;

            CapTemporaryDataModel capTemporaryData = new CapTemporaryDataModel();
            capTemporaryData.capTemporaryDataPKModel = pkModel;
            capTemporaryData.data1 = pageTrack;
            capTemporaryData.entityType = PAGEFLOW_TRACE_ENTITY_TYPE;
            capTemporaryData.auditModel = auditModel;

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            capBll.UpdateCapTemporaryData(capTemporaryData);
        }
        
        /// <summary>
        /// Gets the corresponding page flow settings associated with the specified capModel.
        /// </summary>
        /// <param name="capModel">The capModel to be checked.</param>
        /// <returns>An instance of type PageFlowGroupModel that represents the corresponding page flow settings.</returns>
        public static PageFlowGroupModel GetPageflowGroup(CapModel4WS capModel)
        {
            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();

            if (pageflowGroup == null)
            {
                IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
                pageflowGroup = pageflowBll.GetPageflowGroupByCapType(capModel.capType);
                pageflowGroup = CapUtil.GetPageFlowWithoutBlankPage(capModel, pageflowGroup);

                /* This situation only existing the sub record has no associated page flow and using parent record type's page flow to edit the record on Review Page.
                 * If the parent page flow changes, it should get latest the parent page flow to load Spear Form under Multiple Agencies Environment.
                 */
                if (pageflowGroup == null)
                {
                    pageflowGroup = GetAssociatedParentPageFlowGroup(capModel);
                }

                AppSession.SetPageflowGroupToSession(pageflowGroup);
            }

            return pageflowGroup;
        }

        /// <summary>
        /// Check the CapModel's record type whether it has associated Page Flow or not.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <returns>Indicate the CapModel's record type whether it has associated Page Flow or not.</returns>
        public static bool HasAssociatedPageFlowGroup(CapModel4WS capModel)
        {
            PageFlowGroupModel pageFlow = GetAssociatedPageFlowGroup(capModel);

            return pageFlow != null;
        }

        /// <summary>
        /// Gets the associated page flow group.
        /// </summary>
        /// <param name="capModel">
        /// Cap model. Will use the capModel.capType to get the associated page flow group.
        /// In super agency environment, try to use capModel.refID to get the page flow of parent cap if the page flow of current cap is null.
        /// </param>
        /// <returns>return PageFlowGroupModel if the cap type of the specified <see cref="capModel"/> associated with a valid page flow.</returns>
        public static PageFlowGroupModel GetAssociatedPageFlowGroup(CapModel4WS capModel)
        {
            IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
            PageFlowGroupModel pageFlowGroup = null;

            if (capModel != null && capModel.capType != null)
            {
                pageFlowGroup = pageflowBll.GetPageflowGroupByCapType(capModel.capType);
            }

            if (pageFlowGroup == null && StandardChoiceUtil.IsSuperAgency())
            {
                pageFlowGroup = GetAssociatedParentPageFlowGroup(capModel);
            }

            return pageFlowGroup;
        }

        /// <summary>
        /// Get the associated Parent Record's page flow group.
        /// </summary>
        /// <param name="capModel">
        /// In super agency environment, try to use capModel.refID to get the page flow of parent cap if the page flow of current cap is null
        /// </param>
        /// <returns>The associated parent record's page flow group.</returns>
        public static PageFlowGroupModel GetAssociatedParentPageFlowGroup(CapModel4WS capModel)
        {
            IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
            PageFlowGroupModel pageFlowGroup = null;

            if (capModel != null && !string.IsNullOrEmpty(capModel.refID))
            {
                string[] parentCap = capModel.refID.Split('-');
                CapIDModel parentCapId = new CapIDModel
                {
                    serviceProviderCode = parentCap[0],
                    ID1 = parentCap[1],
                    ID2 = parentCap[2],
                    ID3 = parentCap[3]
                };

                pageFlowGroup = pageflowBll.GetPageFlowGroupByCapID(parentCapId);
            }
            
            return pageFlowGroup;
        }

        /// <summary>
        /// Get the parent component name and children component name if the two page flow configuration is same.
        /// </summary>
        /// <param name="parentPageFlowGroup">The parent page flow.</param>
        /// <param name="childPageFlowGroup">The child page flow.</param>
        /// <returns>Check whether the two page flow configuration is the same.</returns>
        public static Dictionary<string, string> CheckTheSamePageFlow(PageFlowGroupModel parentPageFlowGroup, PageFlowGroupModel childPageFlowGroup)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            if (parentPageFlowGroup == null || childPageFlowGroup == null || parentPageFlowGroup.stepList.Length != childPageFlowGroup.stepList.Length)
            {
                return results;
            }

            for (int stepIndex = 0; stepIndex < parentPageFlowGroup.stepList.Length; stepIndex++)
            {
                StepModel parentStep = parentPageFlowGroup.stepList[stepIndex];
                StepModel childStep = childPageFlowGroup.stepList[stepIndex];

                for (int pageIndex = 0; pageIndex < parentStep.pageList.Length; pageIndex++)
                {
                    if (childStep.pageList.Length - 1 < pageIndex)
                    {
                        return null;
                    }

                    PageModel parentPage = parentStep.pageList[pageIndex];
                    PageModel childPage = childStep.pageList[pageIndex];

                    for (int cptIndex = 0; cptIndex < parentPage.componentList.Length; cptIndex++)
                    {
                        if (childPage.componentList.Length - 1 < cptIndex)
                        {
                            return null;
                        }

                        ComponentModel parentComponent = parentPage.componentList[cptIndex];
                        ComponentModel childComponent = childPage.componentList[cptIndex];

                        if (parentComponent.componentID != childComponent.componentID)
                        {
                            return null;
                        }

                        string parentCptKey = ConstructComponentKey(parentComponent);
                        string childCptKey = ConstructComponentKey(childComponent);

                        if (!string.IsNullOrEmpty(parentCptKey) && !string.IsNullOrEmpty(childCptKey))
                        {
                            results.Add(parentCptKey, childCptKey); 
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Update the component name for the contact and LP record in CapModel.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="componentNameMapping">The component name's mapping.</param>
        /// <returns>The cap model that updated component name.</returns>
        public static CapModel4WS UpdateExistingComponentNameForCapModel(CapModel4WS capModel, Dictionary<string, string> componentNameMapping)
        {
            if (capModel.contactsGroup != null && capModel.contactsGroup.Length > 0)
            {
                foreach (CapContactModel4WS contactRecord in capModel.contactsGroup)
                {
                    if (!string.IsNullOrEmpty(contactRecord.componentName) && componentNameMapping.ContainsKey(contactRecord.componentName))
                    {
                        contactRecord.componentName = componentNameMapping[contactRecord.componentName];
                    }            
                }
            }

            if (capModel.licenseProfessionalList != null && capModel.licenseProfessionalList.Length > 0)
            {
                foreach (LicenseProfessionalModel4WS lp in capModel.licenseProfessionalList)
                {
                    if (!string.IsNullOrEmpty(lp.componentName) && componentNameMapping.ContainsKey(lp.componentName))
                    {
                        lp.componentName = componentNameMapping[lp.componentName];
                    }
                }
            }

            return capModel;
        }

        /// <summary>
        /// Update the Component Name for the Contact and LP Records in CapModel when click Edit button for partial record in Cart List 
        /// or Click View link for partial record in Associated Form back to Review page.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="pageFlow">The page flow.</param>
        /// <returns>The cap model that updated record.</returns>
        public static CapModel4WS UpdateRecordDataForCapModel(CapModel4WS capModel, PageFlowGroupModel pageFlow)
        {
            ContactUtil.PrepareContactsForCopyingRecord(capModel, pageFlow);
            LicenseUtil.PrepareLicensesForCopyingRecord(capModel, pageFlow);

            foreach (StepModel step in pageFlow.stepList)
            {
                foreach (PageModel page in step.pageList)
                {
                    foreach (ComponentModel cptModel in page.componentList)
                    {
                        UpdateContactRecordForCapModel(capModel, cptModel);
                        UpdateLPRecordForCapModel(capModel, cptModel);
                    }
                }
            }

            return capModel;
        }

        /// <summary>
        /// Gets the name of the current page flow.
        /// </summary>
        /// <returns>The name of the current page flow.</returns>
        public static string GetPageFlowName()
        {
            string pageFlowName = string.Empty;

            if (AppSession.IsAdmin)
            {
                pageFlowName = HttpContext.Current.Request[ACAConstant.PAGE_FLOW_NAME];
                pageFlowName = ScriptFilter.DecodeJson(pageFlowName);
            }
            else
            {
                PageFlowGroupModel pageFlowModel = AppSession.GetPageflowGroupFromSession();

                if (pageFlowModel != null)
                {
                    pageFlowName = pageFlowModel.pageFlowGrpCode;
                }
            }

            return pageFlowName;
        }

        /// <summary>
        /// iS contact component exist in current page flow.
        /// </summary>
        /// <returns>True: exist, otherwise not exist.</returns>
        public static bool IsContactComponentExist()
        {
            if (IsComponentExist(PageFlowComponent.APPLICANT)
                || IsComponentExist(PageFlowComponent.CONTACT_1)
                || IsComponentExist(PageFlowComponent.CONTACT_2)
                || IsComponentExist(PageFlowComponent.CONTACT_3)
                || IsComponentExist(PageFlowComponent.CONTACT_LIST))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Is education、exam or continuing education exist in current page flow.
        /// </summary>
        /// <returns>True: exist, otherwise not exist.</returns>
        public static bool IsEduExamComponentExist()
        {
            if (IsComponentExist(PageFlowComponent.EDUCATION)
                || IsComponentExist(PageFlowComponent.EXAMINATION)
                || IsComponentExist(PageFlowComponent.CONTINUING_EDUCATION))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a component is in the current page flow.
        /// </summary>
        /// <param name="component">The page flow component enumeration used to identify a component.</param>
        /// <returns>true, if found; otherwise, false.</returns>
        public static bool IsComponentExist(PageFlowComponent component)
        {
            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();

            foreach (StepModel stepModel in pageflowGroup.stepList)
            {
                foreach (PageModel pageModel in stepModel.pageList)
                {
                    if (pageModel.componentList.Any(cptModel => cptModel.componentID == (long)component))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether a component identified by componentName is in the current page flow.
        /// </summary>
        /// <param name="componentName">The name of a component used to identify a component.</param>
        /// <returns>true, if found; otherwise, false.</returns>
        public static bool IsComponentExist(string componentName)
        {
            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();

            foreach (StepModel stepModel in pageflowGroup.stepList)
            {
                foreach (PageModel pageModel in stepModel.pageList)
                {
                    if (pageModel.componentList.Any(cptModel => cptModel.componentName.ToUpperInvariant().Equals(componentName, StringComparison.InvariantCulture)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified page flow contains the same contact related components.
        /// </summary>
        /// <param name="pageFlowGroup">The page flow to be checked.</param>
        /// <param name="componentNames">The list of component names that can be used to contrast.</param>
        /// <param name="existComponentNames">The list of component names are included in the Page Flow.</param>
        /// <returns>true if it contains the same components; otherwise, false.</returns>
        public static bool IsSamePageflow4Contact(PageFlowGroupModel pageFlowGroup, List<string> componentNames, ref List<string> existComponentNames)
        {
            List<string> cptNameList = new List<string>(componentNames);

            foreach (StepModel step in pageFlowGroup.stepList)
            {
                foreach (PageModel page in step.pageList)
                {
                    foreach (ComponentModel component in page.componentList)
                    {
                        string cptName = string.Empty;
                        bool hasContact = false;

                        switch (component.componentName.ToUpperInvariant())
                        {
                            case GViewConstant.SECTION_APPLICANT:
                                cptName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_APPLICANT_PREFIX, component.componentSeqNbr);
                                hasContact = true;
                                break;
                            case GViewConstant.SECTION_CONTACT1:
                                cptName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT1_PREFIX, component.componentSeqNbr);
                                hasContact = true;
                                break;
                            case GViewConstant.SECTION_CONTACT2:
                                cptName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT2_PREFIX, component.componentSeqNbr);
                                hasContact = true;
                                break;
                            case GViewConstant.SECTION_CONTACT3:
                                cptName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT3_PREFIX, component.componentSeqNbr);
                                hasContact = true;
                                break;
                            case GViewConstant.SECTION_MULTIPLE_CONTACTS:
                                cptName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX, component.componentSeqNbr);
                                hasContact = true;
                                break;
                        }

                        if (hasContact && componentNames.Contains(cptName))
                        {
                            cptNameList.Remove(cptName);
                            existComponentNames.Add(cptName);
                        }
                    }
                }
            }

            // If cptNameList.Count = 0, means all the Contact component Names in componentNames are included in Page Flow.
            if (cptNameList.Count > 0)
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// compare current page flow setting with page flow setting in cache.
        /// </summary>
        /// <param name="capModel">cap model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="pageflowGroup">page flow</param>
        /// <returns>true - is same page flow, false- page flow was change</returns>
        public static bool IsPageflowChanged(CapModel4WS capModel, string moduleName, PageFlowGroupModel pageflowGroup)
        {
            if (pageflowGroup == null)
            {
                return false;
            }
            
            int[] oldPageStateTracking = GetPageTrace(capModel.capID);

            if (oldPageStateTracking == null)
            {
                return false;
            }

            int[] newPageStateTracking = BuildPageStateTrackingByPageFlowSetting(capModel, moduleName, pageflowGroup);

            if (oldPageStateTracking.Length != newPageStateTracking.Length || oldPageStateTracking[0] != newPageStateTracking[0])
            {
                return true;
            }

            int stepNum = oldPageStateTracking[0];

            for (int i = 0; i < stepNum; i++)
            {
                if (oldPageStateTracking[i + 1] != newPageStateTracking[i + 1])
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// update page flow trace by page flow setting
        /// </summary>
        /// <param name="capModel">cap model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="pageflowGroup">page flow group</param>
        public static void UpdatePageFlowTraceByPageFlowSetting(CapModel4WS capModel, string moduleName, PageFlowGroupModel pageflowGroup)
        {
            int[] latestTrace = BuildPageStateTrackingByPageFlowSetting(capModel, moduleName, pageflowGroup);

            if (latestTrace != null)
            {
                SetPageStateTracking(capModel, latestTrace);
                UpdatePageTrace(capModel);
            }
        }

        /// <summary>
        /// Determines whether the specified page flow contains the same license related components.
        /// </summary>
        /// <param name="pageFlowGroup">The page flow to be checked.</param>
        /// <param name="componentNames">The list of component names that can be used to contrast.</param>
        /// <param name="existComponentNames">The list of component names are included in the Page Flow.</param>
        /// <returns>True if it contains the same components; otherwise, false.</returns>
        public static bool IsSamePageflow4License(PageFlowGroupModel pageFlowGroup, List<string> componentNames, ref List<string> existComponentNames)
        {
            List<string> cptNameList = new List<string>(componentNames);

            foreach (StepModel step in pageFlowGroup.stepList)
            {
                foreach (PageModel page in step.pageList)
                {
                    foreach (ComponentModel component in page.componentList)
                    {
                        string cptName = string.Empty;
                        bool hasLicense = false;

                        switch (component.componentName.ToUpperInvariant())
                        {
                            case GViewConstant.SECTION_LICENSE:
                                cptName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_LICENSE_PREFIX, component.componentSeqNbr);
                                hasLicense = true;
                                break;
                            case GViewConstant.SECTION_MULTIPLE_LICENSES:
                                cptName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_MULTI_LICENSE_PREFIX, component.componentSeqNbr);
                                hasLicense = true;
                                break;
                        }

                        if (hasLicense && componentNames.Contains(cptName))
                        {
                            existComponentNames.Add(cptName);
                            cptNameList.Remove(cptName);
                        }
                    }
                }
            }

            // If cptNameList.Count = 0, means all the LP/LP List component Names in componentNames are included in Page Flow.
            if (cptNameList.Count > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Finds a component which matches the search criteria.
        /// </summary>
        /// <param name="pageflowGroup">The page flow to be checked.</param>
        /// <param name="searchComponentModel">The search criteria.</param>
        /// <param name="currentStep">The current step.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns>If find the component, return the component, else return null.</returns>
        public static ComponentModel FindComponent(PageFlowGroupModel pageflowGroup, ComponentModel searchComponentModel, out int currentStep, out int currentPage)
        {
            currentStep = -1;
            currentPage = -1;

            if (pageflowGroup == null || pageflowGroup.stepList == null || pageflowGroup.stepList.Length == 0)
            {
                return null;
            }

            bool isSupportMultipleComponent =
                searchComponentModel.componentID == (long)PageFlowComponent.APPLICANT
                || searchComponentModel.componentID == (long)PageFlowComponent.CONTACT_1
                || searchComponentModel.componentID == (long)PageFlowComponent.CONTACT_2
                || searchComponentModel.componentID == (long)PageFlowComponent.CONTACT_3
                || searchComponentModel.componentID == (long)PageFlowComponent.CONTACT_LIST
                || searchComponentModel.componentID == (long)PageFlowComponent.LICENSED_PROFESSIONAL
                || searchComponentModel.componentID == (long)PageFlowComponent.ATTACHMENT
                || searchComponentModel.componentID == (long)PageFlowComponent.ASI
                || searchComponentModel.componentID == (long)PageFlowComponent.ASI_TABLE;
              
            // Find out the current component and get the Page Flow's StepOrder and PageOrder.
            foreach (StepModel stepModel in pageflowGroup.stepList)
            {
                currentStep++;
                currentPage = -1;

                foreach (PageModel pageModel in stepModel.pageList)
                {
                    currentPage++;

                    foreach (ComponentModel componentModel in pageModel.componentList)
                    {
                        if (componentModel.componentID == searchComponentModel.componentID && 
                            (!isSupportMultipleComponent || componentModel.componentSeqNbr == searchComponentModel.componentSeqNbr))
                        {
                            return componentModel;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get component name for page flow.
        /// </summary>
        /// <param name="pageFlowGroup">The page flow to be checked.</param>
        /// <returns>Return all attachment component name.</returns>
        public static List<string> GetAttachmentComponentName4PageFlow(PageFlowGroupModel pageFlowGroup)
        {
            List<string> cptNameList4PageFlow = new List<string>();

            foreach (StepModel step in pageFlowGroup.stepList)
            {
                foreach (PageModel page in step.pageList)
                {
                    foreach (ComponentModel component in page.componentList)
                    {
                        if (component.componentName.ToUpperInvariant().Equals(GViewConstant.SECTION_ATTACHMENT))
                        {
                            string cptName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_ATTACHMENT_PREFIX, component.componentSeqNbr);
                            cptNameList4PageFlow.Add(cptName);
                        }
                    }
                }
            }

            return cptNameList4PageFlow;
        }

        #region Page Flow Trace

        /// <summary>
        /// Sets up the page trace for the specified page flow group.
        /// </summary>
        /// <param name="capModel">The unique identifier for the associated record whose page trace will be set.</param>
        /// <param name="pageflowGroup">The page flow to be tracked.</param>
        public static void SetupPageTrace(CapModel4WS capModel, PageFlowGroupModel pageflowGroup)
        {
            Dictionary<int, int> pageCountInStep = GetPageCountOfEachStep(pageflowGroup);

            if (pageCountInStep.Count == 0)
            {
                return;
            }

            int[] backendStorage = BuildBackendStorageForPageStateTracking(pageCountInStep);

            SetPageStateTracking(capModel, backendStorage);

            MarkPageTraceIsSetup(capModel, true);
        }

        /// <summary>
        /// Sets the state of the page trace to un-tracking. 
        /// </summary>
        /// <param name="capModel">The associated record whose page trace will be reset.</param>
        public static void ResetPageTrace(CapModel4WS capModel)
        {
            MarkPageTraceIsSetup(capModel, false);
            SetPageStateTracking(capModel, null);
        }

        /// <summary>
        /// Marks the state of the specified page as loaded.
        /// </summary>
        /// <param name="capModel">The associated record whose page will be marked as loaded.</param>
        /// <param name="step">The 32-bit integer that represents the step in which the page resides.</param>
        /// <param name="page">The 32-bit integer that represents the page to be checked.</param>
        public static void MarkPageAsLoaded(CapModel4WS capModel, int step, int page)
        {
            UpdatePageState(capModel, step, page, PageState.Display);
        }

        /// <summary>
        /// Marks the state of the specified page as un-loaded.
        /// </summary>
        /// <param name="capModel">The associated record whose page will be marked as un-loaded (hidden).</param>
        /// <param name="step">The 32-bit integer that represents the step in which the page resides.</param>
        /// <param name="page">The 32-bit integer that represents the page to be checked.</param>
        public static void MarkPageAsUnLoaded(CapModel4WS capModel, int step, int page)
        {
            UpdatePageState(capModel, step, page, PageState.Hidden);
        }

        /// <summary>
        /// Indicates whether the specified page resided in the specified step is hidden.
        /// </summary>
        /// <param name="capModel">The associated record whose page will be checked.</param>
        /// <param name="step">The 32-bit integer that represents the step in which the page resides.</param>
        /// <param name="page">The 32-bit integer that represents the page to be checked.</param>
        /// <returns>true if the specified page in the specified step is hidden; otherwise, false.</returns>
        public static bool IsHidden(CapModel4WS capModel, int step, int page)
        {
            int[] pageTrackingRecorder = GetPageStateTracking(capModel);

            if (pageTrackingRecorder == null)
            {
                return false;
            }

            step++;
            page++;

            return pageTrackingRecorder[pageTrackingRecorder[step] + page] == 0;
        }

        /// <summary>
        /// Removes the data associated with a component in which resides the hidden page.
        /// </summary>
        /// <param name="pageFlowGroup">The page flow to be checked.</param>
        /// <param name="capModel">The capModel whose data will be removed, if needed.</param>
        public static void RemoveComponentDataInHiddenPage(PageFlowGroupModel pageFlowGroup, CapModel4WS capModel)
        {
            if (pageFlowGroup == null || capModel == null || capModel.capID == null)
            {
                return;
            }

            Dictionary<int, List<int>> hiddenStepsAndPages = GetHiddenStepsAndPages(capModel);

            // Nothing to do if there is no any hidden pages.
            if (hiddenStepsAndPages == null)
            {
                return;
            }

            foreach (int step in hiddenStepsAndPages.Keys)
            {
                List<int> pages = hiddenStepsAndPages[step];

                foreach (int page in pages)
                {
                    PageModel pageModel = pageFlowGroup.stepList[step - 1].pageList[page - 1];

                    foreach (ComponentModel component in pageModel.componentList)
                    {
                        RemoveComponentData(capModel, component);
                    }
                }
            }

            //Clear the component data in hidden page for the CapModel.
            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();

            /* Need update the capModel data to DB after remove the component data in hidden page
             * after click continue application button on last visible Spear Form page before Review Page.
             * Otherwise, some component data cleared by RemoveComponentData() function will still exists in DB 
             * if public user leave review page and resume application with change the hidde page condition to visible.
             */
            if (CapUtil.IsSuperCAP(capModel.moduleName))
            {
                CapUtil.FilterSameLicenseType(capModel);
                capModel = capBll.SavePartialCaps(capModel);
            }
            else
            {
                capModel = capBll.SaveWrapperForPartialCap(capModel, string.Empty, false);
            }

            capBll.ClearCapData(capModel, pageFlowGroup);
        }

        /// <summary>
        /// Refresh page tracking for state
        /// </summary>
        /// <param name="capModel">The cap model</param>
        public static void RefreshPageStateTracking(CapModel4WS capModel)
        {
            int[] result = GetPageTrace(capModel.capID);

            if (result != null)
            {
                SetPageStateTracking(capModel, result);
            }
        }

        /// <summary>
        /// Get the page count of each step.
        /// </summary>
        /// <param name="pageflowGroup">The page flow group.</param>
        /// <returns>The page count of each step.</returns>
        private static Dictionary<int, int> GetPageCountOfEachStep(PageFlowGroupModel pageflowGroup)
        {
            /* 
             * Page Trace Initialization data structure
             * Uses a variable of type Dictionary to represent the collection of how many pages in each step.
             * For each item in the corresponding dictionary, the key is the step number and the value is the amount of pages in the specified step.
             */
            Dictionary<int, int> pageCountInStep = new Dictionary<int, int>();

            for (int stepIndex = 1; stepIndex <= pageflowGroup.stepList.Length; stepIndex++)
            {
                pageCountInStep.Add(stepIndex, pageflowGroup.stepList[stepIndex - 1].pageList.Length);
            }

            return pageCountInStep;
        }

        /// <summary>
        /// Update Component Names of the contact records for the CapModel according to the Page Flow Component.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="cptModel">The component model.</param>
        private static void UpdateContactRecordForCapModel(CapModel4WS capModel, ComponentModel cptModel)
        {
            if (capModel.contactsGroup == null || capModel.contactsGroup.Length == 0)
            {
                return;
            }

            string cptName = ConstructComponentKey(cptModel);

            if (cptModel.componentID == (long)PageFlowComponent.APPLICANT
                || cptModel.componentID == (long)PageFlowComponent.CONTACT_1
                || cptModel.componentID == (long)PageFlowComponent.CONTACT_2
                || cptModel.componentID == (long)PageFlowComponent.CONTACT_3)
            {
                int index = cptName.IndexOf('_');
                string cptKeyPrefix = cptName.Substring(0, index);

                CapContactModel4WS contact = ContactUtil.FindContactWithComponentName(capModel.contactsGroup, cptName) ??
                                             ContactUtil.FindContactWithSectionNamePrefix(capModel.contactsGroup, cptKeyPrefix);

                if (contact == null)
                {
                    List<CapContactModel4WS> contactGroup = new List<CapContactModel4WS>(capModel.contactsGroup);
                    contact = contactGroup.Find(c => string.IsNullOrEmpty(c.componentName));
                }

                if (contact != null)
                {
                    contact.validateFlag = cptModel.validateFlag;
                    contact.componentName = cptName;
                }
            }

            if (cptModel.componentID == (long)PageFlowComponent.CONTACT_LIST)
            {
                List<CapContactModel4WS> contactsGroupList = new List<CapContactModel4WS>();

                foreach (CapContactModel4WS contact in capModel.contactsGroup)
                {
                    if (contact == null || !cptName.Equals(contact.componentName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    contact.validateFlag = cptModel.validateFlag;
                    contactsGroupList.Add(contact);
                }

                CapContactModel4WS[] tmpContacts = ContactUtil.FindAppropriatedContacts(capModel.contactsGroup);
                contactsGroupList.AddRange(tmpContacts);
                contactsGroupList.ForEach(contact => contact.componentName = cptName);
            }
        }

        /// <summary>
        /// Update Component Names of the LP records for the CapModel according to the Page Flow Component.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="cptModel">The component model.</param>
        private static void UpdateLPRecordForCapModel(CapModel4WS capModel, ComponentModel cptModel)
        {
            if (capModel.licenseProfessionalList == null || capModel.licenseProfessionalList.Length == 0)
            {
                return;
            }

            string cptName = ConstructComponentKey(cptModel);

            if (cptModel.componentID == (long)PageFlowComponent.LICENSED_PROFESSIONAL)
            {
                LicenseProfessionalModel4WS[] licenseProfessionalList = capModel.licenseProfessionalList;
                string serviceProviderCode = capModel.capID != null ? capModel.capID.serviceProviderCode : string.Empty;
                LicenseUtil.ResetLicenseeAgency(licenseProfessionalList, serviceProviderCode);

                // Find the approprate license from the list of license professional list of capModel
                LicenseProfessionalModel4WS licenseProfessional = LicenseUtil.FindLicenseProfessionalWithComponentName(capModel, cptName);

                // if it from clone record and not find LP, set the first LP whose component name is empty.
                if (licenseProfessional == null)
                {
                    licenseProfessional = licenseProfessionalList.ToList().Find(lp => string.IsNullOrEmpty(lp.componentName));
                }

                if (licenseProfessional != null)
                {
                    licenseProfessional.componentName = cptName;
                }
            }

            if (cptModel.componentID == (long)PageFlowComponent.LICENSED_PROFESSIONAL_LIST)
            {
                /* When rendering the content of LicenseProfessionalList control, we have to remove those items which do not belong to it.
                 * It means an item which belongs to License Professional will be removed.
                 */
                LicenseProfessionalModel4WS[] lpBelongToLPList = LicenseUtil.FindLicenseProfessionalsWithComponentName(capModel, cptName);
                List<LicenseProfessionalModel4WS> remainUnCopiedLP = new List<LicenseProfessionalModel4WS>();

                if (capModel.licenseProfessionalList != null && capModel.licenseProfessionalList.Length > 0)
                {
                    remainUnCopiedLP = capModel.licenseProfessionalList.Where(lp => string.IsNullOrEmpty(lp.componentName)).ToList();
                }

                // If not any LP belong to the LP List component, mean the page flow has changed, it will put all LP whose component name is empty to the LP List component.
                if ((lpBelongToLPList == null || lpBelongToLPList.Length == 0 || remainUnCopiedLP.Count > 0)
                    && capModel.licenseProfessionalList != null
                    && capModel.licenseProfessionalList.Length > 0)
                {
                    List<LicenseProfessionalModel4WS> tempLPList = remainUnCopiedLP;
                    tempLPList.ForEach(lp => lp.componentName = cptName);

                    /*
                     * If the Component Name of the LP List component is not changed and still exists some LP records has no Component Name,
                     * all these LP records with no component name will be auto added to the current(first) LP list.
                     */
                    if (lpBelongToLPList != null && lpBelongToLPList.Length > 0)
                    {
                        tempLPList.AddRange(lpBelongToLPList);
                    }

                    lpBelongToLPList = tempLPList.ToArray();
                }

                string serviceProviderCode = capModel.capID != null ? capModel.capID.serviceProviderCode : string.Empty;
                LicenseUtil.ResetLicenseeAgency(lpBelongToLPList, serviceProviderCode);
            }
        }

        /// <summary>
        /// Sets the data to the corresponding backend storage for page trace.
        /// </summary>
        /// <param name="capModel">The associated record whose backend storage will be updated.</param>
        /// <param name="isSetup">The page trace whether has setup or not.</param>
        private static void MarkPageTraceIsSetup(CapModel4WS capModel, bool isSetup)
        {
            if (capModel == null || capModel.capID == null)
            {
                return;
            }

            string keyOfPageTrace = string.Format(PAGEFLOW_IS_SETUP_PAGE_TRACE_FORMAT, capModel.capID.toKey());
            HttpContext.Current.Session[keyOfPageTrace] = isSetup;
        }

        /// <summary>
        /// Gets the backend storage of page trace for the specific record.
        /// </summary>
        /// <param name="capModel">The unique identifier for the associated record whose backend storage will be returned.</param>
        /// <returns>The backend storage associated with the specific record.</returns>
        private static int[] GetPageStateTracking(CapModel4WS capModel)
        {
            if (capModel == null || capModel.capID == null)
            {
                return null;
            }

            string keyOfRecorder = string.Format(PAGEFLOW_TRACKING_RECORDER_FORMAT, capModel.capID.toKey());
            int[] result = (int[])HttpContext.Current.Session[keyOfRecorder];

            if (result == null)
            {
                result = GetPageTrace(capModel.capID);

                if (result != null)
                {
                    PageFlowGroupModel pageFlowGroup = GetPageflowGroup(capModel);

                    result = AdjustPageStateTracking(pageFlowGroup, result);

                    SetPageStateTracking(capModel, result);

                    MarkPageTraceIsSetup(capModel, true);
                }
            }

            return result;
        }

        /// <summary>
        /// Get the adjusted page state tracking.
        /// </summary>
        /// <param name="pageFlowGroup">The page flow group.</param>
        /// <param name="oldPageStateTracking">The old page state tracking.</param>
        /// <returns>The adjusted page state tracking.</returns>
        private static int[] AdjustPageStateTracking(PageFlowGroupModel pageFlowGroup, int[] oldPageStateTracking)
        {
            Dictionary<int, int> pageCountInStep = GetPageCountOfEachStep(pageFlowGroup);

            int[] newPageStateTracking = BuildBackendStorageForPageStateTracking(pageCountInStep);

            if (oldPageStateTracking.Length != newPageStateTracking.Length)
            {
                return newPageStateTracking;
            }

            if (oldPageStateTracking[0] != newPageStateTracking[0])
            {
                return newPageStateTracking;
            }

            for (int i = 1; i <= newPageStateTracking[0]; i++)
            {
                if (oldPageStateTracking[i] != newPageStateTracking[i])
                {
                    return newPageStateTracking;
                }
            }

            return oldPageStateTracking;
        }

        /// <summary>
        /// Sets the backend storage of page trace for the specific record.
        /// </summary>
        /// <param name="capModel">The associated record whose backend storage will be updated.</param>
        /// <param name="data">The page tracking recorder.</param>
        private static void SetPageStateTracking(CapModel4WS capModel, object data)
        {
            if (capModel == null || capModel.capID == null)
            {
                return;
            }

            string keyOfRecorder = string.Format(PAGEFLOW_TRACKING_RECORDER_FORMAT, capModel.capID.toKey());
            HttpContext.Current.Session[keyOfRecorder] = data;
        }

        /// <summary>
        /// Update the page state.
        /// </summary>
        /// <param name="capModel">The associated record whose backend storage will be updated.</param>
        /// <param name="step">The 32-bit integer that represents the step in which the page resides.</param>
        /// <param name="page">The 32-bit integer that represents the page to be checked.</param>
        /// <param name="state">The state to be set.</param>
        private static void UpdatePageState(CapModel4WS capModel, int step, int page, PageState state)
        {
            int[] pageTrackingRecorder = GetPageStateTracking(capModel);

            if (pageTrackingRecorder == null)
            {
                return;
            }

            /*
             * We started the array with a column storing the amount of steps so that step and page numbers can run from the natural, for example 1 to 3 instead of 0 to 2.
             */
            step++;
            page++;

            pageTrackingRecorder[pageTrackingRecorder[step] + page] = (int)state;

            SetPageStateTracking(capModel, pageTrackingRecorder);
        }

        /// <summary>
        /// Gets tracking records for hidden pages.
        /// </summary>
        /// <param name="capModel">The associated record.</param>
        /// <returns>A data structure of type Dictionary that represents the number of hidden pages in each step, if any; otherwise, null. 
        /// The key of type Dictionary represents the step number; the values mapped to the given key represent the hidden page number.</returns>
        private static Dictionary<int, List<int>> GetHiddenStepsAndPages(CapModel4WS capModel)
        {
            int[] recorder = GetPageStateTracking(capModel);

            if (recorder == null)
            {
                return null;
            }

            Dictionary<int, List<int>> results = new Dictionary<int, List<int>>();           

            // the first element represents the amount of steps. 
            int totalSteps = recorder[0];

            // check the hidden page step by step except for the last step.
            for (int stepIndex = 1; stepIndex + 1 <= totalSteps; stepIndex++)
            {
                // get the amount of pages in each step.
                int pageCount = recorder[stepIndex + 1] - recorder[stepIndex];
                CheckForHiddenPages(results, recorder, stepIndex, pageCount);
            }

            // check the hidden page of the last step.
            int pageCountInLastStep = recorder.Length - recorder[totalSteps] - 1;
            CheckForHiddenPages(results, recorder, totalSteps, pageCountInLastStep);

            return results.Count > 0 ? results : null;
        }

        /// <summary>
        /// Removes the data which belongs to the specified component from the capModel.
        /// </summary>
        /// <param name="capModel">The capModel to be checked.</param>
        /// <param name="component">The component whose data will be removed.</param>
        private static void RemoveComponentData(CapModel4WS capModel, ComponentModel component)
        {
            string componentName = string.Empty;

            PageFlowComponent componentId = EnumUtil<PageFlowComponent>.Parse(component.componentID.ToString(), PageFlowComponent.UNKNOWN);

            switch (componentId)
            {
                case PageFlowComponent.APPLICANT:
                    componentName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_APPLICANT_PREFIX, component.componentSeqNbr);
                    ContactUtil.RemoveRedundantContactsWithComponentName(capModel, componentName);
                    break;
                case PageFlowComponent.CONTACT_1:
                    componentName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT1_PREFIX, component.componentSeqNbr);
                    ContactUtil.RemoveRedundantContactsWithComponentName(capModel, componentName);
                    break;
                case PageFlowComponent.CONTACT_2:
                    componentName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT2_PREFIX, component.componentSeqNbr);
                    ContactUtil.RemoveRedundantContactsWithComponentName(capModel, componentName);
                    break;
                case PageFlowComponent.CONTACT_3:
                    componentName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT3_PREFIX, component.componentSeqNbr);
                    ContactUtil.RemoveRedundantContactsWithComponentName(capModel, componentName);
                    break;
                case PageFlowComponent.CONTACT_LIST:
                    componentName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX, component.componentSeqNbr);
                    ContactUtil.RemoveRedundantContactsWithComponentName(capModel, componentName);
                    break;
                case PageFlowComponent.LICENSED_PROFESSIONAL:
                    componentName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_LICENSE_PREFIX, component.componentSeqNbr);
                    LicenseUtil.RemoveRedundantLPsWithComponentName(capModel, componentName);
                    break;
                case PageFlowComponent.LICENSED_PROFESSIONAL_LIST:
                    componentName = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_MULTI_LICENSE_PREFIX, component.componentSeqNbr);
                    LicenseUtil.RemoveRedundantLPsWithComponentName(capModel, componentName);
                    break;
                case PageFlowComponent.ADDITIONAL_INFORMATION:
                    //The job value can't be set to string.Empty, it will meet technical difficulties during the calculate total fee function.
                    if (capModel.bvaluatnModel != null)
                    {
                        capModel.bvaluatnModel.estimatedValue = "0";
                    }

                    /* Below fields are in capDetailModel, if hide the Additional Information Section, all the value of the fields should be cleared up.
                     * The capDetailModel can't be cleared up to null.
                     * The value of these fields can be set to null, because the null value can't be updated to DB in Cap Webservice.
                     */
                    if (capModel.capDetailModel != null)
                    {
                        capModel.capDetailModel.buildingCount = 0;
                        capModel.capDetailModel.houseCount = 0;
                        capModel.capDetailModel.constTypeCode = string.Empty;
                        capModel.capDetailModel.publicOwned = string.Empty;
                    }

                    break;
                case PageFlowComponent.ADDRESS:
                    capModel.addressModel = null;
                    capModel.addressModels = null;
                    break;
                case PageFlowComponent.ASI:
                    string asiGroup = component.portletRange1;
                    string asiSubGroup = component.portletRange2;
                    ASIBaseUC.RemoveRedundantASIs(capModel, asiGroup, asiSubGroup);
                    break;
                case PageFlowComponent.ASI_TABLE:
                    string asitGroup = component.portletRange1;
                    string asitSubGroup = component.portletRange2;
                    ASIBaseUC.RemoveRedundantASITs(capModel, asitGroup, asitSubGroup);
                    break;
                case PageFlowComponent.ASSETS:
                    capModel.assetList = null;
                    break;
                case PageFlowComponent.CONTINUING_EDUCATION:
                    capModel.contEducationList = null;
                    break;
                case PageFlowComponent.DETAIL_INFORMATION:
                    /* Below fields are in Detail Information Section, if hide the page has Detail Information Section, the value of these fields should be cleared up.
                     * The value of these fields can be set to null, because the null value can't be updated to DB in Cap Webservice.
                     */
                    if (capModel.capWorkDesModel != null)
                    {
                        capModel.capWorkDesModel.description = string.Empty;
                    }

                    capModel.specialText = string.Empty;

                    if (capModel.capDetailModel != null)
                    {
                        capModel.capDetailModel.shortNotes = string.Empty;
                    }

                    break;
                case PageFlowComponent.EDUCATION:
                    capModel.educationList = null;
                    break;
                case PageFlowComponent.EXAMINATION:
                    capModel.examinationList = null;
                    break;
                case PageFlowComponent.OWNER:
                    capModel.ownerModel = null;
                    break;
                case PageFlowComponent.PARCEL:
                    capModel.parcelModel = null;
                    break;
                case PageFlowComponent.VALUATION_CALCULATOR:
                    if (capModel.bCalcValuationListField != null && capModel.bCalcValuationListField.Length > 0)
                    {
                        foreach (BCalcValuatnModel4WS field in capModel.bCalcValuationListField)
                        {
                            field.unitValue = 0.0;
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets up the system to perform the page trace.
        /// </summary>
        /// <param name="pageCountInStep">A data stored the total amount of pages in each step.</param>
        /// <returns>The page state tracking.</returns>
        private static int[] BuildBackendStorageForPageStateTracking(Dictionary<int, int> pageCountInStep)
        {
            /*
             * Implementation Notes:
             * We use an array to keep track of page.
             * Here is an example:
             * Supposes we want to keep track of 3 steps. For step 1, it consists of 5 pages; step 2 consists of 3 pages; step 3 consists of 2 pages.
             * The final array after initialization will look like the below sequence:
             *  3 3 8 11 1 1 1 1 1 1 1 1 1 1
             * Look at the elements of the array:
             * The first element represents the total amount of steps;
             * The next 3 elements represents the offset of the page of each step.
             * By saying offset, it means the starting point for indexing each page in a given step.
             * For example, the index in the array for the page 1 of step 1 is S[4]; for the page 1 of step 2 is S[9]; for the page 1 of step 3 is S[12].
             * It can be get from expression S[S[step] + page]. The step and page number can run from the natural, for example 1 to 3 instead of 0 to 2.
             * For page 1 of step 1, it is S[S[1] + 1] = S[3 + 1] = S[4];
             * For page 1 of step 2, it is S[S[2] + 1] = S[8 + 1] = S[9];
             * For page 1 of step 3, it is S[S[3] + 1] = S[11 + 1] = S[12].
             * The next 5 elements represents the tracking information about the 5 pages in the step 1. 0 means the specified page has not been loaded yet; whereas the 1 means it has been loaded.
             * The next 3 elements represents the tracking information about the 3 pages in the step 2.
             * The next 2 elements represents the tracking information about the 2 pages in the step 3.
             * 
             * 0 means the specific page is in hidden state;
             * 1 means the specific page is in display state.
             * 
             * With this kind of data structure, it takes 
             *  O(n) time to initialize the array; n is the total amount of the steps.
             *  O(1) time to mark a page as loaded;
             *  O(1) time to determine whether a page is loaded or not.
             */

            int totalStepsCount = pageCountInStep.Keys.Count;

            List<int> pagesInStep = pageCountInStep.Values.ToList();

            int totalPagesCount = pagesInStep.Sum();

            int[] pageTrackingContainer = new int[1 + totalStepsCount + totalPagesCount];

            // Mark all pages as in display state
            for (int i = 0; i < pageTrackingContainer.Length; i++)
            {
                pageTrackingContainer[i] = 1;
            }

            pagesInStep.Insert(0, totalStepsCount);

            /*
             * We started the array with a column of value of the total amount of steps so that step and page numbers can run from the natural, for example 1 to 3 instead of 0 to 2.
             */
            pageTrackingContainer[0] = totalStepsCount;
            pageTrackingContainer[1] = totalStepsCount;
            for (int i = 2; i <= totalStepsCount; i++)
            {
                pageTrackingContainer[i] = pagesInStep.Take(i).Sum();
            }

            return pageTrackingContainer;
        }

        /// <summary>
        /// Get the page trace
        /// </summary>
        /// <param name="capID">The CapID model.</param>
        /// <returns>Return the page trace.</returns>
        private static int[] GetPageTrace(CapIDModel4WS capID)
        {
            CapTemporaryDataPKModel pkModel = new CapTemporaryDataPKModel();
            pkModel.servProvCode = capID.serviceProviderCode;
            pkModel.capID1 = capID.id1;
            pkModel.capID2 = capID.id2;
            pkModel.capID3 = capID.id3;

            CapTemporaryDataModel searchModel = new CapTemporaryDataModel();
            searchModel.capTemporaryDataPKModel = pkModel;
            searchModel.entityType = PAGEFLOW_TRACE_ENTITY_TYPE;

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            CapTemporaryDataModel dataModel = capBll.GetCapTemporaryData(searchModel);

            string strPageTrace = dataModel != null ? dataModel.data1 : string.Empty;

            if (!string.IsNullOrEmpty(strPageTrace))
            {
                string[] pageTraces = strPageTrace.Split(ACAConstant.COMMA_CHAR);

                List<int> result = new List<int>();

                foreach (string pageTrace in pageTraces)
                {
                    if (ValidationUtil.IsNumber(pageTrace))
                    {
                        result.Add(int.Parse(pageTrace));
                    }
                }

                return result.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Builds up the hidden pages.
        /// </summary>
        /// <param name="results">A data structure contains the number of all hidden pages.</param>
        /// <param name="pageTrackingContainer">The backend storage stored the page tracking information.</param>
        /// <param name="stepNumber">The step to be checked.</param>
        /// <param name="pageCount">The total amount of the pages in the specified step.</param>
        private static void CheckForHiddenPages(Dictionary<int, List<int>> results, int[] pageTrackingContainer, int stepNumber, int pageCount)
        {
            List<int> hiddenPages = new List<int>();

            for (int pageIndex = 1; pageIndex <= pageCount; pageIndex++)
            {
                if (pageTrackingContainer[pageTrackingContainer[stepNumber] + pageIndex] == 0)
                {
                    hiddenPages.Add(pageIndex);
                }
            }

            if (hiddenPages.Count > 0)
            {
                results.Add(stepNumber, hiddenPages);
            }
        }

        /// <summary>
        /// Get the Component Name (Like Applicant_1234) according the ComponentID for the Contact/LP/Attachment related Sections from page flow.
        /// </summary>
        /// <param name="cptModel">The component model.</param>
        /// <returns>The component key.</returns>
        private static string ConstructComponentKey(ComponentModel cptModel)
        {
            PageFlowComponent pageFlowComponent = EnumUtil<PageFlowComponent>.Parse(cptModel.componentID.ToString());
            string componentKey = string.Empty;

            switch (pageFlowComponent)
            {
                case PageFlowComponent.APPLICANT:
                    componentKey = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_APPLICANT_PREFIX, cptModel.componentSeqNbr);
                    break;
                case PageFlowComponent.CONTACT_1:
                    componentKey = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT1_PREFIX, cptModel.componentSeqNbr);
                    break;
                case PageFlowComponent.CONTACT_2:
                    componentKey = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT2_PREFIX, cptModel.componentSeqNbr);
                    break;
                case PageFlowComponent.CONTACT_3:
                    componentKey = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_CONTACT3_PREFIX, cptModel.componentSeqNbr);
                    break;
                case PageFlowComponent.CONTACT_LIST:
                    componentKey = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX, cptModel.componentSeqNbr);
                    break;
                case PageFlowComponent.LICENSED_PROFESSIONAL:
                    componentKey = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_LICENSE_PREFIX, cptModel.componentSeqNbr);
                    break;
                case PageFlowComponent.LICENSED_PROFESSIONAL_LIST:
                    componentKey = string.Format("{0}_{1}", PageFlowConstant.SECTION_NAME_MULTI_LICENSE_PREFIX, cptModel.componentSeqNbr);
                    break;
            }

            return componentKey;
        }

        /// <summary>
        /// build page state tracking by page flow setting
        /// </summary>
        /// <param name="capModel">cap model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="pageflowGroup">page flow group</param>
        /// <returns>The page state tracking.</returns>
        private static int[] BuildPageStateTrackingByPageFlowSetting(CapModel4WS capModel, string moduleName, PageFlowGroupModel pageflowGroup)
        {
            if (pageflowGroup == null)
            {
                return null;
            }

            IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
            PageFlowGroupModel pageFlowGroup = pageflowBll.GetPageFlowGroup(moduleName, pageflowGroup.pageFlowGrpCode, pageflowGroup.serviceProviderCode);
            pageFlowGroup = CapUtil.GetPageFlowWithoutBlankPage(capModel, pageFlowGroup);

            //if sub cap in super agency and sub cap has no page flow, we should use super agency page flow. Resume the sub cap, StandardChoiceUtil.IsSuperAgency() is false.
            if (pageFlowGroup == null)
            {
                pageFlowGroup = GetAssociatedParentPageFlowGroup(capModel);
            }

            Dictionary<int, int> pageCountInStep = GetPageCountOfEachStep(pageFlowGroup);
            int[] newPageStateTracking = BuildBackendStorageForPageStateTracking(pageCountInStep);
            return newPageStateTracking;
        }

        #endregion
    }
}