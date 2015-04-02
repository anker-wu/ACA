#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Constant.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description: 
 *
 *  Notes:
 *      
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.AuthorizedAgent.Common.Common
{
    /// <summary>
    /// Constant class
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// The accela error header
        /// </summary>
        public static string AccelaErrorHeader = "AccelaError";

        /// <summary>
        /// Http connect timeout (ms)
        /// </summary>
        public static int CONNECT_TIMEOUT = 100 * 60 * 1000; //100 mins;

        /// <summary>
        /// action key for get printer list
        /// </summary>
        public static string ACTION_GET_PRINTER_LIST = "PrinterList";

        /// <summary>
        /// action key for print report
        /// </summary>
        public static string ACTION_PRINT_REPORT = "PrinterReport";
    }
}
