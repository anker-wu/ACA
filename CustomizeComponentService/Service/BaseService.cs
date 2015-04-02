#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BaseService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: BaseService.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.CustomizeAPI;
using Accela.ACA.Web.Common;
using log4net;

namespace Accela.ACA.ComponentService.Service
{
    /// <summary>
    /// This class provide the basic function
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// Logger object.
        /// </summary>
        protected static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(BaseService));

        /// <summary>
        /// User Context
        /// </summary>
        private UserContext _context = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        internal BaseService(UserContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Gets User Sequence Number
        /// </summary>
        /// <returns></returns>
        public string UserSeqNum
        {
            get
            {
                string userSeqNum = string.Empty;

                if (this._context != null && this._context.CallerID.StartsWith(ACAConstant.PUBLIC_USER_NAME))
                {
                    userSeqNum = this._context.CallerID.Substring(ACAConstant.PUBLIC_USER_NAME.Length);
                }

                return userSeqNum;
            }
        }

        /// <summary>
        /// Gets Agency Code
        /// </summary>
        public string AgencyCode
        {
            get
            {
                return ConfigManager.AgencyCode;
            }
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        protected UserContext Context
        {
            get
            {
                return this._context;
            }

            set
            {
                this._context = value;
            }
        }

        /// <summary>
        /// connect the string with blank.
        /// if args[0] = "a",args[1]="b",args[2]="c", the result will be "a b c";
        /// if args[0] = "",args[1]="b",args[2]="c", the result will be "b c";
        /// </summary>
        /// <param name="args">string element</param>
        /// <param name="splitChar">The split char,such as a hyphen or a blank.</param>
        /// <returns>finally string value.</returns>
        public string ConcatStringWithSplitChar(IEnumerable<string> args, string splitChar)
        {
            if (args == null || !args.Any())
            {
                return string.Empty;
            }

            var list = args.Where(argument => !string.IsNullOrEmpty(argument)).ToList();

            if (list.Count == 0)
            {
                return string.Empty;
            }

            var formatString = new StringBuilder();

            for (var i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    formatString.Append(splitChar);
                }

                formatString.Append("{");
                formatString.Append(i.ToString());
                formatString.Append("}");
            }

            return string.Format(formatString.ToString(), list.ToArray());
        }
    }
}
