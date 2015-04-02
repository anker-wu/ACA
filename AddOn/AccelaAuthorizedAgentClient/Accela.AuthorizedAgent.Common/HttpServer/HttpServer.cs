#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: HttpServer.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
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
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Accela.AuthorizedAgent.Common.Common;

namespace Accela.AuthorizedAgent.Common.HttpServer
{
    /// <summary>
    /// The HTTP server
    /// </summary>
    public abstract class HttpServer
    {
        #region Fields

        /// <summary>
        /// The sequence number
        /// </summary>
        private static int _sequenceNumber = 0;

        /// <summary>
        /// The request status
        /// </summary>
        private static Hashtable _requestStatus = new Hashtable();

        /// <summary>
        /// The port
        /// </summary>
        private int _port;

        /// <summary>
        /// The listener
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// The is active
        /// </summary>
        private bool _isActive = true;

        /// <summary>
        /// The listen thread
        /// </summary>
        private Thread _listenThread;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        public HttpServer(int port)
        {
            this._port = port;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// show the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public delegate void ShowMessage(string message);

        /// <summary>
        /// Occurs when [on show message].
        /// </summary>
        public event ShowMessage onShowMessage;

        #endregion Events

        /// <summary>
        /// Listens this instance.
        /// </summary>
        public void Listen()
        {
            IPAddress localIP = Dns.GetHostAddresses(ConfigManager.IPAddress)[0];
            listener = new TcpListener(localIP, _port);
            listener.Start();
            ShowMessageEx("Listen on IP " + ConfigManager.IPAddress + " and port " + _port);

            _listenThread = new Thread(new ParameterizedThreadStart(ListenThread));
            _listenThread.IsBackground = true;
            _listenThread.Start(listener);

            Log.Instance.Write(this.GetType(), LogType.Info, "Starts to listen on port: " + _port + ", and the local IP address: " + localIP.ToString());
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            try
            {
                _isActive = false;
                listener.Stop();
                _listenThread.Abort();

                Log.Instance.Write(this.GetType(), LogType.Info, "Stops to listen on port: " + _port);
            }
            catch (Exception e)
            {
                Log.Instance.Write("httpserver", LogType.Error, e);
            }
        }

        /// <summary>
        /// Shows the message ex.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessageEx(string message)
        {
            if (onShowMessage != null)
            {
                onShowMessage(message);
            }
        }

        /// <summary>
        /// Generate unique number for each request.
        /// </summary>
        /// <returns>New sequence number</returns>
        public static int GetNextNumber()
        {
            int nextNumber = 0;

            lock (_requestStatus)
            {
                _sequenceNumber = _sequenceNumber + 1;
                nextNumber = _sequenceNumber;
            }

            return nextNumber;
        }

        /// <summary>
        /// Handles the GET request.
        /// </summary>
        /// <param name="p">The p.</param>
        public abstract void HandleGETRequest(HttpProcessor p);

        /// <summary>
        /// Handles the POST request.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="inputData">The input data.</param>
        public abstract void HandlePOSTRequest(HttpProcessor p, StreamReader inputData);

        /// <summary>
        /// Listens the thread.
        /// </summary>
        /// <param name="client">The client.</param>
        private void ListenThread(object client)
        {
            TcpListener listenerObj = (TcpListener)client;

            Log.Instance.Write(this.GetType(), LogType.Debug, "Enter into ListenThread");

            while (_isActive)
            {
               	TcpClient socket = null;

                try
                {
                    socket = listenerObj.AcceptTcpClient();
                    HttpProcessor processor = new HttpProcessor(socket, this);
                    Thread thread = new Thread(new ThreadStart(processor.ProcessRequest));
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception e)
                {
                    if (_isActive)
                    {
                        Log.Instance.Write(this.GetType(), LogType.Error,
                            "Authorized Agent Client encounters some problem, please restart it manually to continue your work. The detailed error information: " + e);
                        ShowMessageEx("Authorized Agent Client encounters some problem, please restart it manually to continue your work.");
                    }
					
                    break;
                }
            }

            listenerObj.Stop();
            Log.Instance.Write(this.GetType(), LogType.Debug, "Exit ListenThread");
        }
    }
}
