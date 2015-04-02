#region Header

/**
 *  Accela Citizen Access
 *  File: AroundAdvise.cs
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
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Text;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using AopAlliance.Intercept;
using log4net;

namespace Accela.ACA.Common.Advise
{
    /// <summary>
    /// AroundAdvise class.
    /// </summary>
    public class AroundAdvise : IMethodInterceptor
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(string.Empty);

        /// <summary>
        /// index for the same tick value.
        /// </summary>
        private static int _index;

        #endregion Fields

        /// <summary>
        /// Gets Tick index .
        /// </summary>
        private static int Index
        {
            get
            {
                if (_index > 10000)
                {
                    _index = 0;
                }

                return _index++;
            }
        }

        /// <summary>
        /// Method Invoke
        /// </summary>
        /// <param name="invocation">Intercept.IMethodInvocation for invoking</param>
        /// <returns>return value.</returns>
        public object Invoke(IMethodInvocation invocation)
        {
            if (!Logger.IsDebugEnabled)
            {
                return invocation.Proceed();
            }
            else
            {
                long tick = DateTime.Now.Ticks + Index;
                StringBuilder sbArgs = new StringBuilder();

                try
                {
                    Logger.Debug(string.Format("*** Method BEGIN : {0}->{1}() [{2}] ***", invocation.Target, invocation.Method.Name, tick));

                    sbArgs.Append(string.Format("{0}->{1}()", invocation.Target, invocation.Method.Name));

                    if (invocation.Arguments != null)
                    {
                        sbArgs.Append("\r\n\tArguments  ");

                        foreach (object arg in invocation.Arguments)
                        {
                            sbArgs.Append(string.Format("\r\n\t\t\t: {0}", FormatOutput(arg)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Debug(sbArgs.ToString());
                    Logger.Error(ex);
                }

                object returnValue = null;

                Stopwatch watch = new Stopwatch();

                try
                {
                    watch.Start();

                    returnValue = invocation.Proceed();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw ex;
                }
                finally
                {
                    watch.Stop();
                }

                try
                {
                    StringBuilder sbResult = new StringBuilder();

                    if (watch.ElapsedMilliseconds >= ConfigManager.MethodExecuteTimeMin)
                    {
                        sbResult.Append(sbArgs.ToString());
                        sbResult.Append(string.Format("\r\n\tReturn Value    : {0}", FormatOutput(returnValue)));
                        sbResult.Append(string.Format("\r\n\tCosts(ms)       : {0} ms\r\n", watch.ElapsedMilliseconds)); 
                    }

                    sbResult.Append(string.Format("*** Method END   : {0}->{1}() [{2}],costs(ms): {3} ms ***", invocation.Target, invocation.Method.Name, tick, watch.ElapsedMilliseconds));
                    Logger.Debug(sbResult.ToString());
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }

                watch = null;
                return returnValue;
            }
        }

        /// <summary>
        /// Format value for log(complex object only log the collection count or row total).
        /// </summary>
        /// <param name="value">the value need to be formatted.</param>
        /// <returns>the format output.</returns>
        private string FormatOutput(object value)
        {
            if (value == null)
            {
                return "null";
            }

            int count = -1;

            if (value is Hashtable)
            {
                count = (value as Hashtable).Count;
            }
            else if (value is Array)
            {
                count = (value as Array).Length;
            }
            else if (value is IList)
            {
                count = (value as IList).Count;
            }
            else if (value is DataTable)
            {
                count = (value as DataTable).Rows.Count;
            }
            else if (value is IDictionary)
            {
                count = (value as IDictionary).Count;
            }

            string result = string.Empty;

            if (count != -1)
            {
                result = string.Format("{0} - Count = {1}", value.ToString(), count);
            }
            else
            {
                Type type = value.GetType();

                if (type.IsValueType || value is string)
                {
                    result = value.ToString();
                }
                else
                {
                    result = SerializationUtil.XmlSerialize(value);
                }
            }

            return result;
        }
    }
}
