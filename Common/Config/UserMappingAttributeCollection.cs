#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UserMappingAttributeCollection.cs
*
*  Accela, Inc.
*  Copyright (C): 2012
*
*  Description: Represents the AttributesMapping configuration element containing a collection of the Attribute elements.
*
*  Notes:
* $Id: UserMappingAttributeCollection.cs 217467 2012-04-18 14:46:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  April 18, 2012   Alan Hu      Initial.
* </pre>
*/

#endregion

using System.Configuration;

namespace Accela.ACA.Common.Config
{
    /// <summary>
    /// Represents the AttributesMapping configuration element containing a collection of the Attribute elements.
    /// </summary>
    public class UserMappingAttributeCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// A constant string to defines the child element name.
        /// </summary>
        private const string ChindElementName = "Attribute";

        /// <summary>
        /// Gets the type of the System.Configuration.ConfigurationElementCollection.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        /// <summary>
        /// Gets a value indicating whether an attempt to add a duplicate ConfigurationElement
        ///     to the ConfigurationElementCollection will cause an exception to be thrown.
        /// Note that elements with identical keys and values are not considered duplicates,
        ///     and are accepted silently. Only elements with identical keys but different values are considered duplicates.
        /// </summary>
        protected override bool ThrowOnDuplicate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the name used to identify this collection of elements in the configuration file.
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return ChindElementName;
            }
        }

        /// <summary>
        /// Override the base class, creates a new UserMappingAttribute configuration element.
        /// </summary>
        /// <returns>A new UserMappingAttribute object.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new UserMappingAttribute();
        }

        /// <summary>
        /// Gets the element key for Attribute configuration element.
        /// </summary>
        /// <param name="element">The System.Configuration.ConfigurationElement to return the key.</param>
        /// <returns>An System.Object that acts as the key for the specified System.Configuration.ConfigurationElement.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UserMappingAttribute)element).Name;
        }
    }
}
