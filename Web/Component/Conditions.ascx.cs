#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: Conditions.ascx.cs 278770 2014-09-13 03:36:25Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    #region Enumerations

    /// <summary>
    /// condition type.
    /// </summary>
    public enum ConditionType
    {
        /// <summary>
        /// Undefined condition type.
        /// </summary>
        Undefined,

        /// <summary>
        /// type for address.
        /// </summary>
        Address,

        /// <summary>
        /// type for parcel
        /// </summary>
        Parcel,

        /// <summary>
        /// type for owner.
        /// </summary>
        Owner,

        /// <summary>
        /// License type.
        /// </summary>
        License,

        /// <summary>
        /// Contact type
        /// </summary>
        Contact,

        /// <summary>
        /// Owner type In Contact.
        /// </summary>
        OwnerInContact
    }

    /// <summary>
    /// Condition validated result.
    /// </summary>
    public enum ConditionResult
    {
        /// <summary>
        /// No condition
        /// </summary>
        None,

        /// <summary>
        /// Show condition
        /// </summary>
        Show,

        /// <summary>
        /// Show locked condition
        /// </summary>
        Lock,
    }

    #endregion Enumerations

    /// <summary>
    /// the class for conditions.
    /// </summary>
    public partial class Conditions : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _Logger = LogFactory.Instance.GetLogger(typeof(Conditions));

        /// <summary>
        /// GUI text html.
        /// </summary>
        private Hashtable guiTextHT;

        /// <summary>
        /// Condition validated result
        /// </summary>
        private ConditionResult _conditionResult = ConditionResult.None;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets condition.
        /// </summary>
        public string Condition
        {
            get
            {
                if (ViewState["Condition"] == null)
                {
                    return string.Empty;
                }

                return (string)ViewState["Condition"];
            }

            set
            {
                ViewState["Condition"] = value;
            }
        }

        /// <summary>
        /// Gets or sets condition data source
        /// </summary>
        public DataTable ConditionDataSource
        {
            get
            {
                if (ViewState["ConditionDataSource"] == null)
                {
                    return null;
                }

                return (DataTable)ViewState["ConditionDataSource"];
            }

            set
            {
                ViewState["ConditionDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether work location page.
        /// </summary>
        public bool IsWorkLocationPage
        {
            get
            {
                if (ViewState["IsWorkLocationPage"] == null)
                {
                    return false;
                }

                return (bool)ViewState["IsWorkLocationPage"];
            }

            set
            {
                ViewState["IsWorkLocationPage"] = value;
            }
        }

        /// <summary>
        /// Gets the condition validated result.
        /// </summary>
        public ConditionResult ConditionResult
        {
            get
            {
                return _conditionResult;
            }
        }

        /// <summary>
        /// Gets or sets condition type.
        /// </summary>
        public ConditionType Type
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Admin Data Bound
        /// </summary>
        public void AdminDataBound()
        {
            gdvConditionList.DataSource = ConditionsUtil.GetConditionDataSource(null);
            gdvConditionList.DataBind();
            lnkShowHideCondition.InnerHtml = GetTextByKey("per_condition_Label_hide");
            divConditionList.Attributes.Add("style", "display:''");
        }

        /// <summary>
        /// Compose ID
        /// </summary>
        /// <param name="index">Comment index.</param>
        /// <returns>html for Comment.</returns>
        public string ComposeID(object index)
        {
            return "lnkTogglediv" + Condition + "Comment" + index.ToString() + this.ClientID.Replace('$', '_');
        }

        /// <summary>
        /// Collapse or Expand comment when clicking row
        /// </summary>
        /// <param name="index">comment index.</param>
        /// <returns>html for comment.</returns>
        public string GetLinkExpandCommentID(object index)
        {
            return "lnkExpandComment" + Condition + "Comment" + index.ToString() + this.ClientID.Replace('$', '_');
        }

        /// <summary>
        /// Collapse or Expand comment when clicking row
        /// </summary>
        /// <param name="index">comment index.</param>
        /// <returns>Java Script string for comment.</returns>
        public string ExpandComment(object index)
        {
            string commentDivId = GetCommentDiv(index);
            string linkID = GetLinkExpandCommentID(index);
            string imgID = ComposeID(index);
            return "javascript:expandComment('" + commentDivId + "','" + imgID + "','" + linkID + "');";
        }

        /// <summary>
        /// comment div
        /// </summary>
        /// <param name="index">comment index</param>
        /// <returns>html for comment.</returns>
        public string GetCommentDiv(object index)
        {
            return "div" + Condition + "Comment" + index.ToString() + this.ClientID.Replace('$', '_');
        }

        /// <summary>
        /// Clear conditions
        /// </summary>
        public void HideCondition()
        {
            sepForConditionTop.Visible = divContainer.Visible = false;
            divConditionList.Attributes.Add("style", "display:none");
        }

        /// <summary>
        /// check whether to show conditions
        /// </summary>
        /// <param name="noticeConditions">notice conditions</param>
        /// <param name="hightestCondition">highest condition</param>
        /// <param name="conditionType">condition type</param>
        /// <returns>true or false. return false if condition is lock, otherwise, return true.</returns>
        public bool IsShowCondition(NoticeConditionModel[] noticeConditions, NoticeConditionModel hightestCondition, ConditionType conditionType)
        {
            HideCondition();
            _conditionResult = ConditionResult.None;

            if (noticeConditions == null || noticeConditions.Length == 0)
            {
                return false;
            }

            if (hightestCondition == null || string.IsNullOrEmpty(hightestCondition.impactCode))
            {
                return false;
            }

            Condition = conditionType.ToString().ToLower();

            InitialGuiText();
            RegisterJavaScript();

            if (!AppSession.IsAdmin)
            {
                ConditionDataSource = ConditionsUtil.GetConditionDataSource(noticeConditions);
                gdvConditionList.DataSource = ConditionDataSource;
            }
            else
            {
                gdvConditionList.DataSource = ConditionsUtil.GetConditionDataSource(null);
            }

            gdvConditionList.DataBind();

            ComposeCondition(hightestCondition, noticeConditions);

            if (hightestCondition.impactCode.Equals(ACAConstant.LOCK_CONDITION, StringComparison.InvariantCultureIgnoreCase))
            {
                _conditionResult = ConditionResult.Lock;
                sepForConditionTop.Visible = divContainer.Visible = true;
            }
            else if (hightestCondition.impactCode.Equals(ACAConstant.HOLD_CONDITION, StringComparison.InvariantCultureIgnoreCase)
                     || hightestCondition.impactCode.Equals(ACAConstant.NOTICE_CONDITION, StringComparison.InvariantCultureIgnoreCase))
            {
                _conditionResult = ConditionResult.Show;
                sepForConditionTop.Visible = divContainer.Visible = true;
            }

            return _conditionResult == ConditionResult.Lock;
        }

        /// <summary>
        /// <c>OnInit</c> Event.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string type = Parent.NamingContainer.TemplateControl.AppRelativeVirtualPath;
            if (type.IndexOf("AddressEdit") > -1 || Type == ConditionType.Address)
            {
                Condition = "address";
                gdvConditionList.GridViewNumber = GviewID.AddressConditionList;
                lblConditionTitle.LabelKey = "aca_permit_address_conditions_title";
            }
            else if (type.IndexOf("ParcelEdit") > -1 || Type == ConditionType.Parcel)
            {
                Condition = "parcel";
                gdvConditionList.GridViewNumber = GviewID.ParcelConditionList;
                lblConditionTitle.LabelKey = "aca_permit_parcel_conditions_title";
            }
            else if (type.IndexOf("OwnerEdit") > -1)
            {
                Condition = "owner";
                gdvConditionList.GridViewNumber = GviewID.OwnerConditionList;
                lblConditionTitle.LabelKey = "aca_permit_owner_conditions_title";
            }
            else if (type.IndexOf("ContactEdit") > -1 || type.IndexOf("MultiContactsEdit") > -1
                 || type.IndexOf("ContactInfo") > -1 || Parent is ContactSearchList)
            {
                gdvConditionList.GridViewNumber = GviewID.ContactConditionList;
                lblConditionTitle.LabelKey = "aca_permit_contact_conditions_title";
            }
            else if (type.IndexOf("LicenseEdit") > -1 || type.IndexOf("MultiLicensesEdit") > -1 || Request.Path.IndexOf("LicenseInput", StringComparison.InvariantCultureIgnoreCase) > -1 || Request.Path.IndexOf("LicenseeDetail") > -1 || Request.Path.IndexOf("FoodFacilityInspectionDetail") > -1)
            {
                Condition = "license";
                gdvConditionList.GridViewNumber = GviewID.LicenseConditionList;
                lblConditionTitle.LabelKey = "aca_permit_licenseprofessional_conditions_title";
            }
            else
            {
                if (_Logger.IsDebugEnabled)
                {
                    _Logger.Debug("Cann't find the type " + type + " in Conditions.");
                }
            }

            if (!IsPostBack)
            {
                gdvConditionList.PagerSettings.PreviousPageText = GetTextByKey("ACA_NextPrevNumbericPagerTemplate_PrevText");
                gdvConditionList.PagerSettings.NextPageText = GetTextByKey("ACA_NextPrevNumbericPagerTemplate_NextText");
                string script = "if (typeof(End" + Condition + "Request)!='undefined'){";
                script += "End" + Condition + "Request('" + lnkShowHideCondition.ClientID + "','" + divConditionList.ClientID + "');}";
                ScriptManager.RegisterStartupScript(Page, GetType(), "EndConditionRequest", script, true);
            }

            GridViewBuildHelper.SetSimpleViewElements(gdvConditionList, ModuleName, AppSession.IsAdmin);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                AdminDataBound();
            }
        }

        /// <summary>
        /// GridView ConditionList RowCommand.
        /// </summary>
        /// <param name="sender">GridViewCommandEventArgs e</param>
        /// <param name="e">EventArgs e</param>
        protected void ConditionList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            gdvConditionList.DataSource = ConditionDataSource;
            gdvConditionList.DataBind();
            lnkShowHideCondition.InnerHtml = GetTextByKey("per_condition_Label_hide");
            divConditionList.Attributes.Add("style", "display:''");
        }

        /// <summary>
        /// GridView ConditionList RowDataBound.
        /// </summary>
        /// <param name="sender">GridViewCommandEventArgs e</param>
        /// <param name="e">EventArgs e</param>
        protected void ConditionList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;

            // Make sure we aren't in header/footer rows
            if (row.DataItem == null)
            {
                return;
            }

            DataRowView rowView = (DataRowView)e.Row.DataItem;

            if (rowView["src"].ToString() == string.Empty)
            {
                Image imgSevrity = (Image)row.FindControl("imgSevrity");
                imgSevrity.Visible = false;
            }

            System.Web.UI.HtmlControls.HtmlGenericControl commentPanel = (System.Web.UI.HtmlControls.HtmlGenericControl)row.FindControl("commentPanel");
            commentPanel.Attributes.Add("class", e.Row.DataItemIndex % 2 == 1 ? "ACA_TabRow_Comment_Even font11px" : "ACA_TabRow_Comment_Odd font11px");
        }

        /// <summary>
        /// Compose Condition.
        /// </summary>
        /// <param name="hightestCondition">high test condition.</param>
        /// <param name="noticeConditions">notice conditions.</param>
        private void ComposeCondition(NoticeConditionModel hightestCondition, NoticeConditionModel[] noticeConditions)
        {
            string conditionName = hightestCondition.impactCode.ToUpperInvariant();

            if (guiTextHT.ContainsKey(conditionName + "div"))
            {
                divContainer.Attributes.Add("class", guiTextHT[conditionName + "div"].ToString());
            }

            if (guiTextHT.ContainsKey(conditionName + "icon"))
            {
                divIcon.Attributes.Add("class", guiTextHT[conditionName + "icon"].ToString());
            }

            if (guiTextHT.ContainsKey(conditionName + "title"))
            {
                imgIcon.Attributes.Add("title", guiTextHT[conditionName + "title"].ToString());
                imgIcon.Attributes.Add("alt", guiTextHT[conditionName + "title"].ToString());
            }

            if (string.IsNullOrEmpty(imgIcon.Alt))
            {
                imgIcon.Alt = " ";
            }

            if (guiTextHT.ContainsKey(conditionName + "img"))
            {
                imgIcon.Attributes.Add("src", guiTextHT[conditionName + "img"].ToString());
            }

            //should be NoticConditionModel4WS.auditDate.
            string dateDescription = string.Empty;

            if (hightestCondition.auditDate != null)
            {
                dateDescription = "<span dir=\"ltr\">" + I18nDateTimeUtil.FormatToDateStringForUI(hightestCondition.auditDate) + "</span>.";
            }

            string desc = string.Empty;

            if (guiTextHT.ContainsKey(Condition + conditionName))
            {
                desc = guiTextHT[Condition + conditionName].ToString();
            }

            divMsg.InnerHtml = string.Format("{0}{1}", desc, dateDescription);

            if (!string.IsNullOrEmpty(divMsg.InnerHtml))
            {
                imgIcon.Attributes["class"] = "ACA_Show";
            }

            tdCondition.InnerHtml = "<div style=\"white-space:nowrap;\">" + ConditionsUtil.AddSpaceWithFormat(GetTextByKey("per_condition_description")) + "</div>";
            tdConditionName.InnerHtml = ScriptFilter.EncodeHtml(I18nStringUtil.GetString(hightestCondition.resConditionDescription, hightestCondition.conditionDescription));
            tdSeverity.InnerHtml = ConditionsUtil.AddSpaceWithFormat(GetTextByKey("per_severity_descirption"));
            tdStatus.InnerHtml = LabelUtil.GetGuiTextForCapConditionSeverity(hightestCondition.impactCode);
            tdSummary.InnerHtml = ConditionsUtil.ComposeConditionSummary(noticeConditions);
            lnkShowHideCondition.InnerHtml = GetTextByKey("per_condition_Label_show");
        }

        /// <summary>
        /// Initial GUI text.
        /// </summary>
        private void InitialGuiText()
        {
            guiTextHT = new Hashtable();
            guiTextHT.Add(ACAConstant.LOCK_CONDITION + "icon", "ACA_Locked_Icon");
            guiTextHT.Add(ACAConstant.HOLD_CONDITION + "icon", "ACA_Hold_Icon");
            guiTextHT.Add(ACAConstant.NOTICE_CONDITION + "icon", "ACA_Note_Icon");

            guiTextHT.Add(ACAConstant.LOCK_CONDITION + "img", ImageUtil.GetImageURL("locked_24.gif"));
            guiTextHT.Add(ACAConstant.HOLD_CONDITION + "img", ImageUtil.GetImageURL("hold_24.gif"));
            guiTextHT.Add(ACAConstant.NOTICE_CONDITION + "img", ImageUtil.GetImageURL("note_24.gif"));

            guiTextHT.Add(ACAConstant.LOCK_CONDITION + "title", GetTextByKey("aca_condition_notice_locked"));
            guiTextHT.Add(ACAConstant.HOLD_CONDITION + "title", GetTextByKey("aca_condition_notice_hold"));
            guiTextHT.Add(ACAConstant.NOTICE_CONDITION + "title", GetTextByKey("aca_condition_notice_note"));

            guiTextHT.Add(ACAConstant.LOCK_CONDITION + "div", "ACA_Message_Locked ACA_Message_Locked_FontSize");
            guiTextHT.Add(ACAConstant.HOLD_CONDITION + "div", "ACA_Message_Hold ACA_Message_Hold_FontSize");
            guiTextHT.Add(ACAConstant.NOTICE_CONDITION + "div", "ACA_Message_Note ACA_Message_Note_FontSize");

            guiTextHT.Add("address" + "event", "showMoreaddressCondition(this);");
            guiTextHT.Add("owner" + "event", "showMoreownerCondition(this);");
            guiTextHT.Add("parcel" + "event", "showMoreparcelCondition(this);");
            guiTextHT.Add("license" + "event", "showMorelicenseCondition(this);");
            guiTextHT.Add("contact" + "event", "showMorecontactCondition(this);");

            guiTextHT.Add("address" + ACAConstant.LOCK_CONDITION, GetTextByKey("per_addressCondition_locked"));
            guiTextHT.Add("address" + ACAConstant.HOLD_CONDITION, GetTextByKey("per_addressCondition_hold"));
            guiTextHT.Add("address" + ACAConstant.NOTICE_CONDITION, GetTextByKey("per_addressCondition_notice"));
            guiTextHT.Add("owner" + ACAConstant.LOCK_CONDITION, GetTextByKey("per_ownerCondition_locked"));
            guiTextHT.Add("owner" + ACAConstant.HOLD_CONDITION, GetTextByKey("per_ownerCondition_hold"));
            guiTextHT.Add("owner" + ACAConstant.NOTICE_CONDITION, GetTextByKey("per_ownerCondition_notice"));
            guiTextHT.Add("parcel" + ACAConstant.LOCK_CONDITION, GetTextByKey("per_parcel_condition_locked"));
            guiTextHT.Add("parcel" + ACAConstant.HOLD_CONDITION, GetTextByKey("per_parcel_condition_hold"));
            guiTextHT.Add("parcel" + ACAConstant.NOTICE_CONDITION, GetTextByKey("per_parcel_condition_notice"));
            guiTextHT.Add("license" + ACAConstant.LOCK_CONDITION, GetTextByKey("per_LicenseCondition_locked"));
            guiTextHT.Add("license" + ACAConstant.HOLD_CONDITION, GetTextByKey("per_LicenseCondition_hold"));
            guiTextHT.Add("license" + ACAConstant.NOTICE_CONDITION, GetTextByKey("per_LicenseCondition_notice"));
            guiTextHT.Add("contact" + ACAConstant.LOCK_CONDITION, GetTextByKey("per_ContactCondition_locked"));
            guiTextHT.Add("contact" + ACAConstant.HOLD_CONDITION, GetTextByKey("per_ContactCondition_hold"));
            guiTextHT.Add("contact" + ACAConstant.NOTICE_CONDITION, GetTextByKey("per_ContactCondition_notice"));
        }

        /// <summary>
        /// register java script.
        /// </summary>
        private void RegisterJavaScript()
        {
            string func = string.Format("if (typeof(SetNotAsk)!='undefined') SetNotAsk();showMore{0}Condition('{1}','{2}')", Condition.ToLower(), divConditionList.ClientID, lnkShowHideCondition.ClientID);
            lnkShowHideCondition.Attributes.Add("onclick", func);
        }

        #endregion Methods
    }
}       
