#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ACAContext.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: ACAContext.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.SSOInterface.Model;
using Spring.Context;
using Spring.Context.Support;

namespace Accela.ACA.SSOInterface
{
    /// <summary>
    /// Delegate of ACA public user creation.
    /// </summary>
    /// <param name="args">Argument parameter.</param>
    /// <returns>Return Result Model.</returns>
    public delegate ReturnResultModel UserCreation(UserCreationEventArgs args);

    /// <summary>
    /// ACA context of SSO interface. Provides APIs allow SSO adapter integrate to ACA.
    /// </summary>
    public sealed class ACAContext
    {
        /// <summary>
        /// Spring context object.
        /// </summary>
        private static IApplicationContext _context;

        /// <summary>
        /// ACA context object.
        /// </summary>
        private static IACASSOContext _ssoACAContext;

        /// <summary>
        /// Gets the instance of ACA context.
        /// </summary>
        /// <value>ACA context instance.</value>
        public static IACASSOContext Instance 
        {
            get
            {
                if (_context == null)
                {
                    _context = ContextRegistry.GetContext();
                }

                if (_ssoACAContext == null)
                {
                    string objName = typeof(IACASSOContext).Name;
                    IACASSOContext obj = _context.GetObject(objName) as IACASSOContext;

                    if (obj == null)
                    {
                        throw new Exception("The object " + objName + " can't be found from object container.");
                    }

                    _ssoACAContext = obj;
                }

                return _ssoACAContext;
            }
        }

        /// <summary>
        /// Define the events of ACA public user operation allows SSO adapter to subscribe them and implement some special logic.
        /// </summary>
        public class Events
        {
            /// <summary>
            /// Gets or sets the event handler for pre public user account creation. The <see cref="PreUserCreation"/> event will be triggered before the public user account creating.
            /// </summary>
            public static UserCreation PreUserCreation { get; set; }

            /// <summary>
            /// Gets or sets the event handler for post public user account creation. The <see cref="PostUserCreation"/> event will be triggered after a public user account created.
            /// </summary>
            public static UserCreation PostUserCreation { get; set; }
        }
    }
}
