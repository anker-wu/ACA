#region Header

/**
 *  Accela Citizen Access
 *  File: ThrowsAdvise.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: CommonEventArgs.cs 131314 2009-05-19 06:07:41Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using Accela.ACA.Common.Log;
using log4net;
using Spring.Aop;

namespace Accela.ACA.Common.Advise
{
    /// <summary>
    /// Throws Advise
    /// </summary>
    public class ThrowsAdvise : IThrowsAdvice 
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ThrowsAdvise));

        #endregion Fields

        /// <summary>
        /// After Throwing
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void AfterThrowing(Exception ex)
        {
            Logger.Error(ex);
        }
    }
}
