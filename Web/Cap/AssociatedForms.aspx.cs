#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AssociatedForms.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:A new form for the Hazardous Material feature.
 *
 *  Notes:
 * $Id: AssociatedForms.aspx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// Associated Forms page.
    /// </summary>
    public partial class AssociatedForms : BasePage
    {
        #region Fields

        /// <summary>
        /// Alias of the Record type
        /// </summary>
        protected const string PATTERN_RECORDTYPE_ALIAS = "$$RecordTypeAlias$$";

        /// <summary>
        /// Application name of the Record
        /// </summary>
        protected const string PATTERN_APPLICATION_NAME = "$$ApplicationName$$";

        /// <summary>
        /// General description of the Record
        /// </summary>
        protected const string PATTERN_GENERAL_DESCRIPTION = "$$GeneralDescription$$";

        #endregion Fields

        /// <summary>
        /// Gets a value indicating whether it is super agency's associated form or not.
        /// </summary>
        private bool IsSuperAgencyAssoForm
        {
            get
            {
                if (ViewState["IsSuperAgencyAssoForm"] != null)
                {
                    return (bool)ViewState["IsSuperAgencyAssoForm"];
                }

                bool isSuperAgencyAssoForm = CapUtil.GetAssoFormType(true, ParentCapId) == ACAConstant.AssoFormType.SuperAgency;

                ViewState["IsSuperAgencyAssoForm"] = isSuperAgencyAssoForm;

                return isSuperAgencyAssoForm;
            }
        }

        /// <summary>
        /// Gets parent cap id
        /// </summary>
        private CapIDModel4WS ParentCapId
        {
            get
            {
                return AppSession.GetParentCapIDModelFromSession(ACAConstant.CAP_RELATIONSHIP_ASSOFORM);
            }
        }

        #region Protected Event handler

        /// <summary>
        /// Handle the page load event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                //Disable the Continue button by default.
                DisableContinueButton(true);
            }

            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    if (ParentCapId == null)
                    {
                        return;
                    }
                    
                    ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

                    //Set the parent CapModel to current Session. Currently, HazMat feature does not supports super agency.
                    CapWithConditionModel4WS parentCondCap = capBll.GetCapViewBySingle(ParentCapId, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, false);                   
                    ContactUtil.InitializeContactsGroup4CapModel(parentCondCap.capModel);
                    PageFlowGroupModel parentPageflow = AppSession.GetParentPageflowGroupFromSession();

                    CapModel4WS oldParentCap = AppSession.GetAssociatedParentCapFromSession(ModuleName);

                    if (!CapUtil.IsAssoFormChild(ModuleName))
                    {
                        CapModel4WS parentCap = AppSession.GetCapModelFromSession(ModuleName);
                        oldParentCap = parentCap;

                        AppSession.SetAssociatedParentCapToSession(ModuleName, oldParentCap);
                    }

                    if (!StandardChoiceUtil.IsSuperAgency() && PageFlowUtil.IsPageflowChanged(oldParentCap, ModuleName, parentPageflow))
                    {
                        breadCrumbToolBar.Enabled = false;

                        // The PageFlowUtil.IsPageFlowTraceUpdated is used in the function CapUtil.BuildRedirectUrl(), so the value should be set before the related function used.
                        PageFlowUtil.IsPageFlowTraceUpdated = true;

                        string url = CapUtil.BuildRedirectUrl(null, string.Empty, parentPageflow, string.Empty);
                        
                        string message = string.Format(GetTextByKey("aca_capconfirm_msg_pageflowchange_notice"), url);
                        MessageUtil.ShowMessage(Page, MessageType.Notice, message);
                    }

                    CapUtil.FillCapModelTemplateValue(parentCondCap.capModel);
                    AppSession.SetCapModelToSession(parentCondCap.capModel.moduleName, parentCondCap.capModel);
                    AppSession.SetPageflowGroupToSession(parentPageflow);

                    BreadCrumpUtil.RebuildBreadCrumb(parentPageflow, ModuleName, true);
                    breadCrumbToolBar.PageFlow = BreadCrumpUtil.GetPageFlowConfig(ModuleName, false);

                    //Bind all child Caps.
                    BindChildCaps(ParentCapId);
                }
                else
                {
                    divPatternConfig.Visible = true;
                }
            }
            else
            {
                if (Request["__EVENTTARGET"] == "SaveAndResume")
                {
                    string strIsSuperAgencyAssoForm = IsSuperAgencyAssoForm ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

                    CapUtil.SaveResumeRedirect(Response, ModuleName, ParentCapId, ACAConstant.COMMON_N, strIsSuperAgencyAssoForm);
                }
            }
        }

        /// <summary>
        /// Handle the cap list ItemDataBound event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        protected void ChildCapList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl lblChildCapTitle = e.Item.FindControl("lblChildCapTitle") as HtmlGenericControl;
                AccelaLinkButton lnkChildCapAction = e.Item.FindControl("lnkChildCapAction") as AccelaLinkButton;
                AccelaLinkButton lnkRemoveAction = e.Item.FindControl("lnkRemoveAction") as AccelaLinkButton;

                CapModel4WS childCap = (CapModel4WS)e.Item.DataItem;
                lblChildCapTitle.InnerHtml = FormatCapTitle(childCap);

                lnkChildCapAction.CommandArgument = string.Format(
                    "{1}{0}{2}{0}{3}{0}{4}",
                    ACAConstant.SPLIT_CHAR,
                    childCap.capID.id1,
                    childCap.capID.id2,
                    childCap.capID.id3,
                    childCap.capID.serviceProviderCode);

                lnkRemoveAction.CommandArgument = lnkChildCapAction.CommandArgument;
                lnkRemoveAction.CommandName = "Remove";
                lnkRemoveAction.Text = LabelUtil.GetTextByKey("aca_associatedforms_label_childcapremove", ModuleName);
                lnkRemoveAction.Attributes["onclick"] = "return confirmMsg('" + GetTextByKey("aca_associatedforms_msg_deleteconfirm", ModuleName).Replace("'", "\\'") + "');";

                // In super agency, select multiply services, there only exists Open/View buttons. Because the Open/Resume cannot identify in this case.
                string capStatus = childCap.capClass;

                if (IsSuperAgencyAssoForm && capStatus == ACAConstant.INCOMPLETE_CAP)
                {
                    capStatus = ACAConstant.INCOMPLETE_TEMP_CAP;
                }

                switch (capStatus)
                {
                    case ACAConstant.INCOMPLETE_TEMP_CAP:
                        lnkChildCapAction.Text = LabelUtil.GetTextByKey("aca_associated_forms_childcap_action_open", ModuleName);
                        lnkChildCapAction.CommandName = "Open";
                        break;
                    case ACAConstant.INCOMPLETE_CAP:
                        lnkChildCapAction.Text = LabelUtil.GetTextByKey("aca_associated_forms_childcap_action_resume", ModuleName);
                        lnkChildCapAction.CommandName = "Resume";
                        break;
                    case ACAConstant.INCOMPLETE_EST:
                        lnkChildCapAction.Text = LabelUtil.GetTextByKey("aca_associated_forms_childcap_action_view", ModuleName);
                        lnkChildCapAction.CommandName = "View";
                        break;
                }
            }
        }

        /// <summary>
        /// Handle the cap list ItemCommand event
        /// </summary>
        /// <param name="source">Event source</param>
        /// <param name="e">Event args</param>
        protected void ChildCapList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            IPageflowBll pageflowBll = (IPageflowBll)ObjectFactory.GetObject(typeof(IPageflowBll));

            string url = string.Empty;

            string[] args = e.CommandArgument.ToString().Split(ACAConstant.SPLIT_CHAR);
            CapIDModel4WS childCapID = new CapIDModel4WS();
            childCapID.id1 = args[0];
            childCapID.id2 = args[1];
            childCapID.id3 = args[2];
            childCapID.serviceProviderCode = args[3];

            CapWithConditionModel4WS childCondCap = capBll.GetCapViewBySingle(childCapID, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, false);
            ContactUtil.InitializeContactsGroup4CapModel(childCondCap.capModel);
            CapUtil.FillCapModelTemplateValue(childCondCap.capModel);
            CapModel4WS capModel = childCondCap.capModel;
            PageFlowGroupModel childPageflowGroup = pageflowBll.GetPageflowGroupByCapType(capModel.capType);

            switch (e.CommandName)
            {
                case "Open":
                    url = string.Format("CapEdit.aspx?permitType=resume&Module={0}&stepNumber=2&pageNumber=1&isFromShoppingCart={1}", capModel.moduleName, Request.QueryString[ACAConstant.FROMSHOPPINGCART]);
                    url += "&" + ACAConstant.IS_CLONE_RECORD + "=" + ACAConstant.COMMON_TRUE;

                    //Reset the Page Trace of the current section for the child record. 
                    PageFlowUtil.ResetPageTrace(capModel);

                    break;
                case "Resume":
                    url = string.Format("CapEdit.aspx?permitType=resume&Module={0}&stepNumber=2&pageNumber=1&isFromShoppingCart={1}", capModel.moduleName, Request.QueryString[ACAConstant.FROMSHOPPINGCART]);

                    //Reset the Page Trace of the current section for the child record. 
                    PageFlowUtil.ResetPageTrace(capModel);

                    break;
                case "View":
                    int stepNumber = childPageflowGroup.stepList.Length + 2;
                    url = string.Format("CapConfirm.aspx?Module={0}&TabName={0}&stepNumber={1}&isFromShoppingCart={2}", capModel.moduleName, stepNumber, Request.QueryString[ACAConstant.FROMSHOPPINGCART]);
                    break;
                case "Remove":
                    capBll.RemoveChildPartialCap(ParentCapId, childCapID, AppSession.User.PublicUserId);
                    BindChildCaps(ParentCapId);
                    break;
            }

            if (!e.CommandName.Equals("Remove"))
            {
                if (childPageflowGroup == null || childPageflowGroup.stepList == null || childPageflowGroup.stepList.Length < 1)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_applyPermit_error_noRelatedPageflowGroup"));
                    return;
                }

                //Set the selected CapModel to current Session.
                AppSession.SetCapModelToSession(capModel.moduleName, capModel);
                AppSession.SetPageflowGroupToSession(childPageflowGroup);
                BreadCrumpUtil.RebuildBreadCrumb(childPageflowGroup, ModuleName, false);

                //Clear the Parcel PK Model.
                Session[SessionConstant.APO_SESSION_PARCELMODEL] = null;

                url += "&" + ACAConstant.IS_SUBAGENCY_CAP + "=" + ACAConstant.COMMON_Y;
                url += "&" + UrlConstant.AgencyCode + "=" + childCapID.serviceProviderCode;

                // Use IsSuperAgencyAssoForm to indicates current request is come from Super Agency Associated Forms.
                if (IsSuperAgencyAssoForm)
                {
                    url += "&" + UrlConstant.IS_SUPERAGENCY_ASSOFORM + "=" + ACAConstant.COMMON_Y;
                }

                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Handle the continue button Click event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            //if enable partial submission, delete the child cap which state is 'INCOMPLETE_TEMP_CAP'
            bool enablePartialSubmission = CapUtil.EnablePartialSubmission(ParentCapId);

            if (enablePartialSubmission)
            {
                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                string capRelationship = IsSuperAgencyAssoForm ? ACAConstant.CAP_RELATIONSHIP_RELATED : ACAConstant.CAP_RELATIONSHIP_ASSOFORM;
                CapModel4WS[] childCaps = capBll.GetChildCapDetailsByMasterID(ParentCapId, capRelationship, null);

                if (childCaps != null && childCaps.Count() > 0)
                {
                    foreach (CapModel4WS childCap in childCaps)
                    {
                        if (childCap == null)
                        {
                            continue;
                        }

                        if ((ACAConstant.CAP_RELATIONSHIP_ASSOFORM.Equals(capRelationship, StringComparison.OrdinalIgnoreCase) && ACAConstant.INCOMPLETE_TEMP_CAP.Equals(childCap.capClass, StringComparison.OrdinalIgnoreCase))
                            || (ACAConstant.CAP_RELATIONSHIP_RELATED.Equals(capRelationship, StringComparison.OrdinalIgnoreCase) && ACAConstant.INCOMPLETE_CAP.Equals(childCap.capClass, StringComparison.OrdinalIgnoreCase)))
                        {
                            capBll.RemoveChildPartialCap(ParentCapId, childCap.capID, AppSession.User.PublicUserId);
                        }
                    }
                }
            }

            bool isFromShoppingCart = ACAConstant.COMMON_Y.Equals(Request.QueryString[ACAConstant.FROMSHOPPINGCART]);
            CapUtil.ToPaymentApplication(AppSession.GetCapModelFromSession(ModuleName), ModuleName, isFromShoppingCart);
        }

        #endregion

        #region Private mothods

        /// <summary>
        /// Bind the child caps to the cap list
        /// </summary>
        /// <param name="parentCapID">Parent cap ID</param>
        private void BindChildCaps(CapIDModel4WS parentCapID)
        {
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            string capRelationship = IsSuperAgencyAssoForm ? ACAConstant.CAP_RELATIONSHIP_RELATED : ACAConstant.CAP_RELATIONSHIP_ASSOFORM;

            CapModel4WS[] childCaps = capBll.GetChildCapDetailsByMasterID(parentCapID, capRelationship, null);

            int completedCount = 0;

            if (childCaps != null && childCaps.Length > 0)
            {
                foreach (CapModel4WS childCap in childCaps)
                {
                    if (childCap.capClass.Equals(ACAConstant.INCOMPLETE_EST, StringComparison.InvariantCultureIgnoreCase))
                    {
                        completedCount++;
                    }
                }
            }
            else if (IsSuperAgencyAssoForm)
            {
                //For super agency Associated Form, if the child cap list is empty, the "Save and Resume" logic is meaningless.
                ClientScript.RegisterStartupScript(this.GetType(), "DisableSaveAndResumeButton", "DisableSaveAndResumeButton();", true);
            }

            if (completedCount == 0)
            {
                DisableContinueButton(true);
            }
            else
            {
                //Indicate whether enable partial submission
                bool enablePartialSubmission = CapUtil.EnablePartialSubmission(parentCapID);
                bool enableContinueButton = (enablePartialSubmission && completedCount > 0) || (childCaps != null && completedCount == childCaps.Length);
                this.DisableContinueButton(!enableContinueButton);

                if (completedCount == childCaps.Length)
                {
                    btnContinue.Attributes.Remove("onclick");
                }
                else
                {
                    btnContinue.Attributes["onclick"] = "return confirmMsg('" + GetTextByKey("aca_associatedforms_msg_partialsubmissionconfirm", ModuleName).Replace("'", "\\'") + "');";
                }
            }

            childCapList.DataSource = ReSortChildCaps(childCaps);
            childCapList.DataBind();
        }

        /// <summary>
        /// Format the title column of the cap list.
        /// </summary>
        /// <param name="capModel">Cap model object</param>
        /// <returns>format cap title</returns>
        private string FormatCapTitle(CapModel4WS capModel)
        {
            string capTitlePattern = LabelUtil.GetTextByKey("aca_associated_forms_childcap_pattern", ModuleName);
            capTitlePattern = capTitlePattern.Replace(PATTERN_RECORDTYPE_ALIAS, CAPHelper.GetAliasOrCapTypeLabel(capModel.capType));
            capTitlePattern = capTitlePattern.Replace(PATTERN_APPLICATION_NAME, capModel.specialText);
            capTitlePattern = capTitlePattern.Replace(PATTERN_GENERAL_DESCRIPTION, capModel.capDetailModel != null ? capModel.capDetailModel.shortNotes : string.Empty);

            return capTitlePattern;
        }

        /// <summary>
        /// Re sort the cap list by the format cap title.
        /// </summary>
        /// <param name="childCaps">child caps model</param>
        /// <returns>child cap list</returns>
        private List<CapModel4WS> ReSortChildCaps(CapModel4WS[] childCaps)
        {
            if (childCaps == null)
            {
                return null;
            }

            List<CapModel4WS> childCapList = new List<CapModel4WS>(childCaps);

            childCapList.Sort((firstPair, nextPair) => { return FormatCapTitle(firstPair).CompareTo(FormatCapTitle(nextPair)); });

            return childCapList;
        }

        /// <summary>
        /// Disable the Continue button.
        /// </summary>
        /// <param name="disable">Boolean value. true - disable the button; false - enable the button.</param>
        private void DisableContinueButton(bool disable)
        {
            btnContinue.Enabled = !disable;

            if (disable)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "DisableContinueButton", "DisableContinueButton(true);", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "EnableContinueButton", "DisableContinueButton(false);", true);
            }
        }

        #endregion
    }
}
