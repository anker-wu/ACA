/**
 * <pre>
 *
 *  Accela
 *  File: ExportParameter.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ExportParameter.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/10/2007    daly.zeng    Initial.
 * </pre>
 */

namespace Accela.Web.Controls
{
    /// <summary>
    /// The export parameter.
    /// </summary>
    public class ExportParameter
    {
        /// <summary>
        /// The header text in the download file.
        /// </summary>
        private string _header = string.Empty;

        /// <summary>
        /// The export field belong to the data source
        /// </summary>
        private string _exportDataField = string.Empty;

        /// <summary>
        /// The export field's format
        /// </summary>
        private ExportFormats _exportFormat = ExportFormats.None;

        /// <summary>
        /// Gets or sets the export field belong to the data source
        /// </summary>
        /// <value>The export data field.</value>
        public string ExportDataField
        {
            get
            {
                return _exportDataField;
            }

            set
            {
                _exportDataField = value;
            }
        }

        /// <summary>
        /// Gets or sets the export field's format
        /// </summary>
        public ExportFormats ExportFormat
        {
            get
            {
                return _exportFormat;
            }

            set
            {
                _exportFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the header text in the download file.
        /// </summary>
        public string Header
        {
            get
            {
                return _header;
            }

            set
            {
                _header = value;
            }
        }
    }
}
