#region Header

/**
 *  Accela Citizen Access
 *  File: AfterReturningAdvise.cs
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
using System.Reflection;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using log4net;
using Spring.Aop;

namespace Accela.ACA.Common.Advise
{
    /// <summary>
    /// After Returning Advise.
    /// </summary>
    public class AfterReturningAdvise : IAfterReturningAdvice
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AfterReturningAdvise));

        #endregion Fields

        /// <summary>
        /// After Returning Advise.
        /// </summary>
        /// <param name="returnValue">The return value</param>
        /// <param name="method">The method info</param>
        /// <param name="args">The arguments of method</param>
        /// <param name="target">The target</param>
        public void AfterReturning(object returnValue, MethodInfo method, object[] args, object target)
        {
            try
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.DebugFormat("method name - {0}.{1}()", target, method.Name);

                    if (args != null)
                    {
                        Logger.DebugFormat("argument number - {0}", args.Length);

                        foreach (object arg in args)
                        {
                            Logger.DebugFormat("\t: {0}", arg);
                        }
                    }

                    if (returnValue != null && !(returnValue is string))
                    {
                        string xml = SerializationUtil.XmlSerialize(returnValue);
                        Logger.DebugFormat("return value: {0} - {1}", returnValue.ToString(), xml);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
