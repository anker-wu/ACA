#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: HttpProcessor.cs
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
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading;

using Accela.AuthorizedAgent.Common.Common;

namespace Accela.AuthorizedAgent.Common.HttpServer
{
    /// <summary>
    /// Http Processor
    /// </summary>
    public class HttpProcessor
    {
        #region Fields

        /// <summary>
        /// The buffer size
        /// </summary>
        private const int BUFFER_SIZE = 4096;

        /// <summary>
        /// The HTTP headers
        /// </summary>
        private Hashtable _httpHeaders = new Hashtable();

        /// <summary>
        /// The input stream
        /// </summary>
        private Stream _inputStream;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpProcessor"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="srv">The SRV.</param>
        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            Socket = s;
            Socket.SendTimeout = Constant.CONNECT_TIMEOUT;
            Socket.ReceiveTimeout = Constant.CONNECT_TIMEOUT;
            HttpServer = srv;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the socket
        /// </summary>
        public TcpClient Socket { get; set; }

        /// <summary>
        /// Gets or sets the HTTP server
        /// </summary>
        public HttpServer HttpServer { get; set; }

        /// <summary>
        /// Gets or sets the output stream
        /// </summary>
        public StreamWriter OutputStream { get; set; }

        /// <summary>
        /// Gets or sets the http method
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the http url
        /// </summary>
        public string HttpUrl { get; set; }

        /// <summary>
        /// Gets or sets the http protocol version string
        /// </summary>
        public string HttpProtocolVersionstring { get; set; }

        #endregion Properties
        
        /// <summary>
        /// Processes the request.
        /// </summary>
        public void ProcessRequest()
        {
            try
            {
                // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
                // "processed" view of the world, and we want the data raw after the headers
                _inputStream = new BufferedStream(Socket.GetStream());

                // we probably shouldn't be using a streamwriter for all output from handlers either
                OutputStream = new StreamWriter(new BufferedStream(Socket.GetStream()));

                ParseRequest();
                ReadHeaders();

                if (HttpMethod.Equals("GET"))
                {
                    HandleGETRequest();
                }
                else if (HttpMethod.Equals("POST"))
                {
                    HandlePOSTRequest();
                }
            }
            catch (Exception e)
            {
                Log.Instance.Write("HttpProcessor", LogType.Error, e);
                Console.WriteLine("Exception: " + e.ToString());
            }
            finally
            {
                try
                {
                    if (OutputStream != null)
                    {
                        OutputStream.Flush();
                    }

                    _inputStream = null;
                    OutputStream = null;

                    if (Socket != null)
                    {
                        Socket.Close();
                    }
                }
                catch (Exception e)
                {
                    Log.Instance.Write("HttpProcessor", LogType.Error, e);
                }
            }
        }

        /// <summary>
        /// Parses the request.
        /// </summary>
        /// <exception cref="System.Exception">invalid http request line</exception>
        public void ParseRequest()
        {
            string request = StreamReadLine(_inputStream);
            string[] tokens = request.Split(' ');

            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }

            HttpMethod = tokens[0].ToUpper();
            HttpUrl = tokens[1];
            HttpProtocolVersionstring = tokens[2];
        }

        /// <summary>
        /// Reads the headers.
        /// </summary>
        /// <exception cref="System.Exception">invalid http header line:  + line</exception>
        public void ReadHeaders()
        {
            string line;
            while ((line = StreamReadLine(_inputStream)) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }

                string name = line.Substring(0, separator);
                int pos = separator + 1;

                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                Console.WriteLine("header: {0}:{1}", name, value);
                _httpHeaders[name] = value;
            }
        }

        /// <summary>
        /// Handles the GET request.
        /// </summary>
        public void HandleGETRequest()
        {
            HttpServer.HandleGETRequest(this);
        }

        /// <summary>
        /// Handles the POST request.
        /// </summary>
        public void HandlePOSTRequest()
        {
            /* this post data processing just reads everything into a memory stream.
             * this is fine for smallish things, but for large stuff we should really
             * hand an input stream to the request processor. However, the input stream 
             * we hand him needs to let him see the "end of the stream" at this content 
             * length, because otherwise he won't know when he's seen it all! 
             */
            int content_len = 0;
            MemoryStream ms = new MemoryStream();

            if (this._httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this._httpHeaders["Content-Length"]);

                byte[] buf = new byte[BUFFER_SIZE];
                int to_read = content_len;

                while (to_read > 0)
                {
                    int numread = _inputStream.Read(buf, 0, Math.Min(BUFFER_SIZE, to_read));

                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("client disconnected during post");
                        }
                    }

                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }

                ms.Seek(0, SeekOrigin.Begin);
            }

            try
            {
                HttpServer.HandlePOSTRequest(this, new StreamReader(ms));
            }
            catch (Exception e)
            {
                Log.Instance.Write("HttpProcessor", LogType.Error, e);
            }
            finally
            {
                try
                {
                    if (OutputStream != null)
                    {
                        OutputStream.Flush();
                    }

                    _inputStream = null;
                    OutputStream = null;  

                    if (Socket != null)
                    {
                        Socket.Close();
                    }
                }
                catch (Exception e)
                {
                    Log.Instance.Write("HttpProcessor", LogType.Error, e);
                }
            }
        }

        /// <summary>
        /// Streams the read line.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>return the string.</returns>
        private string StreamReadLine(Stream inputStream)
        {
            string data = string.Empty;

            while (true)
            {
                int next_char = inputStream.ReadByte();

                if (next_char == '\n')
                {
                    break;
                }

                if (next_char == '\r')
                {
                    continue;
                }

                if (next_char == -1)
                {
                    Thread.Sleep(1);
                    continue;
                }

                data += Convert.ToChar(next_char);
            }

            return data;
        }
    }
}
