#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DocumentModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: DocumentModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    public partial class DocumentModel
    {
        /// <summary>
        /// Unique ID.
        /// </summary>
        public string FileId
        {
            get;

            set;
        }

        /// <summary>
        /// Upload status.
        /// </summary>
        public string FileState
        {
            get;

            set;
        }

        /// <summary>
        /// Specific entity type
        /// </summary>
        public string SpecificEntityType
        {
            get;

            set;
        }

        /// <summary>
        /// Also attach to
        /// </summary>
        public string AlsoAttachTo
        {
            get; 
            set; 
        }

        /// <summary>
        /// Gets or Sets the select from account template of document model.
        /// </summary>
        public TemplateModel SelectFromAccountTemplate
        {
            get; 

            set;
        }
    }
}
