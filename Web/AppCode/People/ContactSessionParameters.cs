#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactSessionParameter.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description: Provide the session model for contact.
 *
 *  Notes:
 *      $Id: ContactSessionParameter.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.ExpressionBuild;

namespace Accela.ACA.Web.People
{
    /// <summary>
    /// Contact session parameter.
    /// </summary>
    [Serializable]
    public class ContactSessionParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactSessionParameter"/> class.
        /// </summary>
        public ContactSessionParameter()
        {
            PageFlowComponent = new PageFlowComponentParameters();
            this.Process = new ProcessParameters();
            this.Data = new DataParameters();
        }

        /// <summary>
        /// Gets or sets the page flow component parameter.
        /// </summary>
        /// <value>
        /// The page flow component parameter.
        /// </value>
        public PageFlowComponentParameters PageFlowComponent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the process parameters.
        /// </summary>
        /// <value>
        /// The process parameters.
        /// </value>
        public ProcessParameters Process
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data parameters.
        /// </summary>
        /// <value>
        /// The data parameters.
        /// </value>
        public DataParameters Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the contact.
        /// </summary>
        /// <value>
        /// The type of the contact.
        /// </value>
        public string ContactType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression type.
        /// </summary>
        /// <value>
        /// The type of the expression.
        /// </value>
        public ExpressionType ContactExpressionType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact section position.
        /// </summary>
        /// <value>
        /// The contact section position.
        /// </value>
        public ACAConstant.ContactSectionPosition ContactSectionPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show the view in contact form].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show detail]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowDetail
        {
            get;
            set;
        }
    }
}