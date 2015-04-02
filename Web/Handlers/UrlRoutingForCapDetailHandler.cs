#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UrlRoutingForCapDetailHandler.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: Url routing http handler.
*
*  Notes:
* $Id: UrlRoutingForCapDetailHandler.ashx.cs 171222 2014-03-25 16:10:00Z ACHIEVO\eric.he $.
*  Revision History
*  Date,            Who,        What
*  March 25, 2014   Eric.He     Initial.
* </pre>
*/

#endregion

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// routing url path
    /// </summary>
    public class UrlRoutingForCapDetailHandler : UrlRoutingHandler
    {
        #region IHttpHandler Members

        /// <summary>
        ///  Need validate(CapId1, CapId2, CapId3,ModuleName)
        /// </summary>
        /// <returns>Pass return true. Else redirect error page</returns>
        protected override bool IsParamsValidated()
        {
            return ValidateParams(CapId1, CapId2, CapId3, ModuleName);
        }

        #endregion
    }
}
