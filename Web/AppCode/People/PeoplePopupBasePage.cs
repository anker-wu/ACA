#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PeoplePopupBasePage.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description: Provide the base page for people page inherit.
 *
 *  Notes:
 *      $Id: PeoplePopupBasePage.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.People
{
    /// <summary>
    /// Provide the base page for people page.
    /// </summary>
    public class PeoplePopupBasePage : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// contact session parameter
        /// </summary>
        private ContactSessionParameter _contactSessionParameter;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the contact session parameter
        /// </summary>
        protected ContactSessionParameter ContactSessionParameterModel
        {
            get
            {
                if (AppSession.IsAdmin)
                {
                    _contactSessionParameter = new ContactSessionParameter();
                    string secPostion = Request.QueryString[UrlConstant.CONTACT_SECTION_POSITION];
                    _contactSessionParameter.ContactSectionPosition = string.IsNullOrEmpty(secPostion)
                                                                          ? ACAConstant.ContactSectionPosition.None
                                                                          : EnumUtil<ACAConstant.ContactSectionPosition>.Parse(secPostion);
                }

                if (_contactSessionParameter == null)
                {
                    _contactSessionParameter = AppSession.GetContactSessionParameter();
                }

                return _contactSessionParameter;
            }
        }

        /// <summary>
        /// Gets the parent id.
        /// </summary>
        protected string ParentID
        {
            get
            {
                return ContactSessionParameterModel.Process.CallbackFunctionName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Multiple contact be used.
        /// </summary>
        protected bool IsMultipleContact
        {
            get
            {
                return ContactSessionParameterModel.PageFlowComponent.ComponentID == PageFlowComponent.CONTACT_LIST
                    || ContactSessionParameterModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.AddReferenceContact
                    || ContactSessionParameterModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.ModifyReferenceContact;
            }
        }

        /// <summary>
        /// Gets the contact section position.
        /// </summary>
        protected ACAConstant.ContactSectionPosition ContactSectionPosition
        {
            get
            {
                return AppSession.IsAdmin
                    ? EnumUtil<ACAConstant.ContactSectionPosition>.Parse(Request.QueryString[UrlConstant.CONTACT_SECTION_POSITION])
                    : ContactSessionParameterModel.ContactSectionPosition;
            }
        }

        /// <summary>
        /// Gets the current contact type.
        /// </summary>
        protected ExpressionType ContactExpressionType
        {
            get
            {
                return ContactSessionParameterModel.ContactExpressionType;
            }
        }

        /// <summary>
        /// Gets the current contact type name.
        /// </summary>
        protected string ContactType
        {
            get
            {
                return ContactSessionParameterModel.ContactType;
            }
        }

        /// <summary>
        /// Gets the search type.
        /// </summary>
        protected ContactProcessType ContactSearchType
        {
            get
            {
                ContactProcessType searchType;

                if (AppSession.IsAdmin)
                {
                    searchType = EnumUtil<ContactProcessType>.Parse(Request[UrlConstant.CONTACT_SEARCH_TYPE], ContactProcessType.Lookup);
                }
                else
                {
                    searchType = ContactSessionParameterModel.Process.ContactProcessType;
                }

                return searchType;
            }
        }

        #endregion Properties
    }
}