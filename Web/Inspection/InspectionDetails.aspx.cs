#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: InspectionDetails.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *
 * </pre>
 */
#endregion Header

using System;

using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// inspection detail page.
    /// </summary>
    public partial class InspectionDetails : PopupDialogBasePage
    {
        #region Properties

        /// <summary>
        /// Gets cap model.
        /// </summary>
        /// <value>The cap model.</value>
        private CapModel4WS Cap
        {
            get 
            {
                return AppSession.GetCapModelFromSession(ModuleName);
            }
        }

        /// <summary>
        /// Gets current user
        /// </summary>
        /// <value>The current user.</value>
        private User CurrentUser
        {
            get
            {
                return AppSession.User;
            }
        }

        /// <summary>
        /// Gets the inspection sequence number.
        /// </summary>
        /// <value>The inspection ID.</value>
        private string InspectionID
        {
            get
            {
                return !string.IsNullOrEmpty(Request["ID"]) ? Request["ID"] : string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is admin.
        /// </summary>
        /// <value><c>true</c> if this instance is admin; otherwise, <c>false</c>.</value>
        private bool IsAdmin
        {
            get
            {
                return AppSession.IsAdmin;
            }
        }

        #endregion Properties

        /// <summary>
        /// page load event.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetDialogMaxHeight("600");

            if (!IsPostBack)
            {
                CapIDModel4WS capID = new CapIDModel4WS
                {
                    id1 = Request["RecordID1"],
                    id2 = Request["RecordID2"],
                    id3 = Request["RecordID3"],
                    serviceProviderCode = Request[UrlConstant.AgencyCode]
                };

                // If Cap from AppSession is null, it may be get the cap from the URL parameters. If the CAP from AppSession not equal from URL, it also need use the value from URL.
                if (Cap == null
                    || Cap.capID == null
                    || (!string.IsNullOrEmpty(capID.id1)
                        && !string.IsNullOrEmpty(capID.id2)
                        && !string.IsNullOrEmpty(capID.id3)
                        && !string.IsNullOrEmpty(capID.serviceProviderCode)
                        && !Cap.capID.Equals(capID)))
                {
                    CapWithConditionModel4WS capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capID, AppSession.User.UserSeqNum, true);

                    if (capWithConditionModel != null)
                    {
                        SetInspection(capID.serviceProviderCode, capWithConditionModel.capModel);
                    }
                }
                else
                {
                    SetInspection(ConfigManager.AgencyCode, Cap);
                }
            }

            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey("aca_inspectiondetails_label_title|tip");
                SetPageTitleVisible(false);
            }
        }

        /// <summary>
        /// Set the inspection.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="capModel">The cap model.</param>
        private void SetInspection(string agencyCode, CapModel4WS capModel)
        {
            if (capModel == null)
            {
                return;
            }

            Inspection.ServiceProviderCode = agencyCode;
            Inspection.Cap = capModel;
            Inspection.CurrentUser = CurrentUser;
            Inspection.InspectionID = InspectionID;
            Inspection.IsAdmin = IsAdmin;
        }
    }
}
