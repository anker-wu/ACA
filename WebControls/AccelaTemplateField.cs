#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaTemplateField.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *  Provide a template field with cusomized properties.
 *
 *  Notes:
 * $Id: AccelaTemplateField.cs 194212 2011-03-31 03:01:38Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using Accela.ACA.Common;

namespace Accela.Web.Controls
{
    /// <summary>
    /// ENUM ExportFormats
    /// </summary>
    public enum ExportFormats
    {
        /// <summary>
        /// The none
        /// </summary>
        None,

        /// <summary>
        /// The short date
        /// </summary>
        ShortDate,

        /// <summary>
        /// The only time
        /// </summary>
        OnlyTime,

        /// <summary>
        /// The SSN
        /// </summary>
        SSN,

        /// <summary>
        /// The fein
        /// </summary>
        FEIN,

        /// <summary>
        /// The filter script
        /// </summary>
        FilterScript
    }

    /// <summary>
    /// Provide a template field with customized properties.
    /// </summary>
    public class AccelaTemplateField : System.Web.UI.WebControls.TemplateField
    {
        #region Fields

        /// <summary>
        /// indicating whether it is standard or not.
        /// </summary>
        private bool _isStandard = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the column id.
        /// </summary>
        public string ColumnId
        {
            get;
 
            set;
        }

        /// <summary>
        /// Gets or sets the attribute name.
        /// </summary>
        public string AttributeName
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the template attribute name list, they have the same display label, so need merge to a template field.
        /// </summary>
        public IList<string> MergedAttributeNames
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is standard or not.
        /// </summary>
        /// <value>Return true if it is standard; otherwise, false.</value>
        public bool IsStandard
        {
            get 
            {
                return _isStandard;
            }

            set
            {
                _isStandard = value;
            }
        }

        /// <summary>
        /// Gets or sets the data type that bind to it.
        /// </summary>
        public ControlType BindDataType
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the export field.
        /// </summary>
        /// <value>The export field.</value>
        public string ExportDataField
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the export format.
        /// </summary>
        /// <value>The export format.</value>
        public ExportFormats ExportFormat
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is a hidden field that for export.
        /// </summary>
        public bool HideField4Export 
        {
            get; 

            set; 
        }

        /// <summary>
        /// Gets or sets a value indicating whether should hidden current field.
        /// </summary>
        public bool HideField
        { 
            get; 
            
            set; 
        }

        #endregion Properties
    }
}
