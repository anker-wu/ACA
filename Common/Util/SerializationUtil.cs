#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SerializationUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: SerializationUtil.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// An utility to serialize/deserialize object
    /// </summary>
    public static class SerializationUtil
    {
        #region Methods

        /// <summary>
        /// Deserialize an xml string to special type object.
        /// </summary>
        /// <param name="xml">xml string to be deserialized.</param>
        /// <param name="type">Object type to be deserialized.</param>
        /// <returns>deserialized object.</returns>
        public static object XmlDeserialize(string xml, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                return serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Serialize an Serializable object to xml string.
        /// </summary>
        /// <param name="obj">Serializable object.</param>
        /// <returns>serialized xml string.</returns>
        public static string XmlSerialize(object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            if (obj is Hashtable)
            {
                return "Hashtable cannot be serialized.";
            }

            if (obj is DataRow)
            {
                return "System.Data.DataRow cannot be serialized.";
            }

            Type type = obj.GetType();

            if (type.IsValueType || obj is string)
            {
                return obj.ToString();
            }

            XmlSerializer serializer = new XmlSerializer(type);

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                stream.Close();

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Serialize an Serializable object to specific file.
        /// </summary>
        /// <param name="obj">Serializable object.</param>
        /// <param name="fileName">file name with full path.</param>
        public static void XmlSerializeToFile(object obj, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            FileStream fs = new FileStream(fileName, FileMode.Create);
            TextWriter writer = new StreamWriter(fs, new UTF8Encoding());

            // Serialize using the XmlTextWriter.
            serializer.Serialize(writer, obj);
            writer.Close();
        }

        /// <summary>
        /// Deserialize an xml file to special type object.
        /// </summary>
        /// <param name="type">Object type to be deserialized.</param>
        /// <param name="fileName">file name with full path.</param>
        /// <returns>deserialized object.</returns>
        public static object XmlDeserializeFromFile(Type type, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(type);

            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);

            object obj = serializer.Deserialize(reader);
            fs.Close();

            return obj;
        }

        /// <summary>
        /// Convert Stream To Bytes
        /// </summary>
        /// <param name="stream">Stream Object</param>
        /// <returns>Bytes string</returns>
        public static byte[] ConvertStreamToBytes(Stream stream)
        {
             byte[] bytes = null;
             
             if (stream != null && stream.CanRead && stream.CanSeek)
             {
                 bytes = new byte[stream.Length];
                 stream.Read(bytes, 0, bytes.Length);
                 stream.Seek(0, SeekOrigin.Begin);
             }

             return bytes; 
        }

        /// <summary>
        /// convert object to json
        /// </summary>
        /// <param name="obj">class instance</param>
        /// <returns>json string</returns>
        public static string ToJSON(object obj)
        {
            string result = null;
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());

            using (MemoryStream ms = new MemoryStream())
            {
                json.WriteObject(ms, obj);
                ms.Position = 0;

                using (StreamReader reader = new StreamReader(ms))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        #endregion Methods
    }
}