#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Error.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description: A web page to display the unhandled errors.
 * </pre>
 */
#endregion

using System;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Common;
using log4net;

namespace Accela.ACA.Web
{
    /// <summary>
    /// A web page to display the unhandled errors.
    /// </summary>
    public partial class Error : BasePage
    {
        /// <summary>
        /// Log instance.
        /// </summary>
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(Global));

        /// <summary>
        /// Gets error message from current context.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                string cacheKey = Request.QueryString["ErrorId"];
                string errMessage = cacheManager.GetSingleCachedItem(cacheKey) as string;

                if (string.IsNullOrEmpty(errMessage))
                {
                    errMessage = WebConstant.ExceptionUtilDefaultValue;
                }

                errMessage = ScriptFilter.AntiXssHtmlEncode(errMessage);

                return errMessage;
            }
        }

        /// <summary>
        /// record url, in error page clear the last page.
        /// </summary>
        protected override void RecordUrl()
        {
            Session[ACAConstant.CURRENT_URL] = null;
        }

        /// <summary>
        /// On load event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnLoad(EventArgs e)
        {
            systemErrorMessage.ShowWithText(MessageType.Error, ErrorMessage, MessageSeperationType.Both);

            if (!IsPostBack)
            {
                string link = string.Format("<a href=\"javascript:window.location.href='Welcome.aspx';\" class=\"ACA_Row\">{0}</a>", LabelUtil.GetGlobalTextByKey("aca_error_goback_link"));
                lblGoBack.Text = string.Format(LabelUtil.GetGlobalTextByKey("aca_error_goback_message"), link);
            }
        }
    }
}
