#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SerializationUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description: 
 *
 *  Notes:
 *      
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Accela.AuthorizedAgent.Common.Setting;

namespace Accela.AuthorizedAgent.Common.Common
{
    /// <summary>
    /// An utils to serialize/deseriablize object
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
                object obj = serializer.Deserialize(stream);
                stream.Flush();
                stream.Close();
                return obj;
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
                return null;
            }

            Type type = obj.GetType();

            if (type.IsValueType || obj is string)
            {
                return obj.ToString();
            }

            XmlSerializer serializer = new XmlSerializer(type);

            string content = string.Empty;

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                content = Encoding.UTF8.GetString(stream.ToArray());
                stream.Flush();
                stream.Close();
            }

            return content;
        }

        /// <summary>
        /// Serialize an Serializable object to specific file.
        /// </summary>
        /// <param name="obj">Serializable object.</param>
        /// <param name="fileName">file name with full path.</param>
        public static void XmlSerializeToFile(object obj, string fileName)
        {
            if (obj != null)
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                FileStream fs = null;
                TextWriter writer = null;
                using (fs = new FileStream(fileName, FileMode.Create))
                {
                    using (writer = new StreamWriter(fs, new UTF8Encoding()))
                    {
                        // Serialize using the XmlTextWriter.
                        serializer.Serialize(writer, obj);
                        writer.Flush();
                        writer.Close();
                    }

                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Deserialize an xml file to special type object.
        /// </summary>
        /// <param name="type">Object type to be deserialized.</param>
        /// <param name="fileName">file name with full path.</param>
        /// <returns>deserialized object.</returns>
        public static object XmlDeserializeFromFile(Type type, string fileName)
        {
            if (File.Exists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                FileStream fs = null;
                XmlReader reader = null;
                object obj = null;

                using (fs = new FileStream(fileName, FileMode.Open))
                {
                    using (reader = XmlReader.Create(fs))
                    {
                        // Serialize using the XmlTextWriter.
                        obj = serializer.Deserialize(reader);
                        reader.Close();
                    }

                    fs.Close();
                }

                return obj;
            }

            return null;
        }

        /// <summary>
        /// load xml to ProxyServerSetting, if the xml doesn't exist or xml format is invalidate then generate the xml.
        /// </summary>
        /// <returns>
        /// http proxy setting
        /// </returns>
        public static ProxyServerSetting ConfigureProxySetting()
        {
            string settingPath = AppDomain.CurrentDomain.BaseDirectory + "AccelaDocProxySetting.xml";
            ProxyServerSetting setting = null;

            if (File.Exists(settingPath))
            {
                try
                {
                    setting = (ProxyServerSetting)XmlDeserializeFromFile(typeof(ProxyServerSetting), settingPath);
                }
                catch
                {
                    Log.Instance.Write("ProxyServerSetting", LogType.Error, "No AccelaDocProxySetting.xml  in the path or parse the xml error then generate a default configuratioin of the proxy setting.");
                    setting = GenDefaultProxyServerSetting();
                    XmlSerializeToFile(setting, settingPath);
                }
            }
            else
            {
                setting = GenDefaultProxyServerSetting();
                XmlSerializeToFile(setting, settingPath);
            }

            return setting;
        }

        /// <summary>
        /// Gens the default proxy server setting.
        /// </summary>
        /// <returns>
        /// default proxy server Setting
        /// </returns>
        private static ProxyServerSetting GenDefaultProxyServerSetting()
        {
            ProxyServerSetting setting = new ProxyServerSetting();
            setting.IsUsingProxy = false;
            setting.ServerIP = string.Empty;
            setting.Port = string.Empty;
            setting.UserName = string.Empty;
            setting.Password = string.Empty;
            setting.Domain = string.Empty;
            setting.IsByPassLocalAddr = false;
            setting.IsNeedAuthorized = false;
            return setting;
        }

        #endregion Methods
    }
}
