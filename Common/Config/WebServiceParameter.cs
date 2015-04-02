/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WSFactory.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 *
 *  Description:
 *
 *  Notes:
 * $Id: WebServiceParameter.cs 131314 2009-05-19 06:07:41Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  08/15/2007           Jackie.Yu              Initial.
 * </pre>
 */
namespace Accela.ACA.Common.Config
{
    /// <summary>
    /// Define all parameter for invoking web service.
    /// </summary>
    public class WebServiceParameter
    {
        #region Fields

        /// <summary>
        /// Default web service time out.
        /// </summary>
        private const int DEFAULT_WEB_SERVICE_TIMEOUT = 100000;

        /// <summary>
        /// Web service id.
        /// </summary>
        private string _id = string.Empty;

        /// <summary>
        /// web service time out value.
        /// </summary>
        private int _timeout = DEFAULT_WEB_SERVICE_TIMEOUT;

        /// <summary>
        /// web service URL.
        /// </summary>
        private string _url = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets timeout for connecting to web service.
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeout;
            }

            set
            {
                _timeout = value;
            }
        }

        /// <summary>
        /// Gets or sets web service url.
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
        /// Gets or sets web service unique id.
        /// </summary>
        internal string ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clone the current object.
        /// </summary>
        /// <returns>new WebServiceParameter instance that be cloned.</returns>
        public WebServiceParameter Clone()
        {
            WebServiceParameter cloned = new WebServiceParameter();
            cloned.ID = this._id;
            cloned.Url = this._url;
            cloned.Timeout = this._timeout;
            return cloned;
        }

        #endregion Methods
    }
}