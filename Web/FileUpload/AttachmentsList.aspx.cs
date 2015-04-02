#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AttachmentsList.aspx.cs 278433 2014-09-03 11:43:10Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Xml;

using Accela.ACA.BLL.Attachment;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.Web.Attachment
{
    /// <summary>
    /// show attachment list
    /// </summary>
    public partial class AttachmentsList : BasePageWithoutMaster
    {
        #region Fields

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AttachmentsList));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Get module name according to whether current page is license detail page.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override string GetModuleName(HttpRequest request)
        {
            if (ValidationUtil.IsTrue(Request.QueryString["isLicenseeDetailPage"]))
            {
                return string.Empty;
            }

            return base.GetModuleName(Request);
        }

        #endregion
        #region Methods

        /// <summary>
        /// get pending attachment count
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <returns>pending attachment.</returns>
        [WebMethod(Description = "GetPendingAttachments", EnableSession = true)]
        public static int GetPendingAttachments(string moduleName)
        {
            int pendingAttachmentCount = 0;

            try
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
                string dir = Path.Combine(ConfigurationManager.AppSettings["TempDirectory"], "TempFiles");

                if (Directory.Exists(dir))
                {
                    string[] files = Directory.GetFiles(dir);

                    foreach (string file in files)
                    {
                        XmlDocument dom = new XmlDocument();
                        dom.Load(file);
                        XmlNode node = dom.SelectSingleNode("AttachmentModel/DocumentModel/capID");

                        if (node != null && node.ChildNodes.Count > 4 && node.ChildNodes[4].InnerText == CapUtil.GetAgencyCode(moduleName) && node.ChildNodes[0].InnerText == capModel.capID.customID)
                        {
                            pendingAttachmentCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return pendingAttachmentCount + GetPendingAttachmentsFromDB(moduleName);
        }

        /// <summary>
        /// Raises the pre initial event
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnPreInit(EventArgs e)
        {
            // change the grid view number before OnInit, because the grid view number will be used in AccelaGridView control's OnInit event to set the PageSize property.
            // control's OnInit early than component's OnInit early than page's OnInit
            attachmentList.ChangeGridViewNumber();

            base.OnPreInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ValidationUtil.IsYes(Request[UrlConstant.IS_FOR_CONDITION_DOCUMENT]))
                {
                    attachmentList.DisplayAttachment();
                }
                else
                {
                    /*
                     * There may be multiple attachment lists on the same page. A list is embedded in an iframe.
                     * When trying to refresh the content of iframe, we need to know which attachment list should be updated.
                     * This is what the component name does.
                     */
                    string iframeId = Request[UrlConstant.IFRAME_ID];
                    string componentName = AttachmentUtil.ExtractComponentNameFromClientID(iframeId);

                    attachmentList.DisplayAttachment(componentName);    
                }
            }
        }

        /// <summary>
        /// Handle the LoadComplete event.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnLoadComplete(EventArgs e)
        {
            hfValue.Value = attachmentList.PendingAttachmentCount + "|" + ModuleName;
            base.OnLoadComplete(e);
        }

        /// <summary>
        /// get pending attachment count from DB
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <returns>pending attachment count.</returns>
        private static int GetPendingAttachmentsFromDB(string moduleName)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            if (capModel == null || !(capModel.capClass == ACAConstant.COMPLETED || string.IsNullOrEmpty(capModel.capClass)))
            {
                return 0;
            }

            DocumentModel[] tempDocList = null;

            try
            {
                string serviceProviderCode = ConfigManager.AgencyCode;
                IEDMSDocumentBll edmsDocBll = (IEDMSDocumentBll)ObjectFactory.GetObject(typeof(IEDMSDocumentBll));
                CapIDModel capID = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);
                tempDocList = edmsDocBll.GetRecordDocumentList(serviceProviderCode, moduleName, AppSession.User.PublicUserId, capID, false);
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
            }

            int count = 0;

            if (tempDocList != null && tempDocList.Length > 0)
            {
                foreach (DocumentModel doc in tempDocList)
                {
                    if (doc.entityType == DocumentEntityType.TMP_CAP)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        #endregion Methods
    }
}