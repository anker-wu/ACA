#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UserMappingAttribute.cs
*
*  Accela, Inc.
*  Copyright (C): 2012
*
*  Description: Represents the Attribute configuration element in the AttributesMapping section within a configuration file.
*
*  Notes:
* $Id: UserMappingAttribute.cs 217467 2012-04-18 10:51:00Z ACHIEVO\alan.hu $.
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
    /// Represents the Attribute configuration element in the AttributesMapping section within a configuration file.
    /// </summary>
    public class UserMappingAttribute : ConfigurationElement
    {
        /// <summary>
        /// A constant string to defines the regular expression for valid attribute name.
        /// </summary>
        private const string AttributeNamePattern = @"^(Email|FirstName|MiddleName|LastName|Gender|Salutation|BirthDate|Address|Address2|City|State|Country|ZIP|MobilePhone|HomePhone|WorkPhone|Fax|FEIN|SSN|BusinessName|Title|P\.O\.Box)$";

        /// <summary>
        /// Gets the attribute name.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, DefaultValue = "Email")]
        [RegexStringValidator(AttributeNamePattern)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
        }

        /// <summary>
        /// Gets the property name defined in PublicUser model.
        /// </summary>
        [ConfigurationProperty("dbname", IsRequired = true)]
        public string UserModelPropertyName
        {
            get
            {
                return (string)this["dbname"];
            }
        }

        /// <summary>
        /// Gets the max length of attribute value to avoid database overflow during save value to database.
        /// </summary>
        [ConfigurationProperty("maxlength", IsRequired = true)]
        public int MaxLength
        {
            get
            {
                return (int)this["maxlength"];
            }
        }

        /// <summary>
        /// Gets the property name defined in the third party server.
        /// </summary>
        [ConfigurationProperty("extname")]
        public string ExternalName
        {
            get
            {
                return (string)this["extname"];
            }
        }
    }
}