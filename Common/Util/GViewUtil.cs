#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GViewUtil.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description: Class Generic View Util
 * 
 *  Notes:
 *      $Id$.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// Class Generic View Utility.
    /// </summary>
    public static class GViewUtil
    {
        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <param name="viewID">The view Id.</param>
        /// <returns>the entity type</returns>
        public static GenericTemplateEntityType GetEntityType(string viewID)
        {
            GenericTemplateEntityType result;

            switch (viewID)
            {
                case GviewID.ContinuingEducationEdit:
                case GviewID.RefContactContinuingEducationEdit:
                    result = GenericTemplateEntityType.ContinuingEducationDefinition;
                    break;
                case GviewID.EducationEdit:
                case GviewID.RefContactEducationEdit:
                    result = GenericTemplateEntityType.RefEducation;
                    break;
                case GviewID.ExaminationEdit:
                case GviewID.RefContactExaminationEdit:
                    result = GenericTemplateEntityType.RefExamination;
                    break;
                case GviewID.Attachment:
                case GviewID.PeopleAttachment:
                    result = GenericTemplateEntityType.RefDocTypeDefinition;
                    break;

                //default is contact.
                default:
                    result = GenericTemplateEntityType.RefContactTypeDefinition;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Filter contact type template by reference or transaction.
        /// </summary>
        /// <param name="models">The simple view element model.</param>
        /// <param name="gviewID">The GView id.</param>
        /// <param name="transContactTypes">The transection contact types.</param>
        /// <param name="refContactTypes">The reference contact types.</param>
        /// <returns>The filtered simple view element model.</returns>
        public static SimpleViewElementModel4WS[] FilterPeopleTempate(SimpleViewElementModel4WS[] models, string gviewID, List<string> transContactTypes, List<string> refContactTypes)
        {
            if (models == null || models.Length == 0
                || (gviewID != GviewID.ReferenceContactList && gviewID != GviewID.ContactList
                    && gviewID != GviewID.AccountReferenceContactList && gviewID != GviewID.SearchFormContactList))
            {
                return models;
            }

            List<SimpleViewElementModel4WS> simpleViewList = new List<SimpleViewElementModel4WS>();

            foreach (SimpleViewElementModel4WS model in models)
            {
                if (!ValidationUtil.IsNo(model.standard))
                {
                    simpleViewList.Add(model);
                    continue;
                }

                string viewElementName = model.viewElementName;
                string[] strs = Regex.Split(viewElementName, ACAConstant.SPLIT_DOUBLE_COLON);

                if (strs != null && strs.Length == 2)
                {
                    string contactType = strs[1];
                    List<string> contactTypes = new List<string>();

                    //For Spear form, get Transaction or Both contact type associated people template fields.
                    if (gviewID == GviewID.ContactList || gviewID == GviewID.SearchFormContactList)
                    {
                        contactTypes = transContactTypes;
                    }
                    else if (gviewID == GviewID.AccountReferenceContactList || gviewID == GviewID.ReferenceContactList)
                    {
                        //For account management reference contact list and contact look up result list on Spear Form.
                        contactTypes = refContactTypes;
                    }

                    if (contactTypes != null && contactTypes.Contains(contactType))
                    {
                        simpleViewList.Add(model);
                    }
                }
            }

            return simpleViewList == null || simpleViewList.Count == 0 ? null : simpleViewList.ToArray();
        }
    }
}
