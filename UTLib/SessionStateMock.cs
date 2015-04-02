/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SessionStateMock.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 *  SessionState mock object
 *  
 *  Notes:
 * $Id: SessionStateMock.cs 122597 2010-03-05 07:29:43Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */


using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Web.Hosting;
using System.Reflection;

namespace Accela.Test.Lib
{
    /// <summary>
    /// A SessionState mock object of IHttpSessionState to implement a custom session-state container.
    /// </summary>
    public sealed class SessionStateMock : IHttpSessionState
    {
        const int MAX_TIMEOUT = 24 * 60;  // Timeout cannot exceed 24 hours.

        string pId;
        ISessionStateItemCollection pSessionItems;
        HttpStaticObjectsCollection pStaticObjects;
        int pTimeout;
        bool pNewSession;
        HttpCookieMode pCookieMode;
        SessionStateMode pMode;
        bool pAbandon;
        bool pIsReadonly;

        public SessionStateMock(string id,
                              ISessionStateItemCollection sessionItems,
                              HttpStaticObjectsCollection staticObjects,
                              int timeout,
                              bool newSession,
                              HttpCookieMode cookieMode,
                              SessionStateMode mode,
                              bool isReadonly)
        {
            pId = id;
            pSessionItems = sessionItems;
            pStaticObjects = staticObjects;
            pTimeout = timeout;
            pNewSession = newSession;
            pCookieMode = cookieMode;
            pMode = mode;
            pIsReadonly = isReadonly;
        }

        /// <summary>
        /// Gets and sets the time-out period (in minutes) allowed between requests before the session-state provider terminates the session.
        /// </summary>
        public int Timeout
        {
            get { return pTimeout; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Timeout value must be greater than zero.");

                if (value > MAX_TIMEOUT)
                    throw new ArgumentException("Timout cannot be greater than " + MAX_TIMEOUT.ToString());
                pTimeout = value;
            }
        }

        /// <summary>
        /// Gets the unique session identifier for the session.
        /// </summary>
        public string SessionID
        {
            get { return pId; }
        }

        /// <summary>
        /// Gets a value indicating whether the session was created with the current request.
        /// </summary>
        public bool IsNewSession
        {
            get { return pNewSession; }
        }

        /// <summary>
        /// Gets the current session-state mode.
        /// </summary>
        public SessionStateMode Mode
        {
            get { return pMode; }
        }

        /// <summary>
        /// Gets a value indicating whether the session ID is embedded in the URL or stored in an HTTP cookie.
        /// </summary>
        public bool IsCookieless
        {
            get { return CookieMode == HttpCookieMode.UseUri; }
        }

        /// <summary>
        /// Gets a value that indicates whether the application is configured for cookieless sessions.
        /// </summary>
        public HttpCookieMode CookieMode
        {
            get { return pCookieMode; }
        }
        // Abandon marks the session as abandoned. The IsAbandoned property is used by the
        // session state module to perform the abandon work during the ReleaseRequestState event.
        public void Abandon()
        {
            pAbandon = true;
        }

        /// <summary>
        /// Ends the current session.
        /// </summary>
        public bool IsAbandoned
        {
            get { return pAbandon; }
        }

        /// <summary>
        /// Gets or sets the locale identifier (LCID) of the current session.
        /// </summary>
        public int LCID
        {
            get { return Thread.CurrentThread.CurrentCulture.LCID; }
            set { Thread.CurrentThread.CurrentCulture = CultureInfo.ReadOnly(new CultureInfo(value)); }
        }

        /// <summary>
        /// Gets or sets the code-page identifier for the current session.
        /// Session.CodePage exists only to support legacy ASP compatibility. ASP.NET developers should use
        /// Response.ContentEncoding instead.
        /// </summary>
        public int CodePage
        {
            get
            {
                if (HttpContext.Current != null)
                    return HttpContext.Current.Response.ContentEncoding.CodePage;
                else
                    return Encoding.Default.CodePage;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding(value);
            }
        }

        /// <summary>
        /// Gets a collection of objects declared by <object Runat="Server" Scope="Session"/> tags within the ASP.NET application file Global.asax.
        /// </summary>
        public HttpStaticObjectsCollection StaticObjects
        {
            get { return pStaticObjects; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object this[string name]
        {
            get { return pSessionItems[name]; }
            set { pSessionItems[name] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object this[int index]
        {
            get { return pSessionItems[index]; }
            set { pSessionItems[index] = value; }
        }

        /// <summary>
        /// Adds a new item to the session-state collection.
        /// </summary>
        public void Add(string name, object value)
        {
            pSessionItems[name] = value;
        }

        /// <summary>
        /// Deletes an item from the session-state item collection.
        /// </summary>
        public void Remove(string name)
        {
            pSessionItems.Remove(name);
        }

        /// <summary>
        /// Deletes an item at a specified index from the session-state item collection.
        /// </summary>
        public void RemoveAt(int index)
        {
            pSessionItems.RemoveAt(index);
        }

        /// <summary>
        /// Clears all values from the session-state item collection.
        /// </summary>
        public void Clear()
        {
            pSessionItems.Clear();
        }

        /// <summary>
        /// Clears all values from the session-state item collection.
        /// </summary>
        public void RemoveAll()
        {
            Clear();
        }

        /// <summary>
        /// Gets the number of items in the session-state item collection.
        /// </summary>
        public int Count
        {
            get { return pSessionItems.Count; }
        }

        /// <summary>
        /// Gets a collection of the keys for all values stored in the session-state item collection.
        /// </summary>
        public NameObjectCollectionBase.KeysCollection Keys
        {
            get { return pSessionItems.Keys; }
        }

        /// <summary>
        /// Returns an enumerator that can be used to read all the session-state item values in the current session.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return pSessionItems.GetEnumerator();
        }

        /// <summary>
        /// Copies the collection of session-state item values to a one-dimensional array, starting at the specified index in the array.
        /// </summary>
        public void CopyTo(Array items, int index)
        {
            foreach (object o in items)
                items.SetValue(o, index++);
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection of session-state values.
        /// </summary>
        public object SyncRoot
        {
            get { return this; }
        }

        /// <summary>
        /// Gets a value indicating whether the session is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return pIsReadonly; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the collection of session-state values is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }
    }
}
