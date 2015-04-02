#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EmailReport.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: EmailReport.aspx.cs 277863 2014-08-22 05:23:34Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.People;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Report
{
    /// <summary>
    /// Email Report Class
    /// </summary>
    public partial class EmailReport : BasePageWithoutMaster
    {
        /// <summary>
        /// ID prefix.
        /// </summary>
        private const string ID_PREFIX = "id";

        /// <summary>
        /// Gets Attach Contact Type.
        /// </summary>
        private string AttachContactType
        {
            get
            {
                string attachContacttype = Request.QueryString[ACAConstant.ATTACH_CONTACT_TYPE];
                return string.IsNullOrEmpty(attachContacttype) ? string.Empty : attachContacttype;
            }
        }

        /// <summary>
        /// Gets or sets people count.
        /// </summary>
        private int PeopleCount
        {
            get
            {
                return ViewState["peopleCount"] == null ? 0 : (int)ViewState["peopleCount"];
            }

            set
            {
                ViewState["peopleCount"] = value;
            }
        }

        /// <summary>
        /// Gets or sets sub CAP ID.
        /// </summary>
        private CapIDModel4WS SubCapID
        {
            get
            {
                if (ViewState["subCapID"] == null)
                {
                    return null;
                }
                else
                {
                    return (CapIDModel4WS)ViewState["subCapID"];
                }
            }

            set
            {
                ViewState["subCapID"] = value;
            }
        }

        /// <summary>
        /// Gets email Report Name.
        /// </summary>
        private string EmailReportName
        {
            get
            {
                string emailReportName = Request.QueryString[ACAConstant.EMAIL_REPORT_NAME];
                return !string.IsNullOrEmpty(emailReportName) ? emailReportName : this.Session.SessionID + ACAConstant.SPLIT_CHAR4URL1 + CommonUtil.GetRandomUniqueID().Substring(0, 18);
            }
        }

        /// <summary>
        /// Gets selected ALtIDs in permit list.
        /// </summary>
        private string AltIDs
        {
            get
            {
                string id = Request.QueryString[ACAConstant.ID];
                return string.IsNullOrEmpty(id) ? string.Empty : id;
            }
        }

        /// <summary>
        /// Page Load Event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SubCapID = ReportUtil.GetSubCapID();
            if (!Page.IsPostBack)
            {
                BindEmailList();
            }
        }

        /// <summary>
        /// Email List ItemDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void EmailList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PeopleModel4WS people = (PeopleModel4WS)e.Row.DataItem;

                HtmlGenericControl divInstruction = (HtmlGenericControl)e.Row.FindControl("divInstruction");
                AccelaCheckBox chkEmailName = (AccelaCheckBox)e.Row.FindControl("chkEmailName");
                AccelaEmailText txtEmailAddress = (AccelaEmailText)e.Row.FindControl("txtEmailAddress");

                chkEmailName.InputAttributes.Add("relatedEmailControlID", txtEmailAddress.ClientID);
                chkEmailName.ToolTip = people.fullName;
                txtEmailAddress.ToolTip = GetTextByKey("aca_report_email_instruction_label");

                if (e.Row.RowIndex == PeopleCount - 1)
                {
                    divInstruction.Visible = true;
                    chkEmailName.LabelKey = "aca_report_email_address_other_label";
                }
                else
                {
                    chkEmailName.Checked = true;
                    chkEmailName.Text = people.fullName;
                }
            }
        }

        /// <summary>
        /// Click save button event
        /// </summary>
        /// <param name="sender">event sender for Save button</param>
        /// <param name="e">event args</param>
        protected void SendEmailButton_Click(object sender, EventArgs e)
        {
            StringBuilder emailAddressBuilder = new StringBuilder();

            foreach (GridViewRow row in EmailList.Rows)
            {
                AccelaCheckBox curChkEmailName = (AccelaCheckBox)row.FindControl("chkEmailName");
                AccelaEmailText txtEmailAddress = (AccelaEmailText)row.FindControl("txtEmailAddress");

                if (curChkEmailName.Checked && !string.IsNullOrEmpty(txtEmailAddress.Text))
                {
                    emailAddressBuilder.Append(txtEmailAddress.Text.Trim()).Append(";");
                }
            }

            string emailAddresses = emailAddressBuilder.ToString();

            if (string.IsNullOrEmpty(emailAddresses))
            {
                resultMessage.Show(MessageType.Notice, "aca_report_email_address_noselected_label", MessageSeperationType.Both);
                return;
            }

            string reportID = HttpContext.Current.Request.QueryString["reportID"];
            string reportType = Request.QueryString["reportType"];

            ReportInfoModel4WS reportInfoModel4WS = ReportUtil.GetReportInfoModel4WS(reportType, reportID, SubCapID, ModuleName, EmailReportName);
            reportInfoModel4WS.emailAddress = emailAddresses;

            try
            {
                IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));
                reportBll.SendReportInEmail(reportInfoModel4WS);
                string message = GetTextByKey("aca_report_email_sendsuccessinformation_label");
                string script = "window.opener.showNormalMessage('" + MessageUtil.FilterQuotation(message) + "','Success');window.close();";
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ShowReportErrorJS", script, true);
            }
            catch (Exception exp)
            {
                resultMessage.Show(MessageType.Error, "aca_report_email_sendfailedinformation_label", MessageSeperationType.Both);
            }
        }

        /// <summary>
        /// Bind Email list.
        /// </summary>
        private void BindEmailList()
        {
            //according to attachContact get the contact and professor
            ArrayList peopleList = new ArrayList();

            if (!AppSession.IsAdmin && (ACAConstant.ATTACH_ALL_CAP_CONTACTS.Equals(AttachContactType) ||
                    ACAConstant.ATTACH_PRIMARY_CAP_CONTACTS.Equals(AttachContactType)))
            {
                CapIDModel4WS[] capIds = GetCapIdModels();

                if (capIds != null && capIds.Length > 0)
                {
                    IPeopleBll peopleBLL = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));
                    PeopleModel4WS[] peopleModels = peopleBLL.GetPeoplesByCapIDs(capIds);

                    if (peopleModels != null && peopleModels.Length > 0)
                    {
                        if (ACAConstant.ATTACH_PRIMARY_CAP_CONTACTS.Equals(AttachContactType))
                        {
                            var peoples = from p in peopleModels where p.flag != ACAConstant.COMMON_N select p;
                            peopleModels = peoples.ToArray();
                        }

                        if (peopleModels != null && peopleModels.Length > 0)
                        {
                            peopleList.AddRange(peopleModels);
                        }
                    }
                }
            }

            PeopleModel4WS peopleModel = new PeopleModel4WS();
            peopleModel.fullName = GetTextByKey("aca_report_email_address_other_label", ModuleName);
            peopleList.Add(peopleModel);

            int index = 0;
            foreach (PeopleModel4WS model in peopleList)
            {
                model.id = ID_PREFIX + index;
                index++;
            }

            PeopleCount = peopleList.Count;

            this.EmailList.DataSource = peopleList;
            this.EmailList.DataBind();
        }

        /// <summary>
        /// Gets cap ID models
        /// </summary>
        /// <returns>cap id models</returns>
        private CapIDModel4WS[] GetCapIdModels()
        {
            List<CapIDModel4WS> capIdList = new List<CapIDModel4WS>();

            if (!string.IsNullOrEmpty(AltIDs))
            {
                string[] ids = AltIDs.Split(ACAConstant.COMMA_CHAR);

                foreach (string id in ids)
                {
                    string[] capIds = id.Split(ACAConstant.SPLIT_CHAR4.ToCharArray());
                    CapIDModel4WS capId = new CapIDModel4WS();
                    capId.serviceProviderCode = capIds[0];
                    capId.id1 = capIds[1];
                    capId.id2 = capIds[2];
                    capId.id3 = capIds[3];
                    capIdList.Add(capId);
                }
            }
            else
            {
                SubCapID = ReportUtil.GetSubCapID();

                if (SubCapID != null || !string.IsNullOrEmpty(ModuleName))
                {
                    CapIDModel4WS capId = ReportUtil.GetCapID(SubCapID, ModuleName);
                    if (!string.IsNullOrEmpty(capId.serviceProviderCode))
                    {
                        capIdList.Add(capId);
                    }
                }
            }

            return capIdList.ToArray();
        }
    }
}
