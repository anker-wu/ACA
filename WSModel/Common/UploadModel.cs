/**
 *  Accela Citizen Access
 *  File: UploadModule.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2013
 * 
 *  Description:
 *   Provide Key-Value pair object.
 * 
 *  Notes:
 * $Id: UploadModel.cs 258040 2013-10-08 08:35:38Z ACHIEVO\alan.hu $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Attachment
{
    /// <summary>
    /// AttachmentModel is used to be persisted(xml file) to web server local.
    /// </summary>
    [System.SerializableAttribute()]
    public class AttachmentModel
    {
        private string _module;
        private DocumentModel _documentModel4WS;

        /// <summary>
        /// Parameterless construct.
        /// </summary>
        public AttachmentModel()
        {
            
        }
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="module">module name</param>
        /// <param name="documentModel4WS">documentModel4WS object.</param>
        public AttachmentModel(string module, DocumentModel documentModel4WS)
        {
            _module = module;
            _documentModel4WS = documentModel4WS;
        }

        /// <summary>
        /// Gets or sets the DocumentModel4WS object.
        /// </summary>
        public DocumentModel DocumentModel
        {
            get
            {
                return _documentModel4WS;
            }
            set
            {
                _documentModel4WS = value;
            }
        }


        /// <summary>
        /// Gets or sets the model name.
        /// </summary>
        public string ModelName
        {
            get
            {
                return _module;
            }
            set
            {
                _module = value;
            }
        }

        /// <summary>
        /// Gets or sets document category for distingush what function to upload document, DocumentUploadAfter event will use this value to handle some logic.
        /// </summary>
        public string Category4EMSE
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current cap's status.
        /// </summary>
        public bool IsPartialCap { get; set; }
    }
}
