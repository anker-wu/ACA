/**
 *  Accela Citizen Access
 *  File: ParameterNode.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: ParameterNode.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System;
using System.Xml;
using System.Xml.Serialization;

namespace Accela.ACA.Payment.Xml
{
    /// <summary>
    /// the parameter node
    /// </summary>
    public sealed class ParameterNode
    {
        /// <summary>
        /// The original parameter key
        /// </summary>
        [XmlAttribute("ACA")]
        public string ACA;

        /// <summary>
        /// The target parameter key
        /// </summary>
        [XmlAttribute("ThirdParty")]
        public string ThirdParty;

        /// <summary>
        /// the key of the parameter
        /// </summary>
        [XmlAttribute("MappingName")]
        public string MappingName;

        /// <summary>
        /// Gets or sets the parameter value
        /// </summary>
        public string ParamValue
        {
            get;
            set;
        }
    }
}
