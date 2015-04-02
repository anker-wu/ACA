#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: HttpUtility.cs
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

using System.Net;
using System.Text;
using Accela.AuthorizedAgent.Common.Setting;

namespace Accela.AuthorizedAgent.Common.Common
{
    /// <summary>
    /// Http Utility
    /// </summary>
    public static class HttpUtility
    {
        /// <summary>
        /// URLs the decode.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string UrlDecode(string str)
        {
            if (str == null)
            {
                return null;
            }
            return UrlDecode(str, Encoding.UTF8);
        }

        /// <summary>
        /// URLs the decode.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public static string UrlDecode(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            return UrlDecodeStringFromStringInternal(str, e);
        }

        /// <summary>
        /// URLs the decode string from string internal.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
        {
            int bufferSize = s.Length;
            UrlDecoder decoder = new UrlDecoder(bufferSize, e);
            for (int i = 0; i < bufferSize; i++)
            {
                char ch = s[i];
                if ((ch == '%') && (i < (bufferSize - 2)))
                {
                    if ((s[i + 1] == 'u') && (i < (bufferSize - 5)))
                    {
                        int num3 = HexToInt(s[i + 2]);
                        int num4 = HexToInt(s[i + 3]);
                        int num5 = HexToInt(s[i + 4]);
                        int num6 = HexToInt(s[i + 5]);
                        if (((num3 < 0) || (num4 < 0)) || ((num5 < 0) || (num6 < 0)))
                        {
                            if ((ch & 0xff80) == 0)
                            {
                                decoder.AddByte((byte)ch);
                            }
                            else
                            {
                                decoder.AddChar(ch);
                            }
                            continue;
                        }
                        ch = (char)((((num3 << 12) | (num4 << 8)) | (num5 << 4)) | num6);
                        i += 5;
                        decoder.AddChar(ch);
                        continue;
                    }
                    int num7 = HexToInt(s[i + 1]);
                    int num8 = HexToInt(s[i + 2]);
                    if ((num7 >= 0) && (num8 >= 0))
                    {
                        byte b = (byte)((num7 << 4) | num8);
                        i += 2;
                        decoder.AddByte(b);
                        continue;
                    }
                }
                if ((ch & 0xff80) == 0)
                {
                    decoder.AddByte((byte)ch);
                }
                else
                {
                    decoder.AddChar(ch);
                }

            }
            return decoder.GetString();
        }

        /// <summary>
        /// 
        /// </summary>
        private class UrlDecoder
        {
            /// <summary>
            /// The _buffer size
            /// </summary>
            private int _bufferSize;

            private byte[] _byteBuffer;
            /// <summary>
            /// The _char buffer
            /// </summary>
            private char[] _charBuffer;

            /// <summary>
            /// The _encoding
            /// </summary>
            private Encoding _encoding;

            /// <summary>
            /// The _num bytes
            /// </summary>
            private int _numBytes;

            /// <summary>
            /// The _num chars
            /// </summary>
            private int _numChars;

            /// <summary>
            /// Initializes a new instance of the <see cref="UrlDecoder"/> class.
            /// </summary>
            /// <param name="bufferSize">Size of the buffer.</param>
            /// <param name="encoding">The encoding.</param>
            internal UrlDecoder(int bufferSize, Encoding encoding)
            {
                this._bufferSize = bufferSize;
                this._encoding = encoding;
                this._charBuffer = new char[bufferSize];
            }

            /// <summary>
            /// Adds the byte.
            /// </summary>
            /// <param name="b">The b.</param>
            internal void AddByte(byte b)
            {
                if (this._byteBuffer == null)
                {
                    this._byteBuffer = new byte[this._bufferSize];
                }
                this._byteBuffer[this._numBytes++] = b;
            }

            /// <summary>
            /// Adds the char.
            /// </summary>
            /// <param name="ch">The ch.</param>
            internal void AddChar(char ch)
            {
                if (this._numBytes > 0)
                {
                    this.FlushBytes();
                }
                this._charBuffer[this._numChars++] = ch;
            }

            /// <summary>
            /// Flushes the bytes.
            /// </summary>
            private void FlushBytes()
            {
                if (this._numBytes > 0)
                {
                    this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
                    this._numBytes = 0;
                }
            }

            /// <summary>
            /// Gets the string.
            /// </summary>
            /// <returns></returns>
            internal string GetString()
            {
                if (this._numBytes > 0)
                {
                    this.FlushBytes();
                }
                if (this._numChars > 0)
                {
                    return new string(this._charBuffer, 0, this._numChars);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Hexes to int.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <returns></returns>
        private static int HexToInt(char h)
        {
            if ((h >= '0') && (h <= '9'))
            {
                return (h - '0');
            }
            if ((h >= 'a') && (h <= 'f'))
            {
                return ((h - 'a') + 10);
            }
            if ((h >= 'A') && (h <= 'F'))
            {
                return ((h - 'A') + 10);
            }
            return -1;
        }

        /// <summary>
        /// add proxy if user configure the proxy server setting
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="request"></param>
        public static void AddProxy(ProxyServerSetting setting, HttpWebRequest request)
        {
            if (setting.IsUsingProxy)
            {
                WebProxy proxy = new WebProxy("http://" + setting.ServerIP + ":" + setting.Port, setting.IsByPassLocalAddr);
                NetworkCredential networkCredential = new NetworkCredential();
                if (setting.IsNeedAuthorized)
                {
                    if (!string.IsNullOrEmpty(setting.UserName))
                    {
                        networkCredential.UserName = setting.UserName;
                        networkCredential.Password = setting.Password;
                    }
                    if (!string.IsNullOrEmpty(setting.Domain))
                    {
                        networkCredential.Domain = setting.Domain;
                    }
                }
                request.Proxy = proxy;
                request.Credentials = networkCredential;
                request.Proxy.Credentials = networkCredential;
            }
        }
    }
}
