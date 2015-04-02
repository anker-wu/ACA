#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ITemplateBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ITemplateBll.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/20/2008    Steven.lee    Initial version.
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.APO
{
    /// <summary>
    /// Provide the ability to get APO or People template attributes.
    /// Save the template attribute values feature is put to cap.
    /// </summary>
    public interface ITemplateBll
    {
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
        /// <remarks>
        /// 1.Pass "APO" constant to templateGenus parameter that TemplateWebService.getAttributes required.
        /// 2.Pass templateType.ToString() to templateType parameter of TemplateWebService.getAttributes method.
        /// 3.Pass null to template Reference Number parameter that TemplateWebService.getAttributes required.
        /// 4.Invokes TemplateWebService.getAttributes method to return;
        /// </remarks>
        TemplateAttributeModel[] GetAPOTemplateAttributes(TemplateType templateType, string agencyCode, string callerID);

        /// <summary>
        /// Gets daily APO template attributes with value by template type,Cap Id and daily APO number.
        /// This method returns not only template attributes but also the detail values related to detail a cap.
        /// </summary>
        /// <remarks>
        /// 1.Pass "APO" constant to templateGenus parameter that TemplateWebService.getEditAttributes required.
        /// 2.Pass templateType.ToString() to templateType parameter of TemplateWebService.getEditAttributes method.
        /// 3.Invokes TemplateWebService.getEditAttributes method to return;
        /// </remarks>
        /// <param name="templateType">One value of TemplateType enumeration includes CAP_ADDRESS,CAP_PARCEL,CAP_OWNER.</param>
        /// <param name="capIDModel">cap id model.</param>
        /// <param name="dailyAPONumber">daily APO number.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        TemplateAttributeModel[] GetDailyAPOTemplateAttributes(TemplateType templateType, CapIDModel4WS capIDModel, string dailyAPONumber, string agencyCode, string callerID);

        /// <summary>
        /// Gets daily People template attributes with value by template type,Cap Id and daily APO number.
        /// This method returns not only template attributes but also the detail values related to detail contact or license.
        /// </summary>
        /// <remarks>
        /// 1.Pass "People" constant to templateGenus parameter that TemplateWebService.getEditAttributes required.
        /// 2.Invokes TemplateWebService.getEditAttributes method to return;
        /// </remarks>
        /// <param name="templateType">
        /// template type may be one of below values:
        /// 1.one of standard choice items in 'CONTACT TYPE' category;
        /// 2.one of standard choice items in 'LICENSED PROFESSIONAL TYPE' category;
        /// </param>
        /// <param name="capIDModel">cap id model.</param>
        /// <param name="dailyPeopleNumber">daily contact number or license sequence number.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        TemplateAttributeModel[] GetDailyPeopleTemplateAttributes(string templateType, CapIDModel4WS capIDModel, string dailyPeopleNumber, string agencyCode, string callerID);

        /// <summary>
        /// Gets People template attributes by template type.
        /// This method only returns template attributes but not includes the detail values.
        /// When creating people data,you need to present template field without value.
        /// this method will be invoked to return all attributes fields for dynamically creating fields/controls.
        /// </summary>
        /// <remarks>
        /// 1.Pass "People" constant to templateGenus parameter that TemplateWebService.getAttributes required.
        /// 2.Pass null to template Reference Number parameter that TemplateWebService.getAttributes required.
        /// 3.Invokes TemplateWebService.getAttributes method to return;
        /// </remarks>
        /// <param name="templateType">
        /// template type may be one of below values:
        /// 1.one of standard choice items in 'CONTACT TYPE' category;
        /// 2.one of standard choice items in 'LICENSED PROFESSIONAL TYPE' category;
        /// </param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        TemplateAttributeModel[] GetPeopleTemplateAttributes(string templateType, string agencyCode, string callerID);

        /// <summary>
        /// Gets reference APO template attributes with value by template type and reference APO number.
        /// This method returns not only template attributes but also the detail values for reference.
        /// When it is referenced,the field name should be matched to fill to daily field.
        /// </summary>
        /// <remarks>
        /// 1.Pass "APO" constant to templateGenus parameter that TemplateWebService.getAttributes required.
        /// 2.Pass templateType.ToString() to templateType parameter of TemplateWebService.getAttributes method.
        /// 3.Invokes TemplateWebService.getAttributes method to return;
        /// </remarks>
        /// <param name="templateType">One value of TemplateType enumeration includes CAP_ADDRESS,CAP_PARCEL,CAP_OWNER.</param>
        /// <param name="refAPONumber">reference APO sequence number.it should be long type parameter.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        TemplateAttributeModel[] GetRefAPOTemplateAttributes(TemplateType templateType, string refAPONumber, string agencyCode, string callerID);

        /// <summary>
        /// Gets reference People template attributes with value by template type and reference contact or license number.
        /// This method returns not only template attributes but also the detail values for reference.
        /// When it is referenced,the field name should be matched to fill to daily field.
        /// </summary>
        /// <remarks>
        /// 1.Pass "People" constant to templateGenus parameter that TemplateWebService.getAttributes required.
        /// 2.Invokes TemplateWebService.getAttributes method to return;
        /// </remarks>
        /// <param name="templateType">
        /// template type may be one of below values:
        /// 1.one of standard choice items in 'CONTACT TYPE' category;
        /// 2.one of standard choice items in 'LICENSED PROFESSIONAL TYPE' category;
        /// </param>
        /// <param name="refPeopleNumber">reference contact number or license sequence number.it should be long type parameter.</param>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="callerID">pubic user id.</param>
        /// <returns>Returns the TemplateAttributeModel array includes the attribute list item if the attribute is configured as dropdownlist.</returns>
        TemplateAttributeModel[] GetRefPeopleTemplateAttributes(string templateType, string refPeopleNumber, string agencyCode, string callerID);

        /// <summary>
        /// Fill up select option for each dropdown list template.
        /// </summary>
        /// <param name="templateType">APO Template Type</param>
        /// <param name="attributes">Template attributes</param>
        /// <returns>TemplateAttributeModel Array</returns>
        TemplateAttributeModel[] FillTemplateDropDownList(TemplateType templateType, TemplateAttributeModel[] attributes);

        /// <summary>
        /// Get Template attributes with EMSE by each license professional.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="licenseType">license type</param>
        /// <param name="licenseSeqNum">license sequence number</param>
        /// <param name="licenseNbr">license number</param>
        /// <param name="callerID">public user id</param>
        /// <returns>Template attribute model</returns>
        TemplateAttributeModel[] GetLPAttributes4SupportEMSE(
            string agencyCode,
            string licenseType,
            string licenseSeqNum,
            string licenseNbr,
            string callerID);

        /// <summary>
        /// Get Document template fields.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="documentGroupCode">document group code.</param>
        /// <param name="documentType">document type.</param>
        /// <returns>return templateModel</returns>
        TemplateModel GetDocumentTemplates(string agencyCode, string documentGroupCode, string documentType);

        /// <summary>
        /// Get Contact generic template fields.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="contactType">contact type.</param>
        /// <param name="filterSearchable">true - only retrieve Searchable fields, false - retrieve all fields.</param>
        /// <param name="callerId">Public user ID.</param>
        /// <returns>return templateModel</returns>
        TemplateModel GetContactTemplates(string agencyCode, string contactType, bool filterSearchable, string callerId);

        /// <summary>
        /// Get Generic template Model by entityPKModel
        /// </summary>
        /// <param name="entityPK">entityPK Model</param>
        /// <returns>return templateModel</returns>
        TemplateModel GetDailyGenericTemplate(EntityPKModel entityPK);

        /// <summary>
        /// Gets the generic template.
        /// </summary>
        /// <param name="entityPK">The entity PK.</param>
        /// <param name="filterSearchable">if set to <c>true</c> [filter searchable].</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>Template information.</returns>
        TemplateModel GetGenericTemplateStructureByEntityPKModel(EntityPKModel entityPK, bool filterSearchable, string callerId);

        /// <summary>
        /// Set Dropdown List Item for template field
        /// </summary>
        /// <param name="item">template item</param>
        /// <param name="type">APO or People</param>
        void FillTemplateSelectOption(TemplateAttributeModel item, string type);

        /// <summary>
        /// Gets the template associate asi group.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityKey">The entity key.</param>
        /// <returns>ASI group.</returns>
        string GetTemplateAssociateASIGroup(GenericTemplateEntityType entityType, string entityKey);

        #endregion Methods
    }
}
