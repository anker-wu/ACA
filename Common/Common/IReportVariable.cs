/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: IReportVariable.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 *  Defined an interface for ReportVariable.
 *  Notes:
 *      $Id: IReportVariable.cs 121852 2009-05-08 07:09:47Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.Common
{
    /// <summary>
    /// Defined an interface for report variable page.
    /// Cap alt ids only can be got from page, so page class should implement this interface.
    /// Use this interface to get cap alt ids or other necessary variables. 
    /// </summary>
    public interface IReportVariable
    {
        /// <summary>
        /// Gets cap alt ids
        /// </summary>
        string[] CapAltIDs
        {
            get;
        }
    }
}