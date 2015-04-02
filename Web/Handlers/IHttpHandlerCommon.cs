#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: IHttpHandlerCommon.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: Http handler common behavior.
*
*  Notes:
* $Id: FileUploadHandler.ashx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// http handler validate interface
    /// </summary>
    public interface IHttpHandlerCommon
    {
        /// <summary>
        /// Gets or sets the config section.
        /// </summary>
        /// <value>The config section.</value>
        HttpHandlerConfigObject ConfigObject { get; set; }

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="type">The handler type.</param>
        /// <returns>validation parameters successful</returns>
        bool ValidateParams(ServiceType type);
    }
}
