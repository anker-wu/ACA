/**
 *  Accela Citizen Access
 *  File: ParamMapping.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: ParamMapping.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System.Xml;
using System.Xml.Serialization;

namespace Accela.ACA.Payment.Xml
{
    /// <summary>
    /// the parameter mapping class
    /// </summary>
    [XmlRoot("ParamMapping")]
    public sealed class ParamMapping
    {
        /// <summary>
        /// The parameters group
        /// </summary>
        [XmlElement("Adapter", typeof(AdapterParameter))]
        public AdapterParameter Adapter = new AdapterParameter();
    }
}
