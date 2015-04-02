/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ObjectCloneUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: ObjectCloneUtil.cs 199677 2011-07-18 09:31:31Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// provide a class to clone object.
    /// </summary>
    public static class ObjectCloneUtil
    {
        /// <summary>
        /// Clones the simple object.
        /// </summary>
        /// <param name="cloneObject">The clone object.</param>
        /// <param name="newObject">The new object.</param>
        /// <returns>cloned object</returns>
        public static object CloneSimpleObject(object cloneObject, object newObject)
        {
            Type type = cloneObject.GetType();
            PropertyInfo[] pis = type.GetProperties();

            foreach (PropertyInfo pi in pis)
            {
                if (pi.CanWrite && pi.CanRead)
                {
                    pi.SetValue(newObject, pi.GetValue(cloneObject, null), null);
                }
            }

            return newObject;
        }

        /// <summary>
        /// Create deep copy object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="obj">Source object.</param>
        /// <returns>Copy of object.</returns>
        public static T DeepCopy<T>(T obj)
        {
            if (obj == null)
            {
                return default(T);
            }

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                T result = (T)formatter.Deserialize(stream);
                stream.Close();
                return result;
            }
        }
    }
}
