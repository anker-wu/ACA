/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TemplateAttribute.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TemplateAttribute.cs 130107 2010-08-13 12:23:56Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy.Common
{
    /// <summary>
    /// Simple template attribute model for the attributes preservation in ACA Admin.
    /// </summary>
    public class TemplateAttribute
    {
        /// <summary>
        /// Initializes a new instance of the TemplateAttribute class.
        /// </summary>
        public TemplateAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TemplateAttribute class by APO/People template field.
        /// </summary>
        /// <param name="templateAttributeModel">APO/People template model.</param>
        public TemplateAttribute(TemplateAttributeModel templateAttributeModel)
        {
            this.EntityType = templateAttributeModel.entityType;
            this.TemplateCode = this.EntityType.ToString();
            this.TemplateType = templateAttributeModel.templateType;
            this.AttributeName = templateAttributeModel.attributeName;
            this.DefaultLanguageLabel = templateAttributeModel.defaultAttributeLabel;
        }

        /// <summary>
        /// Initializes a new instance of the TemplateAttribute class by generic template field.
        /// </summary>
        /// <param name="genericTemplateAttribute">Generic template model.</param>
        public TemplateAttribute(GenericTemplateAttribute genericTemplateAttribute)
        {
            //Only can customize the form-field(ASI fields) for generic template fields.
            this.EntityType = TemplateEntityType.ASI;
            this.TemplateCode = genericTemplateAttribute.groupName;
            this.TemplateType = genericTemplateAttribute.subgroupName;
            this.AttributeName = genericTemplateAttribute.fieldName;
            this.DefaultLanguageLabel = genericTemplateAttribute.fieldName;
        }

        /// <summary>
        /// Gets or sets template entity type.
        /// </summary>
        public TemplateEntityType EntityType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets template code.
        /// for APO/People template:APOTEMPLATE/PEOPLETEMPLATE
        /// for Generic template:[ASI group code]/[ASIT group code]
        /// </summary>
        public string TemplateCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets template field type.
        /// for Generic template:[ASI sub group code]/[ASIT sub group code]
        /// for APO template:ADDRESS/PARCEL/OWNER
        /// for LP/Contact(people template):[defined in standard choice]
        /// </summary>
        public string TemplateType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets template field name.
        /// </summary>
        public string AttributeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets template field label of default language.
        /// </summary>
        public string DefaultLanguageLabel
        {
            get;
            set;
        }
    }
}