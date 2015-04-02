#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TemplateBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: TemplateBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.APO
{
    /// <summary>
    /// Provide the ability to get APO/People template attributes.
    /// Save the template attribute values feature is put to cap business.
    /// </summary>
    public class TemplateBll : BaseBll, ITemplateBll
    {
        #region Fields

        /// <summary>
        /// template genus APO.
        /// </summary>
        private const string TEMPLATE_GENUS_APO = "APO";

        /// <summary>
        /// template genus people.
        /// </summary>
        private const string TEMPLATE_GENUS_PEOPLE = "People";

        /// <summary>
        /// drop down list
        /// </summary>
        private const string DROPDOWNLIST = "DropdownList";

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(TemplateBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of TemplateService.
        /// </summary>
        private TemplateWebServiceService TemplateService
        {
            get
            {
                return WSFactory.Instance.GetWebService<TemplateWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets APO template attributes by template type.
        /// This method only returns template attribute but not includes the detail values.
        /// When creating a cap/APO data,you need to present template field without value.
        /// this method will be invoked to return all attribute fields for dynamically creating fields/controls.
        /// </summary>
        /// <param name="templateType">One value of TemplateType enumeration includes CAP_ADDRESS,CAP_PARCEL,CAP_OWNER.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1.Pass "APO" constant to templateGenus parameter that TemplateWebService.getAttributes required.
        /// 2.Pass templateType.ToString() to templateType parameter of TemplateWebService.getAttributes method.
        /// 3.Pass null to template Reference Number parameter that TemplateWebService.getAttributes required.
        /// 4.Invokes TemplateWebService.getAttributes method to return;</remarks>
        public TemplateAttributeModel[] GetAPOTemplateAttributes(TemplateType templateType, string agencyCode, string callerID)
        {
            if (string.IsNullOrEmpty(agencyCode))
            {
                return null;
            }

            try
            {
                return GetTemplateFromCache(agencyCode, TEMPLATE_GENUS_APO, templateType.ToString());
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets daily APO template attributes with value by template type,Cap Id and daily APO number.
        /// This method returns not only template attributes but also the detail values related to detail a cap.
        /// </summary>
        /// <param name="templateType">One value of TemplateType enumeration includes CAP_ADDRESS,CAP_PARCEL,CAP_OWNER.</param>
        /// <param name="capIDModel">cap id model.</param>
        /// <param name="dailyAPONumber">daily APO number.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1.Pass "APO" constant to templateGenus parameter that TemplateWebService.getEditAttributes required.
        /// 2.Pass templateType.ToString() to templateType parameter of TemplateWebService.getEditAttributes method.
        /// 3.Invokes TemplateWebService.getEditAttributes method to return;</remarks>
        public TemplateAttributeModel[] GetDailyAPOTemplateAttributes(TemplateType templateType, CapIDModel4WS capIDModel, string dailyAPONumber, string agencyCode, string callerID)
        {
            if (capIDModel == null || string.IsNullOrEmpty(dailyAPONumber))
            {
                return null;
            }

            try
            {
                string servProvCode = capIDModel.serviceProviderCode;
                TemplateAttributeModel[] templateAttributes = TemplateService.getEditAttributes(servProvCode, TEMPLATE_GENUS_APO, templateType.ToString(), capIDModel, dailyAPONumber, callerID);

                if (templateAttributes != null && templateAttributes.Length > 0)
                {
                    foreach (TemplateAttributeModel item in templateAttributes)
                    {
                        if (string.Equals(item.attributeValueDataType, DROPDOWNLIST))
                        {
                            TemplateAttributeModel[] templateAttrs = GetTemplateFromCache(agencyCode, TEMPLATE_GENUS_APO, templateType.ToString());

                            SetTemplateSelectOption4SuperAgency(templateAttrs, item);
                        }
                    }
                }

                return templateAttributes;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets daily People template attributes with value by template type,Cap Id and daily APO number.
        /// This method returns not only template attributes but also the detail values related to detail contact or license.
        /// </summary>
        /// <param name="templateType">template type may be one of below values:
        /// 1.one of standard choice items in 'CONTACT TYPE' category;
        /// 2.one of standard choice items in 'LICENSED PROFESSIONAL TYPE' category;</param>
        /// <param name="capIDModel">cap id model.</param>
        /// <param name="dailyPeopleNumber">daily contact number or license sequence number.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1.Pass "People" constant to templateGenus parameter that TemplateWebService.getEditAttributes required.
        /// 2.Invokes TemplateWebService.getEditAttributes method to return;</remarks>
        public TemplateAttributeModel[] GetDailyPeopleTemplateAttributes(string templateType, CapIDModel4WS capIDModel, string dailyPeopleNumber, string agencyCode, string callerID)
        {
            if (capIDModel == null || string.IsNullOrEmpty(dailyPeopleNumber))
            {
                return null;
            }

            try
            {
                string servProvCode = capIDModel.serviceProviderCode;
                TemplateAttributeModel[] temlateAttributes = TemplateService.getEditAttributes(servProvCode, TEMPLATE_GENUS_PEOPLE, templateType, capIDModel, dailyPeopleNumber, callerID);

                if (temlateAttributes != null && temlateAttributes.Length > 0)
                {
                    CapIDModel capId = TempModelConvert.Trim4WSOfCapIDModel(capIDModel);

                    foreach (TemplateAttributeModel item in temlateAttributes)
                    {
                        /*
                         * If the template field is merged from reference, the Cap ID will not be set to field.
                         * So need to set the capID attribute.
                         */
                        if (item.capID == null)
                        {
                            item.capID = capId;
                        }

                        if (string.Equals(item.attributeValueDataType, DROPDOWNLIST))
                        {
                            TemplateAttributeModel[] templateAttrs = GetPeopleTemplateAttributes(templateType, agencyCode, callerID);

                            SetTemplateSelectOption4SuperAgency(templateAttrs, item);
                        }
                    }
                }

                return temlateAttributes;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Set Dropdown List Item for template field
        /// </summary>
        /// <param name="item">template item</param>
        /// <param name="type">APO or People</param>
        public void FillTemplateSelectOption(TemplateAttributeModel item, string type)
        {
            if (DROPDOWNLIST.Equals(item.attributeValueDataType) && item.selectOptions == null)
            {
                if (TEMPLATE_GENUS_APO.Equals(type) || TEMPLATE_GENUS_PEOPLE.Equals(type))
                {
                    TemplateAttributeModel[] templateAttrs = GetTemplateFromCache(item.serviceProviderCode, type, item.templateType);

                    SetTemplateSelectOption4SuperAgency(templateAttrs, item);
                }
            }
        }

        /// <summary>
        /// Gets People template attributes by template type.
        /// This method only returns template attributes but not includes the detail values.
        /// When creating people data,you need to present template field without value.
        /// this method will be invoked to return all attributes fields for dynamically creating fields/controls.
        /// </summary>
        /// <param name="templateType">template type may be one of below values:
        /// 1.one of standard choice items in 'CONTACT TYPE' category;
        /// 2.one of standard choice items in 'LICENSED PROFESSIONAL TYPE' category;</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1.Pass "People" constant to templateGenus parameter that TemplateWebService.getAttributes required.
        /// 2.Pass null to template Reference Number parameter that TemplateWebService.getAttributes required.
        /// 3.Invokes TemplateWebService.getAttributes method to return;</remarks>
        public TemplateAttributeModel[] GetPeopleTemplateAttributes(string templateType, string agencyCode, string callerID)
        {
            if (string.IsNullOrEmpty(templateType) || string.IsNullOrEmpty(agencyCode))
            {
                return null;
            }

            try
            {
                TemplateAttributeModel[] attributes = GetTemplateFromCache(agencyCode, TEMPLATE_GENUS_PEOPLE, templateType);
                return ObjectCloneUtil.DeepCopy(attributes);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets reference APO template attributes with value by template type and reference APO number.
        /// This method returns not only template attributes but also the detail values for reference.
        /// When it is referenced,the field name should be matched to fill to daily field.
        /// </summary>
        /// <param name="templateType">One value of TemplateType enumeration includes CAP_ADDRESS,CAP_PARCEL,CAP_OWNER.</param>
        /// <param name="refAPONumber">reference APO sequence number.it should be long type parameter.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1.Pass "APO" constant to templateGenus parameter that TemplateWebService.getAttributes required.
        /// 2.Pass templateType.ToString() to templateType parameter of TemplateWebService.getAttributes method.
        /// 3.Invokes TemplateWebService.getAttributes method to return;</remarks>
        public TemplateAttributeModel[] GetRefAPOTemplateAttributes(TemplateType templateType, string refAPONumber, string agencyCode, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin TemplateBll.GetRefAPOTemplateAttributes()");
            }

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(callerID) || string.IsNullOrEmpty(refAPONumber))
            {
                return null;
            }

            try
            {
                var attributeModels = TemplateService.getAttributes(agencyCode, TEMPLATE_GENUS_APO, templateType.ToString(), refAPONumber, callerID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End TemplateBll.GetRefAPOTemplateAttributes()");
                }

                return attributeModels;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets reference People template attributes with value by template type and reference contact or license number.
        /// This method returns not only template attributes but also the detail values for reference.
        /// When it is referenced,the field name should be matched to fill to daily field.
        /// </summary>
        /// <param name="templateType">template type may be one of below values:
        /// 1.one of standard choice items in 'CONTACT TYPE' category;
        /// 2.one of standard choice items in 'LICENSED PROFESSIONAL TYPE' category;</param>
        /// <param name="refPeopleNumber">reference contact number or license sequence number.it should be long type parameter.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1.Pass "People" constant to templateGenus parameter that TemplateWebService.getAttributes required.
        /// 2.Invokes TemplateWebService.getAttributes method to return;</remarks>
        public TemplateAttributeModel[] GetRefPeopleTemplateAttributes(string templateType, string refPeopleNumber, string agencyCode, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin TemplateBll.GetRefPeopleTemplateAttributes()");
            }

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(callerID) || string.IsNullOrEmpty(refPeopleNumber) || string.IsNullOrEmpty(templateType))
            {
                return null;
            }

            try
            {
                TemplateAttributeModel[] attributeModels;
                attributeModels = TemplateService.getAttributes(agencyCode, TEMPLATE_GENUS_PEOPLE, templateType, refPeopleNumber, callerID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End TemplateBll.GetRefPeopleTemplateAttributes()");
                }

                return attributeModels;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Fill up select option for each dropdown list template.
        /// </summary>
        /// <param name="templateType">APO Template Type</param>
        /// <param name="attributes">Template attributes</param>
        /// <returns>TemplateAttributeModel Array</returns>
        TemplateAttributeModel[] ITemplateBll.FillTemplateDropDownList(TemplateType templateType, TemplateAttributeModel[] attributes)
        {
            TemplateAttributeModel[] refAttributes = GetTemplateFromCache(AgencyCode, TEMPLATE_GENUS_APO, templateType.ToString());

            if (refAttributes == null || attributes == null)
            {
                return attributes;
            }

            IList<TemplateAttributeModel> attributeList = new List<TemplateAttributeModel>();

            foreach (TemplateAttributeModel tmpModel in attributes)
            {
                if (tmpModel == null)
                {
                    continue;
                }

                if (DROPDOWNLIST.Equals(tmpModel.attributeValueDataType))
                {
                    foreach (TemplateAttributeModel model in refAttributes)
                    {
                        if (model == null)
                        {
                            continue;
                        }

                        if (tmpModel.attributeName == model.attributeName)
                        {
                            tmpModel.selectOptions = model.selectOptions;
                            break;
                        }
                    }
                }

                attributeList.Add(tmpModel);
            }

            TemplateAttributeModel[] returnAttributeArray = new TemplateAttributeModel[attributeList.Count];
            attributeList.CopyTo(returnAttributeArray, 0);

            return returnAttributeArray;
        }

        /// <summary>
        /// Get Template attributes with EMSE by each license professional.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="licenseType">license type</param>
        /// <param name="licenseSeqNum">license sequence number</param>
        /// <param name="licenseNbr">license number</param>
        /// <param name="callerID">public user id</param>
        /// <returns>Template attribute model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public TemplateAttributeModel[] GetLPAttributes4SupportEMSE(
            string agencyCode,
            string licenseType,
            string licenseSeqNum,
            string licenseNbr,
            string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin TemplateBll.GetLPAttributes4SupportEMSE()");
            }

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(callerID) || string.IsNullOrEmpty(licenseType))
            {
                return null;
            }

            try
            {
                TemplateAttributeModel[] attributeModels;
                attributeModels = TemplateService.getLPAttributes4SupportEMSE(agencyCode, licenseType, licenseSeqNum, licenseNbr, callerID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End TemplateBll.GetLPAttributes4SupportEMSE()");
                }

                return attributeModels;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Document template fields.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="documentGroupCode">document group code.</param>
        /// <param name="documentType">document type.</param>
        /// <returns>return templateModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public TemplateModel GetDocumentTemplates(string agencyCode, string documentGroupCode, string documentType)
        {
            try
            {
                TemplateModel model = TemplateService.getDocTemplate(agencyCode, documentGroupCode, documentType);
                return model;
            }
            catch (Exception ex)
            {
                throw new ACAException(ex);
            }
        }

        /// <summary>
        /// Get Contact generic template fields.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="contactType">contact type.</param>
        /// <param name="filterSearchable">true - only retrieve Searchable fields, false - retrieve all fields.</param>
        /// <param name="callerId">Public user ID.</param>
        /// <returns>return templateModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public TemplateModel GetContactTemplates(string agencyCode, string contactType, bool filterSearchable, string callerId)
        {
            try
            {
                if (string.IsNullOrEmpty(contactType))
                {
                    return null;
                }

                TemplateModel model = TemplateService.getContactTemplate(agencyCode, contactType, filterSearchable, callerId);

                return model;
            }
            catch (Exception ex)
            {
                throw new ACAException(ex);
            }
        }

        /// <summary>
        /// Get Generic template Model by entityPKModel
        /// </summary>
        /// <param name="entityPK">entityPK Model</param>
        /// <returns>return templateModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public TemplateModel GetDailyGenericTemplate(EntityPKModel entityPK)
        {
            try
            {
                TemplateModel model = TemplateService.getDailyGenericTemplate(entityPK);

                return model;
            }
            catch (Exception ex)
            {
                throw new ACAException(ex);
            }
        }

        /// <summary>
        /// Gets the generic template.
        /// </summary>
        /// <param name="entityPK">The entity PK.</param>
        /// <param name="filterSearchable">if set to <c>true</c> [filter searchable].</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>Template information.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public TemplateModel GetGenericTemplateStructureByEntityPKModel(EntityPKModel entityPK, bool filterSearchable, string callerId)
        {
            try
            {
                TemplateModel model = TemplateService.getGenericTemplateStructureByEntityPKModel(entityPK, filterSearchable, callerId);

                return model;
            }
            catch (Exception ex)
            {
                throw new ACAException(ex);
            }
        }

        /// <summary>
        /// Gets the template associate asi group.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityKey">The entity key.</param>
        /// <returns>ASI group.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string GetTemplateAssociateASIGroup(GenericTemplateEntityType entityType, string entityKey)
        {
            try
            {
                return TemplateService.getTemplateAssociateASIGroup(AgencyCode, (int)entityType, true, entityKey);
            }
            catch (Exception ex)
            {
                throw new ACAException(ex);
            }
        }

        /// <summary>
        /// Get template data from cache.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="bigTemplateType">big template type,one of TEMPLATE_GENUS_PEOPLE,TEMPLATE_GENUS_APO </param>
        /// <param name="templateType">template type.</param>
        /// <returns>TemplateAttributeModel array.</returns>
        private TemplateAttributeModel[] GetTemplateFromCache(string agencyCode, string bigTemplateType, string templateType)
        {
            ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
            string cacheKey = agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_TEMPLATE);
            Hashtable htTemplates = cacheManager.GetCachedItem(agencyCode, cacheKey);

            if (htTemplates == null || htTemplates.Count == 0)
            {
                return null;
            }

            string templateTypeKey = bigTemplateType + "|" + templateType.ToLower();

            if (htTemplates.ContainsKey(templateTypeKey) && htTemplates[templateTypeKey] != null)
            {
                TemplateAttributeModel[] attributes = htTemplates[templateTypeKey] as TemplateAttributeModel[];

                return attributes;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// set template dropdown select option under super agency.
        /// </summary>
        /// <param name="templateAttrs">Template Attribute Model list</param>
        /// <param name="editAttribute">edit Attribute</param>
        private void SetTemplateSelectOption4SuperAgency(TemplateAttributeModel[] templateAttrs, TemplateAttributeModel editAttribute)
        {
            if (templateAttrs != null && editAttribute != null && templateAttrs.Count(f => f.attributeName == editAttribute.attributeName) > 0)
            {
                TemplateAttributeModel attribute = templateAttrs.First(f => f.attributeName == editAttribute.attributeName);

                if (attribute != null && attribute.selectOptions != null)
                {
                    editAttribute.selectOptions = attribute.selectOptions;
                }
            }
        }

        #endregion
    }
}
