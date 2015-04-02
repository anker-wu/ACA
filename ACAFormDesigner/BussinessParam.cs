/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: BussinessParam.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Accela.ACA.FormDesigner
{
    /// <summary>
    /// the class for Bussiness parameter.
    /// </summary>
    public class BussinessParam
    {
        /// <summary>
        /// Gets or sets ServProvCode
        /// </summary>
        public string ServProvCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets LevelType
        /// </summary>
        public string LevelType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets LevelName
        /// </summary>
        public string LevelName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets CallerId
        /// </summary>
        public string CallerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ViewId
        /// </summary>
        public string ViewId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets permissionLevel
        /// </summary>
        public string PermissionLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets permissionValue
        /// </summary>
        public string PermissionValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Service url
        /// </summary>
        public string ServiceUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets CountryCode
        /// </summary>
        public string CountryCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets LangCode
        /// </summary>
        public string LangCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets SaveErrorMessage
        /// </summary>
        public string SaveErrorMessge
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets LoadingErrorMessage
        /// </summary>
        public string LoadingErrorMessge
        {
            get;
            set;
        }
    }
}
