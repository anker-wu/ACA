#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ASITUIField.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2013
*
*  Description: A derived class from UIField for ASIT.
*
*  Notes:
* $Id: ASITUIField.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Jul 6, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;

using Accela.ACA.WSProxy;

namespace Accela.ACA.UI.Model
{
    /// <summary>
    /// A derived class from UIField for ASIT
    /// </summary>
    [Serializable]
    public class ASITUIField : UIField
    {
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        public string DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the display length.
        /// </summary>
        public int DisplayLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the field is drill-down field.
        /// </summary>
        public bool IsDrillDown
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the field is hidden by expression.
        /// </summary>
        public bool IsHiddenByExp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the required flag for fee calculation.
        /// </summary>
        public string RequiredFeeCalc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ASIT column security value.
        /// </summary>
        public string SecurityValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the seq num for generic template table field.
        /// </summary>
        public long? GenericTemplateSeqNum
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value list for ASIT drop-down field.
        /// </summary>
        public IEnumerable<RefAppSpecInfoDropDownModel4WS> ValueList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the field is readonly by expression.
        /// </summary>
        public bool IsReadOnlyByExp
        {
            get;
            set;
        }
    }
}