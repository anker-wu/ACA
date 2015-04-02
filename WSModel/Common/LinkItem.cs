/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: LinkItem.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: LinkItem.cs 239452 2012-12-12 05:56:43Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  10/18/2007    gopal.narra    Initial.
 * </pre>
 */
using System;
using System.Collections.Generic;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Contains Information for the Tab
    /// </summary>
    public class LinkItem : ILinkItem
    {
        private int _order;
        private string _label;
        private string _key;
        private string _url;
        private string _roles = "1"; //1- anonymous user
        private string _module;
        private bool _forceLogin = false;//default no need to Force Login.
        private bool _singleServiceOnly = false;//default allow select multiple service.
        private bool _needFilter = false;//default no need to have record type filter settings.

        /// <summary>
        /// Gets or sets tab order or link item order in the same group.
        /// </summary>
        public int Order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
            }
        }

        /// <summary>
        /// Gets or stes the label/text to be showed to user.
        /// </summary>
        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        /// <summary>
        /// Gets or sets the item key according to standard choice key.
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        /// <summary>
        /// Gets or set the final url.
        /// </summary>
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        /// <summary>
        /// Gets or sets the all of roles that the current link can be showed.
        /// the value like below: 0 or 0|1 or 1, 0 - citizen user, 1-anonymous user.
        /// </summary>
        public string Roles
        {
            get
            {
                return _roles;
            }
            set
            {
                _roles = value;
            }
        }

        /// <summary>
        /// Indicates the current link whether can be used by anonymous user.
        /// </summary>
        /// <returns>true-anonymous is in roles. false-anonymous user is not in roles</returns>
        public bool IsAnonymousInRoles
        {
            get
            {
                // 0-register user,1-anonymous user
                if (_roles.IndexOf("1", StringComparison.InvariantCulture) > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Indicates the current link whether can be used by register user.
        /// </summary>
        /// <returns>true-register user is in roles. false-register user is not in roles</returns>
        public bool IsRegisterInRoles
        {
            get
            {
                // 0-register user,1-anonymous user
                if (_roles.IndexOf("0", StringComparison.InvariantCulture) > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current link module name. 
        /// </summary>
        public string Module
        {
            get
            {
                return _module;
            }
            set
            {
                _module = value;
            }
        }

        /// <summary>
        /// gets or sets whether allow multiple service selection.
        /// </summary>
        public bool SingleServiceOnly
        {
            get { return _singleServiceOnly; }
            set { _singleServiceOnly = value; }
        }

        /// <summary>
        /// Gets or sets the current link force login.
        /// </summary>
        public bool ForceLogin
        {
            get
            {
                return _forceLogin;
            }
            set
            {
                _forceLogin = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the current link need record type filter settings or not.
        /// </summary>
        public bool NeedFilter
        {
            get
            {
                return _needFilter;
            }
            set
            {
                _needFilter = value;
            }
        }

    }

    /// <summary>
    /// LinkItem comparer.
    /// </summary>
    public sealed class LinkItemComparer : IComparer<LinkItem>
    {
        #region IComparer<TabInfo> Members

        /// <summary>
        /// TabLinkItem Compare method
        /// </summary>
        int IComparer<LinkItem>.Compare(LinkItem x, LinkItem y)
        {
            int result = 0;

            // Tab Orders can never be null, there are either a number or Int.Max
            if (x.Order < y.Order)
            {
                result = -1;
            }
            else if (x.Order == y.Order)
            {
                result = 0;
            }
            else if (x.Order > y.Order)
            {
                result = 1;
            }

            return result;
        }

        #endregion
    }
}
