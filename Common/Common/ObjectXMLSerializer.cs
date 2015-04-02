#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ObjectXMLSerializer.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Object and XML serialization to create dummp data for admin preview feature.
 *
 *  Notes:
 * $Id: ObjectXMLSerializer.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.IO;
using System.Xml.Serialization;

namespace Accela.ACA.Common
{
    /// <summary>
    ///  Facade to XML serialization and deserialization of strongly typed objects to/from an XML file.
    /// </summary>
    /// <typeparam name="T">object data type.</typeparam>
    public static class ObjectXMLSerializer<T> where T : class
    {
        #region Methods

        /// <summary>
        /// Loads an object from an XML file in Document format.
        /// </summary>
        /// <param name="path">Path of the file to load the object from.</param>
        /// <returns>Object loaded from an XML file in Document format.</returns>
        public static T Load(string path)
        {
            T serializableObject = LoadFromDocumentFormat(path);
            return serializableObject;
        }

        /// <summary>
        /// Saves an object to an XML file in Document format.
        /// </summary>
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="path">Path of the file to save the object to.</param> 
        public static void Save(T serializableObject, string path)
        {
            SaveToDocumentFormat(serializableObject, path);
        }

        /// <summary>
        /// create text reader.
        /// </summary>
        /// <param name="path">path url information</param>
        /// <returns>text reader.</returns>
        private static TextReader CreateTextReader(string path)
        {
            TextReader textReader = null;
            textReader = new StreamReader(path);

            return textReader;
        }

        /// <summary>
        /// create text writer
        /// </summary>
        /// <param name="path">path URL info.</param>
        /// <returns>text writer information</returns>
        private static TextWriter CreateTextWriter(string path)
        {
            TextWriter textWriter = null;
            textWriter = new StreamWriter(path);

            return textWriter;
        }

        /// <summary>
        /// Create Xml serializer.
        /// </summary>
        /// <returns> Xml serializer.</returns>
        private static XmlSerializer CreateXmlSerializer()
        {
            Type objectType = typeof(T);
            XmlSerializer xmlSerializer = new XmlSerializer(objectType);

            return xmlSerializer;
        }

        /// <summary>
        /// Load from document format
        /// </summary>
        /// <param name="path">path URL info</param>
        /// <returns>serializable Object</returns>
        private static T LoadFromDocumentFormat(string path)
        {
            T serializableObject = null;

            using (TextReader textReader = CreateTextReader(path))
            {
                XmlSerializer xmlSerializer = CreateXmlSerializer();
                serializableObject = xmlSerializer.Deserialize(textReader) as T;
            }

            return serializableObject;
        }

        /// <summary>
        /// Save to document format
        /// </summary>
        /// <param name="serializableObject">serializable Object</param>
        /// <param name="path">path URL info</param>
        private static void SaveToDocumentFormat(T serializableObject, string path)
        {
            using (TextWriter textWriter = CreateTextWriter(path))
            {
                XmlSerializer xmlSerializer = CreateXmlSerializer();
                xmlSerializer.Serialize(textWriter, serializableObject);
                textWriter.Flush();
            }
        }

        #endregion Methods
    }
}