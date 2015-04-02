#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UploadAttachmentWS.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 *
 *  Description:
 *
 *  Notes:
 * $Id: UploadAttachmentWS.cs 131606 2009-05-20 09:39:17Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common.Config;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Plan
{
    #region Delegates

    /// <summary>
    /// Delegate for Upload Attachment Completed Event Handler.
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">UploadAttachmentCompletedEventArgs e</param>
    [System.CodeDom.Compiler.GeneratedCode("System.Web.Services", "2.0.50727.3053")]
    public delegate void UploadAttachmentCompletedEventHandler(object sender, UploadAttachmentCompletedEventArgs e);

    #endregion Delegates

    /// <summary>
    /// This class provide the ablity to operation upload attachment completed event args.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThrough]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class UploadAttachmentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        #region Fields

        /// <summary>
        /// object for result.
        /// </summary>
        private object[] results;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UploadAttachmentCompletedEventArgs class.
        /// </summary>
        /// <param name="results">object[] results</param>
        /// <param name="exception">exception message.</param>
        /// <param name="cancelled">Is cancelled.</param>
        /// <param name="userState">object for user state.</param>
        internal UploadAttachmentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets Result
        /// </summary>
        public UploadResult4WS Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return (UploadResult4WS) this.results[0];
            }
        }

        #endregion Properties
    }

    /// <summary>
    /// the class for Upload Attachment.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Web.Services.WebServiceBinding(Name = "UploadAttachmentSoap", Namespace = "http://microsoft.com/wse/samples/UploadAttachments")]
    internal class UploadAttachmentWS : Microsoft.Web.Services3.WebServicesClientProtocol
    {
        #region Fields
        /// <summary>
        /// Upload Attachment Operation Completed
        /// </summary>
        private System.Threading.SendOrPostCallback uploadAttachmentOperationCompleted;

        /// <summary>
        /// use Default Credentials Set Explicitly.
        /// </summary>
        private bool useDefaultCredentialsSetExplicitly;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UploadAttachmentWS class.
        /// </summary>
        public UploadAttachmentWS()
        {
            WebServiceParameter defaultParam = WebServiceConfig.GetDefaultConfigParameter();
            this.Url = defaultParam.Url + "/UploadWebService";
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Upload Attachment Completed.
        /// </summary>
        public event UploadAttachmentCompletedEventHandler UploadAttachmentCompleted;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether use default credentials.
        /// </summary>
        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }

            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Cancel Async
        /// </summary>
        /// <param name="userState">object for user state.</param>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        /// <summary>
        /// Upload Attachment.
        /// </summary>
        /// <param name="arg0">Upload Model for arg0</param>
        /// <returns>Upload Result Model</returns>
        [System.Web.Services.Protocols.SoapDocumentMethod("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/",
            Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElement("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UploadResult4WS UploadAttachment([System.Xml.Serialization.XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] UploadModel4WS arg0)
        {
            object[] results = this.Invoke("UploadAttachment", new object[] { arg0 });
            return (UploadResult4WS) results[0];
        }

        /// <summary>
        /// Upload Attachment Async.
        /// </summary>
        /// <param name="arg0">Upload Model for arg0</param>
        public void UploadAttachmentAsync(UploadModel4WS arg0)
        {
            this.UploadAttachmentAsync(arg0, null);
        }

        /// <summary>
        /// Upload Attachment Async
        /// </summary>
        /// <param name="arg0">Upload Model for arg0</param>
        /// <param name="userState">object for user state.</param>
        public void UploadAttachmentAsync(UploadModel4WS arg0, object userState)
        {
            if (this.uploadAttachmentOperationCompleted == null)
            {
                this.uploadAttachmentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadAttachmentOperationCompleted);
            }

            this.InvokeAsync("UploadAttachment", new object[] { arg0 }, this.uploadAttachmentOperationCompleted, userState);
        }

        /// <summary>
        /// Is Local File System WebService
        /// </summary>
        /// <param name="url">string for url.</param>
        /// <returns>true or false.</returns>
        private bool IsLocalFileSystemWebService(string url)
        {
            if ((url == null) || (url == string.Empty))
            {
                return false;
            }

            System.Uri wsUri = new System.Uri(url);

            if ((wsUri.Port >= 1024) && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// On Upload Attachment Operation Completed.
        /// </summary>
        /// <param name="arg">object for arg.</param>
        private void OnUploadAttachmentOperationCompleted(object arg)
        {
            if (this.UploadAttachmentCompleted != null)
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = (System.Web.Services.Protocols.InvokeCompletedEventArgs) arg;
                this.UploadAttachmentCompleted(this, new UploadAttachmentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        #endregion Methods
    }
}