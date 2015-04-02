#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: HttpServerHelper.cs
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

namespace Accela.AuthorizedAgent.Common.Common
{
    /// <summary>
    /// Http server helper
    /// </summary>
    public static class HttpServerHelper
    {
        /// <summary>
        /// Get action url from client soap request body.
        /// </summary>
        /// <param name="soapContent">soap request body.</param>
        /// <returns>A action Url.</returns>
        public static string GetActionUrlFromPostContent(string soapContent)
        {
            string actionBegin = "<action xsi:type=\"xsd:string\">";
            string actionEnd = "</action>";
            int beginPos = soapContent.IndexOf(actionBegin, StringComparison.InvariantCultureIgnoreCase);
            int endPos = soapContent.IndexOf(actionEnd, StringComparison.InvariantCultureIgnoreCase);

            string actionUrl = string.Empty;

            if (beginPos > 0 && endPos > 0)
            {
                actionUrl = soapContent.Substring(beginPos + actionBegin.Length, endPos - (beginPos + actionBegin.Length));

                string xmlSnippet = "<?xml version=\"1.0\" encoding=\"utf-8\"?><action>" + actionUrl + "</action>";

                //XML decode
                Stream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(xmlSnippet));
                XmlReader reader = XmlReader.Create(stream);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            actionUrl = reader.Value;
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            break;
                    }
                }
            }

            return actionUrl;
        }

        /// <summary>
        /// Queries the value by key.
        /// </summary>
        /// <param name="key">The key of query string.</param>
        /// <param name="url">The URL.</param>
        /// <returns>query string value</returns>
        public static string QueryValueByKey(string key, string url)
        {
            // e.g upload?file=D:/DocumentReviewer/Collab/test6/test6.pdf&name=renamefile.pdf
            string value = string.Empty;

            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            int startPos = url.IndexOf("?");

            if (startPos < 0)
            {
                startPos = 0;
            }

            string queryString = url.Substring(startPos);
            int keyPos = queryString.IndexOf(key + "=", StringComparison.InvariantCultureIgnoreCase);

            if (keyPos < 0)
            {
                return string.Empty;
            }

            int beginPos = keyPos + (key + "=").Length;
            int endPos = queryString.IndexOf("&", keyPos);

            if (endPos < 0)
            {
                endPos = queryString.Length;
            }

            value = queryString.Substring(beginPos, endPos - beginPos);
            value = HttpUtility.UrlDecode(value);
            return value;
        }
    }
}
