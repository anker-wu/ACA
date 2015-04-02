#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactTypeUIModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: ContactTypeUIModel.cs 130988 2009-9-18  16:26:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    public class ContactTypeUIModel
    {
        /// <summary>
        /// Gets or sets key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the checked status.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets or sets the min num
        /// </summary>
        public string MinNum { get; set; }

        /// <summary>
        /// Gets or sets the max num
        /// </summary>
        public string MaxNum { get; set; }

    }
}
