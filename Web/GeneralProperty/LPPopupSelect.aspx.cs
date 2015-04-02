#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LPPopupSelect.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description: Popup for select license professional.
 *
 *  Notes:
 *      $Id: LPPopupSelect.aspx.cs 183850 2013-07-18 15:43:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.GeneralProperty
{
    /// <summary>
    /// This class provides select the license professional in file upload page.
    /// </summary>
    public partial class LPPopupSelect : PopupDialogBasePage
    {
        /// <summary>
        /// Page load event method.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Set pop up page title.
                SetPageTitleKey("ACA_FileUploadPage_PageTitle");

                List<LicenseModel4WS> lpList = AttachmentUtil.GetAvaliableLicense4PeopleDocument(ConfigManager.AgencyCode);

                List<ListItem> listItems = new List<ListItem>();

                foreach (LicenseModel4WS lpModel in lpList)
                {
                    ListItem listItem = new ListItem();

                    string currentUserName = string.Empty;

                    if (ContactType4License.Organization.ToString().Equals(lpModel.typeFlag, StringComparison.OrdinalIgnoreCase))
                    {
                        currentUserName = ScriptFilter.EncodeHtmlEx(lpModel.businessName);
                    }
                    else
                    {
                        currentUserName = UserUtil.FormatToFullName(
                            ScriptFilter.EncodeHtmlEx(lpModel.contactFirstName),
                            ScriptFilter.EncodeHtmlEx(lpModel.contactMiddleName),
                            ScriptFilter.EncodeHtmlEx(lpModel.contactLastName));
                    }

                    listItem.Text = currentUserName;
                    listItem.Value = lpModel.licSeqNbr;

                    listItems.Add(listItem);
                }

                DropDownListBindUtil.BindDDL(listItems, ddlProfessional);
            }
        }
    }
}