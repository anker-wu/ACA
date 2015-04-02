#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MyRecordsCapView4Ui.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: MyRecordsCapView4Ui.cs 183096 2014-8-11 03:00:43Z ACHIEVO\Awen-deng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Headerusing System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accela.ACA.Web.WebApi.Entity.Adapter
{
    /// <summary>
    /// My record cap view for UI data bind
    /// </summary>
    [Serializable]
    public class MyRecordsCapView4Ui : CustomCapView4Ui
    {
        /// <summary>
        /// Gets or sets agency zip code.
        /// </summary>
        public string AgencyStateZip { get; set; }

        /// <summary>
        /// Gets or sets english trade name.
        /// </summary>
        public string EnglishTradeName { get; set; }

        /// <summary>
        /// Gets or sets cap index.
        /// </summary>
        public string CapIndex { get; set; }
 
        /// <summary>
        /// Gets or sets record expiration date.
        /// </summary>
        public string ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the record created by some one.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show record or not.
        /// </summary>
        public bool IsShowCopyRecord { get; set; }

        /// <summary>
        /// Gets or sets audit date.
        /// </summary>
        public string AuditDate { get; set; }
    }
}