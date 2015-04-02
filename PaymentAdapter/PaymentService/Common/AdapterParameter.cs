/**
 *  Accela Citizen Access
 *  File: Adapter.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: Adapter.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Accela.ACA.Payment.Xml
{
    /// <summary>
    /// the adapter node
    /// </summary>
    public sealed class AdapterParameter
    {
        /// <summary>
        /// the adapter name
        /// </summary>
        [XmlAttribute("AdapterName")]
        public string AdapterName;

        /// <summary>
        /// The parameters list group
        /// </summary>
        [XmlElement("Parameter", typeof(ParameterNode))]
        public List<ParameterNode> Parameters = new List<ParameterNode>();
    }
}
