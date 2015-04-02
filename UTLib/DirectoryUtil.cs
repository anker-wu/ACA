#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DirectoryUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010
 *
 *  Description:
 *  DirectoryUtil for UT
 *
 *  Notes:
 * $Id: SerializationUtil.cs 179604 2010-08-24 01:00:45Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;

namespace Accela.Test.Lib
{
    /// <summary>
    /// DirectoryUtil for UT
    /// </summary>
    public class DirectoryUtil
    {
        /// <summary>
        /// Gets UT cases root directory
        /// </summary>
        public static string CasesRoot
        {
            get
            {
                return System.Environment.CurrentDirectory + "\\" + "UTCases";
            }
        }
    }
}
