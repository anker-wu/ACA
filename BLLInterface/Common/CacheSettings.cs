#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CacheSettings.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 *
 *  Description:
 *
 *  Notes:
 * $Id: CacheSettings.cs 131464 2009-05-20 01:42:02Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Cache setting model,which is used to store cache parameter but not cache content.
    /// </summary>
    [Serializable]
    public class CacheSettings
    {
        #region Fields

        /// <summary>
        /// variable field _description
        /// </summary>
        private string _description;

        /// <summary>
        /// variable field _expireTime. 
        /// </summary>
        private int _expireTime;

        /// <summary>
        /// variable field _key. 
        /// </summary>
        private string _key;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CacheSettings class.
        /// </summary>
        public CacheSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the CacheSettings class.overload, instance a CacheSettings object.
        /// </summary>
        /// <param name="key">cache key.</param>
        /// <param name="description">cache item description, which is used to describe what is cached.</param>
        /// <param name="expirationTime">expiration time(minutes)</param>
        public CacheSettings(string key, string description, int expirationTime)
        {
            _key = key;
            _expireTime = expirationTime;
            _description = description;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the cache description.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Gets or sets the expiration time(seconds).
        /// </summary>
        public int ExpireTime
        {
            get
            {
                return _expireTime;
            }

            set
            {
                _expireTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the cache key.
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

        #endregion Properties
    }
}