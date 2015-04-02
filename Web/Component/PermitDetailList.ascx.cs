#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PermitDetailList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PermitDetailList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.Util;
using Accela.ACA.Web.VO;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// display applicant address license project description and owner information in detail page.
    /// </summary>
    public partial class PermitDetailList : PermitDetailBaseUserControl
    {
        #region Fields

        /// <summary>
        /// Display contact section or not.
        /// </summary>
        private bool _isDispalyContact = false;

        /// <summary>
        /// Display parcel section or not.
        /// </summary>
        private bool _isDisplayParcel = false;

        /// <summary>
        /// Display AdditionalInfo section or not.
        /// </summary>
        private bool _isDisplayAdditionalInfo = false;

        /// <summary>
        /// Display ASI section or not.
        /// </summary>
        private bool _isDisplayASI = false;

        /// <summary>
        /// Display ASIT section or not.
        /// </summary>
        private bool _isDisplayASIT = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets hidden sections.
        /// </summary>
        public IList<string> HiddenSections
        {
            get
            {
                if (ViewState["HiddenSections"] == null)
                {
                    ViewState["HiddenSections"] = new List<string>();
                }

                return (IList<string>)ViewState["HiddenSections"];
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Arabic language is true, others is false or not.
        /// </summary>
        protected bool IsRightToLeft
        {
            get;
            set;
        }

        /// <summary>
        /// Gets ReArray sequence of more detail section
        /// </summary>
        private Dictionary<string, object> MoreDetailSections
        {
            get
            {
                if (ViewState["MoreDetailSections"] == null)
                {
                    Dictionary<string, object> _moreDetailSections = new Dictionary<string, object>();
                    _moreDetailSections.Add(GViewConstant.SECTION_ADDITIONAL_INFO, CapDetailSectionType.ADDITIONAL_INFORMATION.ToString());
                    _moreDetailSections.Add(GViewConstant.SECTION_ASI, CapDetailSectionType.APPLICATION_INFORMATION.ToString());
                    _moreDetailSections.Add(GViewConstant.SECTION_ASIT, CapDetailSectionType.APPLICATION_INFORMATION_TABLE.ToString());
                    _moreDetailSections.Add(GViewConstant.SECTION_PARCEL, CapDetailSectionType.PARCEL_INFORMATION.ToString());

                    _moreDetailSections.Add(GViewConstant.SECTION_CONTACT1, CapDetailSectionType.RELATED_CONTACTS.ToString());
                    _moreDetailSections.Add(GViewConstant.SECTION_CONTACT2, CapDetailSectionType.RELATED_CONTACTS.ToString());
                    _moreDetailSections.Add(GViewConstant.SECTION_CONTACT3, CapDetailSectionType.RELATED_CONTACTS.ToString());

                    ViewState["MoreDetailSections"] = _moreDetailSections;
                }

                return (Dictionary<string, object>)ViewState["MoreDetailSections"];
            }
        }

        /// <summary>
        /// Gets ReArray sequence of sections
        /// </summary>
        private Dictionary<string, object> ReOrderSection
        {
            get
            {
                if (ViewState["ReOrderSection"] == null)
                {
                    Dictionary<string, object> _reOrderSection = new Dictionary<string, object>();
                    _reOrderSection.Add(GViewConstant.SECTION_APPLICANT, CapDetailSectionType.APPLICANT.ToString());
                    _reOrderSection.Add(GViewConstant.SECTION_LICENSE, CapDetailSectionType.LICENSED_PROFESSIONAL.ToString());
                    _reOrderSection.Add(GviewID.DetailInformation, CapDetailSectionType.PROJECT_DESCRIPTION.ToString());
                    _reOrderSection.Add(GViewConstant.SECTION_OWNER, CapDetailSectionType.OWNER.ToString());
                    ViewState["ReOrderSection"] = _reOrderSection;
                }

                return (Dictionary<string, object>)ViewState["ReOrderSection"];
            }
        }

        /// <summary>
        /// Gets a value indicating whether display Hijri Calendar 
        /// </summary>
        private bool IsDisplayHijriCalendar
        {
            get
            {
                return StandardChoiceUtil.IsEnableDisplayISLAMICCalendar();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Refreshes the control.
        /// </summary>
        public void RefreshControl()
        {
            DisplayCapDetail();
            ControlSectionOfMoreDetail();

            updatePanel.Update();
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DisplayCapDetail();
                ControlSectionOfMoreDetail();
                IsRightToLeft = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;

                HideControlsInAdmin();
            }
        }

        //===============================================================================

        /// <summary>
        /// add displayed section into panel.
        /// </summary>
        /// <param name="input">output string format</param>
        /// <param name="labelkey">display name</param>
        /// <param name="htc">callback panel</param>
        /// <param name="isLicenseSection">Judge the license section</param>
        private void AddCtrlToPanel(string input, string labelkey, ref Panel htc, bool isLicenseSection)
        {
            Literal ltStart = new Literal();
            ltStart.Text = @"<h1 style='font-size:1.4em;'>";
            Literal ltEnd = new Literal();
            ltEnd.Text = @"</h1>";
            bool isDisplayCurrentSection = false;
            Panel tc = new Panel();
            AccelaLabel alb = CreateLabel(labelkey);

            if (GViewConstant.SECTION_APPLICANT.Equals(input))
            {
                UserRoleType type = GetApplicantRestrictDisplay();
                alb.LabelType = LabelType.ApplicantText;
                alb.RestrictDisplay = type;
            }

            tc.Controls.Add(ltStart);
            tc.Controls.Add(alb);
            tc.Controls.Add(ltEnd);

            if (!AppSession.IsAdmin)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    alb = CreateLabel(string.Empty);
                    alb.IsNeedEncode = false;
                    alb.Text = input;
                    tc.Controls.Add(alb);

                    isDisplayCurrentSection = true;
                }
            }
            else
            {
                isDisplayCurrentSection = true;
            }

            if (isDisplayCurrentSection)
            {
                htc = tc;
            }
        }

        /// <summary>
        /// generate additional info html tag base label key and field value
        /// </summary>
        /// <param name="lablkey">label key for some field</param>
        /// <param name="fieldValue">content for some one field</param>
        /// <param name="count">amount  of the enable field</param>
        /// <returns>final html tag for some one field</returns>
        private string BuildingOneDivContent(string lablkey, string fieldValue, ref int count)
        {
            string divRow = "<div class=\"ACA_TabRow\">";
            string divLeft = "<div class=\"MoreDetail_ItemCol MoreDetail_ItemCol1\">";
            string divRight = "<div class=\"MoreDetail_ItemCol MoreDetail_ItemCol2\">";
            string endDiv = "</div>";

            string lblrightSpan = "<span>";
            string lblleftSpan = "<span>";

            string rightSpan = "<span class=\"ACA_SmLabel ACA_SmLabel_FontSize\">";
            string leftSpan = "<span class=\"ACA_SmLabel ACA_SmLabel_FontSize\">";
            string endSpan = "</span>";
            string infoContent = string.Empty;

            if (!string.IsNullOrEmpty(fieldValue))
            {
                infoContent += count % 2 == 0 ? divRow + divLeft + lblleftSpan : divRight + lblrightSpan;
                string tempText = LabelUtil.GetTextByKey(lablkey, ModuleName);

                if (lablkey.Equals("capDescriptionEdit_label_jobValue", StringComparison.InvariantCultureIgnoreCase))
                {
                    tempText = string.Format(tempText, I18nNumberUtil.CurrencySymbol);
                }

                if (lablkey.Equals("capDescriptionEdit_label_publicOwner", StringComparison.InvariantCultureIgnoreCase))
                {
                    tempText = tempText + ":";
                }

                infoContent += "<h2>" + tempText + "</h2>";
                infoContent += endSpan;
                infoContent += count % 2 == 0 ? leftSpan : rightSpan;
                infoContent += ScriptFilter.FilterScript(fieldValue);
                infoContent += endSpan;
                infoContent += endDiv;
                count++;
                infoContent += count % 2 == 0 ? endDiv : string.Empty;
            }

            return infoContent;
        }

        /// <summary>
        /// control more detail section in detail page.
        /// </summary>
        private void ControlSectionOfMoreDetail()
        {
            tbRCList.Visible = AppSession.IsAdmin ? true : this._isDispalyContact;
            tbADIList.Visible = AppSession.IsAdmin ? true : this._isDisplayAdditionalInfo;
            tbASIList.Visible = AppSession.IsAdmin ? true : this._isDisplayASI;
            tbASITList.Visible = AppSession.IsAdmin ? true : this._isDisplayASIT;
            tbParcelList.Visible = AppSession.IsAdmin ? true : this._isDisplayParcel;

            tbMoreDetail.Visible = tbRCList.Visible || tbADIList.Visible || tbASIList.Visible || tbASITList.Visible || tbParcelList.Visible;
        }

        /// <summary>
        /// create a label.
        /// </summary>
        /// <param name="labelKey">display name</param>
        /// <returns>return a label</returns>
        private AccelaLabel CreateLabel(string labelKey)
        {
            AccelaLabel alb = ControlBuildHelper.CreateUnitLabel(string.Empty);

            if (labelKey != string.Empty)
            {
                alb.LabelKey = labelKey;
                alb.ID = labelKey + DateTime.Now.Ticks;
            }
            else
            {
                alb.CssClass = "ACA_SmLabel ACA_SmLabel_FontSize";
            }

            return alb;
        }

        /// <summary>
        /// create a table.
        /// </summary>
        /// <param name="tableID">the table id</param>
        /// <returns>return a table</returns>
        private HtmlTable CreateTable(string tableID)
        {
            HtmlTable tb = new HtmlTable();
            tb.ID = tableID;
            tb.Style.Add(HtmlTextWriterStyle.Width, "100%");
            tb.Style.Add(HtmlTextWriterStyle.BorderWidth, "0");
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();
            tr.Cells.Add(tc);
            tb.Rows.Add(tr);
            return tb;
        }

        /// <summary>
        /// Sort app spec info groups to let the main group to be the first one
        /// </summary>
        /// <param name="appSpecInfoGroups">App spec info groups</param>
        /// <returns>Sorted app spec info groups</returns>
        private AppSpecificInfoGroupModel4WS[] SortAppSepcInfoGroups(AppSpecificInfoGroupModel4WS[] appSpecInfoGroups)
        {
            AppSpecificInfoGroupModel4WS[] newGroups = new AppSpecificInfoGroupModel4WS[appSpecInfoGroups.Length];

            int i = 0;

            foreach (AppSpecificInfoGroupModel4WS model in appSpecInfoGroups)
            {
                if (CapModel.capType.specInfoCode == model.groupCode)
                {
                    newGroups[i++] = model;
                }
            }

            foreach (AppSpecificInfoGroupModel4WS model in appSpecInfoGroups)
            {
                if (CapModel.capType.specInfoCode != model.groupCode)
                {
                    newGroups[i++] = model;
                }
            }

            return newGroups;
        }

        /// <summary>
        /// Display specific information
        /// </summary>
        /// <param name="appSpecInfoGroups">App SpecInfo Group Model Array</param>
        private void DisplayASIList(AppSpecificInfoGroupModel4WS[] appSpecInfoGroups)
        {
            // Reset default value
            this._isDisplayASI = false;

            try
            {
                //loop each field in each group and create corresponding control on UI
                if (appSpecInfoGroups == null || appSpecInfoGroups.Length == 0)
                {
                    return;
                }

                appSpecInfoGroups = SortAppSepcInfoGroups(appSpecInfoGroups);

                phPlumbingGroup.Controls.Clear();

                int groupIndex = 0; //indicates which group a field belongs to
                var expressionResult = ExpressionUtil.GetExpressionResultFromSession();

                foreach (AppSpecificInfoGroupModel4WS groupModel in appSpecInfoGroups)
                {
                    if (groupModel.fields == null || groupModel.fields.Length == 0)
                    {
                        continue;
                    }

                    string agencyCode = groupModel.capID != null ? groupModel.capID.serviceProviderCode : string.Empty;
                    string asiSecurity = ASISecurityUtil.GetASISecurity(agencyCode, groupModel.groupCode, groupModel.groupName, string.Empty, ModuleName);

                    if (ACAConstant.ASISecurity.None.Equals(asiSecurity, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    //if attributeValues in groupModule.fields are all null,they will not display in session
                    bool isEmpty = true;
                    ExpressionFieldModel[] expFields = null;

                    string capId = groupModel.fields[0].permitID1 + groupModel.fields[0].permitID2 + groupModel.fields[0].permitID3;

                    if (expressionResult != null && expressionResult.Count > 0 && expressionResult.ContainsKey(capId))
                    {
                        expFields = expressionResult[capId].Value.fields;
                    }

                    foreach (AppSpecificInfoModel4WS item in groupModel.fields)
                    {
                        if (item == null
                            || ACAConstant.COMMON_N.Equals(item.vchDispFlag, StringComparison.InvariantCulture)
                            || ACAConstant.COMMON_H.Equals(item.vchDispFlag, StringComparison.InvariantCulture))
                        {
                            continue;
                        }

                        ExpressionFieldModel expField = null;

                        if (expFields != null && expFields.Length > 0)
                        {
                            string key = string.Format("ASI::{0}::{1}", item.checkboxType, item.fieldLabel);
                            expField = expFields.FirstOrDefault(f => f.variableKey.Equals(key) && f.servProvCode.Equals(item.serviceProviderCode));
                        }

                        if (expField != null && expField.hidden != null && expField.hidden.Value)
                        {
                            continue;
                        }

                        string fieldValue = ScriptFilter.FilterScript(CapUtil.GetASIFieldValue(item, expField));

                        if (string.IsNullOrEmpty(fieldValue))
                        {
                            continue;
                        }

                        isEmpty = false;
                        break;
                    }

                    if (isEmpty)
                    {
                        continue;
                    }

                    groupIndex++;

                    if (groupIndex > 1)
                    {
                        string css = @"<div class='ACA_TabRow ACA_BkGray'>&nbsp;</div>";
                        this.phPlumbingGroup.Controls.Add(new LiteralControl(css));
                    }

                    phPlumbingGroup.Controls.Add(new LiteralControl("<div class=\"MoreDetail_ItemTitle MoreDetail_ItemCol1 ACA_Title_Text font12px\">" + I18nStringUtil.GetString(groupModel.resAlternativeLabel, groupModel.alternativeLabel, groupModel.resGroupName, groupModel.groupName) + "</div>"));
                    bool isOddColumn = false;
                    int itemIndex = 0;

                    foreach (AppSpecificInfoModel4WS item in groupModel.fields)
                    {
                        if (item == null
                            || ACAConstant.COMMON_N.Equals(item.vchDispFlag, StringComparison.InvariantCulture)
                            || ACAConstant.COMMON_H.Equals(item.vchDispFlag, StringComparison.InvariantCulture))
                        {
                            continue;
                        }

                        ExpressionFieldModel expField = null;

                        if (expFields != null && expFields.Length > 0)
                        {
                            string key = string.Format("ASI::{0}::{1}", item.checkboxType, item.fieldLabel);
                            expField = expFields.FirstOrDefault(f => f.variableKey.Equals(key) && f.servProvCode.Equals(item.serviceProviderCode));
                        }

                        if (expField != null && expField.hidden != null && expField.hidden.Value)
                        {
                            continue;
                        }

                        string fieldValue = ScriptFilter.FilterScript(CapUtil.GetASIFieldValue(item, expField));

                        if (string.IsNullOrEmpty(fieldValue))
                        {
                            continue;
                        }

                        if (ACAConstant.ASISecurity.None.Equals(ASISecurityUtil.GetASISecurity(item, ModuleName)))
                        {
                            continue;
                        }

                        // Only one of fields has value, ASI Section should be displayed
                        if (!string.IsNullOrEmpty(fieldValue))
                        {
                            this._isDisplayASI = true;

                            if (item.fieldType == ((int)FieldType.HTML_TEXTBOX_OF_DATE).ToString() && IsDisplayHijriCalendar)
                            {
                                fieldValue = HijriDateUtil.ToHijriDate(fieldValue);
                            }
                        }

                        if (groupModel.columnLayout == 2)
                        {
                            isOddColumn = itemIndex % 2 == 0;
                            LayoutASIWith2Columns(item, fieldValue, isOddColumn);

                            itemIndex++;
                        }
                        else
                        {
                            LayoutASIWith1Column(item, fieldValue);
                        }
                    }

                    //The last column is odd column add div close tag.
                    if (groupModel.columnLayout == 2 && isOddColumn)
                    {
                        phPlumbingGroup.Controls.Add(new LiteralControl("</div>"));
                    }
                }
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        /// <summary>
        /// Layout asi view with one column
        /// </summary>
        /// <param name="appSpecificInfo">the appSpecificInfo model.</param>
        /// <param name="fieldValue">the field value.</param>
        /// <param name="isOddColumn">is odd column or not</param>
        private void LayoutASIWith2Columns(AppSpecificInfoModel4WS appSpecificInfo, string fieldValue, bool isOddColumn)
        {
            string htmlTemplate = string.Empty;

            if (isOddColumn)
            {
                htmlTemplate = "<div class='ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize ACA_Overflow'>";
            }

            string labelWidth = StandardChoiceUtil.GetASILabelWidth();
            string widthStyle = string.IsNullOrEmpty(labelWidth) ? string.Empty : " style='width:" + labelWidth + ";'";

            htmlTemplate += string.Format(
                                          @"
                                           <div class='ACA_FLeft ASIReview2Columns'>
                                               <span class='ACA_SmLabelBolder'{0}>{1}</span>
                                               <span class='ACA_SmLabel'>{2}</span>
                                           </div>", 
                                           widthStyle, 
                                           ScriptFilter.FilterScript(CapUtil.GetASIFieldLabel(appSpecificInfo)), 
                                           fieldValue);

            if (!isOddColumn)
            {
                htmlTemplate += "</div>";
            }

            phPlumbingGroup.Controls.Add(new LiteralControl(htmlTemplate));
        }

        /// <summary>
        /// Layout asi view with one column
        /// </summary>
        /// <param name="appSpecificInfo">the appSpecificInfo model.</param>
        /// <param name="fieldValue">the field value.</param>
        private void LayoutASIWith1Column(AppSpecificInfoModel4WS appSpecificInfo, string fieldValue)
        {
            string htmlTemplate = string.Format(
                                                @"
                                                 <div class='MoreDetail_ItemColASI MoreDetail_ItemCol1' style='width:{0};vertical-align:top'>
                                                     <span class='ACA_SmLabelBolder font11px'>{1}</span>
                                                 </div>
                                                 <div class='MoreDetail_ItemColASI MoreDetail_ItemCol2'>
                                                     <span class='ACA_SmLabel ACA_SmLabel_FontSize'>{2}</span>
                                                 </div>",
                                                StandardChoiceUtil.GetASILabelWidth(),
                                                CapUtil.GetASIFieldLabel(appSpecificInfo),
                                                fieldValue);

            phPlumbingGroup.Controls.Add(new LiteralControl(htmlTemplate));
        }

        /// <summary>
        /// Display cap detail information
        /// </summary>
        private void DisplayCapDetail()
        {
            ExpressionUtil.RunExpressionForOnload(ModuleName);
            int sectionCount = 0;
            string projectDesc = string.Empty;
            string applicatnt = string.Empty;
            string licensedProfessional = string.Empty;
            string owner = string.Empty;
            Panel htc = null;
            HtmlTableRow tr = new HtmlTableRow();

            foreach (KeyValuePair<string, object> section in this.ReOrderSection)
            {
                int lines = 0;
                bool isDisplayCurrentSection;
                bool isAddInLeft = sectionCount % 2 == 0;
                string sectionName = section.Value.ToString();
                bool showSection = !AppSession.IsAdmin && !HiddenSections.Contains(sectionName);

                switch (section.Key)
                {
                    case GviewID.DetailInformation:

                        if (showSection)
                        {
                            projectDesc = ModelUIFormat.FormatProjectDesc4Desplay(CapModel, out lines, out isDisplayCurrentSection, ModuleName);
                        }

                        AddCtrlToPanel(projectDesc, "per_permitDetail_label_projectl", ref htc, false);
                        break;

                    case GViewConstant.SECTION_APPLICANT:

                        if (IsNeedShowApplicantSection() && showSection)
                        {
                            applicatnt = ModelUIFormat.FormatApplicant4Display(CapModel.applicantModel, ModuleName, out lines);
                        }

                        AddCtrlToPanel(applicatnt, "per_permitDetail_label_applicant", ref htc, false);
                        break;

                    case GViewConstant.SECTION_LICENSE:

                        if (showSection)
                        {
                            ReOrderLicenseList();
                            licensedProfessional = ModelUIFormat.FormatLicenseModel4Basic(TempModelConvert.ConvertToLicenseProfessionalModelList(CapModel.licenseProfessionalList), ModuleName, out lines, false);
                        }

                        AddCtrlToPanel(licensedProfessional, "per_permitDetail_label_license", ref htc, true);
                        break;

                    case GViewConstant.SECTION_OWNER:

                        if (showSection)
                        {
                            owner = ModelUIFormat.FormatOwner4Detail(CapModel.ownerModel, ModuleName, out lines);
                        }

                        AddCtrlToPanel(owner, "per_permitdetail_label_owner", ref htc, false);
                        break;
                }

                if ((htc != null && lines > 0) || AppSession.IsAdmin)
                {
                    sectionCount++;

                    if (AppSession.IsAdmin)
                    {
                        // create divs and put them into permit detail table for Admin
                        if (TBPermitDetailTest.Rows.Count == 0)
                        {
                            HtmlTableCell cell = new HtmlTableCell();
                            tr = new HtmlTableRow();
                            tr.Cells.Add(cell);
                            TBPermitDetailTest.Rows.Add(tr);
                        }

                        AccelaDiv div = new AccelaDiv();
                        div.Attributes.Add("class", "ACA_FLeft td_parent_left");
                        div.ID = sectionName;
                        div.Controls.Add(htc);
                        TBPermitDetailTest.Rows[0].Cells[0].Controls.Add(div);
                    }
                    else
                    {
                        if (isAddInLeft)
                        {
                            tr = new HtmlTableRow();
                            tr.Attributes.Add("class", "ACA_FLeft");
                            tr.Attributes.Add("style", "margin-bottom:5px");
                        }

                        HtmlTableCell cell = new HtmlTableCell();
                        cell.Attributes.Add("class", "td_parent_left");
                        tr.Cells.Add(cell);
                        cell.Controls.Add(htc);
                        TBPermitDetailTest.Rows.Add(tr);
                    }
                }
            }

            TBPermitDetailTest.Style.Add("table-layout", "fixed");
            TBPermitDetailTest.Style.Add("border-collapse", "collapse");
            TBPermitDetailTest.Width = "714px";

            DisplayMoreDetailSection();
        }

        /// <summary>
        /// Create and display controls for more details
        /// </summary>
        private void DisplayMoreDetailSection()
        {
            if (!AppSession.IsAdmin && !HiddenSections.Contains(CapDetailSectionType.RELATED_CONTACTS.ToString()))
            {
                RelatContactList.DataSource = ChangeContactData(CapModel);
                RelatContactList.DataBind();
            }

            if (!HiddenSections.Contains(CapDetailSectionType.APPLICATION_INFORMATION.ToString()))
            {
                if (!ACAConstant.ASISecurity.None.Equals(ASISecurityUtil.GetASISecurity(CapModel.appSpecificInfoGroups, ModuleName)))
                {
                    DisplayASIList(CapModel.appSpecificInfoGroups);
                }
            }

            if (!HiddenSections.Contains(CapDetailSectionType.APPLICATION_INFORMATION_TABLE.ToString()))
            {
                ASITable asit = new ASITable(CapModel, ModuleName, IsPostBack, false);

                var expressionResut = ExpressionUtil.GetExpressionResultFromSession();

                if (expressionResut != null)
                {
                    foreach (var exp in expressionResut)
                    {
                        ExpressionUtil.HandleASITExpressionResult(exp.Value.Key, exp.Value.Value, false);
                    }
                }

                pnlASITable.Controls.Add(asit.DisplayInDetailView());

                // Reset this field not show ASIT Section
                this._isDisplayASIT = false;

                if (AppSession.IsAdmin || !string.IsNullOrEmpty(asit.StrASIT))
                {
                    this._isDisplayASIT = true;
                }
            }

            if (!HiddenSections.Contains(CapDetailSectionType.ADDITIONAL_INFORMATION.ToString()))
            {
                AddtionalInfo additionalInfo = CapUtil.BuildAddtionalInfo(CapModel);

                string agencyCode = CapModel.capID.serviceProviderCode;
                FillAdditionalInfo(additionalInfo, agencyCode);
            }

            if (!HiddenSections.Contains(CapDetailSectionType.PARCEL_INFORMATION.ToString()))
            {
                DisplayParcelList();
            }
        }

        /// <summary>
        /// Hide corresponding sections for ACA Admin
        /// </summary>
        private void HideControlsInAdmin()
        {
            if (AppSession.IsAdmin)
            {
                StringBuilder hideControlIds = new StringBuilder();

                if (HiddenSections.Count > 0)
                {
                    foreach (string controlId in HiddenSections)
                    {
                        hideControlIds.AppendFormat("'{0}',", controlId);
                    }

                    hideControlIds.Remove(hideControlIds.Length - 1, 1);
                    hideControlIds.Insert(0, "[");
                    hideControlIds.Append("]");
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideControl", "hideControls(" + hideControlIds + ");", true);
            }
        }

        /// <summary>
        /// display parcel list.
        /// </summary>
        private void DisplayParcelList()
        {
            // Reset it not show Parcel Senction
            this._isDisplayParcel = false;

            Parcel pcList = new Parcel(CapModel);
            try
            {
                //loop each field in each group and create corresponding control on UI
                if (pcList == null)
                {
                    return;
                }

                palParceList.Controls.Clear();

                int itemIndex = 0;

                foreach (ParcelField field in pcList.ParcelList)
                {
                    if (string.IsNullOrEmpty(field.Key) || string.IsNullOrEmpty(field.Value))
                    {
                        continue;
                    }

                    Literal ltlField = new Literal();
                    string labelValue = LabelUtil.GetTextByKey(field.Key, ModuleName);

                    if (itemIndex % 2 == 0)
                    {
                        ltlField.Text = @"<div class='MoreDetail_ItemCol MoreDetail_ItemCol1'><h2>";
                    }
                    else
                    {
                        ltlField.Text = @"<div class='MoreDetail_ItemCol MoreDetail_ItemCol2'><h2>";
                    }

                    // If the Label's Value is empty, it should show Lebel's key and ":"
                    string strKey = field.Key + ":";
                    ltlField.Text += ScriptFilter.FilterScript(labelValue == string.Empty ? strKey : labelValue) + "</h2>";
                    ltlField.Text += "<div class='ACA_SmLabel ACA_SmLabel_FontSize'>" + field.Value + "</div></div>";

                    this._isDisplayParcel = true;

                    palParceList.Controls.Add(ltlField);

                    itemIndex++;
                }
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        /// <summary>
        /// Fill Additional Information on Cap Detail Page
        /// </summary>
        /// <param name="additionalInfo">Additional Information Entity</param>
        /// <param name="agencyCode">Agency Code</param>
        private void FillAdditionalInfo(AddtionalInfo additionalInfo, string agencyCode)
        {
            // Reset default value
            _isDisplayAdditionalInfo = false;

            // In ACA Admin, Additional Information Section should display under More Detail
            if (AppSession.IsAdmin)
            {
                _isDisplayAdditionalInfo = true;
                return;
            }

            // If no Additional Inforamtion, it will hidden this section
            if (additionalInfo == null)
            {
                _isDisplayAdditionalInfo = false;
                return;
            }

            Literal infoContainer = new Literal();

            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.SECTION_CAPDESCRIPTION
            };

            SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, permission, GviewID.CAPDescriptionEdit);
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            int displayFieldCount = 0;
            double parsedJobValue = 0;
            double? jobValue = 0;

            if (I18nNumberUtil.TryParseMoneyFromWebService(additionalInfo.JobValue, out parsedJobValue))
            {
                jobValue = parsedJobValue;
            }

            // Job value
            if (jobValue != null && jobValue.Value != 0 && gviewBll.IsFieldVisible(models, "txtJobValue"))
            {
                this._isDisplayAdditionalInfo = true;
                string jobvalueString = I18nNumberUtil.FormatMoneyForUI(jobValue);
                infoContainer.Text += BuildingOneDivContent("capDescriptionEdit_label_jobValue", jobvalueString, ref displayFieldCount);
            }

            // Housing Unit
            if (!string.IsNullOrEmpty(additionalInfo.HousingUnit) && !"0".Equals(additionalInfo.HousingUnit) && gviewBll.IsFieldVisible(models, "txtHousingUnit"))
            {
                this._isDisplayAdditionalInfo = true;
                infoContainer.Text += BuildingOneDivContent("capDescriptionEdit_label_houseUnit", additionalInfo.HousingUnit, ref displayFieldCount);
            }

            // Buildings Numbers
            if (!string.IsNullOrEmpty(additionalInfo.BuildingNumber) && !"0".Equals(additionalInfo.BuildingNumber) && gviewBll.IsFieldVisible(models, "txtBuildingsNumbers"))
            {
                this._isDisplayAdditionalInfo = true;
                infoContainer.Text += BuildingOneDivContent("capDescriptionEdit_label_buildNumber", additionalInfo.BuildingNumber, ref displayFieldCount);
            }

            // Public Owned
            if (!string.IsNullOrEmpty(additionalInfo.PublicOwner) && gviewBll.IsFieldVisible(models, "chkPublicOwned"))
            {
                this._isDisplayAdditionalInfo = true;
                infoContainer.Text += BuildingOneDivContent("capDescriptionEdit_label_publicOwner", ModelUIFormat.FormatYNLabel(additionalInfo.PublicOwner), ref displayFieldCount);
            }

            // Construc Type
            if (!string.IsNullOrEmpty(additionalInfo.ConstructionType) && gviewBll.IsFieldVisible(models, "ddlConstrucType"))
            {
                this._isDisplayAdditionalInfo = true;
                infoContainer.Text += BuildingOneDivContent("capDescriptionEdit_label_constructType", CapUtil.GetOneConstuctionTypeDescription(additionalInfo.ConstructionType, agencyCode), ref displayFieldCount);
            }

            tdADIContent.Controls.Add(infoContainer);
        }

        /// <summary>
        /// get applicant restrict display
        /// </summary>
        /// <returns>return user role</returns>
        private UserRoleType GetApplicantRestrictDisplay()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            BizDomainModel4WS model = new BizDomainModel4WS();
            model.bizdomain = ACAConstant.ACA_APPLICANT_DISPLAY_RULE;
            model.bizdomainValue = ModuleName;
            model.serviceProviderCode = CapUtil.GetAgencyCode(ModuleName);
            model = bizBll.GetBizDomainListByModel(model, model.bizdomainValue, "ACA");
            if (model != null && model.description != null && ValidationUtil.IsInt(model.description))
            {
                return (UserRoleType)int.Parse(model.description);
            }
            else
            {
                return UserRoleType.AllACAUsers;
            }
        }

        /// <summary>
        /// Have the right to access or not.
        /// </summary>
        /// <param name="contactType">Contact type of contact</param>
        /// <returns>return true or false</returns>
        private bool HasRightAccess(string contactType)
        {
            IXPolicyBll ixpBll = ObjectFactory.GetObject<IXPolicyBll>();
            UserRolePrivilegeModel userRole = ixpBll.GetPolicyByContactType(ModuleName, Server.HtmlDecode(contactType));

            //If only set contact permission,we must be consider the permission set in capcontact model in spear form.
            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            bool hasContactAccess = proxyUserRoleBll.HasReadOnlyPermission(AppSession.GetCapModelFromSession(ModuleName), userRole);

            return hasContactAccess;
        }

        /// <summary>
        /// judge if to show applicant section
        /// </summary>
        /// <returns>return true or false</returns>
        private bool IsNeedShowApplicantSection()
        {
            if (AppSession.IsAdmin)
            {
                return false;
            }

            bool hasRight = true;

            if (CapModel.applicantModel != null && CapModel.applicantModel.people != null)
            {
                hasRight = HasRightAccess(CapModel.applicantModel.people.contactType);
            }

            return hasRight;
        }

        //===============================================================================

        /// <summary>
        /// select one license to change order to top when primary license is "Y".
        /// </summary>
        private void ReOrderLicenseList()
        {
            if (CapModel.licenseProfessionalList != null && CapModel.licenseProfessionalList.Length > 0)
            {
                LicenseProfessionalModel changeLocationLS;
                for (int i = 0; i < CapModel.licenseProfessionalList.Length; i++)
                {
                    if (CapModel.licenseProfessionalList[i].printFlag == ACAConstant.COMMON_Y)
                    {
                        changeLocationLS = TempModelConvert.ConvertToLicenseProfessionalModel(CapModel.licenseProfessionalList[i]);
                        CapModel.licenseProfessionalList[i] = CapModel.licenseProfessionalList[0];
                        CapModel.licenseProfessionalList[0] = TempModelConvert.ConvertToLicenseProfessionalModel4WS(changeLocationLS);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get contact data on Data View format return 
        /// </summary>
        /// <param name="capModel">CapModel value</param>
        /// <returns>Return DataView</returns>
        private DataView ChangeContactData(CapModel4WS capModel)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            // Rest it not to show Related Contacts Section
            this._isDispalyContact = false;

            // Define the columns of the table.
            dt.Columns.Add(new DataColumn("ContactContent", typeof(string)));

            if (capModel != null && capModel.contactsGroup != null)
            {
                // Populate the table with sample values.
                for (int i = 0; i < capModel.contactsGroup.Length; i++)
                {
                    string content = string.Empty;

                    //add privilege to control display contact
                    bool hasRight = true;

                    if (capModel.contactsGroup[i] != null && capModel.contactsGroup[i].people != null)
                    {
                        hasRight = HasRightAccess(capModel.contactsGroup[i].people.contactType);
                    }

                    if (hasRight)
                    {
                        content = GetSpecificContentForContact(capModel.contactsGroup[i], i);
                    }

                    if (!string.IsNullOrEmpty(content))
                    {
                        dr = dt.NewRow();
                        dr[0] = content;
                        dt.Rows.Add(dr);
                    }
                }
            }

            DataView dv = new DataView(dt);
            return dv;
        }

        /// <summary>
        /// Get specific format content for contact object.
        /// </summary>
        /// <param name="ccm">Cap Contact Model</param>
        /// <param name="intNumber">The number of contact</param>
        /// <returns>Return contact string</returns>
        private string GetSpecificContentForContact(CapContactModel4WS ccm, int intNumber)
        {
            StringBuilder contents = new StringBuilder();
            StringBuilder tempContents = new StringBuilder();
            string tempContent = string.Empty;
            bool isEmptyRow = true;

            if (intNumber % 2 == 0)
            {
                contents.Append("<div class='MoreDetail_ItemCol MoreDetail_ItemCol1'><h2>");
            }
            else
            {
                contents.Append("<div class='MoreDetail_ItemCol MoreDetail_ItemCol2'><h2>");
            }

            tempContents.Append(contents.ToString());

            if (ccm != null 
                && !ACAConstant.INVALID_STATUS.Equals(ccm.people.auditStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                string contactTypeText = StandardChoiceUtil.GetContactTypeByKey(ccm.people.contactType);
                contents.Append(string.Format(GetTextByKey("aca_permitDetail_label_contactTypeTitlePattern"), contactTypeText));
                contents.Append("</h2>");
                contents.Append(" <div class='ACA_ConfigInfo ACA_FLeft' style='width:30%;'>");
                contents.Append(" <ul>");

                tempContent = ModelUIFormat.FormatCapContactModel4Basic(ccm, ModuleName, false);
                contents.Append(tempContent);

                if (!string.IsNullOrEmpty(tempContent))
                {
                    isEmptyRow = false;
                    this._isDispalyContact = true;
                }

                tempContent = ModelUIFormat.FormatCapContactModel4Ext(ccm, ModuleName);
                contents.Append(tempContent);

                if (!string.IsNullOrEmpty(tempContent))
                {
                    isEmptyRow = false;
                    this._isDispalyContact = true;
                }

                contents.Append(" </ul>");
                contents.Append("</div>");
                contents.Append(" <div class='ACA_TabRow ACA_FLeft'>");

                //get template data
                if (CapModel.contactsGroup[intNumber] != null && CapModel.contactsGroup[intNumber].people.attributes != null)
                {
                    foreach (TemplateAttributeModel item in CapModel.contactsGroup[intNumber].people.attributes)
                    {
                        if (CapModel.capID.serviceProviderCode.Equals(item.serviceProviderCode, StringComparison.InvariantCultureIgnoreCase)
                            && !string.IsNullOrEmpty(item.attributeValue)
                            && !ValidationUtil.IsNo(item.vchFlag))
                        {
                            contents.Append(" <ul><p>");
                            string valueText = ModelUIFormat.GetTemplateValue4Display(item);
                            contents.Append(ScriptFilter.FilterScript(valueText));

                            if (!string.IsNullOrEmpty(valueText))
                            {
                                isEmptyRow = false;
                                this._isDispalyContact = true;
                            }

                            contents.Append(" </p></ul>");
                        }
                    }
                }

                //get generic template data.
                if (CapModel.contactsGroup[intNumber] != null && CapModel.contactsGroup[intNumber].people.template != null)
                {
                    var fields = GenericTemplateUtil.GetAllFields(CapModel.contactsGroup[intNumber].people.template);

                    if (fields != null && fields.Count() > 0)
                    {
                        foreach (var field in fields)
                        {
                            string asiSecurity = ASISecurityUtil.GetASISecurity(field.serviceProviderCode, field.groupName, field.subgroupName, field.fieldName, ModuleName);

                            if (ACAConstant.ASISecurity.None.Equals(asiSecurity, StringComparison.OrdinalIgnoreCase)
                                || (field.acaTemplateConfig != null && (ValidationUtil.IsHidden(field.acaTemplateConfig.acaDisplayFlag) || ValidationUtil.IsNo(field.acaTemplateConfig.acaDisplayFlag))))
                            {
                                continue;
                            }

                            string fieldValue = ScriptFilter.FilterScript(ModelUIFormat.GetTemplateValue4Display(field));

                            if (!string.IsNullOrEmpty(fieldValue))
                            {
                                contents.Append("<ul><p>");
                                string atlLabel = string.Empty;

                                if (field.acaTemplateConfig != null)
                                {
                                    atlLabel = I18nStringUtil.GetString(field.acaTemplateConfig.resFieldLabel, field.acaTemplateConfig.fieldLabel);
                                }

                                string fieldName = I18nStringUtil.GetString(atlLabel, field.displayFieldName, field.fieldName);
                                contents.Append(fieldName);
                                contents.Append(ACAConstant.COLON_CHAR);
                                contents.Append(fieldValue);
                                contents.Append("</p></ul>");

                                isEmptyRow = false;
                                this._isDispalyContact = true;
                            }
                        }
                    }

                    if (CapModel.contactsGroup[intNumber].people.template.templateTables != null
                        && CapModel.contactsGroup[intNumber].people.template.templateTables.Length > 0)
                    {
                        string genericTemplateTableContent = ModelUIFormat.GenerateGenericTemplateTableInDetailView(CapModel.contactsGroup[intNumber].people.template.templateTables, ModuleName);

                        if (!string.IsNullOrEmpty(genericTemplateTableContent))
                        {
                            contents.Append("<ul><div class='capdetail_template_table'><h2>");
                            contents.Append("<a href=\"javascript:void(0);\" onclick='ControlDisplay($(this).parent().next().get(0),this.firstChild,true,this,$(this).next().get(0))'");
                            contents.AppendFormat("title=\"{0}\" class=\"NotShowLoading\">", GetTitleByKey("img_alt_expand_icon", "aca_capdetail_contact_label_templatetable"));
                            contents.AppendFormat("<img style=\"cursor: pointer; border-width:0px;\" alt=\"{0}\" src=\"{1}\"/>", GetTextByKey("img_alt_expand_icon"), ImageUtil.GetImageURL("plus_expand.gif"));
                            contents.Append("</a>&nbsp;");
                            contents.AppendFormat("<span style=\"display: inline-block\">{0}</span></h2>", GetTextByKey("aca_capdetail_contact_label_templatetable"));
                            contents.Append("<div style=\"display:none;\">");
                            contents.Append(genericTemplateTableContent);
                            contents.Append("</div>");
                            contents.Append("</div></ul>");

                            isEmptyRow = false;
                            this._isDispalyContact = true;
                        }
                    }
                }

                contents.Append("</div>");

                //contact address section
                if (StandardChoiceUtil.IsEnableContactAddress())
                {
                    contents.Append("<div class='ACA_SmLabel ACA_SmLabel_FontSize'>");
                    tempContent = ModelUIFormat.FormatContactAddressSection(ModuleName, ccm);
                    contents.Append(tempContent);

                    if (!string.IsNullOrEmpty(tempContent))
                    {
                        isEmptyRow = false;
                        this._isDispalyContact = true;
                    }

                    contents.Append("</div>");
                }
            }

            contents.Append("</div>");
            tempContents.Append("</div>");

            if (tempContents.ToString() == contents.ToString())
            {
                contents.Replace(contents.ToString(), string.Empty);
            }

            return isEmptyRow ? string.Empty : contents.ToString();
        }
        
        #endregion Methods
    }
}
