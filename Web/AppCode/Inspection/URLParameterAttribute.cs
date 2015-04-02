#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: URLParameterAttribute.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010
 *
 *  Description:
 *
 *  Notes:
 *      $Id: URLParameterAttribute.cs 182945 2010-10-22 08:22:49Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// URL Parameter Attribute
    /// </summary>
    public class URLParameterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="URLParameterAttribute"/> class.
        /// </summary>
        /// <param name="key">The attribute key.</param>
        public URLParameterAttribute(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The attribute key.</value>
        public string Key
        {
            get;
            set;
        }
    }
}
