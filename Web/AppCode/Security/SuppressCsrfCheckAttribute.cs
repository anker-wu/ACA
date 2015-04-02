#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: SuppressCsrfCheckAttribute.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description: Provide a custom attribute to suppress the CSRF check.
 *
 * </pre>
 */

#endregion

using System;

namespace Accela.ACA.Web.Security
{
    /// <summary>
    /// Provide a custom attribute to suppress the CSRF check.
    /// If there are some pages do not want to check the CSRF attacks,
    ///     please put this attribute at the class header.
    /// </summary>
    public class SuppressCsrfCheckAttribute : Attribute
    {
    }
}