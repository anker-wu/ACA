#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DocumentTypeUIModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: DocumentTypeUIModel.cs 12ACC-00881 2013-9-18  16:26:01Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.UI.Model
{
    /// <summary>
    /// Document type UI Model. It use in ACA admin data transmission when save PageFlow. 
    /// </summary>
    public class DocumentTypeUIModel
    {
        /// <summary>
        /// Gets or sets CapTypeKey
        /// </summary>
        public string CapTypeKey { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// DocumentTypes: document list configuration, it is a DocumentTypeOptionModel serialized.
        /// </summary>
        public string DocumentTypes { get; set; }
    }

    /// <summary>
    /// Document type option model. It use in ACA Admin Page Flow configuration
    /// </summary>
    public class DocumentTypeOptionModel
    {
        /// <summary>
        /// Gets or sets the DocumentType. 
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the ResDocumentType. 
        /// </summary>
        public string ResDocumentType { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the document type is display in ACA daily side.
        /// </summary>
        public bool Checked { get; set; }
    }
}